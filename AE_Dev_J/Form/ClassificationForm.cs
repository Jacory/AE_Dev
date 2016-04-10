using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace AE_Dev_J.Form
{
    public partial class ClassificationForm : DevExpress.XtraEditors.XtraForm
    {
        public ClassificationForm()
        {
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

            // 参数设置面板
            showOnlyIndexTabPage(0, this.paramSetting_xtraTabControl);
            showOnlyIndexTabPage(0, this.super_param_xtraTabControl);
            showOnlyIndexTabPage(0, this.unsuper_param_xtraTabControl);
        }

        #region Select Method Page
        
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
            else { this.superviseMethod_radioGroup.Enabled = false;}
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
            else{ this.unsuperviseMethod_radioGroup.Enabled = false; }
        }

        #endregion Select Method Page

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
            else if(e.Button == nextBtn)
            {
                if ( preBtn.Properties.Enabled == false)
                    preBtn.Properties.Enabled = true;
                this.classfication_backstageViewControl.SelectedTabIndex += 1;
                if (this.classfication_backstageViewControl.SelectedTab != null)
                    this.classfication_backstageViewControl.SelectedTab.Enabled = true;
            }
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

        #endregion 支持向量机

        #region ISODATA
        private void isodata_maxIter_spinEdit_ValueChanged(object sender, EventArgs e)
        {
            this.isodata_maxIter_trackBarControl.Value = (Int32)this.isodata_maxIter_spinEdit.Value;
        }

        private void isodata_maxIter_trackBarControl_ValueChanged(object sender, EventArgs e)
        {
            this.isodata_maxIter_spinEdit.Value = (Decimal)this.isodata_maxIter_trackBarControl.Value;
        }

        private void isodata_chgThresh_spinEdit_ValueChanged(object sender, EventArgs e)
        {
            this.isodata_chgThresh_trackBarControl.Value = (Int32)this.isodata_chgThresh_spinEdit.Value;
        }

        private void isodata_chgThresh_trackBarControl_ValueChanged(object sender, EventArgs e)
        {
            this.isodata_chgThresh_spinEdit.Value = (Decimal)this.isodata_chgThresh_trackBarControl.Value;
        }

        private void isodata_maxMergePixel_trackBarControl_ValueChanged(object sender, EventArgs e)
        {
            this.isodata_maxMergePixel_spinEdit.Value = (Decimal)this.isodata_maxMergePixel_trackBarControl.Value;
        }

        private void isodata_maxMergePixel_spinEdit_ValueChanged(object sender, EventArgs e)
        {
            this.isodata_maxMergePixel_trackBarControl.Value = (Int32)this.isodata_maxMergePixel_spinEdit.Value;
        }

        private void isodata_minClassPixels_trackBarControl_ValueChanged(object sender, EventArgs e)
        {
            this.isodata_minClassPixels_spinEdit.Value = (Decimal)this.isodata_minClassPixels_trackBarControl.Value;
        }

        private void isodata_minClassPixels_spinEdit_ValueChanged(object sender, EventArgs e)
        {
            this.isodata_minClassPixels_trackBarControl.Value = (Int32)this.isodata_minClassPixels_spinEdit.Value;
        }
        #endregion ISODATA

        private void svm_thresh_trackBarControl_EditValueChanged(object sender, EventArgs e)
        {
            this.svm_thresh_spinEdit.Value = (Decimal)this.svm_thresh_trackBarControl.Value;
        }

        private void svm_thresh_spinEdit_EditValueChanged(object sender, EventArgs e)
        {
            this.svm_thresh_trackBarControl.Value = (Int32)this.svm_thresh_spinEdit.Value;
        }

        /// <summary>
        /// 浏览输出文件路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void expData_buttonEdit_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "image files(*.img)|*.img";
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                this.expData_buttonEdit.Text = saveDialog.FileName;
            }
        }


    }
}