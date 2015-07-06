using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;

namespace DataBaseServer
{
    /// <summary>
    /// 事务SQL语句和参数对应建构体
    /// </summary>
    public class OLEDBTransCollection
    {
        public string StrSQL;
        public List<OleDbParameter> Parameters;
    }
}