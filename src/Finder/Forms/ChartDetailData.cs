using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Finder.Forms
{
    public partial class ChartDetailData : Form
    {
        int kid;
        string kwOrWebaddress;
        string webtype;
        int day;
        string UD;
        DataBaseServer.SQLitecommand cmd = new DataBaseServer.SQLitecommand();
        public ChartDetailData(int kid_ , string kwOrWebaddress_ ,string webtype_ , int day_ , string ud_)
        {
            InitializeComponent();
            kid = kid_;
            kwOrWebaddress = kwOrWebaddress_;
            webtype = webtype_;
            day = day_;
            UD = ud_;
        }

        private void ChartDetailData_Load(object sender, EventArgs e)
        {
            string sql = "";
            FormatDataView();
            DataTable dt = new DataTable();
            string time = DateTime.Now.AddDays(0 - day).ToString("yyyy-MM-dd HH:mm:ss");
            if (UD.Equals("U"))
            {
                label1.Text = "关键词为：“" + kwOrWebaddress + "”的" + webtype + "数据透视表如下：";
                label1.ForeColor = Color.IndianRed;
                sql = "SELECT uid,keywords,title,infosource,contexts,releasename,reposts,comments,pid,webaddress,collectdate,part "
                    + " from releaseinfo "
                    + " where keywords like '%" + kwOrWebaddress + "%'"
                    + " and collectdate > '" + time + "'";
            }
            if (UD.Equals("D"))
            {
                label1.Text = "网站为：“" + kwOrWebaddress + "”的" + webtype + "数据透视表如下：";
                label1.ForeColor = Color.IndianRed;
                sql = "SELECT uid,keywords,title,infosource,contexts,releasename,reposts,comments,pid,webaddress,collectdate,part "
                    + " from releaseinfo " 
                    + " where webaddress = '" + kwOrWebaddress + "'"
                    + " and kid = " + kid
                    + " and collectdate > '" + time + "'";
            }
            dt = cmd.GetTabel(sql);
            dataGridView1.DataSource = dt;
        }

        //表格初始化，设定各单元格显示，居中，宽度
        private void FormatDataView()
        {
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "uid", DataPropertyName = "uid" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "关键词", DataPropertyName = "keywords" });
            dataGridView1.Columns.Add(new DataGridViewLinkColumn() { HeaderText = "标题", DataPropertyName = "title" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "链接", DataPropertyName = "infosource" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "内容", DataPropertyName = "contexts" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "发布者", DataPropertyName = "releasename" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "转发量", DataPropertyName = "reposts" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "评论数", DataPropertyName = "comments" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "类别", DataPropertyName = "pid" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "来源", DataPropertyName = "webaddress" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "抓取时间", DataPropertyName = "collectdate" });
            dataGridView1.Columns.Add(new DataGridViewImageColumn() { HeaderText = "评价", DataPropertyName = "part" });

            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[3].Visible = false;
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[6].Visible = false;
            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[8].Visible = false;
            dataGridView1.Columns[4].Width = 480;
            dataGridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[6].Width = 80;
            dataGridView1.Columns[7].Width = 80;
            dataGridView1.Columns[8].Width = 60;
            dataGridView1.Columns[10].Width = 160;
            dataGridView1.Columns[11].Width = 60;
        }

        //表格内容格式化，正负研判调用图片
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex != dataGridView1.NewRowIndex)
            {
                if (e.ColumnIndex == 11)
                {
                    string part = dataGridView1.Rows[e.RowIndex].Cells[11].Value.ToString();
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
                }
                if (e.ColumnIndex == 8)
                {
                    switch (dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString())
                    {
                        case "0":
                            e.Value = "其他";
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
                    }
                }
            }
        }

        //表格点击事件，点击标题时打开浏览器
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex == 2)
            {
                System.Diagnostics.Process.Start(this.dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
