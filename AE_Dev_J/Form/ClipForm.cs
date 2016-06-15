using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SpatialAnalyst;
using ESRI.ArcGIS.AnalysisTools;
using ESRI.ArcGIS.SpatialAnalystTools;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.GeoAnalyst;
using DevExpress.XtraEditors;

namespace AE_Dev_J.Form
{
    public partial class ClipForm : DevExpress.XtraEditors.XtraForm
    {
        public TextEdit getlefttopX() { return clip_lefttopX; }//供主窗口调用，传回坐标值
        public TextEdit getlefttopY() { return clip_lefttopY; }//供主窗口调用，传回坐标值
        public TextEdit getrightbottomX() { return clip_rightbottomX; }//供主窗口调用，传回坐标值
        public TextEdit getrightbottomY() { return clip_rightbottomY; }//供主窗口调用，传回坐标值

        private List<IFeatureLayer> featurelayerlist = null;//存放当前地图控件已加载的所有矢量图层
        private List<IRasterLayer> rasterlayerlist = null;//存放当前地图控件已加载的所有栅格图层
        private string m_resultPath = null;//存储输出文件路径
        private MainForm main = null;//用于调用主窗口的openRasterFile方法与openVectorFile方法

        private delegate void methoddelegate(object source, FileSystemEventArgs e);//负责将文件监测FilesOnChanged方法委托到主线程，用于跨线程调用控件
        private Stopwatch watch = null;//用于计算ToRasterDataset方法的预处理时间，以此估算结果文件大小
        private int ProgPauseflag = 0;//用于文件监测线程与进度条线程的交互，值为1时进度条停止
        private int changelength = 0;//用于文件监测线程与进度条线程的交互，记录文件大小变化的字节数，以此更新进度条
        private int changestep = 0;//用于文件监测线程与进度条线程的交互，根据文件大小变化的字节数计算进度条更新步数
        private int completingflag = 0;//值为1时表示进度条执行到任务的90%，暂停进度条并等待ToRasterDataset方法执行完毕


        public ClipForm( MainForm mainform)
        {
            InitializeComponent();
            main = mainform;

            featurelayerlist = new List<IFeatureLayer>();
            rasterlayerlist = new List<IRasterLayer>();

            //创建存储当前矢量图层的图层列表
            for (int i = 0; i < main.getMapControl().LayerCount; i++)
            {
                ILayer layer = main.getMapControl().get_Layer(i);
                if (layer is IFeatureLayer)
                {
                    featurelayerlist.Add(layer as IFeatureLayer);
                }
            }
            //创建存储当前栅格图层的图层列表
            for (int i = 0; i < main.getMapControl().LayerCount; i++)
            {
                ILayer layer = main.getMapControl().get_Layer(i);
                if (layer is IRasterLayer)
                {
                    rasterlayerlist.Add(layer as IRasterLayer);
                }
            }
        }

