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
        string kwOrEventName;
        string webtype;
        int day;
        string KE;
        DataBaseServer.SQLitecommand cmd = new DataBaseServer.SQLitecommand();
        public ChartDetailData(int kid_ , string kwOrEvent_ ,string webtype_ , int day_ , string ke_)
        {
            InitializeComponent();
            kid = kid_;
            kwOrEventName = kwOrEvent_;
            webtype = webtype_;
            day = day_;
            KE = ke_;
        }

        private DataTable DataDetail = null;
        public ChartDetailData(string kwOrWebaddress_, string webtype_, string ke_, DataTable dataDetail)
        {
            InitializeComponent();

            kwOrEventName = kwOrWebaddress_;
            webtype = webtype_;            
            KE = ke_;
            DataDetail = dataDetail;

        }

        private void ChartDetailData_Load(object sender, EventArgs e)
        {
            if (DataDetail != null)
            {
                if (KE.Equals("K"))
                {
                    label1.Text = "关键词为：“" + kwOrEventName + "”的" + webtype + "数据透视表如下：";
                    DataDetail.DefaultView.RowFilter = string.Format("keywords='{0}'", kwOrEventName);
                }
                else if (KE.Equals("E"))
                {
                    label1.Text = "事件为：“" + kwOrEventName + "”的" + webtype + "数据透视表如下：";
                    DataDetail.DefaultView.RowFilter = string.Format("eventname='{0}'", kwOrEventName);
                }

                dataGridView1.DataSource = DataDetail;

                FormatDataView();
            }
            //else
            //{
            //    string sql = "";
            //    FormatDataView();
            //    DataTable dt = new DataTable();
            //    string time = DateTime.Now.AddDays(0 - day).ToString("yyyy-MM-dd HH:mm:ss");
            //    if (KE.Equals("U"))
            //    {
            //        label1.Text = "关键词为：“" + kwOrEventName + "”的" + webtype + "数据透视表如下：";
            //        label1.ForeColor = Color.IndianRed;
            //        sql = "SELECT uid,keywords,title,infosource,contexts,releasename,reposts,comments,pid,webname,collectdate,part "
            //            + " from releaseinfo "
            //            + " where keywords like '%" + kwOrEventName + "%'"
            //            + " and collectdate > '" + time + "'";
            //    }
            //    if (KE.Equals("D"))
            //    {
            //        label1.Text = "网站为：“" + kwOrEventName + "”的" + webtype + "数据透视表如下：";
            //        label1.ForeColor = Color.IndianRed;
            //        sql = "SELECT uid,keywords,title,infosource,contexts,releasename,reposts,comments,pid,webname,collectdate,part "
            //            + " from releaseinfo "
            //            + " where webname = '" + kwOrEventName + "'"
            //            + " and kid = " + kid
            //            + " and collectdate > '" + time + "'";
            //    }
            //    dt = cmd.GetTabel(sql);
            //    dataGridView1.DataSource = dt;
            //}
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
