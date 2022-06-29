using System;
using FCms.Content;

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
            manager.Save();
            return manager.Data.DbRepositories[0] as IDbRepository;
        }
    }
}
