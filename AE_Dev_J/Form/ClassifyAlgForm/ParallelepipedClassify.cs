using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace AE_Dev_J.Form.ClassifyAlgForm
{
    public partial class ParallelepipedClassify : DevExpress.XtraEditors.XtraForm
    {
        public ParallelepipedClassify()
        {
            InitializeComponent();

            // set valid value input of trackBarControl
            this.paralle_thresh_trackBarControl.Properties.Minimum = 0;
            this.paralle_thresh_trackBarControl.Properties.Maximum = 100;
            this.paralle_thresh_trackBarControl.Properties.SmallChange = 1;

            // set default value of thresh 0.5
            this.parallel_thresh_textEdit.Text = "0.5";
            this.paralle_thresh_trackBarControl.Value = 50;

        }

        private void paralle_thresh_radioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (paralle_thresh_radioGroup.SelectedIndex == 0)
            {
                this.parallel_thresh_textEdit.Enabled = false;
                this.paralle_thresh_trackBarControl.Enabled = false;
            }
            else if (paralle_thresh_radioGroup.SelectedIndex == 1)
            {
                this.parallel_thresh_textEdit.Enabled = true;
                this.paralle_thresh_trackBarControl.Enabled = true;
            }
        }

        private void parallel_thresh_textEdit_Properties_Validating(object sender, CancelEventArgs e)
        {
            try // 判断输入阈值是否为 0~1 的小数，如果不是，就设置默认值0.5
            {
                if (Convert.ToDouble(parallel_thresh_textEdit.Text) < 1 &&
                    Convert.ToDouble(parallel_thresh_textEdit.Text) > 0)
                {
                    this.parallel_thresh_textEdit.TextChanged += new EventHandler(parallel_thresh_textEdit_TextChanged);
                }
                else
                {
                    parallel_thresh_textEdit.Text = "0.5";
                }
            }
            catch (System.Exception)
            {
                parallel_thresh_textEdit.Text = "0.5";
            }
        }

        void parallel_thresh_textEdit_TextChanged(object sender, EventArgs e)
        {
            
            this.paralle_thresh_trackBarControl.Value = (int)(Convert.ToDouble(parallel_thresh_textEdit.Text) * 100);
        }

        private void paralle_thresh_trackBarControl_EditValueChanged(object sender, EventArgs e)
        {
            parallel_thresh_textEdit.Text = (Convert.ToDouble(paralle_thresh_trackBarControl.Value) / 100).ToString();
        }

        private void paralle_thresh_trackBarControl_DoubleClick(object sender, EventArgs e)
        {
            this.paralle_thresh_trackBarControl.Value = 50;
        }


    }
}