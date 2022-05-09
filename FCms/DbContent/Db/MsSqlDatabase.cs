using FCms.DbContent.Interfaces;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Dapper;

namespace FCms.DbContent.Db
{
    internal class MsSqlDatabase : IDatabase
    {

        public MsSqlDatabase()
        {
        }

        public IEnumerable<Models.DbTableModel> GetTables()
        {
            using (SqlConnection connection = MsSqlDbConnection.CreateConnection())
            {
                return connection.Query<Models.DbTableModel>("select Name from sys.tables");
            }
        }

        public void CreateTable(string tableName)
        {
            using (SqlConnection connection = MsSqlDbConnection.CreateConnection())
            {
                connection.Execute($"create table {tableName} ({tableName}Id int not null identity (1,1) primary key)");
            }
        }
    }
}
