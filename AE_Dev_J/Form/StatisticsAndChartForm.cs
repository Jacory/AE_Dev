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
        public GridView gridview = null;//当前属性表
        public string tablename = null;//当前属性表名

        public StatisticsAndChartForm(GridView gv,string name)
        {
            InitializeComponent();
            gridview = gv;
            tablename = name;
        }

        private void StatisticsAndChartForm_Load(object sender, EventArgs e)
        {
            //更改窗体名称

            this.Text ="Statistics And Chart For "+tablename;
            //生成统计表格
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
                        if (gridview.GetRowCellValue(j, field.Properties.Caption).ToString() == "" 
                            || gridview.GetRowCellValue(j, field.Properties.Caption) == null
                            || gridview.GetRowCellValue(j, field.Properties.Caption).ToString() == " ")
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

                    //将数值类型字段添加到combox中
                    comboBoxEdit1.Properties.Items.Add(gridview.Columns[i].FieldName) ;

                }
                    //列类型不为数值型
                else
                {
                    //将该列的数据存储进数组
                    var data = new string[gridview.RowCount];
                    EditorRow row_Nulls = new EditorRow();
                    row_Nulls.Properties.Caption = "Nulls";
                    //计算空值个数
                    int count = 0;
                    for (int n = 0; n < gridview.RowCount; n++)
                    {
                        if (gridview.GetRowCellValue(n, field.Properties.Caption).ToString() == "" 
                            || gridview.GetRowCellValue(n, field.Properties.Caption) == null
                            ||gridview.GetRowCellValue(n, field.Properties.Caption).ToString()==" ")
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
                StatisticsAndChart_vGridControl.Rows.AddRange(new BaseRow[] { field });
            }

        }
        /// <summary>
        /// 单击VGridview生成图表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatisticsAndChart_vGridControl_MouseClick(object sender, MouseEventArgs e)
        {
            DevExpress.XtraVerticalGrid.VGridControl gridcontrol = new DevExpress.XtraVerticalGrid.VGridControl();
            gridcontrol = sender as DevExpress.XtraVerticalGrid.VGridControl;
            VGridHitInfo info = gridcontrol.CalcHitInfo(new Point(e.X,e.Y));
            //当左键单击HeaderCell时
            if (info.HitInfoType==HitInfoTypeEnum.HeaderCell
                && info.Row.HasChildren 
                && e.Button == MouseButtons.Left
                && info.Row.Properties.Caption.IndexOf('*') != 0)
            {
                BaseRow baserow = gridcontrol.FocusedRow;
                CategoryRow gr = baserow as CategoryRow;
                //根据tag加载列数据
                if (gr!=null&&gr.Tag!=null)
                {
                    string[] data =null;
                    //判断字段类型是否为数值型
                    if (gridview.Columns[gr.Properties.Caption].ColumnType.IsValueType)
                    {
                        //读取tag中的数据至数组
                        double[] doubledata = gr.Tag as double[];
                        List<double> doublelist = doubledata.ToList();
                        //计算唯一值及唯一值个数
                        List<double> d_uniquevalue = doubledata.Distinct().ToList();
                        int d_uniquecount = d_uniquevalue.Count;
                        //当唯一值个数大于30时，执行分组处理
                        if (d_uniquecount>30)
                        {
                            //计算分组间距
                            double interval = (d_uniquevalue.Max() - d_uniquevalue.Min())/25;
                            //计算图表中加入点的值
                            List<string> chart_uniquevalue = new List<string>();
                            for (int n   = 0; n < 26; n++)
                            {
                                if (n==25)
                                {
                                    chart_uniquevalue.Add((interval * n).ToString("0.0") + "-" + d_uniquevalue.Max().ToString("0.0"));
                                }
                                else
                                {
                                    chart_uniquevalue.Add((interval * n).ToString("0.0") + "-" + (interval * (n + 1)).ToString("0.0"));
                                }
                            }
                            double max = d_uniquevalue.Max();
                            //chart_uniquevalue.Add((d_uniquevalue.Max()).ToString());
                            //声明26个list类型的数组
                            List<double>[] findlist = new List<double>[26];
                            for (int n = 0; n < 26; n++)
                            {
                                findlist[n] = new List<double>();
                            }
                            //根据分组间距，将所有数据存放到对应的26个list中
                            for (int n = 0; n < doublelist.Count; n++)
                            {
                                findlist[(int)(doublelist[n] / interval)].Add(doublelist[n]);
                            }
                            //完成分组
                            //清除图表数据，重新载入数据
                            for (int i = 0; i < StatisticsChart_chart1.Series.Count; i++)
                            {
                                StatisticsChart_chart1.Series.RemoveAt(i);
                            }
                            //创建图表
                            Series series = new Series(gr.Properties.Caption+"_Count", ViewType.Bar);
                            for (int n = 0; n < 26; n++)
                            {
                                series.Points.Add(new SeriesPoint(chart_uniquevalue[n].ToString(), findlist[n].Count));
                            }
                            StatisticsChart_chart1.Series.Add(series);
                        }
                        else
                        {
                            data = new string[gridview.RowCount];
                            for (int i = 0; i < doubledata.Count(); i++)
                            {
                                data[i] = doubledata[i].ToString();
                            }
                        }
                    }
                    else
                    {
                        data = new string[gridview.RowCount];
                        data = gr.Tag as string[];
                    }
                    if (data!=null)
                    {
                        List<string> datalist = data.ToList();
                        //获取唯一值
                        List<string> UniqueValue = data.Distinct().ToList();
                        int uniquecount = UniqueValue.Count;
                        if ((uniquecount <= 300)||(uniquecount >300 && (MessageBox.Show("分类数较多，是否继续？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)))
                        {
                            //清除图表数据，重新载入数据
                            for (int i = 0; i < StatisticsChart_chart1.Series.Count; i++)                                           
                            {
                                StatisticsChart_chart1.Series.RemoveAt(i);
                            }
                            //创建图表
                            Series series1 = new Series("", ViewType.Bar);
                            for (int i = 0; i < uniquecount; i++)
                            {
                                series1.Points.Add(new SeriesPoint(UniqueValue[i], (datalist.FindAll(value => value.Contains(UniqueValue[i]))).Count));
                            }
                            StatisticsChart_chart1.Series.Add(series1);
                        }
                    }
                    //生成X坐标轴名称
                    XYDiagram diagram = (XYDiagram)StatisticsChart_chart1.Diagram;
                    diagram.AxisX.Title.Visible = true;
                    diagram.AxisX.Title.Alignment = StringAlignment.Center;
                    diagram.AxisX.Title.Text = info.Row.Properties.Caption;
                    //生成Y坐标轴名称
                    diagram.AxisY.Title.Visible = true;
                    diagram.AxisY.Title.Alignment = StringAlignment.Center;
                    diagram.AxisY.Title.Text = "Count";
                }
            }
        }
        /// <summary>
        /// 单击DockPanelHeaderButton，折叠或展开统计信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatisticsTable_CustomButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Caption=="折叠")
            {
                for (int i = 0; i < StatisticsAndChart_vGridControl.Rows.Count; i++)
                {
                    StatisticsAndChart_vGridControl.Rows[i].Expanded = false;
                }
            }
            if (e.Button.Properties.Caption=="展开")
            {
                for (int i = 0; i < StatisticsAndChart_vGridControl.Rows.Count; i++)
                {
                    StatisticsAndChart_vGridControl.Rows[i].Expanded = true;
                }
            }
        }
        /// <summary>
        /// 根据combox所选字段名，重新生成图表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //清除图表数据，重新载入数据
            for (int i = 0; i < StatisticsChart_chart2.Series.Count; i++)
            {
                StatisticsChart_chart2.Series.RemoveAt(i);
            }
            Series series = new Series("", ViewType.Bar);
            for (int i = 0; i < gridview.RowCount; i++)
            {
                series.Points.Add(new SeriesPoint(gridview.GetRowCellValue(i, "*FID"), gridview.GetRowCellValue(i, comboBoxEdit1.Text)));
            }
            StatisticsChart_chart2.Series.Add(series);
            //生成X坐标轴名称
            XYDiagram diagram = (XYDiagram)StatisticsChart_chart2.Diagram;
            diagram.AxisX.Title.Visible = true;
            diagram.AxisX.Title.Alignment = StringAlignment.Center;
            diagram.AxisX.Title.Text = "*FID";
            //生成Y坐标轴名称
            diagram.AxisY.Title.Visible = true;
            diagram.AxisY.Title.Alignment = StringAlignment.Center;
            diagram.AxisY.Title.Text = comboBoxEdit1.SelectedText;
        }

    }
}