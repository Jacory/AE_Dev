using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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
using ESRI.ArcGIS.GeoAnalyst;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SpatialAnalyst;
using AE_Dev_J.Class;



namespace AE_Dev_J
{
    public partial class MainForm : XtraForm
    {
        public AxTOCControl getTocControl() { return m_tocControl; }
        public AxMapControl getMapControl() { return m_mapControl; }

        public string drawflag = ""; //用于为clipform绘制矩形框,值不为空时表示mapcontrol处于包络线绘制状态
        public int drawSampleflag = 0; //用于监督分类选取样本，值为1时表示mapcontrol处于polygon绘制状态
        public int trackPolyonState = 0; //指示mapcontrol是否处于trackpolygon状态，0为退出状态，1表示正在绘制，2表示完成绘制但没有退出

        public delegate void CreateSampleEventHander(IGeometry geometry); //声明创建监督分类样本的委托
        public event CreateSampleEventHander CreateSample;//创建监督分类样本事件

        #region 私有成员变量

        // 为了保留单一实例，存储一些对话框引用
        private ClassificationForm m_classForm = null;
        private AboutForm m_abForm = null;
        private TargetDetectionForm m_tdForm = null;
        private RgbSegForm m_rgbSegForm = null;
        private AttributeTableForm m_attForm = null;
        private IEngineEditor pEngineEditor = null;

        private GlobalSettings m_globalSetting = null;

        private bool MouseIsDown = false; //鼠标绘制矩形裁剪包络线时，监测鼠标是否按下
        private Rectangle MouseRect = Rectangle.Empty; //初始化矩形裁剪包络线时

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
            //清空temp文件夹
            foreach (string d in Directory.GetFileSystemEntries(Application.StartupPath + "\\temp"))
            {
                if (File.Exists(d))
                {
                    FileInfo fi = new FileInfo(d);
                    File.Delete(d);//直接删除其中的文件  
                }
            }
  
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;

            m_globalSetting = new GlobalSettings();
        }

