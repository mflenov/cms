using System;
using FCms.Content;
using FCms.DbContent;

namespace FCms.Tests.Helpers
{
    static class DbTestHelpers
    {
        public const string REPOSITORY_NAME = "Test 1";
        public const string REPOSITORY_DB_NAME = "Test1";

        public static IDbRepository CreateRepository()
        {
            Guid repositoryId1 = Guid.NewGuid();
            ICmsManager manager = new CmsManager();
            manager.Data.DbRepositories.Add(
                    new DbRepository()
                    {
                        Id = repositoryId1,
                        Name = REPOSITORY_NAME,
                        ContentType = ContentType.DbContent
                    }
                );
            return manager.Data.DbRepositories[0] as IDbRepository;
        }

        public static IDbRepository CreateRepositoryWithSimpleDefinition()
        {
            IDbRepository repository = DbTestHelpers.CreateRepository();

            repository.AddDefinition("Name", ContentDefinitionType.String);
            repository.AddDefinition("Description", ContentDefinitionType.String);
            repository.AddDefinition("DateTime", ContentDefinitionType.DateTime);

            DbScaffold scaffold = new DbScaffold();
            scaffold.ScaffoldRepository(repository);

            return repository;
        }
    }
}
