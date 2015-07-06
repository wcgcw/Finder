using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Web;
using DataBaseServer;
using HtmlAgilityPack;
using System.Collections;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using System.Web.UI;
using Sodao.Snap;
using System.Threading;
using Finder.util;

namespace Finder.Forms
{
    public partial class Monitor : Form
    {
        #region 属性
        //3分钟从数据库取一次微博
        System.Timers.Timer weiboSpiderTimer = new System.Timers.Timer(1000 * 60 * 3);
        //12小时抓取一次网页
        System.Timers.Timer webSpiderTimer = new System.Timers.Timer(1000 * 60 * 60 * 12);
        private DataTable dtnewsinfo;
        private DataTable dttiebainfo;
        private DataTable dtbloginfo;
        private DataTable dtbbsinfo;
        private DataTable dtqueryinfo;
        private DataTable dtweixininfo;

        HtmlAgilityPack.HtmlDocument docPage = new HtmlAgilityPack.HtmlDocument();
        private StringBuilder sb;
        private DataTable dtWebNewsInfo;
        private DataTable dtTieBaInfo;
        private DataTable dtWebBlogInfo;
        private DataTable dtWebBBSInfo;
        private DataTable dtWebQueryInfo;
        private DataTable dtWeiXinInfo;
        
        TbReleaseInfo tri;
        System.Threading.AutoResetEvent obj = new System.Threading.AutoResetEvent(false);

        private string WebtopUrl = "";
        Finder.util.QQWeibo qwei;
        Finder.util.SinaWeibo swei;
        //private bool butClike = false;
        DataBaseServer.SQLitecommand cmd = new DataBaseServer.SQLitecommand();
        private DataView dv;

        string SoftVer;
        #endregion

        #region 初始化
        public Monitor()
        {
            InitializeComponent();
        }

