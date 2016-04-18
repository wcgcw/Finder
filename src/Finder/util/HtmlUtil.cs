using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System;
using System.Net;
using System.IO;
using System.Text;
using Sodao.Snap;
using System.Drawing;


namespace Finder
{
    public class HtmlUtil
    {

        #region 获取指定ID的标签内容

        /// <summary>         
        /// 获取指定ID的标签内容         
        /// </summary>          
        /// <param name="html">HTML源码</param>         
        /// <param name="id">标签ID</param>        
        /// <returns></returns>          
        public static string GetElementById(string html, string id)
        {
            string pattern =
                @"<([a-z]+)(?:(?!id)[^<>])*id=([""']?){0}\2[^>]*>(?>(?<o><\1[^>]*>)|(?<-o></\1>)|(?:(?!</?\1).))*(?(o)(?!))</\1>";
            pattern = string.Format(pattern, Regex.Escape(id));
            Match match = Regex.Match(html, pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            return match.Success ? match.Value : "";
        }

        #endregion

        #region 通过class属性获取对应标签集合

        /// <summary>          
        /// 通过class属性获取对应标签集合        
        /// </summary>          
        /// <param name="html">HTML源码</param>
        /// <param name="className">class值</param>   
        /// <returns></returns>        
        public static string[] GetElementsByClass(string html, string className)
        {
            return GetElements(html, "", className);
        }

        /// <summary>          
        /// 通过class属性获取对应标签集合        
        /// </summary>          
        /// <param name="html">HTML源码</param>
        /// <param name="className">class值</param>   
        /// <returns></returns>        
        public static List<string> GetElementsByClassList(string html, string className)
        {
            return GetElementsList(html, "", className);
        }

        #endregion

        #region 通过标签名获取标签集合

        /// <summary>         
        /// 通过标签名获取标签集合   
        /// </summary>        
        /// <param name="html">HTML源码</param>   
        /// <param name="tagName">标签名(如div)</param>    
        /// <returns></returns>       
        public static string[] GetElementsByTagName(string html, string tagName)
        {
            return GetElements(html, tagName, "");
        }

        /// <summary>         
        /// 通过标签名获取标签集合   
        /// </summary>        
        /// <param name="html">HTML源码</param>   
        /// <param name="tagName">标签名(如div)</param>    
        /// <returns></returns>       
        public static List<string> GetElementsByTagNameList(string html, string tagName)
        {
            return GetElementsList(html, tagName, "");
        }

        #endregion

        #region 通过同时指定标签名+class值获取标签集合
        /// <summary>         
        /// 通过同时指定标签名+class值获取标签集合    
        /// </summary>        
        /// <param name="html">HTML源码</param>  
        /// <param name="tagName">标签名</param>   
        /// <param name="className">class值</param>     
        /// <returns></returns>         
        public static string[] GetElementsByTagAndClass(string html, string tagName, string className)
        {
            return GetElements(html, tagName, className);
        }
        #endregion

        
        /// <summary>   通过同时指定标签名+class值获取微信标签集合
        /// </summary>  
        /// <param name="html"></param> 
        /// <param name="tagName"></param> 
        /// <param name="className"></param> 
        /// <returns></returns>  
        public static string[] GetWeiXinElements(string html)
        {
            string pattern = "<p id=\"sogou_vr_\\d+_summary_\\d+\">(.*?)</p>";

            List<string> list = new List<string>();
            Regex reg = new Regex(pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            Match match = reg.Match(html);
            while (match.Success)
            {
                list.Add(match.Value);
                match = reg.Match(html, match.Index + match.Length);
            }
            return list.ToArray();
        }

        #region 通过同时指定标签名+class值获取标签集合（内部方法）
        /// <summary>   通过同时指定标签名+class值获取标签集合（内部方法）
        /// </summary>  
        /// <param name="html"></param> 
        /// <param name="tagName"></param> 
        /// <param name="className"></param> 
        /// <returns></returns>  
        private static string[] GetElements(string html, string tagName, string className)
        {
            string pattern = "";

            if (tagName != "" && className != "")
            {
                //pattern =
                //    @"<({0})(?:(?!class)[^<>])*class=([""']?){1}\2[^>]*>(?>(?<o><\1[^>]*>)|(?<-o></\1>)|(?:(?!</?\ 1).))*(?(o)(?!))</\1>";
                pattern = "<p class=\"c-author\">(.*?)</p>";
                pattern = string.Format(pattern, Regex.Escape(tagName), Regex.Escape(className));

            }
            else if (tagName != "")
            {
                if (tagName == "weixin")
                {
                    pattern = "<div class=\"s-p\" t=\"(.*?)\">";
                    pattern = string.Format(pattern, Regex.Escape(tagName));
                }
                else
                {
                    pattern = @"<({0})(?:[^<>])*>(?>(?<o><\1[^>]*>)|(?<-o></\1>)|(?:(?!</?\1).))*(?(o)(?!))</\1>";
                    pattern = string.Format(pattern, Regex.Escape(tagName));
                }

            }
            else if (className != "")
            {
                if (className == "tieba")
                {
                    pattern = "<div.*?class=\"d_post_content j_d_post_content \".*?>.*?</div>";
                }
                else if (className == "c-summary")
                {
                    pattern = "<div class=\"c-summary c-row \">.*?</div>";
                }else
                {
                    pattern =
                        @"<([a-z]+)(?:(?!class)[^<>])*class=([""']?){0}\2[^>]*>(?>(?<o><\1[^>]*>)|(?<-o></\1>)|(?:(?!</?\1).))*(?(o)(?!))</\1>";
                    pattern = string.Format(pattern, Regex.Escape(className));
                }
            }


            if (pattern == "")
            {
                return new string[] { };
            }

            List<string> list = new List<string>();
            Regex reg = new Regex(pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            Match match = reg.Match(html);
            while (match.Success)
            {
                list.Add(match.Value);
                match = reg.Match(html, match.Index + match.Length);
            }
            return list.ToArray();
        }

        /// <summary>   通过同时指定标签名+class值获取标签集合（内部方法）
        /// </summary>  
        /// <param name="html"></param> 
        /// <param name="tagName"></param> 
        /// <param name="className"></param> 
        /// <returns></returns>  
        private static List<string> GetElementsList(string html, string tagName, string className)
        {
            string pattern = "";

            if (tagName != "" && className != "")
            {
                pattern =
                    @"<({0})(?:(?!class)[^<>])*class=([""']?){1}\2[^>]*>(?>(?<o><\1[^>]*>)|(?<-o></\1>)|(?:(?!</?\ 1).))*(?(o)(?!))</\1>";
                pattern = string.Format(pattern, Regex.Escape(tagName), Regex.Escape(className));

            }
            else if (tagName != "")
            {

                pattern = @"<({0})(?:[^<>])*>(?>(?<o><\1[^>]*>)|(?<-o></\1>)|(?:(?!</?\1).))*(?(o)(?!))</\1>";
                pattern = string.Format(pattern, Regex.Escape(tagName));


            }
            else if (className != "")
            {
                if (className.Equals("weixin"))
                {
                    //string r = "<h4><a.*?href=\"(.*?)\".*?>(.*?)</a></h4><p.*?>(.*?)</p>";
                    //string r = "<h4><a.*?href=\"(.*?)\".*?>(.*?)</a></h4><p.*?>(.*?)</p>.*?<p class=\"s-p\">(.*?)</p>";
                    string r = "<h4><a.*?href=\"(.*?)\".*?>(.*?)</a></h4><p.*?>(.*?)</p>.*?<div class=\"s-p\" t=\"(.*?)\">";
                    pattern = string.Format(r, Regex.Escape(className));
                }
                else
                {
                    //string r = "<h3.*?><a.*?href=\"(.*?)\".*?>(.*?)</a></h3><div class=\"c-abstract\">(.*?)</div>";
                    //string r = "<h3.*?><a.*?href=\"(.*?)\".*?>(.*?)</a></h3><span.*?>(.*?)</span><div class=\"c-summary\">(.*?)</div>";
                    string r = "<h3.*?><a.*?href=\"(.*?)\".*?>(.*?)</a></h3><div class=\"c-summary.*?\">(.*?)</div>";
                    pattern =
                        @"<([a-z]+)(?:(?!class)[^<>])*class=([""']?){0}\2[^>]*>(?>(?<o><\1[^>]*>)|(?<-o></\1>)|(?:(?!</?\1).))*(?(o)(?!))</\1>";
                    pattern = string.Format(r, Regex.Escape(className));
                }
            }


            if (pattern == "")
            {
                return new List<string> { };
            }
            List<string> list = new List<string>();
            try
            {
                Regex reg = new Regex(pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
                Match match = reg.Match(html);
                while (match.Success)
                {
                    list.Add(match.Value);
                    match = reg.Match(html, match.Index + match.Length);
                    //match = match.NextMatch();
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }
        /// <summary>
        /// 获取微信搜索中的时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GetWeiXinTime(long time)
        {
            if (time > 0)
            {
                long lLeft = 621355968000000000;

                long Eticks = (long)(time * 10000000) + lLeft;
                DateTime dt = new DateTime(Eticks).ToLocalTime();
                return dt.ToString();
            }
            else
            {
                return "";
            }
        }
        /// <summary>  
        /// 根据正则获取内容
        /// </summary>  
        /// <param name="text">内容</param> 
        /// <param name="pat">正则</param>  
        public static string[] GetListByHtml(string urlprefix, string text, string pat)
        {
            if (!String.IsNullOrWhiteSpace(urlprefix))
            {
                //Regex r1 = new Regex("href=\"(.*?)\"", RegexOptions.IgnoreCase);
                Regex r1 = new Regex("href=['\"](.*?)['\"]", RegexOptions.IgnoreCase);
                Match m1 = r1.Match(text);
                
                if (m1.Success)
                {
                    string relative_url = urlprefix.Substring(0, urlprefix.LastIndexOf("/") + 1);
                    Regex absolute_r = new Regex("https?://.*?/",RegexOptions.IgnoreCase);
                    Match absolute_m = absolute_r.Match(urlprefix);
                    string absolute_url=null;
                    if (absolute_m.Success)
                    {
                        absolute_url = absolute_m.Groups[0].Value;
                    }
                    if (!m1.Groups[1].ToString().StartsWith("http") && absolute_url != null)
                    {
                        if (m1.Groups[1].Value.StartsWith("/"))
                        {
                            text = r1.Replace(text, "href=\"" + absolute_url + m1.Groups[1].Value.Substring(1) + "\"");
                        }
                        else
                        {
                            text = r1.Replace(text, "href=\"" + relative_url + m1.Groups[1].Value + "\"");
                        }
                    }
                    else
                    {
                        return new string[] { m1.Groups[1].ToString() };
                    }
                }
            }
            List<string> list = new List<string>();
            Regex r = new Regex(pat, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            Match m = r.Match(text);

            //int matchCount = 0; 

            while (m.Success)
            {
                list.Add(m.Value);
                m = m.NextMatch();
            }
            return list.ToArray();
        }
        public static List<string> GetListByHtmlKey(string text, string pat)
        {

            List<string> list = new List<string>();
            Regex r = new Regex(pat, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            Match m = r.Match(text);

            //int matchCount = 0; 

            while (m.Success)
            {
                list.Add(m.Value);
                m = m.NextMatch();
            }
            return list;
        }

        public static List<string> GetListByHtmlArray(string text, string pat)
        {
            List<string> list = new List<string>();
            //if (text.Substring(text.Length - 1, 1) == "/")
            //    text=text.Substring(0, text.Length - 1);
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);
            Match m = r.Match(text);

            //int matchCount = 0; 

            if (m.Value == text)
            {
                list.Add(m.Value);
            }
            //while (m.Success)
            //{
            //    list.Add(m.Value);
            //    m = m.NextMatch();
            //}
            return list;
        }

        /// <summary> 
        /// 去除html标签
        /// </summary>  
        /// <param name="Htmlstring"></param> 
        /// <returns></returns>  
        public static string NoHTML(string Htmlstring)
        {

            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script.*?>.*?</script>", "", RegexOptions.IgnoreCase|RegexOptions.Singleline);
            Htmlstring = Regex.Replace(Htmlstring, @"<style.*?>.*?</style>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            //删除HTML 
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            Htmlstring
            = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = Htmlstring.Trim();
            return Htmlstring;
        }

        /// <summary>  
        /// 取得HTML中所有图片的 URL。 
        /// </summary>        
        /// <param name="sHtmlText">HTML代码</param>   
        /// <returns>图片的URL列表</returns>    
        public static string[] GetHtmlImageUrlList(string sHtmlText)
        {
            // 定义正则表达式用来匹配 img 标签            
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);
            // 搜索匹配的字符串         
            MatchCollection matches = regImg.Matches(sHtmlText);
            int i = 0;
            string[] sUrlList = new string[matches.Count];
            // 取得匹配项列表          
            foreach (Match match in matches)
                sUrlList[i++] = match.Groups["imgUrl"].Value;
            return sUrlList;
        }
        /// <summary>         
        ///  /// 获取img的alt标签     
        ///     /// </summary>    
        ///       /// <param name="strHtml"></param>     
        ///     /// <returns></returns>       
        public static string[] GetHtmlAltList(string strHtml)
        {
            // 定义正则表达式用来匹配 img 标签         
            Regex regImg = new Regex(@"<img\b[^<>]*?\balt[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgAlt>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            // 搜索匹配的字符串    
            MatchCollection matches = regImg.Matches(strHtml);
            int i = 0;
            string[] sUrlList = new string[matches.Count];
            // 取得匹配项列表         
            foreach (Match match in matches)
                sUrlList[i++] = match.Groups["imgAlt"].Value;
            return sUrlList;
        }
        #endregion

        /// <summary>
        /// 对比相似度
        /// </summary>
        /// <param name="txt1">源网址</param>
        /// <param name="txt2">止标网址</param>
        /// <returns>相似值</returns>
        public static double getSimilarDegree(string txt1, string txt2)
        {
            int[] itemCountArray = null;
            Dictionary<string, int[]> vectorSpace = new Dictionary<string, int[]>();
            string[] strArray = txt1.Split('/');
            for (int i = 0; i < strArray.Length; ++i)
            {
                if (vectorSpace.ContainsKey(strArray[i]))
                {
                    ++(vectorSpace[strArray[i]][0]);
                }
                else
                {
                    itemCountArray = new int[2];
                    itemCountArray[0] = 1;
                    itemCountArray[1] = 0;
                    vectorSpace.Add(strArray[i], itemCountArray);
                }
            }
            strArray = txt2.Split('/');
            for (int i = 0; i < strArray.Length; ++i)
            {
                if (vectorSpace.ContainsKey(strArray[i]))
                {
                    ++(vectorSpace[strArray[i]][1]);
                }
                else
                {
                    itemCountArray = new int[2];
                    itemCountArray[0] = 1;
                    itemCountArray[1] = 0;
                    vectorSpace.Add(strArray[i], itemCountArray);
                }
            }

            //计算相似度
            double vector1module = 0.00;
            double vector2module = 0.00;
            double vectorproduct = 0.00;
            IEnumerable<string> iter = vectorSpace.Keys.AsEnumerable();
            IEnumerator<string> iter1 = vectorSpace.Keys.GetEnumerator();
            while (iter1.MoveNext())
            {
                string key = iter1.Current;
                itemCountArray = (int[])vectorSpace[key];
                vector1module += itemCountArray[0] * itemCountArray[0];
                vector2module += itemCountArray[1] * itemCountArray[1];

                vectorproduct += itemCountArray[0] * itemCountArray[1];
            }

            vector1module = Math.Sqrt(vector1module);
            vector2module = Math.Sqrt(vector2module);

            double sim = (vectorproduct / (vector1module * vector2module));
            return sim;
        }

        /// <summary>
        /// 返回目标网页源码
        /// </summary>
        /// <param name="WebUrl">网络地址</param>
        /// <param name="Encodes">编码</param>
        /// <returns></returns>
        public static string GetWebCode(string WebUrl, string Encodes)
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
        /// 获取网页图片
        /// </summary>
        /// <param name="strHtml">网页源码</param>
        /// <param name="Encodes">编码</param>
        /// <param name="imagePath">图片存放地址</param>
        public static void getWebImage(string strHtml, string Encodes, string imagePath)
        {
            SDWebCache wc = new SDWebCache(HtmlUtil.GetWebCode(strHtml, Encodes));
            Bitmap image = wc.Snap();
            image.Save(imagePath);
        }

        public static string UrlCl(string str)
        {
            if (!String.IsNullOrWhiteSpace(str) && str.IndexOf(" ") != -1)
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
        public static string getConnect(string Htmlstr)
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

        public static string getHtml(string url, string charSet)//url是要访问的网站地址，charSet是目标网页的编码，如果传入的是null或者""，那就自动分析网页的编码
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
                //myWebClient.Credentials = CredentialCache.DefaultCredentials;
                //如果服务器要验证用户名,密码 
                //NetworkCredential mycred = new NetworkCredential(struser, strpassword);
                //myWebClient.Credentials = mycred;
                //从资源下载数据并返回字节数组。（加@是因为网址中间有"/"符号）
                byte[] myDataBuffer = myWebClient.DownloadData(url);
                string strWebData = Encoding.Default.GetString(myDataBuffer);
                strWebData = strWebData.Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("\\", "");
                //获取网页字符编码描述信息 
                Match charSetMatch = Regex.Match(strWebData, "<meta([^<]*)charset=[\"]?(.*?)[\"]", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                string webCharSet = charSetMatch.Groups[2].Value;
                webCharSet = webCharSet.Replace("\"", "").Replace("\\", "");
                if (charSet == null || charSet == "")
                    charSet = webCharSet;

                try
                {
                    if (charSet != null && charSet != "" && Encoding.GetEncoding(charSet) != Encoding.Default)
                    {
                        strWebData = Encoding.GetEncoding(charSet).GetString(myDataBuffer);
                        strWebData = strWebData.Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("\\", "");
                    }
                    else
                    {
                        strWebData = GetHtmlSource1(url);
                    }
                }
                catch (Exception e)
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

        //快速排序
        public static void Sort(string[][] a, int left, int right, int index)
        {
            if (left < right)
            {
                int i = Partition(a, left, right, index);
                Sort(a, left, i - 1, index);
                Sort(a, i + 1, right, index);
            }
        }

        public static int Partition(string[][] a, int left, int right, int index)
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

        public static string GetstringByHtmlArray(string text, string pat)
        {

            Regex r = new Regex(pat, RegexOptions.IgnoreCase);
            Match m = r.Match(text);

            return m.Value;
        }

        #region 网络相关，wancg
        public static string HttpGet(string url, Encoding encode)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Timeout = 1000 * 30; //30秒超时
                req.MaximumAutomaticRedirections = 2000;
                HttpWebResponse receiveStream = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(receiveStream.GetResponseStream(), encode);
                string content = reader.ReadToEnd();
                return content;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string HttpGet(string url, Encoding encode, CookieContainer cookies, string domain, ref string cookiesstr)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Timeout = 1000 * 30; //30秒超时
                req.MaximumAutomaticRedirections = 2000;
                req.CookieContainer = new CookieContainer(); //暂存到新实例                
                HttpWebResponse receiveStream = (HttpWebResponse)req.GetResponse();
                cookies = req.CookieContainer; //保存cookies
                cookiesstr = req.CookieContainer.GetCookieHeader(req.RequestUri); //把cookies转换成字符串
                addCookieToContainer(cookiesstr, cookies, domain);
                StreamReader reader = new StreamReader(receiveStream.GetResponseStream(), encode);
                string content = reader.ReadToEnd();
                return content;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static CookieContainer addCookieToContainer(string cookie, CookieContainer cc, string domain)
        {
            string[] tempCookies = cookie.Split(';');
            string tempCookie = null;
            int Equallength = 0;//  =的位置 
            string cookieKey = null;
            string cookieValue = null;
            //qg.gome.com.cn  cookie 
            for (int i = 0; i < tempCookies.Length; i++)
            {
                if (!string.IsNullOrEmpty(tempCookies[i]))
                {
                    tempCookie = tempCookies[i];
                    Equallength = tempCookie.IndexOf("=");

                    if (Equallength != -1)       //有可能cookie 无=，就直接一个cookiename；比如:a=3;ck;abc=; 
                    {
                        cookieKey = tempCookie.Substring(0, Equallength).Trim();
                        if (Equallength == tempCookie.Length - 1)    //这种是等号后面无值，如：abc=; 
                        {
                            cookieValue = "";
                        }
                        else
                        {
                            cookieValue = tempCookie.Substring(Equallength + 1, tempCookie.Length - Equallength - 1).Trim();
                        }
                    }
                    else
                    {
                        cookieKey = tempCookie.Trim();
                        cookieValue = "";
                    }
                    cc.Add(new Cookie(cookieKey, cookieValue, "", domain));
                }

            }

            return cc;
        }

        #endregion
    }
}
