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

        private DataTable dtData;
        Dictionary<string, List<string>> dicKeywords = new Dictionary<string, List<string>>();
        private string selectKwName;

        TbReleaseInfo tri;
        System.Threading.AutoResetEvent obj = new System.Threading.AutoResetEvent(false);

        DataBaseServer.SQLitecommand cmd = new DataBaseServer.SQLitecommand();
        private DataView dv;

        string SoftVer;

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

            #region 初始化表格控件
            tri = new TbReleaseInfo();
            FormatDataView2(dvView);

            dtData = tri.GetReleaseInfoFormat();

            dvView.DataSource = dtData;
            #endregion

            SoftVer = !GlobalPars.GloPars.ContainsKey("SoftVer") ? "1" : GlobalPars.GloPars["SoftVer"].ToString();
            if (!SoftVer.Equals("3"))
            {
                //不能使用微信
                //tabControl1.TabPages.RemoveByKey("tabPage7");
                chkWeixin.Visible = false;
            }

            chkAllWeb.Checked = true;
            chkBBS.Checked = true;
            chkBlog.Checked = true;
            chkCustom.Checked = true;
            chkMedia.Checked = true;
            chkTieba.Checked = true;
            chkWeixin.Checked = true;

            kidlist.SelectedIndex = 4;  //事件类型 (默认选择全部)
            kwlist.SelectedIndex = 0;   //事件名称 (启动时隐藏)
            kwlist.Hide();  //事件名称
            label8.Hide();  //事件名称

            #region 提取事件与关键字
            DataTable kwdtAll = cmd.GetTabel("select name, keyword from keywords");
            for (int i = 0; i < kwdtAll.Rows.Count; i++)
            {
                string key = kwdtAll.Rows[i]["name"].ToString();
                if (!dicKeywords.ContainsKey(key))
                {
                    List<string> keywords = new List<string>();
                    keywords.Add(kwdtAll.Rows[i]["keyword"].ToString());
                    dicKeywords.Add(key, keywords);
                }
                else
                {
                    dicKeywords[key].Add(kwdtAll.Rows[i]["keyword"].ToString());
                }
            }
            #endregion


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
        #region 2015.3 wangcg 抓取调度程序
        #region 通用抓取程序
        private void BeginGeneralEvn()
        {
            if (!Program.ProClose)
            {
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
                RefreshDataGridView();
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
        #region 2015.3 wangcg 添加 抓取解析程序
        #region 通用网站抓取程序
        /// <summary>
        /// 通用网站抓取程序
        /// </summary>
        private void GeneralWebSpider()
        {
            DataBaseServer.SQLitecommand cmd = new SQLitecommand();
            //得到关键字列表
            DataTable dtkey = cmd.GetTabel("select * from Keywords");
            //得到抓取网站的地址
            DataTable dtXs = cmd.GetTabel("Select * from WebAddress");

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

                bool isCatchSubWeb = false;
                BasicWebSpider(url, Similar, dtkey, sheng, shi, xian, name, isCatchSubWeb);

                //防止拉黑
                Thread.Sleep(2000);
            }

            #endregion
        }

        private void BasicWebSpider(string motherUrl, string similar, DataTable keys, string sheng, string shi, string xian, string name, bool catchSub)
        {
            DataBaseServer.SQLitecommand cmd = new SQLitecommand();
            //相似表中的被抓取网址
            string webInfo = "";

            HtmlParse parse = new HtmlParse();

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
        //private int pid = -1;
        public void RefreshDataGridView()
        {
            lock (thisLockSwitch)
            {
                if (!backgroundWorker1.IsBusy)
                {
                    backgroundWorker1.RunWorkerAsync();
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
                    case "releasename":
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

        private void FormatDataView2(DataGridView obj)
        {
            obj.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "uid", DataPropertyName = "uid" });
            obj.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "事件", DataPropertyName = "keywords" });
            obj.Columns.Add(new DataGridViewLinkColumn() { HeaderText = "标题", DataPropertyName = "title" });
            obj.Columns.Add(new DataGridViewLinkColumn() { HeaderText = "链接", DataPropertyName = "infosource" });
            obj.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "内容", DataPropertyName = "contexts" });
            obj.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "发布者", DataPropertyName = "releasename" });
            obj.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "转发量", DataPropertyName = "reposts" });
            obj.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "评论数", DataPropertyName = "comments" });
            obj.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "网站类别", DataPropertyName = "pid" });
            obj.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "来源", DataPropertyName = "webname" });
            obj.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "抓取时间", DataPropertyName = "collectdate" });
            obj.Columns.Add(new DataGridViewImageColumn() { HeaderText = "评价", DataPropertyName = "part" });
            obj.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "发布时间", DataPropertyName = "releasedate" });
            obj.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "快照", DataPropertyName = "snapshot" });

            obj.Columns[0].Visible = false;
            obj.Columns[3].Visible = false;
            obj.Columns[5].Visible = false;
            obj.Columns[6].Visible = false;
            obj.Columns[7].Visible = false;

            obj.Columns[8].Visible = true;
            obj.Columns[12].Visible = true;
            obj.Columns[13].Visible = false;

            obj.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            obj.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            obj.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            obj.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            obj.Columns[4].Width = 480;
            obj.Columns[6].Width = 80;
            obj.Columns[7].Width = 80;
            obj.Columns[8].Width = 60;
            obj.Columns[10].Width = 160;
            obj.Columns[11].Width = 60;
            obj.Columns[12].Width = 160;
            obj.Columns[13].Width = 60;
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
                if (selectKwName != "全部")
                {
                    if (dtkey.Rows[kw]["name"].ToString().Trim() != selectKwName) continue;
                }
                string keyword = dtkey.Rows[kw]["KeyWord"].ToString().Trim();
                //string keyTitle = dtkey.Rows[kw]["Name"].ToString().Trim();
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
            //DataTable dtParts;
            dtkey = cmd.GetTabel("select * from Keywords");
            //dtParts = cmd.GetTabel("SELECT * FROM partword");
            #endregion

            HtmlParse parse = new HtmlParse();
            parse.ReportCatchProcess += new HtmlParse.ReportCatchProcessEventHandler(Weixin_ReportCatchProcess);
            List<ModelReleaseInfo> webDatas = new List<ModelReleaseInfo>();
            //按关键字循环
            for (int kw = 0; kw < dtkey.Rows.Count; kw++)
            {
                //处理关键字
                if (selectKwName != "全部")
                {
                    if (dtkey.Rows[kw]["name"].ToString().Trim() != selectKwName) continue;
                }

                string keyword = dtkey.Rows[kw]["KeyWord"].ToString().Trim();
                //string keyTitle = dtkey.Rows[kw]["Name"].ToString().Trim();
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
                if (selectKwName != "全部")
                {
                    if (dtkey.Rows[kw]["name"].ToString().Trim() != selectKwName) continue;
                }

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
                if (selectKwName != "全部")
                {
                    if (dtkey.Rows[kw]["name"].ToString().Trim() != selectKwName) continue;
                }

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
                //处理关键字
                if (selectKwName != "全部")
                {
                    if (dtkey.Rows[kw]["name"].ToString().Trim() != selectKwName) continue;
                }

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
                if (selectKwName != "全部")
                {
                    if (dtkey.Rows[kw]["name"].ToString().Trim() != selectKwName) continue;
                }

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
                if (selectKwName != "全部")
                {
                    if (dtkey.Rows[kw]["name"].ToString().Trim() != selectKwName) continue;
                }

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

        #region 表格内容格式化，正负研判调用图片
        private void dvGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (!(sender is DoubleBufferDataGridView)) return;
            DoubleBufferDataGridView dgv = sender as DoubleBufferDataGridView;

            if (e.RowIndex != dgv.NewRowIndex)
            {
                switch (dgv.Columns[e.ColumnIndex].Name.ToLower())
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

        #region 证据留存
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
                if (chkMedia.Checked)
                {
                    Thread tCrawl1 = new Thread(new ThreadStart(delegate()
                    {
                        MediaWebSpiderTimer_Elapsed(null, null);
                    }));
                    tCrawl1.IsBackground = true;
                    tCrawl1.Start();
                    MediaWebSpiderTimer.Enabled = true;
                }

                //微信抓取（搜狗微信搜索）
                if (chkWeixin.Checked)
                {
                    Thread tCrawl2 = new Thread(new ThreadStart(delegate()
                    {
                        WeixinWebSpiderTimer_Elapsed(null, null);
                    }));
                    tCrawl2.IsBackground = true;
                    tCrawl2.Start();
                    WeixinWebSpiderTimer.Enabled = true;
                }

                //博客抓取（搜狗博客搜索）
                if (chkBlog.Checked)
                {
                    Thread tCrawl3 = new Thread(new ThreadStart(delegate()
                    {
                        BlogWebSpiderTimer_Elapsed(null, null);
                    }));
                    tCrawl3.IsBackground = true;
                    tCrawl3.Start();
                    BlogWebSpiderTimer.Enabled = true;
                }

                //论坛抓取（搜狗论坛搜索）
                if (chkBBS.Checked)
                {
                    Thread tCrawl4 = new Thread(new ThreadStart(delegate()
                    {
                        BBSWebSpiderTimer_Elapsed(null, null);
                    }));
                    tCrawl4.IsBackground = true;
                    tCrawl4.Start();
                    BBSWebSpiderTimer.Enabled = true;
                }

                //贴吧
                if (chkTieba.Checked)
                {
                    Thread tCrawl5 = new Thread(new ThreadStart(delegate()
                    {
                        TiebaWebSpiderTimer_Elapsed(null, null);
                    }));
                    tCrawl5.IsBackground = true;
                    tCrawl5.Start();
                    TiebaWebSpiderTimer.Enabled = true;
                }

                //微博
                if (chkWeibo.Checked)
                {
                    Thread tCrawl6 = new Thread(new ThreadStart(delegate()
                    {
                        WeiboWebSpiderTimer_Elapsed(null, null);
                    }));
                    tCrawl6.IsBackground = true;
                    tCrawl6.Start();
                    WeiboWebSpiderTimer.Enabled = true;
                }

                //全网
                if (chkAllWeb.Checked)
                {
                    Thread tCrawl7 = new Thread(new ThreadStart(delegate()
                    {
                        BaiduWebWebSpiderTimer_Elapsed(null, null);
                    }));
                    tCrawl7.IsBackground = true;
                    tCrawl7.Start();
                    BaiduWebWebSpiderTimer.Enabled = true;
                }

                //定制
                if (chkCustom.Checked)
                {
                    Thread tCrawl8 = new Thread(new ThreadStart(delegate()
                    {
                        GeneralWebSpiderTimer_Elapsed(null, null);
                    }));
                    tCrawl8.IsBackground = true;
                    tCrawl8.Start();
                    GeneralWebSpiderTimer.Enabled = true;
                }

                //启动刷新线程
                Thread tCrawl9 = new Thread(new ThreadStart(delegate()
                {
                    RefreshDataTimer_Elapsed(null, null);
                }));
                tCrawl9.IsBackground = true;
                tCrawl9.Start();
                RefreshWebSpiderTimer.Enabled = true;

                kidlist.Enabled = false;
                kwlist.Enabled = false;

                chkAllWeb.Enabled = false;
                chkMedia.Enabled = false;
                chkWeibo.Enabled = false;
                chkWeixin.Enabled = false;
                chkTieba.Enabled = false;
                chkBBS.Enabled = false;
                chkBlog.Enabled = false;
                chkCustom.Enabled = false;

            }
            else
            {
                play = true;
                pictureBox6.BackgroundImage = imageList1.Images[0];
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

                kidlist.Enabled = true;
                kwlist.Enabled = true;

                chkAllWeb.Enabled = true;
                chkMedia.Enabled = true;
                chkWeibo.Enabled = true;
                chkWeixin.Enabled = true;
                chkTieba.Enabled = true;
                chkBBS.Enabled = true;
                chkBlog.Enabled = true;
                chkCustom.Enabled = true;

                Program.ProClose = true;
            }
        }
        #endregion

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            DataTable dt = tri.GetLatestData();
            e.Result = dt;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (!(e.Result is DataTable)) return;
                DataTable dt = (DataTable)e.Result;
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    dvView.DataSource = null;
                    dvView.Columns.Clear();
                    dvView.DataSource = dt;
                    FormatDataView(dvView);

                }));
            }
            catch (Exception ex)
            { }
            finally
            {
            }
        }

        private void kwlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectKwName = kwlist.Text;

        }

        private void kidlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            string kid = kidlist.SelectedIndex.ToString();
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

                //dt.AcceptChanges();

                kwlist.DataSource = dt;
                kwlist.SelectedIndex = kwlist.Items.Count - 1;
                kwlist.Show();
                label8.Show();
            }

        }

    }
}
