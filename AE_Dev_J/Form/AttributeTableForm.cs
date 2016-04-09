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
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using DevExpress.XtraGrid;

namespace AE_Dev_J.Form
{
    /// <summary>
    /// 属性表窗口
    /// </summary>
    public partial class AttributeTableForm : DevExpress.XtraEditors.XtraForm
    {
        private IFeatureLayer m_layer = null;
        private AxMapControl m_mapControl = null; // 属性表需要与mapControl做交互

        public AttributeTableForm(IFeatureLayer layer, MainForm mainForm)
        {
            InitializeComponent();

            m_layer = layer;
            m_mapControl= mainForm.getMapControl();
        }

        private void AttributeTableForm_Load(object sender, EventArgs e)
        {
            this.Text = m_layer.Name + " Attribute Table: ";
            //+ m_layer.FeatureClass.FeatureCount(new ESRI.ArcGIS.Geodatabase.QueryFilter()).ToString() + " features";
            importAttribute(m_layer);
        }
        private void importAttribute(IFeatureLayer featurelayer)
        {
            DataTable dt = new DataTable();
            IFeatureClass m_featureclass = featurelayer.FeatureClass;
            if (m_featureclass == null)
            {
                return;
            }
            //spreadsheetControl1.BeginUpdate();
            //DevExpress.Spreadsheet.Worksheet worksheet = spreadsheetControl1.Document.Worksheets[0];
            for (int i = 0; i < m_featureclass.Fields.FieldCount; i++)
            {
                DataColumn dc = new DataColumn(m_featureclass.Fields.get_Field(i).Name);
                dt.Columns.Add(dc);
            }
            IFeatureCursor pFeatureCuror = m_featureclass.Search(null, false);
            IFeature pFeature = pFeatureCuror.NextFeature();
            while (pFeature != null)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < m_featureclass.Fields.FieldCount; j++)
                {
                    dr[j] = pFeature.get_Value(j).ToString();
                }
                dt.Rows.Add(dr);
                pFeature = pFeatureCuror.NextFeature();
            }
            gridControl1.DataSource = dt;

            //worksheet.Columns.AutoFit(0, m_featureclass.Fields.FieldCount-1);
            //spreadsheetControl1.EndUpdate();
        }
        public void appendTable(IFeatureLayer layer)
        {

        }



    }
}