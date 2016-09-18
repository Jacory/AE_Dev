/*
 * Author : Hengyu Tian 
 * 
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using DevExpress.XtraGrid;
using DevExpress.XtraTab;
using DevExpress.XtraTab.ViewInfo;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraEditors;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.Grid;

namespace AE_Dev_J.Form
{
    /// <summary>
    /// 属性表窗口
    /// </summary>
    public partial class AttributeTableForm : DevExpress.XtraEditors.XtraForm
    {
        private AxMapControl m_mapControl = null; // 属性表需要与mapControl做交互
        private List<GridView> gridview_list = new List<GridView>(); // 存放当前显示的表格
        private List<IFeatureLayer> flayer_list = new List<IFeatureLayer>(); // 存放当前传入的所有图层

        public AttributeTableForm(IFeatureLayer layer, AxMapControl mapControl)
        {
            InitializeComponent();

            flayer_list.Add(layer);
            m_mapControl = mapControl;
        }

        private void AttributeTableForm_Load(object sender, EventArgs e)
        {
            //开始导入属性数据
            importAttribute(flayer_list[0]);
            this.Text = "Attribute Table : "
                + flayer_list[0].FeatureClass.FeatureCount(new ESRI.ArcGIS.Geodatabase.QueryFilter()).ToString() + " features";

            att_dockPaneltools.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
        }

        /// <summary>
        /// 导入vector layer的属性到grid view中
        /// 创建tabpage、gridcontrol、gridview
        /// </summary>
        /// <param name="featurelayer">矢量数据</param>
        private void importAttribute(IFeatureLayer featurelayer)
        {
            //检查文件路径是否存在于当前所有标签页的tag中，避免重复创建表格
            IDataLayer datalayer = featurelayer as IDataLayer;
            IWorkspaceName w_name = ((IDatasetName)(datalayer.DataSourceName)).WorkspaceName;

            for (int i = 0; i < att_xtraTabControl.TabPages.Count; i++)
            {
                if (w_name.PathName + "\\" + featurelayer.Name + "_" + featurelayer.DataSourceType == att_xtraTabControl.TabPages[i].Tooltip)
                    return;
            }
            //创建gridcontrol的数据源datatable
            DataTable dt = new DataTable();
            IFeatureClass m_featureclass = featurelayer.FeatureClass;
            if (m_featureclass == null) return;

            for (int i = 0; i < m_featureclass.Fields.FieldCount; i++)
            {
                DataColumn dc = null;
                if (m_featureclass.Fields.get_Field(i).Type.ToString() == "esriFieldTypeOID")
                {
                    // 给主键做标记，生成图表时用
                    dc = new DataColumn("*" + m_featureclass.Fields.get_Field(i).Name);
                }
                else
                {
                    dc = new DataColumn(m_featureclass.Fields.get_Field(i).Name);
                }
                if (m_featureclass.Fields.get_Field(i).Type.ToString() == "esriFieldTypeDouble")
                {
                    dc.DataType = typeof(double);
                }
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

            // 创建标签页tabpage
            att_xtraTabControl.TabPages.Add(featurelayer.FeatureClass.AliasName);
            att_xtraTabControl.TabPages[att_xtraTabControl.TabPages.Count - 1].Tooltip = w_name.PathName + "\\" + featurelayer.Name + "_" + featurelayer.DataSourceType;
            // 创建gridcontrol、gridview
            GridControl att_gridcontrol = new GridControl();
            this.att_xtraTabControl.TabPages[att_xtraTabControl.TabPages.Count - 1].Controls.Add(att_gridcontrol);
            att_gridcontrol.Name = "att_gridcontrol";
            att_gridcontrol.Dock = System.Windows.Forms.DockStyle.Fill;

            DevExpress.XtraGrid.Views.Grid.GridView att_gridview = new DevExpress.XtraGrid.Views.Grid.GridView();
            att_gridview.GridControl = att_gridcontrol;
            att_gridcontrol.MainView = att_gridview;
            att_gridview.OptionsView.ColumnAutoWidth = false;
            att_gridcontrol.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {att_gridview});
            att_gridview.OptionsBehavior.Editable = false;
            att_gridview.OptionsView.ShowAutoFilterRow = true;
            att_gridview.OptionsFind.AlwaysVisible = true;
            att_gridview.OptionsView.ShowFooter = true;
            att_gridview.OptionsSelection.MultiSelect = true;
            att_gridview.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            
            // 在当前的gridview中记录图层路径信息
            att_gridview.Tag = w_name.PathName + "\\" + featurelayer.Name + "_" + featurelayer.DataSourceType;
            att_gridview.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(att_gridview_PopupMenuShowing);
            att_gridview.GridMenuItemClick += new GridMenuItemClickEventHandler(att_gridview_GridMenuItemClick);
            att_gridview.RowClick += new RowClickEventHandler(att_gridview_RowClick);
            att_gridview.RowCellClick += new RowCellClickEventHandler(att_gridview_RowCellClick);

            gridview_list.Add(att_gridview);
            att_gridview.Name = (gridview_list.Count).ToString();
            att_gridcontrol.DataSource = dt;
        }

        /// <summary>
        /// 添加新table来显示新传入矢量图层的属性
        /// </summary>
        /// <param name="vecLayer">矢量图层</param>
        public void appendTable(IFeatureLayer vecLayer)
        {
            importAttribute(vecLayer);
            flayer_list.Add(vecLayer);
            this.att_xtraTabControl.SelectedTabPageIndex = att_xtraTabControl.TabPages.Count - 1;
        }

        /// <summary>
        /// 关闭属性表标签页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xtraTabControl_CloseButtonClick(object sender, EventArgs e)
        {
            XtraTabControl tabControl = sender as XtraTabControl;
            ClosePageButtonEventArgs arg = e as ClosePageButtonEventArgs;
            string indexstring = arg.Page.Tooltip;
            // 删除flayer_list中的图层
            for (int i = 0; i < flayer_list.Count; i++)
            {
                IDataLayer datalayer = flayer_list[i] as IDataLayer;
                IWorkspaceName w_name = ((IDatasetName)(datalayer.DataSourceName)).WorkspaceName;
                if (indexstring == w_name.PathName + "\\" + flayer_list[i].Name + "_" + flayer_list[i].DataSourceType)
                {
                    flayer_list.RemoveAt(i);
                }
            }
            // 删除gridview_list中的表格
            for (int i = 0; i < gridview_list.Count; i++)
            {
                if (indexstring == gridview_list[i].Tag.ToString())
                {
                    gridview_list.RemoveAt(i);
                }
            }
            tabControl.TabPages.Remove(arg.Page as XtraTabPage);
        }

        /// <summary>
        /// 显示表头右键菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void att_gridview_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            //添加“选择当前列”及"Statistics And Chart"
            if (e.MenuType == GridMenuType.Column)
            {
                DXMenuItem m_selecolumn = new DXMenuItem("选择当前列", att_gridview_GridMenuItemClick);
                m_selecolumn.Tag = "selectall/" + e.HitInfo.Column.Name + "/" + ((GridView)sender).Name;

                GridViewColumnMenu menu = e.Menu as GridViewColumnMenu;
                menu.Items.Add(m_selecolumn);
            }
        }

        /// <summary>
        /// 点击表头右键菜单项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void att_gridview_GridMenuItemClick(object sender, EventArgs e)
        {
            DXMenuItem item = sender as DXMenuItem;
            if (item == null) return;
            GridView currentview = new GridView();
            string[] itemstring = item.Tag.ToString().Split('/');
            for (int i = 0; i < gridview_list.Count; i++)
            {
                if (gridview_list[i].Name == itemstring[2])
                {
                    currentview = gridview_list[i];
                    currentview.ClearSelection();
                }
            }
            if (item != null && item.Caption == "选择当前列")
            {
                GridCell start = new GridCell(0, currentview.Columns[itemstring[1].Substring(3)]);
                GridCell end = new GridCell(currentview.RowCount - 1, currentview.Columns[itemstring[1].Substring(3)]);
                currentview.SelectCells(start, end);
            }
        }

        /// <summary>
        /// 选择一行，并在地图中显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void att_gridview_RowClick(object sender, RowClickEventArgs e)
        {
            GridView att_gridview = (GridView)sender;
            GridHitInfo info = att_gridview.CalcHitInfo(e.X, e.Y);
            //只有单击rowinditor时才执行选择行
            if (!info.InRowCell)
            {
                if (info.InRow)
                {
                    //存放selectedrow
                    ArrayList rows = new ArrayList();
                    for (int i = 0; i < att_gridview.SelectedRowsCount; i++)
                    {
                        if (att_gridview.GetSelectedRows()[i] >= 0)
                            rows.Add(att_gridview.GetDataRow(att_gridview.GetSelectedRows()[i]));
                    }
                    if (rows.Count > 0)
                    {
                        //遍历flayer_list寻找当前属性表对应的图层
                        for (int i = 0; i < flayer_list.Count; i++)
                        {
                            IDataLayer datalayer = flayer_list[i] as IDataLayer;
                            IWorkspaceName w_name = ((IDatasetName)(datalayer.DataSourceName)).WorkspaceName;
                            if (att_gridview.Tag.ToString() == w_name.PathName + "\\" + flayer_list[i].Name + "_" + flayer_list[i].DataSourceType)
                            {
                                IFeatureClass m_featureclass = flayer_list[i].FeatureClass;
                                IFeatureSelection m_fselection = flayer_list[i] as IFeatureSelection;
                                
                                //构造查询条件
                                IQueryFilter m_queryfilter = new QueryFilterClass();
                                string key = flayer_list[i].FeatureClass.Fields.get_Field(0).Name;
                                string m_whereclause = key + "=";
                                for (int j = 0; j < rows.Count; j++)
                                {
                                    DataRow m_dr = rows[j] as DataRow;
                                    if (j < 1)
                                        m_whereclause += m_dr[0].ToString();
                                    else
                                        m_whereclause = m_whereclause + " or " + key + "=" + m_dr[0].ToString();
                                }
                                m_queryfilter.WhereClause = m_whereclause;

                                //显示查询的要素
                                m_fselection.SelectFeatures(m_queryfilter, esriSelectionResultEnum.esriSelectionResultNew, false);
                                ISelectionSet m_selectionset = m_fselection.SelectionSet;
                                IEnumGeometry m_enumgeometry = new EnumFeatureGeometry();
                                IEnumGeometryBind m_enumgeobine = m_enumgeometry as IEnumGeometryBind;
                                m_enumgeobine.BindGeometrySource(null, m_selectionset);
                                IGeometryFactory m_geofactory = new GeometryEnvironmentClass();
                                IGeometry m_geometry = m_geofactory.CreateGeometryFromEnumerator(m_enumgeometry);
                                m_mapControl.ActiveView.Extent = m_geometry.Envelope;
                                m_mapControl.Refresh();

                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 单击单元格清空选择集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void att_gridview_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            GridView att_gridview = (GridView)sender;
            //遍历flayer_list寻找当前属性表对应的图层
            for (int i = 0; i < flayer_list.Count; i++)
            {
                IDataLayer datalayer = flayer_list[i] as IDataLayer;
                IWorkspaceName w_name = ((IDatasetName)(datalayer.DataSourceName)).WorkspaceName;
                if (att_gridview.Tag.ToString() == w_name.PathName + "\\" + flayer_list[i].Name + "_" + flayer_list[i].DataSourceType)
                {
                    IFeatureClass m_featureclass = flayer_list[i].FeatureClass;
                    IFeatureSelection m_fselection = flayer_list[i] as IFeatureSelection;
                    m_fselection.Clear();
                    m_mapControl.ActiveView.Refresh();
                }
            }
            //att_gridview.ClearSelection();
        }

        /// <summary>
        /// 根据传入的要素图层删除其对应的属性表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="att_flayer">矢量图层</param>
        public void att_removetable(IFeatureLayer att_flayer)
        {
            //获取传入图层的路径
            IDataLayer datalayer = att_flayer as IDataLayer;
            IWorkspaceName w_name = ((IDatasetName)(datalayer.DataSourceName)).WorkspaceName;
            string indexstring = w_name.PathName + "\\" + att_flayer.Name + "_" + att_flayer.DataSourceType;
            //根据indexstring删除flayer_list中的图层
            for (int i = 0; i < flayer_list.Count; i++)
            {
                IDataLayer listdatalayer = flayer_list[i] as IDataLayer;
                IWorkspaceName listw_name = ((IDatasetName)(datalayer.DataSourceName)).WorkspaceName;
                if (indexstring == w_name.PathName + "\\" + flayer_list[i].Name + "_" + flayer_list[i].DataSourceType)
                {
                    flayer_list.RemoveAt(i);
                }
            }
            //删除gridview_list中的表格
            for (int i = 0; i < gridview_list.Count; i++)
            {
                if (indexstring == gridview_list[i].Tag.ToString())
                {
                    gridview_list.RemoveAt(i);
                }
            }
            //关闭标签页
            for (int i = 0; i < att_xtraTabControl.TabPages.Count; i++)
            {
                if (att_xtraTabControl.TabPages[i].Text == att_flayer.Name)
                {
                    att_xtraTabControl.TabPages.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 打开图表与统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void att_chartbaritem_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            string indexstring = att_xtraTabControl.SelectedTabPage.Tooltip;
            for (int i = 0; i < gridview_list.Count; i++)
            {
                //在gridviewlist中查找当前属性表数据
                if (indexstring == gridview_list[i].Tag.ToString())
                {
                    int formflag = 0;
                    //遍历当前所有已打开的窗口，查找StatisticsAndChartForm，根据数据源路径判断该属性表是否已经生成统计与图表
                    foreach (XtraForm form in Application.OpenForms)
                    {
                        if (form is StatisticsAndChartForm)
                        {
                            StatisticsAndChartForm sc = form as StatisticsAndChartForm;
                            if (sc.gridview.Tag.ToString() == indexstring)
                            {
                                //检测到该属性表已经生成统计与图表，退出
                                formflag = 1;
                                sc.Focus();
                                break;
                            }
                        }
                    }
                    if (formflag == 0)
                    {
                        //未检测到与该属性表对应的统计与图表窗体，生成
                        StatisticsAndChartForm StatiAndChart = new StatisticsAndChartForm(gridview_list[i], att_xtraTabControl.SelectedTabPage.Text);
                        StatiAndChart.Show();
                    }
                    break;
                }
            }
        }

        private void att_xtraTabControl_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            int index = att_xtraTabControl.TabPages.IndexOf(e.Page);
            this.Text = "Attribute Table : "
                + flayer_list[index].FeatureClass.FeatureCount(new ESRI.ArcGIS.Geodatabase.QueryFilter()).ToString() + " features";
        }


    }
}