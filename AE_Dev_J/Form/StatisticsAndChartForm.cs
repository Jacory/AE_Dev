using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraVerticalGrid.Rows;
using MathNet.Numerics.Statistics;

namespace AE_Dev_J.Form
{
    public partial class StatisticsAndChartForm : DevExpress.XtraEditors.XtraForm
    {
        private GridView gridview = null;
        private string currentcolumn = null;

        public StatisticsAndChartForm(GridView gv,string columnName)
        {
            InitializeComponent();
            gridview = gv;
            currentcolumn = columnName;
        }

        private void StatisticsAndChartForm_Load(object sender, EventArgs e)
        {

            for (int i = 0; i < gridview.Columns.Count; i++)
            {
                CategoryRow field = new CategoryRow(gridview.Columns[i].FieldName);

                EditorRow row_count = new EditorRow();
                row_count.Properties.Caption = "Count";
                row_count.Properties.Value= gridview.RowCount;
                row_count.Appearance.Options.UseTextOptions = true;
                row_count.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;

                field.ChildRows.Add(row_count);

                if (gridview.Columns[i].ColumnType.IsValueType)
                {
                    var data = new double[gridview.RowCount];
                    for (int j = 0; j < gridview.RowCount; j++)
                    {
                        data[j] = (double)gridview.GetRowCellValue(j, field.Properties.Caption);
                    }

                    EditorRow row_Minimum = new EditorRow();
                    row_Minimum.Properties.Caption = "Minimum";
                    row_Minimum.Properties.Value = data.Minimum();
                    row_Minimum.Appearance.Options.UseTextOptions = true;
                    row_Minimum.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;

                    EditorRow row_Maximum = new EditorRow();
                    row_Maximum.Properties.Caption = "Maximum";
                    row_Maximum.Properties.Value=data.Maximum();
                    row_Maximum.Appearance.Options.UseTextOptions = true;
                    row_Maximum.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;

                    EditorRow row_Sum = new EditorRow();
                    row_Sum.Properties.Caption = "Sum";
                    row_Sum.Properties.Value = data.Sum();
                    row_Sum.Appearance.Options.UseTextOptions = true;
                    row_Sum.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;


                    EditorRow row_Mean = new EditorRow();
                    row_Mean.Properties.Caption = "Mean";
                    row_Mean.Properties.Value = data.Mean().ToString("0.000000");
                    row_Mean.Appearance.Options.UseTextOptions = true;
                    row_Mean.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;

                    EditorRow row_Standard_Deviation = new EditorRow();
                    row_Standard_Deviation.Properties.Caption = "Standard Deviation";
                    row_Standard_Deviation.Properties.Value = data.StandardDeviation().ToString("0.000000");
                    row_Standard_Deviation.Appearance.Options.UseTextOptions = true;
                    row_Standard_Deviation.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;

                    field.ChildRows.Add(row_Minimum);
                    field.ChildRows.Add(row_Maximum);
                    field.ChildRows.Add(row_Sum);
                    field.ChildRows.Add(row_Mean);
                    field.ChildRows.Add(row_Standard_Deviation);

                }
                EditorRow row_Nulls = new EditorRow();
                row_Nulls.Properties.Caption = "Nulls";
                field.ChildRows.Add(row_Nulls);
                //设置是否折叠字段统计内容
                if (field.Properties.Caption!=currentcolumn)
                {
                    field.Expanded = false;
                }
                StatisticsAndChart_vGridControl.Rows.AddRange(new BaseRow[] { field });

            }

        }


    }
}