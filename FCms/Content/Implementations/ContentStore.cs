using System;
using System.Collections.Generic;
using System.Linq;

namespace FCms.Content
{
    class ContentStore : IContentStore
    {
        public Guid RepositoryId { get; set; }

        public List<ContentItem> Items { get; } = new List<ContentItem>();

        public ContentItem GetById(Guid id)
        {
            int index = 0;
            foreach (var item in this.Items)
            {
                if (item.Id == id)
                    return item;
                index++;
            }
            return null;
        }

        public IEnumerable<ContentItem> GetDefaultByDefinitionId(Guid definitionId)
        {
            return Items.Where(m => m.Filters.Count == 0 && m.DefinitionId == definitionId);
        }

        public IEnumerable<ContentItem> GetByDefinitionId(Guid definitionId)
        {
            return Items.Where(m => m.DefinitionId == definitionId);
        }
    }
}
