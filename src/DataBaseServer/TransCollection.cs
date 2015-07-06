using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace DataBaseServer
{
    /// <summary>
    /// 事务SQL语句和参数对应建构体
    /// </summary>
    public struct TransCollection
    {
        public string StrSQL;
        public List<SqlParameter> Parameters;
    }
}
