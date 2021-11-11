using System.Collections.Generic;
using FCms.Content;
using System.Linq;

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
                manager = new CmsManager();
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
            if (HttpContext.RequestCache.ContainsKey(key))
            {
                repo = HttpContext.RequestCache[key] as IRepository;
                return;
            }
            object cacherepo = Tools.Cacher.Get("key");
            if (cacherepo != null)
            {
                repo = cacherepo as IRepository;
                return;
            }
            repo = manager.GetRepositoryByName(value);
            HttpContext.RequestCache[key] = repo;
        }

        private void LoadContentStore(IRepository repo)
        {
            if (HttpContext.RequestCache.ContainsKey(REPO_CACHE_STORE + repo.Id))
            {
                contentStore = HttpContext.RequestCache[REPO_CACHE_STORE + repo.Id] as IContentStore;
            }
            else
            {
                contentStore = manager.GetContentStore(repo.Id);
                HttpContext.RequestCache[REPO_CACHE_STORE + repo.Id] = contentStore;
                Tools.Cacher.Set(MANAGER_CACHE_KEY, manager, CmsManager.GetContentStoreFilename(repo.Id));
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

        public IEnumerable<ContentItem> GetContentItems(string contentName, object filters)
        {
            return GetContents<ContentItem>(contentName, filters);
        }

        public IEnumerable<ContentFolderItem> GetFolderItems(string contentName, object filters)
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

        public IEnumerable<T> GetContents<T>(string contentName, object filters) where T: ContentItem
        {
            if (filters == null)
            {
                filters = new { };
            }

            IContentDefinition definition = repo.GetByName(contentName);
            List<ContentItem> contentitems = contentStore.GetByDefinitionId(definition.DefinitionId).ToList();

            var filterProperties = filters.GetType().GetProperties().ToLookup(m => m.Name);

            foreach (T contentitem in contentitems)
            {
                if (contentitem.ValidateFilters(filterProperties, filters))
                {
                    yield return contentitem;
                }
            }

        }
    }
}
