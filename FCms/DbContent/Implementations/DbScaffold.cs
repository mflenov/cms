using System;
using System.Linq;
using FCms.DbContent.Db;
using System.Threading.Tasks;

namespace FCms.DbContent
{
    internal class DbScaffold
    {
        
        IDatabase database;

        public DbScaffold(DbType dbType)
        {
            // will need to move it out
            if (dbType == DbType.Microsoft)
                database = new MsSqlDatabase();
            if (dbType == DbType.PostgresSQL)
                database = new PgSqlDatabase();
        }

        public async Task<bool> ScaffoldRepository(IDbRepository repo)
        {
            if (String.IsNullOrEmpty(repo.TableName))
            {
                new Exception($"The table name is not correct {repo.TableName}");
            }

            await database.CreateTable(repo.TableName);
            return await database.CreateColumns(repo.TableName, repo.ContentDefinitions.Select(m => new Models.ColumnModel(m)));
        }
    }
}