        private void Monitoring_Load(object sender, EventArgs e)
        {
            weiboSpiderTimer.Elapsed += new System.Timers.ElapsedEventHandler(weiboSpiderTimer_Elapsed);
            weiboSpiderTimer.AutoReset = true;
            weiboSpiderTimer.Enabled = false;

            webSpiderTimer.Elapsed += new System.Timers.ElapsedEventHandler(webSpiderTimer_Elapsed);
            webSpiderTimer.AutoReset = true;
            webSpiderTimer.Enabled = false;

            tri = new TbReleaseInfo();
            
            FormatDataView(dvAll, false);
            FormatDataView(dvBBs, false);
            FormatDataView(dvBlog, false);
            FormatDataView(dvWBlog, true);
            FormatDataView(dvWeb, false);
            FormatDataView(dvtieba, false);
            FormatDataView(dvWeiXin, false);

            dtnewsinfo = tri.GetReleaseInfoFormat();
            dttiebainfo = tri.GetReleaseInfoFormat();
            dtbloginfo = tri.GetReleaseInfoFormat();
            dtbbsinfo = tri.GetReleaseInfoFormat();
            dtqueryinfo = tri.GetReleaseInfoFormat();
            dtweixininfo = tri.GetReleaseInfoFormat();

            dtWebNewsInfo = tri.SelReleaseInfo(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd HH:mm:ss"), "0 AND webName<>'百度'");
            dvWeb.DataSource = dtnewsinfo;
            dtTieBaInfo = tri.SelReleaseInfo(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd HH:mm:ss"), "5");
            dvtieba.DataSource = dttiebainfo;
            dtWebBlogInfo = tri.SelReleaseInfo(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd HH:mm:ss"), "1");
            dvBlog.DataSource = dtbloginfo;
            dtWebBBSInfo = tri.SelReleaseInfo(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd HH:mm:ss"), "2");
            dvBBs.DataSource = dtbbsinfo;
            dtWebQueryInfo = tri.SelReleaseInfo(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd HH:mm:ss"), "4");
            dvAll.DataSource = dtqueryinfo;
            dtWeiXinInfo = tri.SelReleaseInfo(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd HH:mm:ss"), "6");
            dvWeiXin.DataSource = dtweixininfo;

            SoftVer = !GlobalPars.GloPars.ContainsKey("SoftVer")? "1" : GlobalPars.GloPars["SoftVer"].ToString();
            if (!SoftVer.Equals("3"))
            {
                tabControl1.TabPages.RemoveByKey("tabPage7");
            }
        }
        #endregion

        #region 定时运行
        //刷新微博数据,前台感觉是在搜索微博
        private void reFreshWeibo()
        {
            if (!Program.ProClose)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    lbweibo.Visible = true;
                    lbweibo.Text = "正在搜索微博数据...";
                    lbweibo.ForeColor = Color.DarkBlue;
                }));
                string wblog = "select uid,releasename AS title,contexts,releasedate,infosource,keywords,releasename,collectdate,snapshot,webname,pid,part,reposts,comments from ReleaseInfowb order by uid desc limit 0,300";
                DataBaseServer.SQLitecommand cmd = new SQLitecommand();
                DataTable dtwBlog = new DataTable();
                dtwBlog = cmd.GetTabel(wblog);
                this.BeginInvoke(new MethodInvoker(delegate() {
                    dvWBlog.DataSource = dtwBlog;
                    dvWBlog.Refresh();
                    lbweibo.Text = "一轮搜索完毕！";
                    lbweibo.ForeColor = Color.Red;
                }));
            }
        }
        private void weiboSpiderTimer_Elapsed(object sender, EventArgs e)
        {
            reFreshWeibo();
        }

        private void BeginEvn()
        {
            if (!Program.ProClose)
            {
                Thread t = new Thread(new ThreadStart(GetWebNewsInfo));
                t.IsBackground = true;
                t.Start();

                Thread b = new Thread(new ThreadStart(GetWebBlogInfo));
                b.IsBackground = true;
                b.Start();

                Thread BB = new Thread(new ThreadStart(GetWebBBSInfo));
                BB.IsBackground = true;
                BB.Start();

                Thread BB1 = new Thread(new ThreadStart(GetWebTieBaInfo));
                BB1.IsBackground = true;
                BB1.Start();

                Thread Webexplorer = new Thread(new ThreadStart(GetBaiduInfo));
                Webexplorer.IsBackground = true;
                Webexplorer.Start();

                if (SoftVer.Equals("3"))
                {
                    Thread Weixin = new Thread(new ThreadStart(GetWeiXinInfo));
                    Weixin.IsBackground = true;
                    Weixin.Start();
                }
            }
        }
        private void webSpiderTimer_Elapsed(object sender, EventArgs e)
        {
            BeginEvn();
        }
        #endregion

        #region 网页抓取代码（百度、新闻、博客、论坛类）

        #region 微信搜索
        private void GetWeiXinInfo()
        {
            this.BeginInvoke(new MethodInvoker(delegate()
            {
                lbweixin.Text = "";
                lbweixin.Visible = true;
            }));

            SQLitecommand cmd = new SQLitecommand();
			//得到关键字列表
			DataTable dtkey;
			DataTable dtParts;
			dtkey = cmd.GetTabel("select * from Keywords");
			dtParts = cmd.GetTabel("SELECT * FROM partword");
            Dictionary<string, string> keywords = new Dictionary<string, string>();
            for (int kw = 0; kw < dtkey.Rows.Count; kw++)
            {
                if (keywords.ContainsKey(dtkey.Rows[kw]["Name"].ToString().Trim() + "-" + dtkey.Rows[kw]["kid"].ToString().Trim()))
                {
                    keywords[dtkey.Rows[kw]["Name"].ToString().Trim() + "-" + dtkey.Rows[kw]["kid"].ToString().Trim()] = keywords[dtkey.Rows[kw]["Name"].ToString().Trim() + "-" + dtkey.Rows[kw]["kid"].ToString().Trim()] + "," + dtkey.Rows[kw]["KeyWord"].ToString().Trim();
                }
                else
                {
                    keywords.Add(dtkey.Rows[kw]["Name"].ToString().Trim() + "-" + dtkey.Rows[kw]["kid"].ToString().Trim(), "," + dtkey.Rows[kw]["KeyWord"].ToString().Trim());
                }
            }

            //链接的正则
            string aa = "https?://.[^\"]+";
            string[] sDate;

            sb = new StringBuilder();
            sb.Append("");

            //TbReleaseInfo ri = new TbReleaseInfo();

            //按关键字循环
            foreach (KeyValuePair<string, string> kv in keywords)
            {
                string k = kv.Key;
                string v = kv.Value.Substring(1);
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    lbweixin.Text = "正在搜索事件为<" + k.Substring(0, k.IndexOf("-")) + ">的数据.";
                    lbweixin.ForeColor = Color.DarkBlue;
                }));

                //取得关键字
                string keys = v;
                //组成查询字串
                string url = "http://weixin.sogou.com/weixin?type=2&query="+keys+"&ie=utf8";

                //得到结果放在数组内
                List<string> lis = new List<string>();
                lis = HtmlUtil.GetElementsByClassList(HtmlUtil.getHtml(url, "utf-8"), "weixin");

                //如果没取到,就结束本次循环
                if (lis == null) return;
                //webBrowser1.Navigate(url);

                //循环时判断是否要验证
                bool isThere = false;

                for (int i = 0; i < lis.Count; i++)
                {
                    if (Program.ProClose == true) break;

                    ModelReleaseInfo mri = new ModelReleaseInfo();

                    //发布日期的赋值
                    sDate = HtmlUtil.GetElementsByTagAndClass(lis[i], "weixin", "");
                    if (sDate.Length <= 0) continue;

                    Regex reg = new Regex("\\d{5,}", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                    Match match = reg.Match(sDate[0]);
                    string d = match.Value;
                    mri.ReleaseDate = HtmlUtil.GetWeiXinTime(long.Parse(d));
                    //mri.ReleaseDate = mri.ReleaseDate.Substring(mri.ReleaseDate.Length - 10, 10);

                    //判断日期
                    DateTime ddt;
                    if (DateTime.TryParse(mri.ReleaseDate, out ddt))
                    {
                    }
                    else
                    {
                        //百度的快照日期有时会是9位或8位,如果是这种情况,那么按规则去掉
                        mri.ReleaseDate = mri.ReleaseDate.Substring(1, 9);
                        if (DateTime.TryParse(mri.ReleaseDate, out ddt))
                        {
                        }
                        else
                        {
                            mri.ReleaseDate = mri.ReleaseDate.Substring(1, 8);
                        }
                    }
                    //处理日期
                    try
                    {
                        mri.ReleaseDate = DateTime.Parse(mri.ReleaseDate).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    catch (Exception ex)
                    {
                        StreamWriter sw = File.AppendText("log.txt");
                        sw.WriteLine(DateTime.Now.ToLongDateString());
                        sw.WriteLine("begin");
                        sw.WriteLine(ex.Message);
                        sw.WriteLine(sb.ToString());
                        sw.WriteLine("end");
                        sw.WriteLine("");

                        sw.Close();

                    }

                    //只拿取三天的内的数据
                    try
                    {
                        if (DateTime.Parse(mri.ReleaseDate) < DateTime.Now.AddDays(-30)) continue;
                    }
                    catch (Exception ex) { continue; }
                    try
                    {
                        //得到标题
                        mri.Title = HtmlUtil.NoHTML(HtmlUtil.GetElementsByTagName(lis[i], "h4")[0]);
                        string[] temp = HtmlUtil.GetWeiXinElements(lis[i]);

                        //如果未取到内容部分,就跳出
                        if (temp.Length == 0)
                            continue;

                        mri.Contexts = HtmlUtil.NoHTML(temp[0]);
                        mri.InfoSource = HtmlUtil.GetListByHtml("", HtmlUtil.GetElementsByTagName(lis[i], "a")[0], aa)[0];

                        //去掉重复
                        if (isThere)
                        {
                            continue;
                        }
                        else
                        {
                            if (UrlThereare(mri.Title, this.dtweixininfo, dtWeiXinInfo, false) != 0)
                            { isThere = true; continue; }
                        }

                        //mri.KeyWords = k.Substring(0, k.IndexOf("-"));
                        mri.KeyWords = k;
                        mri.CollectDate = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        mri.Snapshot = "";
                        mri.ReleaseName = "";
                        mri.Sheng = "";
                        mri.Shi = "";
                        mri.Xian = "";
                        mri.WebName = "微信";
                        mri.Pid = 6;
                        mri.Part = GetParts(mri.Contexts);
                        mri.Comments = 0;
                        mri.Reposts = 0;

                        DataRow dr = dtweixininfo.NewRow();
                        if (dvAll.RowCount == 0)
                        {
                            dr[0] = 1;
                        }
                        else
                        {
                            dr[0] = int.Parse(dvAll.Rows[dvAll.RowCount - 1].Cells[0].Value.ToString()) + 1;
                        }
                        dr[1] = mri.Title;
                        dr[2] = mri.Contexts;
                        dr[3] = mri.ReleaseDate;
                        dr[4] = mri.InfoSource;
                        dr[5] = mri.KeyWords.Substring(0,mri.KeyWords.IndexOf("-"));
                        dr[6] = mri.ReleaseName;
                        dr[7] = mri.CollectDate;
                        dr[8] = mri.Snapshot;
                        dr[9] = mri.WebName;
                        dr[10] = mri.Pid;
                        dr[11] = mri.Part;
                        dr[12] = mri.Reposts;
                        dr[13] = mri.Comments;

                        dtweixininfo.Rows.InsertAt(dr, 0);

                        if (dtqueryinfo.Rows.Count >= 500)
                        {
                            dtweixininfo.Rows.RemoveAt(500);
                        }
                        this.BeginInvoke(new MethodInvoker(delegate()
                        {
                            dvWeiXin.Refresh();
                        }));
                    }
                    catch (Exception ex)
                    {
                        StreamWriter sw = File.AppendText("log.txt");
                        sw.WriteLine(DateTime.Now.ToLongDateString());
                        sw.WriteLine("begin");
                        sw.WriteLine(ex.Message);
                        sw.WriteLine(sb.ToString());
                        sw.WriteLine("end");
                        sw.WriteLine("");

                        sw.Close();
                    }

                    try
                    {
                        //得到插入语句
                        if (isThere)
                        {
                            continue;
                        }
                        else
                        {
                            sb.Append(tri.GetInsString(mri) + ";");
                        }

                        //每10次执行一次插入数据库
                        if (sb.ToString().Length != 0)
                        {
                            if (i % 10 == 0)
                            {
                                //执行插入
                                cmd.ExecuteNonQuery(sb.ToString());
                                //清除插入字段串
                                sb.Clear();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        StreamWriter sw = File.AppendText("log.txt");
                        sw.WriteLine(DateTime.Now.ToLongDateString());
                        sw.WriteLine("begin");
                        sw.WriteLine(ex.Message);
                        sw.WriteLine(sb.ToString());
                        sw.WriteLine("end");
                        sw.WriteLine("");

                        sw.Close();
                    }

                }
            }
            try
            {
                if (sb.ToString().Length != 0)
                {
                    //执行插入
                    cmd.ExecuteNonQuery(sb.ToString());
                    //清除插入字段串
                    sb.Clear();
                }
            }
            catch (Exception ex)
            {
                StreamWriter sw = File.AppendText("log.txt");
                sw.WriteLine(DateTime.Now.ToLongDateString());
                sw.WriteLine("begin");
                sw.WriteLine(ex.Message);
                sw.WriteLine(sb.ToString());
                sw.WriteLine("end");
                sw.WriteLine("");

                sw.Close();
            }

            //执行完毕后,重新获取一次数据库的数据
            dtWeiXinInfo = tri.SelReleaseInfo(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd HH:mm:ss"), "6");
            this.BeginInvoke(new MethodInvoker(delegate()
            {
                lbweixin.Text = "一轮搜索完毕！";
                lbweixin.ForeColor = Color.Red;
            }));

        }
        #endregion

        #region 百度搜索
        private void GetBaiduInfo()
        {
            this.BeginInvoke(new MethodInvoker(delegate()
            {
                lbAll.Text = "";
                lbAll.Visible = true;
            }));

            SQLitecommand cmd = new SQLitecommand();
			//得到关键字列表
			DataTable dtkey;
			DataTable dtParts;
			dtkey = cmd.GetTabel("select * from Keywords");
			dtParts = cmd.GetTabel("SELECT * FROM partword");
            Dictionary<string, string> keywords = new Dictionary<string, string>();
            for (int kw = 0; kw < dtkey.Rows.Count; kw++)
            {
                if (keywords.ContainsKey(dtkey.Rows[kw]["Name"].ToString().Trim() + "-" + dtkey.Rows[kw]["kid"].ToString().Trim()))
                {
                    keywords[dtkey.Rows[kw]["Name"].ToString().Trim() + "-" + dtkey.Rows[kw]["kid"].ToString().Trim()] = keywords[dtkey.Rows[kw]["Name"].ToString().Trim() + "-" + dtkey.Rows[kw]["kid"].ToString().Trim()] + "," + dtkey.Rows[kw]["KeyWord"].ToString().Trim();
                }
                else
                {
                    keywords.Add(dtkey.Rows[kw]["Name"].ToString().Trim() + "-" + dtkey.Rows[kw]["kid"].ToString().Trim(), "," + dtkey.Rows[kw]["KeyWord"].ToString().Trim());
                }
            }

            //链接的正则
            string aa = "https?://.[^\"]+";
            string[] sDate;

            sb = new StringBuilder();
            sb.Append("");

            //TbReleaseInfo ri = new TbReleaseInfo();
            
            //按关键字循环
            foreach (KeyValuePair<string,string> kv in keywords)
            {
                string k = kv.Key;
                string v = kv.Value.Substring(1);
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    lbAll.Text = "正在搜索事件为<" + k.Substring(0,k.IndexOf("-")) + ">的数据.";
                    lbAll.ForeColor = Color.DarkBlue;
                }));

                //取得关键字
                string keys = v;
                //组成查询字串
                //string url = "http://www.baidu.com/s?wd=" + keys + "&rn=20";
                string url = "http://news.baidu.com/ns?rn=20&word=" + keys;
                //得到结果放在数组内
                List<string> lis = new List<string>();
                lis = HtmlUtil.GetElementsByClassList(HtmlUtil.getHtml(url, "utf-8"), "result");
                
                //如果没取到,就结束本次循环
                if (lis == null) return;
                //webBrowser1.Navigate(url);
                if (lis.Count <= 0)
                {
                    continue;
                }
                //循环时判断是否要验证
                bool isThere = false;

                for (int i = 0; i < lis.Count; i++)
                {
                    if (Program.ProClose == true) break;

                    ModelReleaseInfo mri = new ModelReleaseInfo();

                    //发布日期的赋值
                    sDate = HtmlUtil.GetElementsByTagAndClass(lis[i], "span", "g");
                    if (sDate.Length <= 0) continue;

                    mri.ReleaseDate = HtmlUtil.NoHTML(sDate[0]);
                    mri.ReleaseDate = mri.ReleaseDate.Substring(mri.ReleaseDate.IndexOf('2'), 17);

                    //判断日期
                    DateTime ddt;
                    if (DateTime.TryParse(mri.ReleaseDate, out ddt))
                    {
                    }
                    else
                    {
                        //百度的快照日期有时会是9位或8位,如果是这种情况,那么按规则去掉
                        mri.ReleaseDate = mri.ReleaseDate.Substring(1, 9);
                        if (DateTime.TryParse(mri.ReleaseDate, out ddt))
                        {
                        }
                        else
                        {
                            mri.ReleaseDate = mri.ReleaseDate.Substring(1, 8);
                        }
                    }
                    //处理日期
                    try
                    {
                        mri.ReleaseDate = DateTime.Parse(mri.ReleaseDate).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    catch (Exception ex)
                    {
                        StreamWriter sw = File.AppendText("log.txt");
                        sw.WriteLine(DateTime.Now.ToLongDateString());
                        sw.WriteLine("begin");
                        sw.WriteLine(ex.Message);
                        sw.WriteLine(sb.ToString());
                        sw.WriteLine("end");
                        sw.WriteLine("");

                        sw.Close();

                    }

                    //只拿取三天的内的数据
                    try
                    {
                        if (DateTime.Parse(mri.ReleaseDate) < DateTime.Now.AddDays(-30)) continue;
                    }
                    catch (Exception ex) { continue; }
                    try
                    {
                        //得到标题
                        mri.Title = HtmlUtil.NoHTML(HtmlUtil.GetElementsByTagName(lis[i], "h3")[0]);
                        string[] temp = HtmlUtil.GetElementsByClass(lis[i], "c-summary");

                        //如果未取到内容部分,就跳出
                        if (temp.Length == 0)
                            continue;

                        mri.Contexts = HtmlUtil.NoHTML(temp[0]);
                        mri.InfoSource = HtmlUtil.GetListByHtml("",HtmlUtil.GetElementsByTagName(lis[i], "a")[0], aa)[0];

                        //去掉重复
                        if (isThere)
                        {
                            continue;
                        }
                        else
                        {
                            if (UrlThereare(mri.Title, this.dtqueryinfo, dtWebQueryInfo, false) != 0)
                            { isThere = true; continue; }
                        }

                        mri.KeyWords = k;
                        mri.CollectDate = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        mri.Snapshot = "";
                        mri.ReleaseName = "";
                        mri.Sheng = "";
                        mri.Shi = "";
                        mri.Xian = "";
                        mri.WebName = "百度";
                        mri.Pid = 0;
                        mri.Part = GetParts(mri.Contexts);
                        mri.Comments = 0;
                        mri.Reposts = 0;

                        DataRow dr = dtqueryinfo.NewRow();
                        if (dvAll.RowCount == 0)
                        {
                            dr[0] = 1;
                        }
                        else
                        {
                            dr[0] = int.Parse(dvAll.Rows[dvAll.RowCount - 1].Cells[0].Value.ToString()) + 1;
                        }
                        dr[1] = mri.Title;
                        dr[2] = mri.Contexts;
                        dr[3] = mri.ReleaseDate;
                        dr[4] = mri.InfoSource;
                        dr[5] = mri.KeyWords.Substring(0, k.IndexOf("-"));
                        dr[6] = mri.ReleaseName;
                        dr[7] = mri.CollectDate;
                        dr[8] = mri.Snapshot;
                        dr[9] = mri.WebName;
                        dr[10] = mri.Pid;
                        dr[11] = mri.Part;
                        dr[12] = mri.Reposts;
                        dr[13] = mri.Comments;

                        dtqueryinfo.Rows.InsertAt(dr, 0);

                        if (dtqueryinfo.Rows.Count >= 500)
                        {
                            dtqueryinfo.Rows.RemoveAt(500);
                        }
                        this.BeginInvoke(new MethodInvoker(delegate() {
                            dvAll.Refresh();
                        }));
                    }
                    catch (Exception ex)
                    {
                        StreamWriter sw = File.AppendText("log.txt");
                        sw.WriteLine(DateTime.Now.ToLongDateString());
                        sw.WriteLine("begin");
                        sw.WriteLine(ex.Message);
                        sw.WriteLine(sb.ToString());
                        sw.WriteLine("end");
                        sw.WriteLine("");

                        sw.Close();
                    }

                    try
                    {
                        //得到插入语句
                        if (isThere)
                        {
                            continue;
                        }
                        else
                        {
                            sb.Append(tri.GetInsString(mri) + ";");
                        }

                        //每10次执行一次插入数据库
                        if (sb.ToString().Length != 0)
                        {
                            if (i % 10 == 0)
                            {
                                //执行插入
                                cmd.ExecuteNonQuery(sb.ToString());
                                //清除插入字段串
                                sb.Clear();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        StreamWriter sw = File.AppendText("log.txt");
                        sw.WriteLine(DateTime.Now.ToLongDateString());
                        sw.WriteLine("begin");
                        sw.WriteLine(ex.Message);
                        sw.WriteLine(sb.ToString());
                        sw.WriteLine("end");
                        sw.WriteLine("");

                        sw.Close();
                    }

                }
            }
            try
            {
                if (sb.ToString().Length != 0)
                {
                    //执行插入
                    cmd.ExecuteNonQuery(sb.ToString());
                    //清除插入字段串
                    sb.Clear();
                }
            }
            catch (Exception ex)
            {
                StreamWriter sw = File.AppendText("log.txt");
                sw.WriteLine(DateTime.Now.ToLongDateString());
                sw.WriteLine("begin");
                sw.WriteLine(ex.Message);
                sw.WriteLine(sb.ToString());
                sw.WriteLine("end");
                sw.WriteLine("");

                sw.Close();
            }

            //执行完毕后,重新获取一次数据库的数据
            dtWebQueryInfo = tri.SelReleaseInfo(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd HH:mm:ss"), "0 AND webName='百度'");
            this.BeginInvoke(new MethodInvoker(delegate()
            {
                lbAll.Text = "一轮搜索完毕！";
                lbAll.ForeColor = Color.Red;
            }));

        }
        #endregion

        #region 得到网站的贴吧类数据
        /// <summary>
        /// 得到网站的贴吧类数据
        /// </summary>
        private void GetWebTieBaInfo()
        {
            this.BeginInvoke(new MethodInvoker(delegate()
            {
                lbtieba.Text = "";
                lbtieba.Visible = true;
            }));

            //相似链接
            string Similar = "";

            DataBaseServer.SQLitecommand cmd = new SQLitecommand();
			//得到关键字列表
			DataTable dtkey;
			DataTable dtParts;
			dtkey = cmd.GetTabel("select * from Keywords");
			dtParts = cmd.GetTabel("SELECT * FROM partword");
            //得到相似表
            DataTable dtXs = new DataTable();
            dtXs = cmd.GetTabel("Select * from WebAddress WHERE pid=5");

            //相似表中的被抓取网址
            string webInfo = "";

            //要过滤链接中首页的正则
            string strTopFormat = "https?://.+/";
            List<string> strTop = new List<string>();
            sb = new StringBuilder();
            sb.Append("");
            string filterStr = "";

            #region 读取相似度表中的数据据,循环抓取
            for (int xs = 0; xs < dtXs.Rows.Count; xs++)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    lbtieba.ForeColor = Color.DarkBlue;
                    lbtieba.Text = "正在搜索:" + dtXs.Rows[xs]["name"].ToString();
                }));

                //读取相似表中要抓取的网址
                webInfo = HtmlUtil.getHtml(dtXs.Rows[xs]["url"].ToString(), "");
                //读取相似链接
                Similar = dtXs.Rows[xs]["likeurl"].ToString();

                //取出
                //string[] strA = HtmlUtil.GetElementsByTagName(webInfo, "a");
                List<string> strList = HtmlUtil.GetElementsByTagNameList(webInfo, "a");

                string strURLformat = "https?://.[^\"]+";

                TbReleaseInfo ri = new TbReleaseInfo();

                string[] strA = GetLIstDate(strList.Distinct());
                #region 逐个链接判断
                //循环时判断是否要验证
                bool isThere = false;

                for (int i = 0; i < strA.Length; i++)
                {
                    if (Program.ProClose == true) break;
                    Application.DoEvents();
                    Dictionary<string, int> events = new Dictionary<string, int>();
                    //创建数据对象
                    ModelReleaseInfo newsInfo = new ModelReleaseInfo();
                    try
                    {
                        //得到目标网址中的所有链接,如果未得到,那么就继续读取下一个
                        string tmp = strA[i];
                        newsInfo.Title = HtmlUtil.NoHTML(tmp);
                        //得到目标网址中的所有链接,如果未得到,那么就继续读取下一个
                        for (int j = 0; j < dtkey.Rows.Count; j++)
                        {
                            //Application.DoEvents();
                            string[] keys = dtkey.Rows[j][4].ToString().Split(new char[] { ' ' });
                            if (!events.ContainsKey(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString()))
                            {
                                events.Add(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString(), 1);
                                foreach (string k in keys)
                                {
                                    if (!strA[i].ToLower().Contains(k.ToLower()))
                                    {
                                        events.Remove(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString());
                                        break;
                                    }
                                }
                            }
                        }
                        foreach (KeyValuePair<string, int> ev in events)
                        {
                            if (ev.Value == 1)
                            {
                                newsInfo.KeyWords += "," + ev.Key.Split(new char[] { '-' })[0] + "-" + int.Parse(ev.Key.Split(new char[] { '-' })[1]);
                            }
                        }

                        string[] _a = HtmlUtil.GetListByHtml(dtXs.Rows[xs]["url"].ToString(), strA[i], strURLformat);
                        if (_a.Length > 0)
                        {
                            strA[i] = _a[0];
                        }
                        else
                        {
                            strA[i] = "";
                        }
                        //处理含有单引号的链接
                        strA[i] = HtmlUtil.UrlCl(strA[i]);

                        //处理单引号的链接
                        if (strA[i].IndexOf("'") != -1)
                        {
                            strA[i] = HtmlUtil.GetstringByHtmlArray(strA[i], "https?://.[^\']+");
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    //得到相似值,大于0.70的认为相同,并开始抓取
                    if (HtmlUtil.getSimilarDegree(Similar, strA[i]) >= 0.70)
                    {
                        //判断这个链接是否已经在库中或者列表中,如果存在,此次就不再执行
                        strTop = HtmlUtil.GetListByHtmlArray(strA[i], strTopFormat);
                        if (strTop.Count != 0)
                        {
                            continue;//同新闻,如果将首页去掉
                        }

                        if (isThere)
                        {
                            continue;
                        }
                        else
                        {
                            if (UrlThereare(strA[i], this.dttiebainfo, dtTieBaInfo, true) != 0) { 
                                //isThere = true; 
                                continue; 
                            }
                        }
                        Thread.Sleep(2000);
                        //得到此链接的源码
                        webInfo = HtmlUtil.getHtml(strA[i], "");
                        if (webInfo.Length == 0) { continue; }

                        try
                        {
                            //流水+1
                            newsInfo.Uid = this.dvAll.Rows.Count + 1;

                            //标题
                            //string[] strT = HtmlUtil.GetElementsByTagName(webInfo, "title");
                            //if (strT.Length == 0)
                            //{
                            //    continue;
                            //}
                            //else
                            //{
                            //    newsInfo.Title = HtmlUtil.NoHTML(HtmlUtil.GetElementsByTagName(webInfo, "title")[0]);
                            //}

                            //得到正文,以P标签来区分
                            string[] strContext = HtmlUtil.GetElementsByClass(webInfo, "tieba");
                            //string[] strContext = HtmlUtil.GetElementsByTagName(webInfo, "post_content");
                            newsInfo.Contexts = "";
                            for (int j = 0; j < strContext.Length; j++)
                            {
                                //循环累加正文信息
                                newsInfo.Contexts += HtmlUtil.NoHTML(strContext[j]);
                            }

                            //如果正文信息为空,那么将无法做关键字对照,此条数据舍弃
                            if (newsInfo.Contexts.Length == 0)
                            {
                                continue;
                            }

                            //网站链接
                            newsInfo.InfoSource = strA[i].Trim();

                            //关键字的设置
                            //newsInfo.KeyWords = "";
                            //Dictionary<string, int> events = new Dictionary<string, int>();
                            //for (int j = 0; j < dtkey.Rows.Count; j++)
                            //{
                            //    Application.DoEvents();
                            //    if (!events.ContainsKey(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString()))
                            //    {
                            //        events.Add(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString(), 1);
                            //    }
                            //    if (!newsInfo.Contexts.Contains(dtkey.Rows[j][4].ToString()))
                            //    {
                            //        if (events.ContainsKey(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString()) && events[dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString()] == 1)
                            //        {
                            //            events[dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString()] = 0;
                            //        }
                            //    }
                            //}
                            if (newsInfo.KeyWords == null || newsInfo.KeyWords.Length == 0)
                            {
                                for (int j = 0; j < dtkey.Rows.Count; j++)
                                {
                                    Application.DoEvents();
                                    string[] keys = dtkey.Rows[j][4].ToString().Split(new char[] { ' ' });
                                    if (!events.ContainsKey(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString()))
                                    {
                                        events.Add(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString(), 1);
                                        foreach (string k in keys)
                                        {
                                            if (!newsInfo.Contexts.ToLower().Contains(k.ToLower()))
                                            {
                                                events.Remove(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString());
                                                break;
                                            }
                                        }
                                    }
                                }
                                foreach (KeyValuePair<string, int> ev in events)
                                {
                                    if (ev.Value == 1)
                                    {
                                        newsInfo.KeyWords += "," + ev.Key.Split(new char[] { '-' })[0] + "-" + int.Parse(ev.Key.Split(new char[] { '-' })[1]);
                                    }
                                }
                            }
                            if (newsInfo.KeyWords.Length == 0) { continue; }
                            newsInfo.KeyWords = newsInfo.KeyWords.Substring(1);
                            
                            //收集日期
                            newsInfo.CollectDate = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                            //发布人和发布日期暂时无法取到,手工赋值为空
                            newsInfo.ReleaseDate = "";
                            newsInfo.ReleaseName = "";

                            //网页快照,这里为用户指定生成,如果未选择生成,那么为空
                            newsInfo.Snapshot = "";
                            newsInfo.Sheng = dtXs.Rows[xs]["sheng"] == null ? "" : dtXs.Rows[xs]["sheng"].ToString();
                            newsInfo.Shi = dtXs.Rows[xs]["shi"] == null ? "" : dtXs.Rows[xs]["shi"].ToString();
                            newsInfo.Xian = dtXs.Rows[xs]["xian"] == null ? "" : dtXs.Rows[xs]["xian"].ToString();
                            //网站名
                            newsInfo.WebName = dtXs.Rows[xs]["Name"].ToString();
                            //pid
                            newsInfo.Pid = 5;
                            //part正负判断
                            newsInfo.Part = GetParts(newsInfo.Contexts);
                            //reposts
                            newsInfo.Reposts = 0;
                            //comments
                            newsInfo.Comments = 0;

                            //新建数据行
                            DataRow dr = dttiebainfo.NewRow();
                            if (dvtieba.RowCount == 0)
                            {
                                dr[0] = 1;
                            }
                            else
                            {
                                dr[0] = int.Parse(dvtieba.Rows[dvtieba.RowCount - 1].Cells[0].Value.ToString()) + 1;
                            }
                            //dr[0] = newsInfo.Uid;
                            dr[1] = newsInfo.Title;
                            dr[2] = newsInfo.Contexts;
                            dr[3] = newsInfo.ReleaseDate;
                            dr[4] = newsInfo.InfoSource;
                            dr[5] = newsInfo.KeyWords.Substring(0, newsInfo.KeyWords.IndexOf("-"));
                            dr[6] = newsInfo.ReleaseName;
                            dr[7] = newsInfo.CollectDate;
                            dr[8] = newsInfo.Snapshot;
                            dr[9] = newsInfo.WebName;
                            dr[10] = newsInfo.Pid;
                            dr[11] = newsInfo.Part;
                            dr[12] = newsInfo.Reposts;
                            dr[13] = newsInfo.Comments;

                            //把行加到DT中
                            dttiebainfo.Rows.InsertAt(dr, 0);

                            //数据源刷新
                            if (dttiebainfo.Rows.Count >= 500)
                            {
                                dttiebainfo.Rows.RemoveAt(500);
                            }
                            this.BeginInvoke(new MethodInvoker(delegate()
                            {
                                dvtieba.Refresh();
                            }));
                        }
                        catch (Exception ex)
                        {
                            Comm.WriteErrorLog(ex.StackTrace);
                        }

                        ////总表刷新
                        //dt.Rows.Add(dr);
                        //dvAll.Refresh();

                        //得到插入语句
                        try
                        {
                            if (isThere)
                            {
                                continue;
                            }
                            else
                            {
                                sb.Append(ri.GetInsString(newsInfo) + ";");
                            }

                            //每10次执行一次插入数据库
                            if (sb.ToString().Length != 0)
                            {
                                if (i % 10 == 0)
                                {
                                    filterStr = sb.ToString();
                                    filterStr = filterStr.Replace("[ ", "[");
                                    filterStr = filterStr.Replace(" ]", "]");
                                    //执行插入
                                    cmd.ExecuteNonQuery(filterStr);
                                    //清除插入字段串
                                    sb.Clear();
                                    filterStr = "";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Comm.WriteErrorLog(ex.StackTrace);
                        }
                    }
                }
                #endregion
            }
            #endregion

            try
            {
                if (sb.ToString().Length != 0)
                {
                    filterStr = sb.ToString();
                    filterStr = filterStr.Replace("[ ", "[");
                    filterStr = filterStr.Replace(" ]", "]");
                    //执行插入
                    cmd.ExecuteNonQuery(filterStr);
                    //清除插入字段串
                    sb.Clear();
                    filterStr = "";
                }
            }
            catch (Exception ex)
            {
                Comm.WriteErrorLog(ex.StackTrace);
            }

            //执行完毕后,重新获取一次数据库的数据
            dtTieBaInfo = tri.SelReleaseInfo(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd HH:mm:ss"), "5");
            this.BeginInvoke(new MethodInvoker(delegate()
            {
                lbtieba.Text = "一轮搜索完毕！";
                lbtieba.ForeColor = Color.Red;
            }));

        }
        #endregion

        #region 得到网站的新闻类数据
        /// <summary>
        /// 得到网站的新闻类数据
        /// </summary>
        private void GetWebNewsInfo()
        {
            this.BeginInvoke(new MethodInvoker(delegate()
            {
                lbWeb.Text = "";
                lbWeb.Visible = true;
            }));

            //相似链接
            string Similar = "";

            DataBaseServer.SQLitecommand cmd = new SQLitecommand();
			//得到关键字列表
			DataTable dtkey;
			DataTable dtParts;
			dtkey = cmd.GetTabel("select * from Keywords");
			dtParts = cmd.GetTabel("SELECT * FROM partword");
            //得到相似表
            DataTable dtXs = new DataTable();
            dtXs = cmd.GetTabel("Select * from WebAddress WHERE pid=4");

            //相似表中的被抓取网址
            string webInfo = "";

            //要过滤链接中首页的正则
            string strTopFormat = "https?://.+/";
            List<string> strTop = new List<string>();
            sb = new StringBuilder();
            sb.Append("");
            string filterStr = "";

            #region 读取相似度表中的数据据,循环抓取
            for (int xs = 0; xs < dtXs.Rows.Count; xs++)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    lbWeb.ForeColor = Color.DarkBlue;
                    lbWeb.Text = "正在搜索:" + dtXs.Rows[xs]["name"].ToString();
                }));

                //读取相似表中要抓取的网址
                webInfo = HtmlUtil.getHtml(dtXs.Rows[xs]["url"].ToString(), "");
                //读取相似链接
                Similar = dtXs.Rows[xs]["likeurl"].ToString();

                //取出
                //string[] strA = HtmlUtil.GetElementsByTagName(webInfo, "a");
                List<string> strList = HtmlUtil.GetElementsByTagNameList(webInfo, "a");

                string strURLformat = "https?://.[^\"]+";

                TbReleaseInfo ri = new TbReleaseInfo();

                string[] strA = GetLIstDate(strList.Distinct());
                #region 逐个链接判断
                //循环时判断是否要验证
                bool isThere = false;

                for (int i = 0; i < strA.Length; i++)
                {
                    if (Program.ProClose == true) break;
                    Application.DoEvents();
                    Dictionary<string, int> events = new Dictionary<string, int>();
                    //创建数据对象
                    ModelReleaseInfo newsInfo = new ModelReleaseInfo();
                    try
                    {
                        string tmp = strA[i];
                        newsInfo.Title = HtmlUtil.NoHTML(tmp);
                        //得到目标网址中的所有链接,如果未得到,那么就继续读取下一个
                        for (int j = 0; j < dtkey.Rows.Count; j++)
                        {
                            //Application.DoEvents();
                            string[] keys = dtkey.Rows[j][4].ToString().Split(new char[] { ' ' });
                            if (!events.ContainsKey(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString()))
                            {
                                events.Add(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString(), 1);
                                foreach (string k in keys)
                                {
                                    if (!strA[i].ToLower().Contains(k.ToLower()))
                                    {
                                        events.Remove(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString());
                                        break;
                                    }
                                }
                            }
                        }
                        foreach (KeyValuePair<string, int> ev in events)
                        {
                            if (ev.Value == 1)
                            {
                                newsInfo.KeyWords += "," + ev.Key.Split(new char[] { '-' })[0] + "-" + int.Parse(ev.Key.Split(new char[] { '-' })[1]);
                            }
                        }

                        string[] _a = HtmlUtil.GetListByHtml(dtXs.Rows[xs]["url"].ToString(),strA[i], strURLformat);
                        if (_a.Length > 0)
                        {
                            strA[i] = _a[0];
                        }
                        else
                        {
                            strA[i] = "";
                        }
                        //处理含有单引号的链接
                        strA[i] = HtmlUtil.UrlCl(strA[i]);

                        //处理单引号的链接
                        if (strA[i].IndexOf("'") != -1 || strA[i].IndexOf("\"") != -1)
                        {
                            strA[i] = HtmlUtil.GetstringByHtmlArray(strA[i], "https?://.[^\']+");
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    //得到相似值,大于0.70的认为相同,并开始抓取
                    if (HtmlUtil.getSimilarDegree(Similar, strA[i]) >= 0.70)
                    {
                        //判断这个链接是否已经在库中或者列表中,如果存在,此次就不再执行
                        strTop = HtmlUtil.GetListByHtmlArray(strA[i], strTopFormat);
                        if (strTop.Count != 0)
                        {
                            continue;//同新闻,如果将首页去掉
                        }

                        if (isThere)
                        {
                            continue;
                        }
                        else
                        {
                            if (UrlThereare(strA[i], this.dtnewsinfo, dtWebNewsInfo, true) != 0) { 
                                //isThere = true; 
                                continue; 
                            }
                        }
                        //因为有的网站会出现访问过快的话，会屏蔽访问者，所以在此让线程停止2秒钟。这样的话，会出现总体访问时间过长的问题
                        Thread.Sleep(2000);
                        //得到此链接的源码
                        webInfo = HtmlUtil.getHtml(strA[i], "");
                        if (webInfo.Length == 0) { continue; }

                        
                        try
                        {
                            //流水+1
                            newsInfo.Uid = this.dvAll.Rows.Count + 1;

                            //标题
                            //string[] strT = HtmlUtil.GetElementsByTagName(webInfo, "title");
                            //if (strT.Length == 0)
                            //{
                            //    continue;
                            //}
                            //else
                            //{
                            //    newsInfo.Title = HtmlUtil.NoHTML(HtmlUtil.GetElementsByTagName(webInfo, "title")[0]);
                            //}

                            //得到正文,以P标签来区分
                            string[] strContext = HtmlUtil.GetElementsByTagName(webInfo, "p");
                            newsInfo.Contexts = "";
                            for (int j = 0; j < strContext.Length; j++)
                            {
                                //循环累加正文信息
                                newsInfo.Contexts += HtmlUtil.NoHTML(strContext[j]);
                            }

                            //如果正文信息为空,那么将无法做关键字对照,此条数据舍弃
                            if (events.Count <= 0 && newsInfo.Contexts.Length == 0)
                            {
                                continue;
                            }

                            //网站链接
                            newsInfo.InfoSource = strA[i].Trim();

                            //关键字的设置
                            //newsInfo.KeyWords = "";
                            //Dictionary<string, int> events = new Dictionary<string, int>();
                            //以下判断事件的所有关键词都必须包含在文章里才可以
                            //for (int j = 0; j < dtkey.Rows.Count; j++)
                            //{
                            //    Application.DoEvents();
                            //    if (!events.ContainsKey(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString()))
                            //    {
                            //        events.Add(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString(), 1);
                            //    }
                            //    if (!newsInfo.Contexts.Contains(dtkey.Rows[j][4].ToString()))
                            //    {
                            //        if (events.ContainsKey(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString()) && events[dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString()] == 1)
                            //        {
                            //            events[dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString()] = 0;
                            //        }
                            //    }
                            //}
                            if (newsInfo.KeyWords == null || newsInfo.KeyWords.Length == 0)
                            {
                                for (int j = 0; j < dtkey.Rows.Count; j++)
                                {
                                    Application.DoEvents();
                                    string[] keys = dtkey.Rows[j][4].ToString().Split(new char[] { ' ' });
                                    if (!events.ContainsKey(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString()))
                                    {
                                        events.Add(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString(), 1);
                                        foreach (string k in keys)
                                        {
                                            if (!newsInfo.Contexts.ToLower().Contains(k.ToLower()))
                                            {
                                                events.Remove(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString());
                                                break;
                                            }
                                        }
                                    }
                                }
                                foreach (KeyValuePair<string, int> ev in events)
                                {
                                    if (ev.Value == 1)
                                    {
                                        newsInfo.KeyWords += "," + ev.Key.Split(new char[] { '-' })[0] + "-" + int.Parse(ev.Key.Split(new char[] { '-' })[1]);
                                    }
                                }
                            }
                            if (newsInfo.KeyWords.Length == 0) { continue; }
                            newsInfo.KeyWords = newsInfo.KeyWords.Substring(1);

                            //收集日期
                            newsInfo.CollectDate = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                            //发布人和发布日期暂时无法取到,手工赋值为空
                            newsInfo.ReleaseDate = "";
                            newsInfo.ReleaseName = "";

                            //网页快照,这里为用户指定生成,如果未选择生成,那么为空
                            newsInfo.Snapshot = "";
                            newsInfo.Sheng = dtXs.Rows[xs]["sheng"] == null ? "" : dtXs.Rows[xs]["sheng"].ToString();
                            newsInfo.Shi = dtXs.Rows[xs]["shi"] == null ? "" : dtXs.Rows[xs]["shi"].ToString();
                            newsInfo.Xian = dtXs.Rows[xs]["xian"] == null ? "" : dtXs.Rows[xs]["xian"].ToString();
                            //网站名
                            newsInfo.WebName = dtXs.Rows[xs]["Name"].ToString();
                            //pid
                            newsInfo.Pid = 4;
                            //part正负判断
                            newsInfo.Part = GetParts(newsInfo.Contexts);
                            //reposts
                            newsInfo.Reposts = 0;
                            //comments
                            newsInfo.Comments = 0;

                            //新建数据行
                            DataRow dr = dtnewsinfo.NewRow();
                            if (dvWeb.RowCount == 0)
                            {
                                dr[0] = 1;
                            }
                            else
                            {
                                dr[0] = int.Parse(dvWeb.Rows[dvWeb.RowCount - 1].Cells[0].Value.ToString()) + 1;
                            }
                            //dr[0] = newsInfo.Uid;
                            dr[1] = newsInfo.Title;
                            dr[2] = newsInfo.Contexts;
                            dr[3] = newsInfo.ReleaseDate;
                            dr[4] = newsInfo.InfoSource;
                            dr[5] = newsInfo.KeyWords.Substring(0, newsInfo.KeyWords.IndexOf("-"));
                            dr[6] = newsInfo.ReleaseName;
                            dr[7] = newsInfo.CollectDate;
                            dr[8] = newsInfo.Snapshot;
                            dr[9] = newsInfo.WebName;
                            dr[10] = newsInfo.Pid;
                            dr[11] = newsInfo.Part;
                            dr[12] = newsInfo.Reposts;
                            dr[13] = newsInfo.Comments;

                            //把行加到DT中
                            dtnewsinfo.Rows.InsertAt(dr, 0);

                            //数据源刷新
                            if (dtnewsinfo.Rows.Count >= 500)
                            {
                                dtnewsinfo.Rows.RemoveAt(500);
                            }
                            this.BeginInvoke(new MethodInvoker(delegate() {
                                dvWeb.Refresh();
                            }));
                        }
                        catch (Exception ex)
                        {
                            Comm.WriteErrorLog(ex.StackTrace);
                        }

                        ////总表刷新
                        //dt.Rows.Add(dr);
                        //dvAll.Refresh();

                        //得到插入语句
                        try
                        {
                            if (isThere)
                            {
                                continue;
                            }
                            else
                            {
                                sb.Append(ri.GetInsString(newsInfo) + ";");
                            }

                            //每10次执行一次插入数据库
                            if (sb.ToString().Length != 0)
                            {
                                if (i % 10 == 0)
                                {
                                    filterStr = sb.ToString();
                                    filterStr = filterStr.Replace("[ ", "[");
                                    filterStr = filterStr.Replace(" ]", "]");
                                    //执行插入
                                    cmd.ExecuteNonQuery(filterStr);
                                    //清除插入字段串
                                    sb.Clear();
                                    filterStr = "";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //StreamWriter sw = File.AppendText("log.txt");
                            //sw.WriteLine(DateTime.Now.ToLongDateString());
                            //sw.WriteLine("begin");
                            //sw.WriteLine(ex.Message);
                            //sw.WriteLine(sb.ToString());
                            //sw.WriteLine("end");
                            //sw.WriteLine("");

                            //sw.Close();
                        }
                    }
                }
                #endregion
            }
            #endregion

            try
            {
                if (sb.ToString().Length != 0)
                {
                    filterStr = sb.ToString();
                    filterStr = filterStr.Replace("[ ", "[");
                    filterStr = filterStr.Replace(" ]", "]");
                    //执行插入
                    cmd.ExecuteNonQuery(filterStr);
                    //清除插入字段串
                    sb.Clear();
                    filterStr = "";
                }
            }
            catch (Exception ex)
            {
                Comm.WriteErrorLog(ex.StackTrace);
            }

            //执行完毕后,重新获取一次数据库的数据
            dtWebNewsInfo = tri.SelReleaseInfo(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd HH:mm:ss"), "0 AND webName<>'百度'");
            this.BeginInvoke(new MethodInvoker(delegate()
            {
                lbWeb.Text = "一轮搜索完毕！";
                lbWeb.ForeColor = Color.Red;
            }));

        }
        #endregion

        #region 得到网站的博客类数据
        /// <summary>
        /// 得到网站的博客类数据
        /// </summary>
        private void GetWebBlogInfo()
        {
            this.BeginInvoke(new MethodInvoker(delegate()
            {
                lbBlog.Text = "";
                lbBlog.Visible = true;
            }));

            //相似链接
            string Similar = "";

            DataBaseServer.SQLitecommand cmd = new SQLitecommand();
			//得到关键字列表
			DataTable dtkey;
			DataTable dtParts;
			dtkey = cmd.GetTabel("select * from Keywords");
			dtParts = cmd.GetTabel("SELECT * FROM partword");
            //得到相似表
            DataTable dtXs = new DataTable();
            dtXs = cmd.GetTabel("Select * from WebAddress WHERE pid=1");

            //相似表中的被抓取网址
            string webInfo = "";
            sb = new StringBuilder();
            sb.Append("");
            string filterStr = "";

            //要过滤链接中首页的正则
            string strTopFormat = "https?://.+/";
            List<string> strTop = new List<string>();

            #region 读取相似度表中的数据据,循环抓取
            for (int xs = 0; xs < dtXs.Rows.Count; xs++)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    lbBlog.ForeColor = Color.DarkBlue;
                    lbBlog.Text = "正在搜索:" + dtXs.Rows[xs]["name"].ToString();
                }));

                //读取相似表中要抓取的网址
                webInfo = HtmlUtil.getHtml(dtXs.Rows[xs]["url"].ToString(), "");
                //读取相似链接
                Similar = dtXs.Rows[xs]["likeurl"].ToString();

                //取出
                List<string> strList = HtmlUtil.GetElementsByTagNameList(webInfo, "a");
                //string[] strA = HtmlUtil.GetElementsByTagName(webInfo, "a");

                string strURLformat = "https?://.[^\"]+";

                TbReleaseInfo ri = new TbReleaseInfo();

                string[] strA = GetLIstDate(strList.Distinct());

                #region 逐个链接判断
                //循环时判断是否要验证
                bool isThere = false;

                for (int i = 0; i < strA.Length; i++)
                {
                    if (Program.ProClose == true) break;

                    Dictionary<string, int> events = new Dictionary<string, int>();
                    //创建数据对象
                    ModelReleaseInfo newsInfo = new ModelReleaseInfo();
                    try
                    {
                        string tmp = strA[i];
                        newsInfo.Title = HtmlUtil.NoHTML(tmp);
                        //得到目标网址中的所有链接,如果未得到,那么就继续读取下一个
                        for (int j = 0; j < dtkey.Rows.Count; j++)
                        {
                            //Application.DoEvents();
                            string[] keys = dtkey.Rows[j][4].ToString().Split(new char[] { ' ' });
                            if (!events.ContainsKey(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString()))
                            {
                                events.Add(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString(), 1);
                                foreach (string k in keys)
                                {
                                    if (!strA[i].ToLower().Contains(k.ToLower()))
                                    {
                                        events.Remove(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString());
                                        break;
                                    }
                                }
                            }
                        }
                        foreach (KeyValuePair<string, int> ev in events)
                        {
                            if (ev.Value == 1)
                            {
                                newsInfo.KeyWords += "," + ev.Key.Split(new char[] { '-' })[0] + "-" + int.Parse(ev.Key.Split(new char[] { '-' })[1]);
                            }
                        }

                        string[] _a = HtmlUtil.GetListByHtml(dtXs.Rows[xs]["url"].ToString(), strA[i], strURLformat);
                        if (_a.Length > 0)
                        {
                            strA[i] = _a[0];
                        }
                        else
                        {
                            strA[i] = "";
                        }
                        //处理含有单引号的链接
                        strA[i] = HtmlUtil.UrlCl(strA[i]);

                        //处理单引号的链接
                        if (strA[i].IndexOf("'") != -1)
                        {
                            strA[i] = HtmlUtil.GetstringByHtmlArray(strA[i], "https?://.[^\']+");
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                    //得到相似值,大于0.70的认为相同,并开始抓取
                    //this.listBox1.Items.Add((strA[i]) + "-" + HtmlUtil.getSimilarDegree(Similar, strA[i]).ToString());
                    if (!String.IsNullOrWhiteSpace(strA[i]) && HtmlUtil.getSimilarDegree(Similar, strA[i]) >= 0.7)
                    {
                        //判断这个链接是否已经在库中或者列表中,如果存在,此次就不再执行
                        strTop = HtmlUtil.GetListByHtmlArray(strA[i], strTopFormat);
                        if (strTop.Count != 0)
                        {
                            continue;//同新闻,如果将首页去掉
                        }

                        if (isThere)
                        {
                            continue;
                        }
                        else
                        {
                            if (UrlThereare(strA[i], this.dtbloginfo, dtWebBlogInfo, true) != 0) { 
                                //isThere = true; 
                                continue; 
                            }
                        }
                        Thread.Sleep(2000);
                        //得到此链接的源码
                        webInfo = HtmlUtil.getHtml(strA[i], "");
                        if (webInfo.Length == 0) { continue; }

                        try
                        {
                            //流水+1
                            newsInfo.Uid = this.dvAll.Rows.Count + 1;

                            //标题:如果连标题都没有,那么忽略掉这个页
                            //try
                            //{
                            //    newsInfo.Title = HtmlUtil.NoHTML(HtmlUtil.GetElementsByTagName(webInfo, "title")[0]);
                            //}
                            //catch (Exception)
                            //{
                            //    continue;
                            //}

                            //得到正文,以P标签来区分
                            string[] strContext = HtmlUtil.GetElementsByTagName(webInfo, "p");
                            newsInfo.Contexts = "";
                            for (int j = 0; j < strContext.Length; j++)
                            {
                                //循环累加正文信息
                                newsInfo.Contexts += HtmlUtil.NoHTML(strContext[j]);
                            }

                            //如果正文信息为空,那么将无法做关键字对照,此条数据舍弃
                            if (events.Count<=0 && newsInfo.Contexts.Length == 0)
                            {
                                continue;
                            }

                            //网站链接
                            newsInfo.InfoSource = strA[i].Trim();

                            //关键字的设置
                            //newsInfo.KeyWords = "";
                            //List<string> events2 = new List<string>();
                            Dictionary<string, int> events1 = new Dictionary<string, int>();
                            //for (int j = 0; j < dtkey.Rows.Count; j++)
                            //{
                            //    Application.DoEvents();
                            //    if (!events1.ContainsKey(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString()))
                            //    {
                            //        events1.Add(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString(), 1);
                            //    }
                            //    if (!newsInfo.Contexts.Contains(dtkey.Rows[j][4].ToString()))
                            //    {
                            //        if (events1.ContainsKey(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString()) && events1[dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString()] == 1)
                            //        {
                            //            events1[dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString()] = 0;
                            //        }
                            //    }
                            //}
                            if (newsInfo.KeyWords == null || newsInfo.KeyWords.Length == 0)
                            {
                                for (int j = 0; j < dtkey.Rows.Count; j++)
                                {
                                    Application.DoEvents();
                                    string[] keys = dtkey.Rows[j][4].ToString().Split(new char[] { ' ' });
                                    if (!events1.ContainsKey(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString()))
                                    {
                                        events1.Add(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString(), 1);
                                        foreach (string k in keys)
                                        {
                                            if (!newsInfo.Contexts.ToLower().Contains(k.ToLower()))
                                            {
                                                events1.Remove(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString());
                                                break;
                                            }
                                        }
                                    }
                                }
                                foreach (KeyValuePair<string, int> ev in events1)
                                {
                                    if (ev.Value == 1)
                                    {
                                        newsInfo.KeyWords += "," + ev.Key.Split(new char[] { '-' })[0] + "-" + int.Parse(ev.Key.Split(new char[] { '-' })[1]);
                                    }
                                }
                            }
                            if (newsInfo.KeyWords.Length == 0) { continue; }
                            newsInfo.KeyWords = newsInfo.KeyWords.Substring(1);
                            //收集日期
                            newsInfo.CollectDate = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                            //发布人和发布日期暂时无法取到,手工赋值为空
                            newsInfo.ReleaseDate = "";
                            newsInfo.ReleaseName = "";

                            //网页快照,这里为用户指定生成,如果未选择生成,那么为空
                            newsInfo.Snapshot = "";
                            newsInfo.Sheng = dtXs.Rows[xs]["sheng"] == null ? "" : dtXs.Rows[xs]["sheng"].ToString();
                            newsInfo.Shi = dtXs.Rows[xs]["shi"] == null ? "" : dtXs.Rows[xs]["shi"].ToString();
                            newsInfo.Xian = dtXs.Rows[xs]["xian"] == null ? "" : dtXs.Rows[xs]["xian"].ToString();
                            //网站名
                            newsInfo.WebName = dtXs.Rows[xs]["Name"].ToString();
                            //pid
                            newsInfo.Pid = 1;
                            //part正负判断
                            newsInfo.Part = GetParts(newsInfo.Contexts);
                            //reposts
                            newsInfo.Reposts = 0;
                            //comments
                            newsInfo.Comments = 0;

                            //新建数据行
                            DataRow dr = dtbloginfo.NewRow();
                            if (dvBlog.RowCount == 0)
                            {
                                dr[0] = 1;
                            }
                            else
                            {
                                dr[0] = int.Parse(dvBlog.Rows[dvBlog.RowCount - 1].Cells[0].Value.ToString()) + 1;
                            }

                            dr[1] = newsInfo.Title;
                            dr[2] = newsInfo.Contexts;
                            dr[3] = newsInfo.ReleaseDate;
                            dr[4] = newsInfo.InfoSource;
                            dr[5] = newsInfo.KeyWords.Substring(0, newsInfo.KeyWords.IndexOf("-"));
                            dr[6] = newsInfo.ReleaseName;
                            dr[7] = newsInfo.CollectDate;
                            dr[8] = newsInfo.Snapshot;
                            dr[9] = newsInfo.WebName;
                            dr[10] = newsInfo.Pid;
                            dr[11] = newsInfo.Part;
                            dr[12] = newsInfo.Reposts;
                            dr[13] = newsInfo.Comments;
                            this.BeginInvoke(new MethodInvoker(delegate() {
                                //把行加到DT中
                                dtbloginfo.Rows.InsertAt(dr, 0);

                                //数据源刷新
                                if (dtbloginfo.Rows.Count >= 500)
                                {
                                    dtbloginfo.Rows.RemoveAt(500);
                                }
                                dvBlog.Refresh();
                            }));
                        }
                        catch (Exception ex)
                        {
                            Comm.WriteErrorLog(ex.StackTrace);
                        }

                        ////总表刷新
                        //dt.Rows.Add(dr);
                        //dvAll.Refresh();

                        //得到插入语句
                        try
                        {
                            if (isThere)
                            {
                                continue;
                            }
                            else
                            {
                                sb.Append(ri.GetInsString(newsInfo) + ";");
                            }

                            //每10次执行一次插入数据库
                            if (sb.ToString().Length != 0)
                            {
                                if (i % 10 == 0)
                                {
                                    filterStr = sb.ToString();
                                    filterStr = filterStr.Replace("[ ", "[");
                                    filterStr = filterStr.Replace(" ]", "]");
                                    //执行插入
                                    cmd.ExecuteNonQuery(filterStr);
                                    //清除插入字段串
                                    sb.Clear();
                                    filterStr = "";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Comm.WriteErrorLog(ex.StackTrace);
                        }
                    }
                }
                #endregion
            }
            #endregion

            try
            {
                if (sb.ToString().Length != 0)
                {
                    filterStr = sb.ToString();
                    filterStr = filterStr.Replace("[ ", "[");
                    filterStr = filterStr.Replace(" ]", "]");
                    //执行插入
                    cmd.ExecuteNonQuery(filterStr);
                    //清除插入字段串
                    sb.Clear();
                    filterStr = "";
                }
            }
            catch (Exception ex)
            {
                Comm.WriteErrorLog(ex.StackTrace);
            }

            //执行完毕后,重新获取一次数据库的数据
            dtWebBlogInfo = tri.SelReleaseInfo(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd HH:mm:ss"), "1");
            this.BeginInvoke(new MethodInvoker(delegate()
            {
                lbBlog.Text = "一轮搜索完毕！";
                lbBlog.ForeColor = Color.Red;
                lbBlog.Visible = true;
            }));

        }
        #endregion

        #region 得到网站的论坛类数据
        /// <summary>
        /// 得到网站的论坛类数据
        /// </summary>
        private void GetWebBBSInfo()
        {
            this.BeginInvoke(new MethodInvoker(delegate()
            {
                lbBBs.Text = "";
                lbBBs.Visible = true;
            }));

            //相似链接
            string Similar = "";

            DataBaseServer.SQLitecommand cmd = new SQLitecommand();
			//得到关键字列表
			DataTable dtkey;
			DataTable dtParts;
			dtkey = cmd.GetTabel("select * from Keywords");
			dtParts = cmd.GetTabel("SELECT * FROM partword");
            //得到相似表
            DataTable dtXs = new DataTable();
            dtXs = cmd.GetTabel("Select * from WebAddress WHERE pid=2");

            //相似表中的被抓取网址
            string webInfo = "";

            sb = new StringBuilder();
            sb.Append("");
            string filterStr = "";

            //要过滤链接中首页的正则
            string strTopFormat = "https?://.+/";
            List<string> strTop = new List<string>();

            #region 读取相似度表中的数据据,循环抓取
            for (int xs = 0; xs < dtXs.Rows.Count; xs++)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    lbBBs.ForeColor = Color.DarkBlue;
                    lbBBs.Text = "正在搜索:" + dtXs.Rows[xs]["name"].ToString();
                }));

                //读取相似表中要抓取的网址
                webInfo = HtmlUtil.getHtml(dtXs.Rows[xs]["url"].ToString(), "");
                //读取相似链接
                Similar = dtXs.Rows[xs]["likeurl"].ToString();

                //取出
                List<string> strList = HtmlUtil.GetElementsByTagNameList(webInfo, "a");
                //string[] strA = HtmlUtil.GetElementsByTagName(webInfo, "a");

                string strURLformat = "(?<=href=['\"]?)[^'\"<>]+";

                TbReleaseInfo ri = new TbReleaseInfo();

                string[] strA = GetLIstDate(strList.Distinct());
                #region 逐个链接判断
                //循环时判断是否要验证
                bool isThere = false;

                for (int i = 0; i < strA.Length; i++)
                {
                    if (Program.ProClose == true) break;
                    Dictionary<string, int> events = new Dictionary<string, int>();
                    //创建数据对象
                    ModelReleaseInfo newsInfo = new ModelReleaseInfo();
                    try
                    {
                        string tmp = strA[i];
                        newsInfo.Title = HtmlUtil.NoHTML(tmp);
                        //得到目标网址中的所有链接,如果未得到,那么就继续读取下一个
                        for (int j = 0; j < dtkey.Rows.Count; j++)
                        {
                            //Application.DoEvents();
                            string[] keys = dtkey.Rows[j][4].ToString().Split(new char[] { ' ' });
                            if (!events.ContainsKey(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString()))
                            {
                                events.Add(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString(), 1);
                                foreach (string k in keys)
                                {
                                    if (!strA[i].ToLower().Contains(k.ToLower()))
                                    {
                                        events.Remove(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString());
                                        break;
                                    }
                                }
                            }
                        }
                        foreach (KeyValuePair<string, int> ev in events)
                        {
                            if (ev.Value == 1)
                            {
                                newsInfo.KeyWords += "," + ev.Key.Split(new char[] { '-' })[0] + "-" + int.Parse(ev.Key.Split(new char[] { '-' })[1]);
                            }
                        }

                        string[] _a = HtmlUtil.GetListByHtml(dtXs.Rows[xs]["url"].ToString(), strA[i], strURLformat);
                        if (_a.Length > 0)
                        {
                            strA[i] = _a[0];
                        }
                        else
                        {
                            strA[i] = "";
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    //得到相似值,大于0.70的认为相同,并开始抓取
                    //this.listBox1.Items.Add((strA[i]) + "-" + HtmlUtil.getSimilarDegree(Similar, strA[i]).ToString() + "-" + i.ToString());
                    if (HtmlUtil.getSimilarDegree(Similar, strA[i]) >= 0.7)
                    {
                        //判断这个链接是否已经在库中或者列表中,如果存在,此次就不再执行
                        //去列表页,具体抓取数据是不要抓取列表页的
                        strTop = HtmlUtil.GetListByHtmlArray(strA[i], strTopFormat);
                        if (strTop.Count != 0)
                        {
                            continue;//同新闻,如果将首页去掉
                        }

                        strTop = HtmlUtil.GetListByHtmlArray(strA[i], "http://[^+/]+");
                        if (strTop.Count != 0)
                        {
                            continue;//同新闻,如果将首页去掉
                        }

                        //判断是否要组合链接
                        if (strA[i].Substring(0, 4).ToLower() != "http")
                        {
                            //得到网页首页
                            WebtopUrl = dtXs.Rows[xs]["url"].ToString();
                            if (WebtopUrl.IndexOf(".com/") > 0)
                                WebtopUrl = WebtopUrl.Substring(0, WebtopUrl.IndexOf(".com/") + 4);

                            //组合链接
                            strA[i] = WebtopUrl + strA[i];
                        }

                        if (isThere)
                        {
                            continue;
                        }
                        else
                        {
                            if (UrlThereare(strA[i], this.dtbbsinfo, dtWebBBSInfo, true) != 0) { 
                                //isThere = true; 
                                continue; 
                            }
                        }
                        
                        try
                        {
                            Thread.Sleep(2000);
                            //得到此链接的源码
                            webInfo = HtmlUtil.getHtml(strA[i], "");
                            if (webInfo.Length == 0) { continue; }
                            

                            //流水+1
                            newsInfo.Uid = this.dvAll.Rows.Count + 1;

                            //得到正文,以P标签来区分
                            string[] strContext = HtmlUtil.GetElementsByTagName(webInfo, "p");
                            newsInfo.Contexts = "";
                            for (int j = 0; j < strContext.Length; j++)
                            {
                                //循环累加正文信息
                                newsInfo.Contexts += HtmlUtil.NoHTML(strContext[j]);
                            }

                            newsInfo.Contexts = HtmlUtil.getConnect(webInfo);

                            //如果正文信息为空,那么将无法做关键字对照,此条数据舍弃
                            if (events.Count <= 0 && newsInfo.Contexts.Length == 0)
                            {
                                continue;
                            }

                            //网站链接
                            newsInfo.InfoSource = strA[i].Trim();

                            //关键字的设置
                            //newsInfo.KeyWords = "";
                            Dictionary<string, int> events2 = new Dictionary<string, int>();
                            //for (int j = 0; j < dtkey.Rows.Count; j++)
                            //{
                            //    Application.DoEvents();
                            //    if (!events2.ContainsKey(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString()))
                            //    {
                            //        events2.Add(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString(), 1);
                            //    }
                            //    if (!newsInfo.Contexts.Contains(dtkey.Rows[j][4].ToString()))
                            //    {
                            //        if (events2.ContainsKey(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString()) && events2[dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString()] == 1)
                            //        {
                            //            events2[dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString()] = 0;
                            //        }
                            //    }
                            //}
                            if (newsInfo.KeyWords == null || newsInfo.KeyWords.Length == 0)
                            {
                                for (int j = 0; j < dtkey.Rows.Count; j++)
                                {
                                    Application.DoEvents();
                                    string[] keys = dtkey.Rows[j][4].ToString().Split(new char[] { ' ' });
                                    if (!events2.ContainsKey(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString()))
                                    {
                                        events2.Add(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString(), 1);
                                        foreach (string k in keys)
                                        {
                                            if (!newsInfo.Contexts.ToLower().Contains(k.ToLower()))
                                            {
                                                events2.Remove(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString());
                                                break;
                                            }
                                        }
                                    }
                                }
                                foreach (KeyValuePair<string, int> ev in events2)
                                {
                                    if (ev.Value == 1)
                                    {
                                        newsInfo.KeyWords += "," + ev.Key.Split(new char[] { '-' })[0] + "-" + int.Parse(ev.Key.Split(new char[] { '-' })[1]);
                                    }
                                }
                            }
                            if (newsInfo.KeyWords.Length == 0) { continue; }
                            newsInfo.KeyWords = newsInfo.KeyWords.Substring(1);

                            //收集日期
                            newsInfo.CollectDate = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                            //发布人和发布日期暂时无法取到,手工赋值为空
                            newsInfo.ReleaseDate = "";
                            newsInfo.ReleaseName = "";

                            //网页快照,这里为用户指定生成,如果未选择生成,那么为空
                            newsInfo.Snapshot = "";
                            newsInfo.Sheng = dtXs.Rows[xs]["sheng"] == null ? "" : dtXs.Rows[xs]["sheng"].ToString();
                            newsInfo.Shi = dtXs.Rows[xs]["shi"] == null ? "" : dtXs.Rows[xs]["shi"].ToString();
                            newsInfo.Xian = dtXs.Rows[xs]["xian"] == null ? "" : dtXs.Rows[xs]["xian"].ToString();
                            //网站名
                            newsInfo.WebName = dtXs.Rows[xs]["Name"].ToString();
                            //pid
                            newsInfo.Pid = 2;
                            //part正负判断
                            newsInfo.Part = GetParts(newsInfo.Contexts);
                            //reposts
                            newsInfo.Reposts = 0;
                            //comments
                            newsInfo.Comments = 0;

                            //新建数据行
                            DataRow dr = dtbbsinfo.NewRow();
                            if (dvBBs.RowCount == 0)
                            {
                                dr[0] = 1;
                            }
                            else
                            {
                                dr[0] = int.Parse(dvBBs.Rows[dvBBs.RowCount - 1].Cells[0].Value.ToString()) + 1;
                            }

                            dr[1] = newsInfo.Title;
                            dr[2] = newsInfo.Contexts;
                            dr[3] = newsInfo.ReleaseDate;
                            dr[4] = newsInfo.InfoSource;
                            dr[5] = newsInfo.KeyWords.Substring(0, newsInfo.KeyWords.IndexOf("-"));
                            dr[6] = newsInfo.ReleaseName;
                            dr[7] = newsInfo.CollectDate;
                            dr[8] = newsInfo.Snapshot;
                            dr[9] = newsInfo.WebName;
                            dr[10] = newsInfo.Pid;
                            dr[11] = newsInfo.Part;
                            dr[12] = newsInfo.Reposts;
                            dr[13] = newsInfo.Comments;

                            //把行加到DT中
                            dtbbsinfo.Rows.InsertAt(dr, 0);

                            //数据源刷新                        
                            if (dtbbsinfo.Rows.Count >= 500)
                            {
                                dtbbsinfo.Rows.RemoveAt(500);
                            }
                            dvBBs.Refresh();
                        }
                        catch (Exception ex)
                        {
                            Comm.WriteErrorLog(ex.StackTrace);
                        }


                        ////总表刷新
                        //dt.Rows.Add(dr);
                        //dvAll.Refresh();

                        try
                        {
                            //得到插入语句
                            if (isThere)
                            {
                                continue;
                            }
                            else
                            {
                                sb.Append(ri.GetInsString(newsInfo) + ";");
                            }

                            //每10次执行一次插入数据库
                            if (sb.ToString().Length != 0)
                            {
                                if (i % 10 == 0)
                                {
                                    filterStr = sb.ToString();
                                    filterStr = filterStr.Replace("[ ", "[");
                                    filterStr = filterStr.Replace(" ]", "]");
                                    //执行插入
                                    cmd.ExecuteNonQuery(filterStr);
                                    //清除插入字段串
                                    sb.Clear();
                                    filterStr = "";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Comm.WriteErrorLog(ex.StackTrace);
                        }
                    }
                }
                #endregion
            }
            #endregion
            try
            {

                if (sb.ToString().Length != 0)
                {
                    filterStr = sb.ToString();
                    filterStr = filterStr.Replace("[ ", "[");
                    filterStr = filterStr.Replace(" ]", "]");
                    //执行插入
                    cmd.ExecuteNonQuery(filterStr);
                    //清除插入字段串
                    sb.Clear();
                    filterStr = "";
                }
            }
            catch (Exception ex)
            {
                Comm.WriteErrorLog(ex.StackTrace);
            }
            //执行完毕后,重新获取一次数据库的数据
            dtWebBBSInfo = tri.SelReleaseInfo(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd HH:mm:ss"), "2");
            this.BeginInvoke(new MethodInvoker(delegate()
            {
                lbBBs.Text = "一轮搜索完毕！";
                lbBBs.ForeColor = Color.Red;
            }));

        }
        #endregion

        #endregion

        #region 控件事件

        #region 表格点击事件，点击标题时打开浏览器
        //表格点击事件，点击标题时打开浏览器
        private void dvAll_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dvAll.SelectedRows.Count != 0 && e.ColumnIndex == 1)
            {
                System.Diagnostics.Process.Start(this.dvAll.Rows[e.RowIndex].Cells[4].Value.ToString());
            }
        }

        private void dvBBs_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dvBBs.SelectedRows.Count != 0 && e.ColumnIndex == 1)
            {
                System.Diagnostics.Process.Start(this.dvBBs.Rows[e.RowIndex].Cells[4].Value.ToString());
            }
        }

        private void dvBlog_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dvBlog.SelectedRows.Count != 0 && e.ColumnIndex == 1)
            {
                System.Diagnostics.Process.Start(this.dvBlog.Rows[e.RowIndex].Cells[4].Value.ToString());
            }
        }

        private void dvWBlog_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dvWBlog.SelectedRows.Count != 0 && e.ColumnIndex == 1)
            {
                System.Diagnostics.Process.Start(this.dvWBlog.Rows[e.RowIndex].Cells[4].Value.ToString());
            }
        }

        private void dvWeb_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dvWeb.SelectedRows.Count != 0 && e.ColumnIndex == 1)
            {
                System.Diagnostics.Process.Start(this.dvWeb.Rows[e.RowIndex].Cells[4].Value.ToString());
            }
        }
        private void dvTieBa_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dvtieba.SelectedRows.Count != 0 && e.ColumnIndex == 1)
            {
                System.Diagnostics.Process.Start(this.dvtieba.Rows[e.RowIndex].Cells[4].Value.ToString());
            }
        }
        private void dvWeiXin_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dvWeiXin.SelectedRows.Count != 0 && e.ColumnIndex == 1)
            {
                System.Diagnostics.Process.Start(this.dvWeiXin.Rows[e.RowIndex].Cells[4].Value.ToString());
            }
        }
        #endregion

        #region 表格选中事件，获取选中行的标题，关键字，链接，内容，拼合到下面的dataView里
        //表格选中事件，获取选中行的标题，关键字，链接，内容，拼合到下面的dataView里
        private void dvAll_SelectionChanged(object sender, EventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate()
            {
                dataView.Clear();
                if (dvAll.SelectedRows.Count == 0) return;
                if (dvAll.CurrentCell != null)
                {
                    string title = dvAll.CurrentCell.OwningRow.Cells[1].Value.ToString();

                    //设置textbox内容为标题加链接
                    dataView.Text = "标题：" + title + "\n链接：" + dvAll.CurrentCell.OwningRow.Cells[4].Value.ToString() + "\n";

                    //设置标题粗体
                    dataView.Select(3, title.Length);
                    //dataView.SelectionFont = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold);

                    //获取当前textbox内容长度，+4是表示内容文本空两个空格
                    int length = dataView.Text.Length + 4;
					//得到关键字列表
					DataTable dtkey;
					DataTable dtParts;
					dtkey = cmd.GetTabel("select * from Keywords");
					dtParts = cmd.GetTabel("SELECT * FROM partword");
                    //添加内容
                    dataView.AppendText("    " + dvAll.CurrentCell.OwningRow.Cells[2].Value.ToString());
                    StringBuilder keySb = new StringBuilder();
                    for (int i = 0; i < dtkey.Rows.Count; i++)
                    {
                        keySb.Append(","+dtkey.Rows[i]["KeyWord"].ToString());
                    }
                    if (keySb.Length > 0)
                    {
                        keySb = keySb.Remove(0, 1);
                        //分割关键字
                        string[] keywords = keySb.ToString().Trim().Split(',');
                        foreach (string kw in keywords)
                        {
                            int kl = length;//设定文本开始位置
                            int wl = kw.Length;

                            //以关键词分割内容为数组
                            string[] str = Regex.Split(dvAll.CurrentCell.OwningRow.Cells[2].Value.ToString(), kw, RegexOptions.IgnoreCase);
                            for (int i = 0, l = str.Length - 1; i < l; i++)
                            {
                                //获取内容分段的长度
                                kl += str[i].Length;
                                dataView.Select(kl, wl);
                                dataView.SelectionFont = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold);
                                dataView.SelectionColor = Color.Red;
                                kl += wl;
                            }
                        }
                    }
                    dataView.Select(0, 0);
                }
            }));
        }
        private void dvBBs_SelectionChanged(object sender, EventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate() {
                dataView.Clear();
                if (dvBBs.SelectedRows.Count == 0) return;
                if (dvBBs.CurrentCell != null)
                {
                    string title = dvBBs.CurrentCell.OwningRow.Cells[1].Value.ToString();

                    //设置textbox内容为标题加链接
                    dataView.Text = "标题：" + title + "\n链接：" + dvBBs.CurrentCell.OwningRow.Cells[4].Value.ToString() + "\n";

                    //设置标题粗体
                    dataView.Select(3, title.Length);
                    dataView.SelectionFont = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold);

                    //获取当前textbox内容长度，+4是表示内容文本空两个空格
                    int length = dataView.Text.Length + 4;
					//得到关键字列表
					DataTable dtkey;
					DataTable dtParts;
					dtkey = cmd.GetTabel("select * from Keywords");
					dtParts = cmd.GetTabel("SELECT * FROM partword");
                    //添加内容
                    dataView.AppendText("    " + dvBBs.CurrentCell.OwningRow.Cells[2].Value.ToString());
                    StringBuilder keySb = new StringBuilder();
                    for (int i = 0; i < dtkey.Rows.Count; i++)
                    {
                        keySb.Append("," + dtkey.Rows[i]["KeyWord"].ToString());
                    }
                    if (keySb.Length > 0)
                    {
                        keySb = keySb.Remove(0, 1);
                        //分割关键字
                        string[] keywords = keySb.ToString().Trim().Split(',');
                        foreach (string kw in keywords)
                        {
                            int kl = length;//设定文本开始位置
                            int wl = kw.Length;

                            //以关键词分割内容为数组
                            string[] str = Regex.Split(dvBBs.CurrentCell.OwningRow.Cells[2].Value.ToString(), kw, RegexOptions.IgnoreCase);
                            for (int i = 0, l = str.Length - 1; i < l; i++)
                            {
                                //获取内容分段的长度
                                kl += str[i].Length;
                                dataView.Select(kl, wl);
                                dataView.SelectionFont = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold);
                                dataView.SelectionColor = Color.Red;
                                kl += wl;
                            }
                        }
                    }
                    dataView.Select(0, 0);
                }
            }));
            
        }
        private void dvWeb_SelectionChanged(object sender, EventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate()
            {
                dataView.Clear();
                if (dvWeb.SelectedRows.Count == 0) return;
                if (dvWeb.CurrentCell != null)
                {
                    string title = dvWeb.CurrentCell.OwningRow.Cells[1].Value.ToString();

                    //设置textbox内容为标题加链接
                    dataView.Text = "标题：" + title + "\n链接：" + dvWeb.CurrentCell.OwningRow.Cells[4].Value.ToString() + "\n";

                    //设置标题粗体
                    dataView.Select(3, title.Length);
                    dataView.SelectionFont = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold);

                    //获取当前textbox内容长度，+4是表示内容文本空两个空格
                    int length = dataView.Text.Length + 4;
					//得到关键字列表
					DataTable dtkey;
					DataTable dtParts;
					dtkey = cmd.GetTabel("select * from Keywords");
					dtParts = cmd.GetTabel("SELECT * FROM partword");
                    //添加内容
                    dataView.AppendText("    " + dvWeb.CurrentCell.OwningRow.Cells[2].Value.ToString());
                    StringBuilder keySb = new StringBuilder();
                    for (int i = 0; i < dtkey.Rows.Count; i++)
                    {
                        keySb.Append("," + dtkey.Rows[i]["KeyWord"].ToString());
                    }
                    if (keySb.Length > 0)
                    {
                        keySb = keySb.Remove(0, 1);
                        //分割关键字
                        string[] keywords = keySb.ToString().Trim().Split(',');
                        foreach (string kw in keywords)
                        {
                            int kl = length;//设定文本开始位置
                            int wl = kw.Length;

                            //以关键词分割内容为数组
                            string[] str = Regex.Split(dvWeb.CurrentCell.OwningRow.Cells[2].Value.ToString(), kw, RegexOptions.IgnoreCase);
                            for (int i = 0, l = str.Length - 1; i < l; i++)
                            {
                                //获取内容分段的长度
                                kl += str[i].Length;
                                dataView.Select(kl, wl);
                                dataView.SelectionFont = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold);
                                dataView.SelectionColor = Color.Red;
                                kl += wl;
                            }
                        }
                    }
                    dataView.Select(0, 0);
                }
            }));
        }
        private void dvTieBa_SelectionChanged(object sender, EventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate()
            {
                dataView.Clear();
                if (dvtieba.SelectedRows.Count == 0) return;
                if (dvtieba.CurrentCell != null)
                {
                    string title = dvtieba.CurrentCell.OwningRow.Cells[1].Value.ToString();

                    //设置textbox内容为标题加链接
                    dataView.Text = "标题：" + title + "\n链接：" + dvtieba.CurrentCell.OwningRow.Cells[4].Value.ToString() + "\n";

                    //设置标题粗体
                    dataView.Select(3, title.Length);
                    dataView.SelectionFont = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold);

                    //获取当前textbox内容长度，+4是表示内容文本空两个空格
                    int length = dataView.Text.Length + 4;
					//得到关键字列表
					DataTable dtkey;
					DataTable dtParts;
					dtkey = cmd.GetTabel("select * from Keywords");
					dtParts = cmd.GetTabel("SELECT * FROM partword");
                    //添加内容
                    dataView.AppendText("    " + dvtieba.CurrentCell.OwningRow.Cells[2].Value.ToString());
                    StringBuilder keySb = new StringBuilder();
                    for (int i = 0; i < dtkey.Rows.Count; i++)
                    {
                        keySb.Append("," + dtkey.Rows[i]["KeyWord"].ToString());
                    }
                    if (keySb.Length > 0)
                    {
                        keySb = keySb.Remove(0, 1);
                        //分割关键字
                        string[] keywords = keySb.ToString().Trim().Split(',');
                        foreach (string kw in keywords)
                        {
                            int kl = length;//设定文本开始位置
                            int wl = kw.Length;

                            //以关键词分割内容为数组
                            string[] str = Regex.Split(dvtieba.CurrentCell.OwningRow.Cells[2].Value.ToString(), kw, RegexOptions.IgnoreCase);
                            for (int i = 0, l = str.Length - 1; i < l; i++)
                            {
                                //获取内容分段的长度
                                kl += str[i].Length;
                                dataView.Select(kl, wl);
                                dataView.SelectionFont = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold);
                                dataView.SelectionColor = Color.Red;
                                kl += wl;
                            }
                        }
                    }
                    dataView.Select(0, 0);
                }
            }));
        }
        private void dvBlog_SelectionChanged(object sender, EventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate()
            {
                dataView.Clear();
                if (dvBlog.SelectedRows.Count == 0) return;
                if (dvBlog.CurrentCell != null)
                {
                    string title = dvBlog.CurrentCell.OwningRow.Cells[1].Value.ToString();

                    //设置textbox内容为标题加链接
                    dataView.Text = "标题：" + title + "\n链接：" + dvBlog.CurrentCell.OwningRow.Cells[4].Value.ToString() + "\n";

                    //设置标题粗体
                    dataView.Select(3, title.Length);
                    dataView.SelectionFont = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold);

                    //获取当前textbox内容长度，+4是表示内容文本空两个空格
                    int length = dataView.Text.Length + 4;
					//得到关键字列表
					DataTable dtkey;
					DataTable dtParts;
					dtkey = cmd.GetTabel("select * from Keywords");
					dtParts = cmd.GetTabel("SELECT * FROM partword");
                    //添加内容
                    dataView.AppendText("    " + dvBlog.CurrentCell.OwningRow.Cells[2].Value.ToString());
                    StringBuilder keySb = new StringBuilder();
                    for (int i = 0; i < dtkey.Rows.Count; i++)
                    {
                        keySb.Append("," + dtkey.Rows[i]["KeyWord"].ToString());
                    }
                    if (keySb.Length > 0)
                    {
                        keySb = keySb.Remove(0, 1);
                        //分割关键字
                        string[] keywords = keySb.ToString().Trim().Split(',');
                        foreach (string kw in keywords)
                        {
                            int kl = length;//设定文本开始位置
                            int wl = kw.Length;

                            //以关键词分割内容为数组
                            string[] str = Regex.Split(dvBlog.CurrentCell.OwningRow.Cells[2].Value.ToString(), kw, RegexOptions.IgnoreCase);
                            for (int i = 0, l = str.Length - 1; i < l; i++)
                            {
                                //获取内容分段的长度
                                kl += str[i].Length;
                                dataView.Select(kl, wl);
                                dataView.SelectionFont = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold);
                                dataView.SelectionColor = Color.Red;
                                kl += wl;
                            }
                        }
                    }
                    dataView.Select(0, 0);
                }
            }));
        }
        private void dvWBlog_SelectionChanged(object sender, EventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate()
            {
                dataView.Clear();
                if (dvWBlog.SelectedRows.Count == 0) return;
                if (dvWBlog.CurrentCell != null)
                {
                    string title = dvWBlog.CurrentCell.OwningRow.Cells[1].Value == null ? "" : dvWBlog.CurrentCell.OwningRow.Cells[1].Value.ToString();

                    //设置textbox内容为标题加链接
                    dataView.Text = "标题：" + title + "\n链接：" + dvWBlog.CurrentCell.OwningRow.Cells[4].Value == null ? "" : dvWBlog.CurrentCell.OwningRow.Cells[4].Value.ToString() + "\n";

                    //设置标题粗体
                    dataView.Select(3, title.Length);
                    dataView.SelectionFont = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold);

                    //获取当前textbox内容长度，+4是表示内容文本空两个空格
                    int length = dataView.Text.Length + 4;
					//得到关键字列表
					DataTable dtkey;
					DataTable dtParts;
					dtkey = cmd.GetTabel("select * from Keywords");
					dtParts = cmd.GetTabel("SELECT * FROM partword");
                    //添加内容
                    dataView.AppendText("    " + (dvWBlog.CurrentCell.OwningRow.Cells[2].Value == null ? "" : dvWBlog.CurrentCell.OwningRow.Cells[2].Value.ToString()));
                    StringBuilder keySb = new StringBuilder();
                    for (int i = 0; i < dtkey.Rows.Count; i++)
                    {
                        keySb.Append("," + dtkey.Rows[i]["KeyWord"].ToString());
                    }
                    if (keySb.Length > 0)
                    {
                        keySb = keySb.Remove(0, 1);
                        //分割关键字
                        string[] keywords = keySb.ToString().Trim().Split(',');
                        foreach (string kw in keywords)
                        {
                            int kl = length;//设定文本开始位置
                            int wl = kw.Length;

                            //以关键词分割内容为数组
                            string[] str = Regex.Split(dvWBlog.CurrentCell.OwningRow.Cells[2].Value == null ? "" : dvWBlog.CurrentCell.OwningRow.Cells[2].Value.ToString(), kw, RegexOptions.IgnoreCase);
                            for (int i = 0, l = str.Length - 1; i < l; i++)
                            {
                                //获取内容分段的长度
                                kl += str[i].Length;
                                dataView.Select(kl, wl);
                                dataView.SelectionFont = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold);
                                dataView.SelectionColor = Color.Red;
                                kl += wl;
                            }
                        }
                    }
                    dataView.Select(0, 0);
                }
            }));
        }
        private void dvWeiXin_SelectionChanged(object sender, EventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate()
            {
                dataView.Clear();
                if (dvWeiXin.SelectedRows.Count == 0) return;
                if (dvWeiXin.CurrentCell != null)
                {
                    if (dvWeiXin.CurrentCell.OwningRow.Cells[1].Value != null)
                    {
                        string title = dvWeiXin.CurrentCell.OwningRow.Cells[1].Value.ToString();

                        //设置textbox内容为标题加链接
                        dataView.Text = "标题：" + title + "\n链接：" + dvWeiXin.CurrentCell.OwningRow.Cells[4].Value.ToString() + "\n";

                        //设置标题粗体
                        dataView.Select(3, title.Length);
                        dataView.SelectionFont = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold);

                        //获取当前textbox内容长度，+4是表示内容文本空两个空格
                        int length = dataView.Text.Length + 4;
						//得到关键字列表
						DataTable dtkey;
						DataTable dtParts;
						dtkey = cmd.GetTabel("select * from Keywords");
						dtParts = cmd.GetTabel("SELECT * FROM partword");
                        //添加内容
                        dataView.AppendText("    " + dvWeiXin.CurrentCell.OwningRow.Cells[2].Value.ToString());
                        StringBuilder keySb = new StringBuilder();
                        for (int i = 0; i < dtkey.Rows.Count; i++)
                        {
                            keySb.Append("," + dtkey.Rows[i]["KeyWord"].ToString());
                        }
                        if (keySb.Length > 0)
                        {
                            keySb = keySb.Remove(0, 1);
                            //分割关键字
                            string[] keywords = keySb.ToString().Trim().Split(',');
                            foreach (string kw in keywords)
                            {
                                int kl = length;//设定文本开始位置
                                int wl = kw.Length;

                                //以关键词分割内容为数组
                                string[] str = Regex.Split(dvWeiXin.CurrentCell.OwningRow.Cells[2].Value.ToString(), kw, RegexOptions.IgnoreCase);
                                for (int i = 0, l = str.Length - 1; i < l; i++)
                                {
                                    //获取内容分段的长度
                                    kl += str[i].Length;
                                    dataView.Select(kl, wl);
                                    dataView.SelectionFont = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold);
                                    dataView.SelectionColor = Color.Red;
                                    kl += wl;
                                }
                            }
                        }
                        dataView.Select(0, 0);
                    }
                }
            }));
        }
        #endregion

        #region 表格内容格式化，正负研判调用图片
        //表格内容格式化，正负研判调用图片
        private void dvAll_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex != dvAll.NewRowIndex)
            {
                if (e.ColumnIndex == 11)
                {
                    string part = dvAll.Rows[e.RowIndex].Cells[11].Value.ToString();
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
                    switch (dvAll.Rows[e.RowIndex].Cells[8].Value.ToString())
                    {
                        case "4":
                            e.Value = "主流媒体";
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
                        case "5":
                            e.Value = "贴吧";
                            break;
                        case "6":
                            e.Value = "微信";
                            break;
                    }
                }
            }
        }

        private void dvBlog_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex != dvBlog.NewRowIndex)
            {
                if (e.ColumnIndex == 11)
                {
                    string part = dvBlog.Rows[e.RowIndex].Cells[11].Value.ToString();
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
                    switch (dvBlog.Rows[e.RowIndex].Cells[8].Value.ToString())
                    {
                        case "4":
                            e.Value = "主流媒体";
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
                        case "5":
                            e.Value = "贴吧";
                            break;
                        case "6":
                            e.Value = "微信";
                            break;
                    }
                }
            }
        }

        private void dvWBlog_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.RowIndex != dvWBlog.NewRowIndex)
                {
                    if (e.ColumnIndex == 11)
                    {
                        string part = dvWBlog.Rows[e.RowIndex].Cells[11].Value.ToString();
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
                        switch (dvWBlog.Rows[e.RowIndex].Cells[8].Value.ToString())
                        {
                            case "4":
                                e.Value = "主流媒体";
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
                            case "5":
                                e.Value = "贴吧";
                                break;
                            case "6":
                                e.Value = "微信";
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private void dvBBs_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex != dvBBs.NewRowIndex)
            {
                if (e.ColumnIndex == 11)
                {
                    string part = dvBBs.Rows[e.RowIndex].Cells[11].Value.ToString();
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
                    switch (dvBBs.Rows[e.RowIndex].Cells[8].Value.ToString())
                    {
                        case "4":
                            e.Value = "主流媒体";
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
                        case "5":
                            e.Value = "贴吧";
                            break;
                        case "6":
                            e.Value = "微信";
                            break;
                    }
                }
            }
        }

        private void dvWeb_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex != dvWeb.NewRowIndex)
            {
                if (e.ColumnIndex == 11)
                {
                    string part = dvWeb.Rows[e.RowIndex].Cells[11].Value.ToString();
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
                    switch (dvWeb.Rows[e.RowIndex].Cells[8].Value.ToString())
                    {
                        case "4":
                            e.Value = "主流媒体";
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
                        case "5":
                            e.Value = "贴吧";
                            break;
                        case "6":
                            e.Value = "微信";
                            break;
                    }
                }
            }
        }
        private void dvTieBa_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex != dvtieba.NewRowIndex)
            {
                if (e.ColumnIndex == 11)
                {
                    string part = dvtieba.Rows[e.RowIndex].Cells[11].Value.ToString();
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
                    switch (dvtieba.Rows[e.RowIndex].Cells[8].Value.ToString())
                    {
                        case "4":
                            e.Value = "主流媒体";
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
                        case "5":
                            e.Value = "贴吧";
                            break;
                        case "6":
                            e.Value = "微信";
                            break;
                    }
                }
            }
        }
        private void dvWeiXin_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex != dvWeiXin.NewRowIndex)
            {
                if (e.ColumnIndex == 11)
                {
                    string part = dvWeiXin.Rows[e.RowIndex].Cells[11].Value.ToString();
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
                    switch (dvWeiXin.Rows[e.RowIndex].Cells[8].Value.ToString())
                    {
                        case "4":
                            e.Value = "主流媒体";
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
                        case "5":
                            e.Value = "贴吧";
                            break;
                        case "6":
                            e.Value = "微信";
                            break;
                    }
                }
            }
        }
        #endregion

        //切换页签时刷新下方的内容
        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            try
            {
                dataView.Clear();
                switch (e.TabPage.Text)
                {
                    case "搜索引擎":
                        dvAll_SelectionChanged(null, null);
                        break;
                    case "博客":
                        dvBlog_SelectionChanged(null, null);
                        break;
                    case "论坛":
                        dvBBs_SelectionChanged(null, null);
                        break;
                    case "微博":
                        dvWBlog_SelectionChanged(null, null);
                        break;
                    case "主流媒体":
                        dvWeb_SelectionChanged(null, null);
                        break;
                    case "贴吧":
                        dvTieBa_SelectionChanged(null, null);
                        break;
                    case "微信":
                        dvWeiXin_SelectionChanged(null, null);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #region 证据留存
        private void stmKZ_Click(object sender, EventArgs e)
        {
            if (dvAll.SelectedRows.Count == 0)
                return;
            string temp = "";

            try
            {
                Entities.SystemSet ss = (Entities.SystemSet)GlobalPars.GloPars["systemset"];
                temp = ss.EvidenceImgSavePath + "\\" + DateTime.Now.ToString().Replace("/", "-").Replace(" ", "-").Replace(":", "-") + ".jpg";

                Bitmap image = util.WebSnap.StartSnap(this.dvAll.SelectedRows[0].Cells[4].Value.ToString());
                image.Save(temp);
                MessageBox.Show("证据图片生成成功！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("目标网站原因,证据图片生成失败!");
            }
        }

        private void ssmBlog_Click(object sender, EventArgs e)
        {
            if (dvBlog.SelectedRows.Count == 0)
                return;
            string temp = "";

            try
            {
                Entities.SystemSet ss = (Entities.SystemSet)GlobalPars.GloPars["systemset"];
                temp = ss.EvidenceImgSavePath + "\\" + DateTime.Now.ToString().Replace("/","-").Replace(" ","-").Replace(":", "-") + ".jpg";

                Bitmap image = util.WebSnap.StartSnap(this.dvBlog.SelectedRows[0].Cells[4].Value.ToString());
                image.Save(temp);
                MessageBox.Show("证据图片生成成功！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("目标网站原因,证据图片生成失败!");
            }

        }

        private void ssmBBS_Click(object sender, EventArgs e)
        {
            if (dvBBs.SelectedRows.Count == 0)
                return;
            string temp = "";

            try
            {
                Entities.SystemSet ss = (Entities.SystemSet)GlobalPars.GloPars["systemset"];
                temp = ss.EvidenceImgSavePath + "\\" + DateTime.Now.ToString().Replace("/", "-").Replace(" ", "-").Replace(":", "-") + ".jpg";

                Bitmap image = util.WebSnap.StartSnap(this.dvBlog.SelectedRows[0].Cells[4].Value.ToString());
                image.Save(temp);
                MessageBox.Show("证据图片生成成功！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("目标网站原因,证据图片生成失败!");
            }
        }

        private void ssmWblog_Click(object sender, EventArgs e)
        {
            if (dvWBlog.SelectedRows.Count == 0)
                return;
            string temp = "";

            try
            {
                Entities.SystemSet ss = (Entities.SystemSet)GlobalPars.GloPars["systemset"];
                temp = ss.EvidenceImgSavePath + "\\" + DateTime.Now.ToString().Replace("/", "-").Replace(" ", "-").Replace(":", "-") + ".jpg";

                Bitmap image = util.WebSnap.StartSnap(this.dvWBlog.SelectedRows[0].Cells[4].Value.ToString());
                image.Save(temp);
                MessageBox.Show("证据图片生成成功！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("目标网站原因,证据图片生成失败!");
            }
        }

        private void ssmWeb_Click(object sender, EventArgs e)
        {
            if (dvWeb.SelectedRows.Count == 0)
                return;
            string temp = "";

            try
            {
                Entities.SystemSet ss = (Entities.SystemSet)GlobalPars.GloPars["systemset"];
                temp = ss.EvidenceImgSavePath + "\\" + DateTime.Now.ToString().Replace("/", "-").Replace(" ", "-").Replace(":", "-") + ".jpg";

                Bitmap image = util.WebSnap.StartSnap(this.dvWeb.SelectedRows[0].Cells[4].Value.ToString());
                image.Save(temp);
                MessageBox.Show("证据图片生成成功！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("目标网站原因,证据图片生成失败!");
            }
        }
        private void ssmTieba_Click(object sender, EventArgs e)
        {
            if (dvtieba.SelectedRows.Count == 0)
                return;
            string temp = "";

            try
            {
                Entities.SystemSet ss = (Entities.SystemSet)GlobalPars.GloPars["systemset"];
                temp = ss.EvidenceImgSavePath + "\\" + DateTime.Now.ToString().Replace("/", "-").Replace(" ", "-").Replace(":", "-") + ".jpg";

                Bitmap image = util.WebSnap.StartSnap(this.dvtieba.SelectedRows[0].Cells[4].Value.ToString());
                image.Save(temp);
                MessageBox.Show("证据图片生成成功！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("目标网站原因,证据图片生成失败!");
            }
        }
        private void ssmWeiXin_Click(object sender, EventArgs e)
        {
            if (dvWeiXin.SelectedRows.Count == 0)
                return;
            string temp = "";

            try
            {
                Entities.SystemSet ss = (Entities.SystemSet)GlobalPars.GloPars["systemset"];
                temp = ss.EvidenceImgSavePath + "\\" + DateTime.Now.ToString().Replace("/", "-").Replace(" ", "-").Replace(":", "-") + ".jpg";

                Bitmap image = util.WebSnap.StartSnap(this.dvWeiXin.SelectedRows[0].Cells[4].Value.ToString());
                image.Save(temp);
                MessageBox.Show("证据图片生成成功！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("目标网站原因,证据图片生成失败!");
            }
        }
        #endregion

        bool play = true;
        private void pictureBox6_Click(object sender, EventArgs e)
        {
            if (play)
            {
                play = false;
                pictureBox6.BackgroundImage = imageList1.Images[2];
                //butClike = !butClike;

                this.pictureBox1.Visible = true;
                if (Program.isBeyondDate == true)
                { Program.ProClose = true; }
                else
                { Program.ProClose = false; }

                BeginEvn();

                webSpiderTimer.Enabled = true;
                weiboSpiderTimer.Enabled = true;

				//reFreshWeibo();

				qwei = new util.QQWeibo(2 * 60 * 1000);
				qwei.Start();

				swei = new util.SinaWeibo(2 * 60 * 1000);
				swei.Start();
            }
            else
            {
                play = true;
                pictureBox6.BackgroundImage = imageList1.Images[0];
                //butClike = !butClike;
				qwei.Stop();
				swei.Stop();
                webSpiderTimer.Enabled = false;
                weiboSpiderTimer.Enabled = false;
                this.pictureBox1.Visible = false;
                Program.ProClose = true;
            }
        }
        #endregion

        /// <summary>
        /// 正文的正负判断
        /// </summary>
        /// <param name="str1">正文</param>
        /// <returns></returns>
        private int GetParts(string str1)
        {
			DataTable dtParts;
			dtParts = cmd.GetTabel("SELECT * FROM partword");
            int z = 0;
            int f = 0;
            for (int i = 0; i < dtParts.Rows.Count; i++)
            {
                if (dtParts.Rows[i][2].ToString() == "0")
                {
                    f += util.Comm.partCount(str1, dtParts.Rows[i][1].ToString());
                }
                else
                {
                    z += util.Comm.partCount(str1, dtParts.Rows[i][1].ToString());
                }
            }

            if (z > f)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        private static string[] GetLIstDate(IEnumerable<string> str)
        {
            List<string> temp = new List<string>();

            foreach (var s in str)
            {
                temp.Add(s);
            }
            return temp.ToArray();
        }

        /// <summary>
        /// 判断列表和数据表中是否有相同链接或标题的数据
        /// </summary>
        /// <param name="sUrl">网址/标题</param>
        /// <param name="obj">数据列表</param>
        /// <param name="dt">库中数据表</param>
        /// <param name="isTitle">true:传递的是网址,false:传递的是标题</param>
        /// <returns></returns>
        private int UrlThereare(string sUrl, DataTable obj, DataTable dt, bool isTitle)
        {
            try
            {
                if (isTitle)
                {
                    dv = new DataView(obj);
                    string aa = string.Format("InfoSource='{0}'", sUrl);
                    dv.RowFilter = aa;
                    if (dv.Count > 0)
                    {
                        return 1;
                    }
                    //先判断列表
                    //for (int i = obj.Rows.Count; i > 0; i--)
                    //{
                    //    if (obj.Rows[i - 1].Cells[4].Value.ToString() == sUrl)
                    //    {
                    //        return 1;
                    //    }
                    //}

                    dv = new DataView(dt);
                    aa = string.Format("InfoSource='{0}'", sUrl);
                    dv.RowFilter = aa;
                    if (dv.Count > 0)
                    {
                        return 1;
                    }
                    ////如果列表中不存在,就去库中判断
                    //for (int i = dt.Rows.Count; i > 0; i--)
                    //{
                    //    if (dt.Rows[i - 1]["InfoSource"].ToString() == sUrl)
                    //    {
                    //        return 1;
                    //    }
                    //}

                    //如果没有一样的就返回0
                    return 0;
                }
                else
                {
                    //先判断列表
                    dv = new DataView(obj);
                    string aa = string.Format("title='{0}'", sUrl);
                    dv.RowFilter = aa;
                    if (dv.Count > 0)
                    {
                        return 1;
                    }
                    ////先判断列表
                    //for (int i = obj.Rows.Count; i > 0; i--)
                    //{
                    //    if (obj.Rows[i - 1].Cells[1].Value.ToString() == sUrl)
                    //    {
                    //        return 1;
                    //    }
                    //}
                    //如果列表中不存在,就去库中判断
                    dv = new DataView(dt);
                    aa = string.Format("title='{0}'", sUrl);
                    dv.RowFilter = aa;
                    if (dv.Count > 0)
                    {
                        return 1;
                    }
                    //for (int i = dt.Rows.Count; i > 0; i--)
                    //{
                    //    if (dt.Rows[i - 1]["title"].ToString() == sUrl)
                    //    {
                    //        return 1;
                    //    }
                    //}

                    //如果没有一样的就返回0
                    return 0;
                }
            }
            catch (Exception ex)
            {
                StreamWriter sw = File.AppendText("log.txt");
                sw.WriteLine(DateTime.Now.ToLongDateString());
                sw.WriteLine("begin");
                sw.WriteLine(ex.Message);
                sw.WriteLine("end");
                sw.WriteLine("");

                sw.Close();

                return 1;
            }
        }

        private void FormatDataView(DataGridView obj, bool isWeibo)
        {
            obj.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "uid", DataPropertyName = "uid" });
            obj.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "事件", DataPropertyName = "keywords" });
            obj.Columns.Add(new DataGridViewLinkColumn() { HeaderText = "标题", DataPropertyName = "title" });
            obj.Columns.Add(new DataGridViewLinkColumn() { HeaderText = "链接", DataPropertyName = "infosource" });
            obj.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "内容", DataPropertyName = "contexts" });
            obj.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "发布者", DataPropertyName = "releaseName" });
            obj.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "转发量", DataPropertyName = "reposts" });
            obj.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "评论数", DataPropertyName = "comments" });
            obj.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "类别", DataPropertyName = "pid" });
            obj.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "来源", DataPropertyName = "webName" });
            obj.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "抓取时间", DataPropertyName = "collectdate" });
            obj.Columns.Add(new DataGridViewImageColumn() { HeaderText = "评价", DataPropertyName = "part" });
            obj.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "发布时间", DataPropertyName = "releaseDate" });
            obj.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "快照", DataPropertyName = "snapshot" });
            //obj.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "关键词分类", DataPropertyName = "kid" });
            //obj.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "省", DataPropertyName = "sheng" });
            //obj.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "市", DataPropertyName = "shi" });
            //obj.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "县", DataPropertyName = "xian" });

            obj.Columns[0].Visible = false;
            obj.Columns[3].Visible = false;
            obj.Columns[5].Visible = false;
            obj.Columns[6].Visible = false;
            obj.Columns[7].Visible = false;

            obj.Columns[8].Visible = false;
            obj.Columns[12].Visible = false;
            obj.Columns[13].Visible = false;
            //obj.Columns[14].Visible = false;
            //obj.Columns[15].Visible = false;
            //obj.Columns[16].Visible = false;
            //obj.Columns[17].Visible = false;

            obj.Columns[4].Width = 480;
            obj.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            obj.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            obj.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            obj.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            obj.Columns[6].Width = 80;
            obj.Columns[7].Width = 80;
            obj.Columns[8].Width = 60;
            obj.Columns[10].Width = 160;
            obj.Columns[11].Width = 60;
            obj.Columns[12].Width = 160;
            obj.Columns[13].Width = 60;
            //obj.Columns[14].Width = 60;
            //obj.Columns[15].Width = 60;
            //obj.Columns[16].Width = 60;
            //obj.Columns[17].Width = 60;
            if (isWeibo == true)
            {
                obj.Columns[3].Visible = false;
                obj.Columns[12].Visible = true;
            }
        }

    }
}
