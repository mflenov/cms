﻿using System;
using FCms.Content;
using FCms.DbContent;

namespace FCms.Tests.Helpers
{
    static class DbTestHelpers
    {
        public const string REPOSITORY_NAME = "Test 1";
        public const string REPOSITORY_DB_NAME = "Test1";

        public static IDbRepository CreateRepository(DbType dbType, string ConnectionString)
        {
            Guid repositoryId1 = Guid.NewGuid();
            ICmsManager manager = CmsManager.GetInstance();

            Guid connectionId = Guid.NewGuid();            
            manager.Data.DbConnections.Add(
                new FCms.DbContent.DbConnection {
                    Id = connectionId,
                    DatabaseType = dbType,
                    ConnectionString = ConnectionString
                }
            );

            manager.Data.DbRepositories.Add(
                    new DbRepository()
                    {
                        Id = repositoryId1,
                        DatabaseConnectionId = connectionId,
                        Name = REPOSITORY_NAME,
                        ContentType = ContentType.DbContent
                    }
                );
            return manager.Data.DbRepositories[0] as IDbRepository;
        }

        public static IDbRepository CreateRepositoryWithSimpleDefinition(DbType dbType, string connectionString)
        {
            IDbRepository repository = DbTestHelpers.CreateRepository(dbType, connectionString);

            repository.AddDefinition("Name", ContentDefinitionType.String);
            repository.AddDefinition("Description", ContentDefinitionType.String);
            repository.AddDefinition("DateTime", ContentDefinitionType.DateTime);

            DbScaffold scaffold = new DbScaffold();
            scaffold.ScaffoldRepository(repository).GetAwaiter().GetResult();

            return repository;
        }
    }
}
