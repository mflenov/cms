using System.Collections.Generic;
using FCms.Content;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace FCms
{
    public class ContentEngine
    {
        readonly ICmsManager manager;
        IRepository repo;
        IContentStore contentStore;
        const string MANAGER_CACHE_KEY = "FCMS_MANAGER";
        const string REPO_CACHE_KEY = "FCMS_REPO";
        const string REPO_CACHE_STORE = "FCMS_STORE";

        public ContentEngine(string repositoryName)
        {
            manager = (ICmsManager)Tools.Cacher.Get(MANAGER_CACHE_KEY);
            if (manager == null)
            {
                manager = new CmsManager(CMSConfigurator.ContentBaseFolder);
                Tools.Cacher.Set(MANAGER_CACHE_KEY, manager, manager.Filename);
            }
            this.RepositoryName = repositoryName;
        }

        public string RepositoryName
        {
            get { return repo.Name; }
            private set {
                GetRepository(value);
                LoadContentStore(repo);
            }
        }

        private void GetRepository(string value)
        {
            string key = REPO_CACHE_KEY + "_" + value;
            if (Tools.Cacher.Contains(key))
            {
                repo = Tools.Cacher.Get(key) as IRepository;
                return;
            }
            object cacherepo = Tools.Cacher.Get("key");
            if (cacherepo != null)
            {
                repo = cacherepo as IRepository;
                return;
            }
            repo = manager.GetRepositoryByName(value);
            Tools.Cacher.Set(key, repo);
        }

        private void LoadContentStore(IRepository repo)
        {
            if (Tools.Cacher.Contains(REPO_CACHE_STORE + repo.Id))
            {
                contentStore = Tools.Cacher.Get(REPO_CACHE_STORE + repo.Id) as IContentStore;
            }
            else
            {
                contentStore = manager.GetContentStore(repo.Id);
                Tools.Cacher.Set(REPO_CACHE_STORE + repo.Id, contentStore, manager.GetContentStoreFilename(repo.Id));
            }
        }

        public string GetContentString(string contentName)
        {            
            IContentDefinition definition = repo.GetByName(contentName);
            ContentItem contentItem = contentStore.GetDefaultByDefinitionId(definition.DefinitionId).FirstOrDefault();
            if (contentItem == null)
            {
                return "";
            }

            return contentItem == null ? "" : contentItem.GetValue().ToString();
        }

        public IEnumerable<string> GetContentStrings(string contentName)
        {
            IContentDefinition definition = repo.GetByName(contentName);
            return contentStore.GetDefaultByDefinitionId(definition.DefinitionId).Select(m => m.GetValue().ToString());
        }

        public IEnumerable<ContentItem> GetContentItems(string contentName, Dictionary<string, object>  filters)
        {
            return GetContents<ContentItem>(contentName, filters);
        }

        public IEnumerable<ContentFolderItem> GetFolderItems(string contentName, Dictionary<string, object>  filters)
        {
            return GetContents<ContentFolderItem>(contentName, filters);
        }

        public IContent GetFolderItem(ContentFolderItem folder, string itemname)
        {
            if (folder == null)
                return null;
            var folderDefinition = repo.ContentDefinitions.Where(m => m.DefinitionId == folder.DefinitionId).FirstOrDefault();
            if (folderDefinition.GetDefinitionType() != ContentDefinitionType.Folder)
            {
                return null;
            }
            return folder.GetItem((folderDefinition as FolderContentDefinition).Definitions.Where(m => m.Name == itemname).FirstOrDefault()?.DefinitionId);
        }

        public IEnumerable<T> GetContents<T>(string contentName, Dictionary<string, object> filters) where T: ContentItem
        {
            if (filters == null)
            {
                filters = new Dictionary<string, object> { };
            }

            IContentDefinition definition = repo.GetByName(contentName);
            List<ContentItem> contentitems = contentStore.GetByDefinitionId(definition.DefinitionId).ToList();

            foreach (T contentitem in contentitems)
            {
                if (contentitem.ValidateFilters(filters))
                {
                    yield return contentitem;
                }
            }

        }
    }
}
