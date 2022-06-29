using System;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

namespace FCms.Content
{
    public class CmsManager : ICmsManager
    {
        const string filename = "cms.json";

        private CmsData data;
        public CmsData Data {
            get { return data; }
        }

        public string Filename {
            get { return CMSConfigurator.ContentBaseFolder + filename; }
        }
        public CmsManager()
        {
            if (File.Exists(CMSConfigurator.ContentBaseFolder + filename))
            {
                data = JsonConvert.DeserializeObject<CmsData>(File.ReadAllText(CMSConfigurator.ContentBaseFolder + filename),
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

        public void AddDbRepository(IDbRepository repository)
        {
            if (repository != null && data.DbRepositories.Select(x => x.Name).Contains(repository.Name))
            {
                throw new Exception($"The repository {repository.Name} already exists");
            }
            data.DbRepositories.Add(repository);
        }

        public void Save()
        {
            System.IO.File.WriteAllText(CMSConfigurator.ContentBaseFolder + filename, JsonConvert.SerializeObject(this.Data, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented
            }));
        }

        public void DeleteRepository(Guid repositoryid)
        {
            data.Repositories.Remove(data.Repositories.Where(m => m.Id == repositoryid).FirstOrDefault());
            data.DbRepositories.Remove(data.DbRepositories.Where(m => m.Id == repositoryid).FirstOrDefault());
        }

        public IRepository GetRepositoryByName(string name)
            => data.Repositories.Where(m => m.Name == name).FirstOrDefault() ?? 
               data.DbRepositories.Where(m => m.Name == name).FirstOrDefault();

        public IRepository GetRepositoryById(Guid id)
            => data.Repositories.Where(m => m.Id == id).FirstOrDefault() ?? 
               data.DbRepositories.Where(m => m.Id == id).FirstOrDefault();

        public int GetIndexById(Guid id) 
            => data.Repositories
              .Select((repos, id) => (repos, id))
              .Single(tuple => tuple.repos.Id == id).id;
    }
}
