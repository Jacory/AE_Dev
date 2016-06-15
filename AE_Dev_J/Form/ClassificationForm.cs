using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Threading;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using AE_Dev_J.Form.ClassifyAlgForm;

using AE_Dev_J.Class;

namespace AE_Dev_J.Form
{
    public partial class ClassificationForm : DevExpress.XtraEditors.XtraForm
    {
        private string m_idlPath = ""; // IDL的pro文件路径
        static string m_proFileFullPath = "";

        private MainForm m_mainForm = null;

        private string m_inDataPath = ""; // 输入文件路径，若是批处理模式，则为文件夹路径
        private string m_outDataPath = ""; // 输出文件路径，若是批处理模式，则为文件夹路径

        private ClassifyAlgBase.ClassifyMethod m_selectedMethod = ClassifyAlgBase.ClassifyMethod.None; // 指定用户选择的算法
        private bool m_processIsDone = false; // 指定算法的是否执行完毕

        private string m_outfilename = "";

        static string m_runStr = "";

        #region 保留分类参数设置面板窗口引用
        private List<DevExpress.XtraEditors.XtraForm> m_formList = null;

        private ParallelepipedClassify parallepipedForm = null;
        private MinimumDistanceClassify miniDisForm = null;
        private MaximumLikelihoodClassify maxLikeForm = null;
        private MahlanobisDistanceClassify mahForm = null;
        private SamClassify samForm = null;
        private SidClassify sidForm = null;
        private BinaryEncodingClassify binEncodForm = null;
        private ANNClassify annForm = null;
        private SvmClassify svmForm = null;
        private IsoDataClassify isodataForm = null;
        private KmeansClassify kmeansForm = null;

        #endregion 保留分类参数设置面板窗口引用



        public ClassificationForm()
        {
            InitializeComponent();
        }

        public ClassificationForm(MainForm mf, string idlPath)
        {
            m_idlPath = idlPath;
            m_mainForm = mf;

            InitializeComponent();
        }

        /// <summary>
        /// 初始化面板中的控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClassificationForm_Load(object sender, EventArgs e)
        {
            this.setParam_TabItem.Enabled = false;
            this.expData_TabItem.Enabled = false;
            this.run_TabItem.Enabled = false;
            this.finish_TabItem.Enabled = false;

            this.tabPageControl_windowsUIButtonPanel.Buttons[0].Properties.Enabled = false;

            this.selectMethod_TabItem.Selected = true;

            this.expToMap_btn.Enabled = false;
            this.closeWindow_btn.Enabled = false;

            // 加载已打开栅格列表
            for (int i = 0; i < m_mainForm.getMapControl().LayerCount; i++)
            {
                ILayer layer = m_mainForm.getMapControl().get_Layer(i);
                if (layer is IRasterLayer)
                {
                    IDataLayer datalayer = layer as IDataLayer;
                    IWorkspaceName w_name = ((IDatasetName)(datalayer.DataSourceName)).WorkspaceName;
                    if (w_name.PathName.LastIndexOf("\\") == w_name.PathName.Length - 1)
                    {
                        inDataFile_btn.Properties.Items.Add(w_name.PathName + layer.Name);
                    }
                    else
                    {
                        inDataFile_btn.Properties.Items.Add(w_name.PathName + "\\" + layer.Name);
                    }
                }
            }

            // 动态加载ROI工具窗口
            ROIToolForm tt = new ROIToolForm(m_mainForm);
            tt.TopLevel = false;
            tt.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            tt.Parent = this.roi_panelControl;
            tt.Dock = DockStyle.Fill;
            tt.Show();

            m_formList = new List<XtraForm>();
        }

        /// <summary>
        /// 用于控制面板的翻页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabPageControl_windowsUIButtonPanel_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            DevExpress.XtraEditors.ButtonPanel.IBaseButton preBtn = this.tabPageControl_windowsUIButtonPanel.Buttons[0];
            DevExpress.XtraEditors.ButtonPanel.IBaseButton nextBtn = this.tabPageControl_windowsUIButtonPanel.Buttons[1];

