using System;
using System.Transactions;
using Xunit;
using FCms.Content;
using FCms.DbContent;
using FCms.DbContent.Db;
using Microsoft.Data.SqlClient;
using FCms.Tests.Helpers;

namespace FCms.Tests.DbTests
{
    [Trait("Category", "Integration")]
    public class DbSearchDataTest
    {
        const string REPOSITORY_NAME = "Test 1";

        public DbSearchDataTest()
        {
            CMSConfigurator.Configure("./", FCmsTests.Helpers.TestConstants.TestDbConnectionString);
        }

        [Fact]
        public void StringColumnsTest()
        {
            using (TransactionScope ts = new TransactionScope())
            using (SqlConnection connection = MsSqlDbConnection.CreateConnection())
            {
                IDbRepository repository = CreateTestRepository();

                DbContentStore store = new DbContentStore(repository);
            }
        }

        #region test helpers

        private IDbRepository CreateTestRepository()
        {
            IDbRepository repository = DbTestHelpers.CreateRepository();

            repository.AddDefinition("Name", ContentDefinitionType.String);
            repository.AddDefinition("Description", ContentDefinitionType.String);
            repository.AddDefinition("DateTime", ContentDefinitionType.DateTime);

            DbScaffold scaffold = new DbScaffold();
            scaffold.ScaffoldRepository(repository);

            return repository;
        }

        #endregion
    }
}
