using System;
using System.Linq;
using Xunit;
using FCms.Content;
using FCmsTests.Helpers;
using FCms;

namespace FCmsTests.DbTests
{
    [Trait ("Category", "Integration")]
    public class DbScafoldIntegrationTest
    {
        public DbScafoldIntegrationTest()
        {
            CMSConfigurator.Configure("", "Data Source=.;Initial Catalog=fcms;Integrated Security=true;Trust Server Certificate=true;");
        }

        [Fact]
        public void CreateTableTest() {
            Guid repositoryId1 = Guid.NewGuid();
            ICmsManager manager = new CmsManager();
            manager.Data.Repositories.Add(
                    new Repository()
                    {
                        Id = repositoryId1,
                        Name = "Test 1",
                        ContentType = ContentType.DbContent
                    }
                );
            manager.Save();
            
            DbScaffold scaffold = new DbScaffold();
        }
    }
}
