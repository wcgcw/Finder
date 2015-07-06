using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataBaseServer;
using System.Text.RegularExpressions;

namespace Finder.Forms
{
    public partial class Webhref : Form
    {
        DataBaseServer.SQLitecommand cmd = new DataBaseServer.SQLitecommand();

        public Webhref()
        {
            InitializeComponent();
        }

        private void ClearForm()
        {
            url.Text = "";
            uname.Text = "";
            likeurl.Text = "";
            upid.SelectedIndex = 0;
        }

        private bool isUrl(string url) 
        {
            return Regex.IsMatch(url, @"^(http|https|ftp)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&amp;%\$\-]+)*@)?((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.[a-zA-Z]{2,4})(\:[0-9]+)?(/[^/][a-zA-Z0-9\.\,\?\'\\/\+&amp;%\$#\=~_\-@]*)*$");
        }

        private string GetDataViewSelectedUids()
        {
            int row = dataGridView1.SelectedRows.Count;
            string[] uids = new string[row];
            while (row > 0)
            {
                uids[--row] = ((DataRowView)dataGridView1.SelectedRows[row].DataBoundItem).Row["uid"].ToString();
            }
            return string.Join(",", uids);
        }

        private bool checkHasData(string dataName, string dataValue)
        {
            string sql = "select count(0) from webaddress where " + dataName + "='" + dataValue + "'";
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

        private bool AddKeyWord(string url, string uname, string likeurl, int upid)
        {
            if (checkHasData("url", url))
            {
                MessageBox.Show("网址 ：" + url + " 已存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            string sql = "insert into webaddress (url,name,likeurl,pid) values ('" + url + "','" + uname + "','" + likeurl + "'," + upid + ")";
            cmd.ExecuteNonQuery(sql);
            return true;
        }

        private void GetWebHrefData()
        {
            DataTable dt = new DataTable();
            dt = cmd.GetTabel("select uid,name as 别名 ,url as 网址 ,likeurl as 相似链接 ,case pid when 1 then '博客' when 2 then '论坛' else '其他' end as 网站类别 from webaddress order by uid desc");
            dataGridView1.DataSource = dt;
            dataGridView1.Columns[0].Visible = false;
            //dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].Width = 300;
            //dataGridView1.Columns[3].Width = 400;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void Webhref_Load(object sender, EventArgs e)
        {
            upid.SelectedIndex = 0;
            GetWebHrefData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (url.Text.Equals(""))
            {
                MessageBox.Show("请填写网站地址！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                url.Focus();
                return;
            }
            if (!isUrl(url.Text))
            {
                MessageBox.Show("网站地址格式错误，示例：http://www.163.com", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                url.Focus();
                return;
            }
            if (!likeurl.Text.Equals("") && !isUrl(likeurl.Text))
            {
                MessageBox.Show("相似链接格式错误，示例：http://www.163.com", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                likeurl.Focus();
                return;
            }
            if (uname.Text.Equals(""))
            {
                MessageBox.Show("请填写网站别名！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                uname.Focus();
                return;
            }
            AddKeyWord(url.Text, uname.Text, likeurl.Text,upid.SelectedIndex);
            ClearForm();
            GetWebHrefData();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int row = dataGridView1.SelectedRows.Count;
            if (MessageBox.Show("确认删除选中的" + row.ToString() + "条记录吗？", "请确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = "delete from webaddress where uid in (" + GetDataViewSelectedUids() + ")";
                cmd.ExecuteNonQuery(sql);
                GetWebHrefData();
            }
        }
    }
}
