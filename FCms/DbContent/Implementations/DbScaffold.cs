using System;
using System.Linq;
using FCms.DbContent.Interfaces;
using FCms.DbContent.Db;

namespace FCms.DbContent.Implementations
{
    internal class DbScaffold
    {
        IDatabase database;

        public DbScaffold()
        {
            database = new MsSqlDatabase();
        }

        public void ScaffoldRepository(Content.IRepository repo)
        {
            string tableName = DbHelpers.SanitizeDbName(repo.Name);
            if (String.IsNullOrEmpty(tableName))
            {
                new Exception($"The table name is not correct {tableName}");
            }

            database.CreateTable(tableName);
            database.CreateColumns(tableName, repo.ContentDefinitions.Select(m => new Models.DbColumnModel(m)));
        }
    }
}
