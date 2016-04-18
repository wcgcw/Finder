using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataBaseServer;
using System.IO;
using System.Text.RegularExpressions;
using Finder.util;

namespace Finder.Forms
{
    public partial class Filterdata : Form
    {
        DataBaseServer.SQLitecommand cmd = new DataBaseServer.SQLitecommand();

        Dictionary<string, List<string>> dicKeywords = new Dictionary<string, List<string>>();

        public Filterdata()
        {
            InitializeComponent();
        }

        private void Filterdata_Load(object sender, EventArgs e)
        {
            kidlist.SelectedIndex = 4;  //事件类型 (默认选择全部)
            kwlist.SelectedIndex = 0;   //事件名称 (启动时隐藏)
            kwlist.Hide();  //事件名称
            label8.Hide();  //事件名称

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

            dateTimePicker1.Value = DateTime.Now.AddDays(-30);

            label3.Hide();      //选择网站
            weblist.Hide();     //选择网站
            shilist.Hide();     //市
            xianlist.Hide();    //县

            #region 省
            shenglist.DataSource = null;
            shenglist.Items.Clear();

            string sql = "select id , name from area where lvl = 1";
            DataTable dt = cmd.GetTabel(sql);

            shenglist.DisplayMember = "name";
            shenglist.ValueMember = "id";

            DataRow dr = dt.NewRow();
            dr["name"] = "全部";
            dr["id"] = "0";

            dt.Rows.Add(dr);

            shenglist.DataSource = dt;
            shenglist.SelectedIndex = shenglist.Items.Count - 1;
            #endregion

            #region 网站类别 pid
            string softVer = GlobalPars.GloPars.ContainsKey("SoftVer") ? GlobalPars.GloPars["SoftVer"].ToString() : "1";
            if (softVer.Equals("1") || softVer.Equals("2"))
            {
                sql = "select pid , name from pid where pid != 6";
            }
            else
            {
                sql = "select pid , name from pid";
            }
            DataTable pidDt = cmd.GetTabel(sql);
            pidlist.DisplayMember = "name";
            pidlist.ValueMember = "pid";
            dr = pidDt.NewRow();
            dr["name"] = "全部";
            dr["pid"] = "8";
            pidDt.Rows.Add(dr);
            pidlist.DataSource = pidDt;
            pidlist.SelectedIndex = pidlist.Items.Count - 1;
            #endregion

            lblSetTop.Visible = false;
            lblCancel.Visible = false;
            lblFocus.Visible = false;
            lblMainFocus.Visible = false;
            lblDelete.Visible = false;
            lblPre.Visible = false;
            lblNext.Visible = false;
            panel4.Visible = false;

            lblSetTop.ForeColor = Color.Blue;
            lblCancel.ForeColor = Color.FromArgb(64, 64, 64);
            lblFocus.ForeColor = Color.Green;
            lblMainFocus.ForeColor = Color.Red;
            lblDelete.ForeColor = Color.FromArgb(64, 64, 64);

            txtTitle.Text = "";
            lblCount.Text = "";
            txtTitle.BorderStyle = BorderStyle.None;

        }

