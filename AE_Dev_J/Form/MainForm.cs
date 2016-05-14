using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using DevExpress.UserSkins;
using DevExpress.XtraEditors;
using DevExpress.XtraBars.Helpers;
using AE_Dev_J.Form;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.CartoUI;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.DataSourcesFile;



namespace AE_Dev_J
{
    public partial class MainForm : XtraForm
    {
        public AxTOCControl getTocControl() { return m_tocControl; }
        public AxMapControl getMapControl() { return m_mapControl; }

        #region 私有成员变量

        // 为了保留单一实例，存储一些对话框引用
        private ClassificationForm m_classForm = null;
        private AboutForm m_abForm = null;
        private TargetDetectionForm m_tdForm = null;
        private RgbSegForm m_rgbSegForm = null;
        private AttributeTableForm m_attForm = null;
        IEngineEditor pEngineEditor =null;

        #endregion 私有成员变量


        /// <summary>
        /// 打开栅格文件.
        /// </summary>
        /// <param name="rasfilename">栅格文件路径</param>
        public void openRasterFile(string rasfilename)
        {
            FileInfo finfo = new FileInfo(rasfilename);

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

            m_mapControl.AddLayer(pLayer);
            m_mapControl.Refresh();
        }

        /// <summary>
        /// 打开矢量文件
        /// </summary>
        /// <param name="vecFilename">矢量文件路径</param>
        public void openVectorFile(string vecFilename)
        {
            FileInfo finfo = new FileInfo(vecFilename);

            if (finfo.Extension == ".shp")
                m_mapControl.AddShapeFile(finfo.DirectoryName, finfo.Name);
        }


        public MainForm()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop); // ESRI license
            InitializeComponent();
            InitSkinGallery();
            //设置地图名称
            m_mapControl.Map.Name = "Layers";
            //创建临时文件夹temp
            DirectoryInfo dir = new DirectoryInfo(Application.StartupPath + "\\temp");
            if (!dir.Exists)
            {
                System.IO.Directory.CreateDirectory(Application.StartupPath + "\\temp");
            }

        }

        void InitSkinGallery()
        {
            SkinHelper.InitSkinGallery(rgbiSkins, true);
        }

        #region Home 菜单事件

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        // ===== Project =====
        /// <summary>
        /// 打开工程文件，*.mxd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iOpenProject_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ESRI.ArcGIS.SystemUI.ICommand openProjectCommand = new ControlsOpenDocCommand();
            openProjectCommand.OnCreate(m_mapControl.Object);
            openProjectCommand.OnClick();
        }

        /// <summary>
        /// 关闭工程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iCloseProject_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        /// <summary>
        /// 保存工程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iSaveProject_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        /// <summary>
        /// 工程另存为
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iSaveProjectAs_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ESRI.ArcGIS.SystemUI.ICommand saveCommand = new ControlsSaveAsDocCommand();
            saveCommand.OnCreate(m_mapControl.Object);
            saveCommand.OnClick();
        }

        /// <summary>
        /// 建立新工程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iNewProject_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        // ====== File ======
        /// <summary>
        /// 添加数据文件, *.shp or image data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iAddData_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ICommand addData = new ControlsAddDataCommandClass();
            addData.OnCreate(m_mapControl.Object);
            m_mapControl.CurrentTool = addData as ITool;
            addData.OnClick();
        }

        #endregion

        #region Process 菜单事件
        /// <summary>
        /// RGB自动分割
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iRgbSeg_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_rgbSegForm == null || m_rgbSegForm.IsDisposed == true)
            {
                m_rgbSegForm = new RgbSegForm(); 
            }
            m_rgbSegForm.Show();
            m_rgbSegForm.Focus();
        }

        /// <summary>
        /// 分类，点击弹出分类面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iClassification_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if(m_classForm == null || m_classForm.IsDisposed==true)
                m_classForm = new ClassificationForm();
            m_classForm.Show();
            m_classForm.Focus();
        }

        /// <summary>
        /// 目标探测，点击弹出目标探测面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iTargetDetection_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_tdForm == null || m_tdForm.IsDisposed == true)
                m_tdForm = new TargetDetectionForm();
            m_tdForm.Show();
            m_tdForm.Focus();
        }

        #endregion

        #region Data Managment 菜单事件

        /// <summary>
        ///New Feature
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iNewFeature_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AddFeatureClassForm addafeature = new AddFeatureClassForm(this.getMapControl());
            addafeature.ShowDialog();
        }
        #endregion Data Managment 菜单事件

        #region Home and Skin 菜单事件

        /// <summary>
        /// “关于”对话框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iAbout_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_abForm == null || m_abForm.IsDisposed == true)
                m_abForm = new AboutForm();
            m_abForm.Show();
            m_abForm.Focus();
        }

        #endregion Home and Skin 菜单事件

        #region ToolBar 工具条事件

        /// <summary>
        /// 矢量“编辑”工具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_edittool_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pEngineEditor.EditState==esriEngineEditState.esriEngineStateEditing)
            {
                ICommand t_editcommand = new ESRI.ArcGIS.Controls.ControlsEditingEditToolClass();
                t_editcommand.OnCreate(m_mapControl.Object);
                m_mapControl.CurrentTool = t_editcommand as ITool;
                t_editcommand.OnClick();
            }
        }

        /// <summary>
        /// 矢量“草图”工具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_sketchtool_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                ICommand t_sketchcommand = new ControlsEditingSketchToolClass();
                t_sketchcommand.OnCreate(m_mapControl.Object);
                m_mapControl.CurrentTool = t_sketchcommand as ITool;
                t_sketchcommand.OnClick();
            }
        }

        /// <summary>
        /// 停止编辑所选图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_stoptool_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pEngineEditor != null && pEngineEditor.HasEdits() == false)
            {
                pEngineEditor.StopEditing(false);
            }
            else
            {
                if (MessageBox.Show("Save Edits?", "Save Prompt", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    pEngineEditor.StopEditing(true);
                }
                else
                {
                    pEngineEditor.StopEditing(false);
                }
            }
            //恢复光标
            ICommand t_editcommand = new ESRI.ArcGIS.Controls.ControlsEditingEditToolClass();
            t_editcommand.OnCreate(m_mapControl.Object);
            m_mapControl.CurrentTool = t_editcommand as ITool;
            t_editcommand.OnClick();

            m_editinglayer.Caption = "当前图层：";
            map_edittools.Visible = false;
            
        }

        /// <summary>
        /// 保存编辑内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_savetool_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                ICommand savecommand = new ControlsEditingSaveCommandClass();
                savecommand.OnCreate(m_mapControl.Object);
                m_mapControl.CurrentTool = savecommand as ITool;
                savecommand.OnClick();
            }
        }

        /// <summary>
        /// 撤销操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_undotool_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pEngineEditor != null && pEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                m_redotool.Enabled = true;
                IWorkspaceEdit workspaceedit = (IWorkspaceEdit)pEngineEditor.EditWorkspace;

                bool bHasUndo = true;
                workspaceedit.HasUndos(ref bHasUndo);

                if (bHasUndo)
                {
                    workspaceedit.UndoEditOperation();
                    ((IActiveView)m_mapControl.Map).Refresh();
                }
            }
        }

        /// <summary>
        /// 恢复操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_redotool_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                m_redotool.Enabled = true;
                IWorkspaceEdit workspaceedit = (IWorkspaceEdit)pEngineEditor.EditWorkspace;

                bool bHasRedo = true;
                workspaceedit.HasRedos(ref bHasRedo);

                if (bHasRedo)
                {
                    workspaceedit.RedoEditOperation();
                    ((IActiveView)m_mapControl.Map).Refresh();
                }
            }
        }

        #endregion ToolBar 工具条事件

        #region m_tocControl右键菜单项

        /// <summary>
        /// 打开属性表右键菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openAttTable_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IBasicMap map = null;
            ILayer layer = null;
            object unk = null;
            object data = null;
            esriTOCControlItem item = esriTOCControlItem.esriTOCControlItemNone;
            m_tocControl.GetSelectedItem(ref item, ref map, ref layer, ref unk, ref data);

            IFeatureLayer selectedLayer = layer as IFeatureLayer;
            if (item == esriTOCControlItem.esriTOCControlItemLayer && selectedLayer != null)
            {

                if (selectedLayer is IFeatureLayer)
                {   // 打开属性表窗口，如果当前没有属性表，就创建一个，如果当前有，就在原有窗口中添加一张表格
                    if (m_attForm == null || m_attForm.IsDisposed == true)
                        m_attForm = new AttributeTableForm(selectedLayer, this.getMapControl());
                    else
                        m_attForm.appendTable(selectedLayer);
                    m_attForm.Show();
                    m_attForm.Focus();
                }
            }
        }

        /// <summary>
        /// 移除图层右键菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeLayer_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IBasicMap map = null;
            ILayer selectedLayer = null;
            object unk = null;
            object data = null;
            esriTOCControlItem item = esriTOCControlItem.esriTOCControlItemNone;
            m_tocControl.GetSelectedItem(ref item, ref map, ref selectedLayer, ref unk, ref data);

            if (item == esriTOCControlItem.esriTOCControlItemLayer)
            {
                IDataLayer2 datalayer = selectedLayer as IDataLayer2;
                datalayer.Disconnect();
                m_mapControl.Map.DeleteLayer(selectedLayer);
                if (m_attForm != null && selectedLayer is IFeatureLayer)
                    m_attForm.att_removetable(selectedLayer as IFeatureLayer);
            }
            m_mapControl.Focus();
        }

        /// <summary>
        /// 缩放至图层所在范围右键菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zoomToLayer_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IBasicMap map = null;
            ILayer selectedLayer = null;
            object unk = null;
            object data = null;
            esriTOCControlItem item = esriTOCControlItem.esriTOCControlItemNone;
            m_tocControl.GetSelectedItem(ref item, ref map, ref selectedLayer, ref unk, ref data);

            if (item == esriTOCControlItem.esriTOCControlItemLayer)
            {
                m_mapControl.ActiveView.Extent = selectedLayer.AreaOfInterest;
                m_mapControl.ActiveView.Refresh();
            }
        }

        /// <summary>
        /// 编辑所选图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editLayer_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pEngineEditor = new EngineEditorClass();
            IBasicMap map = null;
            ILayer selectedLayer = null;
            object unk = null;
            object data = null;
            esriTOCControlItem item = esriTOCControlItem.esriTOCControlItemNone;
            m_tocControl.GetSelectedItem(ref item, ref map, ref selectedLayer, ref unk, ref data);

            if (item == esriTOCControlItem.esriTOCControlItemLayer)
            {
                //启动编辑
                if (pEngineEditor.EditState!=esriEngineEditState.esriEngineStateNotEditing)
                {
                    return;
                }
                IFeatureLayer featurelayer = selectedLayer as IFeatureLayer;
                IDataset dataset = featurelayer.FeatureClass as IDataset;
                IWorkspace workspace = dataset.Workspace;

                pEngineEditor.StartEditing(workspace,m_mapControl.Map);
                ((IEngineEditLayers)pEngineEditor).SetTargetLayer(featurelayer,-1);

                pEngineEditor.StartOperation();

                //设置目标图层
                IEngineEditLayers pEditLayer = pEngineEditor as IEngineEditLayers;
                pEditLayer.SetTargetLayer(featurelayer,0);
                m_editinglayer.Caption += " "+featurelayer.Name;

                ICommand t_editcommand = new ESRI.ArcGIS.Controls.ControlsEditingEditToolClass();
                t_editcommand.OnCreate(m_mapControl.Object);
                m_mapControl.CurrentTool = t_editcommand as ITool;
                t_editcommand.OnClick();

                map_edittools.Visible = true;
            }
        }

        /// <summary>
        /// 当右键菜单弹出时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tocControlLayer_ContextMenu_Opened(object sender, EventArgs e)
        {
            //判断所选图层类型 
            IBasicMap map = null;
            ILayer selectedLayer = null;
            object unk = null;
            object data = null;
            esriTOCControlItem item = esriTOCControlItem.esriTOCControlItemNone;
            m_tocControl.GetSelectedItem(ref item, ref map, ref selectedLayer, ref unk, ref data);

            if (item == esriTOCControlItem.esriTOCControlItemLayer)
            {
                //只有矢量图层“打开属性表”“编辑图层”才可用
                if (selectedLayer is IFeatureLayer )
                {
                    tocControlLayer_ContextMenu.Items["openAttTable_ToolStripMenuItem"].Enabled = true;
                    //只有处于非编辑状态时“编辑图层”才可用
                    if (pEngineEditor == null)
                    {
                        tocControlLayer_ContextMenu.Items["editLayer_ToolStripMenuItem"].Enabled = true;
                    }
                    else
                    {
                        if (pEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
                        {
                            tocControlLayer_ContextMenu.Items["editLayer_ToolStripMenuItem"].Enabled = false;
                        }
                        else
                        {
                            tocControlLayer_ContextMenu.Items["editLayer_ToolStripMenuItem"].Enabled = true;
                        }
                    }
                }
                else
                {
                    tocControlLayer_ContextMenu.Items["editLayer_ToolStripMenuItem"].Enabled= false;
                    tocControlLayer_ContextMenu.Items["openAttTable_ToolStripMenuItem"].Enabled = false;
                }
            }
        }

        #endregion

        #region m_tocControl鼠标事件

        /// <summary>
        /// tocControl鼠标按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_tocControl_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
            esriTOCControlItem item = esriTOCControlItem.esriTOCControlItemNone;
            IBasicMap map = null;
            ILayer layer = null;
            object index = null;
            object other = null;
            m_tocControl.HitTest(e.x, e.y, ref item, ref map, ref layer, ref other, ref index);

            if (e.button == 2) // 鼠标右键
            {
                switch (item)
                {
                    case esriTOCControlItem.esriTOCControlItemMap:  // 点击的是地图
                        tocControl_contextMenuStrip.Show(m_tocControl, new Point(e.x, e.y));
                        break;
                    case esriTOCControlItem.esriTOCControlItemLayer:    // 点击的是图层
                        tocControlLayer_ContextMenu.Show(m_tocControl, new Point(e.x, e.y));
                        break;
                    case esriTOCControlItem.esriTOCControlItemHeading:
                        break;
                    default:
                        tocControl_contextMenuStrip.Show(m_tocControl, new Point(e.x, e.y));
                        break;
                }
            }
        }

        #endregion m_tocControl鼠标事件

        #region m_mapControl右键菜单项
        /// <summary>
        /// 识别
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void indentify_ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 漫游
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pan_ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 放大
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zoomIn_ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 缩小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zoomOut_ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 全局显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fullExtent_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_mapControl.Extent = m_mapControl.FullExtent;
        }

        #endregion

        #region m_mapControl鼠标事件
        /// <summary>
        /// mapControl鼠标移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_mapControl_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            if (m_mapControl.Map.LayerCount == 0) return;

            // 当前鼠标指针坐标显示
            double x = double.Parse(e.mapX.ToString("0.000"));
            double y = double.Parse(e.mapY.ToString("0.000"));
            this.coordinate_textEdit.EditValue = x.ToString() + ", " + y.ToString();
            
        }

        /// <summary>
        /// mapControl鼠标按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_mapControl_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            switch (e.button)
            {
                case 1:     // 鼠标左键
                    break;
                case 2:     // 鼠标右键     
                    mapControl_contextMenuStrip.Show(m_mapControl, new Point(e.x, e.y));
                    break;
                case 3:
                    break;
                case 4:     // 鼠标中键
                    m_mapControl.MousePointer = esriControlsMousePointer.esriPointerPan;
                    m_mapControl.Pan();
                    break;
            }
        }

        /// <summary>
        /// mapControl鼠标放开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_mapControl_OnMouseUp(object sender, IMapControlEvents2_OnMouseUpEvent e)
        {
            m_mapControl.Focus();
        }


        #endregion m_mapControl鼠标事件

        /// <summary>
        /// 要素识别
        /// </summary>
        /// <param name="activeView"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void doIdentify(IActiveView activeView, Int32 x, Int32 y)
        {
            IMap map = activeView.FocusMap;
            IdentifyDialog idenfityDialog = new IdentifyDialog();
            idenfityDialog.Map = map;

            // clear the dialog on each mouse click
            idenfityDialog.ClearLayers();
            IScreenDisplay screenDisplay = activeView.ScreenDisplay;

            IDisplay display = screenDisplay; // implicit cast
            idenfityDialog.Display = display;

            IIdentifyDialogProps idenfityDialogProps = (IIdentifyDialogProps)idenfityDialog; // explicit cast
            IEnumLayer enumLayer = map.Layers;
            enumLayer.Reset();
            ILayer layer = enumLayer.Next();
            while (layer != null)
            {
                idenfityDialog.AddLayerIdentifyPoint(layer, x, y);
                layer = enumLayer.Next();
            }
            idenfityDialog.Show();
        }

        /// <summary>
        /// 关闭主窗口
        /// </summary>
        /// <param name="rasfilename">栅格文件名</param>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //检查编辑状态
            if (pEngineEditor != null && pEngineEditor.HasEdits() == false)
            {
                pEngineEditor.StopEditing(false);
            }
            else
            {
                if (pEngineEditor!=null)
                {
                    if (MessageBox.Show("Save Edits?", "Save Prompt", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        pEngineEditor.StopEditing(true);
                    }
                    else
                    {
                        pEngineEditor.StopEditing(false);
                    }
                }
            }
            //恢复光标
            ICommand t_editcommand = new ESRI.ArcGIS.Controls.ControlsEditingEditToolClass();
            t_editcommand.OnCreate(m_mapControl.Object);
            m_mapControl.CurrentTool = t_editcommand as ITool;
            t_editcommand.OnClick();

            m_editinglayer.Caption = "当前图层：";
            map_edittools.Visible = false;

            //解除图层锁
            for (int i = 0; i < m_mapControl.LayerCount; i++)
            {
                IDataLayer2 datalayer = m_mapControl.get_Layer(i) as IDataLayer2;
                datalayer.Disconnect();
            }
            //清空temp文件夹
            foreach (string d in Directory.GetFileSystemEntries(Application.StartupPath+"\\temp"))
            {
                if (File.Exists(d))
                {
                    FileInfo fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(d);//直接删除其中的文件  
                }
            }
        }

    }
}