using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;

namespace AE_Dev_J.Form
{
    public partial class AddFeatureClassForm : DevExpress.XtraEditors.XtraForm
    {
        private AxMapControl m_mapControl = null;
        public AddFeatureClassForm(AxMapControl mapControl)
        {
            InitializeComponent();
            AddFeatureClass_groupControl.Enabled = false;
            m_mapControl = mapControl;
        }
        /// <summary>
        /// 更改选项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddFeatrueClass_radioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AddFeatrueClass_radioGroup.SelectedIndex==0)
            {
                AddFeatureClass_groupControl.Enabled = false;
            }
            else
            {
                AddFeatureClass_groupControl.Enabled = true;
            }
        }
        /// <summary>
        /// 添加矢量文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddFeatrueClass_buttonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if(fbd .ShowDialog() == DialogResult.OK)
            {
                AddFeatrueClass_locationbutton.Text = fbd.SelectedPath;
            }
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddFeatrueClass_Cancelbutton_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddFeatrueClass_OKbutton_Click(object sender, EventArgs e)
        {
            if (AddFeatrueClass_radioGroup.SelectedIndex==0)
            {
                //临时图层
                //检查文件名是否已存在
                int changefilename = 0;
                while (System.IO.File.Exists(Application.StartupPath + "\\temp\\TempLayer" + changefilename + ".shp"))
                {
                    changefilename++;
                }

                //创建点
                IFields pFields = new FieldsClass();
                IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;
                IField pField = new FieldClass();
                IFieldEdit pFieldEdit = pField as IFieldEdit;
                pFieldEdit.Name_2 = "Shape";
                pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;

                //设置geometry definition
                IGeometryDef pGeometryDef = new GeometryDefClass();
                IGeometryDefEdit pGeometryDefEdit = pGeometryDef as IGeometryDefEdit;
                if (AddFeatrueClass_geometrytype.SelectedText == "POINT")
                {
                    pGeometryDefEdit.GeometryType_2 = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint;//点
                }
                else 
                {
                    if (AddFeatrueClass_geometrytype.SelectedText == "POLYLINE")
                    {
                        pGeometryDefEdit.GeometryType_2 = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline;//线
                    }
                    else
                    {
                        pGeometryDefEdit.GeometryType_2 = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon;//面
                    }
                }
                pGeometryDefEdit.SpatialReference_2 = null;
                pFieldEdit.GeometryDef_2 = pGeometryDef;
                pFieldsEdit.AddField(pField);

                IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactory();
                IFeatureWorkspace pFeatureWorkspace = pWorkspaceFactory.OpenFromFile(Application.StartupPath + "\\temp", 0) as IFeatureWorkspace;
                pFeatureWorkspace.CreateFeatureClass("TempLayer"+changefilename+".shp", pFields, null, null, esriFeatureType.esriFTSimple, "Shape", "");

                m_mapControl.AddShapeFile(Application.StartupPath + "\\temp", "TempLayer" + changefilename + ".shp");
            }
            else
            {
                //检查输入是否为空
                if (AddFeatrueClass_geometrytype.Text==""||AddFeatrueClass_locationbutton.Text==""||AddFeatrueClass_filename.Text=="")
                {
                    MessageBox.Show("输入不能为空");
                    return;
                }
                else
                {
                    //检查文件名是否已存在
                    if (System.IO.File.Exists(AddFeatrueClass_locationbutton.Text + "\\" + AddFeatrueClass_filename.Text)
                        || System.IO.File.Exists(AddFeatrueClass_locationbutton.Text + "\\" + AddFeatrueClass_filename.Text+".shp"))//检查文件是否存在
                    {
                        MessageBox.Show("该文件夹下已经有同名文件，请更改文件名称。");
                        return;
                    }
                }
                //自定义图层
                IFields pFields = new FieldsClass();
                IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;
                IField pField = new FieldClass();
                IFieldEdit pFieldEdit = pField as IFieldEdit;
                pFieldEdit.Name_2 = "Shape";
                pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;

                //设置geometry definition
                IGeometryDef pGeometryDef = new GeometryDefClass();
                IGeometryDefEdit pGeometryDefEdit = pGeometryDef as IGeometryDefEdit;
                if (AddFeatrueClass_geometrytype.SelectedText=="POINT")
                {
                    pGeometryDefEdit.GeometryType_2 = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint;
                }
                else
                {
                    if (AddFeatrueClass_geometrytype.SelectedText=="POLYLINE")
                    {
                        pGeometryDefEdit.GeometryType_2 = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline;
                    }
                    else
                    {
                        pGeometryDefEdit.GeometryType_2 = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon;
                    }
                }
                pGeometryDefEdit.SpatialReference_2 = null;
                pFieldEdit.GeometryDef_2 = pGeometryDef;
                pFieldsEdit.AddField(pField);

                IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactory();
                IFeatureWorkspace pFeatureWorkspace = pWorkspaceFactory.OpenFromFile(AddFeatrueClass_locationbutton.Text, 0) as IFeatureWorkspace;

                int i = AddFeatrueClass_filename.Text. IndexOf(".shp");
                if (i == -1)
                    pFeatureWorkspace.CreateFeatureClass(AddFeatrueClass_filename.Text + ".shp", pFields, null, null, esriFeatureType.esriFTSimple, "Shape", "");
                else
                    pFeatureWorkspace.CreateFeatureClass(AddFeatrueClass_filename.Text, pFields, null, null, esriFeatureType.esriFTSimple, "Shape", "");

                m_mapControl.AddShapeFile(AddFeatrueClass_locationbutton.Text, AddFeatrueClass_filename.Text);

            }
            this.Close();
            this.Dispose();
        }

    }
}