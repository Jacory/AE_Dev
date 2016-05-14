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
using System.Threading.Tasks;
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
        private AxMapControl mapcontrol = null;

        private string m_resultPath = null;

        private System.ComponentModel.BackgroundWorker backgroundWorker1 = new BackgroundWorker();


        public FeatureToRasterForm(List<IFeatureLayer> flist, AxMapControl map)
        {
            InitializeComponent();
            featurelayerlist = flist;
            mapcontrol = map;

            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
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
            featuretoraster_cellsize.Text = "100";
            featuretoraster_Format.Properties.Items.Add("GRID");
            featuretoraster_Format.Properties.Items.Add("TIFF");
            featuretoraster_Format.Properties.Items.Add("IMAGINE Image");
            featuretoraster_Format.SelectedItem = "GRID";
        }
        /// <summary>
        /// 打开文件对话框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void featuretoraster_inputcombox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
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
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Title = "输出文件";
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                featuretoraster_output.Text = savefile.FileName;
            }

        }
        /// <summary>
        /// 读取矢量要素字段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void featuretoraster_inputcombox_TextChanged(object sender, EventArgs e)
        {
            //清空fieldcombox所有元素，重新添加
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
                    //当矢量文件不存在于featurelayerlist中
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
        /// 点击确定
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
            if (backgroundWorker1.IsBusy != true)
            {
                // Start the asynchronous operation.
                backgroundWorker1.RunWorkerAsync();
            }
        }
        /// <summary>
        /// 点击关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void featuretoraster_Cancelbutton_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker.CancellationPending == true)
            {
                e.Cancel = true;
            }
            else
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
                IRasterDataset opresult = conversionOp.ToRasterDataset(geodatasetop, featuretoraster_Format.Text, outputworkspaceop, outputinfo.Name);
                IRasterLayer rasterlayer = new RasterLayerClass();
                rasterlayer.CreateFromDataset(opresult);

                if (featuretoraster_Format.Text=="TIFF")
                {
                    m_resultPath = featuretoraster_output.Text + ".tif";
                }
                else
                {
                    if (featuretoraster_Format.Text=="IMAGINE Image")
                    {
                        m_resultPath = featuretoraster_output.Text + ".img";
                    }
                }
            }
        }

        // This event handler updates the progress.
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            memoEdit1.Text = (e.ProgressPercentage.ToString() + "%");
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                memoEdit1.Text = "Canceled!";
            }
            else if (e.Error != null)
            {
                memoEdit1.Text = "Error: " + e.Error.Message;
            }
            else
            {
                memoEdit1.Text = "Done!";
                addData(m_resultPath);
            }
        }


        private void addData(string path)
        {
            FileInfo finfo = new FileInfo(path);

            IWorkspaceFactory pWorkspaceFacotry = new RasterWorkspaceFactory();
            IWorkspace pWorkspace = pWorkspaceFacotry.OpenFromFile(finfo.DirectoryName, 0);
            IRasterWorkspace pRasterWorkspace = pWorkspace as IRasterWorkspace;
            IRasterDataset pRasterDataset = pRasterWorkspace.OpenRasterDataset(finfo.Name);

            // 影像金字塔的判断与创建
            IRasterPyramid pRasterPyamid = pRasterDataset as IRasterPyramid;
            if (pRasterPyamid != null)
            {
                if (!(pRasterPyamid.Present))
                {
                    pRasterPyamid.Create();
                }
            }
            // 多波段图像
            IRasterBandCollection pRasterBands = (IRasterBandCollection)pRasterDataset;
            int pBandCount = pRasterBands.Count;
            IRaster pRaster = null;
            if (pBandCount > 3)
            {
                pRaster = (pRasterDataset as IRasterDataset2).CreateFullRaster();
            }
            else
            {
                pRaster = pRasterDataset.CreateDefaultRaster();
            }

            IRasterLayer pRasterLayer = new RasterLayer();
            pRasterLayer.CreateFromRaster(pRaster);

            pBandCount = pRasterLayer.BandCount;

            ILayer pLayer = pRasterLayer as ILayer;

            mapcontrol.AddLayer(pLayer);
            mapcontrol.Refresh();
        }

    }
}