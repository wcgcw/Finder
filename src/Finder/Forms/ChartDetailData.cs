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

        private DataTable DataDetail = null;
        public ChartDetailData(string kwOrWebaddress_, string webtype_, string ud_, DataTable dataDetail)
        {
            InitializeComponent();

            kwOrWebaddress = kwOrWebaddress_;
            webtype = webtype_;            
            UD = ud_;
            DataDetail = dataDetail;

        }

        private void ChartDetailData_Load(object sender, EventArgs e)
        {
            if (DataDetail != null)
            {
                if (UD.Equals("U"))
                {
                    label1.Text = "关键词为：“" + kwOrWebaddress + "”的" + webtype + "数据透视表如下：";
                }
                else if (UD.Equals("D"))
                {
                    label1.Text = "网站为：“" + kwOrWebaddress + "”的" + webtype + "数据透视表如下：";
                }

                DataDetail.DefaultView.RowFilter = string.Format("keywords='{0}'", kwOrWebaddress);
                dataGridView1.DataSource = DataDetail;

                FormatDataView();
            }
            else
            {
                string sql = "";
                FormatDataView();
                DataTable dt = new DataTable();
                string time = DateTime.Now.AddDays(0 - day).ToString("yyyy-MM-dd HH:mm:ss");
                if (UD.Equals("U"))
                {
                    label1.Text = "关键词为：“" + kwOrWebaddress + "”的" + webtype + "数据透视表如下：";
                    label1.ForeColor = Color.IndianRed;
                    sql = "SELECT uid,keywords,title,infosource,contexts,releasename,reposts,comments,pid,webname,collectdate,part "
                        + " from releaseinfo "
                        + " where keywords like '%" + kwOrWebaddress + "%'"
                        + " and collectdate > '" + time + "'";
                }
                if (UD.Equals("D"))
                {
                    label1.Text = "网站为：“" + kwOrWebaddress + "”的" + webtype + "数据透视表如下：";
                    label1.ForeColor = Color.IndianRed;
                    sql = "SELECT uid,keywords,title,infosource,contexts,releasename,reposts,comments,pid,webname,collectdate,part "
                        + " from releaseinfo "
                        + " where webname = '" + kwOrWebaddress + "'"
                        + " and kid = " + kid
                        + " and collectdate > '" + time + "'";
                }
                dt = cmd.GetTabel(sql);
                dataGridView1.DataSource = dt;
            }
        }

        //表格初始化，设定各单元格显示，居中，宽度
        private void FormatDataView()
        {
            dataGridView1.Columns.Add(new DataGridViewImageColumn() { HeaderText = "正负预判", Name = "part_img", DisplayIndex = 19 });
            dataGridView1.Columns.Add(new DataGridViewLinkColumn() { HeaderText = "标题", Name = "title_link", DisplayIndex = 4, Width = 160 });
            foreach (DataGridViewColumn col in dataGridView1.Columns)
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

        }

        //表格内容格式化，正负研判调用图片
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
