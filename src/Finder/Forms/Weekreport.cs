using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using Finder.util;

namespace Finder.Forms
{
    public partial class Weekreport : Form
    {
        public Weekreport()
        {
            InitializeComponent();
        }

        DataBaseServer.SQLitecommand cmd = new DataBaseServer.SQLitecommand();

        private RegistryKey OpenRegistryPath(RegistryKey root, string s)
        {
            s = s.Remove(0, 1) + @"/";
            while (s.IndexOf(@"/") != -1)
            {
                root = root.OpenSubKey(s.Substring(0, s.IndexOf(@"/")));
                s = s.Remove(0, s.IndexOf(@"/") + 1);
            }
            return root;
        }

        private void Weekreport_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now.AddDays(-1);
            dateTimePicker2.Value = DateTime.Now;
            string softVer = GlobalPars.GloPars.ContainsKey("SoftVer") ? GlobalPars.GloPars["SoftVer"].ToString() : "1";
            if (softVer.Equals("1"))
            {
                //checkBox1.Visible = false;
                //checkBox1.Checked = false;
                //checkBox2.Visible = false;
                //checkBox2.Checked = false;
                //checkBox3.Visible = false;
                //checkBox3.Checked = false;
                checkBox16.Visible = false;
                checkBox16.Checked = false;
            }
            else if (softVer.Equals("2"))
            {
                checkBox16.Visible = false;
                checkBox16.Checked = false;
            }

            #region 网站类别 pid
            string sql;

            sql = "select kid , name from kid";
            DataTable kidDt = cmd.GetTabel(sql);
            kidlist.DisplayMember = "name";
            kidlist.ValueMember = "pid";
            DataRow dr = kidDt.NewRow();
            dr["name"] = "全部";
            dr["kid"] = "4";
            kidDt.Rows.Add(dr);
            kidlist.DataSource = kidDt;
            kidlist.SelectedIndex = kidlist.Items.Count - 1;
            #endregion

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (titleTxt.Text.Length == 0)
            {
                MessageBox.Show("报告标题不得为空");
                titleTxt.Focus();
                return;
            }
            if (usernameTxt.Text.Length == 0)
            {
                MessageBox.Show("报告者姓名不得为空");
                usernameTxt.Focus();
                return;
            }
            //if (!checkBox0.Checked && !checkBox1.Checked && !checkBox2.Checked && !checkBox3.Checked)
            //{
            //    MessageBox.Show("请至少选择一项事件分类");
            //    return;
            //}
            if (!checkBox16.Checked && !checkBox10.Checked && !checkBox11.Checked && !checkBox12.Checked && !checkBox13.Checked && !checkBox14.Checked && !checkBox15.Checked && !checkBox16.Checked)
            {
                MessageBox.Show("请至少选择一项网站分类");
                return;
            }

            string sql = @"select b.[Name] eventname, a.keywords, a.title, a.infosource, a.kid, a.pid from v_releaseinfo  a  left join keywords b on a.keywords=b.[KeyWord]
                            where b.[Name] is not null  and a.collectdate  BETWEEN '" + dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd HH:mm:ss") + "'";

            //List<string> kid = new List<string>();
            //if (checkBox0.Checked)
            //{
            //    kid.Add("0");
            //}
            //if (checkBox1.Checked)
            //{
            //    kid.Add("1");
            //}
            //if (checkBox2.Checked)
            //{
            //    kid.Add("2");
            //}
            //if (checkBox1.Checked)
            //{
            //    kid.Add("3");
            //}
            //sql += "and a.kid in (" + string.Join(",", kid) + ")";

            List<string> pid = new List<string>();
            if (checkBox10.Checked)
            {
                pid.Add("0");
            }
            if (checkBox11.Checked)
            {
                pid.Add("1");
            }
            if (checkBox12.Checked)
            {
                pid.Add("2");
            }
            if (checkBox13.Checked)
            {
                pid.Add("3");
            }
            if (checkBox14.Checked)
            {
                pid.Add("4");
            }
            if (checkBox15.Checked)
            {
                pid.Add("5");
            }
            if (checkBox16.Checked)
            {
                pid.Add("6");
            }
            if (checkBox4.Checked)
            {
                //定制
                pid.Add("6");
            }
            sql += " and a.pid in (" + string.Join(",", pid) + ")";

