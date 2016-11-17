using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataBaseServer;
using System.Data;
using System.Collections;
using System.Media;
using System.Net;
using System.IO;
using System.Web;

namespace Finder.util
{
    class Alert
    {
        MySqlCmd cmd = new MySqlCmd();
        System.Timers.Timer t = null;
        private SoundPlayer sPlayer = new SoundPlayer();
        private DateTime soundPlayTime;
        //DataTable dt_SMS_Content = null;
        string userID = "";  //加密狗的用户id
        Hashtable ht = new Hashtable();

        public Alert(int rate)
        {
            try
            {
                userID = (string)GlobalPars.GloPars["userID"];
            }
            catch (KeyNotFoundException knfe) { userID = "000001"; }
            //设置间隔时间为传入参数rate
            t = new System.Timers.Timer(rate);
            //构造方法里面定义多线程计时器
            t.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            //设置是执行一次（false）还是一直执行(true)   
            t.AutoReset = true;
            //是否执行System.Timers.Timer.Elapsed事件
            t.Enabled = true;
        }

        /// <summary>
        /// 开始一轮短信预警
        /// </summary>
        public void Start()
        {
            t.Enabled = true;
        }
        /// <summary>
        /// 停止一轮短信预警
        /// </summary>
        public void Stop()
        {
            t.Enabled = false;
        }

        public void timer_Elapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            //关键字预警方式表
            string timeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string timeBefore = "";

            int aviCount = 1;
            //int sendAviCountSMS=1;
            string sql = "";
            //string url = "http://www.shangwukaocha.cn/finder/users.php?userID=" + userID + "&m=q";      //取短信余额的网址,用userid先去服务端取可用短信数量aviCount
            ////每次往服务端提交一次短信使用量，服务端用来记录用户短信可用数量。当到达一定阀值，给用户发短信提示短信不足。
            //string[] retVal = new string[3] { "", "","" };
            //try
            //{
            //    string _r = postSend(url, "");
            //    if (_r.Contains(','))
            //    {
            //       retVal = _r.Split(',');
            //    }
            //    aviCount = int.Parse(retVal[0].Equals("") ? "1" : retVal[0]);         //可用量
            //    sendAviCountSMS = int.Parse(retVal[1].Equals("") ? "1" : retVal[1]);  //是否已发送短信账户不足提示
            //}
            //catch (FormatException fe) {}

            //DataTable dt_alert_collection_sms = cmd.GetTabel("select * from warn where WarnWay=1");
            //DataTable dt_alert_collection_snd = cmd.GetTabel("select * from warn where WarnWay=2");

            string sendText = "";
            DateTime hashtableTime;
            int thisTimeCount = 0;               //本次短信使用数量
            string tmp_IntervalHours = "";       //每个关键词的预警周期
            int tmp_IntervalHoursTotalInfo = 0;  //每个关键词的预警数量
            string tmp_keyword = "";             //关键词
            string tmp_sendContent = "";         //关键词报警短信发送的内容
            string tmp_phoneNumber = "";         //发送到手机号
            string tmp_hasSmsAlerted = "0";      //同一个关键词短信是否已报过警,报过为1

            DataTable dt_alert_collection = cmd.GetTabel("select * from warn");

