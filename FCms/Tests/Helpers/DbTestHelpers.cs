using System;
using FCms.Content;
using FCms.DbContent;

namespace FCms.Tests.Helpers
{
    static class DbTestHelpers
    {
        public const string REPOSITORY_NAME = "Test 1";
        public const string REPOSITORY_DB_NAME = "Test1";

        public static IDbRepository CreateRepository(DbType dbType)
        {
            Guid repositoryId1 = Guid.NewGuid();
            ICmsManager manager = new CmsManager();
            manager.Data.DbRepositories.Add(
                    new DbRepository()
                    {
                        Id = repositoryId1,
                        DatabaseType = dbType,
                        Name = REPOSITORY_NAME,
                        ContentType = ContentType.DbContent
                    }
                );
            return manager.Data.DbRepositories[0] as IDbRepository;
        }

        public static IDbRepository CreateRepositoryWithSimpleDefinition(DbType dbType)
        {
            IDbRepository repository = DbTestHelpers.CreateRepository(dbType);

            repository.AddDefinition("Name", ContentDefinitionType.String);
            repository.AddDefinition("Description", ContentDefinitionType.String);
            repository.AddDefinition("DateTime", ContentDefinitionType.DateTime);

            DbScaffold scaffold = new DbScaffold();
            scaffold.ScaffoldRepository(repository).GetAwaiter().GetResult();

            return repository;
        }
    }
}
