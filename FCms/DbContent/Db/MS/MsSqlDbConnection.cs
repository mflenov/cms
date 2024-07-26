using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using System.Collections.Generic;

namespace FCms.DbContent.Db;

internal class MsSqlDbConnection
{
    public static SqlConnection CreateConnection()
    {
        return new SqlConnection(CMSConfigurator.DbConnection);
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
