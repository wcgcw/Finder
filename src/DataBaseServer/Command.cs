using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace DataBaseServer
{
    /// <summary>
    /// SQL执行
    /// </summary>
    public class Command
    {
        #region 变量
        private int commandTimeout = 30;
        protected string connString;
        #endregion

        #region 构造
        public Command() {
            
            this.connString = System.Configuration.ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            
        }
        public Command(string strConn)
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
        public virtual SqlConnection ConnObj
        {
            get 
            {
                Connection c = new Connection(this.connString);
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
        public SqlCommand CommObj(string strSql, SqlConnection conn)
        {
            SqlCommand comm = new SqlCommand();
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
        public SqlCommand CommObj(string strSql, List<SqlParameter> parameters, SqlConnection conn)
        {
            SqlCommand comm = this.CommObj(strSql, conn);
            foreach (SqlParameter parameter in parameters)
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
        public bool ExecuteNonQuery(string strSql, SqlConnection conn)
        {
            using (SqlCommand comm = this.CommObj(strSql, conn))
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
            using (SqlConnection conn = this.ConnObj)
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
        public bool ExecuteNonQuery(string strSql, List<SqlParameter> parameters, SqlConnection conn)
        {
            using (SqlCommand comm = this.CommObj(strSql, parameters, conn))
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
        public bool ExecuteNonQuery(string strSql, List<SqlParameter> parameters)
        {
            using (SqlConnection conn = this.ConnObj)
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
        public int ExecuteNonQueryInt(string strSql, SqlConnection conn)
        {
            using (SqlCommand comm = this.CommObj(strSql, conn))
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
            using (SqlConnection conn = this.ConnObj)
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
        public int ExecuteNonQueryInt(string strSql, List<SqlParameter> parameters, SqlConnection conn)
        {
            using (SqlCommand comm = this.CommObj(strSql, parameters, conn))
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
        public int ExecuteNonQueryInt(string strSql, List<SqlParameter> parameters)
        {
            using (SqlConnection conn = this.ConnObj)
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
        public DataSet GetDataSet(string strSelect, SqlConnection conn) {
            using (SqlDataAdapter sda = new SqlDataAdapter(this.CommObj(strSelect, conn))) {
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
            using (SqlConnection conn = this.ConnObj) {
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
        public DataTable GetTabel(string strSelect, SqlConnection conn)
        {
            using (SqlDataAdapter sda = new SqlDataAdapter(this.CommObj(strSelect, conn)))
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
            using (SqlConnection conn = this.ConnObj)
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
        public DataTable GetTabel(string strSelect, List<SqlParameter> parameters, SqlConnection conn)
        {
            using (SqlDataAdapter sda = new SqlDataAdapter(this.CommObj(strSelect, parameters, conn)))
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
        public DataTable GetTabel(string strSelect, List<SqlParameter> parameters)
        {
            using (SqlConnection conn = this.ConnObj)
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
        public object GetOne(string strSelect, SqlConnection conn)
        {
            using (SqlCommand comm = this.CommObj(strSelect, conn))
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
            using (SqlConnection conn = this.ConnObj)
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
        public object GetOne(string strSelect, List<SqlParameter> parameters, SqlConnection conn)
        {
            using (SqlCommand comm = this.CommObj(strSelect, parameters, conn))
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
        public object GetOne(string strSelect, List<SqlParameter> parameters)
        {
            using (SqlConnection conn = this.ConnObj)
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
            using (SqlConnection conn = this.ConnObj)
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
        public bool Transaction(List<string> strSQL, SqlConnection conn)
        {
            SqlTransaction tran = conn.BeginTransaction();
            SqlCommand comm = conn.CreateCommand();
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
        public bool Transaction(string strSQL, List<List<SqlParameter>> parameterCollection)
        {
            using (SqlConnection conn = this.ConnObj)
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
        public bool Transaction(string strSQL, List<List<SqlParameter>> parameterCollection, SqlConnection conn)
        {
            SqlTransaction tran = conn.BeginTransaction();
            int count = 0;
            try
            {
                foreach (List<SqlParameter> paramters in parameterCollection)
                {
                    using (SqlCommand comm = conn.CreateCommand())
                    {
                        comm.CommandText = strSQL;
                        comm.CommandTimeout = CommandTimeout;
                        comm.Transaction = tran;
                        foreach (SqlParameter parameter in paramters)
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
        public bool Transaction(List<TransCollection> transt)
        {
            using (SqlConnection conn = this.ConnObj)
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
        public bool Transaction(List<TransCollection> transt, SqlConnection conn)
        {
            SqlTransaction tran = conn.BeginTransaction();
            int count = 0;
            try
            {
                foreach (TransCollection tc in transt)
                {
                    using (SqlCommand comm = conn.CreateCommand())
                    {
                        comm.CommandText = tc.StrSQL;
                        comm.CommandTimeout = this.CommandTimeout;
                        comm.Transaction = tran;
                        foreach (SqlParameter parameter in tc.Parameters)
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
        public SqlDataReader GetReader(string strSelect, SqlConnection conn)
        {
            return this.CommObj(strSelect, conn).ExecuteReader(CommandBehavior.CloseConnection);
        }
        public SqlDataReader GetReader(string strSelect, List<SqlParameter> parameters, SqlConnection conn)
        {
            return this.CommObj(strSelect, parameters, conn).ExecuteReader(CommandBehavior.CloseConnection);
        }
        #endregion

        public void ExecuteNonQueryInt(string sql, System.Data.Common.DbParameter[] prams) {
            throw new NotImplementedException();
        }
    }
        
}
