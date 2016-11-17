using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using HtmlParse;

namespace Finder.util
{
    public class CrawlHtml
    {
        
        public static ModelReleaseInfo CrawlHtmlSource(string html,string url, DataTable dtkey, string sheng,string shi,string xian,string webName,string webInfo,int pid)
        {
            //string strURLformat = "https?://.[^\"]+";
            Dictionary<string, int> events = new Dictionary<string, int>();
            //创建数据对象
            ModelReleaseInfo newsInfo = new ModelReleaseInfo();
            try
            {
                newsInfo.Title = HtmlUtil.NoHTML(html);
                //newsInfo.Title = html;
                for (int j = 0; j < dtkey.Rows.Count; j++)
                {
                    string[] keys = dtkey.Rows[j][4].ToString().Split(new char[] { ' ' });
                    if (!events.ContainsKey(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString()))
                    {
                        events.Add(dtkey.Rows[j][1].ToString() + "-" + dtkey.Rows[j][6].ToString(), 1);
                        foreach (string k in keys)
                        {
                            if (!html.ToLower().Contains(k.ToLower()))
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
                newsInfo.Contexts = HtmlUtil.NoHTML(webInfo);

                //网站链接
                newsInfo.InfoSource = url;

                //关键字的设置
                if (newsInfo.KeyWords == null || newsInfo.KeyWords.Length == 0)
                {
                    for (int j = 0; j < dtkey.Rows.Count; j++)
                    {
                        //Application.DoEvents();
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
                //if (newsInfo.KeyWords.Length == 0) { continue; }
                if (newsInfo.KeyWords != null)
                {
                    newsInfo.KeyWords = newsInfo.KeyWords.Substring(1);
                }

                //收集日期
                newsInfo.CollectDate = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                //发布人和发布日期暂时无法取到,手工赋值为空
                newsInfo.ReleaseDate = "";
                newsInfo.ReleaseName = "";

                //网页快照,这里为用户指定生成,如果未选择生成,那么为空
                newsInfo.Snapshot = "";
                newsInfo.Sheng = sheng == null ? "" : sheng;
                newsInfo.Shi = shi == null ? "" : shi;
                newsInfo.Xian = xian == null ? "" : xian;
                //网站名
                newsInfo.WebName = webName == null ? "" : webName;
                //pid
                newsInfo.Pid = pid;
                //part正负判断
                newsInfo.Part = GetParts(newsInfo.Contexts);
                //reposts
                newsInfo.Reposts = 0;
                //comments
                newsInfo.Comments = 0;
            }
            catch (Exception ex)
            {
                Comm.WriteErrorLog(ex.StackTrace);
            }

            return newsInfo;
        }

        /// <summary>
        /// 正文的正负判断
        /// </summary>
        /// <param name="str1">正文</param>
        /// <returns></returns>
        public static int GetParts(string str1)
        {
            DataBaseServer.MySqlCmd cmd = new DataBaseServer.MySqlCmd();
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

        //判断url是否已经被抓取过，被抓取过的，就不再抓取
        public static bool UrlExist(string url)
        {
            bool result = false;
            DataBaseServer.MySqlCmd cmd = new DataBaseServer.MySqlCmd();
            String sql = "select count(0) from urls where url='"+url+"'";
            int count = int.Parse(cmd.GetOne(sql).ToString());
            result = count > 0 ? true : false;
            return result;
        }

        public static string processUrl(string dxurl,string url)
        {
            string processdUrl = "";
            string[] _a = HtmlUtil.GetListByHtml(dxurl, url, "https?://.[^\"]+");
            if (_a.Length > 0)
            {
                processdUrl = _a[0];
            }
            else
            {
                processdUrl = "";
            }
            //处理含有单引号的链接
            processdUrl = HtmlUtil.UrlCl(processdUrl);

            //处理单引号的链接
            if (processdUrl.IndexOf("'") != -1 || processdUrl.IndexOf("\"") != -1)
            {
                processdUrl = HtmlUtil.GetstringByHtmlArray(processdUrl, "https?://.[^\']+");
            }
            return processdUrl;
        }

        /// <summary>
        /// 将中文编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }

            return (sb.ToString());
        }

        static string ChEncodeUrl(string str)
        {
            byte[] byt = Encoding.Default.GetBytes(str);
            string ret = HttpUtility.UrlEncode(byt);
            return ret;
        }

        public static string ChDecodeUrl(string str)
        {
            List<string> words = new List<string>();
            //for (int i = 0; i < str.Length; i++)
            //{
            //    if(str.Substring(i,2)=="\u")
            //    {
            //    string tmp=
            //    }
            //}
            string ret = HttpUtility.UrlDecode(str,System.Text.Encoding.Unicode);
            return ret;
        }
    }
}
