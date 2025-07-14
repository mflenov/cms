using System;
using System.Collections.Generic;

namespace FCms.Content
{
    public enum ContentTemplate { EmptyPage, SimplePage }

    public enum ContentType { Page, DbContent, Content }

    public enum RepositoryStorageType { Json }

    public interface IRepository
    {
        Guid Id { get; set; }

        string Name { get; set; }

        ContentType ContentType { get; set; }

        List<IContentDefinition> ContentDefinitions { get; set; }

        IContentDefinition GetByName(string contentName);

        void DeleteDefinition(Guid id);

        void AddDefinition(string name, ContentDefinitionType type);
    }
}
