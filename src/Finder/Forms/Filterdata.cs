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
            lblCancelTop.Visible = false;
            lblFocus.Visible = false;
            lblMainFocus.Visible = false;
            lblCancelFocus.Visible = false;

            lblSetTop.ForeColor = Color.Blue;
            lblCancelTop.ForeColor = Color.FromArgb(64,64,64);
            lblFocus.ForeColor = Color.Green;
            lblMainFocus.ForeColor = Color.Red;
            lblCancelFocus.ForeColor = Color.FromArgb(64, 64, 64);

            txtTitle.Text = "";
            lblCount.Text = "";
            txtTitle.BorderStyle = BorderStyle.None;

        }

        private void GetResultData()
        {
            string sql = @"select 2 as dataSource, 5 as dataSort, b.[Name] as EventName, 
a.uid,a.title,a.contexts,a.releasedate,a.infosource,a.keywords,a.releasename,a.collectdate,a.snapshot,a.webname,
a.pid,a.part,a.reposts,a.comments,a.kid,a.sheng,a.shi,a.xian,a.deleted , '' as memo, '' as FocusLevel, '' as ActionDate
from releaseinfo a  left join keywords b on a.keywords=b.[KeyWord] 
where deleted=0 and a.uid > 0";

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
            sql += " and b.[Name] is not null  order by a.ReleaseDate desc";

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

            //将置顶的数据添加进来

            string focusSql = @"select  1 as dataSource, case focuslevel when '置顶' then 1 when '重点关注' then 2 when '关注' then 3 else 4 end as dataSort,EventName, 
uid,title,contexts,releasedate,infosource,keywords,releasename,collectdate,snapshot,webname,
pid,part,reposts,comments,kid,sheng,shi,xian,deleted ,memo,FocusLevel,ActionDate
from FilterReleaseInfo 
where deleted=0";

            if (searchTxt.Text.ToString().Length > 0)
            {
                focusSql += " and contexts like '%" + searchTxt.Text.ToString() + "%'";
            }
            //事件类别
            if (pidlist.SelectedIndex != pidlist.Items.Count - 1)
            {
                focusSql += " and pid = " + ((DataRowView)pidlist.SelectedItem)["pid"].ToString();
            }

            if (kidlist.SelectedIndex != kidlist.Items.Count - 1)
            {
                focusSql += " and kid = " + kidlist.SelectedIndex.ToString();
                if (kwlist.SelectedIndex != kwlist.Items.Count - 1)
                {
                    string eventName = ((DataRowView)kwlist.SelectedItem)["name"].ToString();
                    if (eventName != "全部" && eventName != "")
                    {
                        if (dicKeywords.ContainsKey(eventName))
                        {
                            if (dicKeywords[eventName] != null)
                            {
                                focusSql += " and keywords in(";
                                foreach (var keyword in dicKeywords[eventName])
                                {
                                    focusSql += "'" + keyword + "',";
                                }
                                focusSql = focusSql.Substring(0, focusSql.Length - 1);
                                focusSql += ")";
                            }
                        }
                    }
                }
            }

            if (((DataRowView)shenglist.SelectedItem)["id"].ToString() != "0")
            {
                focusSql += " and sheng = '" + ((DataRowView)shenglist.SelectedItem)["name"].ToString() + "'";

                if (((DataRowView)shilist.SelectedItem)["id"].ToString() != "0")
                {
                    focusSql += " and shi = '" + ((DataRowView)shilist.SelectedItem)["name"].ToString() + "'";

                    if (((DataRowView)xianlist.SelectedItem)["id"].ToString() != "0")
                    {
                        focusSql += " and xian = '" + ((DataRowView)xianlist.SelectedItem)["name"].ToString() + "'";
                    }
                }
            }

            focusSql += " and collectdate  BETWEEN '" + dateTimePicker1.Value.ToString("yyyy-MM-dd 00:00:00") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd 23:59:59") + "'";

            DataTable dtFocus = cmd.GetTabel(focusSql);

            remove = new List<DataRow>();
            foreach (DataRow row in dt.Rows)
            {
                DataRow[] rows = dtFocus.Select("uid=" + row["uid"].ToString());
                if (rows != null && rows.Length > 0)
                {
                    remove.Add(row);
                }
            }
            if (remove != null && remove.Count > 0)
            {
                foreach (DataRow row in remove)
                {
                    dt.Rows.Remove(row);
                }
            }

            DataTable dtMerge = null;
            if (dtFocus == null || dtFocus.Rows.Count == 0)
            {
                dtMerge = dt;
            }
            else
            {
                dtFocus.Merge(dt);
                dtMerge = dtFocus;
            }

            dtMerge.DefaultView.Sort = "dataSource, dataSort, actiondate desc, collectdate desc";

            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = dtMerge;

            lblCount.Text = string.Format("共计检索到 {0} 条结果", dtMerge.Rows.Count);

            FormatDataView();
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

            foreach (DataGridViewColumn col in array)
            {
                switch (col.Name.ToLower())
                {
                    #region 调整列的隐藏与列序
                    case "uid":
                        col.DisplayIndex = 0;
                        col.Visible = false;
                        break;
                    case "focuslevel":
                        col.HeaderText = "关注度";
                        col.DisplayIndex = 1;
                        col.Width = 120;
                        break;
                    case "eventname":
                        col.HeaderText = "事件名称";
                        col.DisplayIndex = 2;
                        col.Width = 120;
                        break;
                    case "keywords":
                        col.HeaderText = "关键字";
                        col.DisplayIndex = 3;
                        col.Visible = false;
                        break;
                    case "title":
                        col.HeaderText = "标题_txt";
                        col.DisplayIndex = 4;
                        col.Width = 260;
                        col.Visible = false;
                        break;
                    case "title_link":
                        col.HeaderText = "标题";
                        col.DisplayIndex = 5;
                        col.Width = 260;
                        break;
                    case "infosource":
                        col.HeaderText = "链接";
                        col.DisplayIndex = 6;
                        col.Visible = false;
                        break;
                    case "contexts":
                        col.HeaderText = "内容";
                        col.DisplayIndex = 7;
                        col.Width = 480;
                        break;
                    case "webname":
                        col.HeaderText = "来源";
                        col.DisplayIndex = 8;
                        col.Width = 120;
                        break;
                    case "sheng":
                        col.HeaderText = "区域";
                        col.DisplayIndex = 9;
                        col.Width = 160;
                        break;
                    case "shi":
                        col.HeaderText = "市";
                        col.DisplayIndex = 10;
                        col.Visible = false;
                        break;
                    case "xian":
                        col.HeaderText = "县";
                        col.DisplayIndex = 11;
                        col.Visible = false;
                        break;
                    case "releasename":
                        col.HeaderText = "发布者";
                        col.DisplayIndex = 12;
                        col.Width = 160;
                        break;
                    case "releasedate":
                        col.HeaderText = "发布时间";
                        col.DisplayIndex = 13;
                        col.Width = 160;
                        break;
                    case "reposts":
                        col.HeaderText = "转发量";
                        col.DisplayIndex = 14;
                        col.Visible = false;
                        break;
                    case "comments":
                        col.HeaderText = "评论数";
                        col.DisplayIndex = 15;
                        col.Visible = false;
                        break;
                    case "pid":
                        col.HeaderText = "网站类别";
                        col.DisplayIndex = 16;
                        col.Width = 80;
                        break;
                    case "kid":
                        col.HeaderText = "事件类别";
                        col.DisplayIndex = 17;
                        col.Width = 80;
                        break;
                    case "collectdate":
                        col.HeaderText = "抓取时间";
                        col.DisplayIndex = 18;
                        col.Width = 160;
                        break;
                    case "part":
                        col.HeaderText = "正负预判-txt";
                        col.DisplayIndex = 19;
                        col.Width = 80;
                        col.Visible = false;
                        break;
                    case "part_img":
                        col.HeaderText = "正负预判";
                        col.DisplayIndex = 20;
                        col.Width = 80;
                        break;
                    default:
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
                            case "置顶":
                                e.CellStyle.ForeColor = Color.Blue;
                                break;
                            case "关注":
                                e.CellStyle.ForeColor = Color.Green;
                                break;
                            case "重点关注":
                                e.CellStyle.ForeColor = Color.Red;
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
                if(dataGridView1.CurrentRow!=null)
                {
                    string level = dataGridView1.CurrentRow.Cells["focuslevel"].Value.ToString();
                    if (!string.IsNullOrEmpty(level))
                    {
                        switch (level)
                        {
                            case "置顶":
                                lblSetTop.Enabled = false;
                                lblCancelTop.Enabled = true;
                                lblFocus.Enabled = false;
                                lblMainFocus.Enabled = false;
                                lblCancelFocus.Enabled = false;
                                break;
                            case "关注":
                                lblSetTop.Enabled = false;
                                lblCancelTop.Enabled = false;
                                lblFocus.Enabled = false;
                                lblMainFocus.Enabled = true;
                                lblCancelFocus.Enabled = true;
                                break;
                            case "重点关注":
                                lblSetTop.Enabled = true;
                                lblCancelTop.Enabled = false;
                                lblFocus.Enabled = true;
                                lblMainFocus.Enabled = false;
                                lblCancelFocus.Enabled = true;
                                break;
                        }
                    }
                    else
                    {
                        lblSetTop.Enabled = true;
                        lblCancelTop.Enabled = false;
                        lblFocus.Enabled = true;
                        lblMainFocus.Enabled = true;
                        lblCancelFocus.Enabled = false;
                    }
                }

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
            }
        }

        //表格点击事件，点击标题时打开浏览器
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && dataGridView1.Columns[e.ColumnIndex].Name == "title_link")
            {
                System.Diagnostics.Process.Start(this.dataGridView1.Rows[e.RowIndex].Cells["infosource"].Value.ToString());
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
            lblCancelTop.Visible = true;
            lblFocus.Visible = true;
            lblMainFocus.Visible = true;
            lblCancelFocus.Visible = true;

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
                MessageBox.Show("证据保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (MessageBox.Show("确定要删除选定的数据吗？删除后将无法恢复", "警告", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    DataGridViewSelectedRowCollection dvs = dataGridView1.SelectedRows;
                    StringBuilder sb = new StringBuilder("update ReleaseInfo set deleted=1 where uid in (");
                    //StringBuilder wb_sb = new StringBuilder("update ReleaseInfowb set deleted=1 where uid in (");
                    StringBuilder sb1 = new StringBuilder();
                    //StringBuilder wb_sb1 = new StringBuilder();
                    foreach (DataGridViewRow dr in dvs)
                    {
                        string uid = dr.Cells["uid"].Value.ToString();
                        string pid = dr.Cells["pid"].Value.ToString();
                        sb1.Append("," + uid);
                    }
                    sb.Append(sb1.Length > 1 ? sb1.ToString().Substring(1) : sb1.ToString());
                    //wb_sb.Append(wb_sb1.Length > 1 ? wb_sb1.ToString().Substring(1) : wb_sb1.ToString());
                    sb.Append(")");
                    //wb_sb.Append(")");
                    if (sb.Length > 48)
                    {
                        cmd.ExecuteNonQueryInt(sb.ToString());
                    }
                    //if (wb_sb.Length > 50)
                    //{
                    //    cmd.ExecuteNonQueryInt(wb_sb.ToString());
                    //}
                    GetResultData();
                }
            }
        }

        private void kwlist_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void lblSetTop_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0 && dataGridView1.SelectedRows != null)
            {
                DataGridViewSelectedRowCollection dvs = dataGridView1.SelectedRows;
                string uid = dvs[0].Cells["uid"].Value.ToString();
                string dataSource = dvs[0].Cells["dataSource"].Value.ToString();
                if (dataSource == "1")
                {
                    //数据来源于筛选表
                    string sql = @"update FilterReleaseInfo set FocusLevel='{0}', ActionDate='{1}' where uid ={2}";
                    sql = string.Format(sql, "置顶", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), uid);
                    cmd.ExecuteNonQueryInt(sql);
                    GetResultData(); 
                    FormateFoucsStatus();

                }
                else if (dataSource == "2")
                {
                    //数据来源于抓取表
                    string sql = @"insert into FilterReleaseInfo(FocusLevel, ActionDate, EventName, uid ,Title,Contexts,ReleaseDate,InfoSource,KeyWords,ReleaseName,CollectDate,Snapshot,webName,pid,part,reposts,comments,kid,sheng,shi,xian)
                                        select '{0}' as FocusLevl, '{1}', b.[Name] eventname, a.uid ,a.Title,a.Contexts,a.ReleaseDate,a.InfoSource,a.KeyWords,a.ReleaseName,a.CollectDate,a.Snapshot,a.webName,a.pid,a.part,a.reposts,a.comments,a.kid,a.sheng,a.shi,a.xian from releaseinfo a  left join keywords b on a.keywords=b.[KeyWord] 
                                        where a.uid ={2}";
                    sql = string.Format(sql, "置顶", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), uid);                   
                    cmd.ExecuteNonQueryInt(sql);
                    GetResultData();
                    FormateFoucsStatus();
                }
                
            }
        }

        private void lblFocus_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0 && dataGridView1.SelectedRows != null)
            {
                DataGridViewSelectedRowCollection dvs = dataGridView1.SelectedRows;
                string uid = dvs[0].Cells["uid"].Value.ToString();
                string dataSource = dvs[0].Cells["dataSource"].Value.ToString();
                if (dataSource == "1")
                {
                    //数据来源于筛选表
                    string sql = @"update FilterReleaseInfo set FocusLevel='{0}', ActionDate='{1}' where uid ={2}";
                    sql = string.Format(sql, "关注", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), uid);
                    cmd.ExecuteNonQueryInt(sql);
                    GetResultData();
                    FormateFoucsStatus();

                }
                else if (dataSource == "2")
                {
                    //数据来源于抓取表
                    string sql = @"insert into FilterReleaseInfo(FocusLevel, ActionDate, EventName, uid ,Title,Contexts,ReleaseDate,InfoSource,KeyWords,ReleaseName,CollectDate,Snapshot,webName,pid,part,reposts,comments,kid,sheng,shi,xian)
                                        select '{0}' as FocusLevl, '{1}', b.[Name] eventname, a.uid ,a.Title,a.Contexts,a.ReleaseDate,a.InfoSource,a.KeyWords,a.ReleaseName,a.CollectDate,a.Snapshot,a.webName,a.pid,a.part,a.reposts,a.comments,a.kid,a.sheng,a.shi,a.xian from releaseinfo a  left join keywords b on a.keywords=b.[KeyWord] 
                                        where a.uid ={2}";
                    sql = string.Format(sql, "关注", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), uid);
                    cmd.ExecuteNonQueryInt(sql);
                    GetResultData();
                    FormateFoucsStatus();
                }
            }

        }

        private void lblMainFocus_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0 && dataGridView1.SelectedRows != null)
            {
                DataGridViewSelectedRowCollection dvs = dataGridView1.SelectedRows;
                string uid = dvs[0].Cells["uid"].Value.ToString();
                string dataSource = dvs[0].Cells["dataSource"].Value.ToString();
                if (dataSource == "1")
                {
                    //数据来源于筛选表
                    string sql = @"update FilterReleaseInfo set FocusLevel='{0}', ActionDate='{1}' where uid ={2}";
                    sql = string.Format(sql, "重点关注", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), uid);
                    cmd.ExecuteNonQueryInt(sql);
                    GetResultData();
                    FormateFoucsStatus();
                }
                else if (dataSource == "2")
                {
                    //数据来源于抓取表
                    string sql = @"insert into FilterReleaseInfo(FocusLevel, ActionDate, EventName, uid ,Title,Contexts,ReleaseDate,InfoSource,KeyWords,ReleaseName,CollectDate,Snapshot,webName,pid,part,reposts,comments,kid,sheng,shi,xian)
                                        select '{0}' as FocusLevl, '{1}', b.[Name] eventname, a.uid ,a.Title,a.Contexts,a.ReleaseDate,a.InfoSource,a.KeyWords,a.ReleaseName,a.CollectDate,a.Snapshot,a.webName,a.pid,a.part,a.reposts,a.comments,a.kid,a.sheng,a.shi,a.xian from releaseinfo a  left join keywords b on a.keywords=b.[KeyWord] 
                                        where a.uid ={2}";
                    sql = string.Format(sql, "重点关注", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), uid);
                    cmd.ExecuteNonQueryInt(sql);
                    GetResultData();
                    FormateFoucsStatus();
                }
            }
        }

        private void lblCancelTop_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0 && dataGridView1.SelectedRows != null)
            {
                DataGridViewSelectedRowCollection dvs = dataGridView1.SelectedRows;
                string uid = dvs[0].Cells["uid"].Value.ToString();
                string dataSource = dvs[0].Cells["dataSource"].Value.ToString();
                if (dataSource == "1")
                {
                    //数据来源于筛选表
                    string sql = @"update FilterReleaseInfo set FocusLevel='', ActionDate='{0}' where uid ={1}";
                    sql = string.Format(sql, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), uid);
                    cmd.ExecuteNonQueryInt(sql);
                    GetResultData();
                    FormateFoucsStatus();
                }
            }
        }

        private void lblCancelFocus_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0 && dataGridView1.SelectedRows != null)
            {
                DataGridViewSelectedRowCollection dvs = dataGridView1.SelectedRows;
                string uid = dvs[0].Cells["uid"].Value.ToString();
                string dataSource = dvs[0].Cells["dataSource"].Value.ToString();
                if (dataSource == "1")
                {
                    //数据来源于筛选表
                    string sql = @"update FilterReleaseInfo set FocusLevel='', ActionDate='{0}' where uid ={1}";
                    sql = string.Format(sql, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), uid);
                    cmd.ExecuteNonQueryInt(sql);
                    GetResultData();
                    FormateFoucsStatus();
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
                        case "置顶":
                            lblSetTop.Enabled = false;
                            lblCancelTop.Enabled = true;
                            lblFocus.Enabled = false;
                            lblMainFocus.Enabled = false;
                            lblCancelFocus.Enabled = false;
                            break;
                        case "关注":
                            lblSetTop.Enabled = false;
                            lblCancelTop.Enabled = false;
                            lblFocus.Enabled = false;
                            lblMainFocus.Enabled = true;
                            lblCancelFocus.Enabled = true;
                            break;
                        case "重点关注":
                            lblSetTop.Enabled = true;
                            lblCancelTop.Enabled = false;
                            lblFocus.Enabled = true;
                            lblMainFocus.Enabled = false;
                            lblCancelFocus.Enabled = true;
                            break;
                    }
                }
                else
                {
                    lblSetTop.Enabled = true;
                    lblCancelTop.Enabled = false;
                    lblFocus.Enabled = true;
                    lblMainFocus.Enabled = true;
                    lblCancelFocus.Enabled = false;
                }
            }
        }

    }
}
