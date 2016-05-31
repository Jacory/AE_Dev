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
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Geometry;
using DevExpress.XtraEditors;

namespace AE_Dev_J.Form
{
    public partial class RasterToFeatureForm : DevExpress.XtraEditors.XtraForm
    {
        private List<IRasterLayer> rasterlayerlist = null;//存放当前地图控件已加载的所有栅格图层
        private string m_resultPath = null;//存储输出文件路径
        private MainForm main = null;//用于调用主窗口的openRasterFile方法
        private delegate void methoddelegate(object source, FileSystemEventArgs e);//负责将文件监测FilesOnChanged方法委托到主线程，用于跨线程调用控件

        private Stopwatch watch = null;//用于计算ToRasterDataset方法的预处理时间，以此估算结果文件大小
        private int ProgPauseflag = 0;//用于文件监测线程与进度条线程的交互，值为1时进度条停止
        private int changelength = 0;//用于文件监测线程与进度条线程的交互，记录文件大小变化的字节数，以此更新进度条
        private int changestep = 0;//用于文件监测线程与进度条线程的交互，根据文件大小变化的字节数计算进度条更新步数
        private int completingflag = 0;//值为1时表示进度条执行到任务的90%，暂停进度条并等待ToRasterDataset方法执行完毕

        public RasterToFeatureForm(List<IRasterLayer> rlist,MainForm mainform)
        {
            InitializeComponent();
            rasterlayerlist = rlist;
            main = mainform;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RasterToFeatureForm_Load(object sender, EventArgs e)
        {
            //将rasterlayerlist中的元素添加到inputcombox中
            for (int i = 0; i < rasterlayerlist.Count; i++)
            {
                IDataLayer datalayer = rasterlayerlist[i] as IDataLayer;
                IWorkspaceName w_name = ((IDatasetName)(datalayer.DataSourceName)).WorkspaceName;
                //地图文档数据w_name.PathName没有斜杠，非地图文档数据w_name.PathName有斜杠，所以路径拼接的时候需要判断
                if (w_name.PathName.LastIndexOf("\\")==w_name.PathName.Length-1)
                {
                    rastertofeature_input.Properties.Items.Add(w_name.PathName + rasterlayerlist[i].Name);
                }
                else
                {
                    rastertofeature_input.Properties.Items.Add(w_name.PathName +"\\" +rasterlayerlist[i].Name);
                }
            }
            rastertofeature_geometry.Properties.Items.Add("POINT");
            rastertofeature_geometry.Properties.Items.Add("POLYLINE");
            rastertofeature_geometry.Properties.Items.Add("POLYGON");

            rastertofeature_exportbutton.Enabled = false;
        }
        /// <summary>
        /// 打开文件对话框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rastertofeature_input_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //打开文件
            if (e.Button.Index == 1)
            {
                OpenFileDialog openshapefile = new OpenFileDialog();
                openshapefile.Filter = "GRID文件|*.adf|TIFF文件|*.tif|TIFF文件|*.tiff|Image文件|*.img";
                openshapefile.Title = "打开栅格文件";
                if (openshapefile.ShowDialog() == DialogResult.OK)
                {
                    rastertofeature_input.Text = "";
                    rastertofeature_input.Text = openshapefile.FileName.Replace(".adf", "");
                }
            }
        }
        /// <summary>
        /// 输出文件对话框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rastertofeature_output_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //指定输出文件路径
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Title = "输出文件";
            savefile.Filter = "矢量文件|*.shp";
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                rastertofeature_output.Text = savefile.FileName;
            }
        }
        /// <summary>
        /// 点击确定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rastertofeature_OKbutton_Click(object sender, EventArgs e)
        {
            //检查输入是否为空
            if (rastertofeature_input.Text == ""
                || rastertofeature_output.Text == ""
                || rastertofeature_geometry.Text == "")
            {
                MessageBox.Show("输入不能为空");
                return;
            }
            //检查是否重名
            m_resultPath = rastertofeature_output.Text;
            FileInfo fileinfo = new FileInfo(m_resultPath);

            if (fileinfo.Exists)
            {
                MessageBox.Show("输出文件已存在");
                return;
            }
            //当前状态下设置featuretoraster_OKbutton不可用
            rastertofeature_OKbutton.Enabled = false;
            rastertofeature_cancelbutton.Enabled = false;
            rastertofeature_exportbutton.Enabled = false;

            ProgPauseflag = 0;
            changelength = 0;
            changestep = 0;
            completingflag = 0;

            //进度条属性初始化
            RtoF_progressBarControl.Properties.Step = 1;
            RtoF_progressBarControl.Properties.PercentView = true;
            RtoF_progressBarControl.Properties.Minimum = 0;
            RtoF_progressBarControl.Properties.Maximum = 100000;
            RtoF_progressBarControl.EditValue = 0;

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
            FtoR_backgroundWorker.DoWork += new DoWorkEventHandler(RtoF_backgroundWorker_DoWork);
            FtoR_backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(RtoF_backgroundWorker_RunWorkerCompleted);
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
                    file = new FileInfo(e.FullPath + "\\w001001.adf");
                }
                else
                {
                    file = new FileInfo(e.FullPath);
                }
                //检查是否跨线程调用
                if (RtoF_progressBarControl.InvokeRequired)
                {
                    //将方法委托到主线程
                    methoddelegate mydelegate = new methoddelegate((object source1, FileSystemEventArgs e1) => FilesOnChanged(source, e));
                    memoEdit1.Invoke(mydelegate, source, e);
                    RtoF_progressBarControl.Invoke(mydelegate, source, e);
                }
                else
                {
                    FileSystemWatcher filewatcher = source as FileSystemWatcher;
                    if (Convert.ToInt32(RtoF_progressBarControl.EditValue.ToString()) == RtoF_progressBarControl.Properties.Maximum)
                    {
                        //根据进度条当前值判断转换结束
                        filewatcher.EnableRaisingEvents = false;
                        filewatcher.Changed -= new FileSystemEventHandler(FilesOnChanged);
                        filewatcher.Created -= new FileSystemEventHandler(FilesOnChanged);
                        return;
                    }
                    //停止计时，估算结果文件大小并重新设置进度条属性
                    watch.Stop();
                    if (RtoF_progressBarControl.Properties.Maximum == 100000)
                    {

                        int maximum = (int)watch.ElapsedMilliseconds * 2910;
                        RtoF_progressBarControl.Properties.Maximum = maximum;
                        RtoF_progressBarControl.Properties.Step = 10000;
                    }

                    int value = RtoF_progressBarControl.Properties.Maximum / 1000;
                    if (file.Exists)
                    {
                        if (Convert.ToInt32(RtoF_progressBarControl.EditValue.ToString()) <= (int)file.Length)
                        {
                            ProgPauseflag = 0;//文件长度大于进度条当前值
                            RtoF_progressBarControl.EditValue = file.Length;
                        }
                        else
                        {
                            ProgPauseflag = 1;//文件长度小于进度条当前值，暂停并等待
                            changelength = Convert.ToInt32(RtoF_progressBarControl.EditValue.ToString()) - (int)file.Length;

                        }
                    }

                }
            }
        }
        /// <summary>
        /// 执行栅格转矢量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RtoF_backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
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
                    FileInfo inputfinfo = new FileInfo(rastertofeature_input.Text);
                    FileInfo outputinfo = new FileInfo(rastertofeature_output.Text);

                    IWorkspaceFactory workspaceFactory = new ShapefileWorkspaceFactoryClass();
                    IWorkspace workspace = workspaceFactory.OpenFromFile(outputinfo.DirectoryName, 0);//创建输出工作空间


                    IWorkspaceFactory workspacefactory = new RasterWorkspaceFactoryClass();
                    IRasterWorkspace featureworkspace = workspacefactory.OpenFromFile(inputfinfo.DirectoryName, 0) as IRasterWorkspace;
                    IRasterDataset rasterdataset = featureworkspace.OpenRasterDataset(inputfinfo.Name);
                    IGeoDataset geodataset = rasterdataset as IGeoDataset;

                    watch = new Stopwatch();
                    watch.Start();

                    //执行转换
                    IConversionOp conversionOp = new RasterConversionOpClass();
                    IGeoDataset geodatasetresult = null;
                    if (rastertofeature_geometry.Text == "POINT")
                    {
                        geodatasetresult = conversionOp.ToFeatureData(geodataset, esriGeometryType.esriGeometryPoint, workspace, outputinfo.Name);
                    }
                    else
                    {
                        if (rastertofeature_geometry.Text == "POLYLINE")
                        {
                            geodatasetresult = conversionOp.ToFeatureData(geodataset, esriGeometryType.esriGeometryPolyline, workspace, outputinfo.Name);
                        }
                        else
                        {
                            geodatasetresult = conversionOp.ToFeatureData(geodataset, esriGeometryType.esriGeometryPolygon, workspace, outputinfo.Name);
                        }
                    }
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
        private void RtoF_backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //任务线程执行完毕
            BackgroundWorker worker = sender as BackgroundWorker;
            worker.DoWork -= new DoWorkEventHandler(RtoF_backgroundWorker_DoWork);
            worker.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(RtoF_backgroundWorker_RunWorkerCompleted);
            ProgPauseflag = 0;
            //将进度条当前值设置为最大值
            RtoF_progressBarControl.EditValue = RtoF_progressBarControl.Properties.Maximum;
            rastertofeature_OKbutton.Enabled = true;
            rastertofeature_cancelbutton.Enabled = true;
            //设置featuretoraster_OKbutton为可用
            if (e.Cancelled == true)
            {
                memoEdit1.Text += "\r\n" + "Canceled!";
            }
            else if (e.Error != null)
            {
                memoEdit1.Text = "Error: " + e.Error.Message;
            }
            else
            {
                rastertofeature_exportbutton.Enabled = true;
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
                if (Convert.ToInt32(RtoF_progressBarControl.EditValue.ToString()) == RtoF_progressBarControl.Properties.Maximum || worker.CancellationPending)
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
                if (Convert.ToInt32(RtoF_progressBarControl.EditValue.ToString()) > (0.8 * RtoF_progressBarControl.Properties.Maximum))
                {
                    RtoF_progressBarControl.Properties.Step = 5000;
                }
                //进度条执行到90%时
                if (Convert.ToInt32(RtoF_progressBarControl.EditValue.ToString()) > (0.9 * RtoF_progressBarControl.Properties.Maximum))
                {
                    completingflag = 1;
                    ProgPauseflag = 1;
                }
                RtoF_progressBarControl.PerformStep();

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
            RtoF_progressBarControl.Update();
        }
        /// <summary>
        /// 进度条执行完毕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgBar_backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (e.Cancelled == true)
            {
                memoEdit1.Text += "\r\n" + "progressbar Canceled!";
            }
            else if (e.Error != null)
            {
                memoEdit1.Text = "\r\nError: " + e.Error.Message;
            }
            worker.DoWork -= new DoWorkEventHandler(ProgBar_backgroundWorker_DoWork);
            worker.ProgressChanged -= new ProgressChangedEventHandler(ProgBar_backgroundWorker_ProgressChanged);
            worker.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(ProgBar_backgroundWorker_RunWorkerCompleted);

        }
        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rastertofeature_cancelbutton_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(RtoF_progressBarControl.EditValue.ToString()) == 0 || Convert.ToInt32(RtoF_progressBarControl.EditValue.ToString()) == RtoF_progressBarControl.Properties.Maximum)
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
        /// 单击Export To Map 按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rastertofeature_exportbutton_Click(object sender, EventArgs e)
        {
            FileInfo fileinfo = new FileInfo(m_resultPath);
            main.openVectorFile(m_resultPath);
            rastertofeature_exportbutton.Enabled = false;
        }
    }
}