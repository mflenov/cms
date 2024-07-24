using System;
using System.Linq;
using System.Transactions;
using NUnit.Framework;
using FCms.DbContent;
using FCms.DbContent.Db;
using Microsoft.Data.SqlClient;
using FCms.Tests.Helpers;
using FCms.DbContent.Models;

namespace FCmsTests.DbTests
{
    public class DbSearchDataTest
    {
        public DbSearchDataTest()
        {
            FCms.CMSConfigurator.Configure("./", FCmsTests.Helpers.TestConstants.TestDbConnectionString);
        }

        [Test, Sequential]
        public void FindAllTest()
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            using (SqlConnection connection = MsSqlDbConnection.CreateConnection())
            {
                IDbRepository repository = DbTestHelpers.CreateRepositoryWithSimpleDefinition();
                DbContentStore store = new DbContentStore(repository);

                for (int i = 0; i < 10; i++)
                {
                    object[] columns = { $"Row{i}Name", $"Row{i}Description", DateTime.Today.AddMinutes(i * -10) };
                    store.Add(columns.ToList()).GetAwaiter().GetResult();
                }

                ContentSearchRequest request = new ContentSearchRequest();
                var content = store.GetContent(request).GetAwaiter().GetResult();

                for (int i = 0; i < 10; i++)
                {
                    Assert.That(content.Rows[i].GetStringValue(1), Is.EqualTo( $"Row{i}Name"));
                    Assert.That(content.Rows[i].GetStringValue(2), Is.EqualTo($"Row{i}Description"));
                }
            }
        }

    }
}