            string eventCondition = "";
            if (kidlist.Visible && kidlist.Text != "全部")
            {
                eventCondition = "a.kid=" + (kidlist.SelectedItem as DataRowView)["kid"].ToString();
                if (kwlist.Visible && kwlist.Text != "全部")
                {
                    eventCondition += " and b.[Name]='" + (kwlist.SelectedItem as DataRowView)["name"].ToString() + "'";
                    if (cmbKeyword.Visible && cmbKeyword.Text != "全部")
                    {
                        eventCondition += " and a.keywords='" + (cmbKeyword.SelectedItem as DataRowView)["keyword"].ToString() + "'";
                    }
                }
            }
            if (eventCondition != "")
            {
                sql += " and " + eventCondition;
            }
            sql +=" order by a.kid, a.pid, eventname";

            DataTable dt = cmd.GetTabel(sql);
            List<string> list = new List<string>();
            string kw = "";
            string k = "";
            string p = "";
            string kName = "";
            string pName = "";

            list.Add("自 " + dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss") + " 至 " + dateTimePicker2.Value.ToString("yyyy-MM-dd HH:mm:ss") + " 期间,共检索到 " + dt.Rows.Count.ToString() + " 条数据");

            for (int i = 0, l = dt.Rows.Count; i < l; i++)
            {
                if (k != dt.Rows[i]["kid"].ToString())
                {
                    k = dt.Rows[i]["kid"].ToString();
                    p = "";
                    kw = "";
                    kName = "";
                    switch (k)
                    {
                        case "0":
                            kName = "常规舆情";
                            break;
                        case "1":
                            kName = "敏感舆情";
                            break;
                        case "2":
                            kName = "重点舆情";
                            break;
                        case "3":
                            kName = "突发舆情";
                            break;
                    }

                    string sql2 = @"SELECT count(1) kidcount from v_ReleaseInfo  a  left join keywords b on a.keywords=b.[KeyWord]
                            where b.[Name] is not null  and   a.collectdate  BETWEEN '" + dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    sql2 += "' and a.kid=" + k;
                    if (kwlist.Visible && kwlist.Text != "全部")
                    {
                        sql2 += " and b.[Name]='" + (kwlist.SelectedItem as DataRowView)["name"].ToString() + "'";
                        if (cmbKeyword.Visible && cmbKeyword.Text != "全部")
                        {
                            sql2 += " and a.keywords='" + (cmbKeyword.SelectedItem as DataRowView)["keyword"].ToString() +"'";
                        }
                    }
                    DataTable dtCount = cmd.GetTabel(sql2);
                    list.Add("检索到事件分类：" + kName + " 数据 " + dtCount.Rows[0]["kidcount"].ToString() + " 条");
                }
                if (p != dt.Rows[i]["pid"].ToString())
                {
                    p = dt.Rows[i]["pid"].ToString();
                    kw = "";

                    pName = "";
                    switch (p)
                    {
                        case "0":
                            pName = "全网";
                            break;
                        case "1":
                            pName = "博客";
                            break;
                        case "2":
                            pName = "论坛";
                            break;
                        case "3":
                            pName = "微博";
                            break;
                        case "4":
                            pName = "主流媒体";
                            break;
                        case "5":
                            pName = "贴吧";
                            break;
                        case "6":
                            pName = "微信";
                            break;
                        case "7":
                            pName = "定制";
                            break;
                    }

                    string sql3 = @"SELECT count(1) pidcount from v_ReleaseInfo   a  left join keywords b on a.keywords=b.[KeyWord]
                            where b.[Name] is not null  and  a.collectdate  BETWEEN '" + dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd HH:mm:ss") +
                            "' and a.kid=" + k + " and a.pid=" + p;
                    if (kwlist.Visible && kwlist.Text != "全部")
                    {
                        sql3 += " and b.[Name]='" + (kwlist.SelectedItem as DataRowView)["name"].ToString() + "'";
                        if (cmbKeyword.Visible && cmbKeyword.Text != "全部")
                        {
                            sql3 += " and a.keywords='" + (cmbKeyword.SelectedItem as DataRowView)["keyword"].ToString() + "'";
                        }
                    }
                    DataTable dtCount = cmd.GetTabel(sql3);
                    list.Add("  检索到网站分类：" + pName + " 数据 " + dtCount.Rows[0]["pidcount"].ToString() + " 条");
                }
                if (kw != dt.Rows[i]["eventname"].ToString())
                {
                    kw = dt.Rows[i]["eventname"].ToString();
                    string sql4 = @"SELECT count(1) kwcount from v_ReleaseInfo    a  left join keywords b on a.keywords=b.[KeyWord]
                            where b.[Name] is not null  and a. collectdate  BETWEEN '" + dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd HH:mm:ss")
                            + "' and a.kid=" + k + " and a.pid=" + p + " and b.[Name]='" + kw + "'";

                    if (cmbKeyword.Visible && cmbKeyword.Text != "全部")
                    {
                        sql4 += " and a.keywords='" + (cmbKeyword.SelectedItem as DataRowView)["keyword"].ToString() + "'";
                    }
                    DataTable dtCount = cmd.GetTabel(sql4);
                    list.Add("    检索到事件：" + kw + " 数据 " + dtCount.Rows[0]["kwcount"].ToString() + " 条");
                }
                list.Add("    标题：" + dt.Rows[i]["title"].ToString() + "<?p>链接：" + dt.Rows[i]["infosource"].ToString());
            }

            try
            {
                util.WordUtil wu = new util.WordUtil();
                wu.CreateWord("", titleTxt.Text.ToString(), list, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), usernameTxt.Text.ToString());
            }
            catch (Exception ex)
            {

            }