            for (int icount = 0; icount <= dt_alert_collection.Rows.Count - 1; icount++)
            {
                //短信预警开始 
                if (dt_alert_collection.Rows[icount]["WarnWay"].ToString().Equals("1"))
                {
                    if (aviCount > 0)
                    {
                        tmp_IntervalHours = dt_alert_collection.Rows[icount]["IntervalHours"].ToString();
                        tmp_IntervalHoursTotalInfo = int.Parse(dt_alert_collection.Rows[icount]["IntervalHoursTotalInfo"].ToString());
                        timeBefore = DateTime.Now.AddHours(-double.Parse(tmp_IntervalHours)).ToString("yyyy-MM-dd HH:mm:ss");
                        tmp_keyword = dt_alert_collection.Rows[icount]["keyword"].ToString();
                        tmp_sendContent = dt_alert_collection.Rows[icount]["WarnContent"].ToString();
                        tmp_phoneNumber = dt_alert_collection.Rows[icount]["mobile"].ToString();

                        sql = "select * from sms where keyword = '" + tmp_keyword + "' order by sendtime desc";
                        DataTable dtsms = cmd.GetTabel(sql);
                        if (dtsms != null && dtsms.Rows.Count > 0)
                        {
                            string lastTime = dtsms.Rows[0]["sendtime"].ToString();
                            if (string.Compare(timeBefore, lastTime) < 0)
                            {
                                timeBefore = lastTime;
                            }
                        }
                        //2015.3.18 wangcg 修改后，ReleaseInfo数据库的keywords字段存储的是关键字不是事件名称，需要修改sql
                        //sql = "select count(1) cs,keywords from releaseinfo where collectdate between '"
                        //    + timeBefore + "' and '"
                        //    + timeNow + "' and "
                        //    + " keywords = '" + tmp_keyword + "'";
                        //样例sql：
                        //select count(*) cs, b.name from releaseinfo a
                        //left join keywords b on a.keywords=b.[KeyWord]
                        //where b.[Name]='自然灾害'
                        sql = @"select count(1) cs,b.Name keywords from releaseinfo a
                                left join keywords b on a.keywords=b.KeyWord
                                where a.collectdate between '{0}' and '{1}'
                                and  b.Name='{2}'";
                        sql = string.Format(sql, timeBefore, timeNow, tmp_keyword);
                        DataTable dt_SMS = cmd.GetTabel(sql);

                        if (int.Parse(dt_SMS.Rows[0]["cs"].ToString()) >= tmp_IntervalHoursTotalInfo)
                        {
                            tmp_hasSmsAlerted = "1";
                            //短时间内刚发过同样短信就不再发的判断
                            //if (ht.Contains(tmp_keyword))
                            //{
                            //    hashtableTime = (DateTime)ht[tmp_keyword];
                            //    if ((DateTime.Now - hashtableTime).TotalMinutes > double.Parse(tmp_IntervalHours) * 60)
                            //    {
                            sendText = StringReplace(tmp_sendContent, "[事件名称]", insertSign(tmp_keyword));
                            sendSMS(sendText, tmp_phoneNumber, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            thisTimeCount++;
                            //aviCount--;
                            //ht.Remove(tmp_keyword);
                            //ht.Add(tmp_keyword, DateTime.Now);
                            sendText = "";

                            //发完短信在数据库中插入刚才发的短信记录
                            sql = "insert into sms (content,sendtime,mobile,keyword) values ('"
                                        + tmp_sendContent + "','"
                                        + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','"
                                        + tmp_phoneNumber + "','"
                                        + tmp_keyword + "')";
                            cmd.ExecuteNonQueryInt(sql);
                            //    }
                            //}
                            //else
                            //{
                            //    ht.Add(tmp_keyword, DateTime.Now);
                            //    sendText = StringReplace(tmp_sendContent, "[事件名称]", insertSign(tmp_keyword));
                            //    sendSMS(sendText, tmp_phoneNumber, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            //    thisTimeCount++;
                            //    //aviCount--;
                            //    sendText = "";

                            //    //发完短信在数据库中插入刚才发的短信记录
                            //    sql = "insert into sms (content,sendtime,mobile,keyword) values ('"
                            //                + tmp_sendContent + "','"
                            //                + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','"
                            //                + tmp_phoneNumber + "','"
                            //                + tmp_keyword + "')";
                            //    cmd.ExecuteNonQueryInt(sql);
                            //}
                        }
                        else
                        {
                            tmp_hasSmsAlerted = "0";
                        }

                        //if (aviCount <= 100 && sendAviCountSMS == 0)
                        //{
                        //    sendSMS("截止目前，您的舆情系统账户短信剩余量已不足100条，请尽快联系客服续费！",
                        //            tmp_phoneNumber,
                        //            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        //}

                        //"把本次使用量thisTimeCount,userID作为参数请求url"
                        string url = "http://www.shangwukaocha.cn/finder/users.php?userID=" + userID + "&m=usc&smscount=" + thisTimeCount.ToString();
                        //2016.4.17 地址已经失效，去掉短信条数的登记服务
                        //postSend(url, "");



                        //if (int.Parse(retVal[2].Equals("") ? "0" : retVal[2]) == 0 && aviCount <= 0)
                        //{
                        //    sendSMS("截止目前，您的舆情系统账户短信剩余量已为0，短信功能将停止，请尽快联系客服续费！",
                        //            tmp_phoneNumber,
                        //            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        //}
                    }     //短信预警结束
                }
                else
                {
                    //声音预警开始
                    if (dt_alert_collection.Rows[icount]["keyword"].ToString().Equals(tmp_keyword))
                    {
                        //该关键字短信已经报警则声音也报警
                        if (tmp_hasSmsAlerted.Equals("1"))
                        {
                            tmp_sendContent = dt_alert_collection.Rows[icount]["WarnContent"].ToString();
                            soundAlert(tmp_keyword, tmp_sendContent);
                        }
                    }
                    else
                    {
                        tmp_IntervalHours = dt_alert_collection.Rows[icount]["IntervalHours"].ToString();
                        tmp_IntervalHoursTotalInfo = int.Parse(dt_alert_collection.Rows[icount]["IntervalHoursTotalInfo"].ToString());
                        timeBefore = DateTime.Now.AddHours(-double.Parse(tmp_IntervalHours)).ToString("yyyy-MM-dd HH:mm:ss");
                        tmp_keyword = dt_alert_collection.Rows[icount]["keyword"].ToString();
                        tmp_sendContent = dt_alert_collection.Rows[icount]["WarnContent"].ToString();
                        tmp_phoneNumber = dt_alert_collection.Rows[icount]["mobile"].ToString();

                        //sql = "select count(1) cs,keywords from releaseinfo where collectdate between '"
                        //    + timeBefore + "' and '"
                        //    + timeNow + "' and "
                        //    + " keywords = '" + tmp_keyword + "'";

                        sql = "select * from soundAlert where keyword = '" + tmp_keyword + "' order by sendtime desc";
                        DataTable dtsms = cmd.GetTabel(sql);
                        if (dtsms != null && dtsms.Rows.Count > 0)
                        {
                            string lastTime = dtsms.Rows[0]["sendtime"].ToString();
                            if (string.Compare(timeBefore, lastTime) < 0)
                            {
                                timeBefore = lastTime;
                            }
                        }

                        //样例sql：
                        //select count(*) cs, b.name from releaseinfo a
                        //left join keywords b on a.keywords=b.[KeyWord]
                        //where b.[Name]='自然灾害'
                        sql = @"select count(1) cs,b.Name keywords from releaseinfo a
                                left join keywords b on a.keywords=b.KeyWord
                                where a.collectdate between '{0}' and '{1}'
                                and  b.Name='{2}'";
                        sql = string.Format(sql, timeBefore, timeNow, tmp_keyword);

                        DataTable dt_SND = cmd.GetTabel(sql);

                        if (int.Parse(dt_SND.Rows[0]["cs"].ToString()) >= tmp_IntervalHoursTotalInfo)
                        {
                            //此处还应加短时间内刚发过同样内容的声音预警就不再播放声音的判断
                            soundAlert(tmp_keyword, tmp_sendContent);
                        }
                    }
                } //声音预警结束
            }
        }

