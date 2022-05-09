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
            string tableName = DbHelpers.SanitizeTableName(repo.Name);
            if (String.IsNullOrEmpty(tableName))
            {
                new Exception($"The table name is not correct {tableName}");
            }

            if (!database.GetTables().Any(m => m.Name == tableName))
            {
                database.CreateTable(tableName);
            }
        }
    }
}
