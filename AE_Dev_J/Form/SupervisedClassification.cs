using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using DevExpress.XtraEditors;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SpatialAnalyst;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.DataSourcesFile;

namespace AE_Dev_J.Form
{
    public partial class SupervisedClassification : DevExpress.XtraEditors.XtraForm
    {
        private MainForm main = null;//用于调用主窗口mapcontrol控件

        public SupervisedClassification(MainForm mainform)
        {
            InitializeComponent();
            main = mainform;
            main.CreateSample += new MainForm.CreateSampleEventHander((IGeometry geometry) => BegineCreateSample(geometry));//委托BegineCreateSample方法至mapcontrol图形绘制事件
        }
        /// <summary>
        /// 单击工具条按钮“采集”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SC_SelectSampleButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (SampleLayerCombox.Tag==null)
            {
                MessageBox.Show("请选择栅格图层！");
                return;
            }
            main.drawSampleflag = 1;
            main.TrackPolyonState = 2;
            (main.getMapControl()).MousePointer = ESRI.ArcGIS.Controls.esriControlsMousePointer.esriPointerCrosshair;
            main.Focus();
        }
        /// <summary>
        /// 根据mapcontrol绘制的多边形在样本表格中生成样本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BegineCreateSample(IGeometry SampleGeometry)
        {
            IPolygon polygon = (IPolygon)SampleGeometry;
            if (SampleLayerCombox.Tag!=null)
            {
                //计算像元数
                IArea area = polygon as IArea;
                IGeoDataset geodataset = SampleLayerCombox.Tag as IGeoDataset;
                IRaster raster = geodataset as IRaster;
                IRasterProps rasterprops = raster as IRasterProps;
                double pixelcount = System.Math.Abs(area.Area) / (rasterprops.MeanCellSize().X * rasterprops.MeanCellSize().Y);

                //生成表格
                if (SC_dataGridView.ColumnCount==0)
                {
                    SC_dataGridView.Columns.Add("ID","ID");
                    SC_dataGridView.Columns.Add("name","样本名称");
                    SC_dataGridView.Columns.Add("value","样本值");
                    SC_dataGridView.Columns.Add("color","样本颜色");
                    SC_dataGridView.Columns.Add("count","像元数（近似值）");
                }
                SC_dataGridView.Rows.Add();
                SC_dataGridView.Rows[SC_dataGridView.Rows.Count - 1].Cells["ID"].Value = SC_dataGridView.Rows.Count;
                SC_dataGridView.Rows[SC_dataGridView.Rows.Count - 1].Cells["name"].Value = "样本" + (SC_dataGridView.Rows.Count).ToString();
                SC_dataGridView.Rows[SC_dataGridView.Rows.Count - 1].Cells["value"].Value = (SC_dataGridView.Rows.Count).ToString();
                SC_dataGridView.Rows[SC_dataGridView.Rows.Count - 1].Cells["count"].Value = Convert.ToInt32(pixelcount) + 1;

                //生成随机色
                Random random = new Random();
                Color linecolor = new Color();
                linecolor = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
                //填充单元格颜色
                SC_dataGridView.Rows[SC_dataGridView.Rows.Count - 1].Cells["color"].Style.BackColor = linecolor;
                //将polygon存放到gridview表color列对应的tag中
                SC_dataGridView.Rows[SC_dataGridView.Rows.Count - 1].Cells["color"].Tag = polygon;

                //新建绘制图形的填充符号
                IRgbColor arccolor = new RgbColorClass();
                arccolor.RGB = linecolor.B * 65536 + linecolor.G * 256 + linecolor.R;
                ILineSymbol outline = new SimpleLineSymbolClass();
                outline.Width = 3;
                outline.Color = arccolor;
                IFillSymbol fillsymbol = new SimpleFillSymbolClass();
                ISimpleFillSymbol pFillsyl = fillsymbol as ISimpleFillSymbol;
                pFillsyl.Style = esriSimpleFillStyle.esriSFSNull;
                fillsymbol.Outline = outline;

                IPolygonElement PolygonElement = new PolygonElementClass();
                IElement pElement = PolygonElement as IElement;
                pElement.Geometry = SampleGeometry;

                IFillShapeElement FillShapeElement = pElement as IFillShapeElement;
                FillShapeElement.Symbol = fillsymbol;
                IGraphicsContainer pGraphicsContainer =main.getMapControl().Map as IGraphicsContainer;
                pGraphicsContainer.AddElement((IElement)PolygonElement, 0);
                main.getMapControl().ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
            }

        }
        /// <summary>
        /// 生成工具条combobox下拉菜单项，存放mapcontrol所有栅格图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SampleLayerCombox_ShowingEditor(object sender, DevExpress.XtraBars.ItemCancelEventArgs e)
        {
            LayerCombox.Items.Clear();
            LayerCombox.BeginUpdate();
            //添加栅格图层名称至combobox
            for (int i = 0; i < main.getMapControl().LayerCount; i++)
            {
                if (main.getMapControl().get_Layer(i) is IRasterLayer)
                {
                    IRasterLayer rasterlayer = main.getMapControl().get_Layer(i) as IRasterLayer;
                    LayerCombox.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(rasterlayer.Name, rasterlayer.Raster));
                }
            }
            LayerCombox.EndUpdate();
        }
        /// <summary>
        /// 在SampleLayerCombox的Tag中存放combobox选择项的栅格数据，方便直接调用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LayerCombox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.Controls.ImageComboBoxItem tt = (DevExpress.XtraEditors.Controls.ImageComboBoxItem)(sender as ImageComboBoxEdit).SelectedItem;
            SampleLayerCombox.Tag = tt.Value;
        }
        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SupervisedClassification_FormClosing(object sender, FormClosingEventArgs e)
        {
            main.CreateSample -= new MainForm.CreateSampleEventHander((IGeometry geometry) => BegineCreateSample(geometry));//取消事件订阅
            main.getMapControl().ActiveView.GraphicsContainer.DeleteAllElements();
            main.getMapControl().Refresh();
            main.getMapControl().MousePointer = ESRI.ArcGIS.Controls.esriControlsMousePointer.esriPointerArrow;
            main.TrackPolyonState = 0;
        }

        private void SC_CreateSampleFiles_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            SaveFileDialog SaveSignatureFile = new SaveFileDialog();
            SaveSignatureFile.Title = "生成Signature文件";
            SaveSignatureFile.Filter = "样本文件|*.gsg";
            if (SaveSignatureFile.ShowDialog()==DialogResult.OK)
            {
                IGeoDataset inputraster = SampleLayerCombox.Tag as IGeoDataset;

                //在临时文件夹生成featureclass
                int changefilename = 0;
                while (System.IO.File.Exists(Application.StartupPath + "\\temp\\TempSample" + changefilename + ".shp"))
                {
                    changefilename++;
                }

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
                pGeometryDefEdit.SpatialReference_2 = inputraster.SpatialReference;
                pFieldEdit.GeometryDef_2 = pGeometryDef;
                pFieldsEdit.AddField(pField);

                IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactory();
                IFeatureWorkspace pFeatureWorkspace = pWorkspaceFactory.OpenFromFile(Application.StartupPath + "\\temp", 0) as IFeatureWorkspace;
                IFeatureClass featureclass = pFeatureWorkspace.CreateFeatureClass("TempSample" + changefilename+".shp", pFields, null, null, esriFeatureType.esriFTSimple, "Shape", "");
                for (int i = 0; i < SC_dataGridView.Rows.Count; i++)
                {
                    IFeature feature = featureclass.CreateFeature();
                    feature.Shape = SC_dataGridView.Rows[i].Cells["color"].Tag as IPolygon;
                    feature.Store();
                }

                IGeoDataset Sampledataset = featureclass as IGeoDataset;
                IMultivariateOp Multivariateop = new RasterMultivariateOpClass();
                Multivariateop.CreateSignatures(inputraster, Sampledataset, SaveSignatureFile.FileName+".gsg", true);
            }
        }

        private void SC_OpenSamplefile_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog OpenSampleFile = new OpenFileDialog();
            OpenSampleFile.Title = "打开样本文件";
            OpenSampleFile.Filter = "样本文件|*.shp";
            if (OpenSampleFile.ShowDialog() == DialogResult.OK)
            {
                SC_dataGridView.Rows.Clear();
                FileInfo Samplefile = new FileInfo(OpenSampleFile.FileName);
                IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactory();
                IFeatureWorkspace pFeatureWorkspace = pWorkspaceFactory.OpenFromFile(Samplefile.DirectoryName, 0) as IFeatureWorkspace;
                IFeatureClass featureclass = pFeatureWorkspace.OpenFeatureClass(Samplefile.Name);
                IFeatureCursor pcursor = featureclass.Search(null, false);
                main.openVectorFile(OpenSampleFile.FileName);
                
                //根据要素类填充样本表
                for (int i = 0; i <featureclass.FeatureCount(null); i++)
                {
                    IFeature pfeature = pcursor.NextFeature();

                    SC_dataGridView.Rows.Add();
                    SC_dataGridView.Rows[SC_dataGridView.Rows.Count - 1].Cells["ID"].Value = SC_dataGridView.Rows.Count;
                    SC_dataGridView.Rows[SC_dataGridView.Rows.Count - 1].Cells["name"].Value = pfeature.get_Value(pfeature.Fields.FindField("ClassName"));
                    SC_dataGridView.Rows[SC_dataGridView.Rows.Count - 1].Cells["value"].Value = pfeature.get_Value(pfeature.Fields.FindField("ClassValue"));
                    SC_dataGridView.Rows[SC_dataGridView.Rows.Count - 1].Cells["count"].Value = pfeature.get_Value(pfeature.Fields.FindField("Count"));
                    SC_dataGridView.Rows[SC_dataGridView.Rows.Count - 1].Cells["color"].Style.BackColor
                        = Color.FromArgb((int)pfeature.get_Value(pfeature.Fields.FindField("Red")), (int)pfeature.get_Value(pfeature.Fields.FindField("Green")), (int)pfeature.get_Value(pfeature.Fields.FindField("Blue")));
                    SC_dataGridView.Rows[SC_dataGridView.Rows.Count - 1].Cells["color"].Tag = pfeature.Shape;
                    IGeometry SampleGeometry = pfeature.Shape;
                    Color linecolor = new Color(); 
                    linecolor = Color.FromArgb((int)pfeature.get_Value(pfeature.Fields.FindField("Red")), (int)pfeature.get_Value(pfeature.Fields.FindField("Green")), (int)pfeature.get_Value(pfeature.Fields.FindField("Blue")));

                    //新建绘制图形的填充符号
                    IRgbColor arccolor = new RgbColorClass();
                    arccolor.RGB = linecolor.B * 65536 + linecolor.G * 256 + linecolor.R;
                    ILineSymbol outline = new SimpleLineSymbolClass();
                    outline.Width = 3;
                    outline.Color = arccolor;
                    IFillSymbol fillsymbol = new SimpleFillSymbolClass();
                    ISimpleFillSymbol pFillsyl = fillsymbol as ISimpleFillSymbol;
                    pFillsyl.Style = esriSimpleFillStyle.esriSFSNull;
                    fillsymbol.Outline = outline;

                    IPolygonElement PolygonElement = new PolygonElementClass();
                    IElement pElement = PolygonElement as IElement;
                    pElement.Geometry = SampleGeometry;

                    IFillShapeElement FillShapeElement = pElement as IFillShapeElement;
                    FillShapeElement.Symbol = fillsymbol;
                    IGraphicsContainer pGraphicsContainer = main.getMapControl().Map as IGraphicsContainer;
                    pGraphicsContainer.AddElement((IElement)PolygonElement, 0);
                    main.getMapControl().ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
                }
            }
        }

        private void SC_SaveSample_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveFileDialog SaveShapeFile = new SaveFileDialog();
            SaveShapeFile.Title = "生成样本文件";
            SaveShapeFile.Filter = "样本文件|*.shp";
            if (SaveShapeFile.ShowDialog() == DialogResult.OK)
            {
                IFields pFields = new FieldsClass();
                IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;

                IField pField = new FieldClass();
                IFieldEdit pFieldEdit = pField as IFieldEdit;
                pFieldEdit.Name_2 = "Shape";
                pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;

                IField pField2 = new FieldClass();
                IFieldEdit pFieldEdit2 = pField2 as IFieldEdit;
                pFieldEdit2.Name_2 = "ClassName";
                pFieldEdit2.Type_2 = esriFieldType.esriFieldTypeString;

                IField pField3 = new FieldClass();
                IFieldEdit pFieldEdit3 = pField3 as IFieldEdit;
                pFieldEdit3.Name_2 = "ClassValue";
                pFieldEdit3.Type_2 = esriFieldType.esriFieldTypeInteger;

                IField pField4 = new FieldClass();
                IFieldEdit pFieldEdit4 = pField4 as IFieldEdit;
                pFieldEdit4.Name_2 = "Red";
                pFieldEdit4.Type_2 = esriFieldType.esriFieldTypeInteger;

                IField pField5 = new FieldClass();
                IFieldEdit pFieldEdit5 = pField5 as IFieldEdit;
                pFieldEdit5.Name_2 = "Green";
                pFieldEdit5.Type_2 = esriFieldType.esriFieldTypeInteger;

                IField pField6 = new FieldClass();
                IFieldEdit pFieldEdit6 = pField6 as IFieldEdit;
                pFieldEdit6.Name_2 = "Blue";
                pFieldEdit6.Type_2 = esriFieldType.esriFieldTypeInteger;

                IField pField7 = new FieldClass();
                IFieldEdit pFieldEdit7 = pField7 as IFieldEdit;
                pFieldEdit7.Name_2 = "Count";
                pFieldEdit7.Type_2 = esriFieldType.esriFieldTypeInteger;

                //设置geometry definition
                IGeometryDef pGeometryDef = new GeometryDefClass();
                IGeometryDefEdit pGeometryDefEdit = pGeometryDef as IGeometryDefEdit;
                pGeometryDefEdit.GeometryType_2 = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon;
                pGeometryDefEdit.SpatialReference_2 = null;
                pFieldEdit.GeometryDef_2 = pGeometryDef;

                pFieldsEdit.AddField(pField);
                pFieldsEdit.AddField(pField2);
                pFieldsEdit.AddField(pField3);
                pFieldsEdit.AddField(pField4);
                pFieldsEdit.AddField(pField5);
                pFieldsEdit.AddField(pField6);
                pFieldsEdit.AddField(pField7);

                FileInfo fileinfo = new FileInfo(SaveShapeFile.FileName);

                IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactory();
                IFeatureWorkspace pFeatureWorkspace = pWorkspaceFactory.OpenFromFile(fileinfo.DirectoryName, 0) as IFeatureWorkspace;
                IFeatureClass featureclass = pFeatureWorkspace.CreateFeatureClass(fileinfo.Name, pFields, null, null, esriFeatureType.esriFTSimple, "Shape", "");
                
                for (int i = 0; i < SC_dataGridView.Rows.Count; i++)
                {
                    IFeature feature = featureclass.CreateFeature();
                    feature.Shape = SC_dataGridView.Rows[i].Cells["color"].Tag as IPolygon;
                    feature.set_Value(feature.Fields.FindField("ClassName"), SC_dataGridView.Rows[i].Cells["name"].Value);
                    feature.set_Value(feature.Fields.FindField("ClassValue"), SC_dataGridView.Rows[i].Cells["value"].Value);
                    feature.set_Value(feature.Fields.FindField("Red"), SC_dataGridView.Rows[i].Cells["color"].Style.BackColor.R);
                    feature.set_Value(feature.Fields.FindField("Green"), SC_dataGridView.Rows[i].Cells["color"].Style.BackColor.G);
                    feature.set_Value(feature.Fields.FindField("Blue"), SC_dataGridView.Rows[i].Cells["color"].Style.BackColor.B);
                    feature.set_Value(feature.Fields.FindField("Count"), SC_dataGridView.Rows[i].Cells["count"].Value);
                    feature.Store();
                }
                MessageBox.Show("完成！");
            }
        }

        private void SC_dataGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            for (int i = 0; i < SC_dataGridView.SelectedCells.Count; i++)
            {
                if (i==3)
                {
                    SC_dataGridView.SelectedCells[i].Style.SelectionBackColor = Color.FromArgb(0, Color.Red);
                }
            }
            main.getMapControl().ActiveView.Extent = ((IGeometry)SC_dataGridView.Rows[e.RowIndex].Cells["color"].Tag).Envelope;
            main.getMapControl().Refresh();
        }

        private void SC_RemoveSample_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (SC_dataGridView.SelectedRows.Count>0)
            {
                SC_dataGridView.Rows.Remove(SC_dataGridView.SelectedRows[0]);
            }
        }

        private void SC_ClearSamples_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SC_dataGridView.Rows.Clear();
        }

        private void SC_dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SC_colorPickEdit.Visible = false;
            SC_dataGridView.Refresh();
            this.Refresh();
            if (e.ColumnIndex == SC_dataGridView.Columns["color"].Index && e.RowIndex != -1)
            {
                //SC_dataGridView.SelectedCells[0].Style.SelectionBackColor = Color.FromArgb(0, Color.Red);
                //SC_colorPickEdit.Visible = true;
                //SC_colorPickEdit.Location = this.PointToClient( new System.Drawing.Point(MousePosition.X,MousePosition.Y));
                //SC_colorPickEdit.Tag = e.RowIndex;
                ColorDialog cd = new ColorDialog();
                cd.ShowDialog();
            }
        }

        private void SC_colorPickEdit_EditValueChanged(object sender, EventArgs e)
        {
            SC_dataGridView.Rows[(int)SC_colorPickEdit.Tag].Cells["color"].Style.BackColor = SC_colorPickEdit.Color;
            SC_colorPickEdit.Visible = false;
            //SC_dataGridView.Refresh();
            //this.Refresh();
        }


    }
}