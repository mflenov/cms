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

        public string Filename {
            get { return filename; }
        }

        List<IFilter> filters = new List<IFilter>();
        public List<IFilter> Filters {
            get {
                return filters;
            }
        }

        public void AddRepository(IRepository repository)
        {
            if (repository != null && repositories.Select(x => x.Name).Contains(repository.Name))
            {
                throw new Exception($"The repository {repository.Name} already exists");
            }
            repositories.Add(repository);
        }

        private CmsManager()
        {

        }

        public void Save()
        {
            System.IO.File.WriteAllText(filename, JsonConvert.SerializeObject(this, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented
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

        public void DeleteRepository(Guid repositoryid)
        {
            var item = Repositories.Where(m => m.Id == repositoryid).FirstOrDefault();
            if (item != null)
            {
                Repositories.Remove(item);
            }
        }

        public IRepository GetRepositoryByName(string name)
        {
            return Repositories.Where(m => m.Name == name).FirstOrDefault();
        }
        public IRepository GetRepositoryById(Guid id)
        {
            return Repositories.Where(m => m.Id == id).FirstOrDefault();
        }

        public int GetIndexById(Guid id) 
            => repositories
              .Select((repos, id) => (repos, id))
              .Single(tuple => tuple.repos.Id == id).id;


        void MapFilters(IContentStore store)
        {
            var filterDefinition = this.Filters.ToLookup(m => m.Id);
            foreach (var filter in store.Items.SelectMany(m => m.Filters))
            {
                filter.Filter = filterDefinition[filter.FilterDefinitionId].FirstOrDefault();
            }
        }

        public static string GetContentStoreFilename(Guid repositoryid)
        {
            return repositoryid.ToString() + ".json";
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
    }
}
