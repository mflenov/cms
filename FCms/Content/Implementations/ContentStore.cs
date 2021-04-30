﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FCms.Content
{
    class ContentStore : IContentStore
    {
        public Guid RepositoryId { get; set; }

        public List<ContentItem> Items { get; } = new List<ContentItem>();

        public void Save()
        {
            System.IO.File.WriteAllText(RepositoryId.ToString() + ".json", JsonConvert.SerializeObject(this, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented

            }));
        }

        public int GetIndexById(Guid id)
        {
            int index = 0;
            foreach (var item in this.Items)
            {
                if (item.Id == id)
                    return index;
                index++;
            }
            return -1;
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
