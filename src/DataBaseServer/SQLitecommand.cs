using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
//using System.Data.SqlClient;
using System.Data.SQLite;

namespace DataBaseServer
{
    public class SQLitecommand
    {
        #region 变量
        private int commandTimeout = 30;
        protected string connString;
        #endregion

        #region 构造
        public SQLitecommand() {
            //this.connString = System.Configuration.ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            //this.connString = connString.Replace("@@", System.Web.HttpContext.Current.Server.MapPath("~/"));
            //this.connString = connString.Replace("@@", System.Web.HttpContext.Current.Server.MapPath("~/Date/"));
            this.connString = "Data Source=" + AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "WebAnalysis.db;version=3";
        }
        public SQLitecommand(string strConn)
        {
            this.connString = strConn;
        }
        #endregion

        #region 属性
        /// <summary>
        /// 设置连接超时（秒）
        /// </summary>
        public int CommandTimeout
        {
            get { return commandTimeout; }
            set { commandTimeout = value; }
        }

        /// <summary>
        /// 数据库连接对像
        /// </summary>
        public virtual SQLiteConnection ConnObj
        {
            get 
            {
                SQLiteconnection c = new SQLiteconnection(this.connString);
                return c.ConnObj;
            }
        }

        public string ConnString
        {
            get { return connString; }
            set { connString = value; }
        }
        #endregion

        #region command对像
        /// <summary>
        /// SQL操作对像
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="conn">数据库连接</param>
        /// <returns></returns>
        public SQLiteCommand CommObj(string strSql, SQLiteConnection conn)
        {
            SQLiteCommand comm = new SQLiteCommand();
            comm.CommandText = strSql;
            comm.Connection = conn;
            comm.CommandTimeout = this.CommandTimeout;
            return comm;
        }
        /// <summary>
        /// SQL操作对像
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <param name="conn">数据库连接</param>
        /// <returns></returns>
        public SQLiteCommand CommObj(string strSql, List<SQLiteParameter> parameters, SQLiteConnection conn)
        {
            SQLiteCommand comm = this.CommObj(strSql, conn);
            foreach (SQLiteParameter parameter in parameters)
            {
                comm.Parameters.Add(parameter);
            }
            return comm;
        }
        #endregion

