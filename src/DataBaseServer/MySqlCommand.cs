using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace DataBaseServer
{
    public class MySqlTransCollection
    {
        public string StrSQL;
        public List<MySqlParameter> Parameters;
    }

    /// <summary>
    /// 2016.10.31 增加mysql的数据库支持
    /// </summary>
    public class MySqlCmd
    {        
        #region 变量
        private int _cmdTimeout = 30;
        protected string connString;
        #endregion
        
        #region 构造
        public MySqlCmd() {
            this.connString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
        }
        public MySqlCmd(string strConn)
        {
            this.connString = strConn;
        }
        #endregion

        #region 属性
        /// <summary>
        /// 设置连接超时（秒）
        /// </summary>
        public int CmdTimeout
        {
            get { return _cmdTimeout; }
            set { _cmdTimeout = value; }
        }

        /// <summary>
        /// 数据库连接对像
        /// </summary>
        public virtual MySqlConnection ConnObj
        {
            get 
            {
                MySqlConnection c = new MySqlConnection(this.connString);
                return c;
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
        public MySqlCommand CommObj(string strSql, MySqlConnection conn)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = strSql;
            comm.Connection = conn;
            comm.CommandTimeout = this.CmdTimeout;
            return comm;
        }
        /// <summary>
        /// SQL操作对像
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <param name="conn">数据库连接</param>
        /// <returns></returns>
        public MySqlCommand CommObj(string strSql, List<MySqlParameter> parameters, MySqlConnection conn)
        {
            MySqlCommand comm = this.CommObj(strSql, conn);
            foreach (MySqlParameter parameter in parameters)
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
        public bool ExecuteNonQuery(string strSql, MySqlConnection conn)
        {
            using (MySqlCommand comm = this.CommObj(strSql, conn))
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
            using (MySqlConnection conn = this.ConnObj)
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
        public bool ExecuteNonQuery(string strSql, List<MySqlParameter> parameters, MySqlConnection conn)
        {
            using (MySqlCommand comm = this.CommObj(strSql, parameters, conn))
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
        public bool ExecuteNonQuery(string strSql, List<MySqlParameter> parameters)
        {
            using (MySqlConnection conn = this.ConnObj)
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
        public int ExecuteNonQueryInt(string strSql, MySqlConnection conn)
        {
            using (MySqlCommand comm = this.CommObj(strSql, conn))
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
            using (MySqlConnection conn = this.ConnObj)
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
        public int ExecuteNonQueryInt(string strSql, List<MySqlParameter> parameters, MySqlConnection conn)
        {
            using (MySqlCommand comm = this.CommObj(strSql, parameters, conn))
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
        public int ExecuteNonQueryInt(string strSql, List<MySqlParameter> parameters)
        {
            using (MySqlConnection conn = this.ConnObj)
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
        public DataTable GetTabel(string strSelect, MySqlConnection conn)
        {
            using (MySqlDataAdapter sda = new MySqlDataAdapter(this.CommObj(strSelect, conn)))
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
            using (MySqlConnection conn = this.ConnObj)
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
        public DataTable GetTabel(string strSelect, List<MySqlParameter> parameters, MySqlConnection conn)
        {
            using (MySqlDataAdapter sda = new MySqlDataAdapter(this.CommObj(strSelect, parameters, conn)))
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
        public DataTable GetTabel(string strSelect, List<MySqlParameter> parameters)
        {
            using (MySqlConnection conn = this.ConnObj)
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
        public object GetOne(string strSelect, MySqlConnection conn)
        {
            using (MySqlCommand comm = this.CommObj(strSelect, conn))
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
            using (MySqlConnection conn = this.ConnObj)
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
        public object GetOne(string strSelect, List<MySqlParameter> parameters, MySqlConnection conn)
        {
            using (MySqlCommand comm = this.CommObj(strSelect, parameters, conn))
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
        public object GetOne(string strSelect, List<MySqlParameter> parameters)
        {
            using (MySqlConnection conn = this.ConnObj)
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
            using (MySqlConnection conn = this.ConnObj)
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
        public bool Transaction(List<string> strSQL, MySqlConnection conn)
        {

            MySqlTransaction tran = conn.BeginTransaction();
            MySqlCommand comm = conn.CreateCommand();
            comm.Transaction = tran;
            comm.CommandTimeout = this.CmdTimeout;
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
        public bool Transaction(string strSQL, List<List<MySqlParameter>> parameterCollection)
        {
            using (MySqlConnection conn = this.ConnObj)
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
        public bool Transaction(string strSQL, List<List<MySqlParameter>> parameterCollection, MySqlConnection conn)
        {
            MySqlTransaction tran = conn.BeginTransaction();
            int count = 0;
            try
            {
                foreach (List<MySqlParameter> paramters in parameterCollection)
                {
                    using (MySqlCommand comm = conn.CreateCommand())
                    {
                        comm.CommandText = strSQL;
                        comm.CommandTimeout = CmdTimeout;
                        comm.Transaction = tran;
                        foreach (MySqlParameter parameter in paramters)
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
        public bool Transaction(List<MySqlTransCollection> transt)
        {
            using (MySqlConnection conn = this.ConnObj)
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
        public bool Transaction(List<MySqlTransCollection> transt, MySqlConnection conn)
        {
            MySqlTransaction tran = conn.BeginTransaction();
            int count = 0;
            try
            {
                foreach (MySqlTransCollection tc in transt)
                {
                    using (MySqlCommand comm = conn.CreateCommand())
                    {
                        comm.CommandText = tc.StrSQL;
                        comm.CommandTimeout = this.CmdTimeout;
                        comm.Transaction = tran;
                        foreach (MySqlParameter parameter in tc.Parameters)
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
        public MySqlDataReader GetReader(string strSelect, MySqlConnection conn)
        {
            return this.CommObj(strSelect, conn).ExecuteReader(CommandBehavior.CloseConnection);
        }
        public MySqlDataReader GetReader(string strSelect, List<MySqlParameter> parameters, MySqlConnection conn)
        {
            return this.CommObj(strSelect, parameters, conn).ExecuteReader(CommandBehavior.CloseConnection);
        }
        #endregion

        public void ExecuteNonQueryInt(string sql, System.Data.Common.DbParameter[] prams)
        {
            throw new NotImplementedException();
        }

    }
}