            ////wu.read();
            ////string[] replace = { "<title>", "<kw10>","<kwall>","<kwup>","<kwdown>","<wbkw>","<wbkwup>","<wbkwdown>","<ltkw>","<ltkwup>","<ltkwdown>","<bkkw>","<bkkwup>","<bkkwdown>","<qtkw>","<qtkwup>","<qtkwdown>","<web10>","<weball>","<webup>","<webdown>","<ltweb>","<ltwebup>","<bkweb>","<bkwebup>","<qtweb>","<qtwebup>","<wbuser>", "<time>" };
            ////wu.replace(replace, replaceWith);
            ////wu.saveAs(folders.GetValue("Desktop").ToString() + "\\" + titleTxt.Text + ".doc");
            ////wu.close();

            //DataTable dt = cmd.GetTabel("select keyword from keywords where has=1");
            //keywords = new string[dt.Rows.Count][];
            //string week = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss");
            //string weekstr = " where collectdate > '" + week + "' ";
            //string lweekstr = " where collectdate BETWEEN '" + DateTime.Now.AddDays(-14).ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + week + "' ";

            //DataTable kwdt = cmd.GetTabel("SELECT count(1),keyWords,pid FROM v_ReleaseInfo" + weekstr + "GROUP BY keyWords,pid");
            //DataTable lkwdt = cmd.GetTabel("SELECT count(1),keyWords,pid FROM v_ReleaseInfo" + lweekstr + "GROUP BY keyWords,pid");

            //for (int i = 0, l = dt.Rows.Count; i < l; i++)
            //{
            //    string keyword = dt.Rows[i]["keyword"].ToString();
            //    keywords[i] = new string[21];

            //    //0:关键字
            //    keywords[i][0] = keyword;

            //    keywords[i][6] = "0";
            //    keywords[i][10] = "0";
            //    keywords[i][14] = "0";
            //    keywords[i][18] = "0";
            //    for (int ki = 0, kl = kwdt.Rows.Count; ki < kl; ki++)
            //    {
            //        if (kwdt.Rows[ki][1].ToString().IndexOf(keyword) > -1)
            //        {
            //            switch (kwdt.Rows[ki][2].ToString())
            //            {
            //                case "3":
            //                    keywords[i][6] = Convert.ToString(Convert.ToInt32(keywords[i][6]) + Convert.ToInt32(kwdt.Rows[ki][0]));
            //                    break;
            //                case "2":
            //                    keywords[i][10] = Convert.ToString(Convert.ToInt32(keywords[i][10]) + Convert.ToInt32(kwdt.Rows[ki][0]));
            //                    break;
            //                case "1":
            //                    keywords[i][14] = Convert.ToString(Convert.ToInt32(keywords[i][14]) + Convert.ToInt32(kwdt.Rows[ki][0]));
            //                    break;
            //                case "0":
            //                    keywords[i][18] = Convert.ToString(Convert.ToInt32(keywords[i][18]) + Convert.ToInt32(kwdt.Rows[ki][0]));
            //                    break;
            //            }
            //        }
            //    }

