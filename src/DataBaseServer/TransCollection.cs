using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace DataBaseServer
{
    /// <summary>
    /// ����SQL���Ͳ�����Ӧ������
    /// </summary>
    public struct TransCollection
    {
        public string StrSQL;
        public List<SqlParameter> Parameters;
    }
}
