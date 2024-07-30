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
    [Trait ("Category", "Integration")]
    [Collection("Sequential")]
    public class PgDbScaffoldIntegrationTest
    {
        public PgDbScaffoldIntegrationTest()
        {
            CMSConfigurator.Configure("./", FCmsTests.Helpers.TestConstants.TestPgDbConnectionString);
        }

        [Fact]
        public async Task EmptyRepositoryTest() {
            using (TransactionScope ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            using (NpgsqlConnection connection = PgSqlDbConnection.CreateConnection())
            {
                IDbRepository repository = DbTestHelpers.CreateRepository();
                DbScaffold scaffold = new DbScaffold(DbType.PostgresSQL);
                await scaffold.ScaffoldRepository(repository);

                var result = connection.Query($"select * from \"{DbTestHelpers.REPOSITORY_DB_NAME}\"");
                Assert.Empty(result.ToList());
            }
        }

        [Fact]
        public async Task StringColumnsTest()
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            using (NpgsqlConnection connection = PgSqlDbConnection.CreateConnection())
            {
                IDbRepository repository = DbTestHelpers.CreateRepository();
                repository.AddDefinition("Name", ContentDefinitionType.String);
                repository.AddDefinition("Description", ContentDefinitionType.String);
                repository.AddDefinition("Created", ContentDefinitionType.Date);
                repository.AddDefinition("Updated", ContentDefinitionType.DateTime);

                DbScaffold scaffold = new DbScaffold(DbType.PostgresSQL);
                await scaffold.ScaffoldRepository(repository);

                var result = connection.Query($"select \"Name\", \"Description\", \"Created\", \"Updated\" from \"{DbTestHelpers.REPOSITORY_DB_NAME}\"");
                Assert.Empty(result.ToList());
            }
        }
    }
}
