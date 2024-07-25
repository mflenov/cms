using System;
using System.Linq;
using System.Transactions;
using Xunit;
using FCms.DbContent;
using FCms.DbContent.Db;
using Microsoft.Data.SqlClient;
using FCms.Tests.Helpers;
using FCms.DbContent.Models;
using System.Threading.Tasks;

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
        public async Task FindAllTest()
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            using (SqlConnection connection = MsSqlDbConnection.CreateConnection())
            {
                IDbRepository repository = DbTestHelpers.CreateRepositoryWithSimpleDefinition();
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
