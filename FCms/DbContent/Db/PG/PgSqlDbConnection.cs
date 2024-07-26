using System.Threading.Tasks;
using Npgsql;
using Dapper;
using System.Collections.Generic;

namespace FCms.DbContent.Db;

internal class PgSqlDbConnection
{
    public static NpgsqlConnection CreateConnection()
    {
        return new NpgsqlConnection(CMSConfigurator.DbConnection);
    }

    public async Task ExecuteAsync(string sql, object model)
    {
        using (var connection = CreateConnection())
        {
            await connection.OpenAsync();
            await connection.ExecuteAsync(sql, model);
        }
    }

    public async Task<T> QueryScalarAsync<T>(string sql, object model)
    {
        using (var connection = CreateConnection())
        {
            await connection.OpenAsync();
            var result = await connection.QueryFirstOrDefaultAsync<T>(sql, model);
            return result;
        }
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object model)
    {
        using (var connection = CreateConnection())
        {
            await connection.OpenAsync();
            var result = await connection.QueryAsync<T>(sql, model);
            return result;
        }
    }
}

