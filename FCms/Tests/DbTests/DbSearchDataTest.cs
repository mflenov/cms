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
                IDbRepository repository = DbTestHelpers.CreateRepositoryWithSimpleDefinition();

                DbContentStore store = new DbContentStore(repository);
            }
        }

    }
}
