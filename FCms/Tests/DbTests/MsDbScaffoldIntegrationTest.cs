﻿using System;
using System.Transactions;
using Xunit;
using FCms.Content;
using FCms;
using FCms.DbContent;
using FCms.DbContent.Db;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Linq;
using FCms.Tests.Helpers;

namespace FCmsTests.DbTests
{
    [Trait ("Category", "Integration")]
    [Collection("Sequential")]
    public class MsDbScaffoldIntegrationTest
    {
        public MsDbScaffoldIntegrationTest()
        {
            CMSConfigurator.Configure("./", FCmsTests.Helpers.TestConstants.TestDbConnectionString);
        }

        [Fact]
        public void EmptyRepositoryTest() {
            using (TransactionScope ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            using (SqlConnection connection = MsSqlDbConnection.CreateConnection())
            {
                IDbRepository repository = DbTestHelpers.CreateRepository();
                DbScaffold scaffold = new DbScaffold();
                scaffold.ScaffoldRepository(repository).GetAwaiter().GetResult();

                var result = connection.Query($"select * from {DbTestHelpers.REPOSITORY_DB_NAME}");
                Assert.Empty(result.ToList());
            }
        }

        [Fact]
        public void StringColumnsTest()
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            using (SqlConnection connection = MsSqlDbConnection.CreateConnection())
            {
                IDbRepository repository = DbTestHelpers.CreateRepository();
                repository.AddDefinition("Name", ContentDefinitionType.String);
                repository.AddDefinition("Description", ContentDefinitionType.String);
                repository.AddDefinition("Created", ContentDefinitionType.Date);
                repository.AddDefinition("Updated", ContentDefinitionType.DateTime);

                DbScaffold scaffold = new DbScaffold();
                scaffold.ScaffoldRepository(repository).ConfigureAwait(false).GetAwaiter().GetResult();

                var result = connection.Query($"select Name, Description, Created, Updated from {DbTestHelpers.REPOSITORY_DB_NAME}");
                Assert.Empty(result.ToList());
            }
        }
    }
}