            if (e.Button == preBtn &&
                this.classfication_backstageViewControl.SelectedTabIndex != 0)
            {
                this.classfication_backstageViewControl.SelectedTabIndex -= 1;
            }
            else if (e.Button == nextBtn)
            {
                int currentTabIndex = this.classfication_backstageViewControl.SelectedTabIndex;

                switch (currentTabIndex) 
                {
                    case 0: // Select Method面板
                        if (supervise_checkEdit.Checked == false && unsupervise_checkEdit.Checked == false)
                        {
                            MessageBox.Show("Please select a method.");
                            return;
                        }
                        else
                        {
                            if (supervise_checkEdit.Checked == true)
                                this.m_selectedMethod = (ClassifyAlgBase.ClassifyMethod)(superviseMethod_radioGroup.SelectedIndex + 1);
                            else if (unsupervise_checkEdit.Checked == true)
                                this.m_selectedMethod = (ClassifyAlgBase.ClassifyMethod)(unsuperviseMethod_radioGroup.SelectedIndex + 1 + 9);

                            if (preBtn.Properties.Enabled == false)
                                preBtn.Properties.Enabled = true;

                            initClassParamSettingPage(m_selectedMethod);
                            turnNextTabPage();
                        }
                        break;

                    case 1: // Set Parameters面板
                        turnNextTabPage();
                        break;

                    case 2: // Export Data面板
                        string inputPath = "";
                        string outputPath = "";
                        if (singleMode_checkEdit.Checked == true)
                        {
                            if (inDataFile_btn.Text == "")
                            { MessageBox.Show("Please enter the input file."); return; }
                            if (outDataFile_btn.Text == "")
                            { MessageBox.Show("Please enter the output file."); return; }

                            inputPath = this.inDataFile_btn.Text;
                            outputPath = this.outDataFile_btn.Text;
                        }
                        else if (batchMode_checkEdit.Checked == true)
                        {
                            if (inDataFolder_btn.Text == "")
                            { MessageBox.Show("Please enter the input file folder."); return; }
                            if (outDataFolder_btn.Text == "")
                            { MessageBox.Show("Please enter the output file folder."); return; }

                            inputPath = inDataFolder_btn.Text;
                            outputPath = outDataFolder_btn.Text;
                        }
                            // 设置Run面板中的初始化参数
                        this.class_method_textEdit.Text = ClassifyAlgBase.getMethodString(this.m_selectedMethod);
                        this.class_inputfile_textEdit.Text = inputPath;
                        this.class_outputfile_textEdit.Text = outputPath;

                        turnNextTabPage();
  
                        break;

                    case 3: // Run面板
                        if (m_processIsDone == false)
                            return;

                        turnNextTabPage();
                        break;

                    case 4: // Finish面板

                        break;

                    default:
                        break;
                }
                
                
            }
        }

        #region Select Method 面板界面逻辑

