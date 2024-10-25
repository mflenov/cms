using FCms.Content;
using FCms.DbContent.Db;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCms.DbContent
{
    public interface IDbRepository : IRepository
    {
        public DbType DatabaseType { get; init; }
        public string TableName { get; }
        Task<bool> Scaffold();

        internal IDatabase GetDatabase() {
            if (DatabaseType == DbType.Microsoft)
                return new MsSqlDatabase();
            if (DatabaseType == DbType.PostgresSQL)
                return new PgSqlDatabase();

            throw new Exception("DB type is not supported");
        }
    }
}
