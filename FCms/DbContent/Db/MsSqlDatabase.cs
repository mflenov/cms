using System.Linq;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Dapper;
using FCms.DbContent;
using FCms.DbContent.Models;
using System.Threading.Tasks;

namespace FCms.DbContent.Db
{
    internal class MsSqlDatabase : IDatabase
    {

        public MsSqlDatabase()
        {
        }

        public async Task<IEnumerable<DbTableModel>> GetTables()
        {
            using (SqlConnection connection = MsSqlDbConnection.CreateConnection())
            {
                return await connection.QueryAsync<DbTableModel>("select Name from sys.tables");
            }
        }

        public async Task<IEnumerable<DbColumnModel>> GetTableColumns(string tableName)
        {
            using (SqlConnection connection = MsSqlDbConnection.CreateConnection())
            {
                return await connection.QueryAsync<DbColumnModel>(@"
                    select c.Name as ColumnName, t.Name + 'Value' as TypeName
                    from sys.columns c
	                    join sys.types t on c.system_type_id = t.user_type_id
                    where c.Object_ID = Object_Id(@tablename)
                    ", new { tablename = tableName });
            }
        }

        public async Task<bool> CreateTable(string tableName)
        {
            var tables = await GetTables();
            using (SqlConnection connection = MsSqlDbConnection.CreateConnection())
            {
                if (!tables.Any(m => m.Name == tableName))
                {
                    await connection.ExecuteAsync($"create table [{tableName}] ({tableName}Id int not null identity (1,1) primary key)");
                }
            }
            return true;
        }

        public async Task<bool> CreateColumns(string tableName, IEnumerable<ColumnModel> columns)
        {
            var tables = await GetTableColumns(tableName);
            var dbcolumns = tables.ToDictionary(m => m.ColumnName, m => m);
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

            if (columnsSqlStatements.Count == 0)
                return true;
            using (SqlConnection connection = MsSqlDbConnection.CreateConnection())
            {
                await connection.ExecuteAsync($"alter table {tableName} add " + string.Join(",", columnsSqlStatements));
            }

            return true;
        }

        public async Task<List<List<string>>> GetContent(string tableName)
        {
            List<List<string>> result = new List<List<string>>();
            using (SqlConnection connection = MsSqlDbConnection.CreateConnection())
            {
                var command = new SqlCommand("", connection);
                var datareader = await command.ExecuteReaderAsync();
                while (datareader.Read())
                {
                    List<string> row = new List<string>();
                    for (int i = 0; i < datareader.FieldCount; i++)
                        row.Add(datareader.GetString(i));
                    result.Add(row);
                }
            }
            return result;
        }

        public async Task<int> AddRow(string tableName, List<object> values, List<ColumnModel> columns)
        {
            using (SqlConnection connection = MsSqlDbConnection.CreateConnection())
            {
                await connection.OpenAsync();
                var command = new SqlCommand(
                    $"insert into {tableName} ({string.Join(",", columns.Select(m => m.Name))}) " +
                    "values (" + string.Join(",", Enumerable.Range(0, values.Count).Select(m => "@v" + m.ToString())) + ")"
                    , connection);
                for (int index = 0; index < values.Count; index++)
                {
                    var parameter = new SqlParameter("v" + index.ToString(), columns[index].GetSqlDbTypeName());
                    parameter.Value = values[index];
                    command.Parameters.Add(parameter);
                }
                return await command.ExecuteNonQueryAsync();
            }
        }
    }
}