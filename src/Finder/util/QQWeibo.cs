using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Timers;
using System.Data;
using DataBaseServer;
using System.Net;
using System.Xml;
using System.IO;
using System.Runtime.CompilerServices;
using System.Collections;

namespace Finder.util
{
    class QQWeibo
    {
        util.XmlUtil xmlUtil = new util.XmlUtil();
        string appKey = "";
        string appSecret = "";
        string accessToken;// = System.Configuration.ConfigurationSettings.AppSettings["QQWbAccessToken"];
        string openId;// = System.Configuration.ConfigurationSettings.AppSettings["QQWbOpenId"];
        string callBackUrl;// = System.Configuration.ConfigurationSettings.AppSettings["QQWbCallbackUrl"];
        string openKey;// = System.Configuration.ConfigurationSettings.AppSettings["QQWbOpenKey"];
        string RefreshToken;// = System.Configuration.ConfigurationSettings.AppSettings["QQWbRefreshToken"];
        string expire_in;
        string fetchDateStr;

        private System.Timers.Timer timer;

        //关键词
        string[] keywords = null;
        SQLitecommand cmd = new SQLitecommand();
        DataTable dt_kw = null;  //关键字表
        DataTable dt_partWord = null;  //正负词表
        DataTable dt_event = null;   //事件列表
        long public_timeline_pos = 0;
        //客户端IP
        //string clientIp = Comm.GetIp();
        string weibo_ids = "";  //含有关键字的微博ids

