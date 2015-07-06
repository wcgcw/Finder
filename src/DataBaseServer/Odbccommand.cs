using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.Odbc;

namespace DataBaseServer
{
    public class Odbccommand
    {
        #region 变量
        private int commandTimeout = 30;
        protected string connString;
        #endregion

        #region 构造
        public Odbccommand() {
            
            //this.connString = System.Configuration.ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            
        }
        public Odbccommand(string strConn)
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
        public virtual OdbcConnection ConnObj
        {
            get 
            {
                Odbcconnection c = new Odbcconnection(this.connString);
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
        public OdbcCommand CommObj(string strSql, OdbcConnection conn)
        {
            OdbcCommand comm = new OdbcCommand();
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
        public OdbcCommand CommObj(string strSql, List<OdbcParameter> parameters, OdbcConnection conn)
        {
            OdbcCommand comm = this.CommObj(strSql, conn);
            foreach (OdbcParameter parameter in parameters)
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
        public bool ExecuteNonQuery(string strSql, OdbcConnection conn)
        {
            using (OdbcCommand comm = this.CommObj(strSql, conn))
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
            using (OdbcConnection conn = this.ConnObj)
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
                {//conn.State=ConnectionState.Closed;
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
        public bool ExecuteNonQuery(string strSql, List<OdbcParameter> parameters, OdbcConnection conn)
        {
            using (OdbcCommand comm = this.CommObj(strSql, parameters, conn))
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
        public bool ExecuteNonQuery(string strSql, List<OdbcParameter> parameters)
        {
            using (OdbcConnection conn = this.ConnObj)
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
        public int ExecuteNonQueryInt(string strSql, OdbcConnection conn)
        {
            using (OdbcCommand comm = this.CommObj(strSql, conn))
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
            using (OdbcConnection conn = this.ConnObj)
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
        public int ExecuteNonQueryInt(string strSql, List<OdbcParameter> parameters, OdbcConnection conn)
        {
            using (OdbcCommand comm = this.CommObj(strSql, parameters, conn))
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
        public int ExecuteNonQueryInt(string strSql, List<OdbcParameter> parameters)
        {
            using (OdbcConnection conn = this.ConnObj)
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

        #region 获取DataSet对象
        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <param name="strSelect">查询语句</param>
        /// <param name="conn">数据库连接</param>
        /// <returns></returns>
        public DataSet GetDataSet(string strSelect, OdbcConnection conn)
        {
            using (OdbcDataAdapter sda = new OdbcDataAdapter(this.CommObj(strSelect, conn)))
            {
                DataSet ds = new DataSet();
                sda.Fill(ds);
                return ds;
            }
        }
        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <param name="strSelect">查询语句</param>
        /// <returns></returns>
        public DataSet GetDataSet(string strSelect) {
            using (OdbcConnection conn = this.ConnObj)
            {
                try {
                    conn.Open();
                    return GetDataSet(strSelect, conn);
                } catch (Exception err) {
                    throw err;
                } finally {
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
        public DataTable GetTabel(string strSelect, OdbcConnection conn)
        {
            using (OdbcDataAdapter sda = new OdbcDataAdapter(this.CommObj(strSelect, conn)))
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
            using (OdbcConnection conn = this.ConnObj)
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
        public DataTable GetTabel(string strSelect, List<OdbcParameter> parameters, OdbcConnection conn)
        {
            using (OdbcDataAdapter sda = new OdbcDataAdapter(this.CommObj(strSelect, parameters, conn)))
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
        public DataTable GetTabel(string strSelect, List<OdbcParameter> parameters)
        {
            using (OdbcConnection conn = this.ConnObj)
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
        public object GetOne(string strSelect, OdbcConnection conn)
        {
            using (OdbcCommand comm = this.CommObj(strSelect, conn))
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
            using (OdbcConnection conn = this.ConnObj)
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
        public object GetOne(string strSelect, List<OdbcParameter> parameters, OdbcConnection conn)
        {
            using (OdbcCommand comm = this.CommObj(strSelect, parameters, conn))
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
        public object GetOne(string strSelect, List<OdbcParameter> parameters)
        {
            using (OdbcConnection conn = this.ConnObj)
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
            using (OdbcConnection conn = this.ConnObj)
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
        public bool Transaction(List<string> strSQL, OdbcConnection conn)
        {
            OdbcTransaction tran = conn.BeginTransaction();
            OdbcCommand comm = conn.CreateCommand();
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
        public bool Transaction(string strSQL, List<List<OdbcParameter>> parameterCollection)
        {
            using (OdbcConnection conn = this.ConnObj)
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
        public bool Transaction(string strSQL, List<List<OdbcParameter>> parameterCollection, OdbcConnection conn)
        {
            OdbcTransaction tran = conn.BeginTransaction();
            int count = 0;
            try
            {
                foreach (List<OdbcParameter> paramters in parameterCollection)
                {
                    using (OdbcCommand comm = conn.CreateCommand())
                    {
                        comm.CommandText = strSQL;
                        comm.CommandTimeout = CommandTimeout;
                        comm.Transaction = tran;
                        foreach (OdbcParameter parameter in paramters)
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
        public bool Transaction(List<OdbcTransCollection> transt)
        {
            using (OdbcConnection conn = this.ConnObj)
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
        public bool Transaction(List<OdbcTransCollection> transt, OdbcConnection conn)
        {
            OdbcTransaction tran = conn.BeginTransaction();
            int count = 0;
            try
            {
                foreach (OdbcTransCollection tc in transt)
                {
                    using (OdbcCommand comm = conn.CreateCommand())
                    {
                        comm.CommandText = tc.StrSQL;
                        comm.CommandTimeout = this.CommandTimeout;
                        comm.Transaction = tran;
                        foreach (OdbcParameter parameter in tc.Parameters)
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
        public OdbcDataReader GetReader(string strSelect, OdbcConnection conn)
        {
            return this.CommObj(strSelect, conn).ExecuteReader(CommandBehavior.CloseConnection);
        }
        public OdbcDataReader GetReader(string strSelect, List<OdbcParameter> parameters, OdbcConnection conn)
        {
            return this.CommObj(strSelect, parameters, conn).ExecuteReader(CommandBehavior.CloseConnection);
        }
        #endregion

        public void ExecuteNonQueryInt(string sql, System.Data.Common.DbParameter[] prams) {
            throw new NotImplementedException();
        }
    }
}
