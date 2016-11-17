using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataBaseServer;
using HtmlParse;
using MySql.Data.MySqlClient;
using Finder.util;

namespace Finder
{

    public class TbReleaseInfo
    {
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="obj">用户对象</param>
        /// <returns></returns>
        public int InsReleaseInfo(ModelReleaseInfo obj)
        {
            string sql = @"INSERT INTO ReleaseInfo(Title,Contexts,ReleaseDate,InfoSource,KeyWords,ReleaseName,CollectDate,Snapshot) 
                            VALUES(@Title,@Contexts,@RleaseDate,@InfoSource,@KeyWords,@ReleaseName,@CollectDate,@Snapshot) ";

            List<MySqlParameter> par = new List<MySqlParameter>();
            par.Add(new MySqlParameter("@Title", obj.Title));
            par.Add(new MySqlParameter("@Contexts", obj.Contexts));
            par.Add(new MySqlParameter("@RleaseDate", obj.ReleaseDate));
            par.Add(new MySqlParameter("@InfoSource", obj.InfoSource));
            par.Add(new MySqlParameter("@KeyWords", obj.KeyWords));
            par.Add(new MySqlParameter("@ReleaseName", obj.ReleaseName));
            par.Add(new MySqlParameter("@CollectDate", obj.CollectDate));
            par.Add(new MySqlParameter("@Snapshot", obj.Snapshot));


            try
            {
                MySqlCmd dbobj = new MySqlCmd();
                return dbobj.ExecuteNonQueryInt(sql, par);

            }
            catch (Exception ex)
            {
                throw new Exception("新建失败,位置:InsReleaseInfo.原因:" + ex.Message);
            }
        }

        public String GetInsString(ModelReleaseInfo obj)
        {
            StringBuilder insertSql = new StringBuilder();
            string[] keywords = obj.KeyWords.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string k in keywords)
            {
                string sql = @"INSERT INTO ReleaseInfo(Title,Contexts,ReleaseDate,InfoSource,KeyWords,ReleaseName,CollectDate,Snapshot,webName,pid,part,reposts,comments,kid,sheng,shi,xian) 
                            VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}'); ";

