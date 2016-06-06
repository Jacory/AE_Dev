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

namespace AE_Dev_J.Form
{
    public partial class ClassificationForm : DevExpress.XtraEditors.XtraForm
    {
        private string m_idlPath = ""; // IDL的pro文件路径

        private string m_inDataPath = ""; // 输入文件路径，若是批处理模式，则为文件夹路径
        private string m_outDataPath = ""; // 输出文件路径，若是批处理模式，则为文件夹路径

        private ClassifyMethod m_selectedMethod = ClassifyMethod.None; // 指定用户选择的算法
        private bool m_processIsDone = false; // 指定算法的是否执行完毕

        private MainForm m_mainForm = null;

        private string m_outfilename = "";

        static string m_runStr = null;
        static string m_proFileFullPath = null;


        /// <summary>
        /// 分类方法枚举
        /// </summary>
        public enum ClassifyMethod
        {
            None,
            Parallelepiped,
            MinimumDistance,
            MahalanobisDistance,
            MaximumLikelihood,
            SpectralAngleMapper,
            SpectralInformationDivergence,
            BinaryEncoding,
            NeuralNet,
            SupportVectrorMachine,
            IsoData,
            KMeans
        };

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

            // 参数设置面板
            showOnlyIndexTabPage(0, this.paramSetting_xtraTabControl);
            showOnlyIndexTabPage(0, this.super_param_xtraTabControl);
            showOnlyIndexTabPage(0, this.unsuper_param_xtraTabControl);

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
                                this.m_selectedMethod = (ClassifyMethod)(superviseMethod_radioGroup.SelectedIndex + 1);
                            else if (unsupervise_checkEdit.Checked == true)
                                this.m_selectedMethod = (ClassifyMethod)(unsuperviseMethod_radioGroup.SelectedIndex + 1 + 9);

                            if (preBtn.Properties.Enabled == false)
                                preBtn.Properties.Enabled = true;
                            
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
                        this.class_method_textEdit.Text = this.getMethodString(this.m_selectedMethod);
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

            showOnlyIndexTabPage(index, this.super_param_xtraTabControl);
        }

        /// <summary>
        /// 分类方法选择事件，控制非监督分类参数配置面板显示的方法参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void unsuperviseMethod_radioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = this.unsuperviseMethod_radioGroup.SelectedIndex;
            showOnlyIndexTabPage(index, this.unsuper_param_xtraTabControl);
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

        #region 平行六面体

        private void paralle_thresh_spinEdit_EditValueChanged(object sender, EventArgs e)
        {
            this.paralle_thresh_trackBarControl.Value = (Int32)this.paralle_thresh_spinEdit.Value;
        }

        private void paralle_thresh_trackBarControl_EditValueChanged(object sender, EventArgs e)
        {
            this.paralle_thresh_spinEdit.Value = (Decimal)this.paralle_thresh_trackBarControl.Value;
        }

