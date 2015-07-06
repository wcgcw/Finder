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
using Finder.Entities;
using Finder.UserControles;

namespace Finder.Forms
{
    public partial class Monitor : Form
    {
        #region 属性
        //3分钟从数据库取一次微博
        //System.Timers.Timer weiboSpiderTimer = new System.Timers.Timer(1000 * 60 * 3);
        //定时器，分两个，一个是有相似链接的定时器，一个是没有相似链接的定时器。
        //因为一个网站添加相似链接和不添加相似链接走的逻辑是不一样的。
        //加相似链接的话，只抓取列表网站这一层。
        //如果不加相似链接的话，则从列表网站做为入口，所有能到达的链接都要抓取。
        //System.Timers.Timer likeUrlWebSpiderTimer = new System.Timers.Timer(1000 * 10);
        //System.Timers.Timer nonLikeUrlWebSpiderTimer = new System.Timers.Timer(1000 * 10);

        //主流媒体数据源（百度新闻）
        private DataTable dtbaidunewsinfo;
        //贴吧数据源
        private DataTable dttiebainfo;
        //博客数据源
        private DataTable dtbloginfo;
        //论坛数据源
        private DataTable dtbbsinfo;
        //全网数据源（百度网页搜索）
        private DataTable dtbaiduwebinfo;
        //微信数据源
        private DataTable dtweixininfo;
        //新浪微博数据源
        private DataTable dtweiboinfo;
        //通用，自定义数据源
        private DataTable dtcustominfo;

        HtmlAgilityPack.HtmlDocument docPage = new HtmlAgilityPack.HtmlDocument();
        private StringBuilder sb;
        private DataTable dtWebNewsInfo;
        private DataTable dtTieBaInfo;
        private DataTable dtWebBlogInfo;
        private DataTable dtWebBBSInfo;
        private DataTable dtWebQueryInfo;
        private DataTable dtWeiXinInfo;
        private DataTable dtWeiboInfo;

        TbReleaseInfo tri;
        System.Threading.AutoResetEvent obj = new System.Threading.AutoResetEvent(false);

        //Finder.util.QQWeibo qwei;
        //Finder.util.SinaWeibo swei;
        //private bool butClike = false;
        DataBaseServer.SQLitecommand cmd = new DataBaseServer.SQLitecommand();
        private DataView dv;

        string SoftVer;
        private Queue<UrlInfo> urlQueue = new Queue<UrlInfo>();
        private WebBrowser sinaWeiBoWebBrowser;
        private WebBrowser qqWeiBoWebBrowser;

        //2015.3.9 wangcg
        private static readonly Object thisLockGeneral = new Object();
        bool GeneralTimerRunning = false;
        //通用抓取定时器
        System.Timers.Timer GeneralWebSpiderTimer = new System.Timers.Timer(1000 * 10);

        //2015.3.10 wangcg
        private static readonly Object thisLockMedia = new Object();
        bool MediaTimerRunning = false;
        //主流媒体抓取定时器
        System.Timers.Timer MediaWebSpiderTimer = new System.Timers.Timer(1000 * 10);

        //2015.3.11 wangcg
        private static readonly Object thisLockWeixin = new Object();
        bool WeixinTimerRunning = false;
        //微信抓取定时器
        System.Timers.Timer WeixinWebSpiderTimer = new System.Timers.Timer(1000 * 10);

        //2015.3.11 wangcg
        private static readonly Object thisLockBlog = new Object();
        bool BlogTimerRunning = false;
        //博客抓取定时器
        System.Timers.Timer BlogWebSpiderTimer = new System.Timers.Timer(1000 * 10);

        //2015.3.12 wangcg
        private static readonly Object thisLockBBS = new Object();
        bool BBSTimerRunning = false;
        //论坛抓取定时器
        System.Timers.Timer BBSWebSpiderTimer = new System.Timers.Timer(1000 * 10);

        //2015.3.14 wangcg
        private static readonly Object thisLockTieba = new Object();
        bool TiebaTimerRunning = false;
        //论坛抓取定时器
        System.Timers.Timer TiebaWebSpiderTimer = new System.Timers.Timer(1000 * 10);

        //2015.3.14 wangcg
        private static readonly Object thisLockWeibo = new Object();
        bool WeiboTimerRunning = false;
        //论坛抓取定时器
        System.Timers.Timer WeiboWebSpiderTimer = new System.Timers.Timer(1000 * 10);

        //2015.3.14 wangcg
        private static readonly Object thisLockBaiduWeb = new Object();
        bool BaiduWebTimerRunning = false;
        //全网抓取定时器
        System.Timers.Timer BaiduWebWebSpiderTimer = new System.Timers.Timer(1000 * 10);


        //刷新界面与切换tab的时候进行竞合
        private static readonly Object thisLockSwitch = new Object();

        private static readonly Object thisLockRefresh = new Object();
        bool RefreshTimerRunning = false;
        //全网抓取定时器
        System.Timers.Timer RefreshWebSpiderTimer = new System.Timers.Timer(1000 * 5);

        #endregion

        #region 初始化
        public Monitor()
        {
            InitializeComponent();
        }

        private void Monitoring_Load(object sender, EventArgs e)
        {
            #region 抓取定时器设置
            //2015.3.9 wangcg 通用抓取
            GeneralWebSpiderTimer.Elapsed += new System.Timers.ElapsedEventHandler(GeneralWebSpiderTimer_Elapsed);
            GeneralWebSpiderTimer.AutoReset = true;
            GeneralWebSpiderTimer.Enabled = false;

            //2015.3.10 wangcg 主流媒体抓取
            MediaWebSpiderTimer.Elapsed += new System.Timers.ElapsedEventHandler(MediaWebSpiderTimer_Elapsed);
            MediaWebSpiderTimer.AutoReset = true;
            MediaWebSpiderTimer.Enabled = false;

            //2015.3.11 wangcg 微信抓取
            WeixinWebSpiderTimer.Elapsed += new System.Timers.ElapsedEventHandler(WeixinWebSpiderTimer_Elapsed);
            WeixinWebSpiderTimer.AutoReset = true;
            WeixinWebSpiderTimer.Enabled = false;

            //2015.3.11 wangcg 博客抓取
            BlogWebSpiderTimer.Elapsed += new System.Timers.ElapsedEventHandler(BlogWebSpiderTimer_Elapsed);
            BlogWebSpiderTimer.AutoReset = true;
            BlogWebSpiderTimer.Enabled = false;

            //2015.3.11 wangcg 论坛抓取
            BBSWebSpiderTimer.Elapsed += new System.Timers.ElapsedEventHandler(BBSWebSpiderTimer_Elapsed);
            BBSWebSpiderTimer.AutoReset = true;
            BBSWebSpiderTimer.Enabled = false;

            //2015.3.14 wangcg 贴吧抓取
            TiebaWebSpiderTimer.Elapsed += new System.Timers.ElapsedEventHandler(TiebaWebSpiderTimer_Elapsed);
            TiebaWebSpiderTimer.AutoReset = true;
            TiebaWebSpiderTimer.Enabled = false;

            //2015.3.14 wangcg 新浪微博抓取
            WeiboWebSpiderTimer.Elapsed += new System.Timers.ElapsedEventHandler(WeiboWebSpiderTimer_Elapsed);
            WeiboWebSpiderTimer.AutoReset = true;
            WeiboWebSpiderTimer.Enabled = false;

            //2015.3.17 wangcg 百度网页搜索抓取
            BaiduWebWebSpiderTimer.Elapsed += new System.Timers.ElapsedEventHandler(BaiduWebWebSpiderTimer_Elapsed);
            BaiduWebWebSpiderTimer.AutoReset = true;
            BaiduWebWebSpiderTimer.Enabled = false;
            #endregion

            RefreshWebSpiderTimer.Elapsed += new System.Timers.ElapsedEventHandler(RefreshDataTimer_Elapsed);
            RefreshWebSpiderTimer.AutoReset = true;
            RefreshWebSpiderTimer.Enabled = false;

            //weiboSpiderTimer.Elapsed += new System.Timers.ElapsedEventHandler(weiboSpiderTimer_Elapsed);
            //weiboSpiderTimer.AutoReset = true;
            //weiboSpiderTimer.Enabled = false;

            //带相似链接的网页抓取定时器
            //likeUrlWebSpiderTimer.Elapsed += new System.Timers.ElapsedEventHandler(likeUrlWebSpiderTimer_Elapsed);
            //likeUrlWebSpiderTimer.AutoReset = true;
            //likeUrlWebSpiderTimer.Enabled = false;

            ////不带相似链接的网页抓取定时器
            //nonLikeUrlWebSpiderTimer.Elapsed += new System.Timers.ElapsedEventHandler(nonLikeUrlWebSpiderTimer_elapsed);
            //nonLikeUrlWebSpiderTimer.AutoReset = true;
            //nonLikeUrlWebSpiderTimer.Enabled = false;

            #region 初始化表格控件
            tri = new TbReleaseInfo();
            FormatDataView(dvAll, false);
            FormatDataView(dvBBs, false);
            FormatDataView(dvBlog, false);
            FormatDataView(dvWBlog, true);
            FormatDataView(dvWeb, false);
            FormatDataView(dvtieba, false);
            FormatDataView(dvWeiXin, false);
            FormatDataView(dvCustom, false);

            dtbaidunewsinfo = tri.GetReleaseInfoFormat();
            dttiebainfo = tri.GetReleaseInfoFormat();
            dtbloginfo = tri.GetReleaseInfoFormat();
            dtbbsinfo = tri.GetReleaseInfoFormat();
            dtbaiduwebinfo = tri.GetReleaseInfoFormat();
            dtweixininfo = tri.GetReleaseInfoFormat();
            dtweiboinfo = tri.GetReleaseInfoFormat();
            dtcustominfo = tri.GetReleaseInfoFormat();

            //dtWebNewsInfo = tri.SelReleaseInfo(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd HH:mm:ss"), "0 AND webName<>'百度'");
            dvWeb.DataSource = dtbaidunewsinfo;
            //dtTieBaInfo = tri.SelReleaseInfo(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd HH:mm:ss"), "5");
            dvtieba.DataSource = dttiebainfo;
            //dtWebBlogInfo = tri.SelReleaseInfo(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd HH:mm:ss"), "1");
            dvBlog.DataSource = dtbloginfo;
            //dtWebBBSInfo = tri.SelReleaseInfo(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd HH:mm:ss"), "2");
            dvBBs.DataSource = dtbbsinfo;
            //dtWebQueryInfo = tri.SelReleaseInfo(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd HH:mm:ss"), "4");
            dvAll.DataSource = dtbaiduwebinfo;
            //dtWeiXinInfo = tri.SelReleaseInfo(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd HH:mm:ss"), "6");
            dvWeiXin.DataSource = dtweixininfo;
            //dtWeiboInfo = tri.SelReleaseInfo(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd HH:mm:ss"), "3");
            dvWBlog.DataSource = dtweiboinfo;
            dvCustom.DataSource = dtcustominfo;
            //cmd.ExecuteNonQuery("insert into urls(url) select url from WebAddress where likeurl==null or likeurl=''");
            //cmd.ExecuteNonQuery("insert into Queue(url,pid,sheng,shi,xian) select url,pid,sheng,shi,xian from WebAddress where likeurl='' or likeurl=null");
            #endregion

            //DataTable queue = cmd.GetTabel("select url,title,pid,sheng,shi,xian,webname from Queue limit 0,200");
            //foreach (DataRow dr in queue.Rows)
            //{
            //    UrlInfo ui = new UrlInfo();
            //    ui.Pid = dr[2].ToString();
            //    ui.Title = dr[1].ToString();
            //    ui.Url = dr[0].ToString();
            //    ui.Sheng = dr[3].ToString();
            //    ui.Shi = dr[4].ToString();
            //    ui.Xian = dr[5].ToString();
            //    ui.WebName = dr[6].ToString();
            //    urlQueue.Enqueue(ui);
            //}

            //sinaWeiBoWebBrowser = new WebBrowser();
            //qqWeiBoWebBrowser = new WebBrowser();

            SoftVer = !GlobalPars.GloPars.ContainsKey("SoftVer") ? "1" : GlobalPars.GloPars["SoftVer"].ToString();
            if (!SoftVer.Equals("3"))
            {
                tabControl1.TabPages.RemoveByKey("tabPage7");
            }
            //ThreadPool.SetMaxThreads(20, 20);
            //ThreadPool.SetMinThreads(5, 5);
        }

