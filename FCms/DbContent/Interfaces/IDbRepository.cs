using FCms.Content;
using FCms.DbContent.Db;
using System;
using System.Threading.Tasks;

namespace FCms.DbContent
{
    public interface IDbRepository : IRepository
    {
        public IDbConnection DatabaseConnection { get; set; }

        public string TableName { get; }
        
        Task<bool> Scaffold();

        internal IDatabase GetDatabase() {
            if (DatabaseConnection.DatabaseType == DbType.Microsoft)
                return new MsSqlDatabase(DatabaseConnection.ConnectionString);
            if (DatabaseConnection.DatabaseType == DbType.PostgresSQL)
                return new PgSqlDatabase(DatabaseConnection.ConnectionString);

            throw new Exception("DB type is not supported");
        }
    }
}