        private void Clip_Load(object sender, EventArgs e)
        {
            //将featurelayerlist中的元素添加到inputcombox中与maskcombox中
            for (int i = 0; i < featurelayerlist.Count; i++)
            {
                IDataLayer datalayer = featurelayerlist[i] as IDataLayer;
                IWorkspaceName w_name = ((IDatasetName)(datalayer.DataSourceName)).WorkspaceName;
                clip_input.Properties.Items.Add(w_name.PathName + "\\" + featurelayerlist[i].Name + ".shp");
                clip_mask.Properties.Items.Add(w_name.PathName + "\\" + featurelayerlist[i].Name + ".shp");

            }
            //将rasterlayerlist中的元素添加到inputcombox中
            for (int i = 0; i < rasterlayerlist.Count; i++)
            {
                IDataLayer datalayer = rasterlayerlist[i] as IDataLayer;
                IWorkspaceName w_name = ((IDatasetName)(datalayer.DataSourceName)).WorkspaceName;
                //地图文档数据w_name.PathName没有斜杠，非地图文档数据w_name.PathName有斜杠，所以路径拼接的时候需要判断
                if (w_name.PathName.LastIndexOf("\\") == w_name.PathName.Length - 1)
                {
                    clip_input.Properties.Items.Add(w_name.PathName + rasterlayerlist[i].Name);
                    clip_mask.Properties.Items.Add(w_name.PathName + rasterlayerlist[i].Name);
                }
                else
                {
                    clip_input.Properties.Items.Add(w_name.PathName + "\\" + rasterlayerlist[i].Name);
                    clip_mask.Properties.Items.Add(w_name.PathName + "\\" + rasterlayerlist[i].Name);
                }
            }
            clip_featureboundarycheck.Enabled = false;//是否根据矢量图形边界裁剪选项不可用
            clip_exporttomapbutton.Enabled = false;
            this.Tag = this.Handle;//将main窗口中mapcontrol返回的坐标通过handle传回clip窗口

        }
        /// <summary>
        /// 输入文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clip_input_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //打开文件
            if (e.Button.Index == 1)
            {
                OpenFileDialog openclipfile = new OpenFileDialog();
                openclipfile.Filter = "(shp文件)|*.shp|(TIF文件)|*.tif|(IMG文件)|*.img|(GRID文件)|*.adf";
                openclipfile.Title = "打开文件";
                if (openclipfile.ShowDialog() == DialogResult.OK)
                {
                    clip_input.Text = "";
                    clip_input.Text = openclipfile.FileName;
                }
            }
        }
        /// <summary>
        /// 输出文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clip_output_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //指定输出文件路径
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Title = "输出文件";
            if (clip_input.Text!="")
            {
                FileInfo inputfileinfo = new FileInfo(clip_input.Text);
                if (inputfileinfo.Extension!=".shp")//根据输入文件判断输出文件类型
                {
                    savefile.Filter = "(TIF文件)|*.tif|(IMG文件)|*.img|(GRID文件)|*.GRID";
                }
                else
                {
                    savefile.Filter = "(shp文件)|*.shp";
                }
            }
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                clip_output.Text = savefile.FileName.Replace(".GRID", "");
            }
        }
        /// <summary>
        /// CheckBox更改选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clip_featureboundarycheck_CheckedChanged(object sender, EventArgs e)
        {
            if (clip_featureboundarycheck.Checked)
            {
                layoutControlGroup2.Enabled = false;
            }
            else
            {
                layoutControlGroup2.Enabled = true;
            }
        }
        /// <summary>
        /// 打开mask文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clip_mask_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //打开文件
            if (e.Button.Index == 1)
            {
                OpenFileDialog openclipfile = new OpenFileDialog();
                openclipfile.Filter = "(shp文件)|*.shp|(TIF文件)|*.tif|(IMG文件)|*.img|(GRID文件)|*.adf";
                openclipfile.Title = "打开文件";
                if (openclipfile.ShowDialog() == DialogResult.OK)
                {
                    clip_mask.Text = "";
                    clip_mask.Text = openclipfile.FileName;
                }

            }
        }
        /// <summary>
        /// 点击确定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clip_OKbutton_Click(object sender, EventArgs e)
        {
            //检查输入是否为空
            if (clip_input.Text == ""
                || clip_output.Text == ""
                || (clip_output.Text == ""
                    &&(clip_lefttopX.Text==""
                       ||clip_lefttopY.Text==""
                       ||clip_leftbottomX.Text==""
                       ||clip_leftbottomY.Text==""
                       ||clip_rightbottomX.Text==""
                       ||clip_rightbottomY.Text==""
                       ||clip_righttopX.Text==""
                       ||clip_righttopY.Text=="")
                    )
                )
            {
                MessageBox.Show("输入不能为空");
                return;
            }

            //检查是否重名
            m_resultPath = clip_output.Text;
            FileInfo fileinfo = new FileInfo(m_resultPath);
            if (fileinfo.Exists)
            {
                MessageBox.Show("输出文件已存在");
                return;
            }
            else
            {
                //检查GRID文件
                if (fileinfo.Extension == "" && Directory.Exists(m_resultPath))
                {
                    MessageBox.Show("输出文件已存在");
                    return;
                }
            }

            //当前状态下设置button不可用
            clip_OKbutton.Enabled = false;
            clip_cancelbutton.Enabled = false;
            clip_exporttomapbutton.Enabled = false;

            ProgPauseflag = 0;
            changelength = 0;
            changestep = 0;
            completingflag = 0;

            //进度条属性初始化
            Clip_progressBarControl.Properties.Step = 1;
            Clip_progressBarControl.Properties.PercentView = true;
            Clip_progressBarControl.Properties.Minimum = 0;
            Clip_progressBarControl.Properties.Maximum = 1000;
            Clip_progressBarControl.EditValue = 0;

            //启动文件监测线程
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = fileinfo.DirectoryName;
            watcher.Changed += new FileSystemEventHandler(FilesOnChanged);
            watcher.Created += new FileSystemEventHandler(FilesOnChanged);
            watcher.NotifyFilter = NotifyFilters.Size | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Filter = fileinfo.Name;
            watcher.EnableRaisingEvents = true;

            //启动转换线程
            System.ComponentModel.BackgroundWorker FtoR_backgroundWorker = new BackgroundWorker();
            FtoR_backgroundWorker.WorkerSupportsCancellation = true;
            FtoR_backgroundWorker.WorkerReportsProgress = true;
            FtoR_backgroundWorker.DoWork += new DoWorkEventHandler(Clip_backgroundWorker_DoWork);
            FtoR_backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Clip_backgroundWorker_RunWorkerCompleted);
            FtoR_backgroundWorker.RunWorkerAsync();

            //启动进度条线程
            System.ComponentModel.BackgroundWorker ProgBar_backgroundWorker = new BackgroundWorker();
            ProgBar_backgroundWorker.WorkerSupportsCancellation = true;
            ProgBar_backgroundWorker.WorkerReportsProgress = true;
            ProgBar_backgroundWorker.DoWork += new DoWorkEventHandler(ProgBar_backgroundWorker_DoWork);
            ProgBar_backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(ProgBar_backgroundWorker_ProgressChanged);
            ProgBar_backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ProgBar_backgroundWorker_RunWorkerCompleted);
            ProgBar_backgroundWorker.RunWorkerAsync();
        }
        /// <summary>
        /// 文件监测
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilesOnChanged(object source, FileSystemEventArgs e)
        {
            //当文件创建或更改时执行此方法
            if (e.ChangeType == WatcherChangeTypes.Changed || e.ChangeType == WatcherChangeTypes.Created)
            {
                FileInfo prefile = new FileInfo(e.FullPath);
                FileInfo file = null;
                if (prefile.Extension == "")
                {
                    //如果是GRID文件，就监测w001001.adf
                    file = new FileInfo(e.FullPath + "\\w001001.adf");
                }
                else
                {
                    file = new FileInfo(e.FullPath);
                }
                //检查是否跨线程调用
                if (Clip_progressBarControl.InvokeRequired)
                {
                    //将方法委托到主线程
                    methoddelegate mydelegate = new methoddelegate((object source1, FileSystemEventArgs e1) => FilesOnChanged(source, e));
                    Clip_progressBarControl.Invoke(mydelegate, source, e);
                }
                else
                {
                    FileSystemWatcher filewatcher = source as FileSystemWatcher;
                    if (Convert.ToInt32(Clip_progressBarControl.EditValue.ToString()) == Clip_progressBarControl.Properties.Maximum)
                    {
                        //根据进度条当前值判断转换结束
                        filewatcher.EnableRaisingEvents = false;
                        filewatcher.Changed -= new FileSystemEventHandler(FilesOnChanged);
                        filewatcher.Created -= new FileSystemEventHandler(FilesOnChanged);
                        return;
                    }
                    //停止计时，估算结果文件大小并重新设置进度条属性
                    watch.Stop();
                    if (Clip_progressBarControl.Properties.Maximum == 100000)
                    {
                        int maximum = (int)watch.ElapsedMilliseconds * 2910;
                        Clip_progressBarControl.Properties.Maximum = maximum;
                        Clip_progressBarControl.Properties.Step = 10000;
                    }
                    int value = Clip_progressBarControl.Properties.Maximum / 1000;
                    if (file.Exists)
                    {
                        //控制进度条停止或开始
                        if (Convert.ToInt32(Clip_progressBarControl.EditValue.ToString()) <= (int)file.Length)
                        {
                            ProgPauseflag = 0;//文件长度大于进度条当前值
                            Clip_progressBarControl.EditValue = file.Length;
                        }
                        else
                        {
                            ProgPauseflag = 1;//文件长度小于进度条当前值，暂停并等待
                            changelength = Convert.ToInt32(Clip_progressBarControl.EditValue.ToString()) - (int)file.Length;
                        }
                    }

                }
            }
        }
        /// <summary>
        /// 执行裁剪
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Clip_backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            //设置CancellationPending可以退出任务线程
            if (worker.CancellationPending == true)
            {
                e.Cancel = true;
            }
            else
            {
                    FileInfo inputfileinfo = new FileInfo(clip_input.Text);
                    FileInfo maskfileinfo =null;
                    FileInfo outputfileinfo = new FileInfo(clip_output.Text);
                    if (clip_mask.Text!="")
                    {
                       maskfileinfo = new FileInfo(clip_mask.Text);
                    }

                    IWorkspaceFactory outputworkspaceFactory = new RasterWorkspaceFactoryClass();
                    IWorkspace outputworkspaceop = outputworkspaceFactory.OpenFromFile(outputfileinfo.DirectoryName, 0);//创建输出工作空间

                    string inputextension = "1";//0为矢量，1为栅格
                    string maskextension = "1";//0为矢量，1为栅格

                    if (inputfileinfo.Extension == ".shp")
                    {
                        inputextension = "0";
                    }
                    if (clip_mask.Text == "")
                    {
                        maskextension = "2";//自定义包络线
                    }
                    else
                    {
                        if (maskfileinfo.Extension == ".shp" )
                        {
                            maskextension = "0";
                        }
                    }
                    if (clip_featureboundarycheck.Enabled==true && clip_featureboundarycheck.Checked)
                    {
                        maskextension += "1";//根据图形边界裁剪
                    }
                    else if (clip_featureboundarycheck.Enabled==true)
                    {
                        maskextension += "0";//根据包络线裁剪
                    }
                    //启动预处理计时
                    watch = new Stopwatch();
                    watch.Start();
                    //执行裁剪
                    switch (inputextension + maskextension)
                    {
                        case "00": //矢量裁剪矢量，mask要素类型不是polygon，根据mask包络线裁剪矢量
                        case "001"://矢量裁剪矢量，mask要素类型是polygon，根据polygon裁剪矢量
                            Geoprocessor geoprocessor = new Geoprocessor();
                            object infeature = inputfileinfo.FullName+";"+maskfileinfo.FullName;
                            object outfeature = outputfileinfo.FullName;
                            Intersect intersect = new Intersect(infeature, outfeature);
                            geoprocessor.Execute(intersect, null);
                            break;
                        case "01"://栅格裁剪矢量，根据mask包络线裁剪矢量
                        case "000"://矢量裁剪矢量，mask要素类型是polygon，根据mask包络线裁剪矢量
                        case "100"://矢量裁剪栅格，mask要素类型是polygon，根据mask包络线裁剪矢量
                        case "02"://mask裁剪矢量，没有mask数据，自定义包络线裁剪矢量
                        case "12"://mask裁剪栅格，没有mask数据，自定义包络线裁剪栅格
                        case "10": //矢量裁剪栅格，mask要素类型不是polygon，根据mask包络线裁剪矢量

                            IEnvelope envelope = new EnvelopeClass();

                            envelope.XMax = Convert.ToDouble(clip_rightbottomX.Text);
                            envelope.XMin = Convert.ToDouble(clip_lefttopX.Text);
                            envelope.YMax = Convert.ToDouble(clip_lefttopY.Text);
                            envelope.YMin = Convert.ToDouble(clip_rightbottomY.Text);

                            IPoint point = new PointClass();
                            IPointCollection pCol = new PolygonClass();
                            point.X = envelope.XMax;
                            point.Y = envelope.YMax;
                            pCol.AddPoint(point, Type.Missing, Type.Missing);
                            point.X = envelope.XMax;
                            point.Y = envelope.YMin;
                            pCol.AddPoint(point, Type.Missing, Type.Missing);
                            point.X = envelope.XMin;
                            point.Y = envelope.YMin;
                            pCol.AddPoint(point, Type.Missing, Type.Missing);
                            point.X = envelope.XMin;
                            point.Y = envelope.YMax;
                            pCol.AddPoint(point, Type.Missing, Type.Missing);
                            IPolygon polygon = pCol as IPolygon;
                            polygon.Close();
                            //自定义图层
                            IFields pFields = new FieldsClass();
                            IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;
                            IField pField = new FieldClass();
                            IFieldEdit pFieldEdit = pField as IFieldEdit;
                            pFieldEdit.Name_2 = "Shape";
                            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
                            //设置geometry definition
                            IGeometryDef pGeometryDef = new GeometryDefClass();
                            IGeometryDefEdit pGeometryDefEdit = pGeometryDef as IGeometryDefEdit;
                            pGeometryDefEdit.GeometryType_2 = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon;
                            pGeometryDefEdit.SpatialReference_2 = null;
                            pFieldEdit.GeometryDef_2 = pGeometryDef;
                            pFieldsEdit.AddField(pField);

                            IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactory();
                            IFeatureWorkspace pFeatureWorkspace = pWorkspaceFactory.OpenFromFile(Application.StartupPath + "\\temp", 0) as IFeatureWorkspace;

                            //检查文件名是否已存在
                            int changefilename = 0;
                            while (System.IO.File.Exists(Application.StartupPath + "\\temp\\mask" + changefilename + ".shp"))
                            {
                                changefilename++;//若存在则依次递增，直至文件名不重复
                            }
                            pFeatureWorkspace.CreateFeatureClass("mask" + changefilename+".shp", pFields, null, null, esriFeatureType.esriFTSimple, "Shape", "");
                            IFeatureClass featureclass = pFeatureWorkspace.OpenFeatureClass("mask"+changefilename+".shp");
                            IFeature feature = featureclass.CreateFeature();
                            feature.Shape = polygon;
                            feature.Store();
                            Geoprocessor geoprocessor2 = new Geoprocessor();
                            if (inputextension=="0")
                            {
                                //自定义包络线裁剪矢量
                                object infeature2 = inputfileinfo.FullName + ";" + Application.StartupPath + "\\temp\\mask"+changefilename+".shp";//mask1为根据自定义包络线生成的polygon，放在临时文件夹
                                object outfeature2 = outputfileinfo.FullName;
                                Intersect intersect2 = new Intersect(infeature2, outfeature2);
                                geoprocessor2.Execute(intersect2, null);
                            }
                            else
                            {
                                //自定义包络线裁剪栅格
                                object inraster2 = inputfileinfo.FullName;
                                object maskdata2 = Application.StartupPath + "\\temp\\mask"+changefilename+".shp";//mask为根据自定义包络线生成的polygon，放在临时文件夹
                                object outraster2 = outputfileinfo.FullName;
                                ExtractByMask extractbymask2 = new ExtractByMask(inraster2, maskdata2, outraster2);
                                geoprocessor2.Execute(extractbymask2, null);
                            }
                            break;
                        case "101"://矢量裁剪栅格，mask要素类型是polygon，根据polygon裁剪矢量
                        case "11"://栅格裁剪栅格
                            object inraster = inputfileinfo.FullName;
                            object maskdata = maskfileinfo.FullName;
                            object outraster = outputfileinfo.FullName;
                            ExtractByMask extractbymask = new ExtractByMask(inraster,maskdata,outraster);
                            Geoprocessor geoprocessor3 = new Geoprocessor();
                            geoprocessor3.Execute(extractbymask, null);
                            break;
                    }
            }
        }
        /// <summary>
        /// 完成裁剪
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Clip_backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //任务线程执行完毕
            BackgroundWorker worker = sender as BackgroundWorker;
            worker.DoWork -= new DoWorkEventHandler(Clip_backgroundWorker_DoWork);
            worker.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(Clip_backgroundWorker_RunWorkerCompleted);
            ProgPauseflag = 0;
            //将进度条当前值设置为最大值
            Clip_progressBarControl.EditValue = Clip_progressBarControl.Properties.Maximum;
            //设置featuretoraster_OKbutton、featuretoraster_Cancelbutton为可用
            clip_OKbutton.Enabled = true;
            clip_cancelbutton.Enabled = true;
            if (e.Cancelled == true)
            {
                MessageBox.Show("任务被终止！", "消息");
            }
            else if (e.Error != null)
            {
                MessageBox.Show("错误：" + e.Error.Message, "消息");
            }
            else
            {
                MessageBox.Show("完成！","消息");
                clip_exporttomapbutton.Enabled = true;
            }
        }
        /// <summary>
        /// 进度条运行时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgBar_backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            int i = 0;
            while (true)
            {
                i++;
                if (Convert.ToInt32(Clip_progressBarControl.EditValue.ToString()) == Clip_progressBarControl.Properties.Maximum || worker.CancellationPending)
                {
                    //根据进度条当前值判断转换结束
                    e.Cancel = true;
                    break;
                }
                else
                {
                    System.Threading.Thread.Sleep(50);
                    worker.ReportProgress(i);
                }
            }

        }
        /// <summary>
        /// 进度条更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgBar_backgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (ProgPauseflag == 0)
            {
                //进度条执行到80%时
                if (Convert.ToInt32(Clip_progressBarControl.EditValue.ToString()) > (0.8 * Clip_progressBarControl.Properties.Maximum))
                {
                    Clip_progressBarControl.Properties.Step = 5000;
                }
                //进度条执行到90%时
                if (Convert.ToInt32(Clip_progressBarControl.EditValue.ToString()) > (0.9 * Clip_progressBarControl.Properties.Maximum))
                {
                    completingflag = 1;
                    ProgPauseflag = 1;
                }
                Clip_progressBarControl.PerformStep();

            }
            else
            //当前为暂停状态时，ProgPauseflag值为1
            {
                changestep = changestep + 5;
                if (changestep * 10000 > changelength && completingflag == 0)
                {
                    ProgPauseflag = 0;//完成循环，取消暂停状态
                    changestep = 0;
                    changelength = 0;
                }
            }
            Clip_progressBarControl.Update();
        }
        /// <summary>
        /// 进度条执行完毕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgBar_backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            worker.DoWork -= new DoWorkEventHandler(ProgBar_backgroundWorker_DoWork);
            worker.ProgressChanged -= new ProgressChangedEventHandler(ProgBar_backgroundWorker_ProgressChanged);
            worker.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(ProgBar_backgroundWorker_RunWorkerCompleted);

        }
        /// <summary>
        /// 将结果添加至mapcontrol
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clip_exporttomapbutton_Click(object sender, EventArgs e)
        {
            //检查文件是否存在
            if (m_resultPath==""||m_resultPath==null)
            {
                return;
            }
            FileInfo fileinfo = new FileInfo(m_resultPath);
            if (fileinfo.Exists || (fileinfo.Extension == "" && Directory.Exists(m_resultPath)))
            {
                Stopwatch exportwatch = new Stopwatch();
                exportwatch.Start();
                //等待解除文件锁并加载地图，若等待超过两秒则停止加载
                while (true)
                {
                    if (exportwatch.ElapsedMilliseconds > 3000)
                    {
                        MessageBox.Show("文件正在使用，请稍后再试。", "加载失败");
                        exportwatch.Stop();
                        clip_exporttomapbutton.Enabled = true;
                        break;
                    }
                    string[] files = System.IO.Directory.GetFiles(fileinfo.DirectoryName, fileinfo.Name + "*.sr.lock", System.IO.SearchOption.TopDirectoryOnly);
                    if (files.Length == 0)
                    {
                        exportwatch.Stop();
                        if (fileinfo.Extension==".shp")
                        {
                            main.openVectorFile(m_resultPath);
                        }
                        else
                        {
                            main.openRasterFile(m_resultPath);
                        }
                        break;
                    }
                }
                clip_exporttomapbutton.Enabled = false;
            }
            else
            {
                MessageBox.Show("输出文件不存在");
                return;
            }

        }
        /// <summary>
        /// 自定义包络线按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clip_envelopebutton_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            main.drawflag = this.Tag.ToString();//将main窗口中mapcontrol返回的坐标通过遍历tag传回对应的clip窗口
            main.Focus();

        }
        /// <summary>
        /// 根据mask数据生成包络线坐标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clip_mask_TextChanged(object sender, EventArgs e)
        {
            if (clip_mask.Text == "")
            {
                clip_envelopebutton.Enabled = true;
                return;
            }
            else
            {
                clip_envelopebutton.Enabled = false;
            }
            FileInfo maskfileinfo = new FileInfo(clip_mask.Text);
            if (!maskfileinfo.Exists)
            {
                return;
            }
            if ( maskfileinfo.Extension == ".tif" || maskfileinfo.Extension == ".img" || maskfileinfo.Extension == ".adf")
            {
                IWorkspaceFactory Rworkspacefactory = new RasterWorkspaceFactoryClass();
                IRasterWorkspace Rworkspace = Rworkspacefactory.OpenFromFile(maskfileinfo.DirectoryName, 0) as IRasterWorkspace;
                IRasterDataset rasterdataset = Rworkspace.OpenRasterDataset(maskfileinfo.Name);
                IGeoDataset geodataset = rasterdataset as IGeoDataset;

                clip_righttopX.Text = geodataset.Extent.Envelope.XMax.ToString();
                clip_righttopY.Text = geodataset.Extent.Envelope.YMax.ToString();

                clip_leftbottomX.Text = geodataset.Extent.Envelope.XMin.ToString();
                clip_leftbottomY.Text = geodataset.Extent.Envelope.YMin.ToString();

                clip_lefttopX.Text = geodataset.Extent.Envelope.XMin.ToString();
                clip_lefttopY.Text = geodataset.Extent.Envelope.YMax.ToString();

                clip_rightbottomX.Text = geodataset.Extent.Envelope.XMax.ToString();
                clip_rightbottomY.Text = geodataset.Extent.Envelope.YMin.ToString();
                clip_featureboundarycheck.Enabled = false;
            }
            else
            {
                IWorkspaceFactory Fworkspacefactory = new ShapefileWorkspaceFactory();
                IFeatureWorkspace Fworkspace = Fworkspacefactory.OpenFromFile(maskfileinfo.DirectoryName, 0) as IFeatureWorkspace;
                IFeatureClass featureclass = Fworkspace.OpenFeatureClass(maskfileinfo.Name);

                if (featureclass.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    clip_featureboundarycheck.Enabled = true;
                }
                else
                {
                    clip_featureboundarycheck.Enabled = false;
                }
                int count = featureclass.FeatureCount(null);

                IFeatureCursor pcursor = featureclass.Search(null, false);
                IFeature pfeature = pcursor.NextFeature();

                object missing = Type.Missing;
                IEnvelope envelope = new EnvelopeClass();
                while (pfeature != null)
                {
                    IGeometry geometry = pfeature.Shape;
                    IEnvelope featureExtent = geometry.Envelope;
                    envelope.Union(featureExtent);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pfeature);
                    pfeature = pcursor.NextFeature();
                }

                clip_righttopX.Text = envelope.XMax.ToString();
                clip_righttopY.Text = envelope.YMax.ToString();

                clip_leftbottomX.Text = envelope.XMin.ToString();
                clip_leftbottomY.Text = envelope.YMin.ToString();

                clip_lefttopX.Text = envelope.XMin.ToString();
                clip_lefttopY.Text = envelope.YMax.ToString();

                clip_rightbottomX.Text = envelope.XMax.ToString();
                clip_rightbottomY.Text = envelope.YMin.ToString();
            }
        }
        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clip_cancelbutton_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Clip_progressBarControl.EditValue.ToString()) == 0 || Convert.ToInt32(Clip_progressBarControl.EditValue.ToString()) == Clip_progressBarControl.Properties.Maximum)
            {
                //根据进度条当前值判断任务执行完毕，直接关闭
                this.Close();
                this.Dispose();
            }
            else if (MessageBox.Show("是否终止任务并且关闭窗口？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //任务正在执行
                this.Close();
                this.Dispose();
            }
        }

        #region 生成自定义包络线坐标

        private void clip_lefttopX_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 48 || e.KeyChar >= 57) && (e.KeyChar != 8) && (e.KeyChar != 46))
                e.Handled = true;
        }

        private void clip_lefttopY_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 48 || e.KeyChar >= 57) && (e.KeyChar != 8) && (e.KeyChar != 46))
                e.Handled = true;
        }

        private void clip_leftbottomX_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 48 || e.KeyChar >= 57) && (e.KeyChar != 8) && (e.KeyChar != 46))
                e.Handled = true;
        }

        private void clip_leftbottomY_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 48 || e.KeyChar >= 57) && (e.KeyChar != 8) && (e.KeyChar != 46))
                e.Handled = true;
        }

        private void clip_righttopX_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 48 || e.KeyChar >= 57) && (e.KeyChar != 8) && (e.KeyChar != 46))
                e.Handled = true;
        }

        private void clip_righttopY_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 48 || e.KeyChar >= 57) && (e.KeyChar != 8) && (e.KeyChar != 46))
                e.Handled = true;
        }

        private void clip_rightbottomX_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 48 || e.KeyChar >= 57) && (e.KeyChar != 8) && (e.KeyChar != 46))
                e.Handled = true;
        }

        private void clip_rightbottomY_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 48 || e.KeyChar >= 57) && (e.KeyChar != 8) && (e.KeyChar != 46))
                e.Handled = true;
        }
        private void clip_lefttopX_TextChanged(object sender, EventArgs e)
        {
            clip_leftbottomX.Text = clip_lefttopX.Text;
        }
        private void clip_lefttopY_TextChanged(object sender, EventArgs e)
        {
            clip_righttopY.Text = clip_lefttopY.Text;
        }
        private void clip_rightbottomX_TextChanged(object sender, EventArgs e)
        {
            clip_righttopX.Text = clip_rightbottomX.Text;
        }
        private void clip_rightbottomY_TextChanged(object sender, EventArgs e)
        {
            clip_leftbottomY.Text = clip_rightbottomY.Text;
        }
        private void clip_righttopX_TextChanged(object sender, EventArgs e)
        {
            clip_rightbottomX.Text = clip_righttopX.Text;
        }
        private void clip_righttopY_TextChanged(object sender, EventArgs e)
        {
            clip_lefttopY.Text = clip_righttopY.Text;
        }
        private void clip_leftbottomX_TextChanged(object sender, EventArgs e)
        {
            clip_lefttopX.Text = clip_leftbottomX.Text;
        }
        private void clip_leftbottomY_TextChanged(object sender, EventArgs e)
        {
            clip_rightbottomY.Text = clip_leftbottomY.Text;
        }
        #endregion 生成自定义包络线坐标
    }
}