        /// <summary>
        /// 写入数据库的线程
        /// </summary>
        private void Thread_WriteDataToDB()
        {
            DataPersistenceControl.GetInstance().Init();
            DataPersistenceControl.AsyncCaller caller = new DataPersistenceControl.AsyncCaller(DataPersistenceControl.GetInstance().StartWrite);

            IAsyncResult result = caller.BeginInvoke(null, null);

            result.AsyncWaitHandle.WaitOne();
            caller.EndInvoke(result);
            result.AsyncWaitHandle.Close();

        }
        #endregion

        #region 定时运行
        #region 旧程序
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
                this.BeginInvoke(new MethodInvoker(delegate()
                {
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

        private void BeginLikeUrlEvn()
        {
            if (!Program.ProClose)
            {
                //likeUrlWebSpiderTimer.Enabled = false;

                //AutoResetEvent news_resets = new AutoResetEvent(false);
                //AutoResetEvent blog_resets = new AutoResetEvent(false);
                //AutoResetEvent bbs_resets = new AutoResetEvent(false);
                //AutoResetEvent tieba_resets = new AutoResetEvent(false);

                //ThreadPool.QueueUserWorkItem(new WaitCallback(GetWebNewsInfo), news_resets);
                //ThreadPool.QueueUserWorkItem(new WaitCallback(GetWebBlogInfo), blog_resets);
                //ThreadPool.QueueUserWorkItem(new WaitCallback(GetWebBBSInfo), bbs_resets);
                //ThreadPool.QueueUserWorkItem(new WaitCallback(GetWebTieBaInfo), tieba_resets);

                //news_resets.WaitOne();
                //blog_resets.WaitOne();
                //bbs_resets.WaitOne();
                //tieba_resets.WaitOne();

                //likeUrlWebSpiderTimer.Enabled = true;

                Thread t = new Thread(new ThreadStart(GetWebNewsInfo));
                t.IsBackground = true;
                t.Start();

                //Thread b = new Thread(new ThreadStart(GetWebBlogInfo));
                //b.IsBackground = true;
                //b.Start();

                //Thread BB = new Thread(new ThreadStart(GetWebBBSInfo));
                //BB.IsBackground = true;
                //BB.Start();

                //Thread BB1 = new Thread(new ThreadStart(GetWebTieBaInfo));
                //BB1.IsBackground = true;
                //BB1.Start();

                //Thread Webexplorer = new Thread(new ThreadStart(GetBaiduInfo));
                //Webexplorer.IsBackground = true;
                //Webexplorer.Start();

                //if (SoftVer.Equals("3"))
                //{
                //Thread Weixin = new Thread(new ThreadStart(GetWeiXinInfo));
                //Weixin.IsBackground = true;
                //Weixin.Start();
                //}
            }
        }
        private void likeUrlWebSpiderTimer_Elapsed(object sender, EventArgs e)
        {
            BeginLikeUrlEvn();
        }

        private void BeginNonLikeUrlEvn()
        {
            if (!Program.ProClose)
            {
                //nonLikeUrlWebSpiderTimer.Enabled = false;

                AutoResetEvent sina_resets = new AutoResetEvent(false);
                AutoResetEvent qq_resets = new AutoResetEvent(false);
                AutoResetEvent baidu_resets = new AutoResetEvent(false);
                AutoResetEvent weixin_resets = new AutoResetEvent(false);
                AutoResetEvent nonlikeurl_resets = new AutoResetEvent(false);

                ThreadPool.QueueUserWorkItem(new WaitCallback(GetNonLikeUrlInfo), nonlikeurl_resets);
                ThreadPool.QueueUserWorkItem(new WaitCallback(GetSinaWeiBoInfo), sina_resets);
                ThreadPool.QueueUserWorkItem(new WaitCallback(GetQQWeiBoInfo), qq_resets);
                if (SoftVer.Equals("3"))
                {
                    //ThreadPool.QueueUserWorkItem(new WaitCallback(GetWeiXinInfo), weixin_resets);
                }
                ThreadPool.QueueUserWorkItem(new WaitCallback(GetBaiduInfo), baidu_resets);

                sina_resets.WaitOne();
                qq_resets.WaitOne();
                baidu_resets.WaitOne();
                //weixin_resets.WaitOne();
                nonlikeurl_resets.WaitOne();

                //nonLikeUrlWebSpiderTimer.Enabled = true;
            }
        }
        private void nonLikeUrlWebSpiderTimer_elapsed(object sender, EventArgs e)
        {
            BeginNonLikeUrlEvn();
        }
        #endregion
        #region 2015.3 wangcg 抓取调度程序
        #region 通用抓取程序
        private void BeginGeneralEvn()
        {
            if (!Program.ProClose)
            {
                //Thread t = new Thread(new ThreadStart(GeneralWebSpider));
                //t.IsBackground = true;
                //t.Start();
                GeneralWebSpider();
            }
        }
        private void GeneralWebSpiderTimer_Elapsed(object sender, EventArgs e)
        {
            lock (thisLockGeneral)
            {
                if (GeneralTimerRunning) return;
                GeneralTimerRunning = true;
            }

            try
            {
                BeginGeneralEvn();
            }
            finally
            {
                lock (thisLockGeneral)
                {
                    GeneralTimerRunning = false;
                }
            }
        }
        #endregion

        #region 主流媒体抓取程序
        private void BeginMediaEvn()
        {
            if (!Program.ProClose)
            {
                //Thread t = new Thread(new ThreadStart(GeneralWebSpider));
                //t.IsBackground = true;
                //t.Start();
                MediaWebSpider();
            }
        }
        private void MediaWebSpiderTimer_Elapsed(object sender, EventArgs e)
        {
            lock (thisLockMedia)
            {
                if (MediaTimerRunning) return;
                MediaTimerRunning = true;
            }

            try
            {
                BeginMediaEvn();
            }
            finally
            {
                lock (thisLockMedia)
                {
                    MediaTimerRunning = false;
                }
            }
        }
        #endregion

        #region 微信抓取程序
        private void BeginWeixinEvn()
        {
            if (!Program.ProClose)
            {
                //Thread t = new Thread(new ThreadStart(GeneralWebSpider));
                //t.IsBackground = true;
                //t.Start();
                WeixinWebSpider();
            }
        }
        private void WeixinWebSpiderTimer_Elapsed(object sender, EventArgs e)
        {
            lock (thisLockWeixin)
            {
                if (WeixinTimerRunning) return;
                WeixinTimerRunning = true;
            }

            try
            {
                BeginWeixinEvn();
            }
            finally
            {
                lock (thisLockWeixin)
                {
                    WeixinTimerRunning = false;
                }
            }
        }
        #endregion

        #region 博客抓取程序
        private void BeginBlogEvn()
        {
            if (!Program.ProClose)
            {
                //Thread t = new Thread(new ThreadStart(GeneralWebSpider));
                //t.IsBackground = true;
                //t.Start();
                BlogWebSpider();
            }
        }
        private void BlogWebSpiderTimer_Elapsed(object sender, EventArgs e)
        {
            lock (thisLockBlog)
            {
                if (BlogTimerRunning) return;
                BlogTimerRunning = true;
            }

            try
            {
                BeginBlogEvn();
            }
            finally
            {
                lock (thisLockBlog)
                {
                    BlogTimerRunning = false;
                }
            }
        }
        #endregion

        #region 论坛抓取程序
        private void BeginBBSEvn()
        {
            if (!Program.ProClose)
            {
                //Thread t = new Thread(new ThreadStart(GeneralWebSpider));
                //t.IsBackground = true;
                //t.Start();
                BBSWebSpider();
            }
        }
        private void BBSWebSpiderTimer_Elapsed(object sender, EventArgs e)
        {
            lock (thisLockBBS)
            {
                if (BBSTimerRunning) return;
                BBSTimerRunning = true;
            }

            try
            {
                BeginBBSEvn();
            }
            finally
            {
                lock (thisLockBBS)
                {
                    BBSTimerRunning = false;
                }
            }
        }
        #endregion

        #region 贴吧抓取程序
        private void BeginTiebaEvn()
        {
            if (!Program.ProClose)
            {
                //Thread t = new Thread(new ThreadStart(GeneralWebSpider));
                //t.IsBackground = true;
                //t.Start();
                TiebaWebSpider();
            }
        }
        private void TiebaWebSpiderTimer_Elapsed(object sender, EventArgs e)
        {
            lock (thisLockTieba)
            {
                if (TiebaTimerRunning) return;
                TiebaTimerRunning = true;
            }

            try
            {
                BeginTiebaEvn();
            }
            finally
            {
                lock (thisLockTieba)
                {
                    TiebaTimerRunning = false;
                }
            }
        }
        #endregion

        #region 新浪微博抓取程序
        private void BeginWeiboEvn()
        {
            if (!Program.ProClose)
            {
                //Thread t = new Thread(new ThreadStart(GeneralWebSpider));
                //t.IsBackground = true;
                //t.Start();
                WeiboWebSpider();
            }
        }
        private void WeiboWebSpiderTimer_Elapsed(object sender, EventArgs e)
        {
            lock (thisLockWeibo)
            {
                if (WeiboTimerRunning) return;
                WeiboTimerRunning = true;
            }

            try
            {
                BeginWeiboEvn();
            }
            finally
            {
                lock (thisLockWeibo)
                {
                    WeiboTimerRunning = false;
                }
            }
        }
        #endregion

        #region 百度网页搜索抓取程序
        private void BeginBaiduWebEvn()
        {
            if (!Program.ProClose)
            {
                //Thread t = new Thread(new ThreadStart(GeneralWebSpider));
                //t.IsBackground = true;
                //t.Start();
                BaiduWebWebSpider();
            }
        }
        private void BaiduWebWebSpiderTimer_Elapsed(object sender, EventArgs e)
        {
            lock (thisLockBaiduWeb)
            {
                if (BaiduWebTimerRunning) return;
                BaiduWebTimerRunning = true;
            }

            try
            {
                BeginBaiduWebEvn();
            }
            finally
            {
                lock (thisLockBaiduWeb)
                {
                    BaiduWebTimerRunning = false;
                }
            }
        }
        #endregion

        #region 刷新界面程序
        private void BeginRefreshEvn()
        {
            if (!Program.ProClose)
            {
                RefreshDataGridView(pid);
            }
        }
        private void RefreshDataTimer_Elapsed(object sender, EventArgs e)
        {
            lock (thisLockRefresh)
            {
                if (RefreshTimerRunning) return;
                RefreshTimerRunning = true;
            }

            try
            {
                BeginRefreshEvn();
            }
            finally
            {
                lock (thisLockRefresh)
                {
                    RefreshTimerRunning = false;
                }
            }
        }
        #endregion

        #endregion
        #endregion
        #region 旧程序
        #region 从Queue中取出url，然后抓取数据。主要用于抓取无相似链接的网页
        private void GetNonLikeUrlInfo(Object obj)
        {
            DataTable dtkey = cmd.GetTabel("select * from Keywords");
            UrlInfo urlInfo = urlQueue.Dequeue();
            if (urlInfo == null)
            {
                DataTable queue = cmd.GetTabel("select url,title,pid,sheng,shi,xian,webname from Queue limit 0,200");
                foreach (DataRow dr in queue.Rows)
                {
                    UrlInfo ui = new UrlInfo();
                    ui.Pid = dr[2].ToString();
                    ui.Title = dr[1].ToString();
                    ui.Url = dr[0].ToString();
                    ui.Sheng = dr[3].ToString();
                    ui.Shi = dr[4].ToString();
                    ui.Xian = dr[5].ToString();
                    ui.WebName = dr[6].ToString();
                    urlQueue.Enqueue(ui);
                }
                urlInfo = urlQueue.Dequeue();
            }
            if (urlInfo != null)
            {
                string url = urlInfo.Url;
                string title = urlInfo.Title;
                string pid = urlInfo.Pid;
                string sheng = urlInfo.Sheng;
                string shi = urlInfo.Shi;
                string xian = urlInfo.Xian;
                string webname = urlInfo.WebName;
                string webInfo = null;
                if (!CrawlHtml.UrlExist(url))
                {
                    //因为有的网站会出现访问过快的话，会屏蔽访问者，所以在此让线程停止2秒钟。这样的话，会出现总体访问时间过长的问题
                    Thread.Sleep(2000);
                    //得到此链接的源码
                    webInfo = HtmlUtil.getHtml(url, "");
                    if (webInfo.Length != 0)
                    {
                        ModelReleaseInfo newsInfo = CrawlHtml.CrawlHtmlSource(title, url, dtkey, sheng, shi, xian, webname, webInfo, 4);
                        if (newsInfo.KeyWords != null && newsInfo.KeyWords.Length != 0)
                        {
                            cmd.ExecuteNonQuery("insert into urls(url,title) values('" + newsInfo.InfoSource + "','" + newsInfo.Title + "')");
                            TbReleaseInfo ri = new TbReleaseInfo();
                            cmd.ExecuteNonQuery(ri.GetInsString(newsInfo));
                        }
                    }
                    Dictionary<string, Object> dic = new Dictionary<string, object>();
                    dic.Add("websource", webInfo);
                    dic.Add("urlInfo", urlInfo);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(AddUrlToQueue), dic);
                }
            }
        }
        private void AddUrlToQueue(Object obj)
        {
            lock (obj)
            {
                if (obj != null)
                {
                    Dictionary<string, Object> dic = (Dictionary<string, Object>)obj;
                    String webInfo = dic["websource"].ToString();
                    UrlInfo urlInfo = (UrlInfo)dic["urlInfo"];
                    if (webInfo != null)
                    {
                        string[] strA = HtmlUtil.GetElementsByTagName(webInfo, "a");
                        StringBuilder sql = new StringBuilder();
                        foreach (string url in strA)
                        {
                            string title = HtmlUtil.NoHTML(url);
                            string turl = CrawlHtml.processUrl(urlInfo.Url, url);
                            if (sql.Length > 200)
                            {
                                cmd.ExecuteNonQuery(sql.ToString());
                            }
                            else
                            {
                                sql.Append("insert into queue(title,url,pid,sheng,shi,xian,webname) values('" + title + "','" + turl + "','" + urlInfo.Pid + "','" + urlInfo.Sheng + "','" + urlInfo.Shi + "','" + urlInfo.Xian + "','" + urlInfo.WebName + "')");
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region 新浪微博搜索
        private void GetSinaWeiBoInfo(Object obj)
        {
            DataTable dtkey = cmd.GetTabel("select * from Keywords");
            DataTable dtParts = cmd.GetTabel("SELECT * FROM partword");
            foreach (DataRow dr in dtkey.Rows)
            {
                String encodeKey = CrawlHtml.UrlEncode(dr["KeyWord"].ToString().Trim().ToUpper());
                sinaWeiBoWebBrowser.Url = new Uri("http://s.weibo.com/weibo/" + encodeKey + "?topnav=1&wvr=6&b=1&page=1");
                sinaWeiBoWebBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(sinaWeiBoWebBrowser_DocumentCompleted);
            }
        }
        private void sinaWeiBoWebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string html = sinaWeiBoWebBrowser.Document.Body.InnerHtml;
            string[] contents = HtmlUtil.GetElementsByClass(html, "WB_cardwrap S_bg2 clearfix");
            foreach (string content in contents)
            {

            }
        }
        #endregion

        #region 腾讯微博搜索
        private void GetQQWeiBoInfo(Object obj)
        {

        }
        #endregion

        #region 微信搜索
        private void GetWeiXinInfo(Object obj)
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
                string url = "http://weixin.sogou.com/weixin?type=2&query=" + keys + "&ie=utf8";

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
                        //mri.Part = GetParts(mri.Contexts);
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
                        dr[5] = mri.KeyWords.Substring(0, mri.KeyWords.IndexOf("-"));
                        dr[6] = mri.ReleaseName;
                        dr[7] = mri.CollectDate;
                        dr[8] = mri.Snapshot;
                        dr[9] = mri.WebName;
                        dr[10] = mri.Pid;
                        dr[11] = mri.Part;
                        dr[12] = mri.Reposts;
                        dr[13] = mri.Comments;

                        dtweixininfo.Rows.InsertAt(dr, 0);

                        if (dtbaiduwebinfo.Rows.Count >= 500)
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
        private void GetBaiduInfo(Object obj)
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
            foreach (KeyValuePair<string, string> kv in keywords)
            {
                string k = kv.Key;
                string v = kv.Value.Substring(1);
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    lbAll.Text = "正在搜索事件为<" + k.Substring(0, k.IndexOf("-")) + ">的数据.";
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
                        mri.InfoSource = HtmlUtil.GetListByHtml("", HtmlUtil.GetElementsByTagName(lis[i], "a")[0], aa)[0];

                        //去掉重复
                        if (isThere)
                        {
                            continue;
                        }
                        else
                        {
                            if (UrlThereare(mri.Title, this.dtbaiduwebinfo, dtWebQueryInfo, false) != 0)
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
                        //mri.Part = GetParts(mri.Contexts);
                        mri.Comments = 0;
                        mri.Reposts = 0;

                        DataRow dr = dtbaiduwebinfo.NewRow();
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

                        dtbaiduwebinfo.Rows.InsertAt(dr, 0);

                        if (dtbaiduwebinfo.Rows.Count >= 500)
                        {
                            dtbaiduwebinfo.Rows.RemoveAt(500);
                        }
                        this.BeginInvoke(new MethodInvoker(delegate()
                        {
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
        private void GetWebTieBaInfo(object obj)
        {
            this.BeginInvoke(new MethodInvoker(delegate()
            {
                lbtieba.Text = "";
                lbtieba.Visible = true;
            }));

            DataBaseServer.SQLitecommand cmd = new SQLitecommand();
            //得到关键字列表
            DataTable dtkey = cmd.GetTabel("select * from Keywords");
            DataTable dtParts = cmd.GetTabel("SELECT * FROM partword");
            //得到相似表
            DataTable dtXs = cmd.GetTabel("Select * from WebAddress WHERE pid=5");

            //相似表中的被抓取网址
            string webInfo = "";
            //相似链接
            string Similar = "";
            //要过滤链接中首页的正则
            string strTopFormat = "https?://.+/";
            string filterStr = "";
            List<string> strTop = new List<string>();

            sb = new StringBuilder();
            sb.Append("");

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
                string[] strA = HtmlUtil.GetElementsByTagName(webInfo, "a");
                //List<string> strList = HtmlUtil.GetElementsByTagNameList(webInfo, "a");

                TbReleaseInfo ri = new TbReleaseInfo();

                //string[] strA = GetLIstDate(strList.Distinct());

                #region 逐个链接判断
                for (int i = 0; i < strA.Length; i++)
                {
                    if (Program.ProClose == true) break;
                    Application.DoEvents();

                    string url = CrawlHtml.processUrl(dtXs.Rows[xs]["url"].ToString(), strA[i]);

                    //得到相似值,大于0.70的认为相同,并开始抓取
                    if (HtmlUtil.getSimilarDegree(Similar, url) >= 0.70)
                    {
                        if (CrawlHtml.UrlExist(url))
                        {
                            continue;
                        }
                        //因为有的网站会出现访问过快的话，会屏蔽访问者，所以在此让线程停止2秒钟。这样的话，会出现总体访问时间过长的问题
                        Thread.Sleep(2000);
                        //得到此链接的源码
                        webInfo = HtmlUtil.getHtml(url, "");
                        if (webInfo.Length == 0) { continue; }

                        ModelReleaseInfo newsInfo = CrawlHtml.CrawlHtmlSource(strA[i], url, dtkey, dtXs.Rows[xs]["sheng"].ToString(), dtXs.Rows[xs]["shi"].ToString(), dtXs.Rows[xs]["xian"].ToString(), dtXs.Rows[xs]["Name"].ToString(), webInfo, 5);
                        if (newsInfo.KeyWords == null || newsInfo.KeyWords.Length == 0) { continue; }
                        newsInfo.Uid = dvAll.Rows.Count + 1;

                        //新建数据行
                        DataRow dr = dttiebainfo.NewRow();
                        if (dvAll.RowCount == 0)
                        {
                            dr[0] = 1;
                        }
                        else
                        {
                            dr[0] = int.Parse(dvAll.Rows[dvAll.RowCount - 1].Cells[0].Value.ToString()) + 1;
                        }
                        dr[0] = newsInfo.Uid;
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
                            dvAll.Refresh();
                        }));

                        //得到插入语句
                        try
                        {
                            cmd.ExecuteNonQuery("insert into urls(url,title) values('" + newsInfo.InfoSource + "','" + newsInfo.Title + "')");
                            sb.Append(ri.GetInsString(newsInfo) + ";");

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
            AutoResetEvent are = (AutoResetEvent)obj;
            are.Set();
        }
        #endregion

        #region 得到网站的新闻类数据
        /// <summary>
        /// 得到网站的新闻类数据
        /// </summary>
        private void GetWebNewsInfo()//object obj)
        {
            this.BeginInvoke(new MethodInvoker(delegate()
            {
                lbWeb.Text = "";
                lbWeb.Visible = true;
            }));

            DataBaseServer.SQLitecommand cmd = new SQLitecommand();
            //得到关键字列表
            DataTable dtkey = cmd.GetTabel("select * from Keywords");
            DataTable dtParts = cmd.GetTabel("SELECT * FROM partword");
            //得到相似表
            DataTable dtXs = cmd.GetTabel("Select * from WebAddress WHERE pid=4");

            //相似表中的被抓取网址
            string webInfo = "";
            //相似链接
            string Similar = "";
            //要过滤链接中首页的正则
            string strTopFormat = "https?://.+/";
            string filterStr = "";
            List<string> strTop = new List<string>();

            sb = new StringBuilder();
            sb.Append("");

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
                string[] strA = HtmlUtil.GetElementsByTagName(webInfo, "a");

                TbReleaseInfo ri = new TbReleaseInfo();

                #region 逐个链接判断
                for (int i = 0; i < strA.Length; i++)
                {
                    if (Program.ProClose == true) break;
                    Application.DoEvents();

                    string url = CrawlHtml.processUrl(dtXs.Rows[xs]["url"].ToString(), strA[i]);

                    //得到相似值,大于0.70的认为相同,并开始抓取
                    if (HtmlUtil.getSimilarDegree(Similar, url) >= 0.70)
                    {
                        if (CrawlHtml.UrlExist(url))
                        {
                            continue;
                        }
                        //因为有的网站会出现访问过快的话，会屏蔽访问者，所以在此让线程停止2秒钟。这样的话，会出现总体访问时间过长的问题
                        Thread.Sleep(2000);
                        //得到此链接的源码
                        webInfo = HtmlUtil.getHtml(url, "");
                        if (webInfo.Length == 0) { continue; }

                        ModelReleaseInfo newsInfo = CrawlHtml.CrawlHtmlSource(strA[i], url, dtkey, dtXs.Rows[xs]["sheng"].ToString(), dtXs.Rows[xs]["shi"].ToString(), dtXs.Rows[xs]["xian"].ToString(), dtXs.Rows[xs]["Name"].ToString(), webInfo, 4);
                        if (newsInfo.KeyWords == null || newsInfo.KeyWords.Length == 0) { continue; }
                        newsInfo.Uid = dvWeb.Rows.Count + 1;

                        //新建数据行
                        DataRow dr = dtbaidunewsinfo.NewRow();
                        if (dvWeb.RowCount == 0)
                        {
                            dr[0] = 1;
                        }
                        else
                        {
                            dr[0] = int.Parse(dvWeb.Rows[dvWeb.RowCount - 1].Cells[0].Value.ToString()) + 1;
                        }
                        dr[0] = newsInfo.Uid;
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
                        dtbaidunewsinfo.Rows.InsertAt(dr, 0);

                        //数据源刷新
                        if (dtbaidunewsinfo.Rows.Count >= 500)
                        {
                            dtbaidunewsinfo.Rows.RemoveAt(500);
                        }
                        this.BeginInvoke(new MethodInvoker(delegate()
                        {
                            dvWeb.Refresh();
                        }));

                        //得到插入语句
                        try
                        {
                            cmd.ExecuteNonQuery("insert into urls(url,title) values('" + newsInfo.InfoSource + "','" + newsInfo.Title + "')");
                            sb.Append(ri.GetInsString(newsInfo) + ";");

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
            dtWebNewsInfo = tri.SelReleaseInfo(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd HH:mm:ss"), "0 AND webName<>'百度'");
            this.BeginInvoke(new MethodInvoker(delegate()
            {
                lbWeb.Text = "一轮搜索完毕！";
                lbWeb.ForeColor = Color.Red;
            }));
            //AutoResetEvent are = (AutoResetEvent)obj;
            //are.Set();
        }
        #endregion

        #region 得到网站的博客类数据
        /// <summary>
        /// 得到网站的博客类数据
        /// </summary>
        private void GetWebBlogInfo(object obj)
        {
            this.BeginInvoke(new MethodInvoker(delegate()
            {
                lbBlog.Text = "";
                lbBlog.Visible = true;
            }));

            DataBaseServer.SQLitecommand cmd = new SQLitecommand();
            //得到关键字列表
            DataTable dtkey = cmd.GetTabel("select * from Keywords");
            DataTable dtParts = cmd.GetTabel("SELECT * FROM partword");
            //得到相似表
            DataTable dtXs = cmd.GetTabel("Select * from WebAddress WHERE pid=1");

            //相似表中的被抓取网址
            string webInfo = "";
            //相似链接
            string Similar = "";
            //要过滤链接中首页的正则
            string strTopFormat = "https?://.+/";
            string filterStr = "";
            List<string> strTop = new List<string>();

            sb = new StringBuilder();
            sb.Append("");

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
                string[] strA = HtmlUtil.GetElementsByTagName(webInfo, "a");

                TbReleaseInfo ri = new TbReleaseInfo();

                #region 逐个链接判断
                for (int i = 0; i < strA.Length; i++)
                {
                    if (Program.ProClose == true) break;
                    Application.DoEvents();

                    string url = CrawlHtml.processUrl(dtXs.Rows[xs]["url"].ToString(), strA[i]);

                    //得到相似值,大于0.70的认为相同,并开始抓取
                    if (HtmlUtil.getSimilarDegree(Similar, url) >= 0.70)
                    {
                        if (CrawlHtml.UrlExist(url))
                        {
                            continue;
                        }
                        //因为有的网站会出现访问过快的话，会屏蔽访问者，所以在此让线程停止2秒钟。这样的话，会出现总体访问时间过长的问题
                        Thread.Sleep(2000);
                        //得到此链接的源码
                        webInfo = HtmlUtil.getHtml(url, "");
                        if (webInfo.Length == 0) { continue; }

                        ModelReleaseInfo newsInfo = CrawlHtml.CrawlHtmlSource(strA[i], url, dtkey, dtXs.Rows[xs]["sheng"].ToString(), dtXs.Rows[xs]["shi"].ToString(), dtXs.Rows[xs]["xian"].ToString(), dtXs.Rows[xs]["Name"].ToString(), webInfo, 4);
                        if (newsInfo.KeyWords == null || newsInfo.KeyWords.Length == 0) { continue; }
                        newsInfo.Uid = dvBlog.Rows.Count + 1;

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
                        dr[0] = newsInfo.Uid;
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
                        dtbloginfo.Rows.InsertAt(dr, 0);

                        //数据源刷新
                        if (dtbloginfo.Rows.Count >= 500)
                        {
                            dtbloginfo.Rows.RemoveAt(500);
                        }
                        this.BeginInvoke(new MethodInvoker(delegate()
                        {
                            dvBlog.Refresh();
                        }));

                        //得到插入语句
                        try
                        {
                            cmd.ExecuteNonQuery("insert into urls(url,title) values('" + newsInfo.InfoSource + "','" + newsInfo.Title + "')");
                            sb.Append(ri.GetInsString(newsInfo) + ";");

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
            AutoResetEvent are = (AutoResetEvent)obj;
            are.Set();
        }
        #endregion

        #region 得到网站的论坛类数据
        /// <summary>
        /// 得到网站的论坛类数据
        /// </summary>
        private void GetWebBBSInfo(object obj)
        {
            this.BeginInvoke(new MethodInvoker(delegate()
            {
                lbBBs.Text = "";
                lbBBs.Visible = true;
            }));

            DataBaseServer.SQLitecommand cmd = new SQLitecommand();
            //得到关键字列表
            DataTable dtkey = cmd.GetTabel("select * from Keywords");
            DataTable dtParts = cmd.GetTabel("SELECT * FROM partword");
            //得到相似表
            DataTable dtXs = cmd.GetTabel("Select * from WebAddress WHERE pid=2");

            //相似表中的被抓取网址
            string webInfo = "";
            //相似链接
            string Similar = "";
            //要过滤链接中首页的正则
            string strTopFormat = "https?://.+/";
            string filterStr = "";
            List<string> strTop = new List<string>();

            sb = new StringBuilder();
            sb.Append("");

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
                string[] strA = HtmlUtil.GetElementsByTagName(webInfo, "a");
                TbReleaseInfo ri = new TbReleaseInfo();

                #region 逐个链接判断
                for (int i = 0; i < strA.Length; i++)
                {
                    if (Program.ProClose == true) break;
                    Application.DoEvents();

                    string url = CrawlHtml.processUrl(dtXs.Rows[xs]["url"].ToString(), strA[i]);

                    //得到相似值,大于0.70的认为相同,并开始抓取
                    if (HtmlUtil.getSimilarDegree(Similar, url) >= 0.70)
                    {
                        if (CrawlHtml.UrlExist(url))
                        {
                            continue;
                        }
                        //因为有的网站会出现访问过快的话，会屏蔽访问者，所以在此让线程停止2秒钟。这样的话，会出现总体访问时间过长的问题
                        Thread.Sleep(2000);
                        //得到此链接的源码
                        webInfo = HtmlUtil.getHtml(url, "");
                        if (webInfo.Length == 0) { continue; }

                        ModelReleaseInfo newsInfo = CrawlHtml.CrawlHtmlSource(strA[i], url, dtkey, dtXs.Rows[xs]["sheng"].ToString(), dtXs.Rows[xs]["shi"].ToString(), dtXs.Rows[xs]["xian"].ToString(), dtXs.Rows[xs]["Name"].ToString(), webInfo, 4);
                        if (newsInfo.KeyWords == null || newsInfo.KeyWords.Length == 0) { continue; }
                        newsInfo.Uid = dvBBs.Rows.Count + 1;

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
                        dr[0] = newsInfo.Uid;
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
                        this.BeginInvoke(new MethodInvoker(delegate()
                        {
                            dvBBs.Refresh();
                        }));

                        //得到插入语句
                        try
                        {
                            cmd.ExecuteNonQuery("insert into urls(url,title) values('" + newsInfo.InfoSource + "','" + newsInfo.Title + "')");
                            sb.Append(ri.GetInsString(newsInfo) + ";");

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
            AutoResetEvent are = (AutoResetEvent)obj;
            are.Set();
        }
        #endregion
        #endregion
        #region 2015.3 wangcg 添加 抓取解析程序
        #region 通用网站抓取程序
        /// <summary>
        /// 通用网站抓取程序
        /// </summary>
        private void GeneralWebSpider()
        {
            //this.BeginInvoke(new MethodInvoker(delegate()
            //{
            //    this.lbCustom.Text = "";
            //    lbCustom.Visible = true;
            //}));

            DataBaseServer.SQLitecommand cmd = new SQLitecommand();
            //得到关键字列表
            DataTable dtkey = cmd.GetTabel("select * from Keywords");
            //得到抓取网站的地址
            DataTable dtXs = cmd.GetTabel("Select * from WebAddress");

            sb = new StringBuilder();
            sb.Append("");

            #region 读取相似度表中的数据据,循环抓取
            for (int xs = 0; xs < dtXs.Rows.Count; xs++)
            {
                string url = dtXs.Rows[xs]["url"].ToString();
                string sheng = dtXs.Rows[xs]["sheng"].ToString();
                string shi = dtXs.Rows[xs]["shi"].ToString();
                string xian = dtXs.Rows[xs]["xian"].ToString();
                string name = dtXs.Rows[xs]["Name"].ToString();
                //读取相似链接
                string Similar = dtXs.Rows[xs]["likeurl"].ToString();

                //this.BeginInvoke(new MethodInvoker(delegate()
                //{
                //    lbCustom.Text = "正在搜索网址为<" + url + ">的数据.";
                //    lbCustom.ForeColor = Color.DarkBlue;
                //}));

                bool isCatchSubWeb = false;
                BasicWebSpider(url, Similar, dtkey, sheng, shi, xian, name, isCatchSubWeb);

                //防止拉黑
                Thread.Sleep(2000);
            }

            //this.BeginInvoke(new MethodInvoker(delegate()
            //{
            //    lbCustom.Text = "一轮搜索完毕！";
            //    lbCustom.ForeColor = Color.Red;
            //}));

            #endregion
        }

        private void BasicWebSpider(string motherUrl, string similar, DataTable keys, string sheng, string shi, string xian, string name, bool catchSub)
        {
            DataBaseServer.SQLitecommand cmd = new SQLitecommand();
            //相似表中的被抓取网址
            string webInfo = "";

            HtmlParse parse = new HtmlParse();
            sb = new StringBuilder();
            sb.Append("");

            //读取相似表中要抓取的网址
            webInfo = HtmlUtil.getHtml(motherUrl, "");

            //取出所有的超链
            string[] strA = HtmlUtil.GetElementsByTagName(webInfo, "a");

            //TbReleaseInfo ri = new TbReleaseInfo();
            List<ModelReleaseInfo> mris = new List<ModelReleaseInfo>();
            #region 逐个链接判断
            for (int i = 0; i < strA.Length; i++)
            {
                if (Program.ProClose == true) break;
                Application.DoEvents();

                string url = CrawlHtml.processUrl(motherUrl, strA[i]);
                if (string.IsNullOrEmpty(similar) || (!string.IsNullOrEmpty(similar) && HtmlUtil.getSimilarDegree(similar, url) >= 0.70))
                {
                    //因为有的网站会出现访问过快的话，会屏蔽访问者，所以在此让线程停止2秒钟。这样的话，会出现总体访问时间过长的问题
                    Thread.Sleep(2000);
                    //得到此链接的源码
                    webInfo = HtmlUtil.getHtml(url, "");
                    if (string.IsNullOrEmpty(webInfo)) { continue; }

                    //处理下级页面的超链
                    if (catchSub)
                    {
                        BasicWebSpider(url, similar, keys, sheng, shi, xian, name, false);
                    }

                    //判断该链接是否包含关键词
                    List<ModelReleaseInfo> newsInfos = parse.ParseGeneralWeb(strA[i], url, keys, sheng, shi, xian, Name, webInfo, 7);
                    if (newsInfos != null && newsInfos.Count > 0)
                    {
                        //写入数据库
                        DataPersistenceControl.GetInstance().Add(newsInfos);

                        //刷新界面
                        //RefreshDataGridView(7);
                    }


                }
            }
            #endregion

        }

        #endregion

        #region 刷新界面
        private int pid = -1;
        public void RefreshDataGridView(int pid)
        {
            lock (thisLockSwitch)
            {
                if (!backgroundWorker1.IsBusy)
                {
                    if (pid == -1) return;
                    backgroundWorker1.RunWorkerAsync(pid);                    
                }
            }
        }

        private void FormatDataView(DoubleBufferDataGridView dvg)
        {
            dvg.Columns.Add(new DataGridViewImageColumn() { HeaderText = "正负预判", Name = "part_img", DisplayIndex = 19 });
            dvg.Columns.Add(new DataGridViewLinkColumn() { HeaderText = "标题", Name = "title_link", DisplayIndex = 4, Width = 160 });
            foreach (DataGridViewColumn col in dvg.Columns)
            {
                switch (col.Name.ToLower())
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
                    case "releasedate":
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
                    default:
                        col.Visible = false;
                        break;
                    #endregion
                }
            }
        }

        #endregion

        #region 主流媒体抓取程序
        private void MediaWebSpider()
        {
            #region 处理关键字
            SQLitecommand cmd = new SQLitecommand();
            //得到关键字列表
            DataTable dtkey;
            DataTable dtParts;
            dtkey = cmd.GetTabel("select * from Keywords");
            dtParts = cmd.GetTabel("SELECT * FROM partword");
            #endregion

            HtmlParse parse = new HtmlParse();
            //parse.ReportCatchProcess += new HtmlParse.ReportCatchProcessEventHandler(parse_ReportCatchProcess);
            List<ModelReleaseInfo> webDatas = new List<ModelReleaseInfo>();
            //按关键字循环
            for (int kw = 0; kw < dtkey.Rows.Count; kw++)
            {
                //处理关键字
                string keyword = dtkey.Rows[kw]["KeyWord"].ToString().Trim();
                string keyTitle = dtkey.Rows[kw]["Name"].ToString().Trim();
                int kid = 0;
                int.TryParse(dtkey.Rows[kw]["kid"].ToString().Trim(), out kid);

                #region 百度检索
                //组成查询字串
                string url = "http://news.baidu.com/ns?rn=100&word=" + keyword;
                string html = HtmlUtil.HttpGet(url, Encoding.UTF8);
                List<ModelReleaseInfo> mris = parse.ParseBaiduNews(html, keyword, kid);
                if (mris != null && mris.Count() > 0)
                {
                    //写入数据库
                    DataPersistenceControl.GetInstance().Add(mris);
                }
                //防止拉黑
                Thread.Sleep(1000 * 38);
                #endregion

                #region bing检索
                for (int i = 0; i < 10; i++)
                {
                    //组成查询字串
                    url = "";
                    html = "";
                    mris = null;
                    url = string.Format("http://cn.bing.com/news/search?q={0}&first={1}&FORM=PENR", keyword, i * 10 + 1);
                    html = HtmlUtil.HttpGet(url, Encoding.UTF8);
                    mris = parse.ParseBingNews(html, keyword, kid);
                    if (mris != null && mris.Count() > 0)
                    {
                        //写入数据库
                        DataPersistenceControl.GetInstance().Add(mris);
                    }
                    else 
                    {
                        break;
                    }
                    //防止拉黑
                    Thread.Sleep(1000 * 50);
                }
                #endregion

                #region 搜狗新闻
                for (int i = 0; i < 10; i++)
                {
                    //组成查询字串
                    url = "";
                    html = "";
                    mris = null;
                    url = string.Format("http://news.sogou.com/news?query={0}&page={1}", keyword, i + 1);
                    html = HtmlUtil.HttpGet(url, Encoding.Default);
                    mris = parse.ParseSogouNews(html, keyword, kid);
                    if (mris != null && mris.Count() > 0)
                    {
                        //写入数据库
                        DataPersistenceControl.GetInstance().Add(mris);
                    }
                    else
                    {
                        break;
                    }
                    //防止拉黑（2分钟）
                    Thread.Sleep(1000 * 60 * 2);
                }
                #endregion

                #region 中搜新闻
                for (int i = 0; i < 10; i++)
                {
                    //组成查询字串
                    url = "";
                    html = "";
                    mris = null;
                    url = string.Format("http://zixun.zhongsou.com/n?w={0}&b={1}", keyword, i + 1);
                    html = HtmlUtil.HttpGet(url, Encoding.Default);
                    mris = parse.ParseZhongsouNews(html, keyword, kid);
                    if (mris != null && mris.Count() > 0)
                    {
                        //写入数据库
                        DataPersistenceControl.GetInstance().Add(mris);
                    }
                    else
                    {
                        break;
                    }
                    //防止拉黑（2分钟）
                    Thread.Sleep(1000 * 60 * 2);
                }
                #endregion

                #region 好搜新闻
                for (int i = 0; i < 10; i++)
                {
                    //组成查询字串
                    url = "";
                    html = "";
                    mris = null;
                    url = string.Format("http://news.haosou.com/ns?q={0}&pn={1}&tn=news&rank=rank&j=0", keyword, i + 1);
                    html = HtmlUtil.HttpGet(url, Encoding.UTF8);
                    mris = parse.ParseHaosouNews(html, keyword, kid);
                    if (mris != null && mris.Count() > 0)
                    {
                        //写入数据库
                        DataPersistenceControl.GetInstance().Add(mris);
                    }
                    else
                    {
                        break;
                    }
                    //防止拉黑
                    Thread.Sleep(1000 * 50);
                }
                #endregion
            }
        }

        public void parse_ReportCatchProcess(ModelReleaseInfo mri)
        {
        }

        #endregion

        #region 微信抓取程序
        private void WeixinWebSpider()
        {
            #region 处理关键字
            SQLitecommand cmd = new SQLitecommand();
            //得到关键字列表
            DataTable dtkey;
            DataTable dtParts;
            dtkey = cmd.GetTabel("select * from Keywords");
            dtParts = cmd.GetTabel("SELECT * FROM partword");
            #endregion

            HtmlParse parse = new HtmlParse();
            //parse.ReportCatchProcess += new HtmlParse.ReportCatchProcessEventHandler(Weixin_ReportCatchProcess);
            List<ModelReleaseInfo> webDatas = new List<ModelReleaseInfo>();
            //按关键字循环
            for (int kw = 0; kw < dtkey.Rows.Count; kw++)
            {
                //处理关键字
                string keyword = dtkey.Rows[kw]["KeyWord"].ToString().Trim();
                string keyTitle = dtkey.Rows[kw]["Name"].ToString().Trim();
                int kid = 0;
                int.TryParse(dtkey.Rows[kw]["kid"].ToString().Trim(), out kid);
                #region 按关键字检索
                //组成查询字串                
                string url = "http://weixin.sogou.com/weixin?type=2&query=" + keyword + "&ie=utf8";

                string html = HtmlUtil.HttpGet(url, Encoding.UTF8);
                #endregion
                List<ModelReleaseInfo> mris = parse.ParseSogouWeixin(html, keyword, kid);
                if (mris != null && mris.Count() > 0)
                {
                    DataPersistenceControl.GetInstance().Add(mris);
                    //webDatas.AddRange(mris);
                    //刷新界面
                    //RefreshDataGridView(6);
                }
                //防止拉黑（2分钟）
                Thread.Sleep(1000 * 60 * 2);
            }
        }

        public void Weixin_ReportCatchProcess(ModelReleaseInfo mri)
        {

        }

        #endregion

        #region 博客抓取程序
        private void BlogWebSpider()
        {
            #region 处理关键字
            SQLitecommand cmd = new SQLitecommand();
            //得到关键字列表
            DataTable dtkey;
            DataTable dtParts;
            dtkey = cmd.GetTabel("select * from Keywords");
            dtParts = cmd.GetTabel("SELECT * FROM partword");
            #endregion

            HtmlParse parse = new HtmlParse();
            //parse.ReportCatchProcess += new HtmlParse.ReportCatchProcessEventHandler(Blog_ReportCatchProcess);
            List<ModelReleaseInfo> webDatas = new List<ModelReleaseInfo>();
            //按关键字循环
            for (int kw = 0; kw < dtkey.Rows.Count; kw++)
            {
                //处理关键字
                string keyword = dtkey.Rows[kw]["KeyWord"].ToString().Trim();
                string keyTitle = dtkey.Rows[kw]["Name"].ToString().Trim();
                int kid = 0;
                int.TryParse(dtkey.Rows[kw]["kid"].ToString().Trim(), out kid);
                #region 按关键字检索
                //组成查询字串                
                string url = "http://www.sogou.com/web?interation=196647&query=" + keyword + "&ie=utf8";

                string html = HtmlUtil.HttpGet(url, Encoding.UTF8);
                #endregion
                List<ModelReleaseInfo> mris = parse.ParseSogouBlog(html, keyword, kid);
                if (mris != null && mris.Count() > 0)
                {
                    DataPersistenceControl.GetInstance().Add(mris);
                }
                //防止拉黑
                Thread.Sleep(1000 * 60 * 2);
            }
        }

        public void Blog_ReportCatchProcess(ModelReleaseInfo mri)
        {
        }

        #endregion

        #region 论坛抓取程序
        private void BBSWebSpider()
        {
            #region 处理关键字
            SQLitecommand cmd = new SQLitecommand();
            //得到关键字列表
            DataTable dtkey;
            DataTable dtParts;
            dtkey = cmd.GetTabel("select * from Keywords");
            dtParts = cmd.GetTabel("SELECT * FROM partword");
            #endregion

            HtmlParse parse = new HtmlParse();
            //parse.ReportCatchProcess += new HtmlParse.ReportCatchProcessEventHandler(BBS_ReportCatchProcess);
            List<ModelReleaseInfo> webDatas = new List<ModelReleaseInfo>();
            //按关键字循环
            for (int kw = 0; kw < dtkey.Rows.Count; kw++)
            {
                //处理关键字
                string keyword = dtkey.Rows[kw]["KeyWord"].ToString().Trim();
                string keyTitle = dtkey.Rows[kw]["Name"].ToString().Trim();
                int kid = 0;
                int.TryParse(dtkey.Rows[kw]["kid"].ToString().Trim(), out kid);

                #region 搜狗检索
                //组成查询字串                
                string url = "http://www.sogou.com/web?interation=196648&query=" + keyword + "&ie=utf8";
                string html = HtmlUtil.HttpGet(url, Encoding.UTF8);
                List<ModelReleaseInfo> mris = parse.ParseSogouBBS(html, keyword, kid);
                if (mris != null && mris.Count() > 0)
                {
                    DataPersistenceControl.GetInstance().Add(mris);
                }
                //防止拉黑（2分钟）
                Thread.Sleep(1000 * 60 * 2);
                #endregion

                #region 中搜新闻
                for (int i = 0; i < 10; i++)
                {
                    //组成查询字串
                    url = "";
                    html = "";
                    mris = null;
                    url = string.Format("http://bbs.zhongsou.com/b?w={0}&b={1}", keyword, i + 1);
                    html = HtmlUtil.HttpGet(url, Encoding.Default);
                    mris = parse.ParseZhongsouBBS(html, keyword, kid);
                    if (mris != null && mris.Count() > 0)
                    {
                        //写入数据库
                        DataPersistenceControl.GetInstance().Add(mris);
                    }
                    else
                    {
                        break;
                    }
                    //防止拉黑
                    Thread.Sleep(1000 * 60 * 2);
                }
                #endregion
            }
        }

        public void BBS_ReportCatchProcess(ModelReleaseInfo mri)
        {
        }

        #endregion

        #region 贴吧抓取程序
        private void TiebaWebSpider()
        {
            #region 处理关键字
            SQLitecommand cmd = new SQLitecommand();
            //得到关键字列表
            DataTable dtkey;
            DataTable dtParts;
            dtkey = cmd.GetTabel("select * from Keywords");
            dtParts = cmd.GetTabel("SELECT * FROM partword");
            #endregion

            HtmlParse parse = new HtmlParse();
            //parse.ReportCatchProcess += new HtmlParse.ReportCatchProcessEventHandler(Tieba_ReportCatchProcess);

            //按关键字循环
            for (int kw = 0; kw < dtkey.Rows.Count; kw++)
            {
                List<ModelReleaseInfo> webDatas = new List<ModelReleaseInfo>();
                //处理关键字
                string keyword = dtkey.Rows[kw]["KeyWord"].ToString().Trim();
                string keyTitle = dtkey.Rows[kw]["Name"].ToString().Trim();
                int kid = 0;
                int.TryParse(dtkey.Rows[kw]["kid"].ToString().Trim(), out kid);

                #region 按关键字检索
                //组成查询字串
                //返回前60条数据，暂且不处理翻页
                string url = "http://tieba.baidu.com/f/search/res?ie=utf-8&rn=60&qw=" + keyword;

                string html = HtmlUtil.HttpGet(url, Encoding.Default);
                #endregion
                List<ModelReleaseInfo> mris = parse.ParseBaiduTieba(html, keyword, kid);
                if (mris != null && mris.Count() > 0)
                {
                    DataPersistenceControl.GetInstance().Add(mris);
                }
                //防止拉黑
                Thread.Sleep(1000 * 50);
            }
        }

        public void Tieba_ReportCatchProcess(ModelReleaseInfo mri)
        {
        }

        #endregion

        #region 微博抓取程序
        private void WeiboWebSpider()
        {
            #region 处理关键字
            SQLitecommand cmd = new SQLitecommand();
            //得到关键字列表
            DataTable dtkey;
            DataTable dtParts;
            dtkey = cmd.GetTabel("select * from Keywords");
            dtParts = cmd.GetTabel("SELECT * FROM partword");
            #endregion

            HtmlParse parse = new HtmlParse();
            //parse.ReportCatchProcess += new HtmlParse.ReportCatchProcessEventHandler(Weibo_ReportCatchProcess);
            List<ModelReleaseInfo> webDatas = new List<ModelReleaseInfo>();
            //按关键字循环
            for (int kw = 0; kw < dtkey.Rows.Count; kw++)
            {
                //处理关键字
                string keyword = dtkey.Rows[kw]["KeyWord"].ToString().Trim();
                string keyTitle = dtkey.Rows[kw]["Name"].ToString().Trim();
                int kid = 0;
                int.TryParse(dtkey.Rows[kw]["kid"].ToString().Trim(), out kid);

                #region 新浪微博检索
                String encodeKey = CrawlHtml.UrlEncode(keyword);
                string url = "http://s.weibo.com/weibo/" + encodeKey + "?topnav=1&wvr=6&b=1&page=1";
                string html = HtmlUtil.HttpGet(url, Encoding.UTF8);
                List<ModelReleaseInfo> mris = parse.ParseSinaWeibo(html, keyword, kid);
                if (mris != null && mris.Count() > 0)
                {
                    DataPersistenceControl.GetInstance().Add(mris);
                }
                //防止微博拉黑
                Thread.Sleep(1000 * 40);
                #endregion

                #region 中搜检索
                for (int i = 0; i < 10; i++)
                {
                    //组成查询字串
                    url = "";
                    html = "";
                    mris = null;
                    url = string.Format("http://t.zhongsou.com/wb?w={0}&b={1}", keyword, i + 1);
                    html = HtmlUtil.HttpGet(url, Encoding.Default);
                    mris = parse.ParseZhongsouWeibo(html, keyword, kid);
                    if (mris != null && mris.Count() > 0)
                    {
                        //写入数据库
                        DataPersistenceControl.GetInstance().Add(mris);
                    }
                    else
                    {
                        break;
                    }
                    //防止拉黑
                    Thread.Sleep(1000 * 60 * 2);
                }
                #endregion
            }
        }

        public void Weibo_ReportCatchProcess(ModelReleaseInfo mri)
        {
        }

        #endregion

        #region 全网取程序(百度网页)
        private void BaiduWebWebSpider()
        {
            #region 处理关键字
            SQLitecommand cmd = new SQLitecommand();
            //得到关键字列表
            DataTable dtkey;
            DataTable dtParts;
            dtkey = cmd.GetTabel("select * from Keywords");
            dtParts = cmd.GetTabel("SELECT * FROM partword");
            #endregion

            HtmlParse parse = new HtmlParse();
            //parse.ReportCatchProcess += new HtmlParse.ReportCatchProcessEventHandler(BaiduWeb_ReportCatchProcess);
            List<ModelReleaseInfo> webDatas = new List<ModelReleaseInfo>();
            //按关键字循环
            for (int kw = 0; kw < dtkey.Rows.Count; kw++)
            {
                //处理关键字
                string keyword = dtkey.Rows[kw]["KeyWord"].ToString().Trim();
                string keyTitle = dtkey.Rows[kw]["Name"].ToString().Trim();
                int kid = 0;
                int.TryParse(dtkey.Rows[kw]["kid"].ToString().Trim(), out kid);

                #region 百度检索
                for (int i = 0; i < 5; i++)
                {
                    String encodeKey = CrawlHtml.UrlEncode(keyword);
                    string url = string.Format(@"http://www.baidu.com/s?wd={0}&pn={1}&ie=utf-8", encodeKey, i * 10);
                    string html = HtmlUtil.HttpGet(url, Encoding.UTF8);
                    List<ModelReleaseInfo> mris = parse.ParseBaiduWeb(html, keyword, kid);
                    if (mris != null && mris.Count() > 0)
                    {
                        DataPersistenceControl.GetInstance().Add(mris);
                    }
                    else
                    {
                        break;
                    }
                    //防止拉黑
                    Thread.Sleep(1000 * 50);
                }
                #endregion

                #region bing检索
                for (int i = 0; i < 10; i++)
                {
                    //组成查询字串
                    string url = string.Format("http://cn.bing.com/search?q={0}&first={1}&FORM=PERE", keyword, i * 10 + 1);
                    string html = HtmlUtil.HttpGet(url, Encoding.UTF8);
                    List<ModelReleaseInfo> mris = parse.ParseBingWeb(html, keyword, kid);
                    if (mris != null && mris.Count() > 0)
                    {
                        //写入数据库
                        DataPersistenceControl.GetInstance().Add(mris);
                    }
                    else
                    {
                        break;
                    }
                    //防止拉黑
                    Thread.Sleep(1000 * 45);
                }
                #endregion

                #region 搜狗检索
                for (int i = 0; i < 10; i++)
                {
                    //组成查询字串
                    string url = string.Format("http://www.sogou.com/web?query={0}&page={1}&ie=utf8", keyword, i + 1);
                    string html = HtmlUtil.HttpGet(url, Encoding.UTF8);
                    List<ModelReleaseInfo> mris = parse.ParseSogouWeb(html, keyword, kid);
                    if (mris != null && mris.Count() > 0)
                    {
                        //写入数据库
                        DataPersistenceControl.GetInstance().Add(mris);
                    }
                    else
                    {
                        break;
                    }
                    //防止拉黑
                    Thread.Sleep(1000 * 60 * 2);
                }
                #endregion

                #region 中搜检索
                for (int i = 0; i < 10; i++)
                {
                    //组成查询字串
                    string url = string.Format("http://www.zhongsou.com/third?w={0}&b={1}", keyword, i + 1);
                    string html = HtmlUtil.HttpGet(url, Encoding.Default);
                    List<ModelReleaseInfo> mris = parse.ParseZhongsouWeb(html, keyword, kid);
                    if (mris != null && mris.Count() > 0)
                    {
                        //写入数据库
                        DataPersistenceControl.GetInstance().Add(mris);
                    }
                    else
                    {
                        break;
                    }
                    //防止拉黑
                    Thread.Sleep(1000 * 60 * 2);
                }
                #endregion

                #region 好搜检索
                for (int i = 0; i < 10; i++)
                {
                    //组成查询字串
                    string url = string.Format("http://www.haosou.com/s?q={0}&pn={1}", keyword, i + 1);
                    string html = HtmlUtil.HttpGet(url, Encoding.UTF8);
                    List<ModelReleaseInfo> mris = parse.ParseHaosouWeb(html, keyword, kid);
                    if (mris != null && mris.Count() > 0)
                    {
                        //写入数据库
                        DataPersistenceControl.GetInstance().Add(mris);
                    }
                    else
                    {
                        break;
                    }
                    //防止拉黑
                    Thread.Sleep(1000 * 60);
                }
                #endregion

            }
        }

        public void BaiduWeb_ReportCatchProcess(ModelReleaseInfo mri)
        {
        }

        #endregion

        #endregion

        #region 控件事件

        #region 表格点击事件，点击标题时打开浏览器
        private void dvGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!(sender is DoubleBufferDataGridView)) return;
            DoubleBufferDataGridView dgv = sender as DoubleBufferDataGridView;
            if (dgv.SelectedRows.Count != 0 && dgv.Columns[e.ColumnIndex].Name == "title_link")
            {
                System.Diagnostics.Process.Start(dgv.Rows[e.RowIndex].Cells["infosource"].Value.ToString());
            }
        }
        #endregion

        #region 表格选中事件，获取选中行的标题，关键字，链接，内容，拼合到下面的dataView里
        private void dvGridView_SelectionChanged(object sender, EventArgs e)
        {
            lock (thisLockSwitch)
            {
                try
                {
                    this.BeginInvoke(new MethodInvoker(delegate()
                    {
                        dataView.Clear();
                        if (!(sender is DoubleBufferDataGridView)) return;
                        DoubleBufferDataGridView dgv = sender as DoubleBufferDataGridView;

                        if (dgv.SelectedRows.Count == 0) return;
                        if (dgv.CurrentCell != null)
                        {
                            if (dgv.CurrentCell == null) return;

                            string title = dgv.CurrentCell.OwningRow.Cells["title"].Value == null ? "" : dgv.CurrentCell.OwningRow.Cells["title"].Value.ToString();

                            //设置textbox内容为标题加链接
                            dataView.Text = "标题：" + title + "\n链接：" + (dgv.CurrentCell.OwningRow.Cells["infosource"].Value == null ? "" : dgv.CurrentCell.OwningRow.Cells["infosource"].Value.ToString()) + "\n";

                            //设置标题粗体
                            dataView.Select(3, title.Length);
                            dataView.SelectionFont = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold);
                            //添加内容
                            dataView.AppendText("    " + (dgv.CurrentCell.OwningRow.Cells["contexts"].Value == null ? "" : dgv.CurrentCell.OwningRow.Cells["contexts"].Value.ToString()));
                            //关键字
                            string keyword = dgv.CurrentCell.OwningRow.Cells["keywords"].Value.ToString();
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

                            ////获取当前textbox内容长度，+4是表示内容文本空两个空格
                            //int length = dataView.Text.Length + 4;
                            ////得到关键字列表
                            //DataTable dtkey;
                            //DataTable dtParts;
                            //dtkey = cmd.GetTabel("select * from Keywords");
                            //dtParts = cmd.GetTabel("SELECT * FROM partword");
                            ////添加内容                            
                            //dataView.AppendText("    " + (dgv.CurrentCell.OwningRow.Cells["contexts"].Value == null ? "" : dgv.CurrentCell.OwningRow.Cells["contexts"].Value.ToString()));
                            //StringBuilder keySb = new StringBuilder();
                            //for (int i = 0; i < dtkey.Rows.Count; i++)
                            //{
                            //    keySb.Append("," + dtkey.Rows[i]["KeyWord"].ToString());
                            //}
                            //if (keySb.Length > 0)
                            //{
                            //    keySb = keySb.Remove(0, 1);
                            //    //分割关键字
                            //    string[] keywords = keySb.ToString().Trim().Split(',');
                            //    foreach (string kw in keywords)
                            //    {
                            //        string[] keywords1 = kw.Split(' ');
                            //        foreach (string kw1 in keywords1)
                            //        {
                            //            int wl = kw1.Length;
                            //            int start = 0;
                            //            while (start < dataView.Text.Length && dataView.Text.IndexOf(kw1, start) > -1)
                            //            {
                            //                int kl = dataView.Text.IndexOf(kw1, start);
                            //                if (kl >= 0)
                            //                {
                            //                    dataView.Select(kl, wl);
                            //                    dataView.SelectionFont = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold);
                            //                    dataView.SelectionColor = Color.Red;
                            //                    start += kl + wl;
                            //                }
                            //            }
                            //        }
                            //    }
                            //}
                            //dataView.Select(0, 0);
                        }
                    }));
                }
                catch (Exception ex)
                {
                    Comm.WriteErrorLog(ex.Message);
                    Comm.WriteErrorLog(ex.StackTrace);
                }
            }
        }
        #endregion

        #region 表格内容格式化，正负研判调用图片
        private void dvGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (!(sender is DoubleBufferDataGridView)) return;
            DoubleBufferDataGridView dgv = sender as DoubleBufferDataGridView;

            if (e.RowIndex != dgv.NewRowIndex)
            {
                switch (dgv.Columns[e.ColumnIndex].Name)
                {
                    case "title_link":
                        //超链
                        string title = dgv.Rows[e.RowIndex].Cells["title"].Value.ToString();
                        e.Value = title;
                        break;
                    case "part_img":
                        //设定正负向
                        string part = dgv.Rows[e.RowIndex].Cells["part"].Value.ToString();
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
                        switch (dgv.Rows[e.RowIndex].Cells["kid"].Value.ToString())
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
                        switch (dgv.Rows[e.RowIndex].Cells["pid"].Value.ToString())
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
                }
            }
        }
        #endregion

        private void UnLoadEvent()
        {
            this.dvWeb.CellContentClick -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dvGridView_CellContentClick);
            this.dvWeb.CellFormatting -= new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dvGridView_CellFormatting);
            this.dvWeb.SelectionChanged -= new System.EventHandler(this.dvGridView_SelectionChanged);

            this.dvWBlog.CellContentClick -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dvGridView_CellContentClick);
            this.dvWBlog.CellFormatting -= new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dvGridView_CellFormatting);
            this.dvWBlog.SelectionChanged -= new System.EventHandler(this.dvGridView_SelectionChanged);

            this.dvWeiXin.CellContentClick -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dvGridView_CellContentClick);
            this.dvWeiXin.CellFormatting -= new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dvGridView_CellFormatting);
            this.dvWeiXin.SelectionChanged -= new System.EventHandler(this.dvGridView_SelectionChanged);

            this.dvtieba.CellContentClick -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dvGridView_CellContentClick);
            this.dvtieba.CellFormatting -= new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dvGridView_CellFormatting);
            this.dvtieba.SelectionChanged -= new System.EventHandler(this.dvGridView_SelectionChanged);

            this.dvBBs.CellContentClick -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dvGridView_CellContentClick);
            this.dvBBs.CellFormatting -= new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dvGridView_CellFormatting);
            this.dvBBs.SelectionChanged -= new System.EventHandler(this.dvGridView_SelectionChanged);

            this.dvBlog.CellContentClick -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dvGridView_CellContentClick);
            this.dvBlog.CellFormatting -= new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dvGridView_CellFormatting);
            this.dvBlog.SelectionChanged -= new System.EventHandler(this.dvGridView_SelectionChanged);

            this.dvAll.CellContentClick -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dvGridView_CellContentClick);
            this.dvAll.CellFormatting -= new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dvGridView_CellFormatting);
            this.dvAll.SelectionChanged -= new System.EventHandler(this.dvGridView_SelectionChanged);

            this.dvCustom.CellContentClick -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dvGridView_CellContentClick);
            this.dvCustom.CellFormatting -= new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dvGridView_CellFormatting);
            this.dvCustom.SelectionChanged -= new System.EventHandler(this.dvGridView_SelectionChanged);
        }
        //切换页签时刷新下方的内容
        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            try
            {
                dataView.Clear();
                switch (e.TabPage.Text)
                {
                    case "全网":
                        pid = 0;
                        RefreshDataGridView(0);
                        this.dvAll.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dvGridView_CellContentClick);
                        this.dvAll.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dvGridView_CellFormatting);
                        this.dvAll.SelectionChanged += new System.EventHandler(this.dvGridView_SelectionChanged);

                        //dvGridView_SelectionChanged(dvAll, null);
                        break;
                    case "定制":
                        pid = 7;
                        RefreshDataGridView(7);
                        this.dvCustom.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dvGridView_CellContentClick);
                        this.dvCustom.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dvGridView_CellFormatting);
                        this.dvCustom.SelectionChanged += new System.EventHandler(this.dvGridView_SelectionChanged);

