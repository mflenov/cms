using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

namespace FCms.Content
{
    public class CmsManager : ICmsManager
    {
        static string filename = "./cms.json";
        List<IRepository> repositories = new List<IRepository>();
        public List<IRepository> Repositories {
            get {
                return this.repositories;
            }
        }

        List<IFilter> filters = new List<IFilter>();
        public List<IFilter> Filters {
            get {
                return filters;
            }
        }

        private CmsManager()
        {

        }

        public void Save()
        {
            System.IO.File.WriteAllText(filename, JsonConvert.SerializeObject(this, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            }));
        }

        public static ICmsManager Load()
        {
            if (File.Exists(filename))
            {
                return JsonConvert.DeserializeObject<CmsManager>(File.ReadAllText(filename),
                    new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });
            }
            return new CmsManager();
        }

        public IRepository GetRepositoryByName(string name)
        {
            return Repositories.Where(m => m.Name == name).FirstOrDefault();
        }
        public IRepository GetRepositoryById(Guid id)
        {
            return Repositories.Where(m => m.Id == id).FirstOrDefault();
        }

        public int GetIndexById(Guid id) {
            int index = 0;
            foreach (var repo in this.Repositories)
            {
                if (repo.Id == id)
                    return index;
                index++;
            }
            return -1;
        }

        void MapFilters(IContentStore store)
        {
            var filterDefinition = this.Filters.ToLookup(m => m.Id);
            foreach (var filter in store.Items.SelectMany(m => m.Filters))
            {
                filter.Filter = filterDefinition[filter.FilterDefinitionId].FirstOrDefault();
            }
        }

        public IContentStore GetContentStore(Guid repositoryid)
        {
            string filename = repositoryid.ToString() + ".json";
            if (File.Exists(filename))
            {
                IContentStore store = JsonConvert.DeserializeObject<ContentStore>(File.ReadAllText(filename),
                    new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });
                MapFilters(store);
                return store;
            }
            return new ContentStore() {
                RepositoryId = repositoryid
            };
        }
    }
}
