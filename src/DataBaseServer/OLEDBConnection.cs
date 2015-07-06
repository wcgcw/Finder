using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Configuration;

namespace DataBaseServer
{
    /// <summary>
    /// 数据库连接
    /// </summary>
    public class OLEDBConnection
    {
        private string connStr;
        /// <summary>
        /// 数据库连接
        /// </summary>
        public OLEDBConnection()
        { }
        /// <summary>
        /// 数据库连接
        /// </summary>
        /// <param name="connectionString">连接字符串名</param>
        public OLEDBConnection(string connectionString)
        {
            connStr = connectionString;
        }
        /// <summary>
        /// 连接字符串
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
        /// 连接对像
        /// </summary>
        public OleDbConnection ConnObj
        {
            get
            {
                return new OleDbConnection(this.ConnStr);
            }
        }
    }
}