        private void GetResultData()
        {
            string sql = @"select ifnull(c.[FocusLevel],'99') FocusLevel, ifnull(c.[ActionDate], '') ActionDate, b.[Name] as EventName, 
                                    a.uid,a.title,a.contexts,a.releasedate,a.infosource,a.keywords,a.releasename,a.collectdate,a.snapshot,a.webname,
                                    a.pid,a.part,a.reposts,a.comments,a.kid,a.sheng,a.shi,a.xian,a.deleted
                                    from releaseinfo a  left join keywords b on a.keywords=b.[KeyWord] 
                                    left join FilterReleaseInfo c on a.uid=c.uid
                                    where a.deleted=0 and a.uid > 0";

            #region 拼接sql的条件
            if (searchTxt.Text.ToString().Length > 0)
            {
                sql += " and a.contexts like '%" + searchTxt.Text.ToString() + "%'";
            }
            //事件类别
            if (pidlist.SelectedIndex != pidlist.Items.Count - 1)
            {
                sql += " and a.pid = " + ((DataRowView)pidlist.SelectedItem)["pid"].ToString();
            }

            if (kidlist.SelectedIndex != kidlist.Items.Count - 1)
            {
                sql += " and a.kid = " + kidlist.SelectedIndex.ToString();
                if (kwlist.SelectedIndex != kwlist.Items.Count - 1)
                {
                    string eventName = ((DataRowView)kwlist.SelectedItem)["name"].ToString();
                    if (eventName != "全部" && eventName != "")
                    {
                        if (dicKeywords.ContainsKey(eventName))
                        {
                            if (dicKeywords[eventName] != null)
                            {
                                sql += " and a.keywords in(";
                                foreach (var keyword in dicKeywords[eventName])
                                {
                                    sql += "'" + keyword + "',";
                                }
                                sql = sql.Substring(0, sql.Length - 1);
                                sql += ")";
                            }
                        }
                    }
                }
            }

            if (((DataRowView)shenglist.SelectedItem)["id"].ToString() != "0")
            {
                sql += " and a.sheng = '" + ((DataRowView)shenglist.SelectedItem)["name"].ToString() + "'";

                if (((DataRowView)shilist.SelectedItem)["id"].ToString() != "0")
                {
                    sql += " and a.shi = '" + ((DataRowView)shilist.SelectedItem)["name"].ToString() + "'";

                    if (((DataRowView)xianlist.SelectedItem)["id"].ToString() != "0")
                    {
                        sql += " and a.xian = '" + ((DataRowView)xianlist.SelectedItem)["name"].ToString() + "'";
                    }
                }
            }

            sql += " and a.collectdate  BETWEEN '" + dateTimePicker1.Value.ToString("yyyy-MM-dd 00:00:00") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd 23:59:59") + "'";
            sql += " and b.[Name] is not null  order by FocusLevel, ActionDate desc, a.collectdate desc";
            #endregion

            DataTable dt = cmd.GetTabel(sql);

            #region 2015.7.14 修改成默认精确匹配
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

            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = dt;

            this.dataGridView1.SelectionChanged -= new System.EventHandler(this.dataGridView1_SelectionChanged);
            FormatDataView();
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);

            lblCount.Text = string.Format("共计检索到 {0} 条结果", dt.Rows.Count);
        }

        private DataTable MergeTable(DataTable dt1, DataTable dt2)
        {

            DataTable DataMerge = new DataTable();
            DataColumn col = new DataColumn();
            col.ColumnName = "dataSource";
            col.DataType = typeof(int);
            DataMerge.Columns.Add(col);

            col = new DataColumn();
            col.ColumnName = "dataSort";
            col.DataType = typeof(int);
            DataMerge.Columns.Add(col);

            col = new DataColumn();
            col.ColumnName = "EventName";
            col.DataType = typeof(string);
            DataMerge.Columns.Add(col);

            col = new DataColumn();
            col.ColumnName = "uid";
            col.DataType = typeof(int);
            DataMerge.Columns.Add(col);

            col = new DataColumn();
            col.ColumnName = "title";
            col.DataType = typeof(string);
            DataMerge.Columns.Add(col);

            col = new DataColumn();
            col.ColumnName = "contexts";
            col.DataType = typeof(string);
            DataMerge.Columns.Add(col);

            col = new DataColumn();
            col.ColumnName = "releasedate";
            col.DataType = typeof(string);
            DataMerge.Columns.Add(col);

            col = new DataColumn();
            col.ColumnName = "infosource";
            col.DataType = typeof(string);
            DataMerge.Columns.Add(col);

            col = new DataColumn();
            col.ColumnName = "keywords";
            col.DataType = typeof(string);
            DataMerge.Columns.Add(col);

            col = new DataColumn();
            col.ColumnName = "releasename";
            col.DataType = typeof(string);
            DataMerge.Columns.Add(col);

            col = new DataColumn();
            col.ColumnName = "collectdate";
            col.DataType = typeof(string);
            DataMerge.Columns.Add(col);

            col = new DataColumn();
            col.ColumnName = "snapshot";
            col.DataType = typeof(string);
            DataMerge.Columns.Add(col);
            col = new DataColumn();
            col.ColumnName = "webname";
            col.DataType = typeof(string);
            DataMerge.Columns.Add(col);
            col = new DataColumn();
            col.ColumnName = "pid";
            col.DataType = typeof(string);
            DataMerge.Columns.Add(col);
            col = new DataColumn();
            col.ColumnName = "part";
            col.DataType = typeof(string);
            DataMerge.Columns.Add(col);
            col = new DataColumn();
            col.ColumnName = "reposts";
            col.DataType = typeof(string);
            DataMerge.Columns.Add(col);
            col = new DataColumn();
            col.ColumnName = "comments";
            col.DataType = typeof(string);
            DataMerge.Columns.Add(col);
            col = new DataColumn();
            col.ColumnName = "kid";
            col.DataType = typeof(string);
            DataMerge.Columns.Add(col);
            col = new DataColumn();
            col.ColumnName = "sheng";
            col.DataType = typeof(string);
            DataMerge.Columns.Add(col);
            col = new DataColumn();
            col.ColumnName = "shi";
            col.DataType = typeof(string);
            DataMerge.Columns.Add(col);
            col = new DataColumn();
            col.ColumnName = "xian";
            col.DataType = typeof(string);
            DataMerge.Columns.Add(col);
            col = new DataColumn();
            col.ColumnName = "deleted";
            col.DataType = typeof(string);
            DataMerge.Columns.Add(col);
            //memo,focuslevel,actiondate
            col = new DataColumn();
            col.ColumnName = "memo";
            col.DataType = typeof(string);
            DataMerge.Columns.Add(col);
            col = new DataColumn();
            col.ColumnName = "focuslevel";
            col.DataType = typeof(string);
            DataMerge.Columns.Add(col);
            col = new DataColumn();
            col.ColumnName = "actiondate";
            col.DataType = typeof(string);
            DataMerge.Columns.Add(col);

            return DataMerge;
        }

