using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FCms.Content
{
    public class ContentStore : IContentStore
    {
        private ContentStore()
        {

        }

        private ContentStore (Guid repositoryid) {
            this.RepositoryId = repositoryid;
        }


        public string GetContentStoreFilename()
        {
            return CMSConfigurator.ContentBaseFolder + RepositoryId.ToString() + ".json";
        }

        public static IContentStore Load(Guid repositoryid)
        {
            string filename = CMSConfigurator.ContentBaseFolder + repositoryid.ToString() + ".json";
            if (File.Exists(filename))
            {
                IContentStore store = JsonConvert.DeserializeObject<ContentStore>(File.ReadAllText(filename),
                    new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto, Formatting = Formatting.Indented, ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor });
                (store as ContentStore).MapFilters();
                return store;
            }
            return new ContentStore(repositoryid) { };
        }

        public void MapFilters()
        {
            CmsManager manager = new CmsManager();
            var filterDefinition = manager.Data.Filters.ToLookup(m => m.Id);
            foreach (var filter in Items.SelectMany(m => m.Filters))
            {
                filter.Filter = filterDefinition[filter.FilterDefinitionId].FirstOrDefault();
            }
        }

        public void Save()
        {
            System.IO.File.WriteAllText(GetContentStoreFilename(), JsonConvert.SerializeObject(this, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented
            }));
        }

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
