using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Finder.util;


namespace Finder.Forms
{
    public partial class Datachart : Form
    {
        public Datachart()
        {
            InitializeComponent();
        }

        DataBaseServer.MySqlCmd cmd = new DataBaseServer.MySqlCmd();

        private void Datachart_Load(object sender, EventArgs e)
        {
            kidlist.SelectedIndex = 4;  //事件类型 (默认选择全部)
            kwlist.SelectedIndex = 0;   //事件名称 (启动时隐藏)

            dateTimePicker1.Value = DateTime.Now.AddDays(-30);

            #region 提取事件与关键字
            DataTable kwdtAll = cmd.GetTabel("select name, keyword from keywords");
            for (int i = 0; i < kwdtAll.Rows.Count; i++)
            {
                string key = kwdtAll.Rows[i]["name"].ToString();
                if (!dicKeywords.ContainsKey(key))
                {
                    List<string> keywords = new List<string>();
                    keywords.Add(kwdtAll.Rows[i]["keyword"].ToString());
                    dicKeywords.Add(key, keywords);
                }
                else
                {
                    dicKeywords[key].Add(kwdtAll.Rows[i]["keyword"].ToString());
                }
            }
            #endregion

        }

        private void allKwChart_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            HitTestResult htr = chart10.HitTest(e.X, e.Y);
            //int time = 0;
            if (htr.PointIndex < 0)
            {
                return;
            }
            string keyword = "";
            keyword = chart10.Series["Series"].Points[htr.PointIndex].AxisLabel;

            //ChartDetailData cdd = new ChartDetailData(0, keyword, "全网", time, "U");
            ChartDetailData cdd = new ChartDetailData(keyword, "全网", "K", DataSelected);
            cdd.ShowDialog();
        }

        //private void allWebChart_MouseDoubleClick(object sender, MouseEventArgs e)
        //{
        //    HitTestResult htr = chart10.HitTest(e.X, e.Y);
        //    int time = 0;
        //    if (htr.PointIndex < 0)
        //    {
        //        return;
        //    }
        //    string webaddress = "";
        //    webaddress = chart20.Series["Series"].Points[htr.PointIndex].AxisLabel;

        //    ChartDetailData cdd = new ChartDetailData(0, webaddress, "全网", time, "D");
        //    cdd.ShowDialog();
        //}

