using System;
using System.Collections.Generic;

namespace FCms.Content
{
    public enum RepositoryStorageType { Json }

    public enum ReporitoryType { Page, Content }

    public enum ReporitoryTypeTemplate { EmptyPage, SimplePage, Content }

    public interface IRepository
    {
        Guid Id { get; set; }

        string Name { get; set; }

        ReporitoryType ReporitoryType { get; set; }

        List<IContentDefinition> ContentDefinitions { get; }

        IContentDefinition GetByName(string contentName);

        void DeleteDefinition(Guid id);

        void AddDefinition(string name, IContentDefinition.DefinitionType type);
    }
}
