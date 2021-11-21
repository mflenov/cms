using System;
using System.Collections.Generic;

namespace FCms.Content
{
    public interface IContentStore
    {
        Guid RepositoryId { get; set; }

        List<ContentItem> Items { get; }

        ContentItem GetById(Guid id);

        IEnumerable<ContentItem> GetDefaultByDefinitionId(Guid definitionId);

        IEnumerable<ContentItem> GetByDefinitionId(Guid definitionId);
    }
}