        //发短信
        public void sendSMS(string content_, string phoneNumber_, string sendTime_)
        {
            //此处先写调用短信接口发短信的方法
            #region
            //返回值	说明
            //    -1	手机号码不正确
            //    -2	除时间外，所有参数不能为空
            //    -3	用户名密码不正确
            //    -4	平台不存在
            //    -5	客户短信数量为0
            //    -6	客户账户余额小于要发送的条数
            //    -7	不能超过70个字
            //    -8	非法短信内容
            //    -9	未知系统故障
            //    -10	网络性错误
            //    1	代表发送成功
            //string url = "http://jiekou.56dxw.com/sms/HttpInterface.aspx?comid=" + "企业ID" + "&"
            //              + "username=" + "用户名" + "&"
            //              + "userpwd=" + "密码" + "&"
            //              + "handtel=" + "手机号" + "&"
            //              + "sendcontent=" + "内容限制为70个字" + "&"
            //              + "sendtime=" + "" + "&"
            //              + "smsnumber=" + "所用平台";
            //string r = getHttpContent(url);
            #endregion

            System.Text.Encoding myEncode = System.Text.Encoding.GetEncoding("UTF-8");
            //以下参数为所需要的参数，测试时请修改
            string strReg = "101100-WEB-HUAX-333046";   //注册号（由华兴软通提供）
            string strPwd = "KGJTPSIK";                 //密码（由华兴软通提供）
            string strSourceAdd = "";                   //子通道号，可为空（预留参数）
            //string strPhone = "13391750223,18701657767";//手机号码，多个手机号用半角逗号分开，最多1000个
            //string strContent = HttpUtility.UrlEncode(content_ + "[" + sendTime_ + "]", myEncode);
            string strContent = HttpUtility.UrlEncode(content_, myEncode);
            //短信内容
            string url = "http://www.stongnet.com/sdkhttp/sendsms.aspx";  //华兴软通发送短信地址
            //要发送的内容
            string strSend = "reg=" + strReg + "&pwd=" + strPwd + "&sourceadd=" + strSourceAdd +
                             "&phone=" + phoneNumber_ + "&content=" + strContent;
            //发送
            string strRes = postSend(url, strSend);
        }