        /// <summary>
        /// choose supervised method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void supervise_checkEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (supervise_checkEdit.Checked == true)
            {
                this.superviseMethod_radioGroup.Enabled = true;
                this.unsupervise_checkEdit.Checked = false;

                // 设置非监督分类的参数配置界面不可见
                this.paramSetting_xtraTabControl.TabPages[1].PageVisible = false;
                this.paramSetting_xtraTabControl.TabPages[0].PageVisible = true;
            }
            else { this.superviseMethod_radioGroup.Enabled = false; }
        }

        /// <summary>
        /// choose unsupervised method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void unsupervise_checkEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (unsupervise_checkEdit.Checked == true)
            {
                this.unsuperviseMethod_radioGroup.Enabled = true;
                this.supervise_checkEdit.Checked = false;

                // 设置监督分类的参数配置界面不可见
                this.paramSetting_xtraTabControl.TabPages[0].PageVisible = false;
                this.paramSetting_xtraTabControl.TabPages[1].PageVisible = true;
            }
            else { this.unsuperviseMethod_radioGroup.Enabled = false; }
        }

        /// <summary>
        /// 分类方法选择事件，控制监督分类参数配置面板显示的方法参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void superviseMethod_radioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = this.superviseMethod_radioGroup.SelectedIndex;
            switch(index)
            {
                case 0:
                    m_selectedMethod = ClassifyAlgBase.ClassifyMethod.Parallelepiped;
                    break;
                case 1:
                    m_selectedMethod = ClassifyAlgBase.ClassifyMethod.MinimumDistance;
                    break;
                case 2:
                    m_selectedMethod = ClassifyAlgBase.ClassifyMethod.MahalanobisDistance;
                    break;
                case 3:
                    m_selectedMethod = ClassifyAlgBase.ClassifyMethod.MaximumLikelihood;
                    break;
                case 4:
                    m_selectedMethod = ClassifyAlgBase.ClassifyMethod.MaximumLikelihood;
                    break;
                case 5:
                    m_selectedMethod = ClassifyAlgBase.ClassifyMethod.SpectralAngleMapper;
                    break;
                case 6:
                    m_selectedMethod = ClassifyAlgBase.ClassifyMethod.SpectralInformationDivergence;
                    break;
                case 7:
                    m_selectedMethod = ClassifyAlgBase.ClassifyMethod.BinaryEncoding;
                    break;
                case 8:
                    m_selectedMethod = ClassifyAlgBase.ClassifyMethod.NeuralNet;
                    break;
                case 9:
                    m_selectedMethod = ClassifyAlgBase.ClassifyMethod.SupportVectrorMachine;
                    break;
            }
        }

        /// <summary>
        /// 分类方法选择事件，控制非监督分类参数配置面板显示的方法参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void unsuperviseMethod_radioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = this.unsuperviseMethod_radioGroup.SelectedIndex;
            switch (index)
            {
                case 0:
                    m_selectedMethod = ClassifyAlgBase.ClassifyMethod.IsoData;
                    break;
                case 1:
                    m_selectedMethod = ClassifyAlgBase.ClassifyMethod.KMeans;
                    break;
            }
        }

        /// <summary>
        /// 用于设置仅显示指定index位置的tab页面
        /// </summary>
        /// <param name="index">要显示页面的index</param>
        /// <param name="tabControl">Tab Control</param>
        private void showOnlyIndexTabPage(int index, DevExpress.XtraTab.XtraTabControl tabControl)
        {
            if (index < 0 || index >= tabControl.TabPages.Count) return;

            for (int i = 0; i < tabControl.TabPages.Count; i++)
            {
                tabControl.TabPages[i].PageVisible = false;
            }
            tabControl.TabPages[index].PageVisible = true;
        }

        #endregion Select Method 面板界面逻辑

        #region set parameters 面板界面逻辑

        /// <summary>
        /// 动态加载所选择的分类方法参数设置窗口
        /// </summary>
        /// <param name="classifyMethod"></param>
        private void initClassParamSettingPage(ClassifyAlgBase.ClassifyMethod classifyMethod)
        {
            disposeAllClassifyForm();
            switch (classifyMethod)
            {
                case ClassifyAlgBase.ClassifyMethod.Parallelepiped:
                    parallepipedForm = new ParallelepipedClassify();
                    m_formList.Add(parallepipedForm);
                    buildFormIntoParent(parallepipedForm, this.supervise_splitContainerControl.Panel2);
                    break;
                case ClassifyAlgBase.ClassifyMethod.MinimumDistance:
                    miniDisForm = new MinimumDistanceClassify();
                    m_formList.Add(miniDisForm);
                    buildFormIntoParent(miniDisForm, this.supervise_splitContainerControl.Panel2);
                    break;
                case ClassifyAlgBase.ClassifyMethod.MahalanobisDistance:
                    mahForm = new MahlanobisDistanceClassify();
                    m_formList.Add(mahForm);
                    buildFormIntoParent(mahForm, this.supervise_splitContainerControl.Panel2);
                    break;
                case ClassifyAlgBase.ClassifyMethod.MaximumLikelihood:
                    maxLikeForm = new MaximumLikelihoodClassify();
                    m_formList.Add(maxLikeForm);
                    buildFormIntoParent(maxLikeForm, this.supervise_splitContainerControl.Panel2);
                    break;
                case ClassifyAlgBase.ClassifyMethod.SpectralAngleMapper:
                    samForm = new SamClassify();
                    m_formList.Add(samForm);
                    buildFormIntoParent(samForm, this.supervise_splitContainerControl.Panel2);
                    break;
                case ClassifyAlgBase.ClassifyMethod.SpectralInformationDivergence:
                    sidForm = new SidClassify();
                    m_formList.Add(sidForm);
                    buildFormIntoParent(sidForm, this.supervise_splitContainerControl.Panel2);
                    break;
                case ClassifyAlgBase.ClassifyMethod.BinaryEncoding:
                    binEncodForm = new BinaryEncodingClassify();
                    m_formList.Add(binEncodForm);
                    buildFormIntoParent(binEncodForm, this.supervise_splitContainerControl.Panel2);
                    break;
                case ClassifyAlgBase.ClassifyMethod.NeuralNet:
                    annForm = new ANNClassify();
                    m_formList.Add(annForm);
                    buildFormIntoParent(annForm, this.supervise_splitContainerControl.Panel2);
                    break;
                case ClassifyAlgBase.ClassifyMethod.SupportVectrorMachine:
                    svmForm = new SvmClassify();
                    m_formList.Add(svmForm);
                    buildFormIntoParent(svmForm, this.supervise_splitContainerControl.Panel2);
                    break;
                case ClassifyAlgBase.ClassifyMethod.IsoData:
                    isodataForm = new IsoDataClassify();
                    m_formList.Add(isodataForm);
                    buildFormIntoParent(isodataForm, this.unsupervise_xtraTabPage);
                    break;
                case ClassifyAlgBase.ClassifyMethod.KMeans:
                    kmeansForm = new KmeansClassify();
                    m_formList.Add(kmeansForm);
                    buildFormIntoParent(kmeansForm, this.unsupervise_xtraTabPage);
                    break;
                case ClassifyAlgBase.ClassifyMethod.None:

                default:
                    break;
            }
        }

        #endregion set parameters 面板界面逻辑

        #region Export Data面板界面逻辑

        private void singleMode_checkEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (this.singleMode_checkEdit.Checked == true)
            {
                this.singleMode_groupControl.Enabled = true;
                this.batchMode_checkEdit.Checked = false;
            }
            else
                this.singleMode_groupControl.Enabled = false;
        }

        private void batchMode_checkEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (this.batchMode_checkEdit.Checked == true)
            {
                this.batchMode_groupControl.Enabled = true;
                this.singleMode_checkEdit.Checked = false;
            }
            else
                this.batchMode_groupControl.Enabled = false;
        }

        private void inDataFile_btn_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                OpenFileDialog openDialog = new OpenFileDialog();
                openDialog.Filter = "image files(*.img)|*.img";
                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    this.inDataFile_btn.Text = openDialog.FileName;
                    this.m_inDataPath = this.inDataFile_btn.Text;
                }
            }
        }

        /// <summary>
        /// 浏览输出文件路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void outDataFile_btn_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "image files(*.img)|*.img";
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                this.outDataFile_btn.Text = saveDialog.FileName;
                this.m_inDataPath = this.outDataFile_btn.Text;
            }
        }

        private void inDataFolder_btn_Properties__Click(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                this.inDataFolder_btn.Text = folderDialog.SelectedPath;
                this.m_inDataPath = this.inDataFolder_btn.Text;
            }
        }

        private void outDataFolder_btn_Properties__Click(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                this.outDataFolder_btn.Text = folderDialog.SelectedPath;
                this.m_outDataPath = this.outDataFolder_btn.Text;
            }
        }

        

        #endregion Export Data面板界面逻辑

        #region run 面板界面逻辑

        /// <summary>
        /// 运行分类算法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ok_btn_Click(object sender, EventArgs e)
        {
            if (m_selectedMethod == ClassifyAlgBase.ClassifyMethod.None) return;

            this.class_marqueeProgressBarControl.Show(); // 显示进度条
            
            // 判断文件处理模式
            int mode = 0;
            if (singleMode_checkEdit.Checked == true) mode = 0;
            else if (batchMode_checkEdit.Checked == true) mode = 1;

            // 设置对应的IDL命令字符串
            string proFilename = null;
            switch (m_selectedMethod)
            {
                case ClassifyAlgBase.ClassifyMethod.Parallelepiped:
                    proFilename = "parallelepiped_classify.pro";
                    break;

                case ClassifyAlgBase.ClassifyMethod.MinimumDistance:
                    proFilename = "minimumdistance_classify.pro";
                    break;

                case ClassifyAlgBase.ClassifyMethod.MahalanobisDistance:
                    proFilename = "mahalanobis_classify.pro";
                    break;

                case ClassifyAlgBase.ClassifyMethod.MaximumLikelihood:
                    proFilename = "maximumlikelihood_classify.pro";
                    break;

                case ClassifyAlgBase.ClassifyMethod.SpectralAngleMapper:
                    proFilename = "SAM_classify.pro";
                    break;

                case ClassifyAlgBase.ClassifyMethod.SpectralInformationDivergence:
                    proFilename = "SIM_classify.pro";
                    break;

                case ClassifyAlgBase.ClassifyMethod.BinaryEncoding:
                    proFilename = "BinaryEncoding_classify.pro";
                    break;

                case ClassifyAlgBase.ClassifyMethod.NeuralNet:
                    proFilename = "ANN_classify.pro";
                    break;

                case ClassifyAlgBase.ClassifyMethod.SupportVectrorMachine:
                    proFilename = "svm_classify.pro";
                    break;

                case ClassifyAlgBase.ClassifyMethod.IsoData:
                    proFilename = "isodata.pro";
                    //m_runStr =  "isodata, '" + inDataFile_btn.Text + "','"
                    //            + outDataFile_btn.Text + "',"
                    //            + isodata_maxIter_spinEdit.Value + ","
                    //            + isodata_chgThresh_spinEdit.Value + ","
                    //            + isodata_minDis_spinEdit.Value + ","
                    //            + isodata_maxMergePixel_spinEdit.Value + ","
                    //            + isodata_minClassPixels_spinEdit.Value + ","
                    //            + isodata_maxStd_spinEdit.Value + ","
                    //            + isodata_minClasses_spinEdit.Value + ","
                    //            + mode;
                    break;

                case ClassifyAlgBase.ClassifyMethod.KMeans:
                    proFilename = "k_means.pro";
                    //m_runStr = "k_means , '" + inDataFile_btn.Text + "','"
                    //            + outDataFile_btn.Text + "',"
                    //            + kmeans_numClasses_spinEdit.Value + ","
                    //            + kmeans_maxIter_spinEdit.Value + ","
                    //            + kmeans_changeThresh_spinEdit.Value
                    //            + mode;
                    break;

                default:
                    break;
            }

            // 初始化 IDLConnector, 并在新线程中运行分类算法
            string proFullPath = m_idlPath + proFilename;
            System.IO.FileInfo proFileInfo = new System.IO.FileInfo(proFullPath);
            m_proFileFullPath = proFileInfo.FullName;
            if (proFileInfo.Exists == true && m_runStr != null)
            {
                try
                {
                    this.tabPageControl_windowsUIButtonPanel.Enabled = false;
                    class_backgroundWorker.RunWorkerAsync();

                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString(), "Error");
                }
            }
        }

        /// <summary>
        /// 取消分类窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancel_Btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion run 面板界面逻辑

        /// <summary>
        /// 使分类面板的tabControl向下翻页
        /// </summary>
        private void turnNextTabPage()
        {
            this.classfication_backstageViewControl.SelectedTabIndex += 1;
            if (this.classfication_backstageViewControl.SelectedTab != null)
                this.classfication_backstageViewControl.SelectedTab.Enabled = true;
        }

        /// <summary>
        ///// 执行分类处理，这样封装方便在多线程中进行
        /// </summary>
        /// <param name="idlCon"></param>
        private static void runClassify(object sender, DoWorkEventArgs e)
        {
            IdlConnector idlCon = new IdlConnector(m_proFileFullPath);
            idlCon.RunStr = m_runStr;
            idlCon.run();
        }

        /// <summary>
        /// 新线程处理任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void class_backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            IdlConnector idlCon = new IdlConnector(m_proFileFullPath);
            idlCon.RunStr = m_runStr;
            idlCon.run();
        }

        /// <summary>
        /// 新线程任务完成后执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void class_backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.expToMap_btn.Enabled = true;
            this.closeWindow_btn.Enabled = true;

            // 显示执行完成界面
            this.finish_TabItem.Enabled = true;
            this.classfication_backstageViewControl.SelectedTab = this.finish_TabItem;

            if (singleMode_checkEdit.Checked == true) m_outfilename = outDataFile_btn.Text;
        }

        /// <summary>
        /// 导出分类结果到主窗口地图
        /// </summary>
        /// <param name="sender">分类结果文件路径。如果是批处理模式，这个功能将不可用</param>
        /// <param name="e"></param>
        private void expToMap_btn_Click(object sender, EventArgs e)
        {
            if (m_outfilename == "") return;
            m_mainForm.openRasterFile(m_outfilename);
            this.expToMap_btn.Enabled = false;
        }

        /// <summary>
        /// 销毁分类窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeWindow_btn_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        /// <summary>
        /// 分类完成后执行分类后处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void postClassification_btn_Click(object sender, EventArgs e)
        {
            PostClassificationForm postClassForm = new PostClassificationForm(m_mainForm, m_idlPath);
            postClassForm.Show();
        }

        /// <summary>
        /// 将窗口嵌入容器中
        /// </summary>
        /// <param name="form"></param>
        /// <param name="parent"></param>
        private void buildFormIntoParent(DevExpress.XtraEditors.XtraForm form, System.Windows.Forms.Control parent)
        {
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            form.TopLevel = false;
            form.Parent = parent;
            form.Dock = DockStyle.Fill;
            form.Show();
        }

        /// <summary>
        /// 销毁窗口列表中的所有窗口
        /// </summary>
        private void disposeAllClassifyForm()
        {
            foreach(DevExpress.XtraEditors.XtraForm form in m_formList)
            {
                if(form!=null && form.IsDisposed == false)
                    form.Dispose();
            }
        }
    }
}