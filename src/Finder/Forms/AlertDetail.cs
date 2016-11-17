using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Finder.Forms
{
    public partial class AlertDetail : Form
    {
        string type;
        DataBaseServer.MySqlCmd cmd = new DataBaseServer.MySqlCmd();
        public AlertDetail(string type_)
        {
            InitializeComponent();
            type = type_;
            FormatDataView();
        }
        
        private void AlertDetail_Load(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            string sql = "";
            if (type.Equals("sms"))
            {
                sql = "SELECT  sendtime,keyword from sms        where sendtime like '%" + DateTime.Now.ToString("yyyy-MM-dd") + "%' order by sendtime desc";
            }
            else
            {
                sql = "SELECT  sendtime,keyword from soundAlert where sendtime like '%" + DateTime.Now.ToString("yyyy-MM-dd") + "%' order by sendtime desc";
            }
            dt = cmd.GetTabel(sql);
            dataGridView1.DataSource = dt;
            dataGridView1.Refresh();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormatDataView()
        {
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "报警时间", DataPropertyName = "sendtime" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "报警关键词", DataPropertyName = "keyword" });
            dataGridView1.Columns[0].Width = 180;
            dataGridView1.Columns[1].Width = 120;
        }

        private void cbo1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "";
            if (cbo1.Text.Equals("所有"))
            {
                if (type.Equals("sms"))
                {
                    sql = "SELECT  sendtime,keyword from sms order by sendtime desc";
                }
                else
                {
                    sql = "SELECT  sendtime,keyword from soundAlert order by sendtime desc";
                }
            }
            else
            {
                if (type.Equals("sms"))
                {
                    sql = "SELECT  sendtime,keyword from sms where sendtime >= '" + DateTime.Now.AddDays(0 - int.Parse(cbo1.Text)).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                }
                else
                {
                    sql = "SELECT  sendtime,keyword from soundAlert where sendtime >= '" + DateTime.Now.AddDays(0 - int.Parse(cbo1.Text)).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                }
            }
            DataTable dt = cmd.GetTabel(sql);
            dataGridView1.DataSource = dt;
            dataGridView1.Refresh();
        }

    }
}