        //播声音
        public void soundAlert(string keywords, string send_content)
        {
            //播完报警声音后在数据库中插入刚才的播放记录
            string sql = "insert into soundAlert (sendtime,keyword) values ('"
                       + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','"
                       + keywords + "')";
            cmd.ExecuteNonQueryInt(sql);
            sPlayer.SoundLocation = @"warn_sound\" + send_content;
            sPlayer.Load();

            sPlayer.Play();

            //DateTime currentTime = DateTime.Now;
            ////当前时间比上次声音报警大于等于两分钟时才报警。
            //lock ((object)soundPlayTime)
            //{
            //    if (soundPlayTime == null)
            //    {
            //        //sPlayer.PlayLooping();
            //        sPlayer.Play();
            //        soundPlayTime = currentTime;
            //    }
            //    else
            //    {
            //        TimeSpan cha = currentTime - soundPlayTime;
            //        if (cha.Minutes >= 20)
            //        {
            //            sPlayer.Play();
            //            soundPlayTime = currentTime;
            //        }
            //    }
            //}
        }

        //把str中的toep替换成strRep
        private string StringReplace(string str, string toRep, string strRep)
        {
            StringBuilder sb = new StringBuilder();
            int np = 0, n_ptmp = 0;
            for (; ; )
            {
                string str_tmp = str.Substring(np);
                n_ptmp = str_tmp.IndexOf(toRep);
                if (n_ptmp == -1)
                {
                    sb.Append(str_tmp);
                    break;
                }
                else
                {
                    sb.Append(str_tmp.Substring(0, n_ptmp)).Append(strRep);
                    np += n_ptmp + toRep.Length;
                }
            }
            return sb.ToString();
        }

        //在输入字符串中插入"*",如：炒股票-->*炒*股*票*
        private string insertSign(string input_str)
        {
            string retStr = input_str;
            int j = 0;
            for (int i = 0; i < input_str.Length; i++)
            {
                retStr = retStr.Insert(i + j, "*");
                j++;
            }
            return retStr + "*";
        }

