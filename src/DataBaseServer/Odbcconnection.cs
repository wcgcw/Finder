using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;

namespace DataBaseServer
{
    public class Odbcconnection
    {
        private string connStr;
        /// <summary>
        /// 数据库连接
        /// </summary>
        public Odbcconnection()
        { }
        /// <summary>
        /// 数据库连接
        /// </summary>
        /// <param name="connectionString">连接字符串名</param>
        public Odbcconnection(string connectionString)
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
        public OdbcConnection ConnObj
        {
            get
            {
                return new OdbcConnection(this.ConnStr);
            }
        }
    }
}
