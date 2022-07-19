using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace FCms.DbContent.Models
{
    public class SqlQueryModel
    {
        public SqlQueryModel() { }

        public SqlQueryModel (string sql, List<SqlParameter> parameters)
        {
            Sql = sql;
            Parameters = parameters;
        }

        public string Sql { get; set; }

        public List<SqlParameter> Parameters { get; set; } = new List<SqlParameter>();
    }
}