        //把类似:
        //        关键字数目         关键字
        //            22           城管,拆迁
        //            10              城管
        //梳理为:
        //        关键字数目         关键字        短信预警        声音预警
        //            32              城管            1                1
        //            22              拆迁            0                1
        public DataTable regularDT(DataTable dt, DataTable dt_way)
        {
            DataTable retDt = new DataTable();
            retDt.Columns.Add("0");
            retDt.Columns.Add("1");
            retDt.Columns.Add("2");
            retDt.Columns.Add("3");
            //int j = 0;
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                if (!dt.Rows[i][1].ToString().Contains(","))
                {
                    DataRow dr = retDt.NewRow();
                    dr["0"] = int.Parse(dt.Rows[i][0].ToString());
                    dr["1"] = dt.Rows[i][1].ToString();
                    //retDt.Rows[j][0] = int.Parse(dt.Rows[i][0].ToString());
                    //retDt.Rows[j][1] = dt.Rows[i][1].ToString();
                    //j++;
                    for (int k = 0; k <= dt_way.Rows.Count - 1; k++)
                    {
                        if (dt.Rows[i][1].ToString().Equals(dt_way.Rows[k][0].ToString()))
                        {
                            dr["2"] = dt_way.Rows[k][1].ToString();
                            dr["3"] = dt_way.Rows[k][2].ToString();
                            //retDt.Rows[j][2] = dt_way.Rows[k][1].ToString();
                            //retDt.Rows[j][3] = dt_way.Rows[k][2].ToString();
                        }
                    }
                    retDt.Rows.Add(dr);
                }
            }

            for (int m = 0; m <= dt.Rows.Count - 1; m++)
            {
                if (dt.Rows[m][1].ToString().Contains(","))
                {
                    for (int n = 0; n <= retDt.Rows.Count - 1; n++)
                    {
                        if (dt.Rows[m][1].ToString().Contains(retDt.Rows[n][1].ToString()))
                        {
                            retDt.Rows[n][0] = int.Parse(retDt.Rows[n][0].ToString()) + 1;
                        }
                    }
                }
            }
            return retDt;
        }

        public static string postSend(string url, string param)
        {
            System.Text.Encoding myEncode = System.Text.Encoding.GetEncoding("UTF-8");
            byte[] postBytes = System.Text.Encoding.ASCII.GetBytes(param);

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            req.ContentLength = postBytes.Length;

            try
            {
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(postBytes, 0, postBytes.Length);
                }
                using (WebResponse res = req.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(res.GetResponseStream(), myEncode))
                    {
                        string strResult = sr.ReadToEnd();
                        return strResult;
                    }
                }
            }
            catch (WebException ex)
            {
                //return "无法连接到服务器\r\n错误信息：" + ex.Message;
                return "0";
            }
        }


        private string getHttpContent(string url)
        {
            try
            {
                string retValue = "";
                WebRequest request = WebRequest.Create(url);
                //request.Timeout = 10000;
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream, Encoding.Default);
                retValue = reader.ReadToEnd();
                stream.Close();
                response.Close();
                return retValue;
            }
            catch (Exception e) { return ""; }
        }

        //public Hashtable getParams()
        //{
        //    Hashtable ht = new Hashtable();
        //    DataTable dt = cmd.GetTabel("select * from warn");
        //    if (dt.Rows.Count > 0)
        //    {
        //        for (int i = 0; i <= dt.Rows.Count - 1; i++)
        //        {
        //            Entities.Warn warn = new Entities.Warn();
        //            warn.IntervalHours = dt.Rows[i]["IntervalHours"].ToString();
        //            warn.IntervalHoursTotalInfo = dt.Rows[i]["IntervalHoursTotalInfo"].ToString();
        //            warn.Keyword = dt.Rows[i]["Keyword"].ToString();
        //            warn.Mobile = dt.Rows[i]["WarnWay"].ToString();
        //            warn.WarnContent = dt.Rows[i]["WarnContent"].ToString();
        //            warn.WarnWay = dt.Rows[i]["Mobile"].ToString();
        //            ht.Add("warn" + i.ToString(), warn);
        //        }
        //    }
        //    return ht;
        //}
    }
}
