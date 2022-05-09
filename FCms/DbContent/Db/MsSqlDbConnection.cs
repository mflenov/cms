using Microsoft.Data.SqlClient;

namespace FCms.DbContent.Db
{
    internal class MsSqlDbConnection
    {
        public static SqlConnection CreateConnection()
        {
            return new SqlConnection(CMSConfigurator.DbConnection);
        }
    }
}
