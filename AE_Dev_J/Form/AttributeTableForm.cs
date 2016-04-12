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
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using DevExpress.XtraGrid;
using DevExpress.XtraTab;
using DevExpress.XtraTab.ViewInfo;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
namespace AE_Dev_J.Form
{
    /// <summary>
    /// 属性表窗口
    /// </summary>
    public partial class AttributeTableForm : DevExpress.XtraEditors.XtraForm
    {
        private IFeatureLayer m_layer = null;
        private AxMapControl m_mapControl = null; // 属性表需要与mapControl做交互

        public AttributeTableForm(IFeatureLayer layer, AxMapControl mapControl)
        {
            InitializeComponent();

            m_layer = layer;
            m_mapControl = mapControl;
        }

        private void AttributeTableForm_Load(object sender, EventArgs e)
        {
            this.Text = "Attribute Table";
            //+ m_layer.FeatureClass.FeatureCount(new ESRI.ArcGIS.Geodatabase.QueryFilter()).ToString() + " features";
            importAttribute(m_layer);
        }

        /// <summary>
        /// 导入vector layer的属性到grid view中
        /// **每一个有意义的方法都要具有方法注释**
        /// </summary>
        /// <param name="featurelayer">矢量数据</param>
        private void importAttribute(IFeatureLayer featurelayer)
        {
            //检查文件路径是否存在于tag中，避免重复创建表格
            IDataLayer datalayer = featurelayer as IDataLayer;
            IWorkspaceName w_name = ((IDatasetName)(datalayer.DataSourceName)).WorkspaceName;
            for (int i = 0; i < xtraTabControl1.TabPages.Count; i++)
            {
                if (w_name == xtraTabControl1.TabPages[i].Tag)
                {
                    return;
                }
            }
            //创建gridcontrol的数据源datatable
            DataTable dt = new DataTable();
            IFeatureClass m_featureclass = featurelayer.FeatureClass;
            if (m_featureclass == null)
            {
                return;
            }
            for (int i = 0; i < m_featureclass.Fields.FieldCount; i++)
            {
                DataColumn dc = new DataColumn(m_featureclass.Fields.get_Field(i).Name);
                dt.Columns.Add(dc);
            }
            IFeatureCursor pFeatureCuror = m_featureclass.Search(null, false);
            IFeature pFeature = pFeatureCuror.NextFeature();
            while (pFeature != null)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < m_featureclass.Fields.FieldCount; j++)
                {
                    dr[j] = pFeature.get_Value(j).ToString();
                }
                dt.Rows.Add(dr);
                pFeature = pFeatureCuror.NextFeature();
            }
            //创建tabpage
            xtraTabControl1.TabPages.Add(featurelayer.Name);
            xtraTabControl1.TabPages[xtraTabControl1.TabPages.Count - 1].Tag = w_name;
            //创建gridcontrol、gridview
            GridControl att_gridcontrol = new GridControl();
            this.xtraTabControl1.TabPages[xtraTabControl1.TabPages.Count - 1].Controls.Add(att_gridcontrol);
            att_gridcontrol.Name = "att_gridcontrol";
            att_gridcontrol.Dock = System.Windows.Forms.DockStyle.Fill;

            DevExpress.XtraGrid.Views.Grid.GridView att_gridview = new DevExpress.XtraGrid.Views.Grid.GridView();
            att_gridview.Name = "att_gridview";
            att_gridview.GridControl = att_gridcontrol;
            att_gridcontrol.MainView = att_gridview;
            att_gridcontrol.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            att_gridview});
            att_gridview.OptionsBehavior.Editable = false;
            att_gridview.OptionsView.ShowAutoFilterRow = true;
            att_gridview.OptionsFind.AlwaysVisible = true;
            att_gridview.OptionsView.ShowFooter = true;
            att_gridview.OptionsCustomization.AllowSort = false;
            att_gridview.OptionsSelection.MultiSelect = true;
            att_gridview.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            att_gridview.MouseDown += new MouseEventHandler(att_gridview_MouseDown);

            att_gridcontrol.DataSource = dt;
            gridControl2.DataSource = dt;
        }

        /// <summary>
        /// 添加新table来显示新传入矢量图层的属性
        /// </summary>
        /// <param name="vecLayer">适量图层</param>
        public void appendTable(IFeatureLayer vecLayer)
        {
            importAttribute(vecLayer);
        }
        /// <summary>
        /// 属性表页面关闭
        /// </summary>
        private void xtraTabControl1_CloseButtonClick(object sender, EventArgs e)
        {
            XtraTabControl tabControl = sender as XtraTabControl;
            ClosePageButtonEventArgs arg = e as ClosePageButtonEventArgs;
            tabControl.TabPages.Remove(arg.Page as XtraTabPage);
        }
        /// <summary>
        /// 鼠标左键单击表头选择整列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void att_gridview_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ColumnViewOptionsSelection att_seleoption = new ColumnViewOptionsSelection();
                att_seleoption.MultiSelect=true;
                DevExpress.XtraGrid.Views.Grid.GridView gv = (DevExpress.XtraGrid.Views.Grid.GridView)(sender);
                BaseView att_baseview = gv;
                GridHitInfo info = gv.CalcHitInfo(e.X, e.Y);

                

                if (info.InColumnPanel)
                {
                    gv.ClearSelection();
                    GridCell start = new GridCell(0, info.Column);
                    GridCell end = new GridCell(gv.RowCount-1, info.Column);
 
                    gv.SelectCells(start, end);
                }

            }
        }

        private void gridView2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DevExpress.XtraGrid.Views.Grid.GridView gv = (DevExpress.XtraGrid.Views.Grid.GridView)(sender);
                GridHitInfo info = gv.CalcHitInfo(e.X, e.Y);
                if (info.InColumnPanel)
                {
                    gv.ClearSelection();
                    GridCell start = new GridCell(0, info.Column);
                    GridCell end = new GridCell(gv.RowCount-1, info.Column);
                    gv.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
                    gv.SelectCellAnchorRange(start, end);
                    
                }

            }
        }

    }
}