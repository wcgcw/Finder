using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Configuration;

namespace DataBaseServer
{
    /// <summary>
    /// ���ݿ�����
    /// </summary>
    public class Connection
    {
        private string connStr;
        /// <summary>
        /// ���ݿ�����
        /// </summary>
        public Connection()
        { }
        /// <summary>
        /// ���ݿ�����
        /// </summary>
        /// <param name="connectionString">�����ַ�����</param>
        public Connection(string connectionString)
        {
            connStr = connectionString;
        }
        /// <summary>
        /// �����ַ���
        /// </summary>
        public string ConnStr
        {
            get
            {
                return connStr;
            }
            set
            {
                connStr = value;
            }
        }
        /// <summary>
        /// ���Ӷ���
        /// </summary>
        public SqlConnection ConnObj
        {
            get
            {
                return new SqlConnection(this.ConnStr);
            }
        }
    }
}