                        //dvGridView_SelectionChanged(dvCustom, null);
                        break;
                    case "博客":
                        pid = 1;
                        RefreshDataGridView(1);
                        this.dvBlog.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dvGridView_CellContentClick);
                        this.dvBlog.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dvGridView_CellFormatting);
                        this.dvBlog.SelectionChanged += new System.EventHandler(this.dvGridView_SelectionChanged);

                        //dvGridView_SelectionChanged(dvBlog, null);
                        break;
                    case "论坛":                        
                        pid = 2;
                        RefreshDataGridView(2);
                        this.dvBBs.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dvGridView_CellContentClick);
                        this.dvBBs.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dvGridView_CellFormatting);
                        this.dvBBs.SelectionChanged += new System.EventHandler(this.dvGridView_SelectionChanged);

                        //dvGridView_SelectionChanged(dvBBs, null);
                        break;
                    case "微博":
                        pid = 3;
                        RefreshDataGridView(3);
                        this.dvWBlog.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dvGridView_CellContentClick);
                        this.dvWBlog.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dvGridView_CellFormatting);
                        this.dvWBlog.SelectionChanged += new System.EventHandler(this.dvGridView_SelectionChanged);

                        //dvGridView_SelectionChanged(dvWBlog, null);
                        break;
                    case "主流媒体":
                        pid = 4;
                        RefreshDataGridView(4);
                        this.dvWeb.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dvGridView_CellContentClick);
                        this.dvWeb.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dvGridView_CellFormatting);
                        this.dvWeb.SelectionChanged += new System.EventHandler(this.dvGridView_SelectionChanged);

                        //dvGridView_SelectionChanged(dvWeb, null);
                        break;
                    case "贴吧":
                        pid = 5;
                        RefreshDataGridView(5);
                        this.dvtieba.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dvGridView_CellContentClick);
                        this.dvtieba.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dvGridView_CellFormatting);
                        this.dvtieba.SelectionChanged += new System.EventHandler(this.dvGridView_SelectionChanged);

                        //dvGridView_SelectionChanged(dvtieba, null);
                        break;
                    case "微信":
                        pid = 6;
                        RefreshDataGridView(6);
                        this.dvWeiXin.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dvGridView_CellContentClick);
                        this.dvWeiXin.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dvGridView_CellFormatting);
                        this.dvWeiXin.SelectionChanged += new System.EventHandler(this.dvGridView_SelectionChanged);

                        //dvGridView_SelectionChanged(dvWeiXin, null);
                        break;
                }
            }
            catch (Exception ex)
            {
                Comm.WriteErrorLog(ex.StackTrace);
            }
        }

        #region 证据留存
        #region 旧的事件函数
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

        }
        #endregion

        private void ssmLCZJ_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.ContextMenuStrip cms = null;
            if (sender is System.Windows.Forms.ToolStripMenuItem)
            {
                System.Windows.Forms.ToolStripMenuItem tsm = sender as System.Windows.Forms.ToolStripMenuItem;
                if (tsm.Owner is System.Windows.Forms.ContextMenuStrip)
                {
                    cms = tsm.Owner as System.Windows.Forms.ContextMenuStrip;
                }
            }
            else if (sender is System.Windows.Forms.ContextMenuStrip)
            {
                cms = sender as System.Windows.Forms.ContextMenuStrip;
            }
            if (cms != null && cms.SourceControl is DoubleBufferDataGridView)
            {
                DoubleBufferDataGridView dvGridView = (cms.SourceControl as DoubleBufferDataGridView);
                if (dvGridView.SelectedRows != null && dvGridView.SelectedRows.Count > 0)
                {
                    string temp = "";
                    try
                    {
                        Entities.SystemSet ss = (Entities.SystemSet)GlobalPars.GloPars["systemset"];
                        if (!Directory.Exists(ss.EvidenceImgSavePath))
                        {
                            MessageBox.Show("保存证据的目录[" + ss.EvidenceImgSavePath + "]不存在，请先创建该路径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        temp = ss.EvidenceImgSavePath + "\\" + DateTime.Now.ToString().Replace("/", "-").Replace(" ", "-").Replace(":", "-") + ".jpg";

                        Bitmap image = util.WebSnap.StartSnap(dvGridView.SelectedRows[0].Cells["infosource"].Value.ToString());
                        image.Save(temp);
                        MessageBox.Show("证据图片生成成功！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("目标网站原因,证据图片生成失败!");
                    }
                }
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

                //启动数据入库线程
                Thread t = new Thread(new ThreadStart(Thread_WriteDataToDB));
                t.IsBackground = true;
                t.Start();

                //启动抓取线程
                //主流媒体（百度新闻）抓取
                Thread tCrawl1 = new Thread(new ThreadStart(delegate()
                {
                    MediaWebSpiderTimer_Elapsed(null, null);
                }));
                tCrawl1.IsBackground = true;
                tCrawl1.Start();
                MediaWebSpiderTimer.Enabled = true;

                //微信抓取（搜狗微信搜索）
                Thread tCrawl2 = new Thread(new ThreadStart(delegate()
                {
                    WeixinWebSpiderTimer_Elapsed(null, null);
                }));
                tCrawl2.IsBackground = true;
                tCrawl2.Start();
                WeixinWebSpiderTimer.Enabled = true;

                //博客抓取（搜狗博客搜索）
                Thread tCrawl3 = new Thread(new ThreadStart(delegate()
                {
                    BlogWebSpiderTimer_Elapsed(null, null);
                }));
                tCrawl3.IsBackground = true;
                tCrawl3.Start();
                BlogWebSpiderTimer.Enabled = true;

                //论坛抓取（搜狗论坛搜索）
                Thread tCrawl4 = new Thread(new ThreadStart(delegate()
                {
                    BBSWebSpiderTimer_Elapsed(null, null);
                }));
                tCrawl4.IsBackground = true;
                tCrawl4.Start();
                BBSWebSpiderTimer.Enabled = true;

                //贴吧
                Thread tCrawl5 = new Thread(new ThreadStart(delegate()
                {
                    TiebaWebSpiderTimer_Elapsed(null, null);
                }));
                tCrawl5.IsBackground = true;
                tCrawl5.Start();
                TiebaWebSpiderTimer.Enabled = true;

                //微博
                Thread tCrawl6 = new Thread(new ThreadStart(delegate()
                {
                    WeiboWebSpiderTimer_Elapsed(null, null);
                }));
                tCrawl6.IsBackground = true;
                tCrawl6.Start();
                WeiboWebSpiderTimer.Enabled = true;

                //全网
                Thread tCrawl7 = new Thread(new ThreadStart(delegate()
                {
                    BaiduWebWebSpiderTimer_Elapsed(null, null);
                }));
                tCrawl7.IsBackground = true;
                tCrawl7.Start();
                BaiduWebWebSpiderTimer.Enabled = true;

                //定制
                Thread tCrawl8 = new Thread(new ThreadStart(delegate()
                {
                    GeneralWebSpiderTimer_Elapsed(null, null);
                }));
                tCrawl8.IsBackground = true;
                tCrawl8.Start();
                GeneralWebSpiderTimer.Enabled = true;

                //启动刷新线程
                if (pid == -1) pid = 4;//主流媒体
                Thread tCrawl9 = new Thread(new ThreadStart(delegate()
                {
                    RefreshDataTimer_Elapsed(null, null);
                }));
                tCrawl9.IsBackground = true;
                tCrawl9.Start();
                RefreshWebSpiderTimer.Enabled = true;
               

            }
            else
            {
                play = true;
                pictureBox6.BackgroundImage = imageList1.Images[0];
                //butClike = !butClike;
                //qwei.Stop();
                //swei.Stop();
                //likeUrlWebSpiderTimer.Enabled = false;
                //nonLikeUrlWebSpiderTimer.Enabled = false;
                //weiboSpiderTimer.Enabled = false;

                this.pictureBox1.Visible = false;

                //主流媒体（百度新闻）抓取
                MediaWebSpiderTimer.Enabled = false;
                //微信抓取（搜狗微信搜索）
                WeixinWebSpiderTimer.Enabled = false;
                //博客抓取（搜狗博客搜索）
                BlogWebSpiderTimer.Enabled = false;
                //论坛抓取（搜狗论坛搜索）
                BBSWebSpiderTimer.Enabled = false;
                //贴吧
                TiebaWebSpiderTimer.Enabled = false;
                //微博
                WeiboWebSpiderTimer.Enabled = false;
                //全网
                BaiduWebWebSpiderTimer.Enabled = false;
                //定制
                GeneralWebSpiderTimer.Enabled = false;

                RefreshWebSpiderTimer.Enabled = false;
                Program.ProClose = true;
            }
        }
        #endregion


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
                    dv = new DataView(dt);
                    aa = string.Format("InfoSource='{0}'", sUrl);
                    dv.RowFilter = aa;
                    if (dv.Count > 0)
                    {
                        return 1;
                    }

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
                    //如果列表中不存在,就去库中判断
                    dv = new DataView(dt);
                    aa = string.Format("title='{0}'", sUrl);
                    dv.RowFilter = aa;
                    if (dv.Count > 0)
                    {
                        return 1;
                    }
                    //如果没有一样的就返回0
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Comm.WriteErrorLog(ex.StackTrace);
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

            //if (isWeibo == true)
            //{
            //    obj.Columns[3].Visible = false;
            //    obj.Columns[12].Visible = true;
            //}            
            obj.Columns[12].Visible = true;
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            //卸载表格控件的事件
            UnLoadEvent();
        }

        class MyResult
        {
            public DataTable dt;
            public int pid;
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int pid = (int)e.Argument;
            //dvWeb.DataSource = null;
            //dvWeb.Columns.Clear();
            DataTable dt = tri.GetLatestData(pid);
            //dvWeb.DataSource = dt;
            //FormatDataView(dvWeb);

            MyResult r = new MyResult();
            r.dt = dt;
            r.pid = pid;
            e.Result = r;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (!(e.Result is MyResult)) return;
                MyResult r = (MyResult)e.Result;
                DataTable dt = r.dt;
                int pid = r.pid;
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    if (pid == 4 && tabControl1.SelectedTab.Text == "主流媒体")
                    {
                        dvWeb.DataSource = null;
                        dvWeb.Columns.Clear();
                        dvWeb.DataSource = dt;
                        FormatDataView(dvWeb);
                    }
                    if (pid == 3 && tabControl1.SelectedTab.Text == "微博")
                    {
                        dvWBlog.DataSource = null;
                        dvWBlog.Columns.Clear();
                        dvWBlog.DataSource = dt;
                        FormatDataView(dvWBlog);
                    }
                    if (pid == 6 && tabControl1.SelectedTab.Text == "微信")
                    {
                        dvWeiXin.DataSource = null;
                        dvWeiXin.Columns.Clear();
                        dvWeiXin.DataSource = dt;
                        FormatDataView(dvWeiXin);
                    }
                    if (pid == 5 && tabControl1.SelectedTab.Text == "贴吧")
                    {
                        dvtieba.DataSource = null;
                        dvtieba.Columns.Clear();
                        dvtieba.DataSource = dt;
                        FormatDataView(dvtieba);
                    }
                    if (pid == 2 && tabControl1.SelectedTab.Text == "论坛")
                    {
                        dvBBs.DataSource = null;
                        dvBBs.Columns.Clear();
                        dvBBs.DataSource = dt;
                        FormatDataView(dvBBs);
                    }

                    if (pid == 1 && tabControl1.SelectedTab.Text == "博客")
                    {
                        dvBlog.DataSource = null;
                        dvBlog.Columns.Clear();
                        dvBlog.DataSource = dt;
                        FormatDataView(dvBlog);
                    }
                    if (pid == 0 && tabControl1.SelectedTab.Text == "全网")
                    {
                        dvAll.DataSource = null;
                        dvAll.Columns.Clear();
                        dvAll.DataSource = dt;
                        FormatDataView(dvAll);
                    }
                    if (pid == 7 && tabControl1.SelectedTab.Text == "定制")
                    {
                        dvCustom.DataSource = null;
                        dvCustom.Columns.Clear();
                        dvCustom.DataSource = dt;
                        FormatDataView(dvCustom);
                    }
                }));
            }
            catch (Exception ex)
            { }
            finally
            {
            }
        }

    }
}
