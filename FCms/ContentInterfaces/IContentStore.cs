using System;
using System.Collections.Generic;
using System.Text;

namespace FCms.Content
{
    public interface IContentStore
    {
        Guid RepositoryId { get; set; }

        List<ContentItem> Items { get; }

        void Save();

        int GetIndexById(Guid id);

        IEnumerable<ContentItem> GetDefaultByDefinitionId(Guid definitionId);

        IEnumerable<ContentItem> GetByDefinitionId(Guid definitionId);
    }
}
