using System.Transactions;
using NUnit.Framework;
using FCms.Content;
using FCms;
using FCms.DbContent;
using FCms.DbContent.Db;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Linq;
using FCms.Tests.Helpers;

namespace FCmsTests.DbTests
{
    public class MsDbScaffoldIntegrationTest
    {
        public MsDbScaffoldIntegrationTest()
        {
            CMSConfigurator.Configure("./", FCmsTests.Helpers.TestConstants.TestDbConnectionString);
        }

        [Test, Sequential]
        public void EmptyRepositoryTest() {
            using (TransactionScope ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            using (SqlConnection connection = MsSqlDbConnection.CreateConnection())
            {
                IDbRepository repository = DbTestHelpers.CreateRepository();
                DbScaffold scaffold = new DbScaffold();
                scaffold.ScaffoldRepository(repository).GetAwaiter().GetResult();

                var result = connection.Query($"select * from {DbTestHelpers.REPOSITORY_DB_NAME}");
                Assert.That(result.ToList().Count, Is.EqualTo(0));
            }
        }

        [Test, Sequential]
        public void StringColumnsTest()
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            using (SqlConnection connection = MsSqlDbConnection.CreateConnection())
            {
                IDbRepository repository = DbTestHelpers.CreateRepository();
                repository.AddDefinition("Name", ContentDefinitionType.String);
                repository.AddDefinition("Description", ContentDefinitionType.String);
                repository.AddDefinition("Created", ContentDefinitionType.Date);
                repository.AddDefinition("Updated", ContentDefinitionType.DateTime);

                DbScaffold scaffold = new DbScaffold();
                scaffold.ScaffoldRepository(repository).ConfigureAwait(false).GetAwaiter().GetResult();

                var result = connection.Query($"select Name, Description, Created, Updated from {DbTestHelpers.REPOSITORY_DB_NAME}");
                Assert.That(result.ToList().Count, Is.EqualTo(0));
            }
        }
    }
}
