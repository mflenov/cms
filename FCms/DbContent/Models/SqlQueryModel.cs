using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace FCms.DbContent.Models
{
    internal class SqlQueryModel
    {
        public SqlQueryModel() { }

        public SqlQueryModel (string sql, List<SqlParameter> parameters)
        {

        }
        public string Sql { get; set; }

        public List<SqlParameter> Parameters { get; set; } = new List<SqlParameter>();
    }
}
