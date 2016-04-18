using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
using System.Management;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using log4net;
using System.Reflection;

namespace HtmlParse
{
    public class Comm
    {    
        /// 去除html标签
        public static string NoHTML(string Htmlstring)
        {
            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML 
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = Htmlstring.Trim();
            return Htmlstring;
        }

        public static bool isNumeric(string str)
        {
            Regex regx = new Regex(@"^\d+(\.\d+)?$");
            return regx.IsMatch(str);
        }

        public static bool isMobile(string str)
        {
            Regex regx = new Regex(@"^[1]+[3,5,8]+\d{9}$");
            return regx.IsMatch(str);
        }

        public static bool isInteger(string str)
        {
            string pattern = @"^\d+$";
            return Regex.IsMatch(str, pattern);
        }
 
        /// <summary>
        /// 根据毫秒转为时间
        /// </summary>
        /// <param name="ticks">毫秒数</param>
        /// <returns>时间（含日期）</returns>
        public static string getTime(long ticks)
        {
            string time = "";
            DateTime s = new DateTime(1970, 01, 1, 08, 00, 00);
            s = s.AddSeconds(ticks);
            time = s.ToString("yyyy-MM-dd HH:mm:ss");
            return time;
        }

        //判断str1中包含str2的个数 
        public static int partCount(string str, string constr)
        {
            return Regex.Matches(str, constr).Count;
        }

        public static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static void WriteErrorLog(string msg)
        {
            log.Error(msg);
        }
    }
}