        private void kidlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            string kid = kidlist.SelectedIndex.ToString();
            if (kid == "4")
            {
                //kwlist.Hide();
                //label8.Hide();
                kwlist.SelectedIndex = kwlist.Items.Count - 1;
            }
            else
            {
                string sql = "select uid , name from keywords where kid = '" + kid + "' group by name";
                DataTable dt = cmd.GetTabel(sql);

                kwlist.DisplayMember = "name";
                kwlist.ValueMember = "uid";

                DataRow dr = dt.NewRow();
                dr["name"] = "全部";
                dr["uid"] = "0";

                dt.Rows.Add(dr);

                kwlist.DataSource = dt;
                kwlist.SelectedIndex = kwlist.Items.Count - 1;
                kwlist.Show();
                label8.Show();
            }
        }

        private DataTable DataSelected = null;
        Dictionary<string, List<string>> dicKeywords = new Dictionary<string, List<string>>();
        private void button1_Click(object sender, EventArgs e)
        {
            string baseSql = @"select ifnull(c.FocusLevel,'99') FocusLevel, ifnull(c.ActionDate, '') ActionDate, b.Name as EventName, 
                                    a.uid,a.title,a.contexts,a.releasedate,a.infosource,a.keywords,a.releasename,a.collectdate,a.snapshot,a.webname,
                                    a.pid,a.part,a.reposts,a.comments,a.kid,a.sheng,a.shi,a.xian,a.deleted
                                    from releaseinfo a  left join keywords b on a.keywords=b.KeyWord 
                                    inner join FilterReleaseInfo c on a.uid=c.uid
                                    where a.deleted=0 and a.uid > 0 and b.Name is not null ";

            //事件类别
            if (kidlist.SelectedIndex != kidlist.Items.Count - 1)
            {
                baseSql += " and a.kid = " + kidlist.SelectedIndex.ToString();
                if (kwlist.SelectedIndex != kwlist.Items.Count - 1)
                {
                    string eventName = ((DataRowView)kwlist.SelectedItem)["name"].ToString();
                    if (eventName != "全部" && eventName != "")
                    {
                        if (dicKeywords.ContainsKey(eventName))
                        {
                            if (dicKeywords[eventName] != null)
                            {
                                baseSql += " and a.keywords in(";
                                foreach (var keyword in dicKeywords[eventName])
                                {
                                    baseSql += "'" + keyword + "',";
                                }
                                baseSql = baseSql.Substring(0, baseSql.Length - 1);
                                baseSql += ")";
                            }
                        }
                    }
                }
            }

            string sql = baseSql + " and a.collectdate  BETWEEN '" + dateTimePicker1.Value.ToString("yyyy-MM-dd 00:00:00") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd 23:59:59") + "'";
            sql += " order by FocusLevel, ActionDate desc, a.collectdate desc";

            DataTable dt = cmd.GetTabel(sql);

            #region 默认精确匹配
            List<DataRow> remove = new List<DataRow>();
            foreach (DataRow row in dt.Rows)
            {
                string keywords = row["keywords"].ToString();
                string title = row["title"].ToString();
                string context = row["contexts"].ToString();
                if (!string.IsNullOrEmpty(keywords))
                {
                    bool isFundTitle = true;
                    bool isFundContext = true;
                    string[] keyw = keywords.Split(' ');
                    if (keyw != null && keyw.Count() > 0)
                    {
                        foreach (string key in keyw)
                        {
                            if (title.IndexOf(key) < 0)
                            {
                                isFundTitle = false;
                            }
                            if (context.IndexOf(key) < 0)
                            {
                                isFundContext = false;
                            }
                        }
                    }
                    if (!isFundTitle && !isFundContext)
                    {
                        //如果标题或者内容没有匹配全部关键字则去掉该条数据
                        remove.Add(row);
                    }
                }
            }
            if (remove != null && remove.Count > 0)
            {
                foreach (DataRow row in remove)
                {
                    dt.Rows.Remove(row);
                }
            }
            #endregion

            DataSelected = dt;

            string charttitle = ""; ;
            Dictionary<string, int> dicTotal = new Dictionary<string, int>();
            if (kidlist.SelectedIndex != kidlist.Items.Count - 1 && kwlist.SelectedIndex != kwlist.Items.Count - 1)
            {
                //选择了具体的事件，分析事件下的关键字。
                foreach (DataRow r in dt.Rows)
                {
                    string kw = r["keyWords"].ToString();
                    if (!dicTotal.ContainsKey(kw))
                    {
                        dicTotal.Add(kw, 1);
                    }
                    else
                    {
                        dicTotal[kw] = dicTotal[kw] + 1;
                    }
                }
                charttitle = string.Format("统计事件[{0}]的数据", kwlist.Text);
            }
            else
            {
                //没有选择具体事件，按事件进行统计
                foreach (DataRow r in dt.Rows)
                {
                    string kw = r["EventName"].ToString();
                    if (string.IsNullOrEmpty(kw)) kw = "未知（空）";
                    if (!dicTotal.ContainsKey(kw))
                    {
                        dicTotal.Add(kw, 1);
                    }
                    else
                    {
                        dicTotal[kw] = dicTotal[kw] + 1;
                    }
                }
                charttitle = string.Format("按事件统计", kwlist.Text);
            }

            DataTable dtTotal = new DataTable();
            DataColumn col1 = new DataColumn();
            col1.ColumnName = "columnname";
            col1.DataType = typeof(string);
            dtTotal.Columns.Add(col1);

            DataColumn col2 = new DataColumn();
            col2.ColumnName = "columndata";
            col2.DataType = typeof(int);
            dtTotal.Columns.Add(col2);
            foreach (var k in dicTotal)
            {
                DataRow r = dtTotal.NewRow();
                r["columnname"] = k.Key;
                r["columndata"] = k.Value;

                dtTotal.Rows.Add(r);
            }
            dtTotal.AcceptChanges();
            ViewChart(dtTotal, charttitle);

            //近30天的数据统计
            int day = 30;
            string time = DateTime.Now.AddDays(0 - day).ToString("yyyy-MM-dd HH:mm:ss");
            string sql2 = baseSql + " and a.collectdate  > '" + time + "'";
            DataTable dt2 = cmd.GetTabel(sql2);
            #region 默认精确匹配
            remove = new List<DataRow>();
            foreach (DataRow row in dt2.Rows)
            {
                string keywords = row["keywords"].ToString();
                string title = row["title"].ToString();
                string context = row["contexts"].ToString();
                if (!string.IsNullOrEmpty(keywords))
                {
                    bool isFundTitle = true;
                    bool isFundContext = true;
                    string[] keyw = keywords.Split(' ');
                    if (keyw != null && keyw.Count() > 0)
                    {
                        foreach (string key in keyw)
                        {
                            if (title.IndexOf(key) < 0)
                            {
                                isFundTitle = false;
                            }
                            if (context.IndexOf(key) < 0)
                            {
                                isFundContext = false;
                            }
                        }
                    }
                    if (!isFundTitle && !isFundContext)
                    {
                        //如果标题或者内容没有匹配全部关键字则去掉该条数据
                        remove.Add(row);
                    }
                }
            }
            if (remove != null && remove.Count > 0)
            {
                foreach (DataRow row in remove)
                {
                    dt2.Rows.Remove(row);
                }
            }
            #endregion
            Dictionary<string, Dictionary<string, int>> dicTotal2 = new Dictionary<string, Dictionary<string, int>>();
            if (kidlist.SelectedIndex != kidlist.Items.Count - 1 && kwlist.SelectedIndex != kwlist.Items.Count - 1)
            {
                //选择了具体的事件，分析事件下的关键字。
                foreach (DataRow r in dt2.Rows)
                {
                    string kw = r["keyWords"].ToString();
                    string date = r["collectdate"].ToString();
                    if (date.Length > 10) date = date.Substring(0, 10);
                    if (!dicTotal2.ContainsKey(kw))
                    {
                        dicTotal2.Add(kw, new Dictionary<string, int>());
                        dicTotal2[kw].Add(date, 1);
                    }
                    else
                    {
                        if (!dicTotal2[kw].ContainsKey(date))
                        {
                            dicTotal2[kw].Add(date, 1);
                        }
                        else
                        {
                            dicTotal2[kw][date] = dicTotal2[kw][date] + 1;
                        }

                    }
                }
                charttitle = string.Format("统计事件[{0}]的数据", kwlist.Text);
            }
            else
            {
                //没有选择具体事件，按事件进行统计
                foreach (DataRow r in dt2.Rows)
                {
                    string kw = r["EventName"].ToString();
                    if (string.IsNullOrEmpty(kw)) kw = "未知（空）";
                    string date = r["collectdate"].ToString();
                    if (date.Length > 10) date = date.Substring(0, 10);
                    if (!dicTotal2.ContainsKey(kw))
                    {
                        dicTotal2.Add(kw, new Dictionary<string, int>());
                        dicTotal2[kw].Add(date, 1);
                    }
                    else
                    {
                        if (!dicTotal2[kw].ContainsKey(date))
                        {
                            dicTotal2[kw].Add(date, 1);
                        }
                        else
                        {
                            dicTotal2[kw][date] = dicTotal2[kw][date] + 1;
                        }

                    }
                }
                charttitle = string.Format("按事件统计", kwlist.Text);
            }

            this.chart20.Legends.Clear();
            this.chart20.Legends.Add(new Legend("Stores")); //右边标签列
            this.chart20.Legends["Stores"].Alignment = StringAlignment.Center;
            this.chart20.Legends["Stores"].HeaderSeparator = System.Windows.Forms.DataVisualization.Charting.LegendSeparatorStyle.ThickLine;

            chart20.Series.Clear();
            foreach (var k in dicTotal2)
            {
                Series s = new Series();
                s.ChartType = SeriesChartType.Line;

                string[] x = new string[day];
                int[] y = new int[day];
                for (int d = 0; d < day; d++)
                {
                    string date = DateTime.Now.AddDays(0 - day + d + 1).ToString("yyyy-MM-dd");
                    x[d] = date.Substring(5).Replace("-", "/");
                    y[d] = 0;
                    if (k.Value.ContainsKey(date))
                    {
                        y[d] = k.Value[date];
                    }
                }
                s.Points.DataBindXY(x, y);

                s.LegendText = k.Key;
                chart20.Series.Add(s);
            }


            Dictionary<string, int> dicTotal3 = new Dictionary<string, int>();
            //选择了具体的事件，分析事件下的关键字。
            foreach (DataRow r in dt.Rows)
            {
                string kw = r["keyWords"].ToString();
                if (!dicTotal3.ContainsKey(kw))
                {
                    dicTotal3.Add(kw, 1);
                }
                else
                {
                    dicTotal3[kw] = dicTotal3[kw] + 1;
                }
            }

            int l = dicTotal3.Count;
            string[] xx = new string[l];
            int[] yy = new int[l];
            int i = 0;
            foreach (var k in dicTotal3)
            {
                xx[i] = k.Key;
                yy[i] = k.Value;
                i++;
            }
            chart10.Series[0].Points.DataBindXY(xx, yy);

        }

        //饼图   dt数据结构为 columndata(数据)  columnname(文本) 这两列
        private void ViewChart(DataTable _dt, string _title)
        {
            this.chart1.Series.Clear();
            this.chart1.Legends.Clear();

            this.chart1.Series.Add(new Series("Data"));
            this.chart1.Legends.Add(new Legend("Stores")); //右边标签列

            this.chart1.Series["Data"].ChartType = SeriesChartType.Pie;
            this.chart1.Series["Data"]["PieLabelStyle"] = "Outside";//Inside 数值显示在圆饼内 Outside 数值显示在圆饼外 Disabled 不显示数值

            this.chart1.Series["Data"]["PieLineColor"] = "Black";


            //this.ct_coll.Series["Data"].IsValueShownAsLabel = true;
            //this.ct_coll.Series["Data"].IsVisibleInLegend = true;
            //this.ct_coll.Series["Data"].ShadowOffset = 1;//阴影偏移量

            this.chart1.Series["Data"].ToolTip = "#VAL{D} 次";//鼠标移动到上面显示的文字
            this.chart1.Series["Data"].BackSecondaryColor = Color.DarkCyan;
            this.chart1.Series["Data"].BorderColor = Color.DarkOliveGreen;
            this.chart1.Series["Data"].LabelBackColor = Color.Transparent;

            foreach (DataRow dr in _dt.Rows)
            {
                int ptIdx = this.chart1.Series["Data"].Points.AddY(Convert.ToDouble(dr["columndata"].ToString()));
                DataPoint pt = this.chart1.Series["Data"].Points[ptIdx];
                pt.AxisLabel = dr["columnname"].ToString();
                pt.LegendText = dr["columnname"].ToString() + " " + "#PERCENT{P2}" + " [ " + "#VAL{D} 条" + " ]";//右边标签列显示的文字
                pt.Label = dr["columnname"].ToString() + " " + "#PERCENT{P2}" + " [ " + "#VAL{D} 条" + " ]"; //圆饼外显示的信息

                //  pt.LabelToolTip = "#PERCENT{P2}";
                //pt.LabelBorderColor = Color.Red;//文字背景色 
            }

            // this.ct_coll.Series["Data"].Label = "#PERCENT{P2}"; //
            this.chart1.Series["Data"].Font = new Font("Segoe UI", 8.0f, FontStyle.Bold);
            this.chart1.Series["Data"].Legend = "Stores"; //右边标签列显示信息
            this.chart1.Legends["Stores"].Alignment = StringAlignment.Center;
            this.chart1.Legends["Stores"].HeaderSeparator = System.Windows.Forms.DataVisualization.Charting.LegendSeparatorStyle.ThickLine;


            this.chart1.Titles[0].Text = _title;
            this.chart1.ChartAreas["ChartArea1"].AxisX.IsMarginVisible = false;
            this.chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;

            int int_count = _dt.AsEnumerable().Select(t => t.Field<int>("columndata")).Sum();
            this.chart1.Titles[1].Text = string.Format("总条数: {0} 条", int_count);

            /*
                #VALX      显示当前图例的X轴的对应文本(或数据)
    #VAL, #VALY,  显示当前图例的Y轴的对应文本(或数据)
    #VALY2, #VALY3, 显示当前图例的辅助Y轴的对应文本(或数据)
    #SER:      显示当前图例的名称
    #LABEL       显示当前图例的标签文本
    #INDEX      显示当前图例的索引
    #PERCENT       显示当前图例的所占的百分比
    #TOTAL      总数量
    #LEGENDTEXT      图例文本
                */
        }

        private void chart1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            HitTestResult htr = chart1.HitTest(e.X, e.Y);
            //int time = 0;
            if (htr.PointIndex < 0)
            {
                return;
            }
            string keyword = "";
            keyword = chart1.Series["Data"].Points[htr.PointIndex].AxisLabel;
            string KE = "E";
            if (kidlist.SelectedIndex != kidlist.Items.Count - 1 && kwlist.SelectedIndex != kwlist.Items.Count - 1)
            {
                KE = "K";
            }
            else
            {
                KE = "E";
            }

            ChartDetailData cdd = new ChartDetailData(keyword, "全网", KE, DataSelected);
            cdd.ShowDialog();

        }

    }
}
