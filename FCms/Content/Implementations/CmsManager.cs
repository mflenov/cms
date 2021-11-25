using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

namespace FCms.Content
{
    public class CmsManager : ICmsManager
    {
        const string filename = "cms.json";
        string path = "./"; 

        private CmsData data;
        public CmsData Data {
            get { return data; }
        }

        public string Filename {
            get { return this.path + filename; }
        }

        public CmsManager(): this("./")
        {

        }

        public CmsManager(string location)
        {
            path = location;
            if (File.Exists(location + filename))
            {
                data = JsonConvert.DeserializeObject<CmsData>(File.ReadAllText(path + filename),
                    new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });
            }
            else {
                data = new CmsData();
            }
        }


        public void AddRepository(IRepository repository)
        {
            if (repository != null && data.Repositories.Select(x => x.Name).Contains(repository.Name))
            {
                throw new Exception($"The repository {repository.Name} already exists");
            }
            data.Repositories.Add(repository);
        }

        public void Save()
        {
            System.IO.File.WriteAllText(path + filename, JsonConvert.SerializeObject(this.Data, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented
            }));
        }

        public void DeleteRepository(Guid repositoryid)
        {
            var item = data.Repositories.Where(m => m.Id == repositoryid).FirstOrDefault();
            if (item != null)
            {
                data.Repositories.Remove(item);
            }
        }

        public IRepository GetRepositoryByName(string name)
        {
            return data.Repositories.Where(m => m.Name == name).FirstOrDefault();
        }
        public IRepository GetRepositoryById(Guid id)
        {
            return data.Repositories.Where(m => m.Id == id).FirstOrDefault();
        }

        public int GetIndexById(Guid id) 
            => data.Repositories
              .Select((repos, id) => (repos, id))
              .Single(tuple => tuple.repos.Id == id).id;


        void MapFilters(IContentStore store)
        {
            var filterDefinition = data.Filters.ToLookup(m => m.Id);
            foreach (var filter in store.Items.SelectMany(m => m.Filters))
            {
                filter.Filter = filterDefinition[filter.FilterDefinitionId].FirstOrDefault();
            }
        }

        public string GetContentStoreFilename(Guid repositoryid)
        {
            return path + repositoryid.ToString() + ".json";
        }

        public IContentStore GetContentStore(Guid repositoryid)
        {
            string filename = GetContentStoreFilename(repositoryid);
            if (File.Exists(filename))
            {
                IContentStore store = JsonConvert.DeserializeObject<ContentStore>(File.ReadAllText(filename),
                    new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto, Formatting = Formatting.Indented });
                MapFilters(store);
                return store;
            }
            return new ContentStore() {
                RepositoryId = repositoryid
            };
        }

        public void SaveContentStore(IContentStore store)
        {
            if (store == null) {
                return;
            }
            System.IO.File.WriteAllText(path + store.RepositoryId.ToString() + ".json", JsonConvert.SerializeObject(store, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented
            }));
        }

    }
}
