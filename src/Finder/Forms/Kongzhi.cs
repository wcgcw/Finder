using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Finder.util;
using System.Windows.Forms;

namespace Finder.Forms
{
    public partial class Kongzhi : Form
    {
        DataBaseServer.SQLitecommand cmd = new DataBaseServer.SQLitecommand();

        public Kongzhi()
        {
            InitializeComponent();
        }

        //正负向开始
        private void ClearPartWordForm()
        {
            pWord.Text = "";
            mWord.Text = "";
            aWordView.Checked = true;
        }

        private string GetDataView3SelectedUids()
        {
            int row = dataGridView3.SelectedRows.Count;
            string[] uids = new string[row];
            while (row > 0)
            {
                uids[--row] = ((DataRowView)dataGridView3.SelectedRows[row].DataBoundItem).Row["uid"].ToString();
            }
            return string.Join(",", uids);
        }

        private bool checkHasData(string tabelName, string dataName, string dataValue)
        {
            string sql = "select count(0) from "+tabelName+" where " + dataName + "='" + dataValue + "'";
            int result = Convert.ToInt32(cmd.GetOne(sql));
            if (result.Equals(1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void GetPartWordData()
        {
            DataTable dt = new DataTable();
            string sql = "select uid , word , part from partword ";
            if (pWordView.Checked)
            {
                sql += "where part=1";
            }
            if (mWordView.Checked)
            {
                sql += "where part=0";
            }

            dt = cmd.GetTabel(sql + " order by uid desc");

            dataGridView3.DataSource = dt;
            dataGridView3.Columns[0].Visible = false;
        }

        private void UpdataPartWord(string dataName, string dataValue, string instr)
        {
            string sql = "update partword set " + dataName + " = '" + dataValue + "' where uid in (" + instr + ")";
            cmd.ExecuteNonQuery(sql);
            GetPartWordData();
        }

        private bool AddWord(string word, int part)
        {
            if (checkHasData("partword","word", word))
            {
                MessageBox.Show("关键词 ：" + word + " 已存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            string sql = "insert into partword (word,part) values ('" + word + "'," + part + ")";
            cmd.ExecuteNonQuery(sql);
            return true;
        }

        private void CellFormattingTable(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 2 && e.RowIndex != dataGridView3.NewRowIndex)
            {
                string part = dataGridView3.Rows[e.RowIndex].Cells[2].Value.ToString();
                if (part.Equals("1"))
                {
                    e.Value = File.ReadAllBytes(@"icons\u.png");
                }
                else
                {
                    e.Value = File.ReadAllBytes(@"icons\d.png");
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (pWord.Text.Equals("") && mWord.Text.Equals(""))
            {
                MessageBox.Show("请至少填写一个关键词！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                pWord.Focus();
                return;
            }
            if (!pWord.Text.Equals(""))
            {
                AddWord(pWord.Text, 1);
            }
            if (!mWord.Text.Equals(""))
            {
                AddWord(mWord.Text, 0);
            }
            ClearPartWordForm();
            GetPartWordData();
        }

        private void aWordView_CheckedChanged(object sender, EventArgs e)
        {
            GetPartWordData();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            UpdataPartWord("part", "1", GetDataView3SelectedUids());
        }

        private void button10_Click(object sender, EventArgs e)
        {
            UpdataPartWord("part", "0", GetDataView3SelectedUids());
        }

        private void button12_Click(object sender, EventArgs e)
        {
            int row = dataGridView3.SelectedRows.Count;
            if (MessageBox.Show("确认删除选中的" + row.ToString() + "条记录吗？", "请确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = "delete from partword where uid in (" + GetDataView3SelectedUids() + ")";
                cmd.ExecuteNonQuery(sql);
                GetPartWordData();
            }
        }
        //正负向结束

        //网站管理开始
        private void ClearWebhrefForm()
        {
            url.Text = "";
            uname.Text = "";
            likeurl.Text = "";
            pidlist.SelectedIndex = 0;
            shenglist.SelectedIndex = shenglist.Items.Count - 1;
        }

        private bool isUrl(string url)
        {
            return Regex.IsMatch(url, @"^(http|https|ftp)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&amp;%\$\-]+)*@)?((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.[a-zA-Z]{2,4})(\:[0-9]+)?(/[^/][a-zA-Z0-9\.\,\?\'\\/\+&amp;%\$#\=~_\-@]*)*$");
        }

        private string GetDataView1SelectedUids()
        {
            int row = dataGridView1.SelectedRows.Count;
            string[] uids = new string[row];
            while (row > 0)
            {
                uids[--row] = ((DataRowView)dataGridView1.SelectedRows[row].DataBoundItem).Row["uid"].ToString();
            }
            return string.Join(",", uids);
        }

        private bool AddKeyWord(string url, string uname, string likeurl, int upid,string sheng,string shi,string xian)
        {
            if (checkHasData("webaddress", "url", url))
            {
                MessageBox.Show("网址 ：" + url + " 已存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            string sql = "insert into webaddress (url,name,likeurl,pid,sheng,shi,xian) values ('" + url + "','" + uname + "','" + likeurl + "','" + upid + "','" + sheng + "','" + shi + "','" + xian + "')";
            cmd.ExecuteNonQuery(sql);
            return true;
        }

        private void GetWebHrefData()
        {
            DataTable dt = new DataTable();
            dt = cmd.GetTabel("select uid,name as 网站名称 ,url as 网站链接 ,likeurl as 相似链接 ,case pid when 1 then '博客' when 2 then '论坛' when 4 then '主流媒体'  else '贴吧' end as 网站类别 ,sheng as 省 ,shi as 市 ,xian as 县 from webaddress order by uid desc");
            dataGridView1.DataSource = dt;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Width = 160;
            dataGridView1.Columns[2].Width = 300;
            dataGridView1.Columns[3].Width = 400;
            dataGridView1.Columns[4].Width = 120;
            dataGridView1.Columns[5].Width = 120;
            dataGridView1.Columns[6].Width = 120;
            dataGridView1.Columns[7].Width = 120;
            dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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
                shilist.ValueMember = "uid";

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
                xianlist.ValueMember = "uid";

                DataRow dr = dt.NewRow();
                dr["name"] = "全部";
                dr["id"] = "0";

                dt.Rows.Add(dr);

                xianlist.DataSource = dt;
                xianlist.SelectedIndex = xianlist.Items.Count - 1;
                xianlist.Show();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (url.Text.Equals(""))
            {
                MessageBox.Show("请填写网站链接！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                url.Focus();
                return;
            }
            //if (!isUrl(url.Text))
            //{
            //    MessageBox.Show("网站链接格式错误，示例：http://www.163.com", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    url.Focus();
            //    return;
            //}
            //if (!likeurl.Text.Equals("") && !isUrl(likeurl.Text))
            //{
            //    MessageBox.Show("相似链接格式错误，示例：http://www.163.com", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    likeurl.Focus();
            //    return;
            //}
            if (uname.Text.Equals(""))
            {
                MessageBox.Show("请填写网站名称！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                uname.Focus();
                return;
            }

            string sheng = "";
            string shi = "";
            string xian = "";
            int upidIndex = 1;

            switch (pidlist.SelectedIndex)
            {
                case 0:
                    upidIndex = 1;
                    break;
                case 1:
                    upidIndex = 2;
                    break;
                case 2:
                    upidIndex = 4;
                    break;
                case 3:
                    upidIndex = 5;
                    break;
            }

            if (((DataRowView)shenglist.SelectedItem)["id"].ToString() != "0")
            {
                sheng = ((DataRowView)shenglist.SelectedItem)["name"].ToString();
                if (((DataRowView)shilist.SelectedItem)["id"].ToString() != "0")
                {
                    shi = ((DataRowView)shilist.SelectedItem)["name"].ToString();
                    if (((DataRowView)xianlist.SelectedItem)["id"].ToString() != "0")
                    {
                        xian = ((DataRowView)xianlist.SelectedItem)["name"].ToString();
                    }
                }
            }
            AddKeyWord(url.Text, uname.Text, likeurl.Text, upidIndex,sheng,shi,xian);
            ClearWebhrefForm();
            GetWebHrefData();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int row = dataGridView1.SelectedRows.Count;
            if (MessageBox.Show("确认删除选中的" + row.ToString() + "条记录吗？", "请确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = "delete from webaddress where uid in (" + GetDataView1SelectedUids() + ")";
                cmd.ExecuteNonQuery(sql);
                GetWebHrefData();
            }
        }
        //网站管理结束

        //事件管理开始
        private void ClearKeyWordForm()
        {
            KeyWordName.Text = "";
            keywords.Text = "";
            messageAlarm.Checked = false;
            musicAlarm.Checked = false;
        }

        private bool AddKeyWord(string kwname,string kw, int meal, int mual)
        {
            if (checkHasData("keywords", "name", kwname))
            {
                MessageBox.Show("事件 ：" + kwname + " 已存在！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            int count = Convert.ToInt32(cmd.GetOne(("select count(0) keyword from keywords")));

            string softVer = GlobalPars.GloPars.ContainsKey("SoftVer") ? GlobalPars.GloPars["SoftVer"].ToString() : "1";
            if (softVer.Equals("1") && count > 100)
            {
                MessageBox.Show("关键词数量已达您所购买的基础版上限，如欲添加更多请致电客服提升版本", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (softVer.Equals("2") && count > 300)
            {
                MessageBox.Show("关键词数量已达您所购买的高级版上限，如欲添加更多请致电客服提升版本", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            int kid = kidlist.SelectedIndex;
            string[] keywords = kw.ToString().Trim().Split('，');

            foreach (string kwi in keywords)
            {
                string sql = "insert into keywords (name,keyword,messagealarm,musicalarm,kid,has) values ('" + kwname + "','" + kwi + "'," + meal + "," + mual + "," + kid + ",1)";
                cmd.ExecuteNonQuery(sql);
            }
            return true;
        }

        private string GetDataView2SelectedRows()
        {
            int row = dataGridView2.SelectedRows.Count;
            string[] names = new string[row];
            while (row > 0)
            {
                names[--row] = ((DataRowView)dataGridView2.SelectedRows[row].DataBoundItem).Row["事件名称"].ToString();
            }
            string str = "'" + string.Join("','", names) + "'";
            return str;
        }

        private void GetKeyWordData()
        {
            DataTable dt = cmd.GetTabel("select uid,name as 事件名称,keyword as 关键字,case messagealarm when 0 then '不报警' else '报警' end as 短信报警 ,case musicalarm when 0 then '不报警' else '报警' end as 声音报警, case kid when 0 then '常规舆情' when 1 then '敏感舆情' when 2 then '重点舆情' else '突发舆情' end as 事件分类 from keywords group by name");

            DataTable dtAll = cmd.GetTabel("select * from keywords");

            for (int i = 0, l = dt.Rows.Count; i < l; i++)
            {
                List<string> keywords = new List<string>();
                string name = dt.Rows[i]["事件名称"].ToString();
                for (int j = 0, k = dtAll.Rows.Count; j < k; j++)
                {
                    if (name == dtAll.Rows[j]["name"].ToString())
                    {
                        keywords.Add(dtAll.Rows[j]["keyword"].ToString());
                    }
                }
                dt.Rows[i]["关键字"] = string.Join("，",keywords);
            }

            dataGridView2.DataSource = dt;
            dataGridView2.Columns[0].Visible = false;
            dataGridView2.Columns[1].Width = 120;
            dataGridView2.Columns[2].Width = 600;
            dataGridView2.Columns[3].Width = 80;
            dataGridView2.Columns[4].Width = 80;
            dataGridView2.Columns[5].Width = 80;
        }

        private void UpdataKeyWord(string dataName, string dataValue, string instr)
        {
            string sql = "update keywords set " + dataName + " = '" + dataValue + "' where name in (" + instr + ")";
            cmd.ExecuteNonQuery(sql);
            GetKeyWordData();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            UpdataKeyWord("musicalarm", "0", GetDataView2SelectedRows());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UpdataKeyWord("musicalarm", "1", GetDataView2SelectedRows());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int row = dataGridView2.SelectedRows.Count;
            if (MessageBox.Show("如果您删除的事件已经抓取到数据，可能会花一点时间！确认删除选中的" + row.ToString() + "条记录吗？", "请确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string k = GetDataView2SelectedRows();
                List<string>sqls=new List<string>();
                sqls.Add(string.Format("delete from keywords where name in ({0})", k));
                sqls.Add("delete from ReleaseInfo where KeyWords in (select keyword from keywords where name in (" + k + "))");
                try
                {
                    if(cmd.Transaction(sqls))
                    {
                        //修改成功
                    }
                    else
                    {
                        //修改失败
                        MessageBox.Show("删除失败，请稍后再试！");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("删除失败，请稍后再试！");
                }
            
                //string sql = string.Format("delete from keywords where name in ({0})", k);
                //cmd.ExecuteNonQuery(sql);
                //sql = "delete from ReleaseInfo where KeyWords in (select keyword from keywords where name in (" + k + "))";
                //cmd.ExecuteNonQuery(sql);

                //ReleaseInfowb已经不需要记录数据了
                //sql = "DELETE from ReleaseInfowb where KeyWords=" + k + "";
                //cmd.ExecuteNonQuery(sql);

                GetKeyWordData();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (KeyWordName.Text.Equals(""))
            {
                MessageBox.Show("请填写事件名称！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                KeyWordName.Focus();
                return;
            }
            if (keywords.Text.Equals(""))
            {
                MessageBox.Show("请填写事件包含关键词！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                keywords.Focus();
                return;
            }
            AddKeyWord(KeyWordName.Text,keywords.Text, messageAlarm.Checked ? 1 : 0, musicAlarm.Checked ? 1 : 0);
            ClearKeyWordForm();
            GetKeyWordData();
        }

        private void Kongzhi_Load(object sender, EventArgs e)
        {
            dataGridView3.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "uid", DataPropertyName = "uid" });
            dataGridView3.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "关键词", DataPropertyName = "word" });
            dataGridView3.Columns.Add(new DataGridViewImageColumn() { HeaderText = "正负词性", DataPropertyName = "part" });
            GetPartWordData();

            shilist.Hide();
            xianlist.Hide();
            shenglist.DataSource = null;
            shenglist.Items.Clear();
            string sql = "select id , name from area where lvl = 1";
            DataTable dt = cmd.GetTabel(sql);
            shenglist.DisplayMember = "name";
            shenglist.ValueMember = "uid";
            DataRow dr = dt.NewRow();
            dr["name"] = "全部";
            dr["id"] = "0";
            dt.Rows.Add(dr);
            shenglist.DataSource = dt;
            shenglist.SelectedIndex = shenglist.Items.Count - 1;
            pidlist.SelectedIndex = 0;
            GetWebHrefData();

            GetKeyWordData();
            string softVer = GlobalPars.GloPars.ContainsKey("SoftVer") ? GlobalPars.GloPars["SoftVer"].ToString() : "1";
            if (softVer.Equals("1"))
            {
                sql = "select kid , name from kid where kid < 1";
            }
            else
            {
                sql = "select kid , name from kid order by kid";
            }
            DataTable kidDt = cmd.GetTabel(sql);
            kidlist.DisplayMember = "name";
            kidlist.ValueMember = "kid";
            kidlist.DataSource = kidDt;
            kidlist.SelectedIndex = 0;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            UpdataKeyWord("messagealarm", "1", GetDataView2SelectedRows());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            UpdataKeyWord("messagealarm", "0", GetDataView2SelectedRows());
        }

        private void dataGridView2_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                KeyWordName.Text = dataGridView2.SelectedRows[0].Cells[1].Value.ToString();
                keywords.Text = dataGridView2.SelectedRows[0].Cells[2].Value.ToString();
                messageAlarm.Checked = dataGridView2.SelectedRows[0].Cells[3].Value.ToString().Equals("不报警") ? false : true;
                musicAlarm.Checked = dataGridView2.SelectedRows[0].Cells[4].Value.ToString().Equals("不报警") ? false : true;
                string yqType = dataGridView2.SelectedRows[0].Cells[5].Value.ToString();
                switch (yqType)
                {
                    case "敏感舆情":
                        kidlist.SelectedValue = 1;
                        break;
                    case "重点舆情":
                        kidlist.SelectedValue = 2;
                        break;
                    case "突发舆情":
                        kidlist.SelectedValue = 3;
                        break;
                    default:
                        kidlist.SelectedValue = 0;
                        break;
                }
                button13.Enabled = true;
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要修改吗？", "提示", MessageBoxButtons.YesNo)==DialogResult.Yes)
            {
                string shijian = dataGridView2.SelectedRows[0].Cells[1].Value.ToString();

                List<string> remove = new List<string>();
                 string sql ="select * from keywords where name='" + shijian + "'";
                 DataTable dt = cmd.GetTabel(sql);
                 if (dt != null && dt.Rows.Count > 0)
                 {
                     foreach (DataRow row in dt.Rows)
                     {
                         string key = row["keyword"].ToString();
                         bool isFund = false;
                         string[] tmp = keywords.Text.Trim().Split('，');
                         if (tmp != null && tmp.Count() > 0)
                         {
                             foreach (var s in tmp)
                             {
                                 if (s.ToLower() == key.ToLower())
                                 {
                                     isFund = true;
                                     break;
                                 }
                             }
                         }
                         if (!isFund)
                         {
                             //这个关键词没有找到，最后修改成功后需要删除该关键字的数据
                             remove.Add(key);
                         }
                     }
                 }
                //先删除后添加
                sql = "delete from keywords where name='" + shijian + "'";
                cmd.ExecuteNonQuery(sql);
                bool ret = AddKeyWord(KeyWordName.Text, keywords.Text, messageAlarm.Checked ? 1 : 0, musicAlarm.Checked ? 1 : 0);
                if (ret)
                {
                    //更新成功后，删除去掉的关键词
                    if (remove != null && remove.Count > 0)
                    {
                        sql = "delete from ReleaseInfo where KeyWords in (";
                        foreach (var r in remove)
                        {
                            sql += "'"+ r +"',";
                        }
                        sql = sql.Substring(0, sql.Length - 1);
                        sql += ")";
                        cmd.ExecuteNonQuery(sql);
                    }
                }

                GetKeyWordData();
            }
        }
    }
}
