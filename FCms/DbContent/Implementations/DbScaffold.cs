using System;
using System.Linq;
using FCms.DbContent.Db;
using System.Threading.Tasks;

namespace FCms.DbContent
{
    internal class DbScaffold
    {
        
        public DbScaffold()
        {
        }

        public async Task<bool> ScaffoldRepository(IDbRepository repo)
        {
            if (String.IsNullOrEmpty(repo.TableName))
            {
                new Exception($"The table name is not correct {repo.TableName}");
            }
            IDatabase database = repo.GetDatabase();

            await database.CreateTable(repo.TableName);
            return await database.CreateColumns(repo.TableName, repo.ContentDefinitions.Select(m => new Models.ColumnModel(m)));
        }
    }
}
