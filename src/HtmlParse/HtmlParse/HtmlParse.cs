using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Data;
using System.Net;
using System.IO;

namespace HtmlParse
{
    public class ModelReleaseInfo
    {
        private int _uid;

        /// <summary>
        /// 流水号
        /// </summary>
        public int Uid
        {
            get { return _uid; }
            set { _uid = value; }
        }
        private int _kid;

        public int Kid
        {
            get { return _kid; }
            set { _kid = value; }
        }
        private string _sheng;

        public string Sheng
        {
            get { return _sheng; }
            set { _sheng = value; }
        }
        private string _shi;

        public string Shi
        {
            get { return _shi; }
            set { _shi = value; }
        }
        private string _xian;

        public string Xian
        {
            get { return _xian; }
            set { _xian = value; }
        }
        private string _Title;
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        private string _Contexts;

        /// <summary>
        /// 内容
        /// </summary>
        public string Contexts
        {
            get { return _Contexts; }
            set { _Contexts = value; }
        }
        private string _ReleaseDate;

        /// <summary>
        /// 发布日期
        /// </summary>
        public string ReleaseDate
        {
            get { return _ReleaseDate; }
            set { _ReleaseDate = value; }
        }
        private string _InfoSource;

        /// <summary>
        /// 信息来源
        /// </summary>
        public string InfoSource
        {
            get { return _InfoSource; }
            set { _InfoSource = value; }
        }
        private string _KeyWords;

        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWords
        {
            get { return _KeyWords; }
            set { _KeyWords = value; }
        }
        private string _ReleaseName;

        /// <summary>
        /// 发布人
        /// </summary>
        public string ReleaseName
        {
            get { return _ReleaseName; }
            set { _ReleaseName = value; }
        }
        private string _CollectDate;

        /// <summary>
        /// 采集时间
        /// </summary>
        public string CollectDate
        {
            get { return _CollectDate; }
            set { _CollectDate = value; }
        }
        private string _Snapshot;

        /// <summary>
        /// 快照
        /// </summary>
        public string Snapshot
        {
            get { return _Snapshot; }
            set { _Snapshot = value; }
        }

        private string _WebName;
        /// <summary>
        /// 网站名
        /// </summary>
        public string WebName
        {
            get { return _WebName; }
            set { _WebName = value; }
        }

        private int _pid;
        /// <summary>
        /// 数据类别：0：其他，1：博客，2：论坛，3：微博
        /// </summary>
        public int Pid
        {
            get { return _pid; }
            set { _pid = value; }
        }
        private int _part;

        /// <summary>
        /// 正负词性。0：负，1：正
        /// </summary>
        public int Part
        {
            get { return _part; }
            set { _part = value; }
        }
        private int _reposts;
        /// <summary>
        /// 此项未用
        /// </summary>
        public int Reposts
        {
            get { return _reposts; }
            set { _reposts = value; }
        }
        private int _Comments;

        /// <summary>
        /// 此项未用
        /// </summary>
        public int Comments
        {
            get { return _Comments; }
            set { _Comments = value; }
        }
    }