            //    keywords[i][7] = "0";
            //    keywords[i][11] = "0";
            //    keywords[i][15] = "0";
            //    keywords[i][19] = "0";
            //    for (int ki = 0, kl = lkwdt.Rows.Count; ki < kl; ki++)
            //    {
            //        if (lkwdt.Rows[ki][1].ToString().IndexOf(keyword) > -1)
            //        {
            //            switch (lkwdt.Rows[ki][2].ToString())
            //            {
            //                case "3":
            //                    keywords[i][7] = Convert.ToString(Convert.ToInt32(keywords[i][7]) + Convert.ToInt32(lkwdt.Rows[ki][0]));
            //                    break;
            //                case "2":
            //                    keywords[i][11] = Convert.ToString(Convert.ToInt32(keywords[i][11]) + Convert.ToInt32(lkwdt.Rows[ki][0]));
            //                    break;
            //                case "1":
            //                    keywords[i][15] = Convert.ToString(Convert.ToInt32(keywords[i][15]) + Convert.ToInt32(lkwdt.Rows[ki][0]));
            //                    break;
            //                case "0":
            //                    keywords[i][19] = Convert.ToString(Convert.ToInt32(keywords[i][19]) + Convert.ToInt32(lkwdt.Rows[ki][0]));
            //                    break;
            //            }
            //        }
            //    }

            //    //5：微博出现次数
            //    //6：微博本周次数
            //    //7：微博上周次数
            //    //8：微博上升速度
            //    keywords[i][5] = Convert.ToString(Convert.ToInt32(keywords[i][6]) + Convert.ToInt32(keywords[i][7]));
            //    keywords[i][8] = Convert.ToString((Convert.ToInt32(keywords[i][6]) - Convert.ToInt32(keywords[i][7])) * 100 / (Convert.ToInt32(keywords[i][7]) > 0 ? Convert.ToInt32(keywords[i][7]) : 1));

            //    //9：论坛出现次数
            //    //10：论坛本周次数
            //    //11：论坛上周次数
            //    //12：论坛上升速度
            //    keywords[i][9] = Convert.ToString(Convert.ToInt32(keywords[i][10]) + Convert.ToInt32(keywords[i][11]));
            //    keywords[i][12] = Convert.ToString((Convert.ToInt32(keywords[i][10]) - Convert.ToInt32(keywords[i][11])) * 100 / (Convert.ToInt32(keywords[i][11]) > 0 ? Convert.ToInt32(keywords[i][11]) : 1));

            //    //13：博客出现次数
            //    //14：博客本周次数
            //    //15：博客上周次数
            //    //16：博客上升速度
            //    keywords[i][13] = Convert.ToString(Convert.ToInt32(keywords[i][14]) + Convert.ToInt32(keywords[i][15]));
            //    keywords[i][16] = Convert.ToString((Convert.ToInt32(keywords[i][14]) - Convert.ToInt32(keywords[i][15])) * 100 / (Convert.ToInt32(keywords[i][15]) > 0 ? Convert.ToInt32(keywords[i][15]) : 1));

            //    //17：其他出现次数
            //    //18：其他本周次数
            //    //19：其他上周次数
            //    //20：其他上升速度
            //    keywords[i][17] = Convert.ToString(Convert.ToInt32(keywords[i][18]) + Convert.ToInt32(keywords[i][19]));
            //    keywords[i][20] = Convert.ToString((Convert.ToInt32(keywords[i][18]) - Convert.ToInt32(keywords[i][19])) * 100 / (Convert.ToInt32(keywords[i][19]) > 0 ? Convert.ToInt32(keywords[i][19]) : 1));

