using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetDimension.Weibo;
using DataBaseServer;
using System.IO;
using System.Collections;

namespace Finder.util
{
    class SinaWeibo
    {
        string app_key = "";
        string app_secret = "";       
        string callback_url = "";     
        string access_token = "";

        System.Timers.Timer t = null;
        Client sina = null;
        SQLitecommand cmd = null;
        DataTable dt_kw = null;  //关键字表
        DataTable dt_partWord = null;  //正负词表
        DataTable dt_event = null;   //事件列表
        string weibo_ids = "";  //含有关键字的微博ids
        util.XmlUtil xmlutil = new util.XmlUtil();
        AccessToken at = new AccessToken();
        OAuth oauth = null;

        public SinaWeibo(int rate)
        {
            app_key = "906953775";
            app_secret = "67c042f6ccd8ba4d7e592566b53b3bc5"; 
            callback_url = xmlutil.GetValue("CallbackUrl");
            access_token = xmlutil.GetValue("AccessToken");

            //取得授权过的新浪客户端,给全局变量sina赋值
            getSinaClient();

            //设置间隔时间为120秒
            t = new System.Timers.Timer(rate);
            //构造方法里面定义多线程计时器
            t.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            //设置是执行一次（false）还是一直执行(true)   
            t.AutoReset = true;
            //是否执行System.Timers.Timer.Elapsed事件
            t.Enabled = true;
        }

        /// <summary>
        /// 开始抓取微博内容
        /// </summary>
        public void Start()
        {
            t.Enabled = true;
        }
        /// <summary>
        /// 停止抓取微博内容
        /// </summary>
        public void Stop()
        {
            t.Enabled = false;
        }

        public void timer_Elapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            if (!Program.ProClose)
            {
                getSinaClientAuto();
                cmd = new SQLitecommand();
                dt_kw = cmd.GetTabel("select KeyWord from KeyWords group by KeyWord");
                dt_event = cmd.GetTabel("select name,keyword,kid from KeyWords group by name,keyword,kid");
                dt_partWord = cmd.GetTabel("select word,part from partword");
                getPublic_Timeline_sina(sina, 200);
                getFriends_Timeline_sina(sina, 100);
            }
        }

        //由通过认证的新浪客户端，调取公共微博接口，获得最新的n条含有用户关键字的公共微博信息并存入数据库Public_Timeline
        //n最大是一次200条
        //经测试，200条微博信息，8个关键字大概在10秒内完成
        public void getPublic_Timeline_sina(Client sinaClient, int n)
        {
            StringBuilder sql = new StringBuilder();
            ArrayList lst_kw = new ArrayList();
            Hashtable ht_evt = new Hashtable();
            try
            {
                NetDimension.Weibo.Entities.status.Collection satColl = sinaClient.API.Entity.Statuses.PublicTimeline(n);
                foreach (NetDimension.Weibo.Entities.status.Entity entity in satColl.Statuses)
                {
                    string text = entity.RetweetedStatus == null ? entity.Text : entity.Text + entity.RetweetedStatus.Text;
                    //把正文里包含的关键词都放入list
                    for (int i = 0; i < dt_kw.Rows.Count; i++)
                    {
                        string[] _words = dt_kw.Rows[i][0].ToString().Split(new char[]{' '});
                        if (!lst_kw.Contains(dt_kw.Rows[i][0].ToString()))
                        {
                            lst_kw.Add(dt_kw.Rows[i][0].ToString());
                            foreach (string w in _words)
                            {
                                if (!text.Contains(w))
                                {
                                    lst_kw.Remove(dt_kw.Rows[i][0].ToString());
                                    break;
                                }
                            }
                        }
                    }

                    ht_evt = evtJudge(dt_event,lst_kw);
                    if (ht_evt.Count>0)
                    {
                        int part = partJudge(dt_partWord,entity.Text);  //正负项
                        //string text = entity.RetweetedStatus == null ? entity.Text : entity.Text + entity.RetweetedStatus.Text;
                        foreach (DictionaryEntry de in ht_evt)
                        {
                            int urlTherear = UrlThereare(entity.ID);
                            if (urlTherear <= 0)
                            {
                                sql = sql.Append("insert into ReleaseInfoWB (title,contexts,releasedate,infosource,"
                                    + "keywords,releasename,collectdate,snapshot,webname,pid,part,reposts,comments,kid) values ('"+entity.ID+"','"
                                    + text + "','"
                                    + string.Format(NetDimension.Weibo.Utility.ParseUTCDate(entity.CreatedAt).ToString("yyyy-MM-dd HH:mm:ss"))
                                    + "','" + "http://weibo.com/" + entity.User.ID + "','" + de.Key.ToString() + "','"
                                    + entity.User.Name + "','" + string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")) + "','','"
                                    + "新浪微博" + "','" + "3" + "','" + part.ToString() + "','" + entity.RepostsCount + "','"
                                    + entity.CommentsCount + "'," + de.Value.ToString() + ");");
                            }
                        }
                    }
                    lst_kw.Clear();
                }
            }
            catch (Exception err)
            {
                //MessageBox.Show(err.Message, "警告：");
                Console.Write(err.Message);
            }
            //DateTime dt2 = System.DateTime.Now;
            //MessageBox.Show("Finished in " + (dt2.Second - dt1.Second).ToString() + " seconds.");
            //插入数据库
            if (sql.Length > 0)
            cmd.ExecuteNonQueryInt(sql.ToString());
        }

