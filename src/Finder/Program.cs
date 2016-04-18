using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Finder.util;
using System.Text;
using System.Net;

namespace Finder
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            #region 测试代码
            HtmlParse.Parse parse = new HtmlParse.Parse();
            #region 百度新闻
            //string html = HtmlUtil.getHtml(@"http://news.baidu.com/ns?word=%CE%ED%F6%B2&tn=news&from=news&cl=2&rn=20&ct=1&oq=wumai&f=3&rsp=0", "");
            //parse.ParseBaiduNews(html, "");
            #endregion
            #region 搜狗微信
            //string html = HtmlUtil.HttpGet(@"http://weixin.sogou.com/weixin?type=2&query=反腐&ie=utf8", System.Text.Encoding.UTF8);
            //List<ModelReleaseInfo> mris = parse.ParseSogouWeixin(html, "");
            #endregion
            //string html = HtmlUtil.HttpGet(@"http://www.sogou.com/web?interation=196647&query=雾霾", System.Text.Encoding.UTF8);
            //List<ModelReleaseInfo> mris = parse.ParseSogouBlog(html, "");

            //<cite id="cacheresult_info_0">新浪博客 - blog.sina.com.cn/s/...&nbsp;-&nbsp;2013-1-28</cite>
            //<cite id="cacheresult_info_8">搜狐母婴 - club.baobao.sohu.co...&nbsp;-&nbsp;2天前</cite>
            //<cite id="cacheresult_info_9">商都BBS - bbs.shangdu.com - 2天前</cite>
            //<cite id="cacheresult_info_7">豆瓣 - www.douban.com - 2015-3-10</cite>
            //string text = "搜狐母婴 - club.bao-bao.sohu.co... - 2013 - 1 - 28 ";
            //string txt = parse.GetSogouAuthorAndDate(text);
            //搜狗搜索论坛
            //string html = HtmlUtil.HttpGet(@"http://www.sogou.com/web?interation=196648&query=王学兵", System.Text.Encoding.UTF8);
            //List<ModelReleaseInfo> mris = parse.ParseSogouBBS(html, "");
            //贴吧
            //string html = HtmlUtil.HttpGet(@"http://tieba.baidu.com/f/search/res?ie=utf-8&qw=雾霾", System.Text.Encoding.Default);
            //List<ModelReleaseInfo> mris = parse.ParseBaiduTieba(html, "");

            //新浪微博

            //String encodeKey = CrawlHtml.UrlEncode("雾霾");
            //string Url = "http://s.weibo.com/weibo/" + encodeKey + "?topnav=1&wvr=6&b=1&page=1";

            //string url = "http://weixin.sogou.com/weixin?type=2&query=日本地震&ie=utf8";

            //string html = HtmlUtil.HttpGet(url, Encoding.UTF8);
            //HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            //myHttpWebRequest.Timeout = 20 * 1000; //连接超时
            //myHttpWebRequest.Accept = "*/*";
            //myHttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0;)";
            //myHttpWebRequest.CookieContainer = new CookieContainer(); //暂存到新实例
            //myHttpWebRequest.GetResponse().Close();
            //CookieContainer cookies = myHttpWebRequest.CookieContainer; //保存cookies
            //string cookiesstr = myHttpWebRequest.CookieContainer.GetCookieHeader(myHttpWebRequest.RequestUri); //把cookies转换成字符串

            //url = "http://www.google.com.hk/search?oe=utf8&ie=utf8&source=uds&hl=zh-CN&q=3g";
            //myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            //myHttpWebRequest.Timeout = 20 * 1000; //连接超时
            //myHttpWebRequest.Accept = "*/*";
            //myHttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0;)";
            //myHttpWebRequest.CookieContainer = cookies; //使用已经保存的cookies 方法一
            ////myHttpWebRequest.Headers.Add("Cookie", cookiesstr); //使用已经保存的cookies 方法二
            //HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            //Stream stream = myHttpWebResponse.GetResponseStream();
            //stream.ReadTimeout = 15 * 1000; //读取超时
            //StreamReader sr = new StreamReader(stream, Encoding.GetEncoding("utf-8"));
            //string strWebData = sr.ReadToEnd();


            //addCookieToContainer(

            //string Url = "http://weixin.sogou.com/websearch/art.jsp?sg=xaXJ7lM8QYStF1kWP8Nhf27jjx9-k2V5Pxw2V1TnOVd5c3UQm_RtRRSpHy_Kv-vWcfbESUI3G2Pb-DQNpjh1C9crHcLpEBzF0I_n_tUUfmykMPyDsxj2kwPm7oaSSq-qWyqvhDizzyk.&url=p0OVDH8R4SHyUySb8E88hkJm8GF_McJfBfynRTbN8whUt5mIFg8z7f7OJoKS0COu7nKVURyrL7CSMXyLMuQ9cGQ3JxMQ3374HQ341djCfMVxr6fSRWMeKTzrjWK-vcFuMFpaWebFSBBYy-5x5In7jJFmExjqCxhpkyjFvwP6PuGcQ64lGQ2ZDMuqxplQrsbk";
            //HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(Url);
            ////req.Connection = "keep-alive";            
            //req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            //req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.87 Safari/537.36";
            //req.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
            //req.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
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
            //req.CookieContainer.SetCookies(req.RequestUri);

            //CookieContainer myCookieContainer2 = new CookieContainer();
            //string[] cookstr = cookieStr.Split(';');
            //CookieCollection cookieCollection = new CookieCollection();

            //foreach (string str in cookstr)
            //{
            //    string name = str.Substring(0, str.IndexOf("="));
            //    string value = str.Substring(str.IndexOf("=") + 1);
            //    Cookie ck = new Cookie(name, value);
            //    cookieCollection.Add(ck);

            //}
            //myCookieContainer.Add(new Uri("www.58.com"), cookieCollection);
            //myCookieContainer2.SetCookies(new Uri("www.58.com"), cookieStr);
            //string content = string.Empty;
            //HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(Url);
            //request.Method = "GET";
            //request.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.1 (KHTML, like Gecko) Chrome/21.0.1180.89 Safari/537.1";
            //request.KeepAlive = true;
            //request.Accept = "*/*";
            //request.Referer = "http://post.58.com/541/8/s5?pts=1403055598929";
            //request.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
            //request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
            //request.Headers.Add("Accept-Charset", "GBK,utf-8;q=0.7,*;q=0.3");
            //request.CookieContainer = myCookieContainer2;
            //Stream Stream;
            //Stream = request.GetRequestStream();
            //Stream.Write(streamByImg, 0, streamByImg.Length);

            //req.AllowAutoRedirect = false;
            //HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            //string loc = response.Headers["location"];

            //string html = HtmlUtil.HttpGet(Url, System.Text.Encoding.UTF8);
            //List<ModelReleaseInfo> mris = parse.ParseSinaWeibo(html, "");


            //string txt = CrawlHtml.ChDecodeUrl("\u6709 20 \u6761\u65b0\u5fae\u535a\uff0c\u70b9\u51fb\u67e5\u770b");
            #region 百度网页
            //String encodeKey = CrawlHtml.UrlEncode("果敢");
            //string url = string.Format(@"http://www.baidu.com/s?wd={0}&pn={1}&ie=utf-8", encodeKey, 0 * 10);
            //string html = HtmlUtil.HttpGet(url, Encoding.UTF8);
            //List<ModelReleaseInfo> mris = parse.ParseBaiduWeb(html, "", 0);
            #endregion
            #region 测试短信
            //Alert alert = new Alert(1000 * 60 * 30);
            //alert.sendSMS("测试短信", "18601005461", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            #endregion

            #region 测试日期转换
            //string[] formats = {"yyyy-MM-dd HH:mm:ss","yyyy-M-dd HH:mm:ss","yyyy-M-d HH:mm:ss","yyyy-MM-d HH:mm:ss",
            //        "yyyy-MM-dd HH:mm","yyyy-MM-dd hh:mm","yyyy-MM-dd H:mm","yyyy-MM-dd h:mm","yyyy-MM-dd HH:m","yyyy-MM-dd hh:m","yyyy-MM-dd h:m",
            //        "yyyy-MM-dd hh:mm:ss","yyyy-MM-dd hh:mm:s","yyyy-MM-dd hh:m:s","yyyy-MM-dd hh:m:ss","yyyy-MM-dd h:mm:ss","yyyy-MM-dd h:mm:s","yyyy-MM-dd h:m:s","yyyy-MM-dd h:m:ss",
            //        "yyyy-MM-dd HH:mm:s","yyyy-MM-dd HH:m:s","yyyy-MM-dd HH:m:ss","yyyy-MM-dd H:mm:ss","yyyy-MM-dd H:mm:s","yyyy-MM-dd H:m:s","yyyy-MM-dd H:m:ss",
            //        "yyyy-M-dd HH:mm","yyyy-M-dd hh:mm","yyyy-M-dd H:mm","yyyy-M-dd h:mm","yyyy-M-dd HH:m","yyyy-M-dd hh:m","yyyy-M-dd h:m",
            //        "yyyy-M-dd hh:mm:ss","yyyy-M-dd hh:mm:s","yyyy-M-dd hh:m:s","yyyy-M-dd hh:m:ss","yyyy-M-dd h:mm:ss","yyyy-M-dd h:mm:s","yyyy-M-dd h:m:s","yyyy-M-dd h:m:ss",
            //        "yyyy-M-dd HH:mm:s","yyyy-M-dd HH:m:s","yyyy-M-dd HH:m:ss","yyyy-M-dd H:mm:ss","yyyy-M-dd H:mm:s","yyyy-M-dd H:m:s","yyyy-M-dd H:m:ss",
            //        "yyyy-M-d HH:mm","yyyy-M-d hh:mm","yyyy-M-d H:mm","yyyy-M-d h:mm","yyyy-M-d HH:m","yyyy-M-d hh:m","yyyy-M-d h:m",
            //        "yyyy-M-d hh:mm:ss","yyyy-M-d hh:mm:s","yyyy-M-d hh:m:s","yyyy-M-d hh:m:ss","yyyy-M-d h:mm:ss","yyyy-M-d h:mm:s","yyyy-M-d h:m:s","yyyy-M-d h:m:ss",
            //        "yyyy-M-d HH:mm:s","yyyy-M-d HH:m:s","yyyy-M-d HH:m:ss","yyyy-M-d H:mm:ss","yyyy-M-d H:mm:s","yyyy-M-d H:m:s","yyyy-M-d H:m:ss",
            //        "yyyy-MM-d HH:mm","yyyy-MM-d hh:mm","yyyy-MM-d H:mm","yyyy-MM-d h:mm","yyyy-MM-d HH:m","yyyy-MM-d hh:m","yyyy-MM-d h:m",
            //        "yyyy-MM-d hh:mm:ss","yyyy-MM-d hh:mm:s","yyyy-MM-d hh:m:s","yyyy-MM-d hh:m:ss","yyyy-MM-d h:mm:ss","yyyy-MM-d h:mm:s","yyyy-MM-d h:m:s","yyyy-MM-d h:m:ss",
            //        "yyyy-MM-d HH:mm:s","yyyy-MM-d HH:m:s","yyyy-MM-d HH:m:ss","yyyy-MM-d H:mm:ss","yyyy-MM-d H:mm:s","yyyy-MM-d H:m:s","yyyy-MM-d H:m:ss",
            //        "yyyy-MM-dd","yyyy-M-dd","yyyy-M-d","yyyy-MM-d"};

            //string date = "";
            //DateTime dateValue;
            //bool ret = DateTime.TryParseExact(date, formats, 
            //    System.Globalization.DateTimeFormatInfo.CurrentInfo, System.Globalization.DateTimeStyles.None, out dateValue);
            #endregion

            #region 测试必应新闻
            String encodeKey = CrawlHtml.UrlEncode("雾霾 治理");
            //string url = string.Format(@"http://cn.bing.com/news/search?q={0}&first={1}&FORM=PONR", encodeKey, 1 * 10 + 1);
            //string html = HtmlUtil.HttpGet(url, Encoding.UTF8);
            //List<ModelReleaseInfo> mris = parse.ParseBingNews(html, "", 0);

            //string url = string.Format(@"http://cn.bing.com/search?q={0}&first={1}&FORM=PERE", encodeKey, 0 * 10 + 1); 
            //string html = HtmlUtil.HttpGet(url, Encoding.UTF8);
            //List<ModelReleaseInfo> mris = parse.ParseBingWeb(html, "", 0);

            //string url = string.Format(@"http://news.sogou.com/news?mode=1&query={0}&sut=6247&sst0=1428928362112&page={1}&w=01029901&dr=1", encodeKey, 1); 
            //string html = HtmlUtil.HttpGet(url, Encoding.Default);
            //List<ModelReleaseInfo> mris = parse.ParseSogouNews(html, "", 0);

            //string url = "http://www.sogou.com/web?query=%E5%B8%8C%E6%8B%89%E9%87%8C%E7%AB%9E%E9%80%89%E6%80%BB%E7%BB%9F&page=2&ie=utf8&w=03021800&dr=1";
            //string html = HtmlUtil.HttpGet(url, Encoding.UTF8);
            //List<ModelReleaseInfo> mris = parse.ParseSogouBlog(html, "", 0);

            //string url = "http://zixun.zhongsou.com/n?w=%CE%ED%F6%B2%20%D6%CE%C0%ED&b=1";
            //string html = HtmlUtil.HttpGet(url, Encoding.Default);
            //List<ModelReleaseInfo> mris = parse.ParseZhongsouNews(html, "", 0);

            //string url = "http://news.haosou.com/ns?q=%E6%98%9F%E5%85%89%E5%A4%A7%E9%81%93&pn=1&tn=news&rank=rank&j=0";
            //string html = HtmlUtil.HttpGet(url, Encoding.UTF8);
            //List<ModelReleaseInfo> mris = parse.ParseHaosouNews(html, "", 0);

            //string url = "http://www.sogou.com/web?query=%E9%9B%BE%E9%9C%BE+%E6%B2%BB%E7%90%86&hp=0&sut=4351&lkt=11%2C1429095975318%2C1429095978412&sst0=1429095979669&oq=wumai+zhili&stj0=0&stj1=0&stj=0%3B0%3B0%3B0&stj2=0&hp1=&ri=0&page=1&ie=utf8&p=40040100&dp=1&w=01015002&dr=1";
            //string html = HtmlUtil.HttpGet(url, Encoding.UTF8);
            //List<ModelReleaseInfo> mris = parse.ParseSogouWeb(html, "", 0);

            //string url = "http://www.zhongsou.com/third?w=%CE%ED%F6%B2%20%D6%CE%C0%ED&b=1";
            //string html = HtmlUtil.HttpGet(url, Encoding.Default);
            //List<ModelReleaseInfo> mris = parse.ParseZhongsouWeb(html, "", 0);

            //string url = "http://www.haosou.com/s?q=%E6%98%9F%E5%85%89%E5%A4%A7%E9%81%93&pn=1&j=0&ls=0&src=srp_paging&fr=tab_news&psid=c582f05942d9170285882a26de5a5d12";
            //string html = HtmlUtil.HttpGet(url, Encoding.UTF8);
            //List<ModelReleaseInfo> mris = parse.ParseHaosouWeb(html, "", 0);

            //string url = "http://bbs.zhongsou.com/b?b=1&w=%CE%ED%F6%B2%20%D6%CE%C0%ED&s=&sc=&dt=&t=&pt=&fo=&u=&au=&nt=1";
            //string html = HtmlUtil.HttpGet(url, Encoding.Default);
            //List<ModelReleaseInfo> mris = parse.ParseZhongsouBBS(html, "", 0);

            //string url = "http://t.zhongsou.com/wb?w=%CE%ED%F6%B2%20%D6%CE%C0%ED&b=1";
            //string html = HtmlUtil.HttpGet(url, Encoding.Default);
            //List<ModelReleaseInfo> mris = parse.ParseZhongsouWeibo(html, "", 0);

            #endregion
            //return;
            #endregion


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.DoEvents();
            Application.Run(new MainForm());
        }

        /// <summary>
        /// 函数或方法循环运行变量,如果为False,说明不停止,如果为True,说明在这时要结束循环的执行
        /// </summary>
        public static bool ProClose = false;
        //是否过有效期,true为过期,false未过期
        public static bool isBeyondDate = false;
    }
}
