using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Finder.util;
using System.Text.RegularExpressions;

namespace Finder.Forms
{
    public partial class Changgui : Form
    {
        DataBaseServer.SQLitecommand cmd = new DataBaseServer.SQLitecommand();

        //关键字列表
        Dictionary<string, List<string>> dicKeywords = new Dictionary<string, List<string>>();
        private string kid = "0";//常规舆情

        public Changgui()
        {
            InitializeComponent();

        }

        private void Changgui_Load(object sender, EventArgs e)
        {
            DataTable kwdtAll = cmd.GetTabel("select name, keyword from keywords where kid = " + kid);
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

            dateTimePicker1.Value = DateTime.Now.AddDays(-30);
            keylist.DataSource = null;
            keylist.Items.Clear();

            keylist.Items.Add("全部");
            foreach (var key in dicKeywords.Keys)
            {
                keylist.Items.Add(key);
            }

            keylist.SelectedIndex = 0;
            if (sender is Form && (sender as Form).Tag != null && (sender as Form).Tag is string)
            {
                for (int i = 0; i < keylist.Items.Count; i++)
                {
                    if (keylist.Items[i] is string && (keylist.Items[i] as string) == ((sender as Form).Tag as string))
                    {
                        keylist.SelectedIndex = i;
                    }
                }
            }
            keylist.Enabled = true;
        }

        private void GetResultData()
        {
            //sql样例：
            //select b.[Name] eventname, a.* from v_releaseinfo a
            //left join keywords b on a.keywords=b.[KeyWord]
            //where a.uid > 0 and a.kid = 3
            //and a.keywords in ('地震 塌方')
            string sql = @"select b.[Name] eventname, a.* from v_releaseinfo a  left join keywords b on a.keywords=b.[KeyWord] 
                                    where a.uid > 0 and a.kid = " + kid;
            string eventName = keylist.Text;
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

            if (dateTimePicker1.Value > DateTime.Now.AddDays(-1))
            {
                sql += " and a.collectdate  > '" + dateTimePicker1.Value.ToString("yyyy-MM-dd 00:00:00") + "'";
            }
            else
            {
                sql += " and a.collectdate  BETWEEN '" + dateTimePicker1.Value.ToString("yyyy-MM-dd 00:00:00") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd 23:59:59") + "'";
            }
            sql += " and b.[Name] is not null order by a.ReleaseDate desc";

            DataTable dt = cmd.GetTabel(sql);
            #region 是否精确匹配
            if (chkPrecise.Visible == true && chkPrecise.Checked)
            {
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
            }
            #endregion

            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = dt;

            FormatDataView();

            if (dataGridView1.Height > 508) dataGridView1.Height = 508;
        }

        //表格初始化，设定各单元格显示，居中，宽度
        private void FormatDataView()
        {
            dataGridView1.Columns.Add(new DataGridViewImageColumn() { HeaderText = "正负预判", Name = "part_img", DisplayIndex = 19 });
            dataGridView1.Columns.Add(new DataGridViewLinkColumn() { HeaderText = "标题", Name = "title_link", DisplayIndex = 4, Width = 160 });
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                switch (col.Name)
                {
                    #region 调整列的隐藏与列序
                    case "uid":
                        col.DisplayIndex = 0;
                        col.Visible = false;
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
                    case "title":
                        col.HeaderText = "标题_txt";
                        col.DisplayIndex = 3;
                        col.Width = 160;
                        col.Visible = false;
                        break;
                    case "title_link":
                        col.HeaderText = "标题";
                        col.DisplayIndex = 4;
                        col.Width = 160;
                        break;
                    case "infosource":
                        col.HeaderText = "链接";
                        col.DisplayIndex = 5;
                        col.Visible = false;
                        break;
                    case "contexts":
                        col.HeaderText = "内容";
                        col.DisplayIndex = 6;
                        col.Width = 480;
                        break;
                    case "webaddress":
                        col.HeaderText = "来源";
                        col.DisplayIndex = 7;
                        col.Width = 120;
                        break;
                    case "sheng":
                        col.HeaderText = "区域";
                        col.DisplayIndex = 8;
                        col.Width = 160;
                        break;
                    case "shi":
                        col.HeaderText = "市";
                        col.DisplayIndex = 9;
                        col.Visible = false;
                        break;
                    case "xian":
                        col.HeaderText = "县";
                        col.DisplayIndex = 10;
                        col.Visible = false;
                        break;
                    case "sender":
                        col.HeaderText = "发布者";
                        col.DisplayIndex = 11;
                        col.Width = 160;
                        break;
                    case "ReleaseDate":
                        col.HeaderText = "发布时间";
                        col.DisplayIndex = 12;
                        col.Width = 160;
                        break;
                    case "reposts":
                        col.HeaderText = "转发量";
                        col.DisplayIndex = 13;
                        col.Visible = false;
                        break;
                    case "comments":
                        col.HeaderText = "评论数";
                        col.DisplayIndex = 14;
                        col.Visible = false;
                        break;
                    case "pid":
                        col.HeaderText = "网站类别";
                        col.DisplayIndex = 15;
                        col.Width = 80;
                        break;
                    case "kid":
                        col.HeaderText = "事件类别";
                        col.DisplayIndex = 16;
                        col.Width = 80;
                        break;
                    case "collectdate":
                        col.HeaderText = "抓取时间";
                        col.DisplayIndex = 17;
                        col.Width = 160;
                        break;
                    case "part":
                        col.HeaderText = "正负预判-txt";
                        col.DisplayIndex = 18;
                        col.Width = 80;
                        col.Visible = false;
                        break;
                    case "part_img":
                        col.HeaderText = "正负预判";
                        col.DisplayIndex = 19;
                        col.Width = 80;
                        break;
                    #endregion
                }
            }
        }

        //表格内容格式化，设定正负向，类别，区域
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex != dataGridView1.NewRowIndex)
            {
                switch (dataGridView1.Columns[e.ColumnIndex].Name)
                {
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

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 0)
            {
                string title = dataGridView1.CurrentCell.OwningRow.Cells["title"].Value.ToString();
                //设置textbox内容为标题加链接
                dataView.Text = "标题：" + title + "\n链接：" + dataGridView1.CurrentCell.OwningRow.Cells["infosource"].Value.ToString() + "\n";

                //设置标题粗体
                dataView.Select(3, title.Length);
                dataView.SelectionFont = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold);

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
                }
                dataView.Select(0, 0);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && dataGridView1.Columns[e.ColumnIndex].Name == "title_link")
            {
                System.Diagnostics.Process.Start(this.dataGridView1.Rows[e.RowIndex].Cells["infosource"].Value.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            GetResultData();
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

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null; 
            dataGridView1.Columns.Clear();
            dataView.Clear();
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

        private void keylist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (keylist.Text == "全部")
            {
                chkPrecise.Visible = false;
            }
            else
            {
                chkPrecise.Visible = true;
            }
        }
    }
}
