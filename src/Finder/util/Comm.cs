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

namespace Finder.util
{
    public class Comm
    {
        public static void VisiteUrl(string url)
        {
            System.Diagnostics.Process.Start(url);
        }
        
        //返回描述本地计算机上的网络接口的对象(网络接口也称为网络适配器)。
        public static NetworkInterface[] NetCardInfo()
        {
          return NetworkInterface.GetAllNetworkInterfaces();
        }

        // 通过NetworkInterface读取网卡Mac。
        // 1）如果当前的网卡是禁用状态（硬件处于硬关闭状态），取不到该网卡的MAC地址，（您可以通过禁用网卡进行试验）。
        // 2）如果当前启用了多个网卡，最先返回的地址是最近启用的网络连接的信息
        public static List<string> GetMac()
        {
            List<string> macs = new List<string>();
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface ni in interfaces)
            {
                macs.Add(ni.GetPhysicalAddress().ToString());
            }
            return macs;
        }

        // 获取cpuid。此方法借用WMI服务，需要服务器开启WMI服务。否则无法获取。
        public static string GetCpu()
        {
            string cpuInfo = "";
            ManagementClass cimobject = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = cimobject.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
            }
            return cpuInfo;
        }

        // 获取硬盘ID。此方法借用WMI服务，需要服务器开启WMI服务。否则无法获取。
        public static string GetHd()
        {
            String HDid = "";
            ManagementClass cimobject1 = new ManagementClass("Win32_DiskDrive");
            ManagementObjectCollection moc1 = cimobject1.GetInstances();
            foreach (ManagementObject mo in moc1)
            {
                HDid = (string)mo.Properties["Model"].Value;
            }
            HDid = HDid.Substring(HDid.IndexOf(" ") + 1, 5);
            return HDid;
        }


        // 获取本机外网IP地址信息，包含所在城市
        public static string GetIpInfo()
        {
            try
            {
                string strUrl = "http://iframe.ip138.com/ic.asp";
                Uri uri = new Uri(strUrl);
                WebRequest wr = WebRequest.Create(uri);
                Stream s = wr.GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(s, Encoding.Default);
                string all = sr.ReadToEnd();
                all = NoHTML(all);
                return all;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// 返回大写形式的16位md5加密
        public static string Md5(string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(str)), 4, 8);
            t2 = t2.Replace("-", "");
            t2 = t2.ToUpper();
            return t2;
        }

        /// 获取机器码
        public static string GetMachineCode()
        {
            string code = "";
            try
            {
                string cpu = GetCpu();
                string mac = GetMac()[0];
                string hd = GetHd();
                code = cpu.Substring(0, 3) + mac.Substring(0, 3) + hd.Substring(0, 3);
                code = new string(code.Reverse().ToArray());
                code = Md5(code);
            }
            catch (Exception ex)
            {
                //string[] arr = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            }
            return code;
        }

        /// 获取本机外网IP地址
        public static string GetIp()
        {
            string ipInfo = GetIpInfo();
            if (ipInfo != null)
            {
                int i = ipInfo.IndexOf("[") + 1;
                string tempip = ipInfo.Substring(i, 15);
                string ip = tempip.Replace("]", "").Replace(" ", "");
                return ip;
            }
            else
            {
                return "";
            }
        }

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

        public static string register(string mc,string sn)
        {
            if (string.IsNullOrEmpty(mc) || string.IsNullOrEmpty(sn) || mc.Length != 16 || sn.Length != 16)
            {
                return null;
            }
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://www.shangwukaocha.cn/finder/regist.php?mc=" + mc + "&sn=" + sn);
                req.Method = "GET";
                req.Timeout = 5000;
                Stream stream = req.GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(stream, Encoding.UTF8);
                string content = sr.ReadToEnd();
                if (!string.IsNullOrEmpty(content) && content.Length == 16)
                {
                    return content;
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取超级狗中的数据文件内容
        /// </summary>
        /// <param name="FileId">数据文件ID</param>
        /// <returns></returns>
        public static string GetDogFile(int FileId)
        {
            SuperDog.Dog dog = new SuperDog.Dog(SuperDog.DogFeature.Default);
            SuperDog.DogStatus dogSta = dog.Login(DogVendorCode.Code);
            if (dogSta == SuperDog.DogStatus.StatusOk)
            {
                //读取第一个数据文件（即用户名)
                SuperDog.DogFile df = dog.GetFile(FileId);
                if (df.IsLoggedIn())
                {
                    int size = 0;
                    dogSta = df.FileSize(ref size);
                    if (dogSta == SuperDog.DogStatus.StatusOk)
                    {
                        byte[] bytes = new byte[size];
                        dogSta = df.Read(bytes, 0, bytes.Length);
                        if (SuperDog.DogStatus.StatusOk == dogSta)
                        {
                            string aaa = Encoding.ASCII.GetString(bytes);
                            if (dog.IsLoggedIn()) { dog.Logout(); }
                            return aaa;
                        }
                    }
                }
            }
            if (dog.IsLoggedIn()) { dog.Logout(); }
            return "";
        }
        
        [DllImport("wininet")]
        private extern static bool InternetGetConnectedState(out int connectionDescription, int reservedValue);

        /// <summary>
        /// 判断是否有网络连接
        /// </summary>
        /// <returns></returns>
        public static bool IsNetworkConnectd()
        {
            int i = 0;
            if (InternetGetConnectedState(out i, 0))
            {
                //联网
                return true;
            }
            else
            {
                //断网
                return false;
            }
        }

        public static bool IsServiced()
        {
            try
            {
                //TODO:服务测试页面
                string isServiced = HtmlUtil.getHtml("http://www.zhangyaojiang.com/yqserviced.html","UTF-8");
                //string isServiced = HtmlUtil.getHtml("http://www.shangwukaocha.cn/", "UTF-8");
                return isServiced.Equals("1") ? true : false;
            }
            catch (Exception e)
            {
                return true;
            }
        }


        public static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static void WriteErrorLog(string msg)
        {
            //lock (msg)
            //{
            //    File.AppendAllText("log.txt", msg);
            //}

            log.Error(msg);
        }
    }
}
