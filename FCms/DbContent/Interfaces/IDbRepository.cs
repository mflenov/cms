using FCms.Content;
using FCms.DbContent.Db;
using System;
using System.Threading.Tasks;

namespace FCms.DbContent
{
    public interface IDbRepository : IRepository
    {
        public System.Guid DatabaseConnectionId { get; set; }

        public IDbConnection GetDatabaseConnection();

        public string TableName { get; }
        
        Task<bool> Scaffold();

        internal IDatabase GetDatabase() {
            if (GetDatabaseConnection().DatabaseType == DbType.Microsoft)
                return new MsSqlDatabase(GetDatabaseConnection().ConnectionString);
            if (GetDatabaseConnection().DatabaseType == DbType.PostgresSQL)
                return new PgSqlDatabase(GetDatabaseConnection().ConnectionString);

            throw new Exception("DB type is not supported");
        }
    }
}
