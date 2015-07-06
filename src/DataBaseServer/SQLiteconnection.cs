using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace DataBaseServer
{
    public class SQLiteconnection
    {
        private string connStr;

        /// <summary>
        /// 数据库连接
        /// </summary>
        public SQLiteconnection()
        {
        }

        /// <summary>
        /// 数据库连接
        /// </summary>
        /// <param name="connectionString">连接字符串名</param>
        public SQLiteconnection(string connectionString)
        {
            connStr = connectionString;
            SQLiteConnection a=new SQLiteConnection();
        }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnStr
        {
            get { return connStr; }
            set { connStr = value; }
            
        }

        /// <summary>
        /// 连接对像
        /// </summary>
        public SQLiteConnection ConnObj
        {
            
            get { return new SQLiteConnection(this.ConnStr); }
        }
    }
}