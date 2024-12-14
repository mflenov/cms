using System;
using System.Linq;
using System.Transactions;
using Xunit;
using FCms.DbContent;
using FCms.DbContent.Db;
using Microsoft.Data.SqlClient;
using FCms.Tests.Helpers;
using Dapper;
using System.Threading.Tasks;

namespace FCmsTests.DbTests
{
    [Trait("Category", DbHelpersTest.TEST_CATEGORY_INTEGRATION)]
    [Trait("Category", DbHelpersTest.TEST_CATEGORY_MSSQL)]
    [Collection("Sequential")]
    public class MsDbContentTest: IDisposable
    {
        public MsDbContentTest()
        {
            FCms.CMSConfigurator.Configure("./");
        }

        public void Dispose()
        {
            FCmsTests.Helpers.TestTools.DeleteCmsFile();
            FCms.Tools.Cacher.Clear();
        }

        [Fact]
        public async Task AddRowTest()
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            using (SqlConnection connection = MsSqlDbConnection.CreateConnection(FCmsTests.Helpers.TestConstants.TestMsDbConnectionString))
            {
                IDbRepository repository = DbTestHelpers.CreateRepositoryWithSimpleDefinition(DbType.Microsoft, FCmsTests.Helpers.TestConstants.TestMsDbConnectionString);
                DbContentStore store = new DbContentStore(repository);

                object[] columns = { "Name", "Description", DateTime.Today };
                await store.Add(columns.ToList());
                var result = connection.Query($"select * from {DbTestHelpers.REPOSITORY_DB_NAME}").First();
                Assert.Equal("Name", result.Name);
                Assert.Equal("Description", result.Description);
            }
        }
    }
}