        public QQWeibo(int rate)
        {
            appKey = "801426797";
            appSecret = "0e9382190a5f764b57e09c15cda96e09";
            accessToken = xmlUtil.GetValue("QQWbAccessToken");
            openId = xmlUtil.GetValue("QQWbOpenId");
            callBackUrl = xmlUtil.GetValue("QQWbCallbackUrl");
            openKey = xmlUtil.GetValue("QQWbOpenKey");
            RefreshToken = xmlUtil.GetValue("QQWbRefreshToken");
            expire_in = xmlUtil.GetValue("QQWbExpire_in");
            fetchDateStr = xmlUtil.GetValue("QQWbFetchAccessTokenDate");

            timer = new System.Timers.Timer(rate);
            timer.AutoReset = true;
            timer.Elapsed +=new ElapsedEventHandler(timer_Elapsed);
            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(openKey) || string.IsNullOrEmpty(openId) || string.IsNullOrEmpty(RefreshToken) || string.IsNullOrEmpty(expire_in) || string.IsNullOrEmpty(fetchDateStr))
            {
                if (MessageBox.Show("腾讯微博未授权或授权已过期，请在系统设置中进行授权！", "注意", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.OK)
                {
                    Finder.QQWeibo weibo = new Finder.QQWeibo();
                    if (weibo.ShowDialog() == DialogResult.OK)
                    {
                        accessToken = xmlUtil.GetValue("QQWbAccessToken");
                        openId = xmlUtil.GetValue("QQWbOpenId");
                        callBackUrl = xmlUtil.GetValue("QQWbCallbackUrl");
                        openKey = xmlUtil.GetValue("QQWbOpenKey");
                        RefreshToken = xmlUtil.GetValue("QQWbRefreshToken");
                        expire_in = xmlUtil.GetValue("QQWbExpire_in");
                        fetchDateStr = xmlUtil.GetValue("QQWbFetchAccessTokenDate");
                    }
                }
            }
            timer.Enabled = false;
        }
        //开始抓取微博
        public void Start()
        {
            timer.Enabled = true;
        }
        //停止抓取微博
        public void Stop()
        {
            timer.Enabled = false;
        }
        /// <summary>
        /// AccessToken续期
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private bool RenewToken()
        {
            if (!string.IsNullOrEmpty(fetchDateStr))
            {
                //转换为时间格式
                DateTime fetchDate = DateTime.Parse(fetchDateStr);
                fetchDate = fetchDate.AddSeconds(double.Parse(expire_in));
                TimeSpan ts = DateTime.Now - fetchDate;
                //判断现在的时间减去第一次或刷新时获取的AccessToken的时间，如果小时数小于等于1，则表示该续期了。然后刷新AccessToken
                if (ts.TotalHours > -1)
                {
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://open.t.qq.com/cgi-bin/oauth2/access_token?client_id=" + appKey + "&grant_type=refresh_token&refresh_token=" + RefreshToken);
                    req.Method = "GET";
                    WebResponse response = req.GetResponse();
                    StreamReader sr = new StreamReader(response.GetResponseStream());
                    string content = sr.ReadToEnd();
                    string[] c = content.Split(new char[] { '=', '&' });
                    expire_in = c[3];
                    xmlUtil.SetValue("QQWbExpire_in", c[3]);
                    fetchDateStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    xmlUtil.SetValue("QQWbFetchAccessTokenDate", fetchDateStr);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        private void timer_Elapsed(object sender, EventArgs arg)
        {
            try
            {
                if (!Program.ProClose)
                {
                    //检查并刷新AccessToken;
                    if (RenewToken())
                    {
                        dt_kw = cmd.GetTabel("select KeyWord from KeyWords group by KeyWord");
                        dt_event = cmd.GetTabel("select name,keyword,kid from KeyWords group by name,keyword,kid");
                        dt_partWord = cmd.GetTabel("select word,part from partword");
                        keywords = new string[dt_kw.Rows.Count];
                        for (int i = 0; i < dt_kw.Rows.Count; i++)
                        {
                            keywords[i] = dt_kw.Rows[i][0].ToString();
                        }
                        getPublic_Timeline();
                        getFriends_Timeline();
                    }
                }
            }
            catch (Exception ex)
            {
                timer.Enabled = false;
                Console.WriteLine(ex.Message);
            }
        }
        //获取公共微博时间线
        private void getPublic_Timeline()
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://open.t.qq.com/api/statuses/public_timeline?oauth_consumer_key=" + appKey + "&access_token=" + accessToken + "&openid=" + openId + "&oauth_version=2.a&scope=all&format=xml&pos=0&reqnum=70 ");
            req.Method = "GET";
            WebResponse response = req.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream());
            string xml = sr.ReadToEnd();
            parseXml(xml, true);
        }
        /// <summary>
        /// 获取用户主时间线
        /// </summary>
        private void getFriends_Timeline()
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://open.t.qq.com/api/statuses/home_timeline?format=xml&pageflag=0&pagetime=0&reqnum=70&type=0&contenttype=0&oauth_consumer_key="+appKey+"&access_token="+accessToken+"&openid="+openId+"&oauth_version=2.a&scope=all");
            req.Method = "GET";
            WebResponse response = req.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream());
            string xml = sr.ReadToEnd();
            parseXml(xml, false);
        }
        /// <summary>
        /// 解析微博api返回的数据，并添加数据库
        /// </summary>
        /// <param name="xml"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void parseXml(string xml, bool isPublicTimeline)
        {
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(xml);
                if (xmldoc != null && xmldoc.ChildNodes.Count > 0)
                {
                    XmlNode data = xmldoc.SelectSingleNode("//data");
                    if (isPublicTimeline)
                    {
                        XmlNode t = data.SelectSingleNode("//pos");
                        if (t == null)
                        {
                            return;
                        }
                        public_timeline_pos = int.Parse(t.InnerText);
                    }
                    XmlNodeList xnl = data.SelectNodes("//info");
                    StringBuilder sql = new StringBuilder();
                    ArrayList lst_kw = new ArrayList();
                    Hashtable ht_evt = new Hashtable();

                    string text = "";
                    string origtext = "";
                    string reposts = "";
                    string comments = "";
                    string infosource = "";
                    string nick = "";
                    string type = "";
                    string timestamp = "";
                    string weibo_id = "";
                    string name = "";

                    //foreach (XmlNode xn in xnl)
                    for(int i=0;i<xnl.Count;i++)
                    {
                        //XmlNode info = xn;
                        XmlNode info = xnl[i];
                        //内容
                        text = info.SelectSingleNode("text") == null ? "" : info.SelectSingleNode("text").InnerText;
                        //原始内容
                        origtext = info.SelectSingleNode("origtext") == null ? "" : info.SelectSingleNode("origtext").InnerText;
                        //转发次数
                        reposts = info.SelectSingleNode("count") == null ? "" : info.SelectSingleNode("count").InnerText;
                        //点评次数
                        comments = info.SelectSingleNode("mcount") == null ? "" : info.SelectSingleNode("mcount").InnerText;
                        //来源
                        infosource = info.SelectSingleNode("from") == null ? "" : info.SelectSingleNode("from").InnerText;
                        //发布人
                        nick = info.SelectSingleNode("nick") == null ? "" : info.SelectSingleNode("nick").InnerText;
                        //微博类型。1-原创发表，2-转载，3-私信，4-回复，5-空回，6-提及，7-评论
                        type = info.SelectSingleNode("type") == null ? "" : info.SelectSingleNode("type").InnerText;
                        //发表时间
                        timestamp = info.SelectSingleNode("timestamp") == null ? "" : info.SelectSingleNode("timestamp").InnerText;
                        //微博id
                        weibo_id = info.SelectSingleNode("id") == null ? "" : info.SelectSingleNode("id").InnerText;
                        //博主name,拼微博链接用
                        name = info.SelectSingleNode("name") == null ? "" : info.SelectSingleNode("name").InnerText;

                        StringBuilder sb_keys = new StringBuilder();
                        if (type == "1" || type == "2")
                        {
                            if (!weibo_ids.Equals(weibo_id) && long.Parse(weibo_ids.Equals("") ? "0" : weibo_ids) < long.Parse(weibo_id))
                            {
                                foreach (string key in keywords)
                                {

                                    string[] _words = key.Split(new char[] { ' ' });
                                    if (!lst_kw.Contains(key))
                                    {
                                        lst_kw.Add(key);
                                        foreach (string w in _words)
                                        {
                                            if (!origtext.Contains(w))
                                            {
                                                lst_kw.Remove(key);
                                                break;
                                            }
                                        }
                                    }
                                }

                                ht_evt = evtJudge(dt_event, lst_kw);
                                if (ht_evt.Count > 0)
                                {
                                    int part = partJudge(dt_partWord, origtext);  //判断正负向 
                                    weibo_ids = weibo_id;
                                    foreach (DictionaryEntry de in ht_evt)
                                    {
                                        int urlTherear = UrlThereare(weibo_id);
                                        if (urlTherear <= 0)
                                        {
                                            sql.Append("insert into ReleaseInfoWB (title,contexts,releasedate,infosource,keywords,releasename,collectdate,snapshot,webname,pid,part,reposts,comments,kid) values ('" + weibo_id + "','"
                                                    + origtext + "','" + Comm.getTime(long.Parse(timestamp)) + "','" + "http://t.qq.com/" + name + "','" + de.Key.ToString() + "','"
                                                    + nick + "','" + string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")) + "','','腾讯微博','3','" + part.ToString() + "','" + reposts + "','" + comments + "'," + de.Value.ToString() + ");");
                                            //Console.WriteLine(sql);
                                        }
                                    }
                                }
                                lst_kw.Clear();
                            }
                        }
                    }
                    if (sql.Length > 0) cmd.ExecuteNonQueryInt(sql.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //判断str1中包含str2的个数 
        public int partCount(string str, string constr)
        {
            return System.Text.RegularExpressions.Regex.Matches(str, constr).Count;
        }

        //判断正负向 
        private int partJudge(DataTable dt, string text)
        {
            int part = 0;
            int part_Z_count = 0;
            int part_F_count = 0;
            for (int j = 0; j <= dt.Rows.Count - 1; j++)
            {
                if (text.Contains(dt.Rows[j][0].ToString()))
                {
                    if (dt.Rows[j][1].ToString().Equals("0"))
                    {
                        part_F_count = part_F_count + partCount(text, dt.Rows[j][0].ToString());
                    }
                    else
                    {
                        part_Z_count = part_Z_count + partCount(text, dt.Rows[j][0].ToString());
                    }
                }
            }
            if (part_Z_count >= part_F_count)
            {
                part = 1;
            }
            return part;
        }

        //根据正文包含的关键词判断哪个事件出现
        //参数1为查询的:事件,关键字,事件kid数据表;参数2为一篇文章所包含的的关键字集合
        private Hashtable evtJudge(DataTable dt, ArrayList al)
        {
            Hashtable retHt = new Hashtable();
            string evt_name, evt_kw, evt_kid = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                evt_name = dt.Rows[i][0].ToString();
                evt_kw = dt.Rows[i][1].ToString();
                evt_kid = dt.Rows[i][2].ToString();
                if (al.Contains(evt_kw))
                {
                    if (retHt.Contains(evt_name) == false)
                    {
                        retHt.Add(evt_name, evt_kid);   //满足条件的事件加入返回结果
                    }
                }
            }
            return retHt;
        }

        //从正文包含的关键词判断哪个事件出现
        //参数1为查询的事件,关键字,事件kid数据表;参数2为文章所包含的的关键字集合
        private Hashtable evtJudge1(DataTable dt, ArrayList al)
        {
            Hashtable retHt = new Hashtable();
            string evt_name, evt_kw, evt_kid = "";
            int flag = 0;   //0为此文章所有关键字满足事件,1为不满足
            evt_name = dt.Rows[0][0].ToString();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (evt_name.Equals(dt.Rows[i][0].ToString()))
                {
                    evt_kw = dt.Rows[i][1].ToString();
                    evt_kid = dt.Rows[i][2].ToString();
                    if (al.Contains(evt_kw))
                    {
                        flag = 0;
                    }
                    else
                    {
                        flag = 1;
                        continue;
                    }
                    evt_name = dt.Rows[i][0].ToString();
                }
                else
                {
                    if (flag == 0) retHt.Add(evt_name, evt_kid);   //满足条件的事件加入返回结果

                    evt_kw = dt.Rows[i][1].ToString();
                    evt_kid = dt.Rows[i][2].ToString();
                    if (al.Contains(evt_kw))
                    {
                        flag = 0;
                    }
                    else
                    {
                        flag = 1;
                        continue;
                    }
                    evt_name = dt.Rows[i][0].ToString();
                }
            }
            return retHt;
        }
        private int UrlThereare(string id)
        {
            string sql = "select count(0) from ReleaseInfoWB where title='" + id + "'";
            int result = int.Parse(cmd.GetOne(sql).ToString());
            return result;
        }
    }
}
