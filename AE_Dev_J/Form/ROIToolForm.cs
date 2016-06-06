using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SpatialAnalyst;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Display;

namespace AE_Dev_J.Form
{
    public partial class ROIToolForm : DevExpress.XtraEditors.XtraForm
    {
        private MainForm main = null;//用于调用主窗口mapcontrol控件

        public ROIToolForm(MainForm mainform)
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
            main.drawSampleflag = 1;
            main.trackPolyonState = 2;
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
            main.trackPolyonState = 0;
        }


    }
}