using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using System.IO;
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
        private AxMapControl mapcontrol = null;

        public RasterToFeatureForm(List<IRasterLayer> rlist,AxMapControl map)
        {
            InitializeComponent();
            rasterlayerlist = rlist;
            mapcontrol = map;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RasterToFeatureForm_Load(object sender, EventArgs e)
        {
            //将featurelayerlist中的元素添加到inputcombox中
            for (int i = 0; i < rasterlayerlist.Count; i++)
            {
                IDataLayer datalayer = rasterlayerlist[i] as IDataLayer;
                IWorkspaceName w_name = ((IDatasetName)(datalayer.DataSourceName)).WorkspaceName;
                rastertofeature_input.Properties.Items.Add(w_name.PathName + rasterlayerlist[i].Name);
            }
            rastertofeature_geometry.Properties.Items.Add("POINT");
            rastertofeature_geometry.Properties.Items.Add("POLYLINE");
            rastertofeature_geometry.Properties.Items.Add("POLYGON");
        }
        /// <summary>
        /// 打开文件对话框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rastertofeature_input_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
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
        private void rastertofeature_output_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Title = "输出文件";
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                rastertofeature_output.Text = savefile.FileName;
            }
        }

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
            FileInfo finfo = new FileInfo(rastertofeature_output.Text);
            IGeoDataset geodataset;
            int flag = 0;//该标记用于判断用户输入的矢量文件是否已存在于当前地图中

            IWorkspaceFactory workspaceFactory = new ShapefileWorkspaceFactoryClass();
            IWorkspace workspace = workspaceFactory.OpenFromFile(finfo.DirectoryName, 0);//创建输出工作空间

            IConversionOp conversionOp = new RasterConversionOpClass();
            //创建栅格
            IRasterDataset rasterdataset = null;
            IGeoDataset geodatasetresult=null;
            for (int i = 0; i < rasterlayerlist.Count; i++)
            {
                //判断添加的要素是否已存在于rasterlayerlist
                IDataLayer datalayer = rasterlayerlist[i] as IDataLayer;
                IDatasetName datasetname = datalayer.DataSourceName as IDatasetName;
                IWorkspaceName w_name = datasetname.WorkspaceName;

                if (w_name.PathName + "\\"+datasetname.Name== rastertofeature_input.SelectedText)
                {
                    rasterdataset = rasterlayerlist[i].Raster as IRasterDataset;
                    flag = 1;
                }
            }
            if (flag == 0)
            {
                FileInfo inputfinfo = new FileInfo(rastertofeature_input.Text);
                IWorkspaceFactory workspacefactory=new RasterWorkspaceFactoryClass() ;
                IRasterWorkspace featureworkspace = workspacefactory.OpenFromFile(inputfinfo.DirectoryName, 0) as IRasterWorkspace;
                rasterdataset = featureworkspace.OpenRasterDataset(inputfinfo.Name) ;
            }
            geodataset = rasterdataset as IGeoDataset;
            //执行转换
            if (rastertofeature_geometry.Text=="POINT")
            {
                geodatasetresult = conversionOp.ToFeatureData(geodataset, esriGeometryType.esriGeometryPoint, workspace, finfo.Name);
            }
            else
            {
                if (rastertofeature_geometry.Text=="POLYLINE")
                {
                    geodatasetresult = conversionOp.ToFeatureData(geodataset, esriGeometryType.esriGeometryPolyline, workspace, finfo.Name);
                }
                else
                {
                    geodatasetresult = conversionOp.ToFeatureData(geodataset, esriGeometryType.esriGeometryPolygon, workspace, finfo.Name);
                }
            }
            this.Close();
            if (MessageBox.Show("是否将结果加入地图窗口？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                mapcontrol.AddShapeFile(finfo.DirectoryName, finfo.Name);
                mapcontrol.Refresh();
                this.Dispose();
            }
        }


    }
}