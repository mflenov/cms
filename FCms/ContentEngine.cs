using System.Collections.Generic;
using FCms.Content;
using System.Linq;

namespace FCms
{
    public class ContentEngine
    {
        readonly ICmsManager manager = CmsManager.Load();
        IRepository repo;
        IContentStore contentStore;
        const string REPO_CACHE_KEY = "FCMS_MANAGER";
        const string REPO_CACHE_STORE = "FCMS_STORE";

        public ContentEngine(string repositoryName)
        {
            this.RepositoryName = repositoryName;
        }

        public string RepositoryName
        {
            get { return repo.Name; }
            private set {
                if (HttpContext.RequestCache.ContainsKey(REPO_CACHE_KEY) )
                {
                    repo = HttpContext.RequestCache[REPO_CACHE_KEY] as IRepository;
                }
                else
                {
                    repo = manager.GetRepositoryByName(value);
                    HttpContext.RequestCache[REPO_CACHE_KEY] = repo;
                }

                if (HttpContext.RequestCache.ContainsKey(REPO_CACHE_STORE + repo.Id))
                {
                    contentStore = HttpContext.RequestCache[REPO_CACHE_STORE + repo.Id] as IContentStore;
                }
                else
                {
                    contentStore = manager.GetContentStore(repo.Id);
                    HttpContext.RequestCache[REPO_CACHE_STORE + repo.Id] = contentStore;
                }
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

        public IContent? GetFolderItem(ContentFolderItem folder, string itemname)
        {
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
