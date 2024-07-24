using System;
using System.Linq;
using System.Transactions;
using NUnit.Framework;
using FCms.DbContent;
using FCms.DbContent.Db;
using Microsoft.Data.SqlClient;
using FCms.Tests.Helpers;
using Dapper;

namespace FCmsTests.DbTests
{
    public class DbContentTest
    {
        public DbContentTest()
        {
            FCms.CMSConfigurator.Configure("./", FCmsTests.Helpers.TestConstants.TestDbConnectionString);
        }

        [Test, Sequential]
        public void AddRowTest()
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            using (SqlConnection connection = MsSqlDbConnection.CreateConnection())
            {
                IDbRepository repository = DbTestHelpers.CreateRepositoryWithSimpleDefinition();
                DbContentStore store = new DbContentStore(repository);

                object[] columns = { "Name", "Description", DateTime.Today };
                store.Add(columns.ToList()).GetAwaiter().GetResult();
                var result = connection.Query($"select * from {DbTestHelpers.REPOSITORY_DB_NAME}").First();
                Assert.That(result.Name, Is.EqualTo("Name"));
                Assert.That(result.Description, Is.EqualTo("Description"));
            }
        }
    }
}
