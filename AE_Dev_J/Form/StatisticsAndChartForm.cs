using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraCharts;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraVerticalGrid.ViewInfo;
using DevExpress.XtraVerticalGrid.Rows;
using DevExpress.XtraVerticalGrid;
using MathNet.Numerics.Statistics;

namespace AE_Dev_J.Form
{
    public partial class StatisticsAndChartForm : DevExpress.XtraEditors.XtraForm
    {
        private GridView gridview = null;//当前属性表
        private string currentcolumn = null;//右键单击的列名

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
                //当列类型为数值时
                if (gridview.Columns[i].ColumnType.IsValueType)
                {
                    var data = new double[gridview.RowCount];
                    int count = 0;//计算空值数
                    //将该列的数据存储进数组
                    for (int j = 0; j < gridview.RowCount; j++)
                    {
                        if (gridview.GetRowCellValue(j, field.Properties.Caption).ToString()=="")
                        {
                            count++;
                        }
                        data[j] = (double)gridview.GetRowCellValue(j, field.Properties.Caption);
                    }

                    //存储列数据
                    field.Tag = data;

                    //获取统计数据
                    //设置属性
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

                    EditorRow row_Nulls = new EditorRow();
                    row_Nulls.Properties.Caption = "Nulls";
                    row_Nulls.Properties.Value = count;
                    row_Nulls.Appearance.Options.UseTextOptions = true;
                    row_Nulls.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                    field.ChildRows.Add(row_Nulls);

                }
                    //列类型不为数值型
                else
                {
                    //将该列的数据存储进数组
                    var data = new string[gridview.RowCount];
                    EditorRow row_Nulls = new EditorRow();
                    row_Nulls.Properties.Caption = "Nulls";
                    int count = 0;
                    for (int n = 0; n < gridview.RowCount; n++)
                    {
                        if ( gridview.GetRowCellValue(n, field.Properties.Caption).ToString() == "")
                        {
                            count++;
                        }
                        data[n] = (string)gridview.GetRowCellValue(n, field.Properties.Caption);
                    }
                    //将列数据存入CategoryRow中
                    field.Tag = data;

                    //设置属性
                    row_Nulls.Properties.Value = count;
                    row_Nulls.Appearance.Options.UseTextOptions = true;
                    row_Nulls.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;

                    field.ChildRows.Add(row_Nulls);
                }
                //设置是否折叠字段统计内容
                if (field.Properties.Caption!=currentcolumn)
                {
                    field.Expanded = false;
                }
                StatisticsAndChart_vGridControl.Rows.AddRange(new BaseRow[] { field });

            }

        }
        /// <summary>
        /// 单击VGridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatisticsAndChart_vGridControl_MouseClick(object sender, MouseEventArgs e)
        {
            DevExpress.XtraVerticalGrid.VGridControl gridcontrol = new DevExpress.XtraVerticalGrid.VGridControl();
            gridcontrol = sender as DevExpress.XtraVerticalGrid.VGridControl;
            VGridHitInfo info = gridcontrol.CalcHitInfo(new Point(e.X,e.Y));
            //当左键单击HeaderCell时
            if (info.HitInfoType==HitInfoTypeEnum.HeaderCell&&e.Clicks==1&&e.Button==MouseButtons.Left)
            {
                BaseRow baserow = gridcontrol.FocusedRow;
                CategoryRow gr = baserow as CategoryRow;
                //改变HeaderCell字体颜色为红色
                for (int i = 0; i < gridcontrol.Rows.Count; i++)
                {
                    if (gridcontrol.Rows[i].Properties.Caption != gr.Properties.Caption)
                    {
                        gridcontrol.Rows[i].Appearance.ForeColor = System.Drawing.Color.Blue;
                    }
                }
                gr.Appearance.ForeColor = System.Drawing.Color.Red;
                //清除图表数据，重新载入数据
                for (int i = 0; i < StatisticsChart_chart1.Series.Count; i++)
                {
                    StatisticsChart_chart1.Series.RemoveAt(i);
                }
                //根据tag加载列数据
                if (gr.Tag!=null)
                {
                    var data = new string[gridview.RowCount];
                    if (gridview.Columns[gr.Properties.Caption].ColumnType.IsValueType)
                    {
                        double[] doubledata = gr.Tag as double[];
                        for (int i = 0; i < doubledata.Count(); i++)
                        {
                            data[i] = doubledata[i].ToString();
                        }
                    }
                    else
                    {
                        data = gr.Tag as string[];
                    }
                    List<string> datalist = data.ToList();
                    //获取唯一值
                    List<string> UniqueValue = data.Distinct().ToList();
                    int uniquecount = UniqueValue.Count;
                    //创建图表
                    Series series1 = new Series("", ViewType.Bar);

                    for (int i = 0; i < uniquecount; i++)
                    {
                        series1.Points.Add(new SeriesPoint(UniqueValue[i], (datalist.FindAll(value => value.Contains(UniqueValue[i]))).Count));
                    }
                    StatisticsChart_chart1.Series.Add(series1);

                }
            }
        }



    }
}