                obj.Title = filtRiskChar(obj.Title);
                obj.Contexts = filtRiskChar(obj.Contexts);
                insertSql.Append(string.Format(sql, obj.Title, obj.Contexts, obj.ReleaseDate, obj.InfoSource, k.Contains('-') ? k.Split(new char[] { '-' })[0] : k,
                    obj.ReleaseName, obj.CollectDate, obj.Snapshot, obj.WebName, obj.Pid, obj.Part, obj.Reposts, obj.Comments, k.Contains('-') ? k.Split(new char[] { '-' })[1] : k, obj.Sheng, obj.Shi, obj.Xian));
            }
            return insertSql.ToString();
        }

        public String GetInsertStr(ModelReleaseInfo mri)
        {
            string sql = @"INSERT INTO ReleaseInfo(Title,Contexts,ReleaseDate,InfoSource,KeyWords,ReleaseName,CollectDate,Snapshot,webName,pid,part,reposts,comments,kid,sheng,shi,xian) 
                            VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}'); ";

            mri.Title = filtRiskChar(mri.Title);
            mri.Contexts = filtRiskChar(mri.Contexts);
            //统一处理一下发布时间
            string date = mri.ReleaseDate;
            string[] formats = {"yyyy-MM-dd HH:mm:ss","yyyy-M-dd HH:mm:ss","yyyy-M-d HH:mm:ss","yyyy-MM-d HH:mm:ss",
                    "yyyy-MM-dd HH:mm","yyyy-MM-dd hh:mm","yyyy-MM-dd H:mm","yyyy-MM-dd h:mm","yyyy-MM-dd HH:m","yyyy-MM-dd hh:m","yyyy-MM-dd h:m",
                    "yyyy-MM-dd hh:mm:ss","yyyy-MM-dd hh:mm:s","yyyy-MM-dd hh:m:s","yyyy-MM-dd hh:m:ss","yyyy-MM-dd h:mm:ss","yyyy-MM-dd h:mm:s","yyyy-MM-dd h:m:s","yyyy-MM-dd h:m:ss",
                    "yyyy-MM-dd HH:mm:s","yyyy-MM-dd HH:m:s","yyyy-MM-dd HH:m:ss","yyyy-MM-dd H:mm:ss","yyyy-MM-dd H:mm:s","yyyy-MM-dd H:m:s","yyyy-MM-dd H:m:ss",
                    "yyyy-M-dd HH:mm","yyyy-M-dd hh:mm","yyyy-M-dd H:mm","yyyy-M-dd h:mm","yyyy-M-dd HH:m","yyyy-M-dd hh:m","yyyy-M-dd h:m",
                    "yyyy-M-dd hh:mm:ss","yyyy-M-dd hh:mm:s","yyyy-M-dd hh:m:s","yyyy-M-dd hh:m:ss","yyyy-M-dd h:mm:ss","yyyy-M-dd h:mm:s","yyyy-M-dd h:m:s","yyyy-M-dd h:m:ss",
                    "yyyy-M-dd HH:mm:s","yyyy-M-dd HH:m:s","yyyy-M-dd HH:m:ss","yyyy-M-dd H:mm:ss","yyyy-M-dd H:mm:s","yyyy-M-dd H:m:s","yyyy-M-dd H:m:ss",
                    "yyyy-M-d HH:mm","yyyy-M-d hh:mm","yyyy-M-d H:mm","yyyy-M-d h:mm","yyyy-M-d HH:m","yyyy-M-d hh:m","yyyy-M-d h:m",
                    "yyyy-M-d hh:mm:ss","yyyy-M-d hh:mm:s","yyyy-M-d hh:m:s","yyyy-M-d hh:m:ss","yyyy-M-d h:mm:ss","yyyy-M-d h:mm:s","yyyy-M-d h:m:s","yyyy-M-d h:m:ss",
                    "yyyy-M-d HH:mm:s","yyyy-M-d HH:m:s","yyyy-M-d HH:m:ss","yyyy-M-d H:mm:ss","yyyy-M-d H:mm:s","yyyy-M-d H:m:s","yyyy-M-d H:m:ss",
                    "yyyy-MM-d HH:mm","yyyy-MM-d hh:mm","yyyy-MM-d H:mm","yyyy-MM-d h:mm","yyyy-MM-d HH:m","yyyy-MM-d hh:m","yyyy-MM-d h:m",
                    "yyyy-MM-d hh:mm:ss","yyyy-MM-d hh:mm:s","yyyy-MM-d hh:m:s","yyyy-MM-d hh:m:ss","yyyy-MM-d h:mm:ss","yyyy-MM-d h:mm:s","yyyy-MM-d h:m:s","yyyy-MM-d h:m:ss",
                    "yyyy-MM-d HH:mm:s","yyyy-MM-d HH:m:s","yyyy-MM-d HH:m:ss","yyyy-MM-d H:mm:ss","yyyy-MM-d H:mm:s","yyyy-MM-d H:m:s","yyyy-MM-d H:m:ss",
                    "yyyy-MM-dd","yyyy-M-dd","yyyy-M-d","yyyy-MM-d"};

            DateTime dateValue;
            if (DateTime.TryParseExact(date, formats,
                System.Globalization.DateTimeFormatInfo.CurrentInfo, System.Globalization.DateTimeStyles.None, out dateValue))
            {
                date = dateValue.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                date = mri.CollectDate;
            }
            sql = string.Format(sql, mri.Title, mri.Contexts, date, mri.InfoSource, mri.KeyWords,
                mri.ReleaseName, mri.CollectDate, mri.Snapshot, mri.WebName, mri.Pid, mri.Part, mri.Reposts, mri.Comments, mri.Kid, mri.Sheng, mri.Shi, mri.Xian);
            return sql;
        }

        public string filtRiskChar(string str) //过滤非法字符
        {
            string s = "";
            if (string.IsNullOrEmpty(str)) return str;

            s = str.Replace("'", " ");
            s = s.Replace(";", " ");
            s = s.Replace("1=1", " ");
            s = s.Replace("|", " ");
            s = s.Replace("<", " ");
            s = s.Replace(">", " ");

            return s;
        }


        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="obj">用户对象</param>
        /// <returns></returns>
        public int FixReleaseInfo(ModelReleaseInfo obj)
        {
            string sql = @"UPDATE SET ReleaseInfo Title=@Title,Contexts=@Contexts,ReleaseDate=@ReleaseDate,
                                InfoSource=@InfoSource,KeyWords=@KeyWords,ReleaseName=@ReleaseName,
                                CollectDate=@CollectDate,Snapshot=@Snapshot  
                        WHERE uid=@uid";

            List<MySqlParameter> par = new List<MySqlParameter>();
            par.Add(new MySqlParameter("@uid", obj.Uid));
            par.Add(new MySqlParameter("@Title", obj.Title));
            par.Add(new MySqlParameter("@Contexts", obj.Contexts));
            par.Add(new MySqlParameter("@RleaseDate", obj.ReleaseDate));
            par.Add(new MySqlParameter("@InfoSource", obj.InfoSource));
            par.Add(new MySqlParameter("@KeyWords", obj.KeyWords));
            par.Add(new MySqlParameter("@ReleaseName", obj.ReleaseName));
            par.Add(new MySqlParameter("@CollectDate", obj.CollectDate));
            par.Add(new MySqlParameter("@Snapshot", obj.Snapshot));

            try
            {
                DataBaseServer.MySqlCmd dbobj = new DataBaseServer.MySqlCmd();
                return dbobj.ExecuteNonQueryInt(sql, par);
            }
            catch (Exception ex)
            {
                throw new Exception("新建失败,位置:FixReleaseInfo.原因:" + ex.Message);
            }
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <returns>返回值,1为成功</returns>
        public int DelReleaseInfo(int uid)
        {
            string sql = @"DELETE FROM ReleaseInfo WHERE uid=@uid";
            List<MySqlParameter> par = new List<MySqlParameter>();
            par.Add(new MySqlParameter("@uid", uid));
            try
            {
                DataBaseServer.MySqlCmd dbobj = new MySqlCmd();
                return dbobj.ExecuteNonQueryInt(sql, par);
            }
            catch (Exception ex)
            {
                throw new Exception("删除失败,位置:DelReleaseInfo.原因:" + ex.Message);
            }
        }

        /// <summary>
        /// 查询全部的信息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public DataTable SelReleaseInfo()
        {
            DataPage firstPage = Finder.util.Comm.GetPageInfo();
            string sql = "SELECT * FROM ReleaseInfo where uid between " + firstPage.CurrenPageStartUid + " and " + firstPage.CurrenPageEndUid;
            try
            {
                DataBaseServer.MySqlCmd dbobj = new MySqlCmd();
                return dbobj.GetTabel(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("查询失败,位置:SelReleaseInfo.原因:" + ex.Message);
            }
        }

        /// <summary>
        /// 查询信息(按ID)
        /// </summary>
        /// <returns></returns>
        public DataTable SelReleaseInfo(int uid)
        {
            string sql = "SELECT * FROM ReleaseInfo WHERE uid={0}";
            sql = string.Format(sql, uid);

            try
            {
                DataBaseServer.MySqlCmd dbobj = new MySqlCmd();
                return dbobj.GetTabel(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("查询失败,位置:SelReleaseInfo.原因:" + ex.Message);
            }
        }

        /// <summary>
        /// 查询采集信息(按网址)
        /// </summary>
        /// <returns></returns>
        public DataTable SelReleaseInfo(string sUrl)
        {
            DataPage firstPage = Finder.util.Comm.GetPageInfo();
            string sql = "SELECT * FROM ReleaseInfo WHERE InfoSource ='{0}' and uid between " + firstPage.CurrenPageStartUid + " and " + firstPage.CurrenPageEndUid;
            sql = string.Format(sql, sUrl);

            try
            {
                DataBaseServer.MySqlCmd dbobj = new MySqlCmd();
                return dbobj.GetTabel(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("查询失败,位置:SelReleaseInfo.原因:" + ex.Message);
            }
        }

        /// <summary>
        /// 查询采集信息(按网址和关键字)
        /// </summary>
        /// <returns></returns>
        public int GetReleaseInfoCount(string sUrl, string keyword)
        {
            DataPage firstPage = Finder.util.Comm.GetPageInfo();
            string sql = "SELECT count(*) FROM ReleaseInfo WHERE InfoSource ='{0}' and keywords='{1}'  and uid between " + firstPage.CurrenPageStartUid + " and " + firstPage.CurrenPageEndUid ;
            sql = string.Format(sql, sUrl, keyword);

            try
            {
                DataBaseServer.MySqlCmd dbobj = new MySqlCmd();
                object obj = dbobj.GetOne(sql);
                int val = 0;
                int.TryParse(obj.ToString(), out val);

                return val;
            }
            catch (Exception ex)
            {
                //throw new Exception("查询失败,位置:SelReleaseInfo.原因:" + ex.Message);
            }
            return 0;
        }

        /// <summary>
        /// 查询采集信息(按网址)
        /// </summary>
        /// <returns></returns>
        public DataTable GetReleaseInfoFormat()
        {
            string sql = "SELECT uid,Title,Contexts,ReleaseDate,InfoSource,KeyWords,ReleaseName,CollectDate,Snapshot,webName,pid,part,comments,reposts FROM ReleaseInfo WHERE 1=2";

            try
            {
                DataBaseServer.MySqlCmd dbobj = new MySqlCmd();

                return dbobj.GetTabel(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("查询失败,位置:GetReleaseInfoFormat.原因:" + ex.Message);
            }
        }

        /// <summary>
        /// 查询采集信息
        /// </summary>
        /// <param name="t1">时间起</param>
        /// <param name="t2">时间至</param>
        /// <param name="infoType">数据类型</param>
        /// <returns></returns>
        public DataTable SelReleaseInfo(string t1, string t2, string pid)
        {
            DataPage firstPage = Finder.util.Comm.GetPageInfo();
            string sql = "SELECT * FROM ReleaseInfo WHERE CollectDate BETWEEN '{1}' AND '{0}' AND pid={2}  and uid between " + firstPage.CurrenPageStartUid + " and " + firstPage.CurrenPageEndUid  + " ORDER BY CollectDate";
            sql = string.Format(sql, t1, t2, pid);
            try
            {
                DataBaseServer.MySqlCmd dbobj = new MySqlCmd();

                return dbobj.GetTabel(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("查询失败,位置:SelReleaseInfo.原因:" + ex.Message);
            }
        }

        public DataTable GetLatestData(int pid)
        {
            DataPage firstPage = Finder.util.Comm.GetPageInfo();

            //string sql = "Select * From ReleaseInfo where pid={0} and deleted=0 order by collectdate desc limit 0,100";
            string sql = @"select b.Name eventname, a.* from releaseinfo a  left join keywords b on a.keywords=b.KeyWord 
                                    where b.Name is not null and  a.pid={0} and a.deleted=0 and a.uid between " + firstPage.CurrenPageStartUid + " and " + firstPage.CurrenPageEndUid 
                                    + " order by a.collectdate desc limit 0,50";
            sql = string.Format(sql, pid);
            try
            {
                DataBaseServer.MySqlCmd dbobj = new MySqlCmd();

                return dbobj.GetTabel(sql);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetLatestData(int kid, string eventName)
        {
            DataPage firstPage = Finder.util.Comm.GetPageInfo();

            //string sql = "Select * From ReleaseInfo where pid={0} and deleted=0 order by collectdate desc limit 0,100";
            string sql = @"select b.Name eventname, a.* from releaseinfo a  left join keywords b on a.keywords=b.KeyWord 
                                    where b.Name is not null and  a.deleted=0 and a.uid between " + firstPage.CurrenPageStartUid + " and " + firstPage.CurrenPageEndUid;
            if (kid != -1)
            {
                sql += "    and a.kid=" + kid;
                if (!string.IsNullOrEmpty(eventName) && eventName != "全部")
                {
                    sql += " and  b.Name='" + eventName + "'";
                }
            }
            sql += " order by a.collectdate desc limit 0,50";
            try
            {
                DataBaseServer.MySqlCmd dbobj = new MySqlCmd();

                return dbobj.GetTabel(sql);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int GetMaxUid()
        {
            string sql = "SELECT max(uid) FROM ReleaseInfo";
            try
            {
                DataBaseServer.MySqlCmd dbobj = new MySqlCmd();
                DataTable dt = new DataTable();
                dt = dbobj.GetTabel(sql);
                try
                {
                    return int.Parse(dt.Rows[0][0].ToString());
                }
                catch (Exception)
                {

                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("查询失败,位置:GetMaxUid.原因:" + ex.Message);
            }
        }


    }
}
