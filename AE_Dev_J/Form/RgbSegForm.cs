using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;

namespace AE_Dev_J
{
    enum ProcessMode
    {
        singleFile, // 单文件处理
        multiFile // 文件夹批处理
    }

    /// <summary>
    /// 自动分割算法窗口
    /// </summary>
    public partial class RgbSegForm : DevExpress.XtraEditors.XtraForm
    {
        private string inputPath = ""; // 输入路径
        private string outputPath = ""; // 输出路径
        COM_IDL_connectLib.COM_IDL_connect ocom; // COM对象

        private ProcessMode mode = ProcessMode.singleFile; // 默认为单文件模式

        private void FrmAutoSeg_Load(object sender, EventArgs e)
        {
            ocom = new COM_IDL_connectLib.COM_IDL_connect();
            ocom.CreateObject(0, 0, 0);
        }

        public RgbSegForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 点击选择输入文件路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void browseinputFilebtn_Click(object sender, EventArgs e)
        {
            this.inputTB.Text = null;

            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "image files(*.img,*.tif,*.tiff,*.dat)|*.img;*.tif;*.tiff;*.dat";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog.FileName;
            }
        }

        /// <summary>
        /// 点击选择输出文件路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void browseOutFileBtn_Click(object sender, EventArgs e)
        {
            this.outputTB.Text = null;
            try
            {
                this.saveFileDialog.Filter = "image files(*.img)|*.img";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    this.outputTB.Text = saveFileDialog.FileName;
                }
            }
            catch
            {
                MessageBox.Show("选择数据出错", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 点击选择输入文件夹路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void browseinputFolderBtn_Click_1(object sender, EventArgs e)
        {
            mode = ProcessMode.multiFile;
            try
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.Description = "请选择文件夹";
                if (dialog.ShowDialog() == DialogResult.OK || dialog.ShowDialog() == DialogResult.Yes)
                {
                    this.inputFolderTB.Text = dialog.SelectedPath;
                }
            }
            catch
            {
                MessageBox.Show("选择文件夹出错", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 点击选择输出文件夹路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void browseOutFolderBtn_Click(object sender, EventArgs e)
        {
            mode = ProcessMode.multiFile;
            try
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.Description = "请选择保存文件夹";
                if (dialog.ShowDialog() == DialogResult.OK || dialog.ShowDialog() == DialogResult.Yes)
                {
                    this.outputFolderTB.Text = dialog.SelectedPath;
                }
            }
            catch
            {
                MessageBox.Show("选择文件夹出错", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 运行主要代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okBtn_Click(object sender, EventArgs e)
        {
            string runStr = "";
            if (mode == ProcessMode.singleFile)
            {
                inputPath = this.inputTB.Text;
                outputPath = this.outputTB.Text;
                runStr = "rgb_classify,'" + inputPath + "','" + outputPath + "','" + "singleFileMode'"; 
            }
            else if (mode == ProcessMode.multiFile)
            {
                inputPath = this.inputFolderTB.Text + "\\";
                outputPath = this.outputFolderTB.Text + "\\";
                runStr = "rgb_classify,'" + inputPath + "','" + outputPath + "','" + "multiFileMode'"; 
            }
            else 
            {
                MessageBox.Show("出错了","警告",MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (runStr != "")
            {
                if (inputPath == "" || outputPath == "")
                {
                    MessageBox.Show("请输入正确的路径", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string proPath = AppDomain.CurrentDomain.BaseDirectory + "rgb_classify.pro";
                ocom.ExecuteString(@".compile '" + proPath + "'");

                ocom.ExecuteString(runStr);
                ocom.ExecuteString(@".run");
                ocom.DestroyObject();
                int temp = System.Runtime.InteropServices.Marshal.ReleaseComObject(ocom);
                MessageBox.Show("finish");
                this.Close();
            }
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}