        #region 执行SQL语句
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="conn">数据库连接</param>
        /// <returns></returns>
        public bool ExecuteNonQuery(string strSql, SQLiteConnection conn)
        {
            using (SQLiteCommand comm = this.CommObj(strSql, conn))
            {
                return comm.ExecuteNonQuery() > 0 ? true : false;
            }
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns></returns>
        public bool ExecuteNonQuery(string strSql)
        {
            using (SQLiteConnection conn = this.ConnObj)
            {
                try
                {
                    conn.Open();
                    return this.ExecuteNonQuery(strSql, conn);
                }
                catch (Exception err)
                {
                    throw err;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <param name="conn">数据库连接</param>
        /// <returns></returns>
        public bool ExecuteNonQuery(string strSql, List<SQLiteParameter> parameters, SQLiteConnection conn)
        {
            using (SQLiteCommand comm = this.CommObj(strSql, parameters, conn))
            {
                return comm.ExecuteNonQuery() > 0 ? true : false;
            }
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns></returns>
        public bool ExecuteNonQuery(string strSql, List<SQLiteParameter> parameters)
        {
            using (SQLiteConnection conn = this.ConnObj)
            {
                try
                {
                    conn.Open();
                    return this.ExecuteNonQuery(strSql, parameters, conn);
                }
                catch (Exception err)
                {
                    throw err;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }
        #endregion

        #region 执行SQL语句返回Int
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="conn">数据库连接</param>
        /// <returns></returns>
        public int ExecuteNonQueryInt(string strSql, SQLiteConnection conn)
        {
            using (SQLiteCommand comm = this.CommObj(strSql, conn))
            {
                return comm.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns></returns>
        public int ExecuteNonQueryInt(string strSql)
        {
            using (SQLiteConnection conn = this.ConnObj)
            {
                try
                {
                    conn.Open();
                    return this.ExecuteNonQueryInt(strSql, conn);
                }
                catch (Exception err)
                {
                    throw err;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <param name="conn">数据库连接</param>
        /// <returns></returns>
        public int ExecuteNonQueryInt(string strSql, List<SQLiteParameter> parameters, SQLiteConnection conn)
        {
            using (SQLiteCommand comm = this.CommObj(strSql, parameters, conn))
            {
                return comm.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns></returns>
        public int ExecuteNonQueryInt(string strSql, List<SQLiteParameter> parameters)
        {
            using (SQLiteConnection conn = this.ConnObj)
            {
                try
                {
                    conn.Open();
                    return this.ExecuteNonQueryInt(strSql, parameters, conn);
                }
                catch (Exception err)
                {
                    throw err;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }
        #endregion

        #region 查询
        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <param name="strSelect">查询语句</param>
        /// <param name="conn">数据库连接</param>
        /// <returns></returns>
        public DataTable GetTabel(string strSelect, SQLiteConnection conn)
        {
            using (SQLiteDataAdapter sda = new SQLiteDataAdapter(this.CommObj(strSelect, conn)))
            {
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
        }
        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <param name="strSelect">查询语句</param>
        /// <returns></returns>
        public DataTable GetTabel(string strSelect)
        {
            using (SQLiteConnection conn = this.ConnObj)
            {
                try
                {
                    conn.Open();
                    return GetTabel(strSelect, conn);
                }
                catch (Exception err)
                {
                    throw err;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }
        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <param name="strSelect">查询语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <param name="conn">数据库连接</param>
        /// <returns></returns>
        public DataTable GetTabel(string strSelect, List<SQLiteParameter> parameters, SQLiteConnection conn)
        {
            using (SQLiteDataAdapter sda = new SQLiteDataAdapter(this.CommObj(strSelect, parameters, conn)))
            {
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
        }
        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <param name="strSelect">查询语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns></returns>
        public DataTable GetTabel(string strSelect, List<SQLiteParameter> parameters)
        {
            using (SQLiteConnection conn = this.ConnObj)
            {
                try
                {
                    conn.Open();
                    return GetTabel(strSelect, parameters, conn);
                }
                catch (Exception err)
                {
                    throw err;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }
        /// <summary>
        /// 获取第一行第一列
        /// </summary>
        /// <param name="strSelect">查询语句</param>
        /// <param name="conn">数据库连接</param>
        /// <returns></returns>
        public object GetOne(string strSelect, SQLiteConnection conn)
        {
            using (SQLiteCommand comm = this.CommObj(strSelect, conn))
            {
                return comm.ExecuteScalar();
            }
        }
        /// <summary>
        /// 获取第一行第一列
        /// </summary>
        /// <param name="strSelect">查询语句</param>
        /// <returns></returns>
        public object GetOne(string strSelect)
        {
            using (SQLiteConnection conn = this.ConnObj)
            {
                try
                {
                    conn.Open();
                    return GetOne(strSelect, conn);
                }
                catch (Exception err)
                {
                    throw err;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }
        /// <summary>
        /// 获取第一行第一列
        /// </summary>
        /// <param name="strSelect">查询语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <param name="conn">数据库连接</param>
        /// <returns></returns>
        public object GetOne(string strSelect, List<SQLiteParameter> parameters, SQLiteConnection conn)
        {
            using (SQLiteCommand comm = this.CommObj(strSelect, parameters, conn))
            {
                return comm.ExecuteScalar();
            }
        }
        /// <summary>
        /// 获取第一行第一列
        /// </summary>
        /// <param name="strSelect">查询语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <param name="conn">数据库连接</param>
        /// <returns></returns>
        public object GetOne(string strSelect, List<SQLiteParameter> parameters)
        {
            using (SQLiteConnection conn = this.ConnObj)
            {
                try
                {
                    conn.Open();
                    return GetOne(strSelect, parameters, conn);
                }
                catch (Exception err)
                {
                    throw err;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }
        #endregion

        #region 事务
        /// <summary>
        /// 事务
        /// </summary>
        /// <param name="strSQL">SQL语句组</param>
        /// <returns></returns>
        public bool Transaction(List<string> strSQL)
        {
            using (SQLiteConnection conn = this.ConnObj)
            {
                try
                {
                    conn.Open();
                    return Transaction(strSQL, conn);
                }
                catch (Exception err)
                {
                    throw err;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }
        /// <summary>
        /// 事务
        /// </summary>
        /// <param name="strSQL">SQL语句组</param>
        /// <param name="conn">已打开的SQL连接</param>
        /// <returns></returns>
        public bool Transaction(List<string> strSQL, SQLiteConnection conn)
        {
            
            SQLiteTransaction tran = conn.BeginTransaction();
            SQLiteCommand comm = conn.CreateCommand();
            comm.Transaction = tran;
            comm.CommandTimeout = this.CommandTimeout;
            try
            {
                int count = 0;
                foreach (string sql in strSQL)
                {
                    comm.CommandText = sql;
                    count += comm.ExecuteNonQuery();
                }
                tran.Commit();
                return count > 0 ? true : false;
            }
            catch (Exception err)
            {
                tran.Rollback();
                throw err;
            }
        }
        /// <summary>
        /// 事务
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="parameterCollection">SQL参数</param>
        /// <returns></returns>
        public bool Transaction(string strSQL, List<List<SQLiteParameter>> parameterCollection)
        {
            using (SQLiteConnection conn = this.ConnObj)
            {
                try
                {
                    conn.Open();
                    return Transaction(strSQL, parameterCollection, conn);
                }
                catch (Exception err)
                {
                    throw err;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }
        /// <summary>
        /// 事务
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="parameterCollection">SQL参数</param>
        /// <param name="conn">SQL连接</param>
        /// <returns></returns>
        public bool Transaction(string strSQL, List<List<SQLiteParameter>> parameterCollection, SQLiteConnection conn)
        {
            SQLiteTransaction tran = conn.BeginTransaction();
            int count = 0;
            try
            {
                foreach (List<SQLiteParameter> paramters in parameterCollection)
                {
                    using (SQLiteCommand comm = conn.CreateCommand())
                    {
                        comm.CommandText = strSQL;
                        comm.CommandTimeout = CommandTimeout;
                        comm.Transaction = tran;
                        foreach (SQLiteParameter parameter in paramters)
                        {
                            comm.Parameters.Add(parameter);
                        }
                        count += comm.ExecuteNonQuery();
                    }
                }
                tran.Commit();
                return count > 0 ? true : false;
            }
            catch (Exception err)
            {
                tran.Rollback();
                throw err;
            }
        }
        /// <summary>
        /// 事务
        /// </summary>
        /// <param name="transt">事务SQL语句和参数对应建构体</param>
        /// <returns></returns>
        public bool Transaction(List<SQLiteTransCollection> transt)
        {
            using (SQLiteConnection conn = this.ConnObj)
            {
                try
                {
                    conn.Open();
                    return Transaction(transt, conn);
                }
                catch (Exception err)
                {
                    throw err;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }
        /// <summary>
        /// 事务
        /// </summary>
        /// <param name="transt">事务SQL语句和参数对应建构体</param>
        /// <param name="conn">SQL连接</param>
        /// <returns></returns>
        public bool Transaction(List<SQLiteTransCollection> transt, SQLiteConnection conn)
        {
            SQLiteTransaction tran = conn.BeginTransaction();
            int count = 0;
            try
            {
                foreach (SQLiteTransCollection tc in transt)
                {
                    using (SQLiteCommand comm = conn.CreateCommand())
                    {
                        comm.CommandText = tc.StrSQL;
                        comm.CommandTimeout = this.CommandTimeout;
                        comm.Transaction = tran;
                        foreach (SQLiteParameter parameter in tc.Parameters)
                        {
                            comm.Parameters.Add(parameter);
                        }
                        count += comm.ExecuteNonQuery();
                    }
                }
                tran.Commit();
                return count > 0 ? true : false;
            }
            catch (Exception err)
            {
                tran.Rollback();
                throw err;
            }
        }
        #endregion

        #region 返回Reader对像
        public SQLiteDataReader GetReader(string strSelect, SQLiteConnection conn)
        {
            return this.CommObj(strSelect, conn).ExecuteReader(CommandBehavior.CloseConnection);
        }
        public SQLiteDataReader GetReader(string strSelect, List<SQLiteParameter> parameters, SQLiteConnection conn)
        {
            return this.CommObj(strSelect, parameters, conn).ExecuteReader(CommandBehavior.CloseConnection);
        }
        #endregion

        public void ExecuteNonQueryInt(string sql, System.Data.Common.DbParameter[] prams) {
            throw new NotImplementedException();
        }
    }
}