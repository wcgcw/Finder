using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace DataBaseServer
{
    public class SQLiteTransCollection
    {
        public string StrSQL;
        public List<SQLiteParameter> Parameters;
    }
}
