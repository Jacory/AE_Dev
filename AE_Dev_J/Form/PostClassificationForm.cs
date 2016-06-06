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
using ESRI.ArcGIS.Geodatabase;
using System.Threading;

namespace AE_Dev_J.Form
{
    public partial class PostClassificationForm : DevExpress.XtraEditors.XtraForm
    {
        private string m_idlPath = ""; // IDL的pro文件路径

        private MainForm m_mainForm = null;

        private string m_inputFilePath = "";
        private string m_outputFilePath = "";
        private string m_inputFolder = "";
        private string m_outputFolder = "";

        private enum processMode { single, batch };
        private processMode m_processMode = processMode.single; // 默认单文件模式

        public PostClassificationForm()
        {
            InitializeComponent();
        }

        public PostClassificationForm(MainForm mf, string idlPath)
        {
            m_mainForm = mf;
            m_idlPath = idlPath;

            InitializeComponent();
        }

        private void PostClassificationForm_Load(object sender, EventArgs e)
        {
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
                        inputFile_buttonEdit.Properties.Items.Add(w_name.PathName + layer.Name);
                    }
                    else
                    {
                        inputFile_buttonEdit.Properties.Items.Add(w_name.PathName + "\\" + layer.Name);
                    }
                }
            }

            this.expToMap_btn.Enabled = false;
        }

        #region 处理模式勾选逻辑
        private void batchMode_checkEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (this.batchMode_checkEdit.Checked == true)
            {
                this.singleMode_checkEdit.Checked = false;
                this.inputFile_layoutControl.Enabled = false;
                this.inputFolder_layoutControl.Enabled = true;
            }
            else if (this.batchMode_checkEdit.Checked == false)
            {
                this.inputFolder_layoutControl.Enabled = false;
            }
        }

        private void singleMode_checkEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (this.singleMode_checkEdit.Checked == true)
            {
                this.batchMode_checkEdit.Checked = false;
                this.inputFile_layoutControl.Enabled = true;
                this.inputFolder_layoutControl.Enabled = false;
            }
            else if (this.singleMode_checkEdit.Enabled == false)
            {
                this.inputFile_layoutControl.Enabled = false;
            }
        }
        #endregion 处理模式勾选逻辑

        /// <summary>
        /// 指定输入影像文件路径 -- 单文件模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void inputFile_buttonEdit_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            m_inputFolder = "";
            m_outputFolder = "";

            if (e.Button.Index == 1)
            {
                OpenFileDialog openDialog = new OpenFileDialog();
                openDialog.Filter = "image files(*.img)|*.img";
                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    this.inputFile_buttonEdit.Text = openDialog.FileName;
                    this.m_inputFilePath = this.inputFile_buttonEdit.Text;
                }
            }
        }

        /// <summary>
        /// 指定输出影像文件路径 -- 单文件模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void outputFile_buttonEdit_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            m_inputFolder = "";
            m_outputFolder = "";

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "image files(*.img)|*.img";
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                this.outputFile_buttonEdit.Text = saveDialog.FileName;
                this.m_outputFilePath = this.outputFile_buttonEdit.Text;
            }
        }

        /// <summary>
        /// 指定输入影像文件夹 -- 批处理模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void inputFolder_buttonEdit_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            m_inputFilePath = "";
            m_outputFilePath = "";

            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                this.inputFolder_buttonEdit.Text = folderDialog.SelectedPath;
                this.m_inputFolder = this.inputFolder_buttonEdit.Text;
            }
        }

        /// <summary>
        /// 指定输出文件夹 -- 批处理模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void outputFolder_buttonEdit_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            m_inputFilePath = "";
            m_outputFilePath = "";

            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                this.outputFolder_buttonEdit.Text = folderDialog.SelectedPath;
                this.m_outputFolder = this.outputFolder_buttonEdit.Text;
            }
        }

        /// <summary>
        /// 取消窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancel_btn_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        /// <summary>
        /// 将结果导入地图控件，仅单文件模式可用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void expToMap_btn_Click(object sender, EventArgs e)
        {
            if (m_outputFilePath == "") return;
            m_mainForm.openRasterFile(m_outputFilePath);
            this.expToMap_btn.Enabled = false;
        }

        /// <summary>
        /// 后台执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void postClass_backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        /// <summary>
        /// 后台算法执行完毕后响应此事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void postClass_backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        /// <summary>
        /// 执行算法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ok_btn_Click(object sender, EventArgs e)
        {
            // 判断文件处理模式
            if (this.singleMode_checkEdit.Checked == true)
            {
                m_processMode = processMode.single;
                if (this.inputFile_buttonEdit.Text == "" || this.outputFile_buttonEdit.Text == "")
                { MessageBox.Show("Please input the right file path!", "error"); return; }
            }
            else if (this.batchMode_checkEdit.Checked == true)
            {
                m_processMode = processMode.batch;
                if (this.inputFolder_buttonEdit.Text == "" || this.outputFolder_buttonEdit.Text == "")
                { MessageBox.Show("Please input the right folder path!", "error"); return; }
            }
            else
            {
                MessageBox.Show("Something goes wrong...", "Error");
                return;
            }

            switch (m_processMode)
            {
                case processMode.single: // single file mode

                    break;

                case processMode.batch: // batch file mode

                    break;

                default:
                    break;
            }
        }
    }
}