        //表格初始化，设定各单元格显示，居中，宽度
        private void FormatDataView()
        {
            dataGridView1.Columns.Add(new DataGridViewImageColumn() { HeaderText = "正负预判", Name = "part_img", DisplayIndex = 19 });
            dataGridView1.Columns.Add(new DataGridViewLinkColumn() { HeaderText = "标题", Name = "title_link", DisplayIndex = 4, Width = 160 });

            // 设置可见列、隐藏列的显示顺序和显示样式
            DataGridViewColumn[] array = new DataGridViewColumn[dataGridView1.Columns.Count];
            dataGridView1.Columns.CopyTo(array, 0);
            dataGridView1.Columns.Clear();

            int order = 12;
            foreach (DataGridViewColumn col in array)
            {
                switch (col.Name.ToLower())
                {
                    #region 调整列的隐藏与列序
                    case "focuslevel":
                        col.HeaderText = "关注度";
                        col.DisplayIndex = 0;
                        col.Width = 120;
                        break;
                    case "eventname":
                        col.HeaderText = "事件名称";
                        col.DisplayIndex = 1;
                        col.Width = 120;
                        break;
                    case "keywords":
                        col.HeaderText = "关键字";
                        col.DisplayIndex = 2;
                        col.Visible = false;
                        break;
                    case "title_link":
                        col.HeaderText = "标题";
                        col.DisplayIndex = 3;
                        col.Width = 260;
                        break;
                    case "contexts":
                        col.HeaderText = "内容";
                        col.DisplayIndex = 4;
                        col.Width = 480;
                        break;
                    case "webname":
                        col.HeaderText = "来源";
                        col.DisplayIndex = 5;
                        col.Width = 120;
                        break;
                    case "releasename":
                        col.HeaderText = "发布者";
                        col.DisplayIndex = 6;
                        col.Width = 160;
                        break;
                    case "releasedate":
                        col.HeaderText = "发布时间";
                        col.DisplayIndex = 7;
                        col.Width = 160;
                        break;
                    case "pid":
                        col.HeaderText = "网站类别";
                        col.DisplayIndex = 8;
                        col.Width = 80;
                        break;
                    case "kid":
                        col.HeaderText = "事件类别";
                        col.DisplayIndex = 9;
                        col.Width = 80;
                        break;
                    case "collectdate":
                        col.HeaderText = "抓取时间";
                        col.DisplayIndex = 10;
                        col.Width = 160;
                        break;
                    case "part_img":
                        col.HeaderText = "正负预判";
                        col.DisplayIndex = 11;
                        col.Width = 80;
                        break;
                    case "uid":
                        col.DisplayIndex = order++;
                        col.Visible = false;
                        break;
                    case "title":
                        col.HeaderText = "标题_txt";
                        col.DisplayIndex = order++;
                        col.Width = 260;
                        col.Visible = false;
                        break;
                    case "infosource":
                        col.HeaderText = "链接";
                        col.DisplayIndex = order++;
                        col.Visible = false;
                        break;
                    case "sheng":
                        col.HeaderText = "区域";
                        col.DisplayIndex = order++;
                        col.Width = 160;
                        col.Visible = false;
                        break;
                    case "shi":
                        col.HeaderText = "市";
                        col.DisplayIndex = order++;
                        col.Visible = false;
                        break;
                    case "xian":
                        col.HeaderText = "县";
                        col.DisplayIndex = order++;
                        col.Visible = false;
                        break;
                    case "reposts":
                        col.HeaderText = "转发量";
                        col.DisplayIndex = order++;
                        col.Visible = false;
                        break;
                    case "comments":
                        col.HeaderText = "评论数";
                        col.DisplayIndex = order++;
                        col.Visible = false;
                        break;
                    case "part":
                        col.HeaderText = "正负预判-txt";
                        col.DisplayIndex = order++;
                        col.Width = 80;
                        col.Visible = false;
                        break;
                    default:
                        col.DisplayIndex = order++;
                        col.Visible = false;
                        break;
                    #endregion
                }
            }
            dataGridView1.Columns.AddRange(array);
        }

