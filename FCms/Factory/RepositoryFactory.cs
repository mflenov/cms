using System;
using FCms.Content;
using FCms.DbContent;

namespace FCms.Factory
{
    public static class RepositoryFactory
    {
        public static IRepository CreateRepository(ContentType contentType, string name)
        {
            var repository = contentType == ContentType.DbContent ? new DbRepository() : new Repository();
            repository.Name = name;
            repository.Id = Guid.NewGuid();
            repository.ContentType = contentType;
            return repository;
        }
    }
}
