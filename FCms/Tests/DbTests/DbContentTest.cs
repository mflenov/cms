using System;
using System.Linq;
using System.Transactions;
using Xunit;
using FCms.Content;
using FCms.DbContent;
using FCms.DbContent.Db;
using Microsoft.Data.SqlClient;
using FCms.Tests.Helpers;
using Dapper;
using System.Threading.Tasks;

namespace FCmsTests.DbTests
{
    [Trait("Category", "Integration")]
    public class DbContentTest
    {
        public DbContentTest()
        {
            FCms.CMSConfigurator.Configure("./", FCmsTests.Helpers.TestConstants.TestDbConnectionString);
        }

        [Fact]
        public async Task AddRowTest()
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            using (SqlConnection connection = MsSqlDbConnection.CreateConnection())
            {
                IDbRepository repository = DbTestHelpers.CreateRepositoryWithSimpleDefinition();
                DbContentStore store = new DbContentStore(repository);

                object[] columns = { "Name", "Description", DateTime.Today };
                await store.Add(columns.ToList());
                var result = connection.Query($"select * from {DbTestHelpers.REPOSITORY_DB_NAME}").First();
                Assert.Equal("Name", result.Name);
                Assert.Equal("Description", result.Description);
            }
        }
    }
}
