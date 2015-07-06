using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace DataBaseServer
{
    /// <summary>
    /// SQLִ��
    /// </summary>
    public class Command
    {
        #region ����
        private int commandTimeout = 30;
        protected string connString;
        #endregion

        #region ����
        public Command() {
            
            this.connString = System.Configuration.ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            
        }
        public Command(string strConn)
        {
            this.connString = strConn;
        }
        #endregion

        #region ����
        /// <summary>
        /// �������ӳ�ʱ���룩
        /// </summary>
        public int CommandTimeout
        {
            get { return commandTimeout; }
            set { commandTimeout = value; }
        }

        /// <summary>
        /// ���ݿ����Ӷ���
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

        #region command����
        /// <summary>
        /// SQL��������
        /// </summary>
        /// <param name="strSql">SQL���</param>
        /// <param name="conn">���ݿ�����</param>
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
        /// SQL��������
        /// </summary>
        /// <param name="strSql">SQL���</param>
        /// <param name="parameters">SQL����</param>
        /// <param name="conn">���ݿ�����</param>
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

        #region ִ��SQL���
        /// <summary>
        /// ִ��SQL���
        /// </summary>
        /// <param name="strSql">SQL���</param>
        /// <param name="conn">���ݿ�����</param>
        /// <returns></returns>
        public bool ExecuteNonQuery(string strSql, SqlConnection conn)
        {
            using (SqlCommand comm = this.CommObj(strSql, conn))
            {
                return comm.ExecuteNonQuery() > 0 ? true : false;
            }
        }
        /// <summary>
        /// ִ��SQL���
        /// </summary>
        /// <param name="strSql">SQL���</param>
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
        /// ִ��SQL���
        /// </summary>
        /// <param name="strSql">SQL���</param>
        /// <param name="parameters">SQL����</param>
        /// <param name="conn">���ݿ�����</param>
        /// <returns></returns>
        public bool ExecuteNonQuery(string strSql, List<SqlParameter> parameters, SqlConnection conn)
        {
            using (SqlCommand comm = this.CommObj(strSql, parameters, conn))
            {
                return comm.ExecuteNonQuery() > 0 ? true : false;
            }
        }
        /// <summary>
        /// ִ��SQL���
        /// </summary>
        /// <param name="strSql">SQL���</param>
        /// <param name="parameters">SQL����</param>
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

        #region ִ��SQL��䷵��Int
        /// <summary>
        /// ִ��SQL���
        /// </summary>
        /// <param name="strSql">SQL���</param>
        /// <param name="conn">���ݿ�����</param>
        /// <returns></returns>
        public int ExecuteNonQueryInt(string strSql, SqlConnection conn)
        {
            using (SqlCommand comm = this.CommObj(strSql, conn))
            {
                return comm.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// ִ��SQL���
        /// </summary>
        /// <param name="strSql">SQL���</param>
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
        /// ִ��SQL���
        /// </summary>
        /// <param name="strSql">SQL���</param>
        /// <param name="parameters">SQL����</param>
        /// <param name="conn">���ݿ�����</param>
        /// <returns></returns>
        public int ExecuteNonQueryInt(string strSql, List<SqlParameter> parameters, SqlConnection conn)
        {
            using (SqlCommand comm = this.CommObj(strSql, parameters, conn))
            {
                return comm.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// ִ��SQL���
        /// </summary>
        /// <param name="strSql">SQL���</param>
        /// <param name="parameters">SQL����</param>
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

        #region ��ȡDataSet����
        /// <summary>
        /// ��ȡ���ݱ�
        /// </summary>
        /// <param name="strSelect">��ѯ���</param>
        /// <param name="conn">���ݿ�����</param>
        /// <returns></returns>
        public DataSet GetDataSet(string strSelect, SqlConnection conn) {
            using (SqlDataAdapter sda = new SqlDataAdapter(this.CommObj(strSelect, conn))) {
                DataSet ds = new DataSet();
                sda.Fill(ds);
                return ds;
            }
        }
        /// <summary>
        /// ��ȡ���ݱ�
        /// </summary>
        /// <param name="strSelect">��ѯ���</param>
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

        #region ��ѯ
        /// <summary>
        /// ��ȡ���ݱ�
        /// </summary>
        /// <param name="strSelect">��ѯ���</param>
        /// <param name="conn">���ݿ�����</param>
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
        /// ��ȡ���ݱ�
        /// </summary>
        /// <param name="strSelect">��ѯ���</param>
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
        /// ��ȡ���ݱ�
        /// </summary>
        /// <param name="strSelect">��ѯ���</param>
        /// <param name="parameters">SQL����</param>
        /// <param name="conn">���ݿ�����</param>
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
        /// ��ȡ���ݱ�
        /// </summary>
        /// <param name="strSelect">��ѯ���</param>
        /// <param name="parameters">SQL����</param>
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
        /// ��ȡ��һ�е�һ��
        /// </summary>
        /// <param name="strSelect">��ѯ���</param>
        /// <param name="conn">���ݿ�����</param>
        /// <returns></returns>
        public object GetOne(string strSelect, SqlConnection conn)
        {
            using (SqlCommand comm = this.CommObj(strSelect, conn))
            {
                return comm.ExecuteScalar();
            }
        }
        /// <summary>
        /// ��ȡ��һ�е�һ��
        /// </summary>
        /// <param name="strSelect">��ѯ���</param>
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
        /// ��ȡ��һ�е�һ��
        /// </summary>
        /// <param name="strSelect">��ѯ���</param>
        /// <param name="parameters">SQL����</param>
        /// <param name="conn">���ݿ�����</param>
        /// <returns></returns>
        public object GetOne(string strSelect, List<SqlParameter> parameters, SqlConnection conn)
        {
            using (SqlCommand comm = this.CommObj(strSelect, parameters, conn))
            {
                return comm.ExecuteScalar();
            }
        }
        /// <summary>
        /// ��ȡ��һ�е�һ��
        /// </summary>
        /// <param name="strSelect">��ѯ���</param>
        /// <param name="parameters">SQL����</param>
        /// <param name="conn">���ݿ�����</param>
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

        #region ����
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="strSQL">SQL�����</param>
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
        /// ����
        /// </summary>
        /// <param name="strSQL">SQL�����</param>
        /// <param name="conn">�Ѵ򿪵�SQL����</param>
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
        /// ����
        /// </summary>
        /// <param name="strSQL">SQL���</param>
        /// <param name="parameterCollection">SQL����</param>
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
        /// ����
        /// </summary>
        /// <param name="strSQL">SQL���</param>
        /// <param name="parameterCollection">SQL����</param>
        /// <param name="conn">SQL����</param>
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
        /// ����
        /// </summary>
        /// <param name="transt">����SQL���Ͳ�����Ӧ������</param>
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
        /// ����
        /// </summary>
        /// <param name="transt">����SQL���Ͳ�����Ӧ������</param>
        /// <param name="conn">SQL����</param>
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

        #region ����Reader����
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
