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
    /// <summary>
    /// 查看、收集光谱曲线，并支持将光谱曲线导出到文件
    /// </summary>
    public partial class ViewSpectralForm : DevExpress.XtraEditors.XtraForm
    {
        private MainForm m_mainForm = null;

        public ViewSpectralForm(MainForm mf)
        {
            m_mainForm = mf;
            InitializeComponent();
        }

    }
}