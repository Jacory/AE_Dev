using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;

namespace AE_Dev_J.Form
{
    public partial class BufferAnalysisForm : DevExpress.XtraEditors.XtraForm
    {
        private AxMapControl m_mapControl = null;

        public BufferAnalysisForm(AxMapControl mapControl)
        {
            m_mapControl = mapControl;

            InitializeComponent();

            // 当前矢量图层的图层列表
            if (m_mapControl != null)
            {
                for (int i = 0; i < m_mapControl.LayerCount; i++)
                {
                    ILayer layer = m_mapControl.get_Layer(i);
                    inputFeature_btnEdit.Properties.Items.Add(layer.Name);
                }
            }
           
            // 设置各个控件默认值或初始状态
            linearUnit_checkEdit.Checked = true;
            unit_comboEdit.SelectedIndex = 0;
            sideType_comboEdit.SelectedIndex = 0;
            endType_comboEdit.SelectedIndex = 0;
            dissolveType_comboEdit.SelectedIndex = 0;
            field_comboEdit.Enabled = false;
        }

        private void inputFeature_btnEdit_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1) // 打开选择文件对话框
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "select feature file";
                ofd.Filter = "shpfile(*.shp)|*.shp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    inputFeature_btnEdit.Text = ofd.FileName;
                }
            }
            
        }

        private void outputFeature_btnEdit_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            // 打开保存文件对话框
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "output feature to file";
            sfd.Filter = "shpfile(*.shp)|*.shp";
            sfd.AddExtension = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (sfd.FileName != "")
                {
                    outputFeature_btnEdit.Text = sfd.FileName;
                }
            }
        }

        private void field_checkEdit_CheckStateChanged(object sender, EventArgs e)
        {
            if (field_checkEdit.Checked == true)
            {
                linearUnit_checkEdit.Checked = false;
                field_comboEdit.Enabled = true;
            }
            else
            {
                linearUnit_checkEdit.Checked = true;
                field_comboEdit.Enabled = false;
            }
        }

        private void linearUnit_checkEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (linearUnit_checkEdit.Checked == true)
            {
                field_checkEdit.Checked = false;
                linearUnit_spinEdit.Enabled = true;
            }
            else
            {
                field_checkEdit.Checked = true;
                linearUnit_spinEdit.Enabled = false;
            }
        }

        private void cancel_btn_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }


    }
}