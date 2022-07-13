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

namespace FCmsTests.DbTests
{
    [Trait("Category", "Integration")]
    public class DbSearchDataTest
    {
        public DbSearchDataTest()
        {
            FCms.CMSConfigurator.Configure("./", FCmsTests.Helpers.TestConstants.TestDbConnectionString);
        }

        [Fact]
        public void StringColumnsTest()
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            using (SqlConnection connection = MsSqlDbConnection.CreateConnection())
            {
                IDbRepository repository = DbTestHelpers.CreateRepositoryWithSimpleDefinition();
                DbContentStore store = new DbContentStore(repository);

                for (int i = 0; i < 10; i++)
                {
                    object[] columns = { $"Row{i}Name", "Row{i}Description", DateTime.Today.AddMinutes(i * -10) };
                    store.Add(columns.ToList()).GetAwaiter().GetResult();
                }

                //Assert.Equal("Name", result.Name);
                //Assert.Equal("Description", result.Description);
            }
        }

    }
}
