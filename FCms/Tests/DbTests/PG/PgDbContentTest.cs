using System;
using System.Linq;
using System.Transactions;
using Xunit;
using FCms.DbContent;
using FCms.DbContent.Db;
using FCms.Tests.Helpers;
using Dapper;
using System.Threading.Tasks;

namespace FCmsTests.DbTests
{
    [Trait("Category", DbHelpersTest.TEST_CATEGORY_INTEGRATION)]
    [Trait("Category", DbHelpersTest.TEST_CATEGORY_PGSQL)]
    [Collection("Sequential")]
     public class PgDbContentTest
    {
        public PgDbContentTest()
        {
            FCms.CMSConfigurator.Configure("./", Helpers.TestConstants.TestPgDbConnectionString);
        }

        [Fact]
        public async Task AddRowTest()
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            using (var connection = PgSqlDbConnection.CreateConnection())
            {
                IDbRepository repository = DbTestHelpers.CreateRepositoryWithSimpleDefinition(DbType.PostgresSQL);
                DbContentStore store = new DbContentStore(repository);

                object[] columns = { "Name", "Description", DateTime.Today };
                await store.Add(columns.ToList());
                var result = connection.Query($"select * from \"{DbTestHelpers.REPOSITORY_DB_NAME}\"").First();
                Assert.Equal("Name", result.Name);
                Assert.Equal("Description", result.Description);
            }
        }
    }
}
