using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System;
using System.Net;
using System.IO;
using System.Text;

namespace HtmlParse
{
    public class HtmlUtil
    {
        /// <summary> 
        /// 去除html标签
        /// </summary>  
        /// <param name="Htmlstring"></param> 
        /// <returns></returns>  
        public static string NoHTML(string Htmlstring)
        {

            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script.*?>.*?</script>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
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
    }
}