        //由通过认证的新浪客户端，调取当前登录用户及其所关注用户的最新微博接口Friends_Timeline,n最大是一次100条
        //获得最新的n条含有用户关键字的公共微博信息并存入数据库
        public void getFriends_Timeline_sina(Client sinaClient, int n)
        {
            StringBuilder sql = new StringBuilder();
            ArrayList lst_kw = new ArrayList();
            Hashtable ht_evt = new Hashtable();
            try
            {
                NetDimension.Weibo.Entities.status.Collection satColl = sinaClient.API.Entity.Statuses.FriendsTimeline("", "", n);
                foreach (NetDimension.Weibo.Entities.status.Entity entity in satColl.Statuses)
                {
                    string text = entity.RetweetedStatus == null ? entity.Text : entity.Text + entity.RetweetedStatus.Text;
                    if (!weibo_ids.Equals(entity.ID) || long.Parse(weibo_ids.Equals("") ? "0" : weibo_ids) > long.Parse(entity.ID))
                    {
                        //把正文里包含的关键词都放入list
                        for (int i = 0; i < dt_kw.Rows.Count; i++)
                        {
                            string[] _words = dt_kw.Rows[i][0].ToString().Split(new char[] { ' ' });
                            if (!lst_kw.Contains(dt_kw.Rows[i][0].ToString()))
                            {
                                lst_kw.Add(dt_kw.Rows[i][0].ToString());
                                foreach (string w in _words)
                                {
                                    if (!text.Contains(w))
                                    {
                                        lst_kw.Remove(dt_kw.Rows[i][0].ToString());
                                        break;
                                    }
                                }
                            }
                        }

                        ht_evt = evtJudge(dt_event, lst_kw);
                        if (ht_evt.Count > 0)
                        {
                            int part = partJudge(dt_partWord, text);
                            weibo_ids = entity.ID;
                            //string text = entity.RetweetedStatus == null ? entity.Text : entity.Text + entity.RetweetedStatus.Text;
                            foreach (DictionaryEntry de in ht_evt)
                            {
                                int urlTherear = UrlThereare(entity.ID);
                                if (urlTherear <= 0)
                                {
                                    sql = sql.Append("insert into ReleaseInfoWB (title,contexts,releasedate,infosource,"
                                        + "keywords,releasename,collectdate,snapshot,webname,pid,part,reposts,comments,kid) values ('"+entity.ID+"','"
                                        + text + "','"
                                        + string.Format(NetDimension.Weibo.Utility.ParseUTCDate(entity.CreatedAt).ToString("yyyy-MM-dd HH:mm:ss"))
                                        + "','" + "http://weibo.com/" + entity.User.ID + "','" + de.Key.ToString() + "','"
                                        + entity.User.Name + "','" + string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")) + "','','"
                                        + "新浪微博" + "','" + "3" + "','" + part.ToString() + "','" + entity.RepostsCount + "','"
                                        + entity.CommentsCount + "'," + de.Value.ToString() + ");");
                                }
                            }
                        }
                        lst_kw.Clear();
                    }
                }
            }
            catch (Exception err)
            {
                Console.Write(err.Message);
            }
            //插入数据库
            if (sql.Length > 0)
            cmd.ExecuteNonQueryInt(sql.ToString());
        }

