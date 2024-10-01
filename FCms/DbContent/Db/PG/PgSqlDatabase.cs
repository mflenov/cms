using System.Linq;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Npgsql;
using Dapper;
using FCms.DbContent.Models;
using System.Threading.Tasks;

namespace FCms.DbContent.Db
{
    internal class PgSqlDatabase : IDatabase
    {
        PgSqlDbConnection connection = new PgSqlDbConnection();

        public PgSqlDatabase()
        {
        }

        public async Task<IEnumerable<DbTableModel>> GetTables()
        {
            return await connection.QueryAsync<DbTableModel>("SELECT table_name as Name FROM information_schema.tables WHERE table_schema='public' AND table_type='BASE TABLE' ", new {});
        }

        public async Task<IEnumerable<DbColumnModel>> GetTableColumns(string tableName)
        {
            return await connection.QueryAsync<DbColumnModel>(@"
                SELECT column_name as ColumnName, 
                    case
                     when data_type = 'integer' then 'IntValue'
                     when data_type='character varying' then 'NvarcharValue'
                    end as DatabaseType
                FROM information_schema.columns WHERE table_schema='public' AND table_name=@tablename 
                    ", new { tablename = tableName });
        }

        public async Task<bool> CreateTable(string tableName)
        {
            var tables = await GetTables();
            using (NpgsqlConnection connection = PgSqlDbConnection.CreateConnection())
            {
                if (!tables.Any(m => m.Name == tableName))
                {
                    await connection.ExecuteAsync($"create table \"{tableName}\" (\"{tableName}Id\" serial primary key)", new {});
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
                    columnsSqlStatements.Add($"\"{column.Name}\" {column.GetPgDbTypeName()}");
            }

            if (!dbcolumns.ContainsKey("_modified"))
                columnsSqlStatements.Add("_modified timestamp");
            if (!dbcolumns.ContainsKey("_created"))
                columnsSqlStatements.Add("_created timestamp");

            if (columnsSqlStatements.Count == 0)
                return true;
            await connection.ExecuteAsync($"alter table \"{tableName}\" add column " + string.Join(", add column ", columnsSqlStatements), new {});

            return true;
        }

        public async Task<ContentModel> GetContent(string tableName, SqlQueryModel query)
        {
            using (NpgsqlConnection connection = PgSqlDbConnection.CreateConnection())
            {
                await connection.OpenAsync();
                
                var command = new NpgsqlCommand(query.Sql, connection);
                command.Parameters.AddRange(query.Parameters.ToArray());
                var datareader = await command.ExecuteReaderAsync();
                
                return new ContentModel()
                {
                    Columns = GetSchema(datareader).ToList(),
                    Rows = ReadData(datareader)
                };
            }
        }

        private List<ContentRow> ReadData(NpgsqlDataReader datareader)
        {
            List<ContentRow> result = new List<ContentRow>();

            while (datareader.Read())
            {
                List<object> row = new List<object>();
                for (int i = 0; i < datareader.FieldCount; i++)
                    row.Add(datareader.GetValue(i));
                result.Add(new ContentRow(row));
            }
            return result;
        }

        private IEnumerable<ContentColumn> GetSchema(NpgsqlDataReader datareader)
        {
            var columnSchema = datareader.GetColumnSchema();
            return columnSchema.Select(m => new ContentColumn()
            {
                Name = m.ColumnName,
                IsPrimaryKey = m.IsKey == true
            });
        }

        public async Task AddRow(string tableName, List<object> values, List<ColumnModel> columns)
        {
            var command = $"insert into {tableName} ({string.Join(",", columns.Select(m => m.Name))}) " +
                "values (" + string.Join(",", Enumerable.Range(0, values.Count).Select(m => "@v" + m.ToString())) + ")";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            for (int index = 0; index < values.Count; index++) {
                parameters.Add("v" + index.ToString(), values[index]);
            }

            await connection.ExecuteAsync(command, parameters);
        }

        public SqlGenerator GetSqlGenerator(string tableName)
        {
            return new PgSqlGenerator(tableName);
        }

        public async Task DeleteRow(string tableName, string id)
        {
            await connection.ExecuteAsync($"delete from {tableName} where {tableName}Id = @id", new { id = id});
        }
    }
}