            //    //1：出现次数
            //    //2：本周次数
            //    //3：上周次数
            //    //4：上升速度
            //    keywords[i][2] = Convert.ToString(Convert.ToInt32(keywords[i][6]) + Convert.ToInt32(keywords[i][10]) + Convert.ToInt32(keywords[i][14]) + Convert.ToInt32(keywords[i][18]));
            //    keywords[i][3] = Convert.ToString(Convert.ToInt32(keywords[i][7]) + Convert.ToInt32(keywords[i][11]) + Convert.ToInt32(keywords[i][15]) + Convert.ToInt32(keywords[i][19]));
            //    keywords[i][1] = Convert.ToString(Convert.ToInt32(keywords[i][2]) + Convert.ToInt32(keywords[i][3]));
            //    keywords[i][4] = Convert.ToString((Convert.ToInt32(keywords[i][2]) - Convert.ToInt32(keywords[i][3])) * 100 / (Convert.ToInt32(keywords[i][3]) > 0 ? Convert.ToInt32(keywords[i][3]) : 1));
            //}

            //DataTable bk_webdt = cmd.GetTabel("select name from WebAddress where pid=1");
            //int bk_l = bk_webdt.Rows.Count;
            //bk_webs = new string[bk_l][];

            //DataTable lt_webdt = cmd.GetTabel("select name from WebAddress where pid=2");
            //int lt_l = lt_webdt.Rows.Count;
            //lt_webs = new string[lt_l][];

            //DataTable qt_webdt = cmd.GetTabel("select name from WebAddress where pid=0");
            //int qt_l = qt_webdt.Rows.Count;
            //qt_webs = new string[qt_l][];

            //DataTable webdt = cmd.GetTabel("SELECT count(1),webaddress FROM v_ReleaseInfo" + weekstr + "GROUP BY webaddress");
            //DataTable lwebdt = cmd.GetTabel("SELECT count(1),webaddress FROM v_ReleaseInfo" + lweekstr + "GROUP BY webaddress");

            //webs = new string[bk_l + lt_l + qt_l][];
            //for (int i = 0; i < bk_l; i++)
            //{
            //    string web = bk_webdt.Rows[i]["name"].ToString();
            //    bk_webs[i] = new string[] { "0", "0", "0", "0", "0"};

            //    //0:关键字
            //    bk_webs[i][0] = web;

            //    for (int ki = 0, kl = webdt.Rows.Count; ki < kl; ki++)
            //    {
            //        if (webdt.Rows[ki][1].ToString() == web)
            //        {
            //            bk_webs[i][2] = webdt.Rows[ki][0].ToString();
            //        }
            //    }
            //    for (int ki = 0, kl = lwebdt.Rows.Count; ki < kl; ki++)
            //    {
            //        if (lwebdt.Rows[ki][1].ToString() == web)
            //        {
            //            bk_webs[i][3] = lwebdt.Rows[ki][0].ToString();
            //        }
            //    }

            //    bk_webs[i][1] = Convert.ToString(Convert.ToInt32(bk_webs[i][2]) + Convert.ToInt32(bk_webs[i][3]));
            //    bk_webs[i][4] = Convert.ToString((Convert.ToInt32(bk_webs[i][2]) - Convert.ToInt32(bk_webs[i][3])) * 100 / (Convert.ToInt32(bk_webs[i][3]) > 0 ? Convert.ToInt32(bk_webs[i][3]) : 1));

            //    webs[i] = bk_webs[i];
            //}
            //for (int i = 0; i < lt_l; i++)
            //{
            //    string web = lt_webdt.Rows[i]["name"].ToString();
            //    lt_webs[i] = new string[] { "0", "0", "0", "0", "0" };

            //    //0:关键字
            //    lt_webs[i][0] = web;

            //    for (int ki = 0, kl = webdt.Rows.Count; ki < kl; ki++)
            //    {
            //        if (webdt.Rows[ki][1].ToString() == web)
            //        {
            //            lt_webs[i][2] = webdt.Rows[ki][0].ToString();
            //        }
            //    }
            //    for (int ki = 0, kl = lwebdt.Rows.Count; ki < kl; ki++)
            //    {
            //        if (lwebdt.Rows[ki][1].ToString() == web)
            //        {
            //            lt_webs[i][3] = lwebdt.Rows[ki][0].ToString();
            //        }
            //    }

