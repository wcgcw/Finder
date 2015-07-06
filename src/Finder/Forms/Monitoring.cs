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
    public partial class Monitoring : Form
    {
        //Encoding encoding;
        private DataTable dtnewsinfo;
        private DataTable dtbloginfo;
        private DataTable dtbbsinfo;
        private DataTable dtqueryinfo;

        HtmlAgilityPack.HtmlDocument docPage = new HtmlAgilityPack.HtmlDocument();
        //WebBrowser wbList = new WebBrowser();
        //WebBrowser wbContent = new WebBrowser();
        private StringBuilder sb;
        private DataTable dtWebNewsInfo;
        private DataTable dtWebBlogInfo;
        private DataTable dtWebBBSInfo;
        private DataTable dtWebQueryInfo;
        private DataTable dtParts;
        TbReleaseInfo tri;

        public Monitoring()
        {
            InitializeComponent();
            Monitoring.CheckForIllegalCrossThreadCalls = false;
        }

        System.Threading.AutoResetEvent obj = new System.Threading.AutoResetEvent(false);

        private void Report()
        {
            getData();
            getKwData();
        }

        private void Monitoring_Load(object sender, EventArgs e)
        {
            Thread re = new Thread(new ThreadStart(Report));
            re.IsBackground = true;
            re.Start();

            tri = new TbReleaseInfo();

            FormatDataView(dvAll, false);
            FormatDataView(dvBBs, false);
            FormatDataView(dvBlog, false);
            FormatDataView(dvWBlog, true);
            FormatDataView(dvWeb, false);

            dtnewsinfo = tri.GetReleaseInfoFormat();
            dtbloginfo = tri.GetReleaseInfoFormat();
            dtbbsinfo = tri.GetReleaseInfoFormat();
            dtqueryinfo = tri.GetReleaseInfoFormat();

            dtWebNewsInfo = tri.SelReleaseInfo(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd HH:mm:ss"), "0 AND webName<>'百度'");
            dvWeb.DataSource = dtnewsinfo;
            dtWebBlogInfo = tri.SelReleaseInfo(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd HH:mm:ss"), "1");
            dvBlog.DataSource = dtbloginfo;
            dtWebBBSInfo = tri.SelReleaseInfo(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd HH:mm:ss"), "2");
            dvBBs.DataSource = dtbbsinfo;
            dtWebQueryInfo = tri.SelReleaseInfo(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd HH:mm:ss"), "0 AND webName='百度'");
            dvAll.DataSource = dtqueryinfo;
        }


        /// <summary>
        /// 返回目标网页源码
        /// </summary>
        /// <param name="WebUrl">网络地址</param>
        /// <param name="Encodes">编码</param>
        /// <returns></returns>
        private string GetWebCode(string WebUrl, string Encodes)
        {
            try
            {
                string WebCode = "";
                WebRequest request = WebRequest.Create(WebUrl);
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(Encodes));

                WebCode = reader.ReadToEnd();

                reader.Close();
                reader.Dispose();
                response.Close();

                return WebCode;
            }
            catch (Exception ex)
            {
                throw new Exception("获取失败!原因:" + ex.Message);
            }
        }

        /// <summary>
        /// 正文的正负判断
        /// </summary>
        /// <param name="str1">正文</param>
        /// <returns></returns>
        private int GetParts(string str1)
        {
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

        private void getWebImage(string strHtml, string Encodes, string imagePath)
        {
            SDWebCache wc = new SDWebCache(GetWebCode(strHtml, Encodes));
            Bitmap image = wc.Snap();
            image.Save(imagePath);
        }

        private string WebtopUrl = "";
        Finder.util.QQWeibo qwei;
        Finder.util.SinaWeibo swei;

        private bool butClike = false;

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

                Thread Webexplorer = new Thread(new ThreadStart(GetBaiduInfo));
                Webexplorer.IsBackground = true;
                Webexplorer.Start();
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

        #region 百度搜索

        private void GetBaiduInfo()
        {
            lbAll.Text = "";
            lbAll.Visible = true;

            SQLitecommand cmd = new SQLitecommand();
            ;
            //得到关键字列表
            DataTable dtkey = new DataTable();
            dtkey = cmd.GetTabel("select * from Keywords");

            dtParts = cmd.GetTabel("SELECT * FROM partword");

            //链接的正则
            string aa = "http://.[^\"]+";
            string[] sDate;

            sb = new StringBuilder();
            sb.Append("");

            //TbReleaseInfo ri = new TbReleaseInfo();

            //按关键字循环
            for (int kw = 0; kw < dtkey.Rows.Count; kw++)
            {
                lbAll.Text = "正在搜索关键字为<" + dtkey.Rows[kw]["KeyWord"].ToString().Trim() + ">的数据.";
                lbAll.ForeColor = Color.DarkBlue;
                //取得关键字
                string keys = dtkey.Rows[kw]["KeyWord"].ToString().Trim();
                //组成查询字串
                string url = "http://www.baidu.com/s?wd=\"" + keys + "\"&rn=50";

                //得到结果放在数组内
                List<string> lis = new List<string>();
                lis = HtmlUtil.GetElementsByClassList(getHtml(url, "utf-8"), "result");

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
                    sDate = HtmlUtil.GetElementsByTagAndClass(lis[i], "span", "g");
                    if (sDate.Length <= 0) continue;

                    mri.ReleaseDate = HtmlUtil.NoHTML(sDate[0]);
                    mri.ReleaseDate = mri.ReleaseDate.Substring(mri.ReleaseDate.Length - 10, 10);

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
                        if (DateTime.Parse(mri.ReleaseDate) < DateTime.Now.AddDays(-3)) continue;
                    }
                    catch (Exception ex) { continue; }
                    try
                    {
                        //得到标题
                        mri.Title = HtmlUtil.NoHTML(HtmlUtil.GetElementsByTagName(lis[i], "h3")[0]);
                        string[] temp = HtmlUtil.GetElementsByClass(lis[i], "c-abstract");

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
                            if (UrlThereare(mri.Title, this.dtqueryinfo, dtWebQueryInfo, false) != 0)
                            { isThere = true; continue; }
                        }

                        mri.KeyWords = dtkey.Rows[kw]["KeyWord"].ToString().Trim();
                        mri.CollectDate = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        mri.Snapshot = "";
                        mri.ReleaseName = "";

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
                        dr[5] = mri.KeyWords;
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
                        dvAll.Refresh();
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
            //MessageBox.Show("ok");
            lbAll.Text = "一轮搜索完毕！";
            lbAll.ForeColor = Color.Red;
        }

        #endregion

        #region 得到网站的新闻类数据
        /// <summary>
        /// 得到网站的新闻类数据
        /// </summary>
        private void GetWebNewsInfo()
        {
            lbWeb.Text = "";
            lbWeb.Visible = true;
            //相似链接
            string Similar = "";

            DataBaseServer.SQLitecommand cmd = new SQLitecommand();

            //得到相似表
            DataTable dtXs = new DataTable();
            dtXs = cmd.GetTabel("Select * from WebAddress WHERE pid=0");

            dtParts = cmd.GetTabel("SELECT * FROM partword");

            DataTable dtkey = new DataTable();
            dtkey = cmd.GetTabel("select * from Keywords");

            //相似表中的被抓取网址
            string webInfo = "";

            //要过滤链接中首页的正则
            string strTopFormat = "http://.+/";
            List<string> strTop = new List<string>();
            sb = new StringBuilder();
            sb.Append("");
            string filterStr = "";

            #region 读取相似度表中的数据据,循环抓取
            for (int xs = 0; xs < dtXs.Rows.Count; xs++)
            {
                lbWeb.ForeColor = Color.DarkBlue;
                lbWeb.Text = "正在搜索:" + dtXs.Rows[xs]["name"].ToString();
                //读取相似表中要抓取的网址
                webInfo = getHtml(dtXs.Rows[xs]["url"].ToString(), "");
                //读取相似链接
                Similar = dtXs.Rows[xs]["likeurl"].ToString();

                //取出
                //string[] strA = HtmlUtil.GetElementsByTagName(webInfo, "a");
                List<string> strList = HtmlUtil.GetElementsByTagNameList(webInfo, "a");

                string strURLformat = "http://.[^\"]+";

                TbReleaseInfo ri = new TbReleaseInfo();

                string[] strA = GetLIstDate(strList.Distinct());
                #region 逐个链接判断
                //循环时判断是否要验证
                bool isThere = false;

                for (int i = 0; i < strA.Length; i++)
                {
                    if (Program.ProClose == true) break;
                    Application.DoEvents();
                    try
                    {
                        //得到目标网址中的所有链接,如果未得到,那么就继续读取下一个
                        strA[i] = HtmlUtil.GetListByHtml(dtXs.Rows[xs]["url"].ToString(), strA[i], strURLformat)[0];
                        //处理含有单引号的链接
                        strA[i] = UrlCl(strA[i]);

                        //处理单引号的链接
                        if (strA[i].IndexOf("'") != -1)
                        {
                            strA[i] = GetstringByHtmlArray(strA[i], "http://.[^\']+");
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    //得到相似值,大于0.70的认为相同,并开始抓取
                    if (HtmlUtil.getSimilarDegree(Similar, strA[i]) >= 0.60)
                    {
                        //判断这个链接是否已经在库中或者列表中,如果存在,此次就不再执行
                        strTop = HtmlUtil.GetListByHtmlArray(strA[i], strTopFormat);
                        if (strTop.Count != 0)
                        {
                            //if (strTop[0] == "http://blog.sohu.com/")
                            continue;//同新闻,如果将首页去掉
                        }

                        if (isThere)
                        {
                            continue;
                        }
                        else
                        {
                            //if (strA[i] == "http://news.ifeng.com/mainland/detail_2013_10/18/30459577_0.shtml'>[详细]</a>")
                            //{
                            //    strA[i] = strA[i];
                            //}

                            if (UrlThereare(strA[i], this.dtnewsinfo, dtWebNewsInfo, true) != 0) { isThere = true; continue; }
                        }

                        //得到此链接的源码
                        webInfo = getHtml(strA[i], "");
                        if (webInfo.Length == 0) { continue; }

                        //创建数据对象
                        ModelReleaseInfo newsInfo = new ModelReleaseInfo();

                        try
                        {
                            //流水+1
                            newsInfo.Uid = this.dvAll.Rows.Count + 1;

                            //标题
                            string[] strT = HtmlUtil.GetElementsByTagName(webInfo, "title");
                            if (strT.Length == 0)
                            {
                                continue;
                            }
                            else
                            {
                                newsInfo.Title = HtmlUtil.NoHTML(HtmlUtil.GetElementsByTagName(webInfo, "title")[0]);
                            }

                            //得到正文,以P标签来区分
                            string[] strContext = HtmlUtil.GetElementsByTagName(webInfo, "p");
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
                            newsInfo.KeyWords = "";
                            for (int j = 0; j < dtkey.Rows.Count; j++)
                            {
                                Application.DoEvents();
                                if (newsInfo.Contexts.IndexOf(dtkey.Rows[j][1].ToString()) > 0)
                                { newsInfo.KeyWords += dtkey.Rows[j][1].ToString() + ","; }
                                else
                                {

                                }
                            }
                            if (newsInfo.KeyWords.Length == 0) { continue; }
                            newsInfo.KeyWords = newsInfo.KeyWords.Substring(0, newsInfo.KeyWords.Length - 1);

                            //收集日期
                            newsInfo.CollectDate = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                            //发布人和发布日期暂时无法取到,手工赋值为空
                            newsInfo.ReleaseDate = "";
                            newsInfo.ReleaseName = "";

                            //网页快照,这里为用户指定生成,如果未选择生成,那么为空
                            newsInfo.Snapshot = "";

                            //网站名
                            newsInfo.WebName = dtXs.Rows[xs]["Name"].ToString();
                            //pid
                            newsInfo.Pid = 0;
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
                            dr[5] = newsInfo.KeyWords;
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
                            dvWeb.Refresh();
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
            dtWebNewsInfo = tri.SelReleaseInfo(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd HH:mm:ss"), "0 AND webName<>'百度'");
            //MessageBox.Show("ok");
            lbWeb.Text = "一轮搜索完毕！";
            lbWeb.ForeColor = Color.Red;
        }
        #endregion

        #region 得到网站的博客类数据
        /// <summary>
        /// 得到网站的博客类数据
        /// </summary>
        private void GetWebBlogInfo()
        {
            lbBlog.Text = "";
            lbBlog.Visible = true;
            //相似链接
            string Similar = "";

            DataBaseServer.SQLitecommand cmd = new SQLitecommand();

            //得到相似表
            DataTable dtXs = new DataTable();
            dtXs = cmd.GetTabel("Select * from WebAddress WHERE pid=1");

            dtParts = cmd.GetTabel("SELECT * FROM partword");

            DataTable dtkey = new DataTable();
            dtkey = cmd.GetTabel("select * from Keywords");

            //相似表中的被抓取网址
            string webInfo = "";
            sb = new StringBuilder();
            sb.Append("");
            string filterStr = "";

            //要过滤链接中首页的正则
            string strTopFormat = "http://.+/";
            List<string> strTop = new List<string>();

            #region 读取相似度表中的数据据,循环抓取
            for (int xs = 0; xs < dtXs.Rows.Count; xs++)
            {
                lbBlog.ForeColor = Color.DarkBlue;
                lbBlog.Text = "正在搜索:" + dtXs.Rows[xs]["name"].ToString();
                //读取相似表中要抓取的网址
                webInfo = getHtml(dtXs.Rows[xs]["url"].ToString(), "");
                //读取相似链接
                Similar = dtXs.Rows[xs]["likeurl"].ToString();

                //取出
                List<string> strList = HtmlUtil.GetElementsByTagNameList(webInfo, "a");
                //string[] strA = HtmlUtil.GetElementsByTagName(webInfo, "a");

                string strURLformat = "http://.[^\"]+";

                TbReleaseInfo ri = new TbReleaseInfo();

                string[] strA = GetLIstDate(strList.Distinct());

                #region 逐个链接判断
                //循环时判断是否要验证
                bool isThere = false;

                for (int i = 0; i < strA.Length; i++)
                {
                    if (Program.ProClose == true) break;
                    try
                    {
                        //得到目标网址中的所有链接,如果未得到,那么就继续读取下一个
                        strA[i] = HtmlUtil.GetListByHtml(dtXs.Rows[xs]["url"].ToString(), strA[i], strURLformat)[0];
                        //处理含有单引号的链接
                        strA[i] = UrlCl(strA[i]);

                        //处理单引号的链接
                        if (strA[i].IndexOf("'") != -1)
                        {
                            strA[i] = GetstringByHtmlArray(strA[i], "http://.[^\']+");
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    //得到相似值,大于0.70的认为相同,并开始抓取
                    //this.listBox1.Items.Add((strA[i]) + "-" + HtmlUtil.getSimilarDegree(Similar, strA[i]).ToString());
                    if (HtmlUtil.getSimilarDegree(Similar, strA[i]) >= 0.5)
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
                            if (UrlThereare(strA[i], this.dtbloginfo, dtWebBlogInfo, true) != 0) { isThere = true; continue; }
                        }

                        //得到此链接的源码
                        webInfo = getHtml(strA[i], "");
                        if (webInfo.Length == 0) { continue; }

                        //创建数据对象
                        ModelReleaseInfo newsInfo = new ModelReleaseInfo();

                        try
                        {
                            //流水+1
                            newsInfo.Uid = this.dvAll.Rows.Count + 1;

                            //标题:如果连标题都没有,那么忽略掉这个页
                            try
                            {
                                newsInfo.Title = HtmlUtil.NoHTML(HtmlUtil.GetElementsByTagName(webInfo, "title")[0]);
                            }
                            catch (Exception)
                            {
                                continue;
                            }

                            //得到正文,以P标签来区分
                            string[] strContext = HtmlUtil.GetElementsByTagName(webInfo, "p");
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
                            newsInfo.KeyWords = "";
                            for (int j = 0; j < dtkey.Rows.Count; j++)
                            {
                                Application.DoEvents();
                                if (newsInfo.Contexts.IndexOf(dtkey.Rows[j][1].ToString()) > 0)
                                { newsInfo.KeyWords += dtkey.Rows[j][1].ToString() + ","; }
                                else { }
                            }
                            if (newsInfo.KeyWords.Length == 0) { continue; }
                            newsInfo.KeyWords = newsInfo.KeyWords.Substring(0, newsInfo.KeyWords.Length - 1);
                            //收集日期
                            newsInfo.CollectDate = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                            //发布人和发布日期暂时无法取到,手工赋值为空
                            newsInfo.ReleaseDate = "";
                            newsInfo.ReleaseName = "";

                            //网页快照,这里为用户指定生成,如果未选择生成,那么为空
                            newsInfo.Snapshot = "";

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
                            dr[5] = newsInfo.KeyWords;
                            dr[6] = newsInfo.ReleaseName;
                            dr[7] = newsInfo.CollectDate;
                            dr[8] = newsInfo.Snapshot;
                            dr[9] = newsInfo.WebName;
                            dr[10] = newsInfo.Pid;
                            dr[11] = newsInfo.Part;
                            dr[12] = newsInfo.Reposts;
                            dr[13] = newsInfo.Comments;

                            //把行加到DT中
                            dtbloginfo.Rows.InsertAt(dr, 0);

                            //数据源刷新
                            if (dtbloginfo.Rows.Count >= 500)
                            {
                                dtbloginfo.Rows.RemoveAt(500);
                            }
                            dvBlog.Refresh();
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
            dtWebBlogInfo = tri.SelReleaseInfo(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd HH:mm:ss"), "1");
            //MessageBox.Show("ok");
            lbBlog.Text = "一轮搜索完毕！";
            lbBlog.ForeColor = Color.Red;
            lbBlog.Visible = true;
        }
        #endregion

        #region 得到网站的论坛类数据
        /// <summary>
        /// 得到网站的论坛类数据
        /// </summary>
        private void GetWebBBSInfo()
        {
            lbBBs.Text = "";
            lbBBs.Visible = true;
            //相似链接
            string Similar = "";

            DataBaseServer.SQLitecommand cmd = new SQLitecommand();

            //得到相似表
            DataTable dtXs = new DataTable();
            dtXs = cmd.GetTabel("Select * from WebAddress WHERE pid=2");

            dtParts = cmd.GetTabel("SELECT * FROM partword");

            DataTable dtkey = new DataTable();
            dtkey = cmd.GetTabel("select * from Keywords");

            //相似表中的被抓取网址
            string webInfo = "";

            sb = new StringBuilder();
            sb.Append("");
            string filterStr = "";

            //要过滤链接中首页的正则
            string strTopFormat = "http://.+/";
            List<string> strTop = new List<string>();

            #region 读取相似度表中的数据据,循环抓取
            for (int xs = 0; xs < dtXs.Rows.Count; xs++)
            {
                lbBBs.ForeColor = Color.DarkBlue;
                lbBBs.Text = "正在搜索:" + dtXs.Rows[xs]["name"].ToString();
                //读取相似表中要抓取的网址
                webInfo = getHtml(dtXs.Rows[xs]["url"].ToString(), "");
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
                    try
                    {
                        //得到目标网址中的所有链接,如果未得到,那么就继续读取下一个
                        strA[i] = HtmlUtil.GetListByHtml(dtXs.Rows[xs]["url"].ToString(), strA[i], strURLformat)[0];
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    //得到相似值,大于0.70的认为相同,并开始抓取
                    //this.listBox1.Items.Add((strA[i]) + "-" + HtmlUtil.getSimilarDegree(Similar, strA[i]).ToString() + "-" + i.ToString());
                    if (HtmlUtil.getSimilarDegree(Similar, strA[i]) >= 0.65)
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
                            if (UrlThereare(strA[i], this.dtbbsinfo, dtWebBBSInfo, true) != 0) { isThere = true; continue; }
                        }
                        ModelReleaseInfo newsInfo = new ModelReleaseInfo();
                        try
                        {
                            //得到此链接的源码
                            webInfo = getHtml(strA[i], "");
                            if (webInfo.Length == 0) { continue; }

                            //创建数据对象


                            //流水+1
                            newsInfo.Uid = this.dvAll.Rows.Count + 1;

                            //标题
                            newsInfo.Title = HtmlUtil.NoHTML(HtmlUtil.GetElementsByTagName(webInfo, "title")[0]);

                            //得到正文,以P标签来区分
                            string[] strContext = HtmlUtil.GetElementsByTagName(webInfo, "p");
                            newsInfo.Contexts = "";
                            for (int j = 0; j < strContext.Length; j++)
                            {
                                //循环累加正文信息
                                newsInfo.Contexts += HtmlUtil.NoHTML(strContext[j]);
                            }

                            newsInfo.Contexts = getConnect(webInfo);

                            //如果正文信息为空,那么将无法做关键字对照,此条数据舍弃
                            if (newsInfo.Contexts.Length == 0)
                            {
                                continue;
                            }

                            //网站链接
                            newsInfo.InfoSource = strA[i].Trim();

                            //关键字的设置
                            newsInfo.KeyWords = "";
                            for (int j = 0; j < dtkey.Rows.Count; j++)
                            {
                                Application.DoEvents();
                                if (newsInfo.Contexts.IndexOf(dtkey.Rows[j][1].ToString()) > 0)
                                { newsInfo.KeyWords += dtkey.Rows[j][1].ToString() + ","; }
                                else
                                {

                                }
                            }
                            if (newsInfo.KeyWords.Length == 0) { continue; }
                            newsInfo.KeyWords = newsInfo.KeyWords.Substring(0, newsInfo.KeyWords.Length - 1);

                            //收集日期
                            newsInfo.CollectDate = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                            //发布人和发布日期暂时无法取到,手工赋值为空
                            newsInfo.ReleaseDate = "";
                            newsInfo.ReleaseName = "";

                            //网页快照,这里为用户指定生成,如果未选择生成,那么为空
                            newsInfo.Snapshot = "";

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
                            dr[5] = newsInfo.KeyWords;
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
                            StreamWriter sw = File.AppendText("log.txt");
                            sw.WriteLine(DateTime.Now.ToLongDateString());
                            sw.WriteLine("begin");
                            sw.WriteLine(ex.Message);

                            sw.WriteLine("end");
                            sw.WriteLine("");

                            sw.Close();
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
            dtWebBBSInfo = tri.SelReleaseInfo(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd HH:mm:ss"), "2");
            //MessageBox.Show("ok");
            lbBBs.Text = "一轮搜索完毕！";
            lbBBs.ForeColor = Color.Red;
        }
        #endregion

        private string UrlCl(string str)
        {
            if (str.IndexOf(" ") != -1)
            {
                return str.Substring(0, str.IndexOf(" ") - 1);
            }
            return str;
        }

        /// <summary>
        /// 处理正文
        /// </summary>
        /// <param name="Htmlstr"></param>
        /// <returns></returns>
        private string getConnect(string Htmlstr)
        {
            int ib = 0, ie = 0;
            Htmlstr = Htmlstr.ToLower().Replace("<br>", "</p><p>");
            Htmlstr = Htmlstr.ToLower().Replace("<br />", "</p><p>");

            ib = Htmlstr.IndexOf("</p><p>");
            ie = Htmlstr.LastIndexOf("</p><p>");
            Htmlstr = Htmlstr.Substring(ib + 4, ie - ib);
            Htmlstr = Htmlstr.Replace("'", "\"");
            return HtmlUtil.NoHTML(Htmlstr);
        }

        private DataView dv;
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

        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] SplitString(string strContent, string strSplit)
        {
            if (!StrIsNullOrEmpty(strContent))
            {
                if (strContent.IndexOf(strSplit) < 0)
                    return new string[] { strContent };

                return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
            }
            else
                return new string[0] { };
        }

        /// <summary>
        /// 字段串是否为Null或为""(空)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool StrIsNullOrEmpty(string str)
        {
            if (str == null || str.Trim() == string.Empty)
                return true;

            return false;
        }

        /// <summary>   
        /// 用HttpWebRequest取得网页源码   
        /// 对于带BOM的网页很有效，不管是什么编码都能正确识别   
        /// </summary>   
        /// <param name="url">网页地址" </param>    
        /// <returns>返回网页源文件</returns>   
        public static string GetHtmlSource1(string url)
        {
            try
            { //处理内容   
                string html = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Accept = "*/*"; //接受任意文件
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.1.4322)"; // 模拟使用IE在浏览
                request.AllowAutoRedirect = true;//是否允许302
                request.Timeout = 5000;
                //request.CookieContainer = new CookieContainer();//cookie容器，
                request.Referer = url; //当前页面的引用

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream, Encoding.Default);
                html = reader.ReadToEnd();

                stream.Close();

                return html;
            }
            catch (Exception e) { return ""; }

        }

        private string getHtml(string url, string charSet)//url是要访问的网站地址，charSet是目标网页的编码，如果传入的是null或者""，那就自动分析网页的编码
        {
            try
            {
                WebClient myWebClient = new WebClient();//创建WebClient实例myWebClient 
                // 需要注意的：
                //有的网页可能下不下来，有种种原因比如需要cookie,编码问题等等
                //这是就要具体问题具体分析比如在头部加入cookie
                // webclient.Headers.Add("Cookie", cookie);
                //这样可能需要一些重载方法。根据需要写就可以了 

                //获取或设置用于对向 Internet 资源的请求进行身份验证的网络凭据。
                myWebClient.Credentials = CredentialCache.DefaultCredentials;
                //如果服务器要验证用户名,密码 
                //NetworkCredential mycred = new NetworkCredential(struser, strpassword);
                //myWebClient.Credentials = mycred;
                //从资源下载数据并返回字节数组。（加@是因为网址中间有"/"符号）
                byte[] myDataBuffer = myWebClient.DownloadData(url);
                string strWebData = Encoding.Default.GetString(myDataBuffer);

                //获取网页字符编码描述信息 
                Match charSetMatch = Regex.Match(strWebData, "<meta([^<]*)charset=([^<]*)\"", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                string webCharSet = charSetMatch.Groups[2].Value;
                if (charSet == null || charSet == "")
                    charSet = webCharSet;

                try
                {
                    if (charSet != null && charSet != "" && Encoding.GetEncoding(charSet) != Encoding.Default)
                        strWebData = Encoding.GetEncoding(charSet).GetString(myDataBuffer);
                    else
                    {
                        strWebData = GetHtmlSource1(url);
                    }
                }
                catch (Exception)
                {

                    strWebData = Encoding.Default.GetString(myDataBuffer);
                }

                return strWebData;
            }
            catch (Exception)
            {
                return "";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            getWebImage("http://www.baidu.com", "utf-8", "e:\\123.jpg");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            reFreshWeibo();
        }

        //刷新微博数据,前台感觉是在搜索微博
        private void reFreshWeibo()
        {
            if (!Program.ProClose)
            {
                lbweibo.Visible = true;
                lbweibo.Text = "正在搜索微博数据...";
                lbweibo.ForeColor = Color.DarkBlue;
                string wblog = "select uid,releasename AS title,contexts,releasedate,infosource,keywords,releasename,collectdate,snapshot,webname,pid,part,reposts,comments from ReleaseInfowb order by uid desc limit 0,300";
                DataBaseServer.SQLitecommand cmd = new SQLitecommand();
                DataTable dtwBlog = new DataTable();
                dtwBlog = cmd.GetTabel(wblog);
                dvWBlog.DataSource = dtwBlog;
                dvWBlog.Refresh();
                lbweibo.Text = "一轮搜索完毕！";
                lbweibo.ForeColor = Color.Red;
            }
        }

        bool but = false;
        int js = 0;
        /// <summary>
        /// 时间控制5分钟运行一次
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerMAIN_Tick(object sender, EventArgs e)
        {
            //点击开始后先运行一次
            if (js == 5)
            {
                BeginEvn();
                js = 0;
            }
            else
            {
                js += 1;
            }
        }

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
        #endregion

        #region 表格选中事件，获取选中行的标题，关键字，链接，内容，拼合到下面的dataView里
        //表格选中事件，获取选中行的标题，关键字，链接，内容，拼合到下面的dataView里
        private void dvAll_SelectionChanged(object sender, EventArgs e)
        {
            dataView.Clear();
            if (dvAll.SelectedRows.Count == 0) return;

            string title = dvAll.CurrentCell.OwningRow.Cells[1].Value.ToString();

            //设置textbox内容为标题加链接
            dataView.Text = "标题：" + title + "\n链接：" + dvAll.CurrentCell.OwningRow.Cells[4].Value.ToString() + "\n";

            //设置标题粗体
            dataView.Select(3, title.Length);
            dataView.SelectionFont = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold);

            //获取当前textbox内容长度，+4是表示内容文本空两个空格
            int length = dataView.Text.Length + 4;

            //添加内容
            dataView.AppendText("    " + dvAll.CurrentCell.OwningRow.Cells[2].Value.ToString());

            //分割关键字
            string[] keywords = dvAll.CurrentCell.OwningRow.Cells[5].Value.ToString().Trim().Split(',');
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
            dataView.Select(0, 0);
        }

        private void dvBBs_SelectionChanged(object sender, EventArgs e)
        {
            dataView.Clear();
            if (dvBBs.SelectedRows.Count == 0) return;

            string title = dvBBs.CurrentCell.OwningRow.Cells[1].Value.ToString();

            //设置textbox内容为标题加链接
            dataView.Text = "标题：" + title + "\n链接：" + dvBBs.CurrentCell.OwningRow.Cells[4].Value.ToString() + "\n";

            //设置标题粗体
            dataView.Select(3, title.Length);
            dataView.SelectionFont = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold);

            //获取当前textbox内容长度，+4是表示内容文本空两个空格
            int length = dataView.Text.Length + 4;

            //添加内容
            dataView.AppendText("    " + dvBBs.CurrentCell.OwningRow.Cells[2].Value.ToString());

            //分割关键字
            string[] keywords = dvBBs.CurrentCell.OwningRow.Cells[5].Value.ToString().Trim().Split(',');
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
            dataView.Select(0, 0);
        }

        private void dvWeb_SelectionChanged(object sender, EventArgs e)
        {
            dataView.Clear();
            if (dvWeb.SelectedRows.Count == 0) return;

            string title = dvWeb.CurrentCell.OwningRow.Cells[1].Value.ToString();

            //设置textbox内容为标题加链接
            dataView.Text = "标题：" + title + "\n链接：" + dvWeb.CurrentCell.OwningRow.Cells[4].Value.ToString() + "\n";

            //设置标题粗体
            dataView.Select(3, title.Length);
            dataView.SelectionFont = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold);

            //获取当前textbox内容长度，+4是表示内容文本空两个空格
            int length = dataView.Text.Length + 4;

            //添加内容
            dataView.AppendText("    " + dvWeb.CurrentCell.OwningRow.Cells[2].Value.ToString());

            //分割关键字
            string[] keywords = dvWeb.CurrentCell.OwningRow.Cells[5].Value.ToString().Split(',');
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
            dataView.Select(0, 0);
        }

        private void dvBlog_SelectionChanged(object sender, EventArgs e)
        {
            dataView.Clear();
            if (dvBlog.SelectedRows.Count == 0) return;

            string title = dvBlog.CurrentCell.OwningRow.Cells[1].Value.ToString();

            //设置textbox内容为标题加链接
            dataView.Text = "标题：" + title + "\n链接：" + dvBlog.CurrentCell.OwningRow.Cells[4].Value.ToString() + "\n";

            //设置标题粗体
            dataView.Select(3, title.Length);
            dataView.SelectionFont = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold);

            //获取当前textbox内容长度，+4是表示内容文本空两个空格
            int length = dataView.Text.Length + 4;

            //添加内容
            dataView.AppendText("    " + dvBlog.CurrentCell.OwningRow.Cells[2].Value.ToString());

            //分割关键字
            string[] keywords = dvBlog.CurrentCell.OwningRow.Cells[5].Value.ToString().Trim().Split(',');
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
            dataView.Select(0, 0);
        }

        private void dvWBlog_SelectionChanged(object sender, EventArgs e)
        {
            dataView.Clear();
            if (dvWBlog.SelectedRows.Count == 0) return;

            string title = dvWBlog.CurrentCell.OwningRow.Cells[1].Value.ToString();

            //设置textbox内容为标题加链接
            dataView.Text = "标题：" + title + "\n链接：" + dvWBlog.CurrentCell.OwningRow.Cells[4].Value.ToString() + "\n";

            //设置标题粗体
            dataView.Select(3, title.Length);
            dataView.SelectionFont = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold);

            //获取当前textbox内容长度，+4是表示内容文本空两个空格
            int length = dataView.Text.Length + 4;

            //添加内容
            dataView.AppendText("    " + dvWBlog.CurrentCell.OwningRow.Cells[2].Value.ToString());

            //分割关键字
            string[] keywords = dvWBlog.CurrentCell.OwningRow.Cells[5].Value.ToString().Trim().Split(',');
            foreach (string kw in keywords)
            {
                int kl = length;//设定文本开始位置
                int wl = kw.Length;

                //以关键词分割内容为数组
                string[] str = Regex.Split(dvWBlog.CurrentCell.OwningRow.Cells[2].Value.ToString(), kw, RegexOptions.IgnoreCase);
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
            dataView.Select(0, 0);
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

        private void dvWBlog_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
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

        #endregion

        private void FormatDataView(DataGridView obj, bool isWeibo)
        {
            obj.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "uid", DataPropertyName = "uid" });
            obj.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "关键词", DataPropertyName = "keywords" });
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

            obj.Columns[0].Visible = false;
            obj.Columns[3].Visible = false;
            obj.Columns[5].Visible = false;
            obj.Columns[6].Visible = false;
            obj.Columns[7].Visible = false;

            obj.Columns[8].Visible = false;
            obj.Columns[12].Visible = false;
            obj.Columns[13].Visible = false;

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
            if (isWeibo == true)
            {
                obj.Columns[3].Visible = false;
                obj.Columns[12].Visible = true;
            }
        }

        #region 切换页签时刷新下方的内容
        //切换页签时刷新下方的内容
        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            dataView.Clear();
            switch (e.TabPage.Text)
            {
                case "搜索引擎监控":
                    dvAll_SelectionChanged(null, null);
                    break;
                case "博客监控":
                    dvBlog_SelectionChanged(null, null);
                    break;
                case "论坛监控":
                    dvBBs_SelectionChanged(null, null);
                    break;
                case "微博监控":
                    dvWBlog_SelectionChanged(null, null);
                    break;
                case "其他监控":
                    dvWeb_SelectionChanged(null, null);
                    break;
            }

        }
        #endregion
        private void stmKZ_Click(object sender, EventArgs e)
        {
            if (dvAll.SelectedRows.Count == 0)
                return;
            string temp = "";

            try
            {
                Entities.SystemSet ss = (Entities.SystemSet)GlobalPars.GloPars["systemset"];
                temp = ss.EvidenceImgSavePath + "\\" + DateTime.Now.ToString().Replace(":", "-") + ".jpg";

                Bitmap image = util.WebSnap.StartSnap(this.dvAll.SelectedRows[0].Cells[4].Value.ToString());
                image.Save(temp);
                MessageBox.Show("证据图片生成成功！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                temp = ss.EvidenceImgSavePath + "\\" + DateTime.Now.ToString().Replace(":", "-") + ".jpg";

                Bitmap image = util.WebSnap.StartSnap(this.dvBlog.SelectedRows[0].Cells[4].Value.ToString());
                image.Save(temp);
                MessageBox.Show("证据图片生成成功！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                temp = ss.EvidenceImgSavePath + "\\" + DateTime.Now.ToString().Replace(":", "-") + ".jpg";

                Bitmap image = util.WebSnap.StartSnap(this.dvBlog.SelectedRows[0].Cells[4].Value.ToString());
                image.Save(temp);
                MessageBox.Show("证据图片生成成功！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                temp = ss.EvidenceImgSavePath + "\\" + DateTime.Now.ToString().Replace(":", "-") + ".jpg";

                Bitmap image = util.WebSnap.StartSnap(this.dvWBlog.SelectedRows[0].Cells[4].Value.ToString());
                image.Save(temp);
                MessageBox.Show("证据图片生成成功！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                temp = ss.EvidenceImgSavePath + "\\" + DateTime.Now.ToString().Replace(":", "-") + ".jpg";

                Bitmap image = util.WebSnap.StartSnap(this.dvWeb.SelectedRows[0].Cells[4].Value.ToString());
                image.Save(temp);
                MessageBox.Show("证据图片生成成功！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        DataBaseServer.SQLitecommand cmd = new DataBaseServer.SQLitecommand();

        public void RefreshChartData(object sender, EventArgs e)
        {
            getData();
            getKwData();
        }

        DataTable piddt = new DataTable();
        string[] pidx = new string[] { "微博", "博客", "论坛", "其他" };
        int[] pidy = new int[] { 0, 0, 0, 0 };
        DataTable partdt = new DataTable();
        string[] partx = new string[] { "正向", "负向" };
        int[] party = new int[] { 0, 0 };
        string now = DateTime.Now.ToString("yyyy-MM-dd");

        //快速排序
        private void Sort(string[][] a, int left, int right, int index)
        {
            if (left < right)
            {
                int i = Partition(a, left, right, index);
                Sort(a, left, i - 1, index);
                Sort(a, i + 1, right, index);
            }
        }

        private int Partition(string[][] a, int left, int right, int index)
        {

            string[] tmp = a[left];
            while (left < right)
            {
                while (left < right && Convert.ToInt32(a[right][index]) >= Convert.ToInt32(tmp[index]))
                    right--;

                // 换位后不能将left+1,防止跳位  
                if (left < right)
                    a[left] = a[right];

                while (left < right && Convert.ToInt32(a[left][index]) <= Convert.ToInt32(tmp[index]))
                    left++;

                if (left < right)
                {
                    a[right] = a[left];
                    // 有left < right，可将right向前推一位  
                    right--;
                }
            }

            a[left] = tmp;

            return left;
        }

        private void getKwData()
        {
            kwChart.Titles[1].Text = now;
            DataTable dt = cmd.GetTabel("select keyword from keywords where has=1");
            string[][] keywords = new string[dt.Rows.Count][];
            DataTable kwdt = cmd.GetTabel("SELECT count(1),keyWords FROM v_ReleaseInfo where collectdate > '" + now + "'  GROUP BY keyWords");
            for (int i = 0, l = dt.Rows.Count; i < l; i++)
            {
                string keyword = dt.Rows[i]["keyword"].ToString();
                keywords[i] = new string[2];
                keywords[i][0] = keyword;
                keywords[i][1] = "0";
                for (int ki = 0, kl = kwdt.Rows.Count; ki < kl; ki++)
                {
                    if (kwdt.Rows[ki][1].ToString().IndexOf(keyword) > -1)
                    {
                        keywords[i][1] = Convert.ToString(Convert.ToInt32(keywords[i][1]) + Convert.ToInt32(kwdt.Rows[ki][0]));
                    }
                }
            }
            Sort(keywords, 0, keywords.Length - 1, 1);

            int kwl = 5;
            string[] kw = new string[kwl];
            int[] kwy = new int[kwl];
            int kwll = keywords.Length - 1;
            for (int i = 0; i < kwl; i++)
            {
                kw[i] = keywords[kwll - i][0];
                kwy[i] = Convert.ToInt32(keywords[kwll - i][1]);
            }
            kwChart.Series[0].Points.DataBindXY(kw, kwy);
        }

        private void getData()
        {
            pidChart.Titles[1].Text = now;
            partChart.Titles[1].Text = now;

            pidy = new int[] { 0, 0, 0, 0 };
            party = new int[] { 0, 0 };

            piddt = cmd.GetTabel("select count(1),pid from v_ReleaseInfo where collectdate > '" + now + "' GROUP BY pid");
            for (int i = 0, l = piddt.Rows.Count; i < l; i++)
            {
                switch (Convert.ToInt32(piddt.Rows[i][1]).ToString())
                {
                    case "1":
                        pidy[1] = pidy[1] + Convert.ToInt32(piddt.Rows[i][0]);
                        break;
                    case "2":
                        pidy[2] = pidy[2] + Convert.ToInt32(piddt.Rows[i][0]);
                        break;
                    case "3":
                        pidy[0] = pidy[0] + Convert.ToInt32(piddt.Rows[i][0]);
                        break;
                    default:
                        pidy[3] = pidy[3] + Convert.ToInt32(piddt.Rows[i][0]);
                        break;
                }
            }
            pidChart.Series[0].Points.DataBindXY(pidx, pidy);

            partdt = cmd.GetTabel("select count(1),part from v_ReleaseInfo where collectdate > '" + now + "' GROUP BY part");
            for (int i = 0, l = partdt.Rows.Count; i < l; i++)
            {
                switch (partdt.Rows[i][1].ToString())
                {
                    case "1":
                        party[0] = party[0] + Convert.ToInt32(partdt.Rows[i][0]);
                        break;
                    default:
                        party[1] = party[1] + Convert.ToInt32(partdt.Rows[i][0]);
                        break;
                }
            }
            partChart.Series[0].Points.DataBindXY(partx, party);
        }

        private void Monitoring_Resize(object sender, EventArgs e)
        {
            //this.pictureBox1.Left = this.tabControl1.Width - 20 - pictureBox1.Width;
            //this.button2.Left = this.pictureBox1.Left - 20 - button2.Width;
            //this.button2.Top = this.tabControl1.Top;
            //this.button1.Left = button2.Left - 20 - button1.Width;
            //this.button1.Top = button2.Top;
            ////this.button1.Top = this.tabControl1.Top + 20;
        }

        bool play = true;

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            if (play)
            {
                play = false;
                pictureBox6.BackgroundImage = imageList1.Images[2];
                butClike = !butClike;
                //DataBaseServer.SQLitecommand cmd = new SQLitecommand();
                //string sql = "SELECT * FROM partword";
                //dtParts = cmd.GetTabel(sql);

                this.pictureBox1.Visible = true;
                if (Program.isBeyondDate == true)
                {
                    Program.ProClose = true;
                }
                else
                {
                    Program.ProClose = false;
                }
                this.timerMAIN.Enabled = true;
                BeginEvn();

                reFreshWeibo();

                qwei = new util.QQWeibo(2 * 60 * 1000);
                qwei.Start();

                swei = new util.SinaWeibo(2 * 60 * 1000);
                swei.Start();
            }
            else
            {
                play = true;
                pictureBox6.BackgroundImage = imageList1.Images[0];
                butClike = !butClike;
                qwei.Stop();
                swei.Stop();
                this.pictureBox1.Visible = false;
                Program.ProClose = true;
                but = false;
            }
        }

        public static string GetstringByHtmlArray(string text, string pat)
        {

            Regex r = new Regex(pat, RegexOptions.IgnoreCase);
            Match m = r.Match(text);

            return m.Value;
        }
    }
}
