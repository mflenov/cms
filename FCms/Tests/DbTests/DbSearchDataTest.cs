using System;
using System.Transactions;
using Xunit;
using FCms.Content;
using FCms.DbContent;
using FCms.DbContent.Db;
using Microsoft.Data.SqlClient;


namespace FCms.Tests.DbTests
{
    [Trait("Category", "Integration")]
    public class DbSearchDataTest
    {
        const string REPOSITORY_NAME = "Test 1";

        public DbSearchDataTest()
        {
            CMSConfigurator.Configure("./", "Data Source=.;Initial Catalog=fcms;Integrated Security=true;Trust Server Certificate=true;");
        }

        [Fact]
        public void StringColumnsTest()
        {
            using (TransactionScope ts = new TransactionScope())
            using (SqlConnection connection = MsSqlDbConnection.CreateConnection())
            {
                
            }
        }

        private IRepository CreateRepository()
        {
            Guid repositoryId1 = Guid.NewGuid();

            IRepository repository = CreateRepository();

            ICmsManager manager = new CmsManager();
            manager.Data.Repositories.Add(
                    new Repository()
                    {
                        Id = repositoryId1,
                        Name = REPOSITORY_NAME,
                        ContentType = ContentType.DbContent
                    }
                );
            repository.AddDefinition("Name", ContentDefinitionType.String);
            repository.AddDefinition("Description", ContentDefinitionType.String);

            DbScaffold scaffold = new DbScaffold();
            scaffold.ScaffoldRepository(repository);

            manager.Save();
            return manager.Data.Repositories[0];
        }
    }
}
