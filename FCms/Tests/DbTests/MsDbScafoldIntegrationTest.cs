using System;
using System.Transactions;
using Xunit;
using FCms.Content;
using FCms;
using FCms.DbContent.Implementations;
using FCms.DbContent.Db;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Linq;

namespace FCmsTests.DbTests
{
    [Trait ("Category", "Integration")]
    public class MsDbScafoldIntegrationTest
    {
        const string REPOSITORY_NAME = "Test 1";
        const string REPOSITORY_DB_NAME = "Test1";

        public MsDbScafoldIntegrationTest()
        {
            CMSConfigurator.Configure("", "Data Source=.;Initial Catalog=fcms;Integrated Security=true;Trust Server Certificate=true;");
        }

        [Fact]
        public void CreateTableTest() {
            using (TransactionScope ts = new TransactionScope())
            using (SqlConnection connection = MsSqlDbConnection.CreateConnection())
            {
                IRepository repository = CreateRepository();
                DbScaffold scaffold = new DbScaffold();
                scaffold.ScaffoldRepository(repository);

                var result = connection.Query<FCms.DbContent.Models.DbTableModel>("select Name from sys.tables where Name = @n", new { n = REPOSITORY_DB_NAME });
                Assert.Single(result.ToList());
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