    public class Parse
    {
        #region 查询进度报告
        public delegate void ReportCatchProcessEventHandler(ModelReleaseInfo mri);
        public event ReportCatchProcessEventHandler ReportCatchProcess;
        public void OnReportCactchProcess(ModelReleaseInfo mri)
        {
            if (this.ReportCatchProcess != null)
                this.ReportCatchProcess(mri);
        }
        #endregion
        public string FormateDate(string soureDate)
        {
            string publishDate = DateTime.Now.ToString("yyyy-MM-dd");
            string txt = soureDate;
            if (txt.EndsWith("前"))
            {
                if (txt.LastIndexOf('-') > 0)
                {
                    publishDate = txt.Substring(txt.LastIndexOf('-') + 1);
                    if (publishDate.Contains("天前"))
                    {
                        int offset = int.Parse(publishDate.Substring(0, publishDate.IndexOf("天前")).Trim());
                        publishDate = DateTime.Now.AddDays(offset * -1).ToString("yyyy-MM-dd");
                    }
                    else if (publishDate.Contains("小时前"))
                    {
                        int offset = int.Parse(publishDate.Substring(0, publishDate.IndexOf("小时前")).Trim());
                        publishDate = DateTime.Now.AddHours(offset * -1).ToString("yyyy-MM-dd");
                    }
                    else if (publishDate.Contains("时前"))
                    {
                        int offset = int.Parse(publishDate.Substring(0, publishDate.IndexOf("时前")).Trim());
                        publishDate = DateTime.Now.AddHours(offset * -1).ToString("yyyy-MM-dd");
                    }
                    else if (publishDate.Contains("分") && publishDate.Contains("前"))
                    {
                        int offset = int.Parse(publishDate.Substring(0, publishDate.IndexOf("分")).Trim());
                        publishDate = DateTime.Now.AddMinutes(offset * -1).ToString("yyyy-MM-dd");
                    }
                }
            }
            else
            {
                //2015/4/10
                if (txt.IndexOf('/') > 0)
                {
                    string[] tmp = txt.Split('/');
                    if (tmp != null && tmp.Length == 3)
                    {
                        publishDate = string.Format("{0}-{1}-{2}",
                            tmp[0].Length == 2 ? "20" + tmp[0] : tmp[0],
                            tmp[1].Length == 1 ? "0" + tmp[1] : tmp[1],
                            tmp[2].Length == 1 ? "0" + tmp[2] : tmp[2]);
                    }
                }
                else if (txt.IndexOf('-') > 0)
                {
                    string[] tmp = txt.Split('-');
                    if (tmp != null && tmp.Length == 3)
                    {
                        publishDate = string.Format("{0}-{1}-{2}",
                            tmp[0].Length == 2 ? "20" + tmp[0] : tmp[0],
                            tmp[1].Length == 1 ? "0" + tmp[1] : tmp[1],
                            tmp[2].Length == 1 ? "0" + tmp[2] : tmp[2]);
                    }
                }
            }
            return publishDate;
        }
        public string FormateDate2(string soureDate)
        {
            string publishDate = DateTime.Now.ToString("yyyy-MM-dd");
            string txt = soureDate.Trim();
            if (txt.StartsWith("今天"))
            {
                string time = txt.Substring(2).Trim();
                publishDate = string.Format("{0} {1}", DateTime.Now.ToString("yyyy-MM-dd"), time);
            }
            else if (txt.StartsWith("昨天"))
            {
                string time = txt.Substring(2).Trim();
                publishDate = string.Format("{0} {1}", DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"), time);
            }
            else if (txt.EndsWith("前"))
            {
                if (txt.LastIndexOf('-') > 0)
                {
                    publishDate = txt.Substring(txt.LastIndexOf('-') + 1);
                    if (publishDate.Contains("天前"))
                    {
                        int offset = int.Parse(publishDate.Substring(0, publishDate.IndexOf("天前")).Trim());
                        publishDate = DateTime.Now.AddDays(offset * -1).ToString("yyyy-MM-dd");
                    }
                    else if (publishDate.Contains("小时前"))
                    {
                        int offset = int.Parse(publishDate.Substring(0, publishDate.IndexOf("小时前")).Trim());
                        publishDate = DateTime.Now.AddHours(offset * -1).ToString("yyyy-MM-dd");
                    }
                    else if (publishDate.Contains("时前"))
                    {
                        int offset = int.Parse(publishDate.Substring(0, publishDate.IndexOf("时前")).Trim());
                        publishDate = DateTime.Now.AddHours(offset * -1).ToString("yyyy-MM-dd");
                    }
                    else if (publishDate.Contains("分") && publishDate.Contains("前"))
                    {
                        int offset = int.Parse(publishDate.Substring(0, publishDate.IndexOf("分")).Trim());
                        publishDate = DateTime.Now.AddMinutes(offset * -1).ToString("yyyy-MM-dd");
                    }
                }
            }
            else
            {
                if (txt.IndexOf("年") > 0)
                {
                    publishDate = txt.Replace("年", "-").Replace("月", "-").Replace("日", "");
                }
                else
                {
                    publishDate = DateTime.Now.Year + "-" + txt.Replace("月", "-").Replace("日", "");
                }
            }
            return publishDate;
        }
        public string GetSogouAuthorAndDate(string text)
        {
            //<cite id="cacheresult_info_0">新浪博客 - blog.sina.com.cn/s/...&nbsp;-&nbsp;2013-1-28</cite>
            //<cite id="cacheresult_info_8">搜狐母婴 - club.baobao.sohu.co...&nbsp;-&nbsp;2天前</cite>
            //<cite id="cacheresult_info_9">商都BBS - bbs.shangdu.com - 2天前</cite>
            //<cite id="cacheresult_info_7">豆瓣 - www.douban.com - 2015-3-10</cite>

            //时间假定有 x天前，x小时前，x分前，xxxx年xx月xx日，xxxx-xx-xx

            byte[] space = new byte[] { 0xc2, 0xa0 };
            string UTFSpace = Encoding.GetEncoding("UTF-8").GetString(space);
            string txt = text.Replace(UTFSpace, " ").Trim();

            string author = text;
            string publishDate = DateTime.Now.ToString("yyyy-MM-dd"); ;
            try
            {
                if (txt.IndexOf('-') > 0)
                {
                    author = txt.Substring(0, txt.IndexOf('-')).Trim();

                    if (txt.EndsWith("前"))
                    {
                        if (txt.LastIndexOf('-') > 0)
                        {
                            publishDate = txt.Substring(txt.LastIndexOf('-') + 1);
                            if (publishDate.Contains("天前"))
                            {
                                int offset = int.Parse(publishDate.Substring(0, publishDate.IndexOf("天前")).Trim());
                                publishDate = DateTime.Now.AddDays(offset * -1).ToString("yyyy-MM-dd");
                            }
                            else if (publishDate.Contains("小时前"))
                            {
                                int offset = int.Parse(publishDate.Substring(0, publishDate.IndexOf("小时前")).Trim());
                                publishDate = DateTime.Now.AddHours(offset * -1).ToString("yyyy-MM-dd");
                            }
                            else if (publishDate.Contains("时前"))
                            {
                                int offset = int.Parse(publishDate.Substring(0, publishDate.IndexOf("时前")).Trim());
                                publishDate = DateTime.Now.AddHours(offset * -1).ToString("yyyy-MM-dd");
                            }
                            else if (publishDate.Contains("分") && publishDate.Contains("前"))
                            {
                                int offset = int.Parse(publishDate.Substring(0, publishDate.IndexOf("分")).Trim());
                                publishDate = DateTime.Now.AddMinutes(offset * -1).ToString("yyyy-MM-dd");
                            }
                        }
                    }
                    else
                    {
                        string day = "";
                        string month = "";
                        string year = "";
                        if (txt.LastIndexOf('-') >= 0)
                        {
                            day = txt.Substring(txt.LastIndexOf('-') + 1).Trim();  //日
                            txt = txt.Substring(0, txt.LastIndexOf('-'));
                        }
                        if (txt.LastIndexOf('-') >= 0)
                        {
                            month = txt.Substring(txt.LastIndexOf('-') + 1).Trim();//月
                            txt = txt.Substring(0, txt.LastIndexOf('-'));
                        }
                        if (txt.LastIndexOf('-') >= 0)
                        {
                            year = txt.Substring(txt.LastIndexOf('-') + 1).Trim();//年
                        }
                        if (!string.IsNullOrEmpty(year) && !string.IsNullOrEmpty(year) && !string.IsNullOrEmpty(year))
                        {
                            publishDate = string.Format("{0}-{1}-{2}", year, month, day);
                        }
                    }

                }
            }
            catch (Exception ex)
            { }
            return string.Format("{0},{1}", author, publishDate);
        }

        class seachRecord
        {
            public int _min;
            public int _max;

            public seachRecord(int min, int max)
            {
                this._min = min;
                this._max = max;
            }
        }
        public string GetContexts(string contexts, string keyword)
        {
            string retContexts = "";
            List<seachRecord> seached = new List<seachRecord>();          

            int preCount = 200, nextCount = 100;
            string[] splitKw = keyword.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (splitKw != null && splitKw.Count() > 0)
            {
                int start = 0;
                foreach (string kw in splitKw)
                {
                    start = contexts.IndexOf(kw, start);  
                    while (start >= 0)
                    {                        
                        int len = 0;
                        if (start < preCount)
                        {
                            len = start + kw.Length + nextCount;
                            start = 0;
                        }
                        else
                        {
                            len = preCount + kw.Length + nextCount;
                            start = start - preCount;                            
                        }

                        bool has = false;
                        foreach (var s in seached)
                        {
                            if (start > s._min && start + len < s._max)
                            {
                                has = true;
                                break;
                            }
                        }

                        if (!has)
                        {
                            if (!string.IsNullOrEmpty(retContexts))
                            {
                                retContexts += "  \r\n   ……   \r\n  ";
                            }
                            retContexts += contexts.Substring(start, len);
                            seached.Add(new seachRecord(start, start + len));

                            start = start + len;
                            start = contexts.IndexOf(kw, start);  
                        }
                    }
                }
            }
            return retContexts;
        }

        enum WebSourceType
        {
            BaiduNews = 1,
            BingNews = 2,
            SogouNews = 3,
            ZhongsouNews = 4,
            HaosouNews = 5,
            BaiduWeb = 6,
            BingWeb = 7,
            SogouWeb = 8,
            ZhongsouWeb = 9,
            HaosouWeb = 10,
            SogouWeixin = 11,
            SogouBlog = 12,
            SogouBBS = 13,
            ZhongsouBBS = 14,
            BaiduTieba = 15,
            SinWeibo = 16,
            ZhongsouWeibo = 17
        }
        public List<ModelReleaseInfo> ParseGeneralWeb(string html, string url, DataTable dtkey, string sheng, string shi, string xian, string webName, string webInfo, int pid)
        {
            //string strURLformat = "https?://.[^\"]+";
            List<ModelReleaseInfo> mris = new List<ModelReleaseInfo>();
            for (int i = 0; i < dtkey.Rows.Count; i++)
            {
                string keywords = dtkey.Rows[i]["keyword"].ToString();
                int kid = 0;
                int.TryParse(dtkey.Rows[i]["kid"].ToString().Trim(), out kid);

                string title = HtmlUtil.NoHTML(html);
                string context = HtmlUtil.NoHTML(webInfo);
                if (!string.IsNullOrEmpty(keywords))
                {
                    bool isFundTitle = true;
                    bool isFundContext = true;
                    string[] keyw = keywords.Split(' ');
                    if (keyw != null && keyw.Count() > 0)
                    {
                        foreach (string key in keyw)
                        {
                            if (title.IndexOf(key) < 0)
                            {
                                isFundTitle = false;
                            }
                            if (context.IndexOf(key) < 0)
                            {
                                isFundContext = false;
                            }
                        }
                    }
                    if (isFundTitle || isFundContext)
                    {
                        //如果标题或者内容有一个完全匹配关键字则添加该条数据
                        ModelReleaseInfo mri = new ModelReleaseInfo();
                        mri.Title = title;
                        mri.KeyWords = keywords;
                        mri.Contexts = context;
                        mri.InfoSource = url;

                        //发布人和发布日期暂时无法取到,手工赋值
                        mri.ReleaseDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        mri.ReleaseName = "";

                        //收集日期
                        mri.CollectDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        //网页快照,这里为用户指定生成,如果未选择生成,那么为空
                        mri.Snapshot = "";
                        mri.Sheng = sheng == null ? "" : sheng;
                        mri.Shi = shi == null ? "" : shi;
                        mri.Xian = xian == null ? "" : xian;
                        //网站名
                        mri.WebName = webName == null ? "" : webName;
                        //pid
                        mri.Pid = pid;
                        mri.Kid = kid;
                        //part正负判断
                        mri.Part = 0; //CrawlHtml.GetParts(mri.Contexts);
                        //reposts
                        mri.Reposts = 0;
                        //comments
                        mri.Comments = 0;

                        #region 报告进度
                        OnReportCactchProcess(mri);
                        #endregion

                        mris.Add(mri);
                    }
                }
            }
            return mris;
        }

        #region 主流媒体
        public List<ModelReleaseInfo> ParseBaiduNews(string html, string keyword, int kid)
        {
            List<ModelReleaseInfo> webDatas = new List<ModelReleaseInfo>();
            try
            {
                #region 网页源码样例
                //<li class="result" id="1">
                //    <h3 class="c-title">
                //        <a href="http://news.sina.com.cn/c/2015-03-10/023931587467.shtml"  data-click="{      'f0':'77A717EA',      'f1':'9F63F1E4',      'f2':'4CA6DE6E',      'f3':'54E5243F',      't':'1425975464'      }" target="_blank">澳门中联办主任:<em>反腐</em>后很多官员不敢再来赌博</a>
                //    </h3>
                //    <div class="c-summary c-row">
                //        <p class="c-author">新浪新闻&nbsp;&nbsp;13小时前</p> 由于国家加大<em>反腐</em>力度,很多人不敢再到澳门参与赌博,李刚表示,这也是<em>博彩</em>业连续9个月下跌的原因之一。 【谈“占中”】 澳门不支持类似行为 去年发生在香港的“...  
                //        <span class="c-info">
                //            <a href="/ns?word=%E5%8F%8D%E8%85%90%20%E5%8D%9A%E5%BD%A9+cont:2431691772&amp;same=13&amp;cl=1&amp;tn=news&amp;rn=30&amp;fm=sd" class="c-more_link" data-click="{'fm':'sd'}">13条相同新闻</a>
                //            &nbsp;&nbsp;-&nbsp;&nbsp;<a href="http://cache.baidu.com/c?m=9f65cb4a8c8507ed4fece763104a8023584380147d8c8c4668d4e419ce3b4c413037bfa6763f1006d0c26b6777ad484bea8635702a0120b690c98b4dd7be942c2a9f27433146c01e4cc75cf28b102ad650954d99a90e97b0e741e7b9d2a2c82453dd200e6df0f09c2b73&amp;p=97759a43d08903eb11acc7710f48&amp;newp=8b2a9707829b0aff57eb92291b5992694f08d7267dc8914212d2950ac73c10&amp;user=baidu&amp;fm=sc&amp;query=%B7%B4%B8%AF+%B2%A9%B2%CA&amp;qid=a46011fc00005d39&amp;p1=1" data-click="{'fm':'sc'}" target="_blank" class="c-cache">百度快照</a>
                //        </span>
                //    </div>
                //</li>

                //<li class="result" id="8"><h3 class="c-title"><a href="http://forex.hexun.com/2015-03-06/173805302.html"><em>两会任性翻译</em>姐:英语不是白学的!</a></h3>
                //    <div class="c-summary c-row c-gap-top-small">
                //        <div class="c-span6">
                //            <a class="c_photo" href="http://forex.hexun.com/2015-03-06/173805302.html" ><img class="c-img c-img6" src="http://t12.baidu.com/it/u=3225043934,4141352763&amp;fm=55&amp;s=9F0B834F0A727D966080583C03005068&amp;w=121&amp;h=81&amp;img.JPEG" alt=""></a>
                //        </div>
                //        <div class="c-span18 c-span-last">
                //            <p class="c-author">和讯外汇&nbsp;&nbsp;2015年03月06日 11:00</p> 女翻译当时楞了,回头与吕新华沟通,确认后再翻译,由此<em>两会任性女翻译走红</em>。<em>两会任性女翻译走红</em>后,其身份受到了网友关注,有媒体爆料,两会任性女翻译老公是习大大和奥...  <span class="c-info"><a href="/ns?word=%E4%B8%A4%E4%BC%9A%E4%BB%BB%E6%80%A7%E5%A5%B3%E7%BF%BB%E8%AF%91%E8%B5%B0%E7%BA%A2+cont:1156374414&amp;same=2&amp;cl=1&amp;tn=news&amp;rn=30&amp;fm=sd" class="c-more_link" data-click="{'fm':'sd'}">2条相同新闻</a>&nbsp;&nbsp;-&nbsp;&nbsp;<a href="http://cache.baidu.com/c?m=9d78d513d9d430ac4f9de2697d66c0111a4381132ba6da020ea3843e91732d47506793ac56250777a4d27d1716df4c4b99862104371457c78cc9f85dabbe855e5b9f5747676bf755559347a091006383379129f4b24dbafaa77884aea589881e149644050dd1add4470016c968e71447e1a78e48635d14a7ee3564f55b70289d2357b630a3a66d30&amp;p=b449d016d9c157ff57e69268454a&amp;newp=9277c64ad4891afb00bd9b750b0892695c02dc3051d4d616358fc710&amp;user=baidu&amp;fm=sc&amp;query=%C1%BD%BB%E1%C8%CE%D0%D4%C5%AE%B7%AD%D2%EB%D7%DF%BA%EC&amp;qid=e48e7f4100000633&amp;p1=8" data-click="{'fm':'sc'}" target="_blank" class="c-cache">百度快照</a></span></div></div></li>
                #endregion
                #region 解析网站源码
                //html = html.Replace("<em>", "").Replace("</em>", "");
                MIL.Html.HtmlDocument doc = MIL.Html.HtmlDocument.Create(html);
                MIL.Html.HtmlNodeCollection nodes = doc.Nodes.FindByAttributeNameValue("class", "result", true);
                foreach (MIL.Html.HtmlNode n in nodes)
                {
                    if ((n is MIL.Html.HtmlElement) && (n as MIL.Html.HtmlElement).Nodes != null)
                    {
                        ModelReleaseInfo mri = new ModelReleaseInfo();
                        #region 标题与超链
                        MIL.Html.HtmlNodeCollection titleNodes = (n as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "c-title");
                        if (titleNodes != null && titleNodes.Count > 0)
                        {
                            //title中包含<em>节点，需要进行处理
                            string title = "";
                            MIL.Html.HtmlElement titleElement = ((titleNodes[0] as MIL.Html.HtmlElement).Nodes[0] as MIL.Html.HtmlElement);
                            if (titleElement.Nodes != null && titleElement.Nodes.Count > 0)
                            {
                                foreach (MIL.Html.HtmlNode t in titleElement.Nodes)
                                {
                                    if (t.IsText())
                                    {
                                        title += (t as MIL.Html.HtmlText).Text;
                                    }
                                    else if (t.IsElement() && (t as MIL.Html.HtmlElement).Name.ToLower() == "em")
                                    {
                                        title += (t as MIL.Html.HtmlElement).Text;
                                    }
                                }
                            }
                            //string title = ((titleNodes[0] as MIL.Html.HtmlElement).Nodes[0] as MIL.Html.HtmlElement).Text;
                            string href = ((titleNodes[0] as MIL.Html.HtmlElement).Nodes[0] as MIL.Html.HtmlElement).Attributes["href"].Value;

                            mri.Title = title;
                            mri.InfoSource = href;
                        }
                        #endregion
                        #region 来源，发表时间与内容简介
                        MIL.Html.HtmlNodeCollection authorNodes = (n as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "c-author");
                        if (authorNodes != null && authorNodes.Count > 0)
                        {
                            string author = (authorNodes[0] as MIL.Html.HtmlElement).Text;
                            //string context = ((authorNodes[0] as MIL.Html.HtmlElement).Parent as MIL.Html.HtmlElement).Text;
                            string context = "";
                            MIL.Html.HtmlElement contextElement = ((authorNodes[0] as MIL.Html.HtmlElement).Parent as MIL.Html.HtmlElement);
                            if (contextElement.Nodes != null && contextElement.Nodes.Count > 0)
                            {
                                foreach (MIL.Html.HtmlNode c in contextElement.Nodes)
                                {
                                    if (c.IsText())
                                    {
                                        context += (c as MIL.Html.HtmlText).Text;
                                    }
                                    else if (c.IsElement() && (c as MIL.Html.HtmlElement).Name.ToLower() == "em")
                                    {
                                        context += (c as MIL.Html.HtmlElement).Text;
                                    }
                                }
                            }
                            mri.Contexts = context;

                            byte[] space = new byte[] { 0xc2, 0xa0 };
                            string UTFSpace = Encoding.GetEncoding("UTF-8").GetString(space);

                            if (author.IndexOf(UTFSpace) >= 0)
                            {
                                string source = author.Substring(0, author.IndexOf(UTFSpace)).Trim();
                                mri.ReleaseName = source;
                                string date = author.Substring(author.IndexOf(UTFSpace) + 1).Trim();
                                if (date.IndexOf('年') >= 0)
                                {
                                    //南方网  2015年03月09日 11:51
                                    date = date.Replace('年', '-').Replace('月', '-').Replace("日", "");
                                }
                                else
                                {
                                    //国土资源部  5小时前 
                                    date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd");
                                }
                                mri.ReleaseDate = date;

                            }
                            else
                            {
                                if (author.IndexOf(' ') >= 0)
                                {
                                    string source = author.Substring(0, author.IndexOf(' '));
                                    mri.ReleaseName = source;
                                    string date = author.Substring(author.IndexOf(' ') + 1).Trim();
                                    if (date.IndexOf('年') >= 0)
                                    {
                                        //南方网  2015年03月09日 11:51
                                        date = date.Replace('年', '-').Replace('月', '-').Replace("日", "");
                                    }
                                    else
                                    {
                                        //国土资源部  5小时前 
                                        date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd");
                                    }
                                    mri.ReleaseDate = date;
                                }
                            }
                        }
                        #endregion
                        #region 快照
                        MIL.Html.HtmlNodeCollection cacheNodes = (n as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "c-cache");
                        if (cacheNodes != null && cacheNodes.Count > 0)
                        {
                            string cache = (cacheNodes[0] as MIL.Html.HtmlElement).Attributes["href"].Value;
                            mri.Snapshot = cache;
                        }
                        #endregion
                        #region 其他杂项
                        mri.KeyWords = keyword;
                        mri.CollectDate = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        mri.Kid = kid;
                        mri.Sheng = "";
                        mri.Shi = "";
                        mri.Xian = "";
                        //mri.WebName = "百度";
                        if (!string.IsNullOrEmpty(mri.ReleaseName))
                        {
                            mri.WebName = mri.ReleaseName;
                        }
                        else
                        {
                            mri.WebName = "主流媒体";
                        }
                        mri.Pid = 4;
                        //mri.Part = GetParts(mri.Contexts);
                        mri.Comments = (int)WebSourceType.BaiduNews;
                        mri.Reposts = 0;
                        #endregion
                        #region 2015.8.13 新增获取网址正文
                        if (!string.IsNullOrEmpty(mri.InfoSource))
                        {
                            string strContexts = HtmlUtil.getHtml(mri.InfoSource, "");
                            string noHtmlContexts = HtmlUtil.NoHTML(strContexts);
                            //分析关键字前100，后50个字符
                            string formatContexts = GetContexts(noHtmlContexts, keyword);
                            if (!string.IsNullOrEmpty(formatContexts))
                            {
                                mri.Contexts = formatContexts;
                            }
                        }
                        #endregion
                        #region 报告进度
                        OnReportCactchProcess(mri);
                        #endregion
                        webDatas.Add(mri);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Comm.WriteErrorLog("解析百度新闻网页时报错：" + ex.Message);
                Comm.WriteErrorLog(ex.StackTrace);
            }

            return webDatas;

        }

        /// <summary>
        /// 解析bing的新闻搜索页面
        /// </summary>
        /// <param name="html"></param>
        /// <param name="keyword"></param>
        /// <param name="kid"></param>
        /// <returns></returns>
        public List<ModelReleaseInfo> ParseBingNews(string html, string keyword, int kid)
        {
            List<ModelReleaseInfo> webDatas = new List<ModelReleaseInfo>();
            try
            {
                #region 解析网站源码
                MIL.Html.HtmlDocument doc = MIL.Html.HtmlDocument.Create(html);
                MIL.Html.HtmlNodeCollection nodes = doc.Nodes.FindByAttributeNameValue("class", "NewsResultSet clearfix", true);
                foreach (MIL.Html.HtmlNode n in nodes)
                {
                    if ((n is MIL.Html.HtmlElement) && (n as MIL.Html.HtmlElement).Nodes != null)
                    {
                        MIL.Html.HtmlNodeCollection resultNodes = (n as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "sn_r");
                        if (resultNodes != null && resultNodes.Count > 0)
                        {
                            foreach (MIL.Html.HtmlNode result in resultNodes)
                            {
                                if ((result is MIL.Html.HtmlElement) && (result as MIL.Html.HtmlElement).Nodes != null)
                                {
                                    ModelReleaseInfo mri = new ModelReleaseInfo();
                                    #region 标题与超链
                                    MIL.Html.HtmlNodeCollection titleNodes = (result as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "newstitle");
                                    if (titleNodes != null && titleNodes.Count > 0)
                                    {
                                        //title中包含<strong>节点，需要进行处理
                                        string title = "";
                                        MIL.Html.HtmlElement titleElement = ((titleNodes[0] as MIL.Html.HtmlElement).Nodes[0] as MIL.Html.HtmlElement);
                                        if (titleElement.Nodes != null && titleElement.Nodes.Count > 0)
                                        {
                                            foreach (MIL.Html.HtmlNode t in titleElement.Nodes)
                                            {
                                                if (t.IsText())
                                                {
                                                    title += (t as MIL.Html.HtmlText).Text;
                                                }
                                                else if (t.IsElement() && (t as MIL.Html.HtmlElement).Name.ToLower() == "strong")
                                                {
                                                    title += (t as MIL.Html.HtmlElement).Text;
                                                }
                                            }
                                        }
                                        string href = ((titleNodes[0] as MIL.Html.HtmlElement).Nodes[0] as MIL.Html.HtmlElement).Attributes["href"].Value;

                                        mri.Title = title;
                                        mri.InfoSource = href;
                                    }
                                    #endregion
                                    #region 来源，发表时间与内容简介
                                    MIL.Html.HtmlNodeCollection txtNodes = (result as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "sn_txt");
                                    if (txtNodes != null && txtNodes.Count > 0)
                                    {
                                        if ((txtNodes[0] is MIL.Html.HtmlElement) && (txtNodes[0] as MIL.Html.HtmlElement).Nodes != null)
                                        {
                                            MIL.Html.HtmlNodeCollection contentNodes = (txtNodes[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "sn_snip", true);
                                            if (contentNodes != null && contentNodes.Count > 0)
                                            {
                                                string context = "";
                                                if (contentNodes[0].IsElement())
                                                {
                                                    foreach (MIL.Html.HtmlNode c in (contentNodes[0] as MIL.Html.HtmlElement).Nodes)
                                                    {
                                                        if (c.IsText())
                                                        {
                                                            context += (c as MIL.Html.HtmlText).Text;
                                                        }
                                                        else if (c.IsElement() && (c as MIL.Html.HtmlElement).Name.ToLower() == "strong")
                                                        {
                                                            context += (c as MIL.Html.HtmlElement).Text;
                                                        }
                                                    }
                                                }
                                                mri.Contexts = context;
                                            }
                                            MIL.Html.HtmlNodeCollection sourceNodes = (txtNodes[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "sn_ST", true);
                                            if (sourceNodes != null && sourceNodes.Count > 0)
                                            {
                                                if (sourceNodes[0].IsElement())
                                                {
                                                    MIL.Html.HtmlNodeCollection srcNodes = (sourceNodes[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "sn_src");
                                                    if (srcNodes != null && srcNodes.Count > 0)
                                                    {
                                                        if (srcNodes[0].IsElement())
                                                        {
                                                            mri.ReleaseName = (srcNodes[0] as MIL.Html.HtmlElement).Text;
                                                        }
                                                        else if (srcNodes[0].IsText())
                                                        {
                                                            mri.ReleaseName = (srcNodes[0] as MIL.Html.HtmlText).Text;
                                                        }
                                                    }
                                                    MIL.Html.HtmlNodeCollection dateNodes = (sourceNodes[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "sn_tm");
                                                    if (dateNodes != null && dateNodes.Count > 0)
                                                    {
                                                        if (dateNodes[0].IsElement())
                                                        {
                                                            mri.ReleaseDate = FormateDate((dateNodes[0] as MIL.Html.HtmlElement).Text);
                                                        }
                                                        else if (dateNodes[0].IsText())
                                                        {
                                                            mri.ReleaseDate = FormateDate((dateNodes[0] as MIL.Html.HtmlText).Text);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    #endregion
                                    #region 快照
                                    mri.Snapshot = "";
                                    #endregion
                                    #region 其他杂项
                                    mri.KeyWords = keyword;
                                    mri.CollectDate = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                    mri.Kid = kid;
                                    mri.Sheng = "";
                                    mri.Shi = "";
                                    mri.Xian = "";
                                    //mri.WebName = "必应";
                                    if (!string.IsNullOrEmpty(mri.ReleaseName))
                                    {
                                        mri.WebName = mri.ReleaseName;
                                    }
                                    else
                                    {
                                        mri.WebName = "主流媒体";
                                    }
                                    mri.Pid = 4;
                                    //mri.Part = GetParts(mri.Contexts);
                                    mri.Comments = (int)WebSourceType.BingNews;
                                    mri.Reposts = 0;
                                    #endregion

                                    #region 2015.8.13 新增获取网址正文
                                    if (!string.IsNullOrEmpty(mri.InfoSource))
                                    {
                                        string strContexts = HtmlUtil.getHtml(mri.InfoSource, "");
                                        string noHtmlContexts = HtmlUtil.NoHTML(strContexts);
                                        //分析关键字前100，后50个字符
                                        string formatContexts = GetContexts(noHtmlContexts, keyword);
                                        if (!string.IsNullOrEmpty(formatContexts))
                                        {
                                            mri.Contexts = formatContexts;
                                        }
                                    }
                                    #endregion
                                    #region 报告进度
                                    OnReportCactchProcess(mri);
                                    #endregion


                                    webDatas.Add(mri);
                                }
                            }
                        }
                    }

                }
                #endregion
            }
            catch (Exception ex)
            {
                Comm.WriteErrorLog("解析必应新闻网页时报错：" + ex.Message);
                Comm.WriteErrorLog(ex.StackTrace);
            }

            return webDatas;

        }

        public List<ModelReleaseInfo> ParseSogouNews(string html, string keyword, int kid)
        {
            List<ModelReleaseInfo> webDatas = new List<ModelReleaseInfo>();
            try
            {
                #region 解析网站源码
                MIL.Html.HtmlDocument doc = MIL.Html.HtmlDocument.Create(html);
                MIL.Html.HtmlNodeCollection nodes = doc.Nodes.FindByAttributeNameValue("class", "results", true);
                foreach (MIL.Html.HtmlNode n in nodes)
                {
                    if ((n is MIL.Html.HtmlElement) && (n as MIL.Html.HtmlElement).Nodes != null)
                    {
                        foreach (MIL.Html.HtmlNode news in (n as MIL.Html.HtmlElement).Nodes)
                        {
                            if (news.IsElement() && (news as MIL.Html.HtmlElement).Name == "div")
                            {
                                ModelReleaseInfo mri = new ModelReleaseInfo();

                                if (news.IsElement() && (news as MIL.Html.HtmlElement).Attributes["class"].Value == "rb")
                                {
                                    #region 标题与超链、来源，发表时间
                                    MIL.Html.HtmlNodeCollection titleNodes = (news as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "pt");
                                    if (titleNodes != null && titleNodes.Count > 0)
                                    {
                                        if (titleNodes[0].IsElement() && (titleNodes[0] as MIL.Html.HtmlElement).Nodes.Count > 0)
                                        {
                                            MIL.Html.HtmlNodeCollection herfNodes = (titleNodes[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "pp");
                                            if (herfNodes != null && herfNodes.Count > 0)
                                            {
                                                string title = "";
                                                if (herfNodes[0].IsElement() && (herfNodes[0] as MIL.Html.HtmlElement).Nodes != null && (herfNodes[0] as MIL.Html.HtmlElement).Nodes.Count > 0)
                                                {
                                                    //<b> </b>
                                                    MIL.Html.HtmlNode tNode = herfNodes[0];
                                                    if (((herfNodes[0] as MIL.Html.HtmlElement).Nodes[0] as MIL.Html.HtmlElement).Name == "b")
                                                    {
                                                        tNode = (herfNodes[0] as MIL.Html.HtmlElement).Nodes[0];
                                                    }
                                                    foreach (MIL.Html.HtmlNode t in (tNode as MIL.Html.HtmlElement).Nodes)
                                                    {
                                                        if (t.IsText())
                                                        {
                                                            title += (t as MIL.Html.HtmlText).Text;
                                                        }
                                                        else if (t.IsElement() && (t as MIL.Html.HtmlElement).Name.ToLower() == "em")
                                                        {
                                                            title += (t as MIL.Html.HtmlElement).Text;
                                                        }
                                                    }
                                                }
                                                //title中包含<em>节点，需要进行处理                                        
                                                string href = ((titleNodes[0] as MIL.Html.HtmlElement).Nodes[0] as MIL.Html.HtmlElement).Attributes["href"].Value;
                                                mri.Title = title;
                                                mri.InfoSource = href;
                                            }
                                        }
                                        MIL.Html.HtmlNodeCollection citeNodes = (titleNodes[0] as MIL.Html.HtmlElement).Nodes.FindByName("cite");
                                        if (citeNodes != null && citeNodes.Count > 0)
                                        {
                                            if (citeNodes[0].IsElement() && (citeNodes[0] as MIL.Html.HtmlElement).Attributes != null && (citeNodes[0] as MIL.Html.HtmlElement).Attributes["title"] != null)
                                            {
                                                string srcTitle = (citeNodes[0] as MIL.Html.HtmlElement).Attributes["title"].Value;
                                                mri.ReleaseName = srcTitle;

                                                string srcTxt = "";
                                                if (citeNodes[0].IsElement())
                                                {
                                                    srcTxt = (citeNodes[0] as MIL.Html.HtmlElement).Text;
                                                }
                                                else if (citeNodes[0].IsText())
                                                {
                                                    srcTxt = (citeNodes[0] as MIL.Html.HtmlText).Text;
                                                }

                                                byte[] space = new byte[] { 0xc2, 0xa0 };
                                                string UTFSpace = Encoding.GetEncoding("UTF-8").GetString(space);
                                                string txt = srcTxt.Replace(UTFSpace, " ").Trim().Replace(srcTitle, "").Trim();
                                                mri.ReleaseDate = FormateDate(txt);
                                            }

                                        }
                                    }
                                    #endregion
                                    #region 内容简介
                                    MIL.Html.HtmlNodeCollection contextNodes = (news as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "ft");
                                    if (contextNodes != null && contextNodes.Count > 0)
                                    {
                                        string context = "";
                                        MIL.Html.HtmlElement contextElement = (contextNodes[0] as MIL.Html.HtmlElement);
                                        if (contextElement.Nodes != null && contextElement.Nodes.Count > 0)
                                        {
                                            foreach (MIL.Html.HtmlNode c in contextElement.Nodes)
                                            {
                                                if (c.IsText())
                                                {
                                                    context += (c as MIL.Html.HtmlText).Text;
                                                }
                                                else if (c.IsElement() && (c as MIL.Html.HtmlElement).Name.ToLower() == "em")
                                                {
                                                    context += (c as MIL.Html.HtmlElement).Text;
                                                }
                                            }
                                        }
                                        mri.Contexts = context;
                                    }

                                    #endregion
                                }

                                #region 其他杂项
                                mri.Snapshot = "";
                                mri.KeyWords = keyword;
                                mri.CollectDate = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                mri.Kid = kid;
                                mri.Sheng = "";
                                mri.Shi = "";
                                mri.Xian = "";
                                if (!string.IsNullOrEmpty(mri.ReleaseName))
                                {
                                    mri.WebName = mri.ReleaseName;
                                }
                                else
                                {
                                    mri.WebName = "主流媒体";
                                }
                                mri.Pid = 4;
                                //mri.Part = GetParts(mri.Contexts);
                                mri.Comments = (int)WebSourceType.SogouNews;
                                mri.Reposts = 0;
                                #endregion

                                #region 2015.8.13 新增获取网址正文
                                if (!string.IsNullOrEmpty(mri.InfoSource))
                                {
                                    string strContexts = HtmlUtil.getHtml(mri.InfoSource, "");
                                    string noHtmlContexts = HtmlUtil.NoHTML(strContexts);
                                    //分析关键字前100，后50个字符
                                    string formatContexts = GetContexts(noHtmlContexts, keyword);
                                    if (!string.IsNullOrEmpty(formatContexts))
                                    {
                                        mri.Contexts = formatContexts;
                                    }
                                }
                                #endregion
                                #region 报告进度
                                OnReportCactchProcess(mri);
                                #endregion


                                webDatas.Add(mri);
                            }
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Comm.WriteErrorLog("解析搜狗博客搜索页时报错：" + ex.Message);
                Comm.WriteErrorLog(ex.StackTrace);
            }

            return webDatas;

        }

        /// <summary>
        /// 中搜
        /// </summary>
        /// <param name="html"></param>
        /// <param name="keyword"></param>
        /// <param name="kid"></param>
        /// <returns></returns>
        public List<ModelReleaseInfo> ParseZhongsouNews(string html, string keyword, int kid)
        {
            List<ModelReleaseInfo> webDatas = new List<ModelReleaseInfo>();
            try
            {
                #region 解析网站源码
                MIL.Html.HtmlDocument doc = MIL.Html.HtmlDocument.Create(html);
                MIL.Html.HtmlNodeCollection nodes = doc.Nodes.FindByAttributeNameValue("class", "content-net-ul content-infor-ul", true);
                foreach (MIL.Html.HtmlNode n in nodes)
                {
                    if ((n is MIL.Html.HtmlElement) && (n as MIL.Html.HtmlElement).Nodes != null)
                    {
                        MIL.Html.HtmlNodeCollection resultNodes = (n as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "clearfix");
                        if (resultNodes != null && resultNodes.Count > 0)
                        {
                            foreach (MIL.Html.HtmlNode result in resultNodes)
                            {
                                if ((result is MIL.Html.HtmlElement) && (result as MIL.Html.HtmlElement).Nodes != null)
                                {
                                    ModelReleaseInfo mri = new ModelReleaseInfo();
                                    #region 标题、超链、来源、发表时间
                                    MIL.Html.HtmlNodeCollection topNodes = (result as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "h3-wrap");
                                    if (topNodes != null && topNodes.Count > 0)
                                    {
                                        MIL.Html.HtmlNodeCollection titleNodes = (topNodes[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "h3-zx");
                                        if (titleNodes != null && titleNodes.Count > 0)
                                        {
                                            //title中包含<strong>节点，需要进行处理
                                            if (titleNodes[0].IsElement())
                                            {
                                                MIL.Html.HtmlNodeCollection titleNode2 = (titleNodes[0] as MIL.Html.HtmlElement).Nodes.FindByName("a");
                                                if (titleNode2 != null && titleNode2.Count > 0 && titleNode2[0].IsElement())
                                                {
                                                    string title = "";
                                                    foreach (MIL.Html.HtmlNode t in (titleNode2[0] as MIL.Html.HtmlElement).Nodes)
                                                    {
                                                        if (t.IsText())
                                                        {
                                                            title += (t as MIL.Html.HtmlText).Text;
                                                        }
                                                        else if (t.IsElement() && (t as MIL.Html.HtmlElement).Name.ToLower() == "em")
                                                        {
                                                            title += (t as MIL.Html.HtmlElement).Text;
                                                        }
                                                    }
                                                    string href = (titleNode2[0] as MIL.Html.HtmlElement).Attributes["href"].Value;

                                                    mri.Title = title;
                                                    mri.InfoSource = href;
                                                }
                                            }
                                        }
                                        MIL.Html.HtmlNodeCollection publishNodes = (topNodes[0] as MIL.Html.HtmlElement).Nodes.FindByName("font");
                                        if (publishNodes != null && publishNodes.Count > 0)
                                        {
                                            if (publishNodes[0].IsElement())
                                            {
                                                MIL.Html.HtmlNodeCollection publish = (publishNodes[0] as MIL.Html.HtmlElement).Nodes.FindByName("nobr");
                                                if (publish != null && publish.Count > 0)
                                                {
                                                    string txt = "";
                                                    if (publish[0].IsElement())
                                                    {
                                                        txt = (publish[0] as MIL.Html.HtmlElement).Text;
                                                    }
                                                    else if (publish[0].IsText())
                                                    {
                                                        txt = (publish[0] as MIL.Html.HtmlText).Text;
                                                    }

                                                    byte[] space = new byte[] { 0xc2, 0xa0 };
                                                    string UTFSpace = Encoding.GetEncoding("UTF-8").GetString(space);
                                                    txt = txt.Replace(UTFSpace, " ").Trim();
                                                    if (txt.IndexOf(" ") > 0)
                                                    {
                                                        mri.ReleaseName = txt.Substring(0, txt.IndexOf(" ")).Trim();
                                                        txt = txt.Substring(txt.IndexOf(" ") + 1);
                                                    }
                                                    mri.ReleaseDate = FormateDate(txt);
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                    #region 内容简介
                                    MIL.Html.HtmlNodeCollection txtNodes = (result as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "img-info noimg-txt");
                                    if (txtNodes == null || txtNodes.Count == 0)
                                    {
                                        txtNodes = (result as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "img-info clearfix");
                                    }
                                    if (txtNodes != null && txtNodes.Count > 0)
                                    {
                                        if ((txtNodes[0] is MIL.Html.HtmlElement) && (txtNodes[0] as MIL.Html.HtmlElement).Nodes != null)
                                        {
                                            MIL.Html.HtmlNodeCollection contentNodes = (txtNodes[0] as MIL.Html.HtmlElement).Nodes.FindByName("p");
                                            if (contentNodes != null && contentNodes.Count > 0)
                                            {
                                                string context = "";
                                                if (contentNodes[0].IsElement())
                                                {
                                                    foreach (MIL.Html.HtmlNode c in (contentNodes[0] as MIL.Html.HtmlElement).Nodes)
                                                    {
                                                        if (c.IsText())
                                                        {
                                                            context += (c as MIL.Html.HtmlText).Text;
                                                        }
                                                        else if (c.IsElement() && (c as MIL.Html.HtmlElement).Name.ToLower() == "em")
                                                        {
                                                            context += (c as MIL.Html.HtmlElement).Text;
                                                        }
                                                    }
                                                }
                                                mri.Contexts = context;
                                            }
                                        }
                                    }

                                    #endregion
                                    #region 快照
                                    mri.Snapshot = "";
                                    #endregion
                                    #region 其他杂项
                                    mri.KeyWords = keyword;
                                    mri.CollectDate = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                    mri.Kid = kid;
                                    mri.Sheng = "";
                                    mri.Shi = "";
                                    mri.Xian = "";
                                    //mri.WebName = "必应";
                                    if (!string.IsNullOrEmpty(mri.ReleaseName))
                                    {
                                        mri.WebName = mri.ReleaseName;
                                    }
                                    else
                                    {
                                        mri.WebName = "主流媒体";
                                    }
                                    mri.Pid = 4;
                                    //mri.Part = GetParts(mri.Contexts);
                                    mri.Comments = (int)WebSourceType.ZhongsouNews;
                                    mri.Reposts = 0;
                                    #endregion

                                    #region 2015.8.13 新增获取网址正文
                                    if (!string.IsNullOrEmpty(mri.InfoSource))
                                    {
                                        string strContexts = HtmlUtil.getHtml(mri.InfoSource, "");
                                        string noHtmlContexts = HtmlUtil.NoHTML(strContexts);
                                        //分析关键字前100，后50个字符
                                        string formatContexts = GetContexts(noHtmlContexts, keyword);
                                        if (!string.IsNullOrEmpty(formatContexts))
                                        {
                                            mri.Contexts = formatContexts;
                                        }
                                    }
                                    #endregion
                                    #region 报告进度
                                    OnReportCactchProcess(mri);
                                    #endregion


                                    webDatas.Add(mri);
                                }
                            }
                        }
                    }

                }
                #endregion
            }
            catch (Exception ex)
            {
                Comm.WriteErrorLog("解析搜狗博客搜索页时报错：" + ex.Message);
                Comm.WriteErrorLog(ex.StackTrace);
            }

            return webDatas;

        }

        public List<ModelReleaseInfo> ParseHaosouNews(string html, string keyword, int kid)
        {
            List<ModelReleaseInfo> webDatas = new List<ModelReleaseInfo>();
            try
            {
                #region 解析网站源码
                MIL.Html.HtmlDocument doc = MIL.Html.HtmlDocument.Create(html);
                MIL.Html.HtmlNodeCollection nodes = doc.Nodes.FindByAttributeNameValue("class", "result", true);
                if (nodes != null && nodes.Count > 0 && nodes[0].IsElement())
                {
                    MIL.Html.HtmlNodeCollection resultNodes = (nodes[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "res-list", true);
                    if (resultNodes == null || resultNodes.Count == 0)
                    {
                        resultNodes = (nodes[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "res-list hasimg", true);
                    }
                    if (resultNodes != null && resultNodes.Count > 0)
                    {
                        foreach (MIL.Html.HtmlNode result in resultNodes)
                        {
                            //<div class="res-rich so-rich-news clearfix">
                            if ((result is MIL.Html.HtmlElement) && (result as MIL.Html.HtmlElement).Nodes != null)
                            {
                                ModelReleaseInfo mri = new ModelReleaseInfo();
                                #region 标题、超链
                                MIL.Html.HtmlNodeCollection titleNodes = (result as MIL.Html.HtmlElement).Nodes.FindByName("h3");
                                if (titleNodes != null && titleNodes.Count > 0)
                                {
                                    if (titleNodes[0].IsElement())
                                    {
                                        MIL.Html.HtmlNodeCollection aNodes = (titleNodes[0] as MIL.Html.HtmlElement).Nodes.FindByName("a");
                                        if (aNodes != null && aNodes.Count > 0 && aNodes[0].IsElement())
                                        {
                                            string title = "";
                                            foreach (MIL.Html.HtmlNode t in (aNodes[0] as MIL.Html.HtmlElement).Nodes)
                                            {
                                                if (t.IsText())
                                                {
                                                    title += (t as MIL.Html.HtmlText).Text;
                                                }
                                                else if (t.IsElement() && (t as MIL.Html.HtmlElement).Name.ToLower() == "em")
                                                {
                                                    title += (t as MIL.Html.HtmlElement).Text;
                                                }
                                            }
                                            string href = (aNodes[0] as MIL.Html.HtmlElement).Attributes["href"].Value;

                                            mri.Title = title;
                                            mri.InfoSource = href;
                                        }
                                    }
                                }
                                #endregion
                                #region 内容简介
                                MIL.Html.HtmlNodeCollection contentNodes = (result as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "content", true);
                                if (contentNodes != null && contentNodes.Count > 0)
                                {
                                    string text = "";
                                    foreach (MIL.Html.HtmlNode c in (contentNodes[0] as MIL.Html.HtmlElement).Nodes)
                                    {
                                        if (c.IsText())
                                        {
                                            text += (c as MIL.Html.HtmlText).Text;
                                        }
                                        else if (c.IsElement() && (c as MIL.Html.HtmlElement).Name.ToLower() == "em")
                                        {
                                            text += (c as MIL.Html.HtmlElement).Text;
                                        }
                                    }
                                    mri.Contexts = text;
                                }
                                #endregion
                                #region 发布人，时间
                                MIL.Html.HtmlNodeCollection newsinfoNodes = (result as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "newsinfo");
                                if (newsinfoNodes != null && newsinfoNodes.Count > 0)
                                {
                                    MIL.Html.HtmlNodeCollection sitenameNodes = (newsinfoNodes[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "sitename");
                                    if (sitenameNodes != null && sitenameNodes.Count > 0)
                                    {
                                        if (sitenameNodes[0].IsElement())
                                        {
                                            mri.ReleaseName = (sitenameNodes[0] as MIL.Html.HtmlElement).Text;
                                        }
                                    }

                                    MIL.Html.HtmlNodeCollection posttimeNodes = (newsinfoNodes[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "posttime");
                                    if (posttimeNodes != null && posttimeNodes.Count > 0)
                                    {
                                        if (posttimeNodes[0].IsElement())
                                        {
                                            if ((posttimeNodes[0] as MIL.Html.HtmlElement).Attributes != null && (posttimeNodes[0] as MIL.Html.HtmlElement).Attributes["title"] != null)
                                            {
                                                mri.ReleaseDate = (posttimeNodes[0] as MIL.Html.HtmlElement).Attributes["title"].Value;
                                            }
                                        }
                                    }
                                }
                                #endregion
                                #region 其他杂项
                                mri.KeyWords = keyword;
                                mri.CollectDate = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                mri.Kid = kid;
                                mri.Sheng = "";
                                mri.Shi = "";
                                mri.Xian = "";
                                if (!string.IsNullOrEmpty(mri.ReleaseName))
                                {
                                    mri.WebName = mri.ReleaseName;
                                }
                                else
                                {
                                    mri.WebName = "主流媒体";
                                }
                                mri.Pid = 4;
                                //mri.Part = GetParts(mri.Contexts);
                                mri.Comments = (int)WebSourceType.HaosouNews;
                                mri.Reposts = 0;
                                #endregion

                                #region 2015.8.13 新增获取网址正文
                                if (!string.IsNullOrEmpty(mri.InfoSource))
                                {
                                    string strContexts = HtmlUtil.getHtml(mri.InfoSource, "");
                                    string noHtmlContexts = HtmlUtil.NoHTML(strContexts);
                                    //分析关键字前100，后50个字符
                                    string formatContexts = GetContexts(noHtmlContexts, keyword);
                                    if (!string.IsNullOrEmpty(formatContexts))
                                    {
                                        mri.Contexts = formatContexts;
                                    }
                                }
                                #endregion
                                #region 报告进度
                                OnReportCactchProcess(mri);
                                #endregion


                                webDatas.Add(mri);
                            }
                        }
                    }

                }
                #endregion
            }
            catch (Exception ex)
            {
                Comm.WriteErrorLog("解析好搜新闻搜索时报错：" + ex.Message);
                Comm.WriteErrorLog(ex.StackTrace);
            }

            return webDatas;

        }

        #endregion

        #region 全网
        public List<ModelReleaseInfo> ParseBingWeb(string html, string keyword, int kid)
        {
            List<ModelReleaseInfo> webDatas = new List<ModelReleaseInfo>();
            try
            {
                #region 解析网站源码
                MIL.Html.HtmlDocument doc = MIL.Html.HtmlDocument.Create(html);
                MIL.Html.HtmlNodeCollection nodes = doc.Nodes.FindByAttributeNameValue("id", "b_results", true);
                foreach (MIL.Html.HtmlNode n in nodes)
                {
                    if ((n is MIL.Html.HtmlElement) && (n as MIL.Html.HtmlElement).Nodes != null)
                    {
                        MIL.Html.HtmlNodeCollection resultNodes = (n as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "b_algo");
                        if (resultNodes != null && resultNodes.Count > 0)
                        {
                            foreach (MIL.Html.HtmlNode result in resultNodes)
                            {
                                if ((result is MIL.Html.HtmlElement) && (result as MIL.Html.HtmlElement).Nodes != null)
                                {
                                    ModelReleaseInfo mri = new ModelReleaseInfo();
                                    #region 标题与超链
                                    MIL.Html.HtmlNodeCollection titleNodes = (result as MIL.Html.HtmlElement).Nodes.FindByName("h2");
                                    if (titleNodes != null && titleNodes.Count > 0)
                                    {
                                        //title中包含<strong>节点，需要进行处理
                                        string title = "";
                                        MIL.Html.HtmlElement titleElement = ((titleNodes[0] as MIL.Html.HtmlElement).Nodes[0] as MIL.Html.HtmlElement);
                                        if (titleElement.Nodes != null && titleElement.Nodes.Count > 0)
                                        {
                                            foreach (MIL.Html.HtmlNode t in titleElement.Nodes)
                                            {
                                                if (t.IsText())
                                                {
                                                    title += (t as MIL.Html.HtmlText).Text;
                                                }
                                                else if (t.IsElement() && (t as MIL.Html.HtmlElement).Name.ToLower() == "strong")
                                                {
                                                    title += (t as MIL.Html.HtmlElement).Text;
                                                }
                                            }
                                        }
                                        string href = ((titleNodes[0] as MIL.Html.HtmlElement).Nodes[0] as MIL.Html.HtmlElement).Attributes["href"].Value;

                                        mri.Title = title;
                                        mri.InfoSource = href;
                                    }
                                    #endregion
                                    #region 来源，发表时间与内容简介
                                    MIL.Html.HtmlNodeCollection txtNodes = (result as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "b_caption");
                                    if (txtNodes != null && txtNodes.Count > 0)
                                    {
                                        if ((txtNodes[0] is MIL.Html.HtmlElement) && (txtNodes[0] as MIL.Html.HtmlElement).Nodes != null)
                                        {
                                            foreach (MIL.Html.HtmlNode t in (txtNodes[0] as MIL.Html.HtmlElement).Nodes)
                                            {
                                                if (t.IsElement() && (t as MIL.Html.HtmlElement).Name == "p")
                                                {
                                                    string context = "";
                                                    foreach (MIL.Html.HtmlNode c in (t as MIL.Html.HtmlElement).Nodes)
                                                    {
                                                        if (c.IsText())
                                                        {
                                                            context += (c as MIL.Html.HtmlText).Text;
                                                        }
                                                        else if (c.IsElement() && (c as MIL.Html.HtmlElement).Name.ToLower() == "strong")
                                                        {
                                                            context += (c as MIL.Html.HtmlElement).Text;
                                                        }
                                                    }
                                                    mri.Contexts = context;
                                                }

                                                if (t.IsElement() && (t as MIL.Html.HtmlElement).Name == "div"
                                                    && (t as MIL.Html.HtmlElement).Attributes != null)
                                                {
                                                    MIL.Html.HtmlAttribute attr = (t as MIL.Html.HtmlElement).Attributes.FindByName("class");
                                                    if (attr != null && attr.Value == "b_attribution")
                                                    {
                                                        mri.ReleaseDate = FormateDate((t as MIL.Html.HtmlElement).Text);

                                                        if ((t as MIL.Html.HtmlElement).Nodes != null && (t as MIL.Html.HtmlElement).Nodes.Count > 0)
                                                        {
                                                            MIL.Html.HtmlNodeCollection releaseNodes = (t as MIL.Html.HtmlElement).Nodes.FindByName("cite");
                                                            if (releaseNodes != null && releaseNodes.Count > 0)
                                                            {
                                                                if (releaseNodes[0].IsElement())
                                                                {
                                                                    mri.ReleaseName = (releaseNodes[0] as MIL.Html.HtmlElement).Text;
                                                                }
                                                                else if (releaseNodes[0].IsText())
                                                                {
                                                                    mri.ReleaseName = (releaseNodes[0] as MIL.Html.HtmlText).Text;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    #endregion
                                    #region 快照
                                    mri.Snapshot = "";
                                    #endregion
                                    #region 其他杂项
                                    mri.KeyWords = keyword;
                                    mri.CollectDate = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                    mri.Kid = kid;
                                    mri.Sheng = "";
                                    mri.Shi = "";
                                    mri.Xian = "";
                                    //mri.WebName = "必应";
                                    if (!string.IsNullOrEmpty(mri.ReleaseName))
                                    {
                                        mri.WebName = mri.ReleaseName;
                                    }
                                    else
                                    {
                                        mri.WebName = "全网";
                                    }
                                    mri.Pid = 0;
                                    //mri.Part = GetParts(mri.Contexts);
                                    mri.Comments = (int)WebSourceType.BingWeb;
                                    mri.Reposts = 0;
                                    #endregion
                                    #region 2015.8.13 新增获取网址正文
                                    if (!string.IsNullOrEmpty(mri.InfoSource))
                                    {
                                        string strContexts = HtmlUtil.getHtml(mri.InfoSource, "");
                                        string noHtmlContexts = HtmlUtil.NoHTML(strContexts);
                                        //分析关键字前100，后50个字符
                                        string formatContexts = GetContexts(noHtmlContexts, keyword);
                                        if (!string.IsNullOrEmpty(formatContexts))
                                        {
                                            mri.Contexts = formatContexts;
                                        }
                                    }
                                    #endregion
                                    #region 报告进度
                                    OnReportCactchProcess(mri);
                                    #endregion
                                    webDatas.Add(mri);
                                }
                            }
                        }
                    }

                }
                #endregion
            }
            catch (Exception ex)
            {
                Comm.WriteErrorLog("解析必应新闻网页时报错：" + ex.Message);
                Comm.WriteErrorLog(ex.StackTrace);
            }

            return webDatas;

        }

        /// <summary>
        /// 搜索百度网页
        /// </summary>
        /// <param name="html"></param>
        /// <param name="keyword"></param>
        /// <param name="kid"></param>
        /// <returns></returns>
        public List<ModelReleaseInfo> ParseBaiduWeb(string html, string keyword, int kid)
        {
            List<ModelReleaseInfo> webDatas = new List<ModelReleaseInfo>();
            try
            {
                #region 网页源码样例
                #endregion
                #region 解析网站源码
                //html = html.Replace("<em>", "").Replace("</em>", "");
                MIL.Html.HtmlDocument doc = MIL.Html.HtmlDocument.Create(html);
                MIL.Html.HtmlNodeCollection nodes = doc.Nodes.FindByAttributeNameValue("class", "result c-container ", true);
                foreach (MIL.Html.HtmlNode n in nodes)
                {
                    if ((n is MIL.Html.HtmlElement) && (n as MIL.Html.HtmlElement).Nodes != null)
                    {
                        ModelReleaseInfo mri = new ModelReleaseInfo();
                        #region 标题与超链
                        MIL.Html.HtmlNodeCollection titleNodes = (n as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "t");
                        if (titleNodes != null && titleNodes.Count > 0)
                        {
                            //title中包含<em>节点，需要进行处理
                            string title = "";
                            MIL.Html.HtmlElement titleElement = ((titleNodes[0] as MIL.Html.HtmlElement).Nodes[0] as MIL.Html.HtmlElement);
                            if (titleElement.Nodes != null && titleElement.Nodes.Count > 0)
                            {
                                foreach (MIL.Html.HtmlNode t in titleElement.Nodes)
                                {
                                    if (t.IsText())
                                    {
                                        title += (t as MIL.Html.HtmlText).Text;
                                    }
                                    else if (t.IsElement() && (t as MIL.Html.HtmlElement).Name.ToLower() == "em")
                                    {
                                        title += (t as MIL.Html.HtmlElement).Text;
                                    }
                                }
                            }
                            //string title = ((titleNodes[0] as MIL.Html.HtmlElement).Nodes[0] as MIL.Html.HtmlElement).Text;
                            string href = ((titleNodes[0] as MIL.Html.HtmlElement).Nodes[0] as MIL.Html.HtmlElement).Attributes["href"].Value;

                            mri.Title = title;
                            mri.InfoSource = href;
                            //百度跳转超链，需要二次解析
                            if (href.StartsWith("http://www.baidu.com/link?url="))
                            {
                                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(href);
                                req.AllowAutoRedirect = false;
                                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                                string loc = response.Headers["location"];
                                if (!string.IsNullOrEmpty(loc))
                                {
                                    mri.InfoSource = loc;
                                    if (loc.Trim().StartsWith("http://zhidao.baidu.com/link?url"))
                                    {
                                        string zhidaoHtml = HtmlUtil.HttpGet(href, Encoding.Default);
                                        MIL.Html.HtmlDocument doc2 = MIL.Html.HtmlDocument.Create(zhidaoHtml);
                                        MIL.Html.HtmlNodeCollection nodes2 = doc2.Nodes.FindByAttributeNameValue("rel", "canonical", true);
                                        if (nodes2 != null && nodes2.Count > 0)
                                        {
                                            if (nodes2[0].IsElement() && (nodes2[0] as MIL.Html.HtmlElement).Attributes != null && (nodes2[0] as MIL.Html.HtmlElement).Attributes["href"] != null)
                                            {
                                                mri.InfoSource = (nodes2[0] as MIL.Html.HtmlElement).Attributes["href"].Value;
                                            }
                                        }

                                    }
                                }
                            }
                            else if (href.Trim().StartsWith("http://zhidao.baidu.com/link?url"))
                            {
                                string zhidaoHtml = HtmlUtil.HttpGet(href, Encoding.Default);
                                MIL.Html.HtmlDocument doc2 = MIL.Html.HtmlDocument.Create(zhidaoHtml);
                                MIL.Html.HtmlNodeCollection nodes2 = doc2.Nodes.FindByAttributeNameValue("rel", "canonical", true);
                                if (nodes2 != null && nodes2.Count > 0)
                                {
                                    if (nodes2[0].IsElement() && (nodes2[0] as MIL.Html.HtmlElement).Attributes != null && (nodes2[0] as MIL.Html.HtmlElement).Attributes["href"] != null)
                                    {
                                        mri.InfoSource = (nodes2[0] as MIL.Html.HtmlElement).Attributes["href"].Value;
                                    }
                                }

                            }
                        }
                        #endregion
                        #region 发表时间与内容简介
                        MIL.Html.HtmlNodeCollection contextNodes = (n as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "c-abstract");
                        if (contextNodes != null && contextNodes.Count > 0)
                        {
                            #region 简介
                            string context = "";
                            string date = "";// DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            foreach (MIL.Html.HtmlNode c in (contextNodes[0] as MIL.Html.HtmlElement).Nodes)
                            {
                                if (c.IsElement() && (c as MIL.Html.HtmlElement).Name.ToLower() == "span")
                                {
                                    date = (c as MIL.Html.HtmlElement).Text;
                                }
                                else if (c.IsText())
                                {
                                    context += (c as MIL.Html.HtmlText).Text;
                                }
                                else if (c.IsElement() && (c as MIL.Html.HtmlElement).Name.ToLower() == "em")
                                {
                                    context += (c as MIL.Html.HtmlElement).Text;
                                }
                            }
                            mri.Contexts = context;
                            #endregion
                            #region 处理时间
                            if (string.IsNullOrEmpty(date))
                            {
                                date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            else
                            {
                                //<span class=" newTimeFactor_before_abs m">2009年9月6日&nbsp;-&nbsp;</span>
                                byte[] space = new byte[] { 0xc2, 0xa0 };
                                string UTFSpace = Encoding.GetEncoding("UTF-8").GetString(space);
                                date = date.Replace(UTFSpace, "").Trim();
                                if (date.EndsWith("-")) date = date.Substring(0, date.Length - 1);

                                if (date.EndsWith("前"))
                                {
                                    if (date.Contains("天前"))
                                    {
                                        int offset = int.Parse(date.Substring(0, date.IndexOf("天前")).Trim());
                                        date = DateTime.Now.AddDays(offset * -1).ToString("yyyy-MM-dd");
                                    }
                                    else if (date.Contains("小时前"))
                                    {
                                        int offset = int.Parse(date.Substring(0, date.IndexOf("小时前")).Trim());
                                        date = DateTime.Now.AddHours(offset * -1).ToString("yyyy-MM-dd");
                                    }
                                    else if (date.Contains("时前"))
                                    {
                                        int offset = int.Parse(date.Substring(0, date.IndexOf("时前")).Trim());
                                        date = DateTime.Now.AddHours(offset * -1).ToString("yyyy-MM-dd");
                                    }
                                    else if (date.Contains("分") && date.Contains("前"))
                                    {
                                        int offset = int.Parse(date.Substring(0, date.IndexOf("分")).Trim());
                                        date = DateTime.Now.AddMinutes(offset * -1).ToString("yyyy-MM-dd");
                                    }
                                }
                                else
                                {
                                    if (date.IndexOf('年') >= 0)
                                    {
                                        //南方网  2015年03月09日 11:51
                                        date = date.Replace('年', '-').Replace('月', '-').Replace("日", "");
                                    }
                                    else
                                    {
                                        if (date.IndexOf('月') >= 0)
                                        {
                                            date = DateTime.Now.Year + "-" + date.Replace('月', '-').Replace("日", "");
                                        }
                                    }
                                }
                                mri.ReleaseDate = date;

                            }
                            #endregion
                        }
                        #endregion
                        #region 快照 来源
                        MIL.Html.HtmlNodeCollection footNodes = (n as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "f13");
                        if (footNodes != null && footNodes.Count > 0)
                        {
                            if (footNodes[0].IsElement())
                            {
                                foreach (MIL.Html.HtmlNode foot in (footNodes[0] as MIL.Html.HtmlElement).Nodes)
                                {
                                    if (foot.IsElement() && (foot as MIL.Html.HtmlElement).Name == "span"
                                        && (foot as MIL.Html.HtmlElement).Attributes != null
                                        && (foot as MIL.Html.HtmlElement).Attributes["class"] != null
                                        && (foot as MIL.Html.HtmlElement).Attributes["class"].Value == "g")
                                    {
                                        //<span class="g">www.douban.com/note/43...&nbsp;</span>
                                        mri.ReleaseName = (foot as MIL.Html.HtmlElement).Text;
                                    }
                                    else if (foot.IsElement() && (foot as MIL.Html.HtmlElement).Name == "a"
                                        && (foot as MIL.Html.HtmlElement).Attributes != null
                                        && (foot as MIL.Html.HtmlElement).Attributes["class"] != null
                                        && (foot as MIL.Html.HtmlElement).Attributes["class"].Value == "m"
                                        && (foot as MIL.Html.HtmlElement).Text.Contains("百度快照"))
                                    {
                                        mri.Snapshot = (foot as MIL.Html.HtmlElement).Attributes["href"].Value;
                                    }
                                }
                            }
                        }
                        #endregion
                        #region 其他杂项
                        mri.KeyWords = keyword;
                        mri.CollectDate = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        mri.Kid = kid;
                        mri.Sheng = "";
                        mri.Shi = "";
                        mri.Xian = "";
                        //mri.WebName = "百度";
                        if (!string.IsNullOrEmpty(mri.ReleaseName))
                        {
                            mri.WebName = mri.ReleaseName;
                        }
                        else
                        {
                            mri.WebName = "全网";
                        }
                        mri.Pid = 0;
                        //mri.Part = GetParts(mri.Contexts);
                        mri.Comments = (int)WebSourceType.BaiduWeb;
                        mri.Reposts = 0;
                        #endregion
                        #region 2015.8.13 新增获取网址正文
                        if (!string.IsNullOrEmpty(mri.InfoSource))
                        {
                            string strContexts = HtmlUtil.getHtml(mri.InfoSource, "");
                            string noHtmlContexts = HtmlUtil.NoHTML(strContexts);
                            //分析关键字前100，后50个字符
                            string formatContexts = GetContexts(noHtmlContexts, keyword);
                            if (!string.IsNullOrEmpty(formatContexts))
                            {
                                mri.Contexts = formatContexts;
                            }
                        }
                        #endregion
                        #region 报告进度
                        OnReportCactchProcess(mri);
                        #endregion
                        webDatas.Add(mri);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Comm.WriteErrorLog("解析百度新闻网页时报错：" + ex.Message);
                Comm.WriteErrorLog(ex.StackTrace);
            }

            return webDatas;

        }

        /// <summary>
        /// 解析搜狗的网页搜索，与博客的代码一致
        /// </summary>
        /// <param name="html"></param>
        /// <param name="keyword"></param>
        /// <param name="kid"></param>
        /// <returns></returns>
        public List<ModelReleaseInfo> ParseSogouWeb(string html, string keyword, int kid)
        {
            List<ModelReleaseInfo> webDatas = new List<ModelReleaseInfo>();
            try
            {
                #region 网页源码样例
                //<div id="rb_6" class="rb">  
                //    <h3 class="pt">  <!--awbg6-->  
                //        <a name="dttl" target="_blank" href="http://www.blogbus.com/everclean-logs/262169317.html" id="uigs__6"><!--awbg6--><em><!--red_beg-->雾霾<!--red_end--></em> - 倍儿逗象小树一样茁壮成长 - 博客大巴</a>  
                //    </h3>  
                //    <div class="ft" id="cacheresult_summary_6">  
                //        <!--summary_beg-->版权声明 ：转载时请以超链接形式标明文章原始出处和作者信息及 本声明 一周内能有一天看到太阳，就是好的了。 很难想象如果环境一直这样，是不是就如恐怖片一样。移居，说...<!--summary_end-->
                //    </div>  
                //    <div class="fb">  
                //        <cite id="cacheresult_info_6">博客大巴 - www.blogbus.com/eve...&nbsp;-&nbsp;2014-2-28</cite>  
                //        <a target="_blank" style="color: #666666;" href="/websnapshot?ie=utf8&amp;url=http%3A%2F%2Fwww.blogbus.com%2Feverclean-logs%2F262169317.html&amp;did=53c29d6a07020108-a3fbb6d637fe07d5-af73e37b602371d2e22f42f4362e846e&amp;k=8f41c9f2f14fdb8e687b2d9ff06e513d&amp;encodedQuery=%E9%9B%BE%E9%9C%BE&amp;query=%E9%9B%BE%E9%9C%BE&amp;&amp;w=01020400&amp;m=0&amp;st=0" id="sogou_snapshot_6">快照</a>
                //    </div>  
                //</div>

                //<div class="vrwrap">
                //    <h3 class="vrTitle">
                //        <a target="_blank" href="http://www.360doc.com/content/13/1225/10/11736860_339938566.shtml" id="sogou_vr_30010028_7"><em><!--red_beg-->雾霾<!--red_end--></em>的成因、危害及对策</a>
                //    </h3>
                //    <div class="strBox">
                //        <div class="str_div" id="sogou_vr_30010028_str_div_7">
                //            <a target="_blank" class="str_img size_120_90" id="sogou_vr_30010028_pic_7" href="http://www.360doc.com/content/13/1225/10/11736860_339938566.shtml"><img alt="" id="sogou_vr_30010028_pic_img_7" src="http://img03.sogoucdn.com/net/a/04/link?&amp;url=http%3A%2F%2Fimage67.360doc.com%2FDownloadImg%2F2013%2F12%2F2510%2F37746619_1.gif&amp;appid=100520083&amp;referer=http://www.360doc.com/content/13/1225/10/11736860_339938566.shtml" onerror="this.parentNode.parentNode.style.display=&quot;none&quot;;this.onerror = null;" style="left: -2.292px;"></a>
                //        </div>
                //        <div class="str_info_div">
                //            <ul class="str-list-v4">
                //                <li>来自：<a target="_blank" id="sogou_vr_30010028_value_stc_1_7" href="http://www.360doc.com/userhome/11736860">jywlkljh</a><strong>类别：</strong><a target="_blank" id="sogou_vr_30010028_value_stc_2_7" href="http://www.360doc.com/userhome.aspx?userid=11736860&amp;cid=259">环境保护</a><strong>日期：</strong><span>2013-12-25</span>
                //                </li>
                //                <li class="str-text-info">
                //                    <span><em><!--red_beg-->雾霾<!--red_end--></em>是雾和霾的组合词.因为空气质量的恶化,<em><!--red_beg-->阴霾<!--red_end--></em>天气现象出现增多,危害加重.中国不少地区把<em><!--red_beg-->阴霾<!--red_end--></em>天气现象并入雾一起作为灾害性天气预警预报.统称为＂<em><!--red_beg-->雾霾<!--red_end--></em>天气＂. 雾与... </span>
                //                </li>
                //            </ul>
                //            <div class="fb">
                //                <cite id="cacheresult_info_7">360doc个人图书馆 - www.360doc.com - 2013-12-25</cite>&nbsp;-&nbsp;<a target="_blank" style="color: #666666;" href="/websnapshot?ie=utf8&amp;url=http%3A%2F%2Fwww.360doc.com%2Fcontent%2F13%2F1225%2F10%2F11736860_339938566.shtml&amp;did=cbcd72704ffe6ca0-cf2cae934446a0eb-6fc08790e542c27802ad1b08c02dffd6&amp;k=d6447289cb4f5a76dd743d1bfb7758d5&amp;encodedQuery=%E9%9B%BE%E9%9C%BE&amp;query=%E9%9B%BE%E9%9C%BE&amp;&amp;w=01020400&amp;m=0&amp;st=1" id="sogou_snapshot_7">快照</a>&nbsp;-&nbsp;<a name="sogou_preview_links" style="color: #666666;" href="javascript:void(null);" id="sogou_preview_7" onclick="sogou_preview(this,'7');return false;" sogou_preview_title="<em><!--red_beg-->雾霾<!--red_end--></em>的成因、危害及对策" sogou_preview_link="http://www.360doc.com/content/13/1225/10/11736860_339938566.shtml" url="/websnapshot?ie=utf8&amp;preview=1&amp;url=http%3A%2F%2Fwww.360doc.com%2Fcontent%2F13%2F1225%2F10%2F11736860_339938566.shtml&amp;did=cbcd72704ffe6ca0-cf2cae934446a0eb-6fc08790e542c27802ad1b08c02dffd6&amp;k=d6447289cb4f5a76dd743d1bfb7758d5&amp;encodedQuery=%E9%9B%BE%E9%9C%BE&amp;query=%E9%9B%BE%E9%9C%BE&amp;&amp;title=%E9%9B%BE%E9%9C%BE%E7%9A%84%E6%88%90%E5%9B%A0%E3%80%81%E5%8D%B1%E5%AE%B3%E5%8F%8A%E5%AF%B9%E7%AD%96&amp;st=1">预览</a>
                //            </div>
                //        </div>
                //    </div>
                //</div>
                #endregion
                #region 解析网站源码
                MIL.Html.HtmlDocument doc = MIL.Html.HtmlDocument.Create(html);
                MIL.Html.HtmlNodeCollection nodes = doc.Nodes.FindByAttributeNameValue("class", "results", true);
                foreach (MIL.Html.HtmlNode n in nodes)
                {
                    if ((n is MIL.Html.HtmlElement) && (n as MIL.Html.HtmlElement).Nodes != null)
                    {
                        foreach (MIL.Html.HtmlNode blog in (n as MIL.Html.HtmlElement).Nodes)
                        {
                            if (blog.IsElement() && (blog as MIL.Html.HtmlElement).Name == "div")
                            {
                                ModelReleaseInfo mri = new ModelReleaseInfo();

                                if (blog.IsElement() && (blog as MIL.Html.HtmlElement).Attributes["class"].Value == "rb")
                                {
                                    #region 标题与超链
                                    MIL.Html.HtmlNodeCollection titleNodes = (blog as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "pt");
                                    if (titleNodes != null && titleNodes.Count > 0)
                                    {
                                        //title中包含<em>节点，需要进行处理
                                        string title = "";
                                        MIL.Html.HtmlElement titleElement = ((titleNodes[0] as MIL.Html.HtmlElement).Nodes[0] as MIL.Html.HtmlElement);
                                        if (titleElement.Nodes != null && titleElement.Nodes.Count > 0)
                                        {
                                            foreach (MIL.Html.HtmlNode t in titleElement.Nodes)
                                            {
                                                if (t.IsText())
                                                {
                                                    title += (t as MIL.Html.HtmlText).Text;
                                                }
                                                else if (t.IsElement() && (t as MIL.Html.HtmlElement).Name.ToLower() == "em")
                                                {
                                                    title += (t as MIL.Html.HtmlElement).Text;
                                                }
                                            }
                                        }
                                        string href = ((titleNodes[0] as MIL.Html.HtmlElement).Nodes[0] as MIL.Html.HtmlElement).Attributes["href"].Value;
                                        mri.Title = title;
                                        mri.InfoSource = href;
                                    }
                                    #endregion
                                    #region 内容简介
                                    MIL.Html.HtmlNodeCollection contextNodes = (blog as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "ft");
                                    if (contextNodes != null && contextNodes.Count > 0)
                                    {
                                        string context = "";
                                        MIL.Html.HtmlElement contextElement = (contextNodes[0] as MIL.Html.HtmlElement);
                                        if (contextElement.Nodes != null && contextElement.Nodes.Count > 0)
                                        {
                                            foreach (MIL.Html.HtmlNode c in contextElement.Nodes)
                                            {
                                                if (c.IsText())
                                                {
                                                    context += (c as MIL.Html.HtmlText).Text;
                                                }
                                                else if (c.IsElement() && (c as MIL.Html.HtmlElement).Name.ToLower() == "em")
                                                {
                                                    context += (c as MIL.Html.HtmlElement).Text;
                                                }
                                            }
                                        }
                                        mri.Contexts = context;
                                    }

                                    #endregion
                                    #region 来源，发表时间 快照
                                    MIL.Html.HtmlNodeCollection authorNodes = (blog as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "fb");
                                    if (authorNodes != null && authorNodes.Count > 0)
                                    {
                                        foreach (MIL.Html.HtmlNode child in (authorNodes[0] as MIL.Html.HtmlElement).Nodes)
                                        {
                                            if (child.IsElement() && (child as MIL.Html.HtmlElement).Name == "cite")
                                            {
                                                //<cite id="cacheresult_info_0">新浪博客 - blog.sina.com.cn/s/...&nbsp;-&nbsp;2013-1-28</cite>
                                                string txt = (child as MIL.Html.HtmlElement).Text;
                                                //解析出来源与时间
                                                txt = GetSogouAuthorAndDate(txt);
                                                if (txt.IndexOf(',') >= 0)
                                                {
                                                    mri.ReleaseName = txt.Substring(0, txt.IndexOf(','));
                                                    mri.ReleaseDate = txt.Substring(txt.IndexOf(',') + 1);
                                                }
                                            }
                                            else if (child.IsElement() && (child as MIL.Html.HtmlElement).Name == "a" && (child as MIL.Html.HtmlElement).Attributes["id"].Value.Contains("snapshot"))
                                            {
                                                #region 快照
                                                string snapShot = (child as MIL.Html.HtmlElement).Attributes["href"].Value;
                                                mri.Snapshot = "http://www.sogou.com" + snapShot;
                                                #endregion
                                            }
                                        }

                                    }
                                    #endregion
                                }
                                else if (blog.IsElement() && (blog as MIL.Html.HtmlElement).Attributes["class"].Value == "vrwrap")
                                {
                                    #region 标题与超链
                                    MIL.Html.HtmlNodeCollection titleNodes = (blog as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "vrTitle");
                                    if (titleNodes != null && titleNodes.Count > 0)
                                    {
                                        //title中包含<em>节点，需要进行处理
                                        string title = "";
                                        MIL.Html.HtmlElement titleElement = ((titleNodes[0] as MIL.Html.HtmlElement).Nodes[0] as MIL.Html.HtmlElement);
                                        if (titleElement.Nodes != null && titleElement.Nodes.Count > 0)
                                        {
                                            foreach (MIL.Html.HtmlNode t in titleElement.Nodes)
                                            {
                                                if (t.IsText())
                                                {
                                                    title += (t as MIL.Html.HtmlText).Text;
                                                }
                                                else if (t.IsElement() && (t as MIL.Html.HtmlElement).Name.ToLower() == "em")
                                                {
                                                    title += (t as MIL.Html.HtmlElement).Text;
                                                }
                                            }
                                        }
                                        string href = ((titleNodes[0] as MIL.Html.HtmlElement).Nodes[0] as MIL.Html.HtmlElement).Attributes["href"].Value;
                                        mri.Title = title;
                                        mri.InfoSource = href;
                                    }
                                    #endregion
                                    #region 内容简介
                                    MIL.Html.HtmlNodeCollection contextNodes = (blog as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "str-text-info");
                                    if (contextNodes != null && contextNodes.Count > 0)
                                    {
                                        string context = "";
                                        MIL.Html.HtmlElement contextElement = ((contextNodes[0] as MIL.Html.HtmlElement).Nodes[0] as MIL.Html.HtmlElement);
                                        if (contextElement.Nodes != null && contextElement.Nodes.Count > 0)
                                        {
                                            foreach (MIL.Html.HtmlNode c in contextElement.Nodes)
                                            {
                                                if (c.IsText())
                                                {
                                                    context += (c as MIL.Html.HtmlText).Text;
                                                }
                                                else if (c.IsElement() && (c as MIL.Html.HtmlElement).Name.ToLower() == "em")
                                                {
                                                    context += (c as MIL.Html.HtmlElement).Text;
                                                }
                                            }
                                        }
                                        mri.Contexts = context;
                                    }

                                    #endregion
                                    #region 来源，发表时间 快照
                                    MIL.Html.HtmlNodeCollection authorNodes = (blog as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "fb");
                                    if (authorNodes != null && authorNodes.Count > 0)
                                    {
                                        foreach (MIL.Html.HtmlNode child in (authorNodes[0] as MIL.Html.HtmlElement).Nodes)
                                        {
                                            if (child.IsElement() && (child as MIL.Html.HtmlElement).Name == "cite")
                                            {
                                                //<cite id="cacheresult_info_1">搜狐博客 - beijinghubeigirl.blog.... - 2014-1-17</cite>
                                                string txt = (child as MIL.Html.HtmlElement).Text;
                                                //解析出来源与时间
                                                txt = GetSogouAuthorAndDate(txt);
                                                if (txt.IndexOf(',') >= 0)
                                                {
                                                    mri.ReleaseName = txt.Substring(0, txt.IndexOf(','));
                                                    mri.ReleaseDate = txt.Substring(txt.IndexOf(',') + 1);
                                                }
                                            }
                                            else if (child.IsElement() && (child as MIL.Html.HtmlElement).Name == "a" && (child as MIL.Html.HtmlElement).Attributes["id"].Value.Contains("snapshot"))
                                            {
                                                #region 快照
                                                string snapShot = (child as MIL.Html.HtmlElement).Attributes["href"].Value;
                                                mri.Snapshot = "http://www.sogou.com" + snapShot;
                                                #endregion
                                            }
                                        }
                                    }
                                    #endregion
                                }

                                #region 其他杂项
                                mri.KeyWords = keyword;
                                mri.CollectDate = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                mri.Kid = kid;
                                mri.Sheng = "";
                                mri.Shi = "";
                                mri.Xian = "";
                                if (!string.IsNullOrEmpty(mri.ReleaseName))
                                {
                                    mri.WebName = mri.ReleaseName;
                                }
                                else
                                {
                                    mri.WebName = "全网";
                                }
                                mri.Pid = 0;                                //mri.Part = GetParts(mri.Contexts);
                                mri.Comments = (int)WebSourceType.SogouWeb;
                                mri.Reposts = 0;
                                #endregion
                                #region 2015.8.13 新增获取网址正文
                                if (!string.IsNullOrEmpty(mri.InfoSource))
                                {
                                    string strContexts = HtmlUtil.getHtml(mri.InfoSource, "");
                                    string noHtmlContexts = HtmlUtil.NoHTML(strContexts);
                                    //分析关键字前100，后50个字符
                                    string formatContexts = GetContexts(noHtmlContexts, keyword);
                                    if (!string.IsNullOrEmpty(formatContexts))
                                    {
                                        mri.Contexts = formatContexts;
                                    }
                                }
                                #endregion
                                #region 报告进度
                                OnReportCactchProcess(mri);
                                #endregion
                                webDatas.Add(mri);
                            }
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Comm.WriteErrorLog("解析搜狗新闻搜索页时报错：" + ex.Message);
                Comm.WriteErrorLog(ex.StackTrace);
            }

            return webDatas;

        }

        /// <summary>
        /// 中搜
        /// </summary>
        /// <param name="html"></param>
        /// <param name="keyword"></param>
        /// <param name="kid"></param>
        /// <returns></returns>
        public List<ModelReleaseInfo> ParseZhongsouWeb(string html, string keyword, int kid)
        {
            List<ModelReleaseInfo> webDatas = new List<ModelReleaseInfo>();
            try
            {
                #region 解析网站源码
                MIL.Html.HtmlDocument doc = MIL.Html.HtmlDocument.Create(html);
                MIL.Html.HtmlNodeCollection nodes = doc.Nodes.FindByAttributeNameValue("class", "content-net-ul content-zonghe-ul", true);//content-net-ul
                if (nodes == null || (nodes != null && nodes.Count == 0))
                {
                    nodes = doc.Nodes.FindByAttributeNameValue("class", "content-net-ul", true);
                }
                if (nodes != null && nodes.Count > 0 && nodes[0].IsElement())
                {
                    MIL.Html.HtmlNodeCollection resultNodes = (nodes[0] as MIL.Html.HtmlElement).Nodes.FindByName("li");
                    if (resultNodes != null && resultNodes.Count > 0)
                    {
                        foreach (MIL.Html.HtmlNode result in resultNodes)
                        {
                            if ((result is MIL.Html.HtmlElement)
                                && ((result as MIL.Html.HtmlElement).Attributes == null || ((result as MIL.Html.HtmlElement).Attributes != null && (result as MIL.Html.HtmlElement).Attributes.Count == 0))
                                && (result as MIL.Html.HtmlElement).Nodes != null)
                            {
                                ModelReleaseInfo mri = new ModelReleaseInfo();
                                #region 标题、超链
                                MIL.Html.HtmlNodeCollection titleNodes = (result as MIL.Html.HtmlElement).Nodes.FindByName("h3");
                                if (titleNodes != null && titleNodes.Count > 0)
                                {
                                    //title中包含<strong>节点，需要进行处理
                                    if (titleNodes[0].IsElement())
                                    {
                                        MIL.Html.HtmlNodeCollection aNodes = (titleNodes[0] as MIL.Html.HtmlElement).Nodes.FindByName("a");
                                        if (aNodes != null && aNodes.Count > 0 && aNodes[0].IsElement())
                                        {
                                            string title = "";
                                            foreach (MIL.Html.HtmlNode t in (aNodes[0] as MIL.Html.HtmlElement).Nodes)
                                            {
                                                if (t.IsText())
                                                {
                                                    title += (t as MIL.Html.HtmlText).Text;
                                                }
                                                else if (t.IsElement() && (t as MIL.Html.HtmlElement).Name.ToLower() == "em")
                                                {
                                                    title += (t as MIL.Html.HtmlElement).Text;
                                                }
                                            }
                                            string href = (aNodes[0] as MIL.Html.HtmlElement).Attributes["href"].Value;

                                            mri.Title = title;
                                            mri.InfoSource = href;
                                            //中搜的没有发布者，暂时使用数据的超级链接代替
                                            mri.ReleaseName = href;
                                            mri.ReleaseDate = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                        }
                                    }
                                }
                                #endregion
                                #region 内容简介
                                MIL.Html.HtmlNodeCollection contentNodes = (result as MIL.Html.HtmlElement).Nodes.FindByName("p");
                                if (contentNodes != null && contentNodes.Count > 0)
                                {
                                    string context = "";
                                    if (contentNodes[0].IsElement())
                                    {
                                        foreach (MIL.Html.HtmlNode c in (contentNodes[0] as MIL.Html.HtmlElement).Nodes)
                                        {
                                            if (c.IsText())
                                            {
                                                context += (c as MIL.Html.HtmlText).Text;
                                            }
                                            else if (c.IsElement() && (c as MIL.Html.HtmlElement).Name.ToLower() == "em")
                                            {
                                                context += (c as MIL.Html.HtmlElement).Text;
                                            }
                                        }
                                    }
                                    mri.Contexts = context;
                                }

                                #endregion
                                #region 其他杂项
                                mri.Snapshot = "";
                                mri.KeyWords = keyword;
                                mri.CollectDate = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                mri.Kid = kid;
                                mri.Sheng = "";
                                mri.Shi = "";
                                mri.Xian = "";
                                if (!string.IsNullOrEmpty(mri.ReleaseName))
                                {
                                    mri.WebName = mri.ReleaseName;
                                }
                                else
                                {
                                    mri.WebName = "全网";
                                }
                                mri.Pid = 0;
                                //mri.Part = GetParts(mri.Contexts);
                                mri.Comments = (int)WebSourceType.ZhongsouWeb;
                                mri.Reposts = 0;
                                #endregion
                                #region 2015.8.13 新增获取网址正文
                                if (!string.IsNullOrEmpty(mri.InfoSource))
                                {
                                    string strContexts = HtmlUtil.getHtml(mri.InfoSource, "");
                                    string noHtmlContexts = HtmlUtil.NoHTML(strContexts);
                                    //分析关键字前100，后50个字符
                                    string formatContexts = GetContexts(noHtmlContexts, keyword);
                                    if (!string.IsNullOrEmpty(formatContexts))
                                    {
                                        mri.Contexts = formatContexts;
                                    }
                                }
                                #endregion
                                #region 报告进度
                                OnReportCactchProcess(mri);
                                #endregion
                                webDatas.Add(mri);
                            }
                        }
                    }

                }
                #endregion
            }
            catch (Exception ex)
            {
                Comm.WriteErrorLog("解析中搜网页搜索时报错：" + ex.Message);
                Comm.WriteErrorLog(ex.StackTrace);
            }

            return webDatas;

        }

        /// <summary>
        /// 好搜（360，有道都是使用的好搜的搜索结果）
        /// </summary>
        /// <param name="html"></param>
        /// <param name="keyword"></param>
        /// <param name="kid"></param>
        /// <returns></returns>
        public List<ModelReleaseInfo> ParseHaosouWeb(string html, string keyword, int kid)
        {
            List<ModelReleaseInfo> webDatas = new List<ModelReleaseInfo>();
            try
            {
                #region 解析网站源码
                MIL.Html.HtmlDocument doc = MIL.Html.HtmlDocument.Create(html);
                MIL.Html.HtmlNodeCollection nodes = doc.Nodes.FindByAttributeNameValue("class", "result", true);
                if (nodes != null && nodes.Count > 0 && nodes[0].IsElement())
                {
                    MIL.Html.HtmlNodeCollection resultNodes = (nodes[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "res-list", true);
                    if (resultNodes != null && resultNodes.Count > 0)
                    {
                        foreach (MIL.Html.HtmlNode result in resultNodes)
                        {
                            //<div class="res-rich so-rich-news clearfix">
                            if ((result is MIL.Html.HtmlElement) && (result as MIL.Html.HtmlElement).Nodes != null)
                            {
                                MIL.Html.HtmlNodeCollection contextNodes_Rich = (result as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "res-rich so-rich-news clearfix", true);
                                MIL.Html.HtmlNodeCollection contextNodes_Image = (result as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "res-rich res-image clearfix", true);
                                if ((contextNodes_Rich != null && contextNodes_Rich.Count > 0) || (contextNodes_Image != null && contextNodes_Image.Count > 0))
                                {
                                    ModelReleaseInfo mri = new ModelReleaseInfo();
                                    #region 标题、超链
                                    MIL.Html.HtmlNodeCollection titleNodes = (result as MIL.Html.HtmlElement).Nodes.FindByName("res-title");
                                    if (titleNodes != null && titleNodes.Count > 0)
                                    {
                                        if (titleNodes[0].IsElement())
                                        {
                                            MIL.Html.HtmlNodeCollection aNodes = (titleNodes[0] as MIL.Html.HtmlElement).Nodes.FindByName("a");
                                            if (aNodes != null && aNodes.Count > 0 && aNodes[0].IsElement())
                                            {
                                                string title = "";
                                                foreach (MIL.Html.HtmlNode t in (aNodes[0] as MIL.Html.HtmlElement).Nodes)
                                                {
                                                    if (t.IsText())
                                                    {
                                                        title += (t as MIL.Html.HtmlText).Text;
                                                    }
                                                    else if (t.IsElement() && (t as MIL.Html.HtmlElement).Name.ToLower() == "em")
                                                    {
                                                        title += (t as MIL.Html.HtmlElement).Text;
                                                    }
                                                }
                                                string href = (aNodes[0] as MIL.Html.HtmlElement).Attributes["href"].Value;

                                                mri.Title = title;
                                                mri.InfoSource = href;
                                            }
                                        }
                                    }
                                    #endregion
                                    #region 内容简介
                                    MIL.Html.HtmlNodeCollection context = null;
                                    MIL.Html.HtmlNodeCollection link = null;
                                    if (contextNodes_Image != null && contextNodes_Image.Count > 0)
                                    {
                                        MIL.Html.HtmlNodeCollection resCommNodes = (contextNodes_Image[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "res-comm-con");
                                        if (resCommNodes != null && resCommNodes.Count > 0)
                                        {
                                            context = (resCommNodes[0] as MIL.Html.HtmlElement).Nodes.FindByName("p");
                                            link = (resCommNodes[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "res-linkinfo");
                                        }
                                        else
                                        {
                                            context = (contextNodes_Image[0] as MIL.Html.HtmlElement).Nodes.FindByName("p");
                                            link = (contextNodes_Image[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "res-linkinfo");
                                        }
                                    }
                                    if (contextNodes_Rich != null && contextNodes_Rich.Count > 0)
                                    {
                                        MIL.Html.HtmlNodeCollection resCommNodes = (contextNodes_Rich[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "res-comm-con");
                                        if (resCommNodes != null && resCommNodes.Count > 0)
                                        {
                                            //内容直接写在res-comm-con的div下
                                            context = resCommNodes;
                                            link = (resCommNodes[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "res-linkinfo");
                                        }
                                        else
                                        {
                                            context = (contextNodes_Image[0] as MIL.Html.HtmlElement).Nodes.FindByName("div");
                                            if (context != null && context.Count > 0 && context[0].IsElement())
                                                link = (context[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "res-linkinfo");

                                        }
                                    }
                                    if (context != null && context.Count > 0)
                                    {
                                        string text = "";
                                        foreach (MIL.Html.HtmlNode c in (context[0] as MIL.Html.HtmlElement).Nodes)
                                        {
                                            if (c.IsText())
                                            {
                                                text += (c as MIL.Html.HtmlText).Text;
                                            }
                                            else if (c.IsElement() && (c as MIL.Html.HtmlElement).Name.ToLower() == "em")
                                            {
                                                text += (c as MIL.Html.HtmlElement).Text;
                                            }
                                        }
                                        mri.Contexts = text;
                                    }
                                    if (link != null && link.Count > 0)
                                    {
                                        //<cite>www.7y7.com/yule/8...&nbsp;2015-04-14</cite>
                                        MIL.Html.HtmlNodeCollection citeNodes = (link[0] as MIL.Html.HtmlElement).Nodes.FindByName("cite");
                                        if (citeNodes != null && citeNodes.Count > 0)
                                        {
                                            if (citeNodes[0].IsElement())
                                            {
                                                string date = (citeNodes[0] as MIL.Html.HtmlElement).Text;
                                                byte[] space = new byte[] { 0xc2, 0xa0 };
                                                string UTFSpace = Encoding.GetEncoding("UTF-8").GetString(space);
                                                string txt = date.Replace(UTFSpace, " ").Trim();
                                                if (txt.IndexOf(" ") > 0)
                                                {
                                                    txt = txt.Substring(txt.IndexOf(" ") + 1).Trim();
                                                }
                                                mri.ReleaseDate = FormateDate(txt);
                                            }
                                        }

                                        MIL.Html.HtmlNodeCollection cacheNodes = (link[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "m");
                                        if (cacheNodes != null && cacheNodes.Count > 0)
                                        {
                                            if (cacheNodes[0].IsElement() && (cacheNodes[0] as MIL.Html.HtmlElement).Name == "a")
                                            {
                                                if ((cacheNodes[0] as MIL.Html.HtmlElement).Attributes != null && (cacheNodes[0] as MIL.Html.HtmlElement).Attributes["href"] != null)
                                                {
                                                    string href = (cacheNodes[0] as MIL.Html.HtmlElement).Attributes["href"].Value;
                                                    mri.Snapshot = href;
                                                }
                                            }
                                        }
                                        MIL.Html.HtmlNodeCollection mingpianNodes = (link[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "mingpian mingpian-box");
                                        if (mingpianNodes == null || mingpianNodes.Count == 0)
                                        {
                                            mingpianNodes = (link[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "mingpian");
                                        }
                                        if (mingpianNodes != null && mingpianNodes.Count > 0)
                                        {
                                            if (mingpianNodes[0].IsElement() && (mingpianNodes[0] as MIL.Html.HtmlElement).Name == "a")
                                            {
                                                string src = (mingpianNodes[0] as MIL.Html.HtmlElement).Text;
                                                if (!string.IsNullOrEmpty(src))
                                                {
                                                    mri.ReleaseName = src;
                                                }
                                            }
                                        }
                                    }

                                    #endregion
                                    #region 其他杂项
                                    mri.KeyWords = keyword;
                                    mri.CollectDate = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                    mri.Kid = kid;
                                    mri.Sheng = "";
                                    mri.Shi = "";
                                    mri.Xian = "";
                                    if (!string.IsNullOrEmpty(mri.ReleaseName))
                                    {
                                        mri.WebName = mri.ReleaseName;
                                    }
                                    else
                                    {
                                        mri.WebName = "全网";
                                    }
                                    mri.Pid = 0;
                                    //mri.Part = GetParts(mri.Contexts);
                                    mri.Comments = (int)WebSourceType.HaosouWeb;
                                    mri.Reposts = 0;
                                    #endregion
                                    #region 报告进度
                                    OnReportCactchProcess(mri);
                                    #endregion
                                    webDatas.Add(mri);
                                }
                            }
                        }
                    }

                }
                #endregion
            }
            catch (Exception ex)
            {
                Comm.WriteErrorLog("解析中搜网页搜索时报错：" + ex.Message);
                Comm.WriteErrorLog(ex.StackTrace);
            }

            return webDatas;

        }

        #endregion

        #region 微信
        /// <summary>
        /// 解析搜狗微信
        /// </summary>
        /// <param name="html"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<ModelReleaseInfo> ParseSogouWeixin(string html, string keyword, int kid, CookieContainer cookies, string strCookies)
        {
            List<ModelReleaseInfo> webDatas = new List<ModelReleaseInfo>();
            try
            {
                #region 网页源码样例
                //<div class="wx-rb wx-rb3" id="sogou_vr_11002601_box_0" d="ab735a258a90e8e1-6bee54fcbd896b2a-edc12dfac5e9cc56cc54e1c5029bc26f">
                //    <div class="img_box2">
                //        <a target="_blank" href="http://mp.weixin.qq.com/s?__biz=MjM5MzAwNTYyOA==&amp;mid=208327244&amp;idx=2&amp;sn=b717e3bec57a0ae878a0930148f95ef1&amp;3rd=MzA3MDU4NTYzMw==&amp;scene=6#rd" style="width:80px;height:80px;display:block;border:1px solid #ebebeb;overflow:hidden;" id="sogou_vr_11002601_img_0"><img style="visibility: visible; border: none; height: 80px; margin-left: -18.5px;" onload="vrImgLoad(this, 'fit', 80, 80)" onerror="imgErr(this.parentNode)" src="http://img01.store.sogou.com/net/a/04/link?appid=100520031&amp;url=http://mmbiz.qpic.cn/mmbiz/ymIic4RlQO3Hd2ouOx2RjQ9VCzpXmtev3RFRicZVrHVibM7SBZNmY7h0IcO4GlozCjfKIJIrIibAqqeQQfAGNGTdbg/0"></a>
                //    </div>
                //    <div class="txt-box">
                //        <h4>
                //            <a target="_blank" href="http://mp.weixin.qq.com/s?__biz=MjM5MzAwNTYyOA==&amp;mid=208327244&amp;idx=2&amp;sn=b717e3bec57a0ae878a0930148f95ef1&amp;3rd=MzA3MDU4NTYzMw==&amp;scene=6#rd" id="sogou_vr_11002601_title_0">马化腾:“引入移动互联网治<em><!--red_beg-->雾霾<!--red_end--></em>”</a>
                //        </h4>
                //        <p id="sogou_vr_11002601_summary_0">马化腾:“引入移动互联网治<em>雾霾</em>”来源:新京报(2015/3/5)“任何人在任何时候都应该平等、方便、无障碍地获取和使用信息.”全国人大代表、腾讯公司CEO马化腾在今年两会期间共发出四项议案,均与互联网相关.他建议加快移动互联网在民生领域的普及和应用,通过互...</p>
                //        <script>
                //                    (function(vrid,rank){
                //                        var id = 'sogou_vr_'+vrid+'_summary'+'_'+rank;
                //                        document.getElementById(id).innerHTML = cutLength(document.getElementById(id).innerHTML,240);
                //                })('11002601', '0');
                //        </script>
                //        <div class="s-p" t="1426037274">
                //            <a id="weixin_account" target="_blank" class="zhz" href="/gzh?openid=oIWsFtyE27izVYkKuEP1w4S1XpNQ" title="李志青环境经济工作室" i="oIWsFtyE27izVYkKuEP1w4S1XpNQ"><script>document.write(cutLength('李志青环境经济工作室', 16))</script>李志青环境经济...</a>
                //            <script>vrTimeHandle552write('1426037274')</script>
                //            09:27
                //            <span id="btn_share" class="fx on"><a class="fx-a" href="#" key="1">分享</a><div class="fx-pos" style="display: none"><em class="ico-sj"></em><a id="btn_share_xl" class="xl" href="#"><span></span></a><a target="_blank" id="btn_share_qzone" class="qq2" href="http://sns.qzone.qq.com/cgi-bin/qzshare/cgi_qzshare_onekey?source=shareqq&amp;url=http%3A%2F%2Fmp.weixin.qq.com%2Fs%3F__biz%3DMjM5MzAwNTYyOA%3D%3D%26mid%3D208327244%26idx%3D2%26sn%3Db717e3bec57a0ae878a0930148f95ef1%263rd%3DMzA3MDU4NTYzMw%3D%3D%26scene%3D6%23rd&amp;summary=%E3%80%80&amp;title=%E9%A9%AC%E5%8C%96%E8%85%BE%3A%E2%80%9C%E5%BC%95%E5%85%A5%E7%A7%BB%E5%8A%A8%E4%BA%92%E8%81%94%E7%BD%91%E6%B2%BB%E9%9B%BE%E9%9C%BE%E2%80%9D&amp;pics=http%3A%2F%2Fimg01.store.sogou.com%2Fnet%2Fa%2F04%2Flink%3Fappid%3D100520031%26url%3Dhttp%3A%2F%2Fmmbiz.qpic.cn%2Fmmbiz%2FymIic4RlQO3Hd2ouOx2RjQ9VCzpXmtev3RFRicZVrHVibM7SBZNmY7h0IcO4GlozCjfKIJIrIibAqqeQQfAGNGTdbg%2F0"><span></span></a><a target="_blank" id="btn_share_qq" class="qq" href="http://connect.qq.com/widget/shareqq/index.html?source=shareqq&amp;url=http%3A%2F%2Fmp.weixin.qq.com%2Fs%3F__biz%3DMjM5MzAwNTYyOA%3D%3D%26mid%3D208327244%26idx%3D2%26sn%3Db717e3bec57a0ae878a0930148f95ef1%263rd%3DMzA3MDU4NTYzMw%3D%3D%26scene%3D6%23rd&amp;summary=%E3%80%80&amp;title=%E9%A9%AC%E5%8C%96%E8%85%BE%3A%E2%80%9C%E5%BC%95%E5%85%A5%E7%A7%BB%E5%8A%A8%E4%BA%92%E8%81%94%E7%BD%91%E6%B2%BB%E9%9B%BE%E9%9C%BE%E2%80%9D&amp;desc=%E5%88%9A%E7%9C%8B%E5%88%B0%E8%BF%99%E7%AF%87%E6%96%87%E7%AB%A0%E4%B8%8D%E9%94%99%EF%BC%8C%E6%8E%A8%E8%8D%90%E7%BB%99%E4%BD%A0%E7%9C%8B%E7%9C%8B%EF%BD%9E"><span></span></a></div></span><span id="btn_favorite" class="sc"><a class="sc-a" href="#" key="0">收藏</a><div style="display:none" class="sc-pos" key="0"><em class="ico-sj"></em><p>您确定要取消该收藏？</p><span class="sc-btn"><a id="btn_confirm" i="" class="a2" href="#">确定</a><a id="btn_cancel" href="#">再想想</a></span></div><div style="display:none" class="sc-pos sc-pos-v1"><em class="ico-sj"></em><p>收藏成功！</p><p class="p2">在"<a id="btn_favorite" target="_blank" href="/share?stype=2">我的收藏</a>"可查看所有收藏内容。</p></div><div style="display:none" class="sc-pos sc-pos-wrr"><em class="ico-sj"></em><i class="ico-wrr"></i>收藏失败！请稍后再试。</div></span>
                //        </div>
                //    </div>
                //</div>
                #endregion
                #region 解析网站源码
                MIL.Html.HtmlDocument doc = MIL.Html.HtmlDocument.Create(html);
                MIL.Html.HtmlNodeCollection nodes = doc.Nodes.FindByAttributeNameValue("class", "wx-rb wx-rb3", true);
                foreach (MIL.Html.HtmlNode n in nodes)
                {
                    if ((n is MIL.Html.HtmlElement) && (n as MIL.Html.HtmlElement).Nodes != null)
                    {
                        MIL.Html.HtmlNodeCollection weixinNodes = (n as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "txt-box");
                        ModelReleaseInfo mri = new ModelReleaseInfo();
                        #region 标题与超链
                        if (weixinNodes != null && weixinNodes.Count > 0)
                        {
                            MIL.Html.HtmlNodeCollection titleNodes = (n as MIL.Html.HtmlElement).Nodes.FindByName("h4");
                            if (titleNodes != null && titleNodes.Count > 0)
                            {
                                //title中包含<em>节点，需要进行处理
                                string title = "";
                                MIL.Html.HtmlElement titleElement = ((titleNodes[0] as MIL.Html.HtmlElement).Nodes[0] as MIL.Html.HtmlElement);
                                if (titleElement.Nodes != null && titleElement.Nodes.Count > 0)
                                {
                                    foreach (MIL.Html.HtmlNode t in titleElement.Nodes)
                                    {
                                        if (t.IsText())
                                        {
                                            title += (t as MIL.Html.HtmlText).Text;
                                        }
                                        else if (t.IsElement() && (t as MIL.Html.HtmlElement).Name.ToLower() == "em")
                                        {
                                            title += (t as MIL.Html.HtmlElement).Text;
                                        }
                                    }
                                }
                                string href = ((titleNodes[0] as MIL.Html.HtmlElement).Nodes[0] as MIL.Html.HtmlElement).Attributes["href"].Value;
                                mri.Title = title;
                                //mri.InfoSource = href;
                                //2016.4.17 微信的地址需要添加前缀  http://weixin.sogou.com
                                mri.InfoSource = string.Format("{0}{1}", "http://weixin.sogou.com", href);                                
                                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(mri.InfoSource);
                                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                                req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.87 Safari/537.36";
                                req.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
                                req.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
                                //string domain = "weixin.sogou.com";
                                //CookieContainer myCookieContainer = new CookieContainer();
                                //Cookie ck = new Cookie("SMYUV", "1315876896029400", "/", domain);
                                //myCookieContainer.Add(ck);
                                //Cookie ck2 = new Cookie("SUV", "1315876896030236", "/", domain);
                                //myCookieContainer.Add(ck2);
                                //Cookie ck3 = new Cookie("ssuid", "8259370578", "/", domain);
                                //myCookieContainer.Add(ck3);
                                //Cookie ck4 = new Cookie("SUID", "B9AF8DDB7E23900A00000000558A6F47", "/", domain);
                                //myCookieContainer.Add(ck4);
                                //Cookie ck5 = new Cookie("pgv_pvi", "1353635840", "/", domain);
                                //myCookieContainer.Add(ck5);
                                //Cookie ck6 = new Cookie("usid", "7imwP0sX5gAk5mcI", "/", domain);
                                //myCookieContainer.Add(ck6);
                                //Cookie ck7 = new Cookie("_ga", "7imwP0sX5gAk5mcI", "/", domain);
                                //myCookieContainer.Add(ck7);
                                //Cookie ck8 = new Cookie("IPLOC", "CN1100", "/", domain);
                                //myCookieContainer.Add(ck8);
                                //Cookie ck9 = new Cookie("ABTEST", "0|1460883888|v1", "/", domain);
                                //myCookieContainer.Add(ck9);
                                //Cookie ck10 = new Cookie("weixinIndexVisited", "1", "/", domain);
                                //myCookieContainer.Add(ck10);
                                //Cookie ck11 = new Cookie("SNUID", "3375F7F18B8FBB200375FC0B8B212ABA", "/", domain);
                                //myCookieContainer.Add(ck11);
                                //Cookie ck12 = new Cookie("sct", "77", "/", domain);
                                //myCookieContainer.Add(ck12);
                                //req.CookieContainer = myCookieContainer;
                                //req.CookieContainer = cookies; //使用已经保存的cookies 方法一
                                req.Headers.Add("Cookie", strCookies); //使用已经保存的cookies 方法二

                                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                                if (response.ResponseUri!=null)
                                {
                                    mri.InfoSource = response.ResponseUri.AbsoluteUri;
                                }
                                //string loc = response.Headers["location"];
                                //if (!string.IsNullOrEmpty(loc))
                                //{
                                //    mri.InfoSource = loc;                                    
                                //}
                            }
                        }

                        #endregion
                        #region 内容简介
                        MIL.Html.HtmlNodeCollection contextNodes = (n as MIL.Html.HtmlElement).Nodes.FindByName("p");
                        if (contextNodes != null && contextNodes.Count > 0)
                        {
                            string context = "";
                            MIL.Html.HtmlElement contextElement = (contextNodes[0] as MIL.Html.HtmlElement);
                            if (contextElement.Nodes != null && contextElement.Nodes.Count > 0)
                            {
                                foreach (MIL.Html.HtmlNode c in contextElement.Nodes)
                                {
                                    if (c.IsText())
                                    {
                                        context += (c as MIL.Html.HtmlText).Text;
                                    }
                                    else if (c.IsElement() && (c as MIL.Html.HtmlElement).Name.ToLower() == "em")
                                    {
                                        context += (c as MIL.Html.HtmlElement).Text;
                                    }
                                }
                            }
                            mri.Contexts = context;
                        }

                        #endregion
                        #region 来源，发表时间
                        MIL.Html.HtmlNodeCollection authorNodes = (n as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "s-p");
                        if (authorNodes != null && authorNodes.Count > 0)
                        {
                            string author = ((authorNodes[0] as MIL.Html.HtmlElement).Nodes[0] as MIL.Html.HtmlElement).Attributes["title"].Value;
                            mri.ReleaseName = author;

                            string date = (authorNodes[0] as MIL.Html.HtmlElement).Text;
                            date = ((authorNodes[0] as MIL.Html.HtmlElement).Nodes[1] as MIL.Html.HtmlElement).Text;
                            if (date.IndexOf('\'') >= 0 && date.LastIndexOf('\'') > date.IndexOf('\''))
                            {
                                date = date.Substring(date.IndexOf('\'') + 1, date.LastIndexOf('\'') - date.IndexOf('\'') - 1);
                                Int64 d;
                                if (Int64.TryParse(date, out d))
                                {
                                    date = new DateTime(new DateTime(1970, 1, 1, 8, 0, 0).Ticks + Int64.Parse(date) * 10000000).ToString("yyyy-MM-dd HH:mm:dd");
                                    mri.ReleaseDate = date;
                                }
                            }
                        }
                        #endregion
                        #region 快照
                        mri.Snapshot = "";
                        #endregion
                        #region 其他杂项
                        mri.KeyWords = keyword;
                        mri.CollectDate = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        mri.Kid = kid;
                        mri.Sheng = "";
                        mri.Shi = "";
                        mri.Xian = "";
                        mri.WebName = "微信";
                        mri.Pid = 6;
                        //mri.Part = GetParts(mri.Contexts);
                        mri.Comments = (int)WebSourceType.SogouWeixin;
                        mri.Reposts = 0;
                        #endregion
                        #region 2015.8.13 新增获取网址正文
                        if (!string.IsNullOrEmpty(mri.InfoSource))
                        {
                            string strContexts = HtmlUtil.getHtml(mri.InfoSource, "");
                            string noHtmlContexts = HtmlUtil.NoHTML(strContexts);
                            //分析关键字前100，后50个字符
                            string formatContexts = GetContexts(noHtmlContexts, keyword);
                            if (!string.IsNullOrEmpty(formatContexts))
                            {
                                mri.Contexts = formatContexts;
                            }
                        }
                        #endregion
                        #region 报告进度
                        OnReportCactchProcess(mri);
                        #endregion
                        webDatas.Add(mri);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Comm.WriteErrorLog("解析搜狗微信搜索页时报错：" + ex.Message);
                Comm.WriteErrorLog(ex.StackTrace);
            }

            return webDatas;

        }
        #endregion

        #region 博客
        /// <summary>
        /// 解析搜狗博客
        /// </summary>
        /// <param name="html"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<ModelReleaseInfo> ParseSogouBlog(string html, string keyword, int kid)
        {
            List<ModelReleaseInfo> webDatas = new List<ModelReleaseInfo>();
            try
            {
                #region 网页源码样例
                //<div id="rb_6" class="rb">  
                //    <h3 class="pt">  <!--awbg6-->  
                //        <a name="dttl" target="_blank" href="http://www.blogbus.com/everclean-logs/262169317.html" id="uigs__6"><!--awbg6--><em><!--red_beg-->雾霾<!--red_end--></em> - 倍儿逗象小树一样茁壮成长 - 博客大巴</a>  
                //    </h3>  
                //    <div class="ft" id="cacheresult_summary_6">  
                //        <!--summary_beg-->版权声明 ：转载时请以超链接形式标明文章原始出处和作者信息及 本声明 一周内能有一天看到太阳，就是好的了。 很难想象如果环境一直这样，是不是就如恐怖片一样。移居，说...<!--summary_end-->
                //    </div>  
                //    <div class="fb">  
                //        <cite id="cacheresult_info_6">博客大巴 - www.blogbus.com/eve...&nbsp;-&nbsp;2014-2-28</cite>  
                //        <a target="_blank" style="color: #666666;" href="/websnapshot?ie=utf8&amp;url=http%3A%2F%2Fwww.blogbus.com%2Feverclean-logs%2F262169317.html&amp;did=53c29d6a07020108-a3fbb6d637fe07d5-af73e37b602371d2e22f42f4362e846e&amp;k=8f41c9f2f14fdb8e687b2d9ff06e513d&amp;encodedQuery=%E9%9B%BE%E9%9C%BE&amp;query=%E9%9B%BE%E9%9C%BE&amp;&amp;w=01020400&amp;m=0&amp;st=0" id="sogou_snapshot_6">快照</a>
                //    </div>  
                //</div>

                //<div class="vrwrap">
                //    <h3 class="vrTitle">
                //        <a target="_blank" href="http://www.360doc.com/content/13/1225/10/11736860_339938566.shtml" id="sogou_vr_30010028_7"><em><!--red_beg-->雾霾<!--red_end--></em>的成因、危害及对策</a>
                //    </h3>
                //    <div class="strBox">
                //        <div class="str_div" id="sogou_vr_30010028_str_div_7">
                //            <a target="_blank" class="str_img size_120_90" id="sogou_vr_30010028_pic_7" href="http://www.360doc.com/content/13/1225/10/11736860_339938566.shtml"><img alt="" id="sogou_vr_30010028_pic_img_7" src="http://img03.sogoucdn.com/net/a/04/link?&amp;url=http%3A%2F%2Fimage67.360doc.com%2FDownloadImg%2F2013%2F12%2F2510%2F37746619_1.gif&amp;appid=100520083&amp;referer=http://www.360doc.com/content/13/1225/10/11736860_339938566.shtml" onerror="this.parentNode.parentNode.style.display=&quot;none&quot;;this.onerror = null;" style="left: -2.292px;"></a>
                //        </div>
                //        <div class="str_info_div">
                //            <ul class="str-list-v4">
                //                <li>来自：<a target="_blank" id="sogou_vr_30010028_value_stc_1_7" href="http://www.360doc.com/userhome/11736860">jywlkljh</a><strong>类别：</strong><a target="_blank" id="sogou_vr_30010028_value_stc_2_7" href="http://www.360doc.com/userhome.aspx?userid=11736860&amp;cid=259">环境保护</a><strong>日期：</strong><span>2013-12-25</span>
                //                </li>
                //                <li class="str-text-info">
                //                    <span><em><!--red_beg-->雾霾<!--red_end--></em>是雾和霾的组合词.因为空气质量的恶化,<em><!--red_beg-->阴霾<!--red_end--></em>天气现象出现增多,危害加重.中国不少地区把<em><!--red_beg-->阴霾<!--red_end--></em>天气现象并入雾一起作为灾害性天气预警预报.统称为＂<em><!--red_beg-->雾霾<!--red_end--></em>天气＂. 雾与... </span>
                //                </li>
                //            </ul>
                //            <div class="fb">
                //                <cite id="cacheresult_info_7">360doc个人图书馆 - www.360doc.com - 2013-12-25</cite>&nbsp;-&nbsp;<a target="_blank" style="color: #666666;" href="/websnapshot?ie=utf8&amp;url=http%3A%2F%2Fwww.360doc.com%2Fcontent%2F13%2F1225%2F10%2F11736860_339938566.shtml&amp;did=cbcd72704ffe6ca0-cf2cae934446a0eb-6fc08790e542c27802ad1b08c02dffd6&amp;k=d6447289cb4f5a76dd743d1bfb7758d5&amp;encodedQuery=%E9%9B%BE%E9%9C%BE&amp;query=%E9%9B%BE%E9%9C%BE&amp;&amp;w=01020400&amp;m=0&amp;st=1" id="sogou_snapshot_7">快照</a>&nbsp;-&nbsp;<a name="sogou_preview_links" style="color: #666666;" href="javascript:void(null);" id="sogou_preview_7" onclick="sogou_preview(this,'7');return false;" sogou_preview_title="<em><!--red_beg-->雾霾<!--red_end--></em>的成因、危害及对策" sogou_preview_link="http://www.360doc.com/content/13/1225/10/11736860_339938566.shtml" url="/websnapshot?ie=utf8&amp;preview=1&amp;url=http%3A%2F%2Fwww.360doc.com%2Fcontent%2F13%2F1225%2F10%2F11736860_339938566.shtml&amp;did=cbcd72704ffe6ca0-cf2cae934446a0eb-6fc08790e542c27802ad1b08c02dffd6&amp;k=d6447289cb4f5a76dd743d1bfb7758d5&amp;encodedQuery=%E9%9B%BE%E9%9C%BE&amp;query=%E9%9B%BE%E9%9C%BE&amp;&amp;title=%E9%9B%BE%E9%9C%BE%E7%9A%84%E6%88%90%E5%9B%A0%E3%80%81%E5%8D%B1%E5%AE%B3%E5%8F%8A%E5%AF%B9%E7%AD%96&amp;st=1">预览</a>
                //            </div>
                //        </div>
                //    </div>
                //</div>
                #endregion
                #region 解析网站源码
                MIL.Html.HtmlDocument doc = MIL.Html.HtmlDocument.Create(html);
                MIL.Html.HtmlNodeCollection nodes = doc.Nodes.FindByAttributeNameValue("class", "results", true);
                foreach (MIL.Html.HtmlNode n in nodes)
                {
                    if ((n is MIL.Html.HtmlElement) && (n as MIL.Html.HtmlElement).Nodes != null)
                    {
                        foreach (MIL.Html.HtmlNode blog in (n as MIL.Html.HtmlElement).Nodes)
                        {
                            if (blog.IsElement() && (blog as MIL.Html.HtmlElement).Name == "div")
                            {
                                ModelReleaseInfo mri = new ModelReleaseInfo();

                                if (blog.IsElement() && (blog as MIL.Html.HtmlElement).Attributes["class"].Value == "rb")
                                {
                                    #region 标题与超链
                                    MIL.Html.HtmlNodeCollection titleNodes = (blog as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "pt");
                                    if (titleNodes != null && titleNodes.Count > 0)
                                    {
                                        //title中包含<em>节点，需要进行处理
                                        string title = "";
                                        MIL.Html.HtmlElement titleElement = ((titleNodes[0] as MIL.Html.HtmlElement).Nodes[0] as MIL.Html.HtmlElement);
                                        if (titleElement.Nodes != null && titleElement.Nodes.Count > 0)
                                        {
                                            foreach (MIL.Html.HtmlNode t in titleElement.Nodes)
                                            {
                                                if (t.IsText())
                                                {
                                                    title += (t as MIL.Html.HtmlText).Text;
                                                }
                                                else if (t.IsElement() && (t as MIL.Html.HtmlElement).Name.ToLower() == "em")
                                                {
                                                    title += (t as MIL.Html.HtmlElement).Text;
                                                }
                                            }
                                        }
                                        string href = ((titleNodes[0] as MIL.Html.HtmlElement).Nodes[0] as MIL.Html.HtmlElement).Attributes["href"].Value;
                                        mri.Title = title;
                                        mri.InfoSource = href;
                                    }
                                    #endregion
                                    #region 内容简介
                                    MIL.Html.HtmlNodeCollection contextNodes = (blog as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "ft");
                                    if (contextNodes != null && contextNodes.Count > 0)
                                    {
                                        string context = "";
                                        MIL.Html.HtmlElement contextElement = (contextNodes[0] as MIL.Html.HtmlElement);
                                        if (contextElement.Nodes != null && contextElement.Nodes.Count > 0)
                                        {
                                            foreach (MIL.Html.HtmlNode c in contextElement.Nodes)
                                            {
                                                if (c.IsText())
                                                {
                                                    context += (c as MIL.Html.HtmlText).Text;
                                                }
                                                else if (c.IsElement() && (c as MIL.Html.HtmlElement).Name.ToLower() == "em")
                                                {
                                                    context += (c as MIL.Html.HtmlElement).Text;
                                                }
                                            }
                                        }
                                        mri.Contexts = context;
                                    }

                                    #endregion
                                    #region 来源，发表时间 快照
                                    MIL.Html.HtmlNodeCollection authorNodes = (blog as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "fb");
                                    if (authorNodes != null && authorNodes.Count > 0)
                                    {
                                        foreach (MIL.Html.HtmlNode child in (authorNodes[0] as MIL.Html.HtmlElement).Nodes)
                                        {
                                            if (child.IsElement() && (child as MIL.Html.HtmlElement).Name == "cite")
                                            {
                                                //<cite id="cacheresult_info_0">新浪博客 - blog.sina.com.cn/s/...&nbsp;-&nbsp;2013-1-28</cite>
                                                string txt = (child as MIL.Html.HtmlElement).Text;
                                                //解析出来源与时间
                                                txt = GetSogouAuthorAndDate(txt);
                                                if (txt.IndexOf(',') >= 0)
                                                {
                                                    mri.ReleaseName = txt.Substring(0, txt.IndexOf(','));
                                                    mri.ReleaseDate = txt.Substring(txt.IndexOf(',') + 1);
                                                }
                                            }
                                            else if (child.IsElement() && (child as MIL.Html.HtmlElement).Name == "a" && (child as MIL.Html.HtmlElement).Attributes["id"].Value.Contains("snapshot"))
                                            {
                                                #region 快照
                                                string snapShot = (child as MIL.Html.HtmlElement).Attributes["href"].Value;
                                                mri.Snapshot = "http://www.sogou.com" + snapShot;
                                                #endregion
                                            }
                                        }

                                    }
                                    #endregion
                                }
                                else if (blog.IsElement() && (blog as MIL.Html.HtmlElement).Attributes["class"].Value == "vrwrap")
                                {
                                    #region 标题与超链
                                    MIL.Html.HtmlNodeCollection titleNodes = (blog as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "vrTitle");
                                    if (titleNodes != null && titleNodes.Count > 0)
                                    {
                                        //title中包含<em>节点，需要进行处理
                                        string title = "";
                                        MIL.Html.HtmlElement titleElement = ((titleNodes[0] as MIL.Html.HtmlElement).Nodes[0] as MIL.Html.HtmlElement);
                                        if (titleElement.Nodes != null && titleElement.Nodes.Count > 0)
                                        {
                                            foreach (MIL.Html.HtmlNode t in titleElement.Nodes)
                                            {
                                                if (t.IsText())
                                                {
                                                    title += (t as MIL.Html.HtmlText).Text;
                                                }
                                                else if (t.IsElement() && (t as MIL.Html.HtmlElement).Name.ToLower() == "em")
                                                {
                                                    title += (t as MIL.Html.HtmlElement).Text;
                                                }
                                            }
                                        }
                                        string href = ((titleNodes[0] as MIL.Html.HtmlElement).Nodes[0] as MIL.Html.HtmlElement).Attributes["href"].Value;
                                        mri.Title = title;
                                        mri.InfoSource = href;
                                    }
                                    #endregion
                                    #region 内容简介
                                    MIL.Html.HtmlNodeCollection contextNodes = (blog as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "str-text-info");
                                    if (contextNodes != null && contextNodes.Count > 0)
                                    {
                                        string context = "";
                                        MIL.Html.HtmlElement contextElement = ((contextNodes[0] as MIL.Html.HtmlElement).Nodes[0] as MIL.Html.HtmlElement);
                                        if (contextElement.Nodes != null && contextElement.Nodes.Count > 0)
                                        {
                                            foreach (MIL.Html.HtmlNode c in contextElement.Nodes)
                                            {
                                                if (c.IsText())
                                                {
                                                    context += (c as MIL.Html.HtmlText).Text;
                                                }
                                                else if (c.IsElement() && (c as MIL.Html.HtmlElement).Name.ToLower() == "em")
                                                {
                                                    context += (c as MIL.Html.HtmlElement).Text;
                                                }
                                            }
                                        }
                                        mri.Contexts = context;
                                    }

                                    #endregion
                                    #region 来源，发表时间 快照
                                    MIL.Html.HtmlNodeCollection authorNodes = (blog as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "fb");
                                    if (authorNodes != null && authorNodes.Count > 0)
                                    {
                                        foreach (MIL.Html.HtmlNode child in (authorNodes[0] as MIL.Html.HtmlElement).Nodes)
                                        {
                                            if (child.IsElement() && (child as MIL.Html.HtmlElement).Name == "cite")
                                            {
                                                //<cite id="cacheresult_info_1">搜狐博客 - beijinghubeigirl.blog.... - 2014-1-17</cite>
                                                string txt = (child as MIL.Html.HtmlElement).Text;
                                                //解析出来源与时间
                                                txt = GetSogouAuthorAndDate(txt);
                                                if (txt.IndexOf(',') >= 0)
                                                {
                                                    mri.ReleaseName = txt.Substring(0, txt.IndexOf(','));
                                                    mri.ReleaseDate = txt.Substring(txt.IndexOf(',') + 1);
                                                }
                                            }
                                            else if (child.IsElement() && (child as MIL.Html.HtmlElement).Name == "a" && (child as MIL.Html.HtmlElement).Attributes["id"].Value.Contains("snapshot"))
                                            {
                                                #region 快照
                                                string snapShot = (child as MIL.Html.HtmlElement).Attributes["href"].Value;
                                                mri.Snapshot = "http://www.sogou.com" + snapShot;
                                                #endregion
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                #region 其他杂项
                                mri.KeyWords = keyword;
                                mri.CollectDate = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                mri.Kid = kid;
                                mri.Sheng = "";
                                mri.Shi = "";
                                mri.Xian = "";
                                mri.WebName = "博客";
                                mri.Pid = 1;
                                //mri.Part = GetParts(mri.Contexts);
                                mri.Comments = (int)WebSourceType.SogouBlog;
                                mri.Reposts = 0;
                                #endregion
                                #region 2015.8.13 新增获取网址正文
                                if (!string.IsNullOrEmpty(mri.InfoSource))
                                {
                                    string strContexts = HtmlUtil.getHtml(mri.InfoSource, "");
                                    string noHtmlContexts = HtmlUtil.NoHTML(strContexts);
                                    //分析关键字前100，后50个字符
                                    string formatContexts = GetContexts(noHtmlContexts, keyword);
                                    if (!string.IsNullOrEmpty(formatContexts))
                                    {
                                        mri.Contexts = formatContexts;
                                    }
                                }
                                #endregion
                                #region 报告进度
                                OnReportCactchProcess(mri);
                                #endregion
                                webDatas.Add(mri);
                            }
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Comm.WriteErrorLog("解析搜狗博客搜索页时报错：" + ex.Message);
                Comm.WriteErrorLog(ex.StackTrace);
            }

            return webDatas;

        }
        #endregion

        #region 论坛
        /// <summary>
        /// 解析搜狗论坛搜索
        /// </summary>
        /// <param name="html"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<ModelReleaseInfo> ParseSogouBBS(string html, string keyword, int kid)
        {
            List<ModelReleaseInfo> webDatas = new List<ModelReleaseInfo>();
            try
            {
                #region 网页源码样例
                //<div class="rb">
                //<h3 class="pt">
                //<a name="dttl" target="_blank" href="http://www.douban.com/group/topic/73012913/" id="sogou_vr__7"><em><!--red_beg-->王学兵<!--red_end--></em>张博被抓啦？</a>
                //</h3>
                //<div class="bloginfo">回复：42　　发帖时间：2015-03-10</div>
                //<div class="ft" id="cacheresult_summary_7"><em><!--red_beg-->王学兵<!--red_end--></em>被抓啦？ 没人来扒嚒？ 好像是真的，其余的演员是谁啊？ 有人说是张博 对他的印象还停留在鼻子上带个环的牛魔王 最帅牛魔王。。可惜了 尼玛 我买了他话剧的票啊。 还好...</div>
                //<div class="fb">
                //<cite id="cacheresult_info_7">豆瓣 - www.douban.com - 2015-3-10</cite> - <a target="_blank" style="color: #666666;" href="/websnapshot?ie=utf8&amp;url=http%3A%2F%2Fwww.douban.com%2Fgroup%2Ftopic%2F73012913%2F&amp;did=4353f151360295fa-19fb356d1006e9a2-a79bdc52c64c17d84ef3760ce2d90ecf&amp;k=700a30e8de1c5980883fd5c164cf8fe7&amp;encodedQuery=%E7%8E%8B%E5%AD%A6%E5%85%B5&amp;query=%E7%8E%8B%E5%AD%A6%E5%85%B5&amp;&amp;p=40040100&amp;dp=1&amp;w=01020400&amp;m=0&amp;st=1" id="sogou_snapshot_7">  快照</a> - <a name="sogou_preview_links" style="color: #666666;" href="javascript:void(null);" id="sogou_preview_7" onclick="sogou_preview(this,'7');return false;" sogou_preview_title="<em><!--red_beg-->王学兵<!--red_end--></em>张博被抓啦？" sogou_preview_link="http://www.douban.com/group/topic/73012913/" url="/websnapshot?ie=utf8&amp;preview=1&amp;url=http%3A%2F%2Fwww.douban.com%2Fgroup%2Ftopic%2F73012913%2F&amp;did=4353f151360295fa-19fb356d1006e9a2-a79bdc52c64c17d84ef3760ce2d90ecf&amp;k=700a30e8de1c5980883fd5c164cf8fe7&amp;encodedQuery=%E7%8E%8B%E5%AD%A6%E5%85%B5&amp;query=%E7%8E%8B%E5%AD%A6%E5%85%B5&amp;&amp;p=40040100&amp;dp=1&amp;title=%E7%8E%8B%E5%AD%A6%E5%85%B5%E5%BC%A0%E5%8D%9A%E8%A2%AB%E6%8A%93%E5%95%A6%EF%BC%9F&amp;st=1">预览</a>
                //</div>
                //</div>

                //<div id="rb_8" class="rb">  <h3 class="pt">  <!--awbg8-->  <a name="dttl" target="_blank" href="http://club.baobao.sohu.com/mom_daugh/thread/31gdnmg7ltc" id="uigs__8"><!--awbg8--><em><!--red_beg-->王学兵<!--red_end--></em>涉毒被抓 - 搜狐社区</a>  </h3>  <div class="ft" id="cacheresult_summary_8">  <!--summary_beg-->坚持到永远虎妞 坚持到永远虎妞 坚持到永远虎妞 someone 淡烟疏雨杏花天 someone 坚持到永远虎妞 坚持到永远虎妞 兔妈1975 坚持到永远虎妞 someone 蔷薇雨花落 蔷薇雨花落 心想梦成小顺子 坚持到永远虎妞 心想梦成小顺子 青蛙oO<!--summary_end--></div>  <div class="fb">  <cite id="cacheresult_info_8">搜狐母婴 - club.baobao.sohu.co...&nbsp;-&nbsp;2天前</cite>  <!--resultinfodate:2015-3-10-->&nbsp;-&nbsp;<!--resultsnap_beg--><a target="_blank" style="color: #666666;" href="/websnapshot?ie=utf8&amp;url=http%3A%2F%2Fclub.baobao.sohu.com%2Fmom_daugh%2Fthread%2F31gdnmg7ltc&amp;did=30b30b6c3013fff8-413eedc0ea50c10e-f27e2646a3d448dca9697951702d6bdd&amp;k=26b5166c09911a1fde756d4a6baf7a16&amp;encodedQuery=%E7%8E%8B%E5%AD%A6%E5%85%B5&amp;query=%E7%8E%8B%E5%AD%A6%E5%85%B5&amp;&amp;p=40040100&amp;dp=1&amp;w=01020400&amp;m=0&amp;st=1" id="sogou_snapshot_8">快照<!--resultsnap_end--></a>  &nbsp;-&nbsp;<a name="sogou_preview_links" style="color: #666666;" href="javascript:void(null);" id="sogou_preview_8" onclick="sogou_preview(this,'8');return false;" sogou_preview_title="<em><!--red_beg-->王学兵<!--red_end--></em>涉毒被抓 - 搜狐社区" sogou_preview_link="http://club.baobao.sohu.com/mom_daugh/thread/31gdnmg7ltc" url="/websnapshot?ie=utf8&amp;preview=1&amp;url=http%3A%2F%2Fclub.baobao.sohu.com%2Fmom_daugh%2Fthread%2F31gdnmg7ltc&amp;did=30b30b6c3013fff8-413eedc0ea50c10e-f27e2646a3d448dca9697951702d6bdd&amp;k=26b5166c09911a1fde756d4a6baf7a16&amp;encodedQuery=%E7%8E%8B%E5%AD%A6%E5%85%B5&amp;query=%E7%8E%8B%E5%AD%A6%E5%85%B5&amp;&amp;p=40040100&amp;dp=1&amp;title=%E7%8E%8B%E5%AD%A6%E5%85%B5%E6%B6%89%E6%AF%92%E8%A2%AB%E6%8A%93+-+%E6%90%9C%E7%8B%90%E7%A4%BE%E5%8C%BA&amp;st=1">预览</a>  <div class="fb-remark"><a href="javascript:void(0);" class="vr-sp-evaicon" title="点评"><i></i></a><a href="javascript:void(0);" class="vr-sp-collect" title="收藏"><i></i></a><span style="display:none;" from="java" id="30b30b6c3013fff8-413eedc0ea50c10e-f27e2646a3d448dca9697951702d6bdd" zanurl="http://club.baobao.sohu.com/mom_daugh/thread/31gdnmg7ltc" zantitle="%3Cem%3E%3C!--red_beg--%3E%E7%8E%8B%E5%AD%A6%E5%85%B5%3C!--red_end--%3E%3C/em%3E%E6%B6%89%E6%AF%92%E8%A2%AB%E6%8A%93%20-%20%E6%90%9C%E7%8B%90%E7%A4%BE%E5%8C%BA" zandocid="30b30b6c3013fff8-413eedc0ea50c10e-f27e2646a3d448dca9697951702d6bdd"></span></div><script>initEndorseShow2({"docid":"30b30b6c3013fff8-413eedc0ea50c10e-f27e2646a3d448dca9697951702d6bdd","count":"0"},{"score":"0","total":"0","docid":"30b30b6c3013fff8-413eedc0ea50c10e-f27e2646a3d448dca9697951702d6bdd"});</script></div>  <div class="r-sech ext_query" style="" id="sogou_vr__sq_ext_8"><span>推荐您搜索：</span><a target="_blank" href="http://www.sogou.com/web?query=%E7%8E%8B%E5%AD%A6%E5%85%B5%20%E6%90%9C%E7%8B%90%E7%A4%BE%E5%8C%BA" id="sogou_vr__sq_ext_a_0_8">王学兵 搜狐社区</a><a target="_blank" href="http://www.sogou.com/web?query=%E7%8E%8B%E5%AD%A6%E5%85%B5%E6%B6%89%E6%AF%92%E8%A2%AB%E6%8A%93" id="sogou_vr__sq_ext_a_1_8">王学兵涉毒被抓</a></div>  <div class="r-sech site_query" style="display: none;" id="sogou_vr__sq_ext_site_8" site="" ext="王学兵 搜狐社区；王学兵涉毒被抓"><span>推荐您在<a target="_blank" href="http://club.baobao.sohu.com/mom_daugh/thread/31gdnmg7ltc" id="sogou_vr__sq_ext_site_url_8">http://club.baobao.sohu.com/mom_daugh/thread/31gdnmg7ltc</a>站内搜索： </span><a target="_blank" href="http://club.baobao.sohu.com/mom_daugh/thread/31gdnmg7ltc" id="sogou_vr__sq_ext_stie_a_8"></a></div></div>
                //<div class="vrwrap" style="width:548px">
                //<h3 class="vrTitle">
                //<a target="_blank" href="http://bbs.shangdu.com/t/20150310/01001001684597/684597-1.htm" id="sogou_vr_30000909_9"><em><!--red_beg-->王学兵<!--red_end--></em>涉毒被抓……_商都社区</a>
                //</h3>
                //<div class="strBox">
                //<div class="str_div" id="sogou_vr_30000909_pic_9">
                //<a target="_blank" class="str_img size_120_80" id="sogou_vr_30000909_pic_a_9" href="http://bbs.shangdu.com/t/20150310/01001001684597/684597-1.htm"><img alt="" id="sogou_vr_30000909_pic_img_9" src="http://img01.sogoucdn.com/net/a/04/link?&amp;url=http%3A%2F%2Fbbsimg.shangdu.com%2FUserFiles%2FImage%2F653%2F61751653%2F1425968947558.jpg&amp;appid=100520124&amp;referer=http://bbs.shangdu.com/t/20150310/01001001684597/684597-1.htm" 
                //style="top: -17.572px;"></a>
                //</div>
                //<div class="str_info_div">
                //<p class="str_info">
                //<span class="pink-color" style="display:none" id="sogou_vr_30000909_pink_9">[图文]</span>广告载入中... 广告载入中... 【楼主】 还有谁…… [HUAWEI T8950客户端软件]贴 手机客户端下载： 用户最新文章 广告载入中... 广告载入中... 广告载入中... 回复：<em><!--red_beg-->王学兵<!--red_end--></em>涉毒被...</p>
                //<div class="fb">
                //<cite id="cacheresult_info_9">商都BBS - bbs.shangdu.com - 2天前</cite>&nbsp;-&nbsp;<a target="_blank" style="color: #666666;" href="/websnapshot?ie=utf8&amp;url=http%3A%2F%2Fbbs.shangdu.com%2Ft%2F20150310%2F01001001684597%2F684597-1.htm&amp;did=e9fc19c65cd2272a-d1870cb49bf799d7-31857de99ffe3d54d661d75d7e88abc1&amp;k=cb1ce12d0348d24eb37534155017d22e&amp;encodedQuery=%E7%8E%8B%E5%AD%A6%E5%85%B5&amp;query=%E7%8E%8B%E5%AD%A6%E5%85%B5&amp;&amp;p=40040100&amp;dp=1&amp;w=01020400&amp;m=0&amp;st=1" id="sogou_snapshot_9">快照</a>
                //<div class="fb-remark">
                //</div>
                //</div>
                //</div>
                //</div>
                //<div class="r-sech ext_query" style="display: none;" id="sogou_vr_30000909_sq_ext_9">

                //</div>
                //</div>
                #endregion
                #region 解析网站源码
                MIL.Html.HtmlDocument doc = MIL.Html.HtmlDocument.Create(html);
                MIL.Html.HtmlNodeCollection nodes = doc.Nodes.FindByAttributeNameValue("class", "results", true);
                foreach (MIL.Html.HtmlNode n in nodes)
                {
                    if ((n is MIL.Html.HtmlElement) && (n as MIL.Html.HtmlElement).Nodes != null)
                    {
                        foreach (MIL.Html.HtmlNode blog in (n as MIL.Html.HtmlElement).Nodes)
                        {
                            if (blog.IsElement() && (blog as MIL.Html.HtmlElement).Name == "div")
                            {
                                ModelReleaseInfo mri = new ModelReleaseInfo();
                                if (blog.IsElement() && (blog as MIL.Html.HtmlElement).Attributes["class"].Value == "rb")
                                {
                                    #region 标题与超链
                                    MIL.Html.HtmlNodeCollection titleNodes = (blog as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "pt");
                                    if (titleNodes != null && titleNodes.Count > 0)
                                    {
                                        //title中包含<em>节点，需要进行处理
                                        string title = "";
                                        MIL.Html.HtmlElement titleElement = ((titleNodes[0] as MIL.Html.HtmlElement).Nodes[0] as MIL.Html.HtmlElement);
                                        if (titleElement.Nodes != null && titleElement.Nodes.Count > 0)
                                        {
                                            foreach (MIL.Html.HtmlNode t in titleElement.Nodes)
                                            {
                                                if (t.IsText())
                                                {
                                                    title += (t as MIL.Html.HtmlText).Text;
                                                }
                                                else if (t.IsElement() && (t as MIL.Html.HtmlElement).Name.ToLower() == "em")
                                                {
                                                    title += (t as MIL.Html.HtmlElement).Text;
                                                }
                                            }
                                        }
                                        string href = ((titleNodes[0] as MIL.Html.HtmlElement).Nodes[0] as MIL.Html.HtmlElement).Attributes["href"].Value;
                                        mri.Title = title;
                                        mri.InfoSource = href;
                                    }
                                    #endregion
                                    #region 内容简介
                                    MIL.Html.HtmlNodeCollection contextNodes = (blog as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "ft");
                                    if (contextNodes != null && contextNodes.Count > 0)
                                    {
                                        string context = "";
                                        MIL.Html.HtmlElement contextElement = (contextNodes[0] as MIL.Html.HtmlElement);
                                        if (contextElement.Nodes != null && contextElement.Nodes.Count > 0)
                                        {
                                            foreach (MIL.Html.HtmlNode c in contextElement.Nodes)
                                            {
                                                if (c.IsText())
                                                {
                                                    context += (c as MIL.Html.HtmlText).Text;
                                                }
                                                else if (c.IsElement() && (c as MIL.Html.HtmlElement).Name.ToLower() == "em")
                                                {
                                                    context += (c as MIL.Html.HtmlElement).Text;
                                                }
                                            }
                                        }
                                        mri.Contexts = context;
                                    }

                                    #endregion
                                    #region 来源，发表时间 快照
                                    MIL.Html.HtmlNodeCollection authorNodes = (blog as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "fb");
                                    if (authorNodes != null && authorNodes.Count > 0)
                                    {
                                        foreach (MIL.Html.HtmlNode child in (authorNodes[0] as MIL.Html.HtmlElement).Nodes)
                                        {
                                            if (child.IsElement() && (child as MIL.Html.HtmlElement).Name == "cite")
                                            {
                                                //<cite id="cacheresult_info_0">新浪博客 - blog.sina.com.cn/s/...&nbsp;-&nbsp;2013-1-28</cite>
                                                string txt = (child as MIL.Html.HtmlElement).Text;
                                                //解析出来源与时间
                                                txt = GetSogouAuthorAndDate(txt);
                                                if (txt.IndexOf(',') >= 0)
                                                {
                                                    mri.ReleaseName = txt.Substring(0, txt.IndexOf(','));
                                                    mri.ReleaseDate = txt.Substring(txt.IndexOf(',') + 1);
                                                }
                                            }
                                            else if (child.IsElement() && (child as MIL.Html.HtmlElement).Name == "a" && (child as MIL.Html.HtmlElement).Attributes["id"].Value.Contains("snapshot"))
                                            {
                                                #region 快照
                                                string snapShot = (child as MIL.Html.HtmlElement).Attributes["href"].Value;
                                                mri.Snapshot = "http://www.sogou.com" + snapShot;
                                                #endregion
                                            }
                                        }

                                    }
                                    #endregion
                                }
                                else if (blog.IsElement() && (blog as MIL.Html.HtmlElement).Attributes["class"].Value == "vrwrap")
                                {
                                    #region 标题与超链
                                    MIL.Html.HtmlNodeCollection titleNodes = (blog as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "vrTitle");
                                    if (titleNodes != null && titleNodes.Count > 0)
                                    {
                                        //title中包含<em>节点，需要进行处理
                                        string title = "";
                                        MIL.Html.HtmlElement titleElement = ((titleNodes[0] as MIL.Html.HtmlElement).Nodes[0] as MIL.Html.HtmlElement);
                                        if (titleElement.Nodes != null && titleElement.Nodes.Count > 0)
                                        {
                                            foreach (MIL.Html.HtmlNode t in titleElement.Nodes)
                                            {
                                                if (t.IsText())
                                                {
                                                    title += (t as MIL.Html.HtmlText).Text;
                                                }
                                                else if (t.IsElement() && (t as MIL.Html.HtmlElement).Name.ToLower() == "em")
                                                {
                                                    title += (t as MIL.Html.HtmlElement).Text;
                                                }
                                            }
                                        }
                                        string href = ((titleNodes[0] as MIL.Html.HtmlElement).Nodes[0] as MIL.Html.HtmlElement).Attributes["href"].Value;
                                        mri.Title = title;
                                        mri.InfoSource = href;
                                    }
                                    #endregion
                                    #region 内容简介
                                    MIL.Html.HtmlNodeCollection contextNodes = (blog as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "str_info");
                                    if (contextNodes != null && contextNodes.Count > 0)
                                    {
                                        string context = "";
                                        MIL.Html.HtmlElement contextElement = (contextNodes[0] as MIL.Html.HtmlElement);
                                        if (contextElement.Nodes != null && contextElement.Nodes.Count > 0)
                                        {
                                            foreach (MIL.Html.HtmlNode c in contextElement.Nodes)
                                            {
                                                if (c.IsText())
                                                {
                                                    context += (c as MIL.Html.HtmlText).Text;
                                                }
                                                else if (c.IsElement() && (c as MIL.Html.HtmlElement).Name.ToLower() == "em")
                                                {
                                                    context += (c as MIL.Html.HtmlElement).Text;
                                                }
                                            }
                                        }
                                        mri.Contexts = context;
                                    }

                                    #endregion
                                    #region 来源，发表时间 快照
                                    MIL.Html.HtmlNodeCollection authorNodes = (blog as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "fb");
                                    if (authorNodes != null && authorNodes.Count > 0)
                                    {
                                        foreach (MIL.Html.HtmlNode child in (authorNodes[0] as MIL.Html.HtmlElement).Nodes)
                                        {
                                            if (child.IsElement() && (child as MIL.Html.HtmlElement).Name == "cite")
                                            {
                                                //<cite id="cacheresult_info_9">商都BBS - bbs.shangdu.com - 2天前</cite>
                                                string txt = (child as MIL.Html.HtmlElement).Text;
                                                //解析出来源与时间
                                                txt = GetSogouAuthorAndDate(txt);
                                                if (txt.IndexOf(',') >= 0)
                                                {
                                                    mri.ReleaseName = txt.Substring(0, txt.IndexOf(','));
                                                    mri.ReleaseDate = txt.Substring(txt.IndexOf(',') + 1);
                                                }
                                            }
                                            else if (child.IsElement() && (child as MIL.Html.HtmlElement).Name == "a" && (child as MIL.Html.HtmlElement).Attributes["id"].Value.Contains("snapshot"))
                                            {
                                                #region 快照
                                                string snapShot = (child as MIL.Html.HtmlElement).Attributes["href"].Value;
                                                mri.Snapshot = "http://www.sogou.com" + snapShot;
                                                #endregion
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                #region 其他杂项
                                mri.KeyWords = keyword;
                                mri.CollectDate = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                mri.Kid = kid;
                                mri.Sheng = "";
                                mri.Shi = "";
                                mri.Xian = "";
                                mri.WebName = "论坛";
                                mri.Pid = 2;
                                //mri.Part = GetParts(mri.Contexts);
                                mri.Comments = (int)WebSourceType.SogouBBS;
                                mri.Reposts = 0;
                                #endregion
                                #region 2015.8.13 新增获取网址正文
                                if (!string.IsNullOrEmpty(mri.InfoSource))
                                {
                                    string strContexts = HtmlUtil.getHtml(mri.InfoSource, "");
                                    string noHtmlContexts = HtmlUtil.NoHTML(strContexts);
                                    //分析关键字前100，后50个字符
                                    string formatContexts = GetContexts(noHtmlContexts, keyword);
                                    if (!string.IsNullOrEmpty(formatContexts))
                                    {
                                        mri.Contexts = formatContexts;
                                    }
                                }
                                #endregion

                                #region 报告进度
                                OnReportCactchProcess(mri);
                                #endregion
                                webDatas.Add(mri);
                            }
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Comm.WriteErrorLog("解析搜狗论坛搜索页时报错：" + ex.Message);
                Comm.WriteErrorLog(ex.StackTrace);
            }

            return webDatas;

        }

        public List<ModelReleaseInfo> ParseZhongsouBBS(string html, string keyword, int kid)
        {
            List<ModelReleaseInfo> webDatas = new List<ModelReleaseInfo>();
            try
            {
                #region 解析网站源码
                MIL.Html.HtmlDocument doc = MIL.Html.HtmlDocument.Create(html);
                MIL.Html.HtmlNodeCollection nodes = doc.Nodes.FindByAttributeNameValue("class", "bbs-list", true);
                if (nodes != null && nodes.Count > 0 && nodes[0].IsElement())
                {
                    MIL.Html.HtmlNodeCollection resultNodes = (nodes[0] as MIL.Html.HtmlElement).Nodes.FindByName("li");
                    if (resultNodes != null && resultNodes.Count > 0)
                    {
                        foreach (MIL.Html.HtmlNode result in resultNodes)
                        {
                            if ((result is MIL.Html.HtmlElement)
                                && ((result as MIL.Html.HtmlElement).Attributes == null || ((result as MIL.Html.HtmlElement).Attributes != null && (result as MIL.Html.HtmlElement).Attributes.Count == 0))
                                && (result as MIL.Html.HtmlElement).Nodes != null)
                            {
                                ModelReleaseInfo mri = new ModelReleaseInfo();
                                #region 标题、超链
                                MIL.Html.HtmlNodeCollection titleNodes = (result as MIL.Html.HtmlElement).Nodes.FindByName("h3");
                                if (titleNodes != null && titleNodes.Count > 0)
                                {
                                    //title中包含<strong>节点，需要进行处理
                                    if (titleNodes[0].IsElement())
                                    {
                                        MIL.Html.HtmlNodeCollection aNodes = (titleNodes[0] as MIL.Html.HtmlElement).Nodes.FindByName("a");
                                        if (aNodes != null && aNodes.Count > 0 && aNodes[0].IsElement())
                                        {
                                            string title = "";
                                            foreach (MIL.Html.HtmlNode t in (aNodes[0] as MIL.Html.HtmlElement).Nodes)
                                            {
                                                if (t.IsText())
                                                {
                                                    title += (t as MIL.Html.HtmlText).Text;
                                                }
                                                else if (t.IsElement() && (t as MIL.Html.HtmlElement).Name.ToLower() == "em")
                                                {
                                                    title += (t as MIL.Html.HtmlElement).Text;
                                                }
                                            }
                                            string href = (aNodes[0] as MIL.Html.HtmlElement).Attributes["href"].Value;

                                            mri.Title = title;
                                            mri.InfoSource = href;
                                            //中搜的没有发布者，暂时使用数据的超级链接代替
                                            mri.ReleaseName = href;
                                            mri.ReleaseDate = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                        }

                                        MIL.Html.HtmlNodeCollection spanNodes = (titleNodes[0] as MIL.Html.HtmlElement).Nodes.FindByName("span");
                                        if (spanNodes != null && spanNodes.Count > 0)
                                        {
                                            string date = "";
                                            if (spanNodes[0].IsText())
                                            {
                                                date = (spanNodes[0] as MIL.Html.HtmlText).Text;
                                            }
                                            else if (spanNodes[0].IsElement())
                                            {
                                                date = (spanNodes[0] as MIL.Html.HtmlElement).Text;
                                            }
                                            string href = (aNodes[0] as MIL.Html.HtmlElement).Attributes["href"].Value;
                                            mri.ReleaseDate = FormateDate(date);
                                        }
                                    }
                                }
                                #endregion
                                #region 内容简介
                                MIL.Html.HtmlNodeCollection contentNodes = (result as MIL.Html.HtmlElement).Nodes.FindByName("p");
                                if (contentNodes != null && contentNodes.Count > 0)
                                {
                                    string context = "";
                                    if (contentNodes[0].IsElement())
                                    {
                                        foreach (MIL.Html.HtmlNode c in (contentNodes[0] as MIL.Html.HtmlElement).Nodes)
                                        {
                                            if (c.IsText())
                                            {
                                                context += (c as MIL.Html.HtmlText).Text;
                                            }
                                            else if (c.IsElement() && (c as MIL.Html.HtmlElement).Name.ToLower() == "em")
                                            {
                                                context += (c as MIL.Html.HtmlElement).Text;
                                            }
                                        }
                                    }
                                    mri.Contexts = context;

                                    if (contentNodes.Count > 1)
                                    {
                                        if (contentNodes[1].IsElement())
                                        {
                                            MIL.Html.HtmlNodeCollection aNodes = (contentNodes[1] as MIL.Html.HtmlElement).Nodes.FindByName("a");
                                            if (aNodes != null && aNodes.Count > 1 && aNodes[1].IsElement())
                                            {
                                                mri.ReleaseName = (aNodes[1] as MIL.Html.HtmlElement).Text;
                                            }
                                        }
                                        mri.Contexts = context;
                                    }
                                }

                                #endregion
                                #region 其他杂项
                                mri.Snapshot = "";
                                mri.KeyWords = keyword;
                                mri.CollectDate = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                mri.Kid = kid;
                                mri.Sheng = "";
                                mri.Shi = "";
                                mri.Xian = "";
                                mri.WebName = "论坛";
                                mri.Pid = 2;
                                //mri.Part = GetParts(mri.Contexts);
                                mri.Comments = (int)WebSourceType.ZhongsouBBS;
                                mri.Reposts = 0;
                                #endregion
                                #region 2015.8.13 新增获取网址正文
                                if (!string.IsNullOrEmpty(mri.InfoSource))
                                {
                                    string strContexts = HtmlUtil.getHtml(mri.InfoSource, "");
                                    string noHtmlContexts = HtmlUtil.NoHTML(strContexts);
                                    //分析关键字前100，后50个字符
                                    string formatContexts = GetContexts(noHtmlContexts, keyword);
                                    if (!string.IsNullOrEmpty(formatContexts))
                                    {
                                        mri.Contexts = formatContexts;
                                    }
                                }
                                #endregion
                                #region 报告进度
                                OnReportCactchProcess(mri);
                                #endregion
                                webDatas.Add(mri);
                            }
                        }
                    }

                }
                #endregion
            }
            catch (Exception ex)
            {
                Comm.WriteErrorLog("解析中搜网页搜索时报错：" + ex.Message);
                Comm.WriteErrorLog(ex.StackTrace);
            }

            return webDatas;

        }

        #endregion

        #region 贴吧
        /// <summary>
        /// 解析百度贴吧
        /// </summary>
        /// <param name="html"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<ModelReleaseInfo> ParseBaiduTieba(string html, string keyword, int kid)
        {
            List<ModelReleaseInfo> webDatas = new List<ModelReleaseInfo>();
            try
            {
                #region 网页源码样例
                #endregion
                #region 解析网站源码
                MIL.Html.HtmlDocument doc = MIL.Html.HtmlDocument.Create(html);
                MIL.Html.HtmlNodeCollection nodes = doc.Nodes.FindByAttributeNameValue("class", "s_post", true);
                foreach (MIL.Html.HtmlNode n in nodes)
                {
                    if ((n is MIL.Html.HtmlElement) && (n as MIL.Html.HtmlElement).Nodes != null)
                    {
                        MIL.Html.HtmlNodeCollection titleNodes = (n as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "p_title");
                        ModelReleaseInfo mri = new ModelReleaseInfo();
                        #region 标题与超链
                        if (titleNodes != null && titleNodes.Count > 0)
                        {
                            //title中包含<em>节点，需要进行处理
                            string title = "";
                            MIL.Html.HtmlElement titleElement = ((titleNodes[0] as MIL.Html.HtmlElement).Nodes[0] as MIL.Html.HtmlElement);
                            if (titleElement.Nodes != null && titleElement.Nodes.Count > 0)
                            {
                                foreach (MIL.Html.HtmlNode t in titleElement.Nodes)
                                {
                                    if (t.IsText())
                                    {
                                        title += (t as MIL.Html.HtmlText).Text;
                                    }
                                    else if (t.IsElement() && (t as MIL.Html.HtmlElement).Name.ToLower() == "em")
                                    {
                                        title += (t as MIL.Html.HtmlElement).Text;
                                    }
                                }
                            }
                            string href = ((titleNodes[0] as MIL.Html.HtmlElement).Nodes[0] as MIL.Html.HtmlElement).Attributes["href"].Value;
                            mri.Title = title;
                            mri.InfoSource = "http://tieba.baidu.com/" + href;
                        }

                        #endregion
                        #region 内容简介
                        MIL.Html.HtmlNodeCollection contextNodes = (n as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "p_content");
                        if (contextNodes != null && contextNodes.Count > 0)
                        {
                            string context = "";
                            MIL.Html.HtmlElement contextElement = (contextNodes[0] as MIL.Html.HtmlElement);
                            if (contextElement.Nodes != null && contextElement.Nodes.Count > 0)
                            {
                                foreach (MIL.Html.HtmlNode c in contextElement.Nodes)
                                {
                                    if (c.IsText())
                                    {
                                        context += (c as MIL.Html.HtmlText).Text;
                                    }
                                    else if (c.IsElement() && (c as MIL.Html.HtmlElement).Name.ToLower() == "em")
                                    {
                                        context += (c as MIL.Html.HtmlElement).Text;
                                    }
                                }
                            }
                            mri.Contexts = context;
                        }

                        #endregion
                        #region 来源，发表时间
                        MIL.Html.HtmlNodeCollection authorNodes = (n as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "p_violet");
                        if (authorNodes != null && authorNodes.Count > 0)
                        {
                            string author = (authorNodes[0] as MIL.Html.HtmlElement).Text;
                            mri.ReleaseName = author;
                        }
                        MIL.Html.HtmlNodeCollection publishNodes = (n as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "p_green p_date");
                        if (publishNodes != null && publishNodes.Count > 0)
                        {
                            string date = (publishNodes[0] as MIL.Html.HtmlElement).Text;
                            mri.ReleaseDate = date;
                        }
                        #endregion
                        #region 快照
                        mri.Snapshot = "";
                        #endregion
                        #region 其他杂项
                        mri.KeyWords = keyword;
                        mri.CollectDate = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        mri.Kid = kid;
                        mri.Sheng = "";
                        mri.Shi = "";
                        mri.Xian = "";
                        mri.WebName = "贴吧";
                        mri.Pid = 5;
                        //mri.Part = GetParts(mri.Contexts);
                        mri.Comments = (int)WebSourceType.BaiduTieba;
                        mri.Reposts = 0;
                        #endregion
                        #region 2015.8.13 新增获取网址正文
                        if (!string.IsNullOrEmpty(mri.InfoSource))
                        {
                            string strContexts = HtmlUtil.getHtml(mri.InfoSource, "");
                            string noHtmlContexts = HtmlUtil.NoHTML(strContexts);
                            //分析关键字前100，后50个字符
                            string formatContexts = GetContexts(noHtmlContexts, keyword);
                            if (!string.IsNullOrEmpty(formatContexts))
                            {
                                mri.Contexts = formatContexts;
                            }
                        }
                        #endregion
                        #region 报告进度
                        OnReportCactchProcess(mri);
                        #endregion
                        webDatas.Add(mri);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Comm.WriteErrorLog("解析贴吧搜索页时报错：" + ex.Message);
                Comm.WriteErrorLog(ex.StackTrace);
            }

            return webDatas;

        }

        #endregion

        #region 微博
        [DataContract]
        class WeiboBlock
        {
            [DataMember]
            public string pid { get; set; }
            [DataMember]
            public string[] js { get; set; }
            [DataMember]
            public string[] css { get; set; }
            [DataMember]
            public string html { get; set; }

        }

        public string SinWeiboDecode(string html)
        {
            string retHtml = "";
            MIL.Html.HtmlDocument doc = MIL.Html.HtmlDocument.Create(html);
            MIL.Html.HtmlNodeCollection scriptNodes = doc.Nodes.FindByName("script", true);
            if (scriptNodes != null && scriptNodes.Count > 0)
            {
                foreach (MIL.Html.HtmlNode script in scriptNodes)
                {
                    string figure = "STK && STK.pageletM && STK.pageletM.view";
                    if (script.IsElement())
                    {
                        string text = (script as MIL.Html.HtmlElement).Text.Trim();
                        if (!string.IsNullOrEmpty(text) && text.Trim().ToLower().StartsWith(figure.ToLower()))
                        {
                            if (text.IndexOf('(') >= 0 && text.LastIndexOf(')') > text.IndexOf('('))
                            {
                                string json = text.Substring(text.IndexOf('(') + 1, text.LastIndexOf(')') - text.IndexOf('(') - 1);

                                JavaScriptSerializer serializer = new JavaScriptSerializer();
                                var weiboBlock = serializer.Deserialize<WeiboBlock>(json);
                                if (weiboBlock.pid == "pl_weibo_direct")
                                {
                                    //正文段
                                    retHtml = weiboBlock.html;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return retHtml;

        }
        /// <summary>
        /// 解析新浪微博搜索
        /// </summary>
        /// <param name="html"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<ModelReleaseInfo> ParseSinaWeibo(string html, string keyword, int kid)
        {
            List<ModelReleaseInfo> webDatas = new List<ModelReleaseInfo>();
            try
            {
                //新浪微博的数据进行了加密处理，需要对数据进行解密操作
                string decodeHtml = SinWeiboDecode(html);
                MIL.Html.HtmlDocument doc = MIL.Html.HtmlDocument.Create(decodeHtml);

                #region 网页源码样例
                #endregion
                #region 解析网站源码
                MIL.Html.HtmlNodeCollection nodes = doc.Nodes.FindByAttributeNameValue("class", "WB_cardwrap S_bg2 clearfix", true);
                foreach (MIL.Html.HtmlNode n in nodes)
                {
                    if ((n is MIL.Html.HtmlElement) && (n as MIL.Html.HtmlElement).Nodes != null)
                    {
                        MIL.Html.HtmlNodeCollection contenNodes = (n as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "content clearfix", true);
                        if (contenNodes != null && contenNodes.Count > 0)
                        {
                            try
                            {
                                ModelReleaseInfo mri = new ModelReleaseInfo();
                                MIL.Html.HtmlNodeCollection feedContent = (contenNodes[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "feed_content wbcon", true);
                                if (feedContent != null && feedContent.Count > 0)
                                {
                                    #region 标题与超链
                                    MIL.Html.HtmlNodeCollection wTexta = (feedContent[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "W_texta W_fb");
                                    if (wTexta != null && wTexta.Count > 0)
                                    {
                                        string title = (wTexta[0] as MIL.Html.HtmlElement).Attributes["title"].Value;
                                        string href = (wTexta[0] as MIL.Html.HtmlElement).Attributes["href"].Value;
                                        mri.Title = title;
                                        mri.ReleaseName = title;
                                        mri.InfoSource = href;
                                    }
                                    #endregion
                                    #region 内容简介
                                    MIL.Html.HtmlNodeCollection commentTxt = (feedContent[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "comment_txt");
                                    if (commentTxt != null && commentTxt.Count > 0)
                                    {
                                        string context = "";
                                        MIL.Html.HtmlElement contextElement = (commentTxt[0] as MIL.Html.HtmlElement);
                                        if (contextElement.Nodes != null && contextElement.Nodes.Count > 0)
                                        {
                                            foreach (MIL.Html.HtmlNode c in contextElement.Nodes)
                                            {
                                                if (c.IsText())
                                                {
                                                    context += (c as MIL.Html.HtmlText).Text;
                                                }
                                                else if (c.IsElement() && (c as MIL.Html.HtmlElement).Name.ToLower() == "em")
                                                {
                                                    context += (c as MIL.Html.HtmlElement).Text;
                                                }
                                            }
                                        }
                                        mri.Contexts = context;
                                    }
                                    #endregion
                                }
                                #region 发表时间
                                MIL.Html.HtmlNodeCollection publishNodes = (contenNodes[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "feed_from W_textb", false);
                                if (publishNodes != null && publishNodes.Count > 0)
                                {
                                    MIL.Html.HtmlNodeCollection dateNodes = (publishNodes[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "W_textb");
                                    if (dateNodes != null && dateNodes.Count > 0)
                                    {
                                        string date = (dateNodes[0] as MIL.Html.HtmlElement).Attributes["title"].Value;
                                        mri.ReleaseDate = date;
                                    }
                                }
                                #endregion
                                #region 其他杂项
                                mri.KeyWords = keyword;
                                mri.CollectDate = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                mri.Kid = kid;
                                mri.Sheng = "";
                                mri.Shi = "";
                                mri.Xian = "";
                                mri.WebName = "微博";
                                mri.Pid = 3;
                                //mri.Part = GetParts(mri.Contexts);
                                mri.Comments = (int)WebSourceType.SinWeibo;
                                mri.Reposts = 0;
                                #endregion
                                #region 2015.8.13 新增获取网址正文
                                if (!string.IsNullOrEmpty(mri.InfoSource))
                                {
                                    string strContexts = HtmlUtil.getHtml(mri.InfoSource, "");
                                    string noHtmlContexts = HtmlUtil.NoHTML(strContexts);
                                    //分析关键字前100，后50个字符
                                    string formatContexts = GetContexts(noHtmlContexts, keyword);
                                    if (!string.IsNullOrEmpty(formatContexts))
                                    {
                                        mri.Contexts = formatContexts;
                                    }
                                }
                                #endregion
                                #region 报告进度
                                OnReportCactchProcess(mri);
                                #endregion
                                webDatas.Add(mri);
                            }
                            catch (Exception ex1)
                            {
                                Comm.WriteErrorLog("解析新浪微博搜索页时报错：" + ex1.Message);
                                Comm.WriteErrorLog(ex1.StackTrace);
                            }
                        }
                        else
                        {
                            //相关文章
                            MIL.Html.HtmlNodeCollection linkFeedNodes = (n as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "search_shortlink2 nopic clearfix", true);
                            if (linkFeedNodes != null && linkFeedNodes.Count > 0)
                            {
                                foreach (MIL.Html.HtmlNode linkFeed in linkFeedNodes)
                                {
                                    try
                                    {
                                        ModelReleaseInfo mri = new ModelReleaseInfo();
                                        #region 标题与超链
                                        MIL.Html.HtmlNodeCollection pNodes = (linkFeed as MIL.Html.HtmlElement).Nodes.FindByName("p", false);
                                        if (pNodes != null && pNodes.Count > 0)
                                        {
                                            MIL.Html.HtmlNodeCollection aNodes = (pNodes[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "W_texta W_fb", false);
                                            if (aNodes != null && aNodes.Count > 0)
                                            {
                                                string title = (aNodes[0] as MIL.Html.HtmlElement).Attributes["title"].Value;
                                                string href = (aNodes[0] as MIL.Html.HtmlElement).Attributes["href"].Value;
                                                mri.Title = title;
                                                mri.InfoSource = href;
                                            }
                                        }
                                        #endregion
                                        #region 内容和发布者，日期
                                        MIL.Html.HtmlNodeCollection divNodes = (linkFeed as MIL.Html.HtmlElement).Nodes.FindByName("div", false);
                                        if (divNodes != null && divNodes.Count > 0)
                                        {
                                            MIL.Html.HtmlNodeCollection linkNodes = (divNodes[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "link_con");
                                            if (linkNodes != null && linkNodes.Count > 0)
                                            {
                                                #region "内容"
                                                MIL.Html.HtmlNodeCollection pTitleNodes = (linkNodes[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "link_info W_textb", false);
                                                if (pTitleNodes != null && pTitleNodes.Count > 0)
                                                {
                                                    string context = "";
                                                    MIL.Html.HtmlElement contextElement = (pTitleNodes[0] as MIL.Html.HtmlElement);
                                                    if (contextElement.Nodes != null && contextElement.Nodes.Count > 0)
                                                    {
                                                        foreach (MIL.Html.HtmlNode c in contextElement.Nodes)
                                                        {
                                                            if (c.IsText())
                                                            {
                                                                context += (c as MIL.Html.HtmlText).Text;
                                                            }
                                                            else if (c.IsElement() && (c as MIL.Html.HtmlElement).Name.ToLower() == "em")
                                                            {
                                                                context += (c as MIL.Html.HtmlElement).Text;
                                                            }
                                                        }
                                                    }
                                                    mri.Contexts = context;

                                                }
                                                #endregion
                                                #region 发布者，日期
                                                MIL.Html.HtmlNodeCollection footNodes = (linkNodes[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "link_action clearfix W_linkb W_textb", false);
                                                if (footNodes != null && footNodes.Count > 0)
                                                {
                                                    MIL.Html.HtmlNodeCollection linkAcNodes = (footNodes[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "linkAC_from");
                                                    if (linkAcNodes != null && linkAcNodes.Count > 0)
                                                    {
                                                        if (linkAcNodes.Count >= 2)
                                                        {
                                                            if ((linkAcNodes[0] as MIL.Html.HtmlElement).Nodes != null && (linkAcNodes[0] as MIL.Html.HtmlElement).Nodes.Count > 0)
                                                            {
                                                                //<span class="linkAC_from"><a>新浪网</a></span>
                                                                if ((linkAcNodes[0] as MIL.Html.HtmlElement).Nodes[0].IsElement())
                                                                {
                                                                    mri.ReleaseName = ((linkAcNodes[0] as MIL.Html.HtmlElement).Nodes[0] as MIL.Html.HtmlElement).Text;
                                                                }
                                                                else if ((linkAcNodes[0] as MIL.Html.HtmlElement).Nodes[0].IsText())
                                                                {
                                                                    mri.ReleaseName = ((linkAcNodes[0] as MIL.Html.HtmlElement).Nodes[0] as MIL.Html.HtmlText).Text;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                //<span class="linkAC_from">新浪网 发布</span>
                                                                if (linkAcNodes[0].IsElement())
                                                                {
                                                                    mri.ReleaseName = (linkAcNodes[0] as MIL.Html.HtmlElement).Text;
                                                                }
                                                                else if (linkAcNodes[0].IsText())
                                                                {
                                                                    mri.ReleaseName = (linkAcNodes[0] as MIL.Html.HtmlText).Text;
                                                                }
                                                            }

                                                            string date = (linkAcNodes[1] as MIL.Html.HtmlElement).Text;
                                                            if (date.IndexOf("年") > 0 && date.IndexOf("月") > 0 && date.IndexOf("日") > 0)
                                                            {
                                                                date = date.Replace("年", "-").Replace("月", "-").Replace("日", "");
                                                            }
                                                            else if (date.IndexOf("年") == -1 && date.IndexOf("月") > 0 && date.IndexOf("日") > 0)
                                                            {
                                                                date = DateTime.Now.Year.ToString() + "-" + date.Replace("月", "-").Replace("日", "");
                                                            }
                                                            else
                                                            {
                                                                if (date.Contains("天前"))
                                                                {
                                                                    int offset = int.Parse(date.Substring(0, date.IndexOf("天前")).Trim());
                                                                    date = DateTime.Now.AddDays(offset * -1).ToString("yyyy-MM-dd");
                                                                }
                                                                else if (date.Contains("小时前"))
                                                                {
                                                                    int offset = int.Parse(date.Substring(0, date.IndexOf("小时前")).Trim());
                                                                    date = DateTime.Now.AddHours(offset * -1).ToString("yyyy-MM-dd");
                                                                }
                                                                else if (date.Contains("时前"))
                                                                {
                                                                    int offset = int.Parse(date.Substring(0, date.IndexOf("时前")).Trim());
                                                                    date = DateTime.Now.AddHours(offset * -1).ToString("yyyy-MM-dd");
                                                                }
                                                                else if (date.Contains("分") && date.Contains("前"))
                                                                {
                                                                    int offset = int.Parse(date.Substring(0, date.IndexOf("分")).Trim());
                                                                    date = DateTime.Now.AddMinutes(offset * -1).ToString("yyyy-MM-dd");
                                                                }
                                                            }
                                                            mri.ReleaseDate = date;
                                                        }

                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                        #endregion
                                        #region 其他杂项
                                        mri.KeyWords = keyword;
                                        mri.CollectDate = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                        mri.Kid = kid;
                                        mri.Sheng = "";
                                        mri.Shi = "";
                                        mri.Xian = "";
                                        mri.WebName = "微博";
                                        mri.Pid = 3;
                                        //mri.Part = GetParts(mri.Contexts);
                                        mri.Comments = (int)WebSourceType.SinWeibo;
                                        mri.Reposts = 0;
                                        #endregion
                                        #region 2015.8.13 新增获取网址正文
                                        if (!string.IsNullOrEmpty(mri.InfoSource))
                                        {
                                            string strContexts = HtmlUtil.getHtml(mri.InfoSource, "");
                                            string noHtmlContexts = HtmlUtil.NoHTML(strContexts);
                                            //分析关键字前100，后50个字符
                                            string formatContexts = GetContexts(noHtmlContexts, keyword);
                                            if (!string.IsNullOrEmpty(formatContexts))
                                            {
                                                mri.Contexts = formatContexts;
                                            }
                                        }
                                        #endregion
                                        #region 报告进度
                                        OnReportCactchProcess(mri);
                                        #endregion
                                        webDatas.Add(mri);
                                    }
                                    catch (Exception ex2)
                                    {
                                        Comm.WriteErrorLog("解析新浪微博搜索页时报错：" + ex2.Message);
                                        Comm.WriteErrorLog(ex2.StackTrace);
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Comm.WriteErrorLog("解析新浪微博搜索页时报错：" + ex.Message);
                Comm.WriteErrorLog(ex.StackTrace);
            }

            return webDatas;

        }

        public string GetContent(MIL.Html.HtmlElement node)
        {
            string text = "";
            if (node.Nodes != null && node.Nodes.Count > 0)
            {
                foreach (MIL.Html.HtmlNode n in node.Nodes)
                {
                    if (n.IsText())
                    {
                        text += (n as MIL.Html.HtmlText).Text;
                    }
                    else if (n.IsElement())
                    {
                        text += GetContent(n as MIL.Html.HtmlElement);
                    }
                }
            }
            return text;
        }

        public List<ModelReleaseInfo> ParseZhongsouWeibo(string html, string keyword, int kid)
        {
            List<ModelReleaseInfo> webDatas = new List<ModelReleaseInfo>();
            try
            {
                #region 解析网站源码
                MIL.Html.HtmlDocument doc = MIL.Html.HtmlDocument.Create(html);
                MIL.Html.HtmlNodeCollection nodes = doc.Nodes.FindByAttributeNameValue("class", "main_scenery_left", true);
                if (nodes != null && nodes.Count > 0 && nodes[0].IsElement())
                {
                    MIL.Html.HtmlNodeCollection resultNodes = (nodes[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "godreply_on");
                    if (resultNodes != null && resultNodes.Count > 0)
                    {
                        foreach (MIL.Html.HtmlNode result in resultNodes)
                        {
                            MIL.Html.HtmlNodeCollection weiboItems = (result as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "weibo_item clearfix");
                            if (weiboItems != null && weiboItems.Count > 0 && weiboItems[0].IsElement())
                            {
                                MIL.Html.HtmlNodeCollection weiboRightNodes = (weiboItems[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "weibo_right");
                                if (weiboRightNodes != null && weiboRightNodes.Count > 0 && weiboRightNodes[0].IsElement())
                                {
                                    ModelReleaseInfo mri = new ModelReleaseInfo();
                                    #region 标题
                                    MIL.Html.HtmlNodeCollection weiboTitleNodes = (weiboRightNodes[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "weibo_title");
                                    if (weiboTitleNodes != null && weiboTitleNodes.Count > 0)
                                    {
                                        MIL.Html.HtmlNodeCollection aNodes = (weiboRightNodes[0] as MIL.Html.HtmlElement).Nodes.FindByName("a");
                                        if (aNodes != null && aNodes.Count > 0 && aNodes[0].IsElement())
                                        {
                                            mri.Title = (aNodes[0] as MIL.Html.HtmlElement).Text;
                                        }
                                    }
                                    #endregion
                                    #region 内容
                                    MIL.Html.HtmlNodeCollection weiboTxtNodes = (weiboRightNodes[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "weibo_txt");
                                    if (weiboTxtNodes != null && weiboTxtNodes.Count > 0 && weiboTxtNodes[0].IsElement())
                                    {
                                        mri.Contexts = GetContent(weiboTxtNodes[0] as MIL.Html.HtmlElement);
                                    }
                                    //引用
                                    MIL.Html.HtmlNodeCollection weiboFbboxNodes = (weiboRightNodes[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "weibofbbox");
                                    if (weiboFbboxNodes != null && weiboFbboxNodes.Count > 0 && weiboFbboxNodes[0].IsElement())
                                    {
                                        mri.Contexts += GetContent(weiboFbboxNodes[0] as MIL.Html.HtmlElement);
                                    }
                                    #endregion
                                    #region 发布时间及超链
                                    MIL.Html.HtmlNodeCollection publishNodes = (weiboRightNodes[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "weibo_handle clearfix");
                                    if (publishNodes != null && publishNodes.Count > 0 && publishNodes[0].IsElement())
                                    {
                                        MIL.Html.HtmlNodeCollection dateNodes = (publishNodes[0] as MIL.Html.HtmlElement).Nodes.FindByAttributeNameValue("class", "weibo_time");
                                        if (dateNodes != null && dateNodes.Count > 0 && dateNodes[0].IsElement())
                                        {
                                            string date = (dateNodes[0] as MIL.Html.HtmlElement).Text;
                                            mri.ReleaseDate = FormateDate2(date);
                                        }
                                        MIL.Html.HtmlNodeCollection hrefNodes = (publishNodes[0] as MIL.Html.HtmlElement).Nodes.FindByName("a");
                                        if (hrefNodes != null && hrefNodes.Count > 0 && hrefNodes[0].IsElement())
                                        {
                                            mri.InfoSource = (hrefNodes[0] as MIL.Html.HtmlElement).Attributes["href"].Value;
                                        }
                                    }
                                    #endregion
                                    #region 其他杂项
                                    mri.Snapshot = "";
                                    mri.KeyWords = keyword;
                                    mri.CollectDate = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                    mri.Kid = kid;
                                    mri.Sheng = "";
                                    mri.Shi = "";
                                    mri.Xian = "";
                                    mri.WebName = "微博";
                                    mri.Pid = 3;
                                    //mri.Part = GetParts(mri.Contexts);
                                    mri.Comments = (int)WebSourceType.ZhongsouWeibo;
                                    mri.Reposts = 0;
                                    #endregion
                                    #region 2015.8.13 新增获取网址正文
                                    if (!string.IsNullOrEmpty(mri.InfoSource))
                                    {
                                        string strContexts = HtmlUtil.getHtml(mri.InfoSource, "");
                                        string noHtmlContexts = HtmlUtil.NoHTML(strContexts);
                                        //分析关键字前100，后50个字符
                                        string formatContexts = GetContexts(noHtmlContexts, keyword);
                                        if (!string.IsNullOrEmpty(formatContexts))
                                        {
                                            mri.Contexts = formatContexts;
                                        }
                                    }
                                    #endregion
                                    #region 报告进度
                                    OnReportCactchProcess(mri);
                                    #endregion
                                    webDatas.Add(mri);
                                }
                            }
                        }
                    }

                }
                #endregion
            }
            catch (Exception ex)
            {
                Comm.WriteErrorLog("解析中搜微博搜索时报错：" + ex.Message);
                Comm.WriteErrorLog(ex.StackTrace);
            }
            return webDatas;
        }
        #endregion

    }
}
