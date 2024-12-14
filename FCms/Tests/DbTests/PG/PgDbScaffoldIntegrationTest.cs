using System;
using System.Transactions;
using Xunit;
using FCms.Content;
using FCms;
using FCms.DbContent;
using FCms.DbContent.Db;
using Npgsql;
using Dapper;
using System.Linq;
using FCms.Tests.Helpers;
using System.Threading.Tasks;

namespace FCmsTests.DbTests
{
    [Trait("Category", DbHelpersTest.TEST_CATEGORY_INTEGRATION)]
    [Trait("Category", DbHelpersTest.TEST_CATEGORY_PGSQL)]
    [Collection("Sequential")]
    public class PgDbScaffoldIntegrationTest: IDisposable
    {
        public PgDbScaffoldIntegrationTest()
        {
            CMSConfigurator.Configure("./");
        }

        public void Dispose()
        {
            FCmsTests.Helpers.TestTools.DeleteCmsFile();
            FCms.Tools.Cacher.Clear();
        }

        [Fact]
        public async Task EmptyRepositoryTest() {
            using (TransactionScope ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            using (NpgsqlConnection connection = PgSqlDbConnection.CreateConnection(Helpers.TestConstants.TestPgDbConnectionString))
            {
                IDbRepository repository = DbTestHelpers.CreateRepository(DbType.PostgresSQL, Helpers.TestConstants.TestPgDbConnectionString);
                DbScaffold scaffold = new DbScaffold();
                await scaffold.ScaffoldRepository(repository);

                var result = connection.Query($"select * from \"{DbTestHelpers.REPOSITORY_DB_NAME}\"");
                Assert.Empty(result.ToList());
            }
        }

        [Fact]
        public async Task StringColumnsTest()
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            using (NpgsqlConnection connection = PgSqlDbConnection.CreateConnection(Helpers.TestConstants.TestPgDbConnectionString))
            {
                IDbRepository repository = DbTestHelpers.CreateRepository(DbType.PostgresSQL, Helpers.TestConstants.TestPgDbConnectionString);
                repository.AddDefinition("Name", ContentDefinitionType.String);
                repository.AddDefinition("Description", ContentDefinitionType.String);
                repository.AddDefinition("Created", ContentDefinitionType.Date);
                repository.AddDefinition("Updated", ContentDefinitionType.DateTime);

                DbScaffold scaffold = new DbScaffold();
                await scaffold.ScaffoldRepository(repository);

                var result = connection.Query($"select \"Name\", \"Description\", \"Created\", \"Updated\" from \"{DbTestHelpers.REPOSITORY_DB_NAME}\"");
                Assert.Empty(result.ToList());
            }
        }
    }
}