        private void paralle_thresh_radioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.paralle_thresh_radioGroup.SelectedIndex == 0)
            {
                this.paralle_thresh_spinEdit.Enabled = false;
                this.paralle_thresh_trackBarControl.Enabled = false;
            }
            else if (this.paralle_thresh_radioGroup.SelectedIndex == 1)
            {
                this.paralle_thresh_spinEdit.Enabled = true;
                this.paralle_thresh_trackBarControl.Enabled = true;
            }
        }

        #endregion 平行六面体

        #region 最小距离法

        private void minDis_std_radioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.minDis_std_radioGroup.SelectedIndex == 0)
            {
                this.minDis_std_groupBox.Enabled = false;
            }
            else if (this.minDis_std_radioGroup.SelectedIndex == 1)
            {
                this.minDis_std_groupBox.Enabled = true;
            }
        }

        private void minDis_error_radioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.minDis_error_radioGroup.SelectedIndex == 0)
            {
                this.minDis_error_groupBox.Enabled = false;
            }
            else if (this.minDis_error_radioGroup.SelectedIndex == 1)
            {
                this.minDis_error_groupBox.Enabled = true;
            }
        }

        private void minDis_std_spinEdit_EditValueChanged(object sender, EventArgs e)
        {
            this.minDis_std_trackBarControl.Value = (Int32)this.minDis_std_spinEdit.Value;
        }

        private void minDis_std_trackBarControl_EditValueChanged(object sender, EventArgs e)
        {
            this.minDis_std_spinEdit.Value = (Decimal)this.minDis_std_trackBarControl.Value;
        }

        private void minDis_error_spinEdit_EditValueChanged(object sender, EventArgs e)
        {
            this.minDis_error_trackBarControl.Value = (Int32)this.minDis_error_spinEdit.Value;
        }

        private void minDis_error_trackBarControl_EditValueChanged(object sender, EventArgs e)
        {
            this.minDis_error_spinEdit.Value = (Decimal)this.minDis_error_trackBarControl.Value;
        }

        #endregion 最小距离法

        #region 马氏距离

        private void mahDIs_radioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.mahDIs_radioGroup.SelectedIndex == 0)
                this.mahDis_groupBox.Enabled = false;
            else if (this.mahDIs_radioGroup.SelectedIndex == 1)
                this.mahDis_groupBox.Enabled = true;
        }

        private void mahDis_thresh_spinEdit_EditValueChanged(object sender, EventArgs e)
        {
            this.mahDis_thresh_trackBarControl.Value = (Int32)this.mahDis_thresh_spinEdit.Value;
        }

        private void mahDis_thresh_trackBarControl_EditValueChanged(object sender, EventArgs e)
        {
            this.mahDis_thresh_spinEdit.Value = (Decimal)this.mahDis_thresh_trackBarControl.Value;
        }

        #endregion 马氏距离

        #region 最大似然法
        private void maxLike_thresh_radioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.maxLike_thresh_radioGroup.SelectedIndex == 0)
                this.maxLike_thresh_groupBox.Enabled = false;
            if (this.maxLike_thresh_radioGroup.SelectedIndex == 1)
                this.maxLike_thresh_groupBox.Enabled = true;
        }

        private void maxLike_ratio_radioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.maxLike_ratio_radioGroup.SelectedIndex == 0)
                this.maxLike_ratio_groupBox.Enabled = false;
            if (this.maxLike_ratio_radioGroup.SelectedIndex == 1)
                this.maxLike_ratio_groupBox.Enabled = true;
        }

        private void maxLike_thresh_spinEdit_EditValueChanged(object sender, EventArgs e)
        {
            this.maxLike_thresh_trackBarControl.Value = (Int32)this.maxLike_thresh_spinEdit.Value;
        }

        private void maxLike_thresh_trackBarControl_EditValueChanged(object sender, EventArgs e)
        {
            this.maxLike_thresh_spinEdit.Value = (Decimal)this.maxLike_thresh_trackBarControl.Value;
        }

        private void maxLike_ratio_spinEdit_EditValueChanged(object sender, EventArgs e)
        {
            this.maxLike_ratio_trackBarControl.Value = (Int32)this.maxLike_ratio_spinEdit.Value;
        }

        private void maxLike_ratio_trackBarControl_EditValueChanged(object sender, EventArgs e)
        {
            this.maxLike_ratio_spinEdit.Value = (Decimal)this.maxLike_ratio_trackBarControl.Value;
        }
        #endregion 最大似然法

        #region 神经网络

        private void ann_thresh_trackBarControl_EditValueChanged(object sender, EventArgs e)
        {
            this.ann_thresh_spinEdit.Value = (Decimal)this.ann_thresh_trackBarControl.Value;
        }

        private void ann_thresh_spinEdit_EditValueChanged(object sender, EventArgs e)
        {
            this.ann_thresh_trackBarControl.Value = (Int32)this.ann_thresh_spinEdit.Value;
        }

        private void ann_weightSpeed_trackBarControl_EditValueChanged(object sender, EventArgs e)
        {
            this.ann_weightSpeed_spinEdit.Value = (Decimal)this.ann_weightSpeed_trackBarControl.Value;
        }

        private void ann_weightSpeed_spinEdit_EditValueChanged(object sender, EventArgs e)
        {
            this.ann_weightSpeed_trackBarControl.Value = (Int32)this.ann_weightSpeed_spinEdit.Value;
        }

        private void ann_weight_trackBarControl_EditValueChanged(object sender, EventArgs e)
        {
            this.ann_weight_spinEdit.Value = (Decimal)this.ann_weight_trackBarControl.Value;
        }

        private void ann_weight_spinEdit_EditValueChanged(object sender, EventArgs e)
        {
            this.ann_weight_trackBarControl.Value = (Int32)this.ann_weight_spinEdit.Value;
        }

        private void ann_rms_trackBarControl_EditValueChanged(object sender, EventArgs e)
        {
            this.ann_rms_spinEdit.Value = (Decimal)this.ann_rms_trackBarControl.Value;
        }

        private void ann_rms_spinEdit_EditValueChanged(object sender, EventArgs e)
        {
            this.ann_rms_trackBarControl.Value = (Int32)this.ann_rms_spinEdit.Value;
        }

        private void ann_hideLayer_trackBarControl_EditValueChanged(object sender, EventArgs e)
        {
            this.ann_hideLayer_spinEdit.Value = (Decimal)this.ann_hideLayer_trackBarControl.Value;
        }

        private void ann_hideLayer_spinEdit_EditValueChanged(object sender, EventArgs e)
        {
            this.ann_hideLayer_trackBarControl.Value = (Int32)this.ann_hideLayer_spinEdit.Value;
        }

        private void ann_iterCount_trackBarControl_EditValueChanged(object sender, EventArgs e)
        {
            this.ann_iterCount_spinEdit.Value = (Decimal)this.ann_iterCount_trackBarControl.Value;
        }

        private void ann_iterCount_spinEdit_EditValueChanged(object sender, EventArgs e)
        {
            this.ann_iterCount_trackBarControl.Value = (Int32)this.ann_iterCount_spinEdit.Value;
        }

        #endregion 神经网络

        #region 支持向量机

        private void svm_balance_trackBarControl_EditValueChanged(object sender, EventArgs e)
        {
            this.svm_balance_spinEdit.Value = (Decimal)this.svm_balance_trackBarControl.Value;
        }

        private void svm_balance_spinEdit_EditValueChanged(object sender, EventArgs e)
        {
            this.svm_balance_trackBarControl.Value = (Int32)this.svm_balance_spinEdit.Value;
        }

        private void svm_bias_trackBarControl_EditValueChanged(object sender, EventArgs e)
        {
            this.svm_bias_spinEdit.Value = (Decimal)this.svm_bias_trackBarControl.Value;
        }

        private void svm_bias_spinEdit_EditValueChanged(object sender, EventArgs e)
        {
            this.svm_bias_trackBarControl.Value = (Int32)this.svm_bias_spinEdit.Value;
        }

        private void svm_thresh_trackBarControl_EditValueChanged(object sender, EventArgs e)
        {
            this.svm_thresh_spinEdit.Value = (Decimal)this.svm_thresh_trackBarControl.Value;
        }

        private void svm_thresh_spinEdit_EditValueChanged(object sender, EventArgs e)
        {
            this.svm_thresh_trackBarControl.Value = (Int32)this.svm_thresh_spinEdit.Value;
        }
        #endregion 支持向量机

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
            if (m_selectedMethod == ClassifyMethod.None) return;

            this.class_marqueeProgressBarControl.Show(); // 显示进度条
            
            // 判断文件处理模式
            int mode = 0;
            if (singleMode_checkEdit.Checked == true) mode = 0;
            else if (batchMode_checkEdit.Checked == true) mode = 1;

            // 设置对应的IDL命令字符串
            string proFilename = null;
            switch (m_selectedMethod)
            {
                case ClassifyMethod.Parallelepiped:
                    proFilename = "parallelepiped_classify.pro";
                    break;

                case ClassifyMethod.MinimumDistance:
                    proFilename = "minimumdistance_classify.pro";
                    break;

                case ClassifyMethod.MahalanobisDistance:
                    proFilename = "mahalanobis_classify.pro";
                    break;

                case ClassifyMethod.MaximumLikelihood:
                    proFilename = "maximumlikelihood_classify.pro";
                    break;

                case ClassifyMethod.SpectralAngleMapper:
                    proFilename = "SAM_classify.pro";
                    break;

                case ClassifyMethod.SpectralInformationDivergence:
                    proFilename = "SIM_classify.pro";
                    break;

                case ClassifyMethod.BinaryEncoding:
                    proFilename = "BinaryEncoding_classify.pro";
                    break;

                case ClassifyMethod.NeuralNet:
                    proFilename = "ANN_classify.pro";
                    break;

                case ClassifyMethod.SupportVectrorMachine:
                    proFilename = "svm_classify.pro";
                    break;

                case ClassifyMethod.IsoData:
                    proFilename = "isodata.pro";
                    m_runStr =  "isodata, '" + inDataFile_btn.Text + "','"
                                + outDataFile_btn.Text + "',"
                                + isodata_maxIter_spinEdit.Value + ","
                                + isodata_chgThresh_spinEdit.Value + ","
                                + isodata_minDis_spinEdit.Value + ","
                                + isodata_maxMergePixel_spinEdit.Value + ","
                                + isodata_minClassPixels_spinEdit.Value + ","
                                + isodata_maxStd_spinEdit.Value + ","
                                + isodata_minClasses_spinEdit.Value + ","
                                + mode;
                    break;

                case ClassifyMethod.KMeans:
                    proFilename = "k_means.pro";
                    m_runStr = "k_means , '" + inDataFile_btn.Text + "','"
                                + outDataFile_btn.Text + "',"
                                + kmeans_numClasses_spinEdit.Value + ","
                                + kmeans_maxIter_spinEdit.Value + ","
                                + kmeans_changeThresh_spinEdit.Value
                                + mode;
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
        /// 根据classifyMethod枚举，获取方法字符串
        /// </summary>
        /// <param name="method">classifyMethod枚举</param>
        /// <returns></returns>
        private string getMethodString(ClassifyMethod method)
        {
            switch (method)
            {
                case ClassifyMethod.Parallelepiped:
                    return "Paralleleepiped";
                   
                case ClassifyMethod.MinimumDistance:
                    return "Minimum Distance";

                case ClassifyMethod.MahalanobisDistance:
                        return "Mahalanobis Distance";

                case ClassifyMethod.MaximumLikelihood:
                        return "Maximum Likelihood";

                case ClassifyMethod.SpectralAngleMapper:
                        return "Spectral Angle Mapper";
               
                case ClassifyMethod.SpectralInformationDivergence:
                        return "Spectral Information Divergence";
               
                case ClassifyMethod.BinaryEncoding:
                        return "Binary Encoding";
            
                case ClassifyMethod.NeuralNet:
                        return "Artifical Neural Net";
         
                case ClassifyMethod.SupportVectrorMachine:
                        return "Support Vector Machine";
       
                case ClassifyMethod.IsoData:
                        return "ISODATA";
         
                case ClassifyMethod.KMeans:
                        return "K-Means";
          
                default:
                        return "None";
            }
        }

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
            PostClassificationForm postClassForm = new PostClassificationForm(m_mainForm);
            postClassForm.Show();
        }
    }
}