        public void getSinaClient()
        {
            string code = "";

            if (string.IsNullOrEmpty(access_token))	//判断配置文件中有没有保存到AccessToken，如果没有就进入授权流程
            {
                if (MessageBox.Show("新浪微博未授权或授权已过期，请重新授权！", "注意", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.OK)
                {
                    Forms.SinaWBOauth sinaWbForm = new Forms.SinaWBOauth();
                    sinaWbForm.thread_in = "1";
                    if (sinaWbForm.ShowDialog() == DialogResult.OK)
                    {
                        if (sinaWbForm.retrun_url.Contains("code="))
                        {
                            string[] url = sinaWbForm.retrun_url.Split('=');
                            if (url.Length > 0) { code = url[1]; }
                            oauth = new NetDimension.Weibo.OAuth(app_key, app_secret, callback_url);
                            at = oauth.GetAccessTokenByAuthorizationCode(code);
                            xmlutil.SetValue("AccessToken", at.Token);
                            sina = new Client(new OAuth(app_key, app_secret, at.Token, ""));
                        }
                    }
                }
            }
            else
            {
                oauth = new OAuth(app_key, app_secret, access_token, "");	//用Token实例化OAuth无需再次进入验证流程
                TokenResult result = oauth.VerifierAccessToken();
                if (result == TokenResult.Success)
                {
                    sina = new Client(oauth);
                }
            } 
        }

        public void getSinaClientAuto()
        {
            access_token = xmlutil.GetValue("AccessToken");
            oauth = new OAuth(app_key, app_secret, access_token, "");	//用Token实例化OAuth无需再次进入验证流程
            TokenResult result = oauth.VerifierAccessToken();
            if (result == TokenResult.Success)
            {
                sina = new Client(oauth);
            }
        }

        //判断str1中包含str2的个数 
        public int partCount(string str, string constr)
        {
            return System.Text.RegularExpressions.Regex.Matches(str, constr).Count;
        }

        public int countStr(string str1, string str2)
        {
            int counter = 0;
            if (str1.IndexOf(str2) == -1)
            {
                return 0;
            }
            else if (str1.IndexOf(str2) != -1)
            {
                counter++;
                countStr(str1.Substring(str1.IndexOf(str2) +
                       str2.Length), str2);
                return counter;
            }
            return 0;
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
        private Hashtable evtJudge(DataTable dt,ArrayList al)
        {
            Hashtable retHt = new Hashtable();
            string evt_name,evt_kw,evt_kid = "";
            for (int i = 0; i < dt.Rows.Count; i++ )
            {
                evt_name = dt.Rows[i][0].ToString();
                evt_kw = dt.Rows[i][1].ToString();
                evt_kid = dt.Rows[i][2].ToString();
                if (al.Contains(evt_kw))
                {
                    if (retHt.Contains(evt_name)==false)
                    {
                        retHt.Add(evt_name, evt_kid);   //满足条件的事件加入返回结果
                    }
                } 
            }
            return retHt;
        }

        //从正文包含的关键词判断哪个事件出现,此方法暂时弃用
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
            string sql = "select count(0) from ReleaseInfoWB where title='"+id+"'";
            int result = int.Parse(cmd.GetOne(sql).ToString());
            return result;
        }
    }
}
