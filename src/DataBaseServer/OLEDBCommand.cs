using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
//using System.Data.SqlClient;
using System.Data.OleDb;
namespace DataBaseServer
{
    /// <summary>
    /// OLEDB执行
    /// </summary>
    public class OLEDBCommand
    {
        #region 变量
        private int commandTimeout = 30;
        protected string connString;
        #endregion

        #region 构造
        public OLEDBCommand() {
            //this.connString = System.Configuration.ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            //this.connString = connString.Replace("@@", System.Web.HttpContext.Current.Server.MapPath("~/"));
            //this.connString = connString.Replace("@@", System.Web.HttpContext.Current.Server.MapPath("~/Date/"));
            this.connString = "Data Source=D:\\Project\\C#\\WindowsFormsApplication1\\WindowsFormsApplication1\\bin\\Debug\\WebAnalysis.db;version=3";
        }
        public OLEDBCommand(string strConn)
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
        public virtual OleDbConnection ConnObj
        {
            get 
            {
                OLEDBConnection c = new OLEDBConnection(this.connString);
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
        public OleDbCommand CommObj(string strSql, OleDbConnection conn)
        {
            OleDbCommand comm = new OleDbCommand();
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
        public OleDbCommand CommObj(string strSql, List<OleDbParameter> parameters, OleDbConnection conn)
        {
            OleDbCommand comm = this.CommObj(strSql, conn);
            foreach (OleDbParameter parameter in parameters)
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
        public bool ExecuteNonQuery(string strSql, OleDbConnection conn)
        {
            using (OleDbCommand comm = this.CommObj(strSql, conn))
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
            using (OleDbConnection conn = this.ConnObj)
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
        public bool ExecuteNonQuery(string strSql, List<OleDbParameter> parameters, OleDbConnection conn)
        {
            using (OleDbCommand comm = this.CommObj(strSql, parameters, conn))
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
        public bool ExecuteNonQuery(string strSql, List<OleDbParameter> parameters)
        {
            using (OleDbConnection conn = this.ConnObj)
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
        public int ExecuteNonQueryInt(string strSql, OleDbConnection conn)
        {
            using (OleDbCommand comm = this.CommObj(strSql, conn))
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
            using (OleDbConnection conn = this.ConnObj)
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
        public int ExecuteNonQueryInt(string strSql, List<OleDbParameter> parameters, OleDbConnection conn)
        {
            using (OleDbCommand comm = this.CommObj(strSql, parameters, conn))
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
        public int ExecuteNonQueryInt(string strSql, List<OleDbParameter> parameters)
        {
            using (OleDbConnection conn = this.ConnObj)
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
        public DataTable GetTabel(string strSelect, OleDbConnection conn)
        {
            using (OleDbDataAdapter sda = new OleDbDataAdapter(this.CommObj(strSelect, conn)))
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
            using (OleDbConnection conn = this.ConnObj)
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
        public DataTable GetTabel(string strSelect, List<OleDbParameter> parameters, OleDbConnection conn)
        {
            using (OleDbDataAdapter sda = new OleDbDataAdapter(this.CommObj(strSelect, parameters, conn)))
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
        public DataTable GetTabel(string strSelect, List<OleDbParameter> parameters)
        {
            using (OleDbConnection conn = this.ConnObj)
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
        public object GetOne(string strSelect, OleDbConnection conn)
        {
            using (OleDbCommand comm = this.CommObj(strSelect, conn))
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
            using (OleDbConnection conn = this.ConnObj)
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
        public object GetOne(string strSelect, List<OleDbParameter> parameters, OleDbConnection conn)
        {
            using (OleDbCommand comm = this.CommObj(strSelect, parameters, conn))
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
        public object GetOne(string strSelect, List<OleDbParameter> parameters)
        {
            using (OleDbConnection conn = this.ConnObj)
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
            using (OleDbConnection conn = this.ConnObj)
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
        public bool Transaction(List<string> strSQL, OleDbConnection conn)
        {
            
            OleDbTransaction tran = conn.BeginTransaction();
            OleDbCommand comm = conn.CreateCommand();
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
        public bool Transaction(string strSQL, List<List<OleDbParameter>> parameterCollection)
        {
            using (OleDbConnection conn = this.ConnObj)
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
        public bool Transaction(string strSQL, List<List<OleDbParameter>> parameterCollection, OleDbConnection conn)
        {
            OleDbTransaction tran = conn.BeginTransaction();
            int count = 0;
            try
            {
                foreach (List<OleDbParameter> paramters in parameterCollection)
                {
                    using (OleDbCommand comm = conn.CreateCommand())
                    {
                        comm.CommandText = strSQL;
                        comm.CommandTimeout = CommandTimeout;
                        comm.Transaction = tran;
                        foreach (OleDbParameter parameter in paramters)
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
        public bool Transaction(List<OLEDBTransCollection> transt)
        {
            using (OleDbConnection conn = this.ConnObj)
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
        public bool Transaction(List<OLEDBTransCollection> transt, OleDbConnection conn)
        {
            OleDbTransaction tran = conn.BeginTransaction();
            int count = 0;
            try
            {
                foreach (OLEDBTransCollection tc in transt)
                {
                    using (OleDbCommand comm = conn.CreateCommand())
                    {
                        comm.CommandText = tc.StrSQL;
                        comm.CommandTimeout = this.CommandTimeout;
                        comm.Transaction = tran;
                        foreach (OleDbParameter parameter in tc.Parameters)
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
        public OleDbDataReader GetReader(string strSelect, OleDbConnection conn)
        {
            return this.CommObj(strSelect, conn).ExecuteReader(CommandBehavior.CloseConnection);
        }
        public OleDbDataReader GetReader(string strSelect, List<OleDbParameter> parameters, OleDbConnection conn)
        {
            return this.CommObj(strSelect, parameters, conn).ExecuteReader(CommandBehavior.CloseConnection);
        }
        #endregion

        public void ExecuteNonQueryInt(string sql, System.Data.Common.DbParameter[] prams) {
            throw new NotImplementedException();
        }
    }
}