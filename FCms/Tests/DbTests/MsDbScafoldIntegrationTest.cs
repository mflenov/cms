using System;
using System.Transactions;
using Xunit;
using FCms.Content;
using FCms;
using FCms.DbContent;
using FCms.DbContent.Db;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Linq;

namespace FCmsTests.DbTests
{
    [Trait ("Category", "Integration")]
    [Collection("Sequential")]
    public class MsDbScafoldIntegrationTest
    {
        const string REPOSITORY_NAME = "Test 1";
        const string REPOSITORY_DB_NAME = "Test1";

        public MsDbScafoldIntegrationTest()
        {
            CMSConfigurator.Configure("./", FCmsTests.Helpers.Constants.TestDbConnectionString);
        }

        [Fact]
        public void EmptyRepositoryTest() {
            using (TransactionScope ts = new TransactionScope())
            using (SqlConnection connection = MsSqlDbConnection.CreateConnection())
            {
                IRepository repository = CreateRepository();
                DbScaffold scaffold = new DbScaffold();
                scaffold.ScaffoldRepository(repository);

                var result = connection.Query($"select * from {REPOSITORY_DB_NAME}");
                Assert.Empty(result.ToList());
            }
        }

        [Fact]
        public void StringColumnsTest()
        {
            using (TransactionScope ts = new TransactionScope())
            using (SqlConnection connection = MsSqlDbConnection.CreateConnection())
            {
                IRepository repository = CreateRepository();
                repository.AddDefinition("Name", ContentDefinitionType.String);
                repository.AddDefinition("Description", ContentDefinitionType.String);
                repository.AddDefinition("Created", ContentDefinitionType.Date);
                repository.AddDefinition("Updated", ContentDefinitionType.DateTime);

                DbScaffold scaffold = new DbScaffold();
                scaffold.ScaffoldRepository(repository);

                var result = connection.Query($"select Name, Description, Created, Updated from {REPOSITORY_DB_NAME}");
                Assert.Empty(result.ToList());
            }
        }

        private IRepository CreateRepository()
        {
            Guid repositoryId1 = Guid.NewGuid();
            ICmsManager manager = new CmsManager();
            manager.Data.Repositories.Add(
                    new Repository()
                    {
                        Id = repositoryId1,
                        Name = REPOSITORY_NAME,
                        ContentType = ContentType.DbContent
                    }
                );
            manager.Save();
            return manager.Data.Repositories[0];
        }
    }
}
