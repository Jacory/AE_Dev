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
    public partial class KmeansClassify : DevExpress.XtraEditors.XtraForm
    {
        private int m_numClass = 5;
        public int NumClass
        {
            get { return m_numClass; }
            set { m_numClass = value; }
        }

        private double m_chgThresh = 0.5;
        public double ChgThresh
        {
            get { return m_chgThresh; }
            set { m_chgThresh = value; }
        }

        private int m_maxIter = 1;
        public int MaxIter
        {
            get { return m_maxIter; }
            set { m_maxIter = value; }
        }

        public KmeansClassify()
        {
            InitializeComponent();
        }

        private void kmeans_numClasses_spinEdit_EditValueChanged(object sender, EventArgs e)
        {
            m_numClass = (int)kmeans_numClasses_spinEdit.Value;
        }

        private void kmeans_changeThresh_spinEdit_EditValueChanged(object sender, EventArgs e)
        {
            m_chgThresh = (double)kmeans_changeThresh_spinEdit.Value;
        }

        private void kmeans_maxIter_spinEdit_EditValueChanged(object sender, EventArgs e)
        {
            m_maxIter = (int)kmeans_maxIter_spinEdit.Value;
        }
    }
}