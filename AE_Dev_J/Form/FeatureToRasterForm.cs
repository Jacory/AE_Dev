using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Diagnostics;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.GeoAnalyst;
using DevExpress.XtraEditors;

namespace AE_Dev_J.Form
{

    public partial class FeatureToRasterForm : DevExpress.XtraEditors.XtraForm
    {
        private List<IFeatureLayer> featurelayerlist = null;//存放当前地图控件已加载的所有矢量图层
        private string m_resultPath = null;//存储输出文件路径
        private MainForm main = null;//用于调用主窗口的openRasterFile方法

        private delegate void methoddelegate(object source, FileSystemEventArgs e);//负责将文件监测FilesOnChanged方法委托到主线程，用于跨线程调用控件
        private Stopwatch watch = null;//用于计算ToRasterDataset方法的预处理时间，以此估算结果文件大小
        private int ProgPauseflag = 0;//用于文件监测线程与进度条线程的交互，值为1时进度条停止
        private int changelength = 0;//用于文件监测线程与进度条线程的交互，记录文件大小变化的字节数，以此更新进度条
        private int changestep = 0;//用于文件监测线程与进度条线程的交互，根据文件大小变化的字节数计算进度条更新步数
        private int completingflag = 0;//值为1时表示进度条执行到任务的90%，暂停进度条并等待ToRasterDataset方法执行完毕

        public FeatureToRasterForm( MainForm mainform)
        {
            InitializeComponent();
            featurelayerlist = new List<IFeatureLayer>();
            main = mainform;
            for (int i = 0; i < main.getMapControl().LayerCount; i++)
            {
                ILayer layer = main.getMapControl().get_Layer(i);
                if (layer is IFeatureLayer)
                {
                    featurelayerlist.Add(layer as IFeatureLayer);
                }
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FeatureToRasterForm_Load(object sender, EventArgs e)
        {
            //将featurelayerlist中的元素添加到inputcombox中
            for (int i = 0; i < featurelayerlist.Count; i++)
            {
                IDataLayer datalayer = featurelayerlist[i] as IDataLayer;
                IWorkspaceName w_name = ((IDatasetName)(datalayer.DataSourceName)).WorkspaceName;
                featuretoraster_inputcombox.Properties.Items.Add(w_name.PathName + "\\" + featurelayerlist[i].Name + ".shp");
            }
            featuretoraster_cellsize.Text = "100";//默认转换的像元大小

            featuretoraster_Exportbutton.Enabled = false;
        }
        /// <summary>
        /// 打开文件对话框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void featuretoraster_inputcombox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //打开文件
            if (e.Button.Index == 1)
            {
                OpenFileDialog openshapefile = new OpenFileDialog();
                openshapefile.Filter = "(shp文件)|*.shp";
                openshapefile.Title = "打开shape文件";
                if (openshapefile.ShowDialog() == DialogResult.OK)
                {
                    featuretoraster_inputcombox.Text = "";
                    featuretoraster_inputcombox.Text = openshapefile.FileName;
                }
            }
        }
        /// <summary>
        /// 输出文件对话框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void featuretoraster_output_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //指定输出文件路径
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Title = "输出文件";
            savefile.Filter = "TIFF文件|*.tif|IMAGINE Image文件|*.img|GRID文件|*.GRID";
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                featuretoraster_output.Text = savefile.FileName.Replace(".GRID","");
            }
        }
        /// <summary>
        /// 读取矢量要素字段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void featuretoraster_inputcombox_TextChanged(object sender, EventArgs e)
        {
            //清空字段列表fieldcombox所有元素，重新添加
            featuretoraster_fieldcombox.Properties.Items.Clear();
            if (featuretoraster_inputcombox.Text != "")
            {
                int flag = 0;//该标记用于判断用户输入的矢量文件是否已存在于当前地图中
                for (int i = 0; i < featurelayerlist.Count; i++)
                {
                    //当矢量文件存在于featurelayerlist中
                    if (featurelayerlist[i].Name == featuretoraster_inputcombox.SelectedText)
                    {
                        IFeatureClass featureclass = featurelayerlist[i].FeatureClass;
                        for (int n = 0; n < featureclass.Fields.FieldCount; n++)
                        {
                            featuretoraster_fieldcombox.Properties.Items.Add(featureclass.Fields.get_Field(n).AliasName);
                        }
                        flag = 1;
                        break;
                    }
                }
                if (flag == 0)
                {
                    //当矢量文件不存在于featurelayerlist中时
                    FileInfo info = new FileInfo(featuretoraster_inputcombox.Text);

                    IFeatureWorkspace featureworkspace;
                    IWorkspaceFactory workspacefactory = new ShapefileWorkspaceFactoryClass();
                    featureworkspace = workspacefactory.OpenFromFile(info.DirectoryName, 0) as IFeatureWorkspace;
                    IFeatureClass featureclass = featureworkspace.OpenFeatureClass(info.Name);
                    for (int n = 0; n < featureclass.Fields.FieldCount; n++)
                    {
                        featuretoraster_fieldcombox.Properties.Items.Add(featureclass.Fields.get_Field(n).AliasName);
                    }
                }
            }
        }
        /// <summary>
        /// 点击确定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void featuretoraster_OKbutton_Click(object sender, EventArgs e)
        {
            //检查输入是否为空
            if (featuretoraster_inputcombox.Text == ""
                || featuretoraster_fieldcombox.Text == ""
                || featuretoraster_output.Text == ""
                || featuretoraster_cellsize.Text == "")
            {
                MessageBox.Show("输入不能为空");
                return;
            }
            //检查是否重名
            m_resultPath = featuretoraster_output.Text;
            FileInfo fileinfo = new FileInfo(m_resultPath);
            if (fileinfo.Exists)
            {
                MessageBox.Show("输出文件已存在");
                return;
            }
            else
            {
                if (fileinfo.Extension==""&&Directory.Exists(m_resultPath))
                {
                    MessageBox.Show("输出文件已存在");
                    return;
                }
            }
            //当前状态下设置featuretoraster_OKbutton不可用
            featuretoraster_OKbutton.Enabled = false;
            featuretoraster_Cancelbutton.Enabled = false;
            featuretoraster_Exportbutton.Enabled = false;

            ProgPauseflag = 0;
            changelength = 0;
            changestep = 0;
            completingflag = 0;

            //进度条属性初始化
            FtoR_progressBarControl.Properties.Step = 1;
            FtoR_progressBarControl.Properties.PercentView = true;
            FtoR_progressBarControl.Properties.Minimum = 0;
            FtoR_progressBarControl.Properties.Maximum = 100000;
            FtoR_progressBarControl.EditValue = 0;

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
            FtoR_backgroundWorker.DoWork += new DoWorkEventHandler(FtoR_backgroundWorker_DoWork);
            FtoR_backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(FtoR_backgroundWorker_RunWorkerCompleted);
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
        /// 点击关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void featuretoraster_Cancelbutton_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(FtoR_progressBarControl.EditValue.ToString()) == 0||Convert.ToInt32(FtoR_progressBarControl.EditValue.ToString()) == FtoR_progressBarControl.Properties.Maximum )
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
        /// <summary>
        /// 执行矢量转栅格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FtoR_backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            //设置CancellationPending可以退出任务线程
            if (worker.CancellationPending == true)
            {
                e.Cancel = true;
            }
            else
            {

                try
                {
                    FileInfo inputinfo = new FileInfo(featuretoraster_inputcombox.Text);
                    FileInfo outputinfo = new FileInfo(featuretoraster_output.Text);


                    IWorkspaceFactory outputworkspaceFactory = new RasterWorkspaceFactoryClass();
                    IWorkspace outputworkspaceop = outputworkspaceFactory.OpenFromFile(outputinfo.DirectoryName, 0);//创建输出工作空间

                    IWorkspaceFactory workspacefactory = new ShapefileWorkspaceFactoryClass();
                    IFeatureWorkspace featureworkspace = workspacefactory.OpenFromFile(inputinfo.DirectoryName, 0) as IFeatureWorkspace;
                    IFeatureClass featureclass = featureworkspace.OpenFeatureClass(inputinfo.Name);
                    IFeatureClassDescriptor featureClassDescriptor = new FeatureClassDescriptorClass();
                    featureClassDescriptor.Create(featureclass, null, featuretoraster_fieldcombox.Text);
                    IGeoDataset geodatasetop = featureClassDescriptor as IGeoDataset;

                    IConversionOp conversionOp = new RasterConversionOpClass();
                    //转换设置
                    IRasterAnalysisEnvironment rasterAnalysisEnvironment = conversionOp as IRasterAnalysisEnvironment;
                    //栅格的大小
                    double dCellSize = Convert.ToDouble(featuretoraster_cellsize.Text);
                    object oCellSize = dCellSize as object;
                    //设置栅格大小
                    rasterAnalysisEnvironment.SetCellSize(esriRasterEnvSettingEnum.esriRasterEnvValue, ref oCellSize);
                    string format = "";
                    if (outputinfo.Extension == ".tif")
                    {
                        format = "TIFF";
                    }
                    else if (outputinfo.Extension == ".img")
                    {
                        format = "IMAGINE Image";
                    }
                    else
                    {
                        format = "GRID";
                    }
                    //启动预处理计时
                    watch = new Stopwatch();
                    watch.Start();
                    //执行转换
                    conversionOp.ToRasterDataset(geodatasetop, format, outputworkspaceop, outputinfo.Name);
                }
                catch (Exception erro)
                {

                    MessageBox.Show(erro.Message, "错误");
                }

            }
        }
        /// <summary>
        /// 完成矢量转栅格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FtoR_backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //任务线程执行完毕
            BackgroundWorker worker = sender as BackgroundWorker;
            worker.DoWork -= new DoWorkEventHandler(FtoR_backgroundWorker_DoWork);
            worker.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(FtoR_backgroundWorker_RunWorkerCompleted);
            ProgPauseflag = 0;
            //将进度条当前值设置为最大值
            FtoR_progressBarControl.EditValue = FtoR_progressBarControl.Properties.Maximum;
            //设置featuretoraster_OKbutton、featuretoraster_Cancelbutton为可用
            featuretoraster_OKbutton.Enabled = true;
            featuretoraster_Cancelbutton.Enabled = true;
            if (e.Cancelled == true)
            {
                MessageBox.Show("任务被终止！","消息");
            }
            else if (e.Error != null)
            {
                MessageBox.Show("错误："+e.Error.Message, "消息");
            }
            else
            {
                MessageBox.Show("完成！","消息");
                featuretoraster_Exportbutton.Enabled = true;
            }
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
                if (prefile.Extension=="")
                {
                    file = new FileInfo(e.FullPath + "\\w001001.adf");
                }
                else
                {
                    file = new FileInfo(e.FullPath);
                }
                //检查是否跨线程调用
                if (FtoR_progressBarControl.InvokeRequired)
                {
                    //将方法委托到主线程
                    methoddelegate mydelegate = new methoddelegate((object source1, FileSystemEventArgs e1) => FilesOnChanged(source, e));
                    FtoR_progressBarControl.Invoke(mydelegate, source, e);
                }
                else
                {
                    FileSystemWatcher filewatcher = source as FileSystemWatcher;
                    if (Convert.ToInt32(FtoR_progressBarControl.EditValue.ToString()) == FtoR_progressBarControl.Properties.Maximum)
                    {
                        //根据进度条当前值判断转换结束
                        filewatcher.EnableRaisingEvents = false;
                        filewatcher.Changed -= new FileSystemEventHandler(FilesOnChanged);
                        filewatcher.Created -= new FileSystemEventHandler(FilesOnChanged);
                        return;
                    }
                    //停止计时，估算结果文件大小并重新设置进度条属性
                    watch.Stop();
                    if (FtoR_progressBarControl.Properties.Maximum == 100000)
                    {
                        int maximum = (int)watch.ElapsedMilliseconds * 2910;
                        FtoR_progressBarControl.Properties.Maximum = maximum;
                        FtoR_progressBarControl.Properties.Step = 10000;
                    }
                    int value = FtoR_progressBarControl.Properties.Maximum / 1000;
                    if (file.Exists)
                    {
                        if (Convert.ToInt32(FtoR_progressBarControl.EditValue.ToString()) <= (int)file.Length)
                        {
                            ProgPauseflag = 0;//文件长度大于进度条当前值
                            FtoR_progressBarControl.EditValue = file.Length;
                        }
                        else
                        {
                            ProgPauseflag = 1;//文件长度小于进度条当前值，暂停并等待
                            changelength = Convert.ToInt32(FtoR_progressBarControl.EditValue.ToString()) - (int)file.Length;
                        }
                    }

                }
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
                if (Convert.ToInt32(FtoR_progressBarControl.EditValue.ToString()) == FtoR_progressBarControl.Properties.Maximum || worker.CancellationPending)
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
        private void ProgBar_backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (ProgPauseflag == 0)
            {
                //进度条执行到80%时
                if (Convert.ToInt32(FtoR_progressBarControl.EditValue.ToString()) > (0.8 * FtoR_progressBarControl.Properties.Maximum))
                {
                    FtoR_progressBarControl.Properties.Step = 5000;
                }
                //进度条执行到90%时
                if (Convert.ToInt32(FtoR_progressBarControl.EditValue.ToString()) > (0.9 * FtoR_progressBarControl.Properties.Maximum))
                {
                    completingflag = 1;
                    ProgPauseflag = 1;
                }
                FtoR_progressBarControl.PerformStep();

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
            FtoR_progressBarControl.Update();
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
        /// 单击Export To Map 按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void featuretoraster_Exportbutton_Click(object sender, EventArgs e)
        {
            //检查文件是否存在
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
                        featuretoraster_Exportbutton.Enabled = true;
                        break;
                    }
                    string[] files = System.IO.Directory.GetFiles(fileinfo.DirectoryName, fileinfo.Name + "*.sr.lock", System.IO.SearchOption.TopDirectoryOnly);
                    if (files.Length == 0)
                    {
                        exportwatch.Stop();
                        main.openRasterFile(m_resultPath);
                        break;
                    }
                }
                featuretoraster_Exportbutton.Enabled = false;
            }
            else
            {
                MessageBox.Show("输出文件不存在");
                return;
            }

        }
    }
}