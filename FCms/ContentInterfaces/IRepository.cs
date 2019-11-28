using System;
using System.Collections.Generic;
using FCms.Content;

namespace FCms.Content
{
    public enum RepositoryStorageType { Json }

    public interface IRepository
    {
        Guid Id { get; set; }

        string Name { get; set; }

        List<IContentDefinition> ContentDefinitions { get; }

        IContentDefinition GetByName(string contentName);
    }
}