            //    lt_webs[i][1] = Convert.ToString(Convert.ToInt32(lt_webs[i][2]) + Convert.ToInt32(lt_webs[i][3]));
            //    lt_webs[i][4] = Convert.ToString((Convert.ToInt32(lt_webs[i][2]) - Convert.ToInt32(lt_webs[i][3])) * 100 / (Convert.ToInt32(lt_webs[i][3]) > 0 ? Convert.ToInt32(lt_webs[i][3]) : 1));
            //    webs[i + bk_l] = lt_webs[i];
            //}
            //for (int i = 0; i < qt_l; i++)
            //{
            //    string web = qt_webdt.Rows[i]["name"].ToString();
            //    qt_webs[i] = new string[] { "0", "0", "0", "0", "0" };

            //    //0:关键字
            //    qt_webs[i][0] = web;

            //    for (int ki = 0, kl = webdt.Rows.Count; ki < kl; ki++)
            //    {
            //        if (webdt.Rows[ki][1].ToString() == web)
            //        {
            //            qt_webs[i][2] = webdt.Rows[ki][0].ToString();
            //        }
            //    }
            //    for (int ki = 0, kl = lwebdt.Rows.Count; ki < kl; ki++)
            //    {
            //        if (lwebdt.Rows[ki][1].ToString() == web)
            //        {
            //            qt_webs[i][3] = lwebdt.Rows[ki][0].ToString();
            //        }
            //    }

            //    qt_webs[i][1] = Convert.ToString(Convert.ToInt32(qt_webs[i][2]) + Convert.ToInt32(qt_webs[i][3]));
            //    qt_webs[i][4] = Convert.ToString((Convert.ToInt32(qt_webs[i][2]) - Convert.ToInt32(qt_webs[i][3])) * 100 / (Convert.ToInt32(qt_webs[i][3]) > 0 ? Convert.ToInt32(qt_webs[i][3]) : 1));
            //    webs[i + bk_l + lt_l] = qt_webs[i];
            //}

            //string[] dateStr = { kw10tos(), kwalltos(), kwuptos(), kwdowntos(), wbkwtos(), wbkwuptos(), wbkwdowntos(), ltkwtos(), ltkwuptos(), ltkwdowntos(), bkkwtos(), bkkwuptos(), bkkwdowntos(), qtkwtos(), qtkwuptos(), qtkwdowntos(), web10tos(), weballtos(), webuptos(), webdowntos(), ltwebtos(), ltwebuptos(), bkwebtos(), bkwebuptos(), qtwebtos(), qtwebuptos(), wbusertos() };

            ////RegistryKey folders = OpenRegistryPath(Registry.CurrentUser, @"/software/microsoft/windows/currentversion/explorer/shell folders");
            ////string foldersStr = folders.GetValue("Desktop").ToString() + "\\" + titleTxt.Text + ".doc";

            //wu.creatWord(foldersStr, titleTxt.Text.ToString(), dateStr, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now.AddDays(-1);
            dateTimePicker2.Value = DateTime.Now;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now.AddDays(-7);
            dateTimePicker2.Value = DateTime.Now;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now.AddDays(-30);
            dateTimePicker2.Value = DateTime.Now;
        }

        private void kidlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            string kid = "";
            if (kidlist.SelectedItem != null)
            {
                kid = (kidlist.SelectedItem as DataRowView)["kid"].ToString();
            }
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

        private void kwlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            string kw = "";
            if (kwlist.SelectedItem != null)
            {
                kw = (kwlist.SelectedItem as DataRowView)["name"].ToString();
            }

            if (kw == "全部")
            {
                cmbKeyword.Hide();
                label10.Hide();
                cmbKeyword.SelectedIndex = cmbKeyword.Items.Count - 1;
            }
            else
            {
                string sql = "select uid, keyword from keywords where name = '" + kw + "'";
                DataTable dt = cmd.GetTabel(sql);

                cmbKeyword.DisplayMember = "keyword";
                cmbKeyword.ValueMember = "uid";

                DataRow dr = dt.NewRow();
                dr["keyword"] = "全部";
                dr["uid"] = "0";

                dt.Rows.Add(dr);

                cmbKeyword.DataSource = dt;
                cmbKeyword.SelectedIndex = cmbKeyword.Items.Count - 1;
                cmbKeyword.Show();
                label10.Show();
            }
        }
    }
}
