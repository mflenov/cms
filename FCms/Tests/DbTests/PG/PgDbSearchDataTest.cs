using System.Transactions;
using Xunit;
using FCms.DbContent;
using FCms.DbContent.Db;
using Npgsql;
using System.Linq;
using FCms.Tests.Helpers;
using System.Threading.Tasks;
using FCms.DbContent.Models;
using System;

namespace FCmsTests.DbTests
{
    [Trait("Category", DbHelpersTest.TEST_CATEGORY_INTEGRATION)]
    [Trait("Category", DbHelpersTest.TEST_CATEGORY_PGSQL)]
    [Collection("Sequential")]
    public class PgDbSearchDataTest
    {
        public PgDbSearchDataTest()
        {
            FCms.CMSConfigurator.Configure("./", FCmsTests.Helpers.TestConstants.TestPgDbConnectionString);
        }

        [Fact]
        public async Task FindAllTest()
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            using (NpgsqlConnection connection = PgSqlDbConnection.CreateConnection())
            {
                IDbRepository repository = DbTestHelpers.CreateRepositoryWithSimpleDefinition(DbType.PostgresSQL);
                DbContentStore store = new DbContentStore(repository);

                for (int i = 0; i < 10; i++)
                {
                    object[] columns = { $"Row{i}Name", $"Row{i}Description", DateTime.Today.AddMinutes(i * -10) };
                    await store.Add(columns.ToList());
                }

                ContentSearchRequest request = new ContentSearchRequest();
                var content = await store.GetContent(request);

                for (int i = 0; i < 10; i++)
                {
                    Assert.Equal(content.Rows[i].GetStringValue(1), $"Row{i}Name");
                    Assert.Equal(content.Rows[i].GetStringValue(2), $"Row{i}Description");
                }
            }
        }

    }
}