        //表格内容格式化，设定正负向，类别，区域
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex != dataGridView1.NewRowIndex)
            {
                switch (dataGridView1.Columns[e.ColumnIndex].Name.ToLower())
                {
                    case "focuslevel":
                        string txtFocus = dataGridView1.Rows[e.RowIndex].Cells["focuslevel"].Value.ToString();
                        switch (txtFocus)
                        {
                            case "1":
                                e.Value = "置顶";
                                e.CellStyle.ForeColor = Color.Blue;
                                break;
                            case "2":
                                e.Value = "重点关注";
                                e.CellStyle.ForeColor = Color.Red;
                                break;
                            case "3":
                                e.Value = "关注";
                                e.CellStyle.ForeColor = Color.Green;
                                break;
                            case "99":
                                e.Value = "";
                                e.CellStyle.ForeColor = Color.Green;
                                break;
                        }
                        e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        break;
                    case "title_link":
                        //设定正负向
                        string title = dataGridView1.Rows[e.RowIndex].Cells["title"].Value.ToString();
                        e.Value = title;
                        break;
                    case "part_img":
                        //设定正负向
                        string part = dataGridView1.Rows[e.RowIndex].Cells["part"].Value.ToString();
                        string url = Application.StartupPath.ToString();
                        if (part.Equals("1"))
                        {
                            url += "\\icons\\u.png";
                        }
                        else
                        {
                            url += "\\icons\\d.png";
                        }
                        e.Value = File.ReadAllBytes(url);
                        break;
                    case "kid":
                        switch (dataGridView1.Rows[e.RowIndex].Cells["kid"].Value.ToString())
                        {
                            case "0":
                                e.Value = "常规舆情";
                                break;
                            case "1":
                                e.Value = "敏感舆情";
                                break;
                            case "2":
                                e.Value = "重点舆情";
                                break;
                            case "3":
                                e.Value = "突发舆情";
                                break;
                        }
                        break;
                    case "pid":
                        switch (dataGridView1.Rows[e.RowIndex].Cells["pid"].Value.ToString())
                        {
                            case "0":
                                e.Value = "全网";
                                break;
                            case "1":
                                e.Value = "博客";
                                break;
                            case "2":
                                e.Value = "论坛";
                                break;
                            case "3":
                                e.Value = "微博";
                                break;
                            case "4":
                                e.Value = "主流媒体";
                                break;
                            case "5":
                                e.Value = "贴吧";
                                break;
                            case "6":
                                e.Value = "微信";
                                break;
                            case "7":
                                e.Value = "定制";
                                break;
                        }
                        break;
                    case "sheng":
                        e.Value = dataGridView1.Rows[e.RowIndex].Cells["sheng"].Value.ToString() + dataGridView1.Rows[e.RowIndex].Cells["shi"].Value.ToString() + dataGridView1.Rows[e.RowIndex].Cells["xian"].Value.ToString();
                        break;
                }
            }
        }

        //表格选中事件，获取选中行的标题，关键字，链接，内容，拼合到下面的dataView里
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 0)
            {
                if (dataGridView1.CurrentCell == null) return;
                string title = dataGridView1.CurrentCell.OwningRow.Cells["title"].Value.ToString();

                //设置标题
                txtTitle.Text = "标题：" + title;
                txtTitle.Select(3, title.Length);
                txtTitle.SelectionFont = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold);

                //设置链接及正文
                dataView.Text = "链接：" + dataGridView1.CurrentCell.OwningRow.Cells["infosource"].Value.ToString() + "\n";
                //添加内容
                dataView.AppendText(dataGridView1.CurrentCell.OwningRow.Cells["contexts"].Value.ToString());

                //关键字
                string keyword = dataGridView1.CurrentCell.OwningRow.Cells["keywords"].Value.ToString();
                string[] keywords = keyword.Split(' ');
                foreach (string kw in keywords)
                {
                    int wl = kw.Length;
                    int start = 0;
                    while (start < dataView.Text.Length && dataView.Text.IndexOf(kw, start) > -1)
                    {
                        int kl = dataView.Text.IndexOf(kw, start);
                        if (kl >= 0)
                        {
                            dataView.Select(kl, wl);
                            dataView.SelectionFont = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold);
                            dataView.SelectionColor = Color.Red;
                            start += kl + wl;
                        }
                    }

                    start = 0;
                    while (start < txtTitle.Text.Length && txtTitle.Text.IndexOf(kw, start) > -1)
                    {
                        int kl = txtTitle.Text.IndexOf(kw, start);
                        if (kl >= 0)
                        {
                            txtTitle.Select(kl, wl);
                            txtTitle.SelectionFont = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold);
                            txtTitle.SelectionColor = Color.Red;
                            start += kl + wl;
                        }
                    }
                }
                dataView.Select(0, 0);
                txtTitle.Select(0, 0);

                #region 显示评论
                CommentView.Text = "";
                string sql = "select uid , CommentID, Comment, SubmitDate from UserComment where uid ={0} and Deleted = 0 order by SubmitDate desc";
                sql = string.Format(sql, dataGridView1.CurrentRow.Cells["uid"].Value.ToString());
                DataTable dt = cmd.GetTabel(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string date = row["SubmitDate"].ToString();
                        string comment = row["Comment"].ToString();

                        CommentView.Text += "时间：" + date + "\r\n";
                        CommentView.Text += "内容：" + comment + "\r\n";
                        CommentView.Text += "-----------------------------------\r\n";
                    }
                }
                #endregion

                //设置按钮
                FormateFoucsStatus();

            }
        }

        //表格点击事件，点击标题时打开浏览器
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && dataGridView1.Columns[e.ColumnIndex].Name == "title_link")
            {
                try
                {
                    System.Diagnostics.Process.Start(this.dataGridView1.Rows[e.RowIndex].Cells["infosource"].Value.ToString());
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            dataView.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            GetResultData();

            lblSetTop.Visible = true;
            lblCancel.Visible = true;
            lblFocus.Visible = true;
            lblMainFocus.Visible = true;
            lblDelete.Visible = true;
            lblPre.Visible = true;
            lblNext.Visible = true;
            panel4.Visible = true;

            FormateFoucsStatus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Execl 文件(*.xls)|*.xls";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.CreatePrompt = true;
            saveFileDialog.Title = "保存Execl文件至：";
            Stream myStream;
            StreamWriter sw;

            //导出时，只导出了可见列的数据
            string str = "";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = saveFileDialog.OpenFile()) != null)
                    {
                        using (sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding(-0)))
                        {
                            #region 表头
                            for (int i = 0; i < dataGridView1.ColumnCount; i++)
                            {
                                if (dataGridView1.Columns[i].Visible)
                                {
                                    str += dataGridView1.Columns[i].HeaderText;
                                    if (dataGridView1.Columns[i].Name == "title_link")
                                    {
                                        str += "\t" + dataGridView1.Columns["infosource"].HeaderText;
                                    }
                                    str += "\t";
                                }
                            }
                            sw.WriteLine(str);
                            #endregion
                            #region 数据行
                            for (int j = 0; j < dataGridView1.Rows.Count; j++)
                            {
                                string tempStr = "";
                                for (int k = 0; k < dataGridView1.Columns.Count; k++)
                                {
                                    if (dataGridView1.Columns[k].Visible)
                                    {
                                        if (dataGridView1.Columns[k].Name == "part_img")
                                        {
                                            if (dataGridView1.Rows[j].Cells["part"].Value.ToString().Equals("1"))
                                            {
                                                tempStr += "正向";
                                            }
                                            else
                                            {
                                                tempStr += "负向";
                                            }
                                        }
                                        else if (dataGridView1.Columns[k].Name == "title_link")
                                        {
                                            tempStr += dataGridView1.Rows[j].Cells["title"].Value.ToString();
                                            tempStr += "\t" + dataGridView1.Rows[j].Cells["infosource"].Value.ToString();
                                        }
                                        else
                                        {
                                            tempStr += dataGridView1.Rows[j].Cells[k].Value.ToString();
                                        }
                                        tempStr += "\t";
                                    }
                                }
                                sw.WriteLine(tempStr);
                            }
                            #endregion
                            sw.Close();
                            myStream.Close();
                            MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }
        }

        private void cms_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
                return;
            string temp = "";

            try
            {
                Entities.SystemSet ss = (Entities.SystemSet)GlobalPars.GloPars["systemset"];
                if (!Directory.Exists(ss.EvidenceImgSavePath))
                {
                    MessageBox.Show("保存证据的目录[" + ss.EvidenceImgSavePath + "]不存在，请先创建该路径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                temp = ss.EvidenceImgSavePath + "\\" + DateTime.Now.ToString().Replace(":", "-").Replace("/", "-").Replace(" ", "-") + ".jpg";

                Bitmap image = util.WebSnap.StartSnap(this.dataGridView1.SelectedRows[0].Cells["infosource"].Value.ToString());
                image.Save(temp);
                MessageBox.Show("证据保存成功！", "提示");
            }
            catch (Exception ex)
            {
                MessageBox.Show("目标网站原因,证据图片生成失败!");
            }
        }

        private void shenglist_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sheng = ((DataRowView)shenglist.SelectedItem)["id"].ToString();
            if (sheng == "0")
            {
                shilist.Hide();
                shilist.SelectedIndex = shilist.Items.Count - 1;
            }
            else
            {
                string sql = "select id , name from area where parent = '" + sheng + "'";
                DataTable dt = cmd.GetTabel(sql);

                shilist.DisplayMember = "name";
                shilist.ValueMember = "id";

                DataRow dr = dt.NewRow();
                dr["name"] = "全部";
                dr["id"] = "0";

                dt.Rows.Add(dr);

                shilist.DataSource = dt;
                shilist.SelectedIndex = shilist.Items.Count - 1;
                shilist.Show();
            }
        }

        private void shilist_SelectedIndexChanged(object sender, EventArgs e)
        {
            string shi = ((DataRowView)shilist.SelectedItem)["id"].ToString();
            if (shi == "0")
            {
                xianlist.Hide();
                xianlist.SelectedIndex = xianlist.Items.Count - 1;
            }
            else
            {
                string sql = "select id , name from area where parent = '" + shi + "'";
                DataTable dt = cmd.GetTabel(sql);

                xianlist.DisplayMember = "name";
                xianlist.ValueMember = "id";

                DataRow dr = dt.NewRow();
                dr["name"] = "全部";
                dr["id"] = "0";

                dt.Rows.Add(dr);

                xianlist.DataSource = dt;
                xianlist.SelectedIndex = xianlist.Items.Count - 1;
                xianlist.Show();
            }
        }

        private void kidlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            string kid = kidlist.SelectedIndex.ToString();
            if (kid == "4")
            {
                kwlist.Hide();
                label8.Hide();
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

        private bool DeleteData()
        {
            if (MessageBox.Show("确定要删除选定的数据吗？删除后将无法恢复", "警告", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                DataGridViewSelectedRowCollection dvs = dataGridView1.SelectedRows;
                StringBuilder sb = new StringBuilder("update ReleaseInfo set deleted=1 where uid in (");
                StringBuilder sb1 = new StringBuilder();
                List<string> removes = new List<string>();
                foreach (DataGridViewRow dr in dvs)
                {                    
                    string uid = dr.Cells["uid"].Value.ToString();
                    sb1.Append("," + uid);

                    removes.Add(uid);                        
                }

                string nextUid = "";
                if (removes.Count > 0)
                {
                    bool fund = false;
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (string.Compare(row.Cells["uid"].Value.ToString(), removes[0]) == 0)
                        {
                            fund = true;                            
                        }
                        if (fund)
                        {
                            if (string.Compare(row.Cells["uid"].Value.ToString(), removes[0]) != 0 && !removes.Contains(row.Cells["uid"].Value.ToString()))
                            {
                                nextUid = row.Cells["uid"].Value.ToString();
                                break;
                            }
                        }
                    }
                }

                sb.Append(sb1.Length > 1 ? sb1.ToString().Substring(1) : sb1.ToString());
                sb.Append(")");
                if (sb.Length > 48)
                {
                    cmd.ExecuteNonQueryInt(sb.ToString());
                }
                GetResultData();

                if (!string.IsNullOrEmpty(nextUid))
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (string.Compare(row.Cells["uid"].Value.ToString(), nextUid) == 0)
                        {
                            dataGridView1.CurrentCell = row.Cells[0];
                            break;
                        }
                    }
                }
                else
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0];
                }
            }
            return true;
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteData();
            }
        }

        private void kwlist_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private bool SetRecordFocusLevel(string uid, int focusLevel)
        {
            string sql = @"select count(*) from FilterReleaseInfo where uid ={0}";
            sql = string.Format(sql, uid);
            DataTable dt = cmd.GetTabel(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                int count = 0;
                if (int.TryParse(dt.Rows[0][0].ToString(), out count))
                {
                    sql = "";
                    if (count > 0)
                    {
                        sql = @"update FilterReleaseInfo set FocusLevel='{0}', ActionDate='{1}' where uid ={2}";
                        sql = string.Format(sql, focusLevel, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), uid);
                    }
                    else
                    {
                        sql = @"insert into FilterReleaseInfo(FocusLevel, uid, ActionDate) values({0}, {1}, '{2}')";
                        sql = string.Format(sql, focusLevel, uid, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    cmd.ExecuteNonQueryInt(sql);
                }
            }
            return true;
        }

        private void lblSetTop_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0 && dataGridView1.SelectedRows != null)
            {
                DataGridViewSelectedRowCollection dvs = dataGridView1.SelectedRows;
                string uid = dvs[0].Cells["uid"].Value.ToString();

                int col = 0;// dataGridView1.CurrentCell.ColumnIndex;
                int next = dataGridView1.Rows.GetNextRow(dataGridView1.CurrentCell.RowIndex, DataGridViewElementStates.None);
                string nextUid = "";
                if (next > -1)
                {
                    nextUid = dataGridView1.Rows[next].Cells["uid"].Value.ToString();
                }

                SetRecordFocusLevel(uid, 1);
                //刷新数据
                GetResultData();

                if (next > -1 && !string.IsNullOrEmpty(nextUid))
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (string.Compare(row.Cells["uid"].Value.ToString(), nextUid) == 0)
                        {
                            dataGridView1.CurrentCell = row.Cells[col];
                            break;
                        }
                    }
                }
                else
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[col];
                }

                FormateFoucsStatus();
            }
        }

        private void lblFocus_Click(object sender, EventArgs e)
        {
            //关注
            if (dataGridView1.Rows.Count > 0 && dataGridView1.SelectedRows != null)
            {
                DataGridViewSelectedRowCollection dvs = dataGridView1.SelectedRows;
                string uid = dvs[0].Cells["uid"].Value.ToString();

                int col = 0;// dataGridView1.CurrentCell.ColumnIndex;
                int next = dataGridView1.Rows.GetNextRow(dataGridView1.CurrentCell.RowIndex, DataGridViewElementStates.None);
               string nextUid = "";
                if (next > -1)
                {
                    nextUid = dataGridView1.Rows[next].Cells["uid"].Value.ToString();
                }

                SetRecordFocusLevel(uid, 3);
                GetResultData();

                if (next > -1 && !string.IsNullOrEmpty(nextUid))
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (string.Compare(row.Cells["uid"].Value.ToString(), nextUid) == 0)
                        {
                            dataGridView1.CurrentCell = row.Cells[col];
                            break;
                        }
                    }
                }
                else
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[col];
                }

                FormateFoucsStatus();
            }

        }

        private void lblMainFocus_Click(object sender, EventArgs e)
        {
            //重点关注
            if (dataGridView1.Rows.Count > 0 && dataGridView1.SelectedRows != null)
            {
                DataGridViewSelectedRowCollection dvs = dataGridView1.SelectedRows;
                string uid = dvs[0].Cells["uid"].Value.ToString();

                int col = 0;// dataGridView1.CurrentCell.ColumnIndex;
                int next = dataGridView1.Rows.GetNextRow(dataGridView1.CurrentCell.RowIndex, DataGridViewElementStates.None);
                string nextUid = "";
                if (next > -1)
                {
                    nextUid = dataGridView1.Rows[next].Cells["uid"].Value.ToString();
                }
                
                SetRecordFocusLevel(uid, 2);
                GetResultData();

                if (next > -1 && !string.IsNullOrEmpty(nextUid))
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (string.Compare(row.Cells["uid"].Value.ToString(), nextUid) == 0)
                        {
                            dataGridView1.CurrentCell = row.Cells[col];
                            break;
                        }
                    }
                }
                else
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[col];
                }

                FormateFoucsStatus();
            }
        }

        private void lblCancel_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0 && dataGridView1.SelectedRows != null)
            {
                DataGridViewSelectedRowCollection dvs = dataGridView1.SelectedRows;
                string uid = dvs[0].Cells["uid"].Value.ToString();

                int col = 0;// dataGridView1.CurrentCell.ColumnIndex;
                int next = dataGridView1.Rows.GetNextRow(dataGridView1.CurrentCell.RowIndex, DataGridViewElementStates.None);
                string nextUid = "";
                if (next > -1)
                {
                    nextUid = dataGridView1.Rows[next].Cells["uid"].Value.ToString();
                }

                //数据来源于筛选表
                string sql = @"delete from FilterReleaseInfo where uid={0}";
                sql = string.Format(sql, uid);
                cmd.ExecuteNonQueryInt(sql);
                GetResultData();
                FormateFoucsStatus();

                if (next > -1 && !string.IsNullOrEmpty(nextUid))
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (string.Compare(row.Cells["uid"].Value.ToString(), nextUid) == 0)
                        {
                            dataGridView1.CurrentCell = row.Cells[col];
                            break;
                        }
                    }
                }
                else
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[col];
                }
            }
        }

        private void FormateFoucsStatus()
        {
            if (dataGridView1.CurrentRow != null)
            {
                string level = dataGridView1.CurrentRow.Cells["focuslevel"].Value.ToString();
                if (!string.IsNullOrEmpty(level))
                {
                    switch (level)
                    {
                        case "1": //置顶
                            lblSetTop.Enabled = false;  //置顶
                            lblMainFocus.Enabled = true; //重点关注
                            lblFocus.Enabled = true; //关注
                            lblCancel.Enabled = true; //取消
                            lblDelete.Enabled = true; //删除
                            break;
                        case "2": //重点关注
                            lblSetTop.Enabled = true; //置顶
                            lblMainFocus.Enabled = false; //重点关注
                            lblFocus.Enabled = true; //关注
                            lblCancel.Enabled = true; //取消
                            lblDelete.Enabled = true; //删除
                            break;
                        case "3": //关注
                            lblSetTop.Enabled = true; //置顶
                            lblMainFocus.Enabled = true; //重点关注
                            lblFocus.Enabled = false; //关注
                            lblCancel.Enabled = true; //取消
                            lblDelete.Enabled = true; //删除
                            break;
                        case "99": //没有标志的记录
                            lblSetTop.Enabled = true; //置顶
                            lblMainFocus.Enabled = true; //重点关注
                            lblFocus.Enabled = true; //关注
                            lblCancel.Enabled = false; //取消
                            lblDelete.Enabled = true; //删除
                            break;
                    }
                }
                else
                {
                    lblSetTop.Enabled = true; //置顶
                    lblMainFocus.Enabled = true; //重点关注
                    lblFocus.Enabled = true; //关注
                    lblCancel.Enabled = false; //取消
                    lblDelete.Enabled = true; //删除
                }
            }
        }

        private void lblDelete_Click(object sender, EventArgs e)
        {
            DeleteData();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtComment.Text.Trim()))
            {
                MessageBox.Show("内容不能为空。");
                txtComment.Focus();
                return;
            }

            DataGridViewSelectedRowCollection dvs = dataGridView1.SelectedRows;
            string uid = dvs[0].Cells["uid"].Value.ToString();
            //数据来源于筛选表
            string sql = @"insert into UserComment(uid,Comment,SubmitDate,UserName,Deleted) values({0},'{1}','{2}','{3}',0)";
            sql = string.Format(sql, uid, txtComment.Text.Trim(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "");
            cmd.ExecuteNonQueryInt(sql);

            CommentView.Text = "";

        }

        private void lblPre_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                int col = dataGridView1.CurrentCell.ColumnIndex;
                int pre = dataGridView1.Rows.GetPreviousRow(dataGridView1.CurrentCell.RowIndex, DataGridViewElementStates.None);
                if (pre > -1)
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[pre].Cells[col];
                }
            }

        }

        private void lblNext_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                int col =dataGridView1.CurrentCell.ColumnIndex;
                int next = dataGridView1.Rows.GetNextRow(dataGridView1.CurrentCell.RowIndex, DataGridViewElementStates.None);
                if (next > -1)
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[next].Cells[col];
                }
            }
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && dataGridView1.Columns[e.ColumnIndex].Name.ToLower() == "contexts")
            {
                string title = this.dataGridView1.Rows[e.RowIndex].Cells["title"].Value.ToString();
                string link = this.dataGridView1.Rows[e.RowIndex].Cells["infosource"].Value.ToString();
                WebBrowser browser = new WebBrowser(title, link);
                browser.ShowDialog();
                
            }

        }

    }
}
