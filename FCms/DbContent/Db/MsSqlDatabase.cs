using System.Linq;
using System.Collections.Generic;
using FCms.DbContent.Interfaces;
using Microsoft.Data.SqlClient;
using Dapper;
using FCms.DbContent.Models;

namespace FCms.DbContent.Db
{
    internal class MsSqlDatabase : IDatabase
    {

        public MsSqlDatabase()
        {
        }

        public IEnumerable<DbTableModel> GetTables()
        {
            using (SqlConnection connection = MsSqlDbConnection.CreateConnection())
            {
                return connection.Query<Models.DbTableModel>("select Name from sys.tables");
            }
        }

        public IEnumerable<DbColumnModel> GetTableColumns(string tableName)
        {
            using (SqlConnection connection = MsSqlDbConnection.CreateConnection())
            {
                return connection.Query<DbColumnModel>(@"
                    select c.Name as ColumnName, t.Name + 'Value' as TypeName
                    from sys.columns c
	                    join sys.types t on c.system_type_id = t.user_type_id
                    where c.Object_ID = Object_Id(@tablename)
                    ", new { tablename  = tableName });
            }
        }

        public void CreateTable(string tableName)
        {
            var tables = GetTables();
            using (SqlConnection connection = MsSqlDbConnection.CreateConnection())
            {
                if (!tables.Any(m => m.Name == tableName))
                {
                    connection.Execute($"create table [{tableName}] ({tableName}Id int not null identity (1,1) primary key)");
                }
            }
        }

        public void CreateColumns(string tableName, IEnumerable<ColumnModel> columns)
        {
            var dbcolumns = GetTableColumns(tableName).ToDictionary(m => m.ColumnName, m => m);
            List<string> columnsSqlStatements = new List<string>();

            foreach (var column in columns)
            {
                if (!dbcolumns.ContainsKey(column.Name))
                    columnsSqlStatements.Add($"[{column.Name}] {column.GetDbTypeName()}");
            }

            if (!dbcolumns.ContainsKey("_modified"))
                columnsSqlStatements.Add("_modified datetime");
            if (!dbcolumns.ContainsKey("_created"))
                columnsSqlStatements.Add("_created datetime");

            using (SqlConnection connection = MsSqlDbConnection.CreateConnection())
            {
                connection.Execute($"alter table {tableName} add " + string.Join(",", columnsSqlStatements));
            }
        }
    }
}
