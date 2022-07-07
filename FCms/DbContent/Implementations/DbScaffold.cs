using System;
using System.Linq;
using FCms.DbContent;
using FCms.DbContent.Db;
using System.Threading.Tasks;

namespace FCms.DbContent
{
    internal class DbScaffold
    {
        IDatabase database;

        public DbScaffold()
        {
            database = new MsSqlDatabase();
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
