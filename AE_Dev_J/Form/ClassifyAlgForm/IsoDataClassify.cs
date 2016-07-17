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
    public partial class IsoDataClassify : DevExpress.XtraEditors.XtraForm
    {
        private int maxIter = 1;
        public int MaxIter
        {
            get { return maxIter; }
            set { maxIter = value; }
        }

        private int minClassNum = 5;
        public int MinClassNum
        {
            get { return minClassNum; }
            set { minClassNum = value; }
        }

        private int maxClassNum = 10;
        public int MaxClassNum
        {
            get { return maxClassNum; }
            set { maxClassNum = value; }
        }

        private double chgThresh = 0.05;
        public double ChgThresh
        {
            get { return chgThresh; }
            set { chgThresh = value; }
        }

        private int minClassPixels = 1;
        public int MinClassPixels
        {
            get { return minClassPixels; }
            set { minClassPixels = value; }
        }

        private double maxStd = 0.5;
        public double MaxStd
        {
            get { return maxStd; }
            set { maxStd = value; }
        }

        private double minDis = 5;
        public double MinDis
        {
            get { return minDis; }
            set { minDis = value; }
        }

        private int maxMergePixel = 2;
        public int MaxMergePixel
        {
            get { return maxMergePixel; }
            set { maxMergePixel = value; }
        }

        public IsoDataClassify()
        {
            InitializeComponent();
        }

        private void isodata_minClassNum_spinEdit_EditValueChanged(object sender, EventArgs e)
        {
            this.minClassNum = (int)isodata_minClassNum_spinEdit.Value;
        }

        private void isodata_maxClassNum_spinEdit_EditValueChanged(object sender, EventArgs e)
        {
            maxClassNum = (int)isodata_maxClassNum_spinEdit.Value;
        }

        private void maxIter_spinEdit_EditValueChanged(object sender, EventArgs e)
        {
            maxIter = (int)maxIter_spinEdit.Value;
        }

        private void chgThresh_spinEdit_EditValueChanged(object sender, EventArgs e)
        {
            chgThresh = (double)chgThresh_spinEdit.Value;
        }

        private void minClassPixels_spinEdit_EditValueChanged(object sender, EventArgs e)
        {
            minClassPixels = (int)minClassPixels_spinEdit.Value;
        }

        private void maxStd_spinEdit_EditValueChanged(object sender, EventArgs e)
        {
            maxStd = (double)maxStd_spinEdit.Value;
        }

        private void minDis_spinEdit_EditValueChanged(object sender, EventArgs e)
        {
            minDis = (double)minDis_spinEdit.Value;
        }

        private void maxMergePixel_spinEdit_EditValueChanged(object sender, EventArgs e)
        {
            maxMergePixel = (int)maxMergePixel_spinEdit.Value;
        }

        
    }
}