        /// <summary>
        /// 加载皮肤
        /// </summary>
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
            ICommand openmxd = new ControlsOpenDocCommandClass();
            openmxd.OnCreate(m_mapControl.Object);
            openmxd.OnClick();
        }

        /// <summary>
        /// 关闭工程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iCloseProject_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_mapControl.DocumentFilename != null)
            {
                IMapDocument pMapDocument = new MapDocumentClass();
                pMapDocument.Open(m_mapControl.DocumentFilename, "");

                for (int i = m_mapControl.Map.LayerCount - 1; i >= 0; i--)
                {
                    m_mapControl.DeleteLayer(i);
                }
                m_mapControl.DocumentFilename = null;
                pMapDocument.Close();
            }
        }

        /// <summary>
        /// 保存工程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iSaveProject_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //判断当前是否存在mxd文件，若存在则执行保存，否则执行另存为。
            if (m_mapControl.DocumentFilename!=null)
            {
                IMxdContents pMxdC = m_mapControl.Map as IMxdContents;
                IMapDocument pMapDocument = new MapDocumentClass();
                pMapDocument.Open(m_mapControl.DocumentFilename, "");
                IActiveView pActiveView = m_mapControl.Map as IActiveView;
                pMapDocument.ReplaceContents(pMxdC);
                IObjectCopy lip_ObjCopy = new ObjectCopyClass(); //使用Copy，避免共享引用
                m_mapControl.Map = (IMap)lip_ObjCopy.Copy(pMapDocument.Map[0]);
                lip_ObjCopy = null;
                pMapDocument.Save(true,false);
                pMapDocument.Close();
                MessageBox.Show("保存成功");
            }
            else
            {
                IMapDocument pMapDocument = new MapDocumentClass();
                SaveFileDialog opensavemxd = new SaveFileDialog();
                opensavemxd.Filter = "地图文档(*.mxd)|*.mxd"; //对话框的过滤器
                if (opensavemxd.ShowDialog() == DialogResult.OK)
                {
                    string filePath = opensavemxd.FileName; //获取文件全路径
                    pMapDocument.New(filePath);
                    IMxdContents pMxdC = m_mapControl.Map as IMxdContents;
                    pMapDocument.ReplaceContents(pMxdC);
                    pMapDocument.Save(true, false);
                    m_mapControl.LoadMxFile(filePath, 0, Type.Missing);
                    //循环遍历所有的地图
                    for (int i = 0; i < pMapDocument.MapCount; i++)
                    {
                        m_mapControl.Map = pMapDocument.get_Map(i); //绑定地图控件
                    }
                    m_mapControl.Map.Name = "Layers";
                    m_tocControl.SetBuddyControl(m_mapControl.Object);
                    pMapDocument.Close();

                }
            }
        }

        /// <summary>
        /// 工程另存为
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iSaveProjectAs_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //ESRI.ArcGIS.SystemUI.ICommand saveCommand = new ControlsSaveAsDocCommand();
            //saveCommand.OnCreate(m_mapControl.Object);
            //saveCommand.OnClick();

            IMapDocument pMapDocument = new MapDocumentClass();
            SaveFileDialog opensavemxd = new SaveFileDialog();
            opensavemxd.Filter = "地图文档(*.mxd)|*.mxd"; //对话框的过滤器
            if (opensavemxd.ShowDialog() == DialogResult.OK)
            {
                string filePath = opensavemxd.FileName; //获取文件全路径
                pMapDocument.New(filePath);
                IMxdContents pMxdC = m_mapControl.Map as IMxdContents;
                pMapDocument.ReplaceContents(pMxdC);
                pMapDocument.Save(true, false);
                m_mapControl.LoadMxFile(filePath, 0, Type.Missing);
                //循环遍历所有的地图
                for (int i = 0; i < pMapDocument.MapCount; i++)
                {
                    m_mapControl.Map = pMapDocument.get_Map(i); //绑定地图控件
                }
                pMapDocument.Close();
            }

        }

        /// <summary>
        /// 建立新工程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iNewProject_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            IMapDocument pMapDocument = new MapDocumentClass();
            SaveFileDialog newmxd = new SaveFileDialog();
            newmxd.Filter = "地图文档(*.mxd)|*.mxd";    
            newmxd.Title = "New Map";
            if (newmxd.ShowDialog() == DialogResult.OK)
            {
                string filePath = newmxd.FileName; 
                pMapDocument.New(filePath);
                m_mapControl.LoadMxFile(filePath, 0, Type.Missing);
                m_mapControl.Map.Name = "Layers";
                m_tocControl.SetBuddyControl(m_mapControl.Object);
            }
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
            if(m_classForm == null || m_classForm.IsDisposed == true)
                m_classForm = new ClassificationForm(this, m_globalSetting.idlPath);
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
        /// 新建要素图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iNewFeature_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AddFeatureClassForm addafeature = new AddFeatureClassForm(this.getMapControl());
            addafeature.ShowDialog();
        }
        
        /// <summary>
        /// 矢量转栅格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iFeatureToRaster_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FeatureToRasterForm featuretoraster = new FeatureToRasterForm(this);
            featuretoraster.Show();
        }
        
        /// <summary>
        /// 栅格转矢量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iRasterToFeature_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            RasterToFeatureForm rastertofeature = new RasterToFeatureForm(this);
            rastertofeature.Show();
        }
        
        /// <summary>
        /// 裁剪图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iClip_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ClipForm clip = new ClipForm(this);
            clip.Show();
        }

        /// <summary>
        /// 调用ROI工具窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iRoiTool_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ROIToolForm roiTool = new ROIToolForm(this);
            roiTool.Show();
        }

        /// <summary>
        /// 打开光谱曲线查看工具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iViewSpectralTool_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ViewSpectralForm specViewer = new ViewSpectralForm(this);
            specViewer.Show();
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
                if (datalayer.DataSourceName.NameString!="")
                {
                    datalayer.Disconnect();
                }
                m_mapControl.Map.DeleteLayer(selectedLayer);
                if (m_attForm != null && selectedLayer is IFeatureLayer)
                    m_attForm.att_removetable(selectedLayer as IFeatureLayer);
            }
            this.Focus();
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
                        tocControl_contextMenuStrip.Show(m_tocControl, new System.Drawing. Point(e.x, e.y));
                        break;
                    case esriTOCControlItem.esriTOCControlItemLayer:    // 点击的是图层
                        tocControlLayer_ContextMenu.Show(m_tocControl, new System.Drawing.Point(e.x, e.y));
                        break;
                    case esriTOCControlItem.esriTOCControlItemHeading:
                        break;
                    default:
                        tocControl_contextMenuStrip.Show(m_tocControl, new System.Drawing.Point(e.x, e.y));
                        break;
                }
            }
        }
        /// <summary>
        /// 添加图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addData_toolStripMenuItem_Click(object sender, EventArgs e)
        {
            ICommand addData = new ControlsAddDataCommandClass();
            addData.OnCreate(m_mapControl.Object);
            m_mapControl.CurrentTool = addData as ITool;
            addData.OnClick();
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
            if (MouseIsDown)
                ResizeToRectangle(e.x + m_tocControl.Size.Width + 20, e.y + ribbonMenu.Size.Height + 35); 
            
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
                case 1:// 鼠标左键
                    if (drawflag!="")
                    {
                        MouseIsDown = true;
                        DrawStart(e.x + m_tocControl.Size.Width + 20, e.y + ribbonMenu.Size.Height + 35);
                        foreach (XtraForm form in Application.OpenForms)//遍历所有窗口，查找对应的clip窗口
                        {
                            if (form is ClipForm)
                            {
                                ClipForm clipform = form as ClipForm;
                                if (clipform.Tag.ToString() == drawflag)//根据窗口tag值，即窗口handle，将坐标传回该clip窗口
                                {
                                    (clipform.getlefttopX()).Text = e.mapX.ToString();
                                    (clipform.getlefttopY()).Text = e.mapY.ToString();
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (drawSampleflag== 1)
                        {
                            trackPolyonState = 1;
                            //产生拖拽多边形
                            IGeometry SampleGeometry = m_mapControl.TrackPolygon();

                            if (SampleGeometry != null)
                            {
                                //触发事件，激活监督分类窗口
                                if (CreateSample != null)
                                {
                                    CreateSample(SampleGeometry);
                                }

                            }
                        }
                    }
                    break;
                case 2:     // 鼠标右键     
                    mapControl_contextMenuStrip.Show(m_mapControl, new System.Drawing. Point(e.x, e.y));
                    break;
                case 3:
                    break;
                case 4:     // 鼠标中键
                    m_mapControl.MousePointer = esriControlsMousePointer.esriPointerPan;
                    m_mapControl.Pan();
                    //判断事件并恢复光标
                    if (drawSampleflag==1)
                    {
                        //之前处于绘制多边形状态，将光标恢复至十字形
                        m_mapControl.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
                    }
                    else
                    {
                        m_mapControl.MousePointer = esriControlsMousePointer.esriPointerArrow;
                    }
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
            switch (e.button)
            {
                case 1://左键
                     m_mapControl.Focus();
                    //执行裁剪，自定义包络线
                    this.Capture = false;
                    System.Windows.Forms.Cursor.Clip = Rectangle.Empty;
                    MouseIsDown = false;
                    DrawRectangle();
                    MouseRect = Rectangle.Empty;
                    //遍历当前所有已打开的窗口，查找clipform
                    if (drawflag!="")
                    {
                        foreach (XtraForm form in Application.OpenForms)//遍历所有窗口，查找对应的clip窗口
                        {
                            if (form is ClipForm)
                            {
                                ClipForm clipform = form as ClipForm;
                                if (clipform.Tag.ToString() == drawflag)//根据窗口tag值，即窗口handle，将坐标传回该clip窗口
                                {
                                    (clipform.getrightbottomX()).Text = e.mapX.ToString();
                                    (clipform.getrightbottomY()).Text = e.mapY.ToString();
                                    clipform.Focus();
                                    clipform.Refresh();
                                    break;
                                }
                            }
                        }
                    }
                    drawflag = "";

                    break;
                case 2://右键
                    break;
                case 3:
                    break;
                case 4://中键
                    break;

            }
        }

        /// <summary>
        /// mapControl中绘制矩形
        /// </summary>
        private void DrawRectangle()
        {
            Rectangle rect = this.RectangleToScreen(MouseRect);
            ControlPaint.DrawReversibleFrame(rect, this.BackColor, FrameStyle.Dashed);
        }

        /// <summary>
        /// mapControl中绘制clip矩形包络线
        /// </summary>
        private void DrawStart(int x, int y)
        {
            System.Windows.Forms.Cursor.Clip = this.RectangleToScreen(new Rectangle(m_tocControl.Size.Width + 19, ribbonMenu.Size.Height + 35, m_mapControl.Size.Width - 5, m_mapControl.Size.Height - 5));//指定鼠标活动区域
            MouseRect = new Rectangle(x, y, 0, 0);
        }

        /// <summary>
        /// mapControl中绘制矩形
        /// </summary>
        private void ResizeToRectangle(int x, int y)
        {
            DrawRectangle();
            MouseRect.Width = x - MouseRect.Left;
            MouseRect.Height = y - MouseRect.Top;
            DrawRectangle();
        }

        /// <summary>
        /// 绘制多边形
        /// </summary>
        /// <param name="pGeom"></param>

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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //try
            //{

                //检查编辑状态
                if (pEngineEditor != null && pEngineEditor.HasEdits() == false)
                {
                    pEngineEditor.StopEditing(false);
                }
                else
                {
                    if (pEngineEditor != null)
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

                //移除图层并解除图层锁
                for (int i = m_mapControl.Map.LayerCount - 1; i >= 0; i--)
                {
                    IDataLayer2 datalayer = m_mapControl.get_Layer(i) as IDataLayer2;
                    if (datalayer.DataSourceName.NameString != "")
                    {
                        datalayer.Disconnect();
                    }
                    m_mapControl.DeleteLayer(i);

                }
            //}
            //catch (Exception err)
            //{

            //    MessageBox.Show(err.Message);
            //}

        }
        
        /// <summary>
        /// 键盘按下ESC键，用于取消绘图状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_mapControl_OnKeyUp(object sender, IMapControlEvents2_OnKeyUpEvent e)
        {
            if (e.keyCode == (char)Keys.Escape)
            {
                if (trackPolyonState==1)//如果正在绘制则将状态改为完成绘制状态
                {
                    trackPolyonState = 2;
                }
                else if (trackPolyonState == 2)//如果为完成绘制状态则改为退出状态
                {
                    drawSampleflag = 0;//解除绘制监督分类样本区域状态
                    m_mapControl.MousePointer = esriControlsMousePointer.esriPointerArrow;//恢复光标
                    trackPolyonState = 0;
                }
            }
        }
    }
}