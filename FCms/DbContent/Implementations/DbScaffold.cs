using System;
using System.Linq;
using FCms.DbContent;
using FCms.DbContent.Db;

namespace FCms.DbContent
{
    internal class DbScaffold
    {
        IDatabase database;

        public DbScaffold()
        {
            database = new MsSqlDatabase();
        }

        public void ScaffoldRepository(IDbRepository repo)
        {
            if (String.IsNullOrEmpty(repo.TableName))
            {
                new Exception($"The table name is not correct {repo.TableName}");
            }

            database.CreateTable(repo.TableName);
            database.CreateColumns(repo.TableName, repo.ContentDefinitions.Select(m => new Models.ColumnModel(m)));
        }
    }
}
