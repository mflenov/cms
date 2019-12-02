using System.Collections.Generic;
using FCms.Content;
using System.Linq;

namespace FCms
{
    public class ContentEngine
    {
        readonly ICmsManager manager = CmsManager.Load();
        readonly IRepository repo;
        readonly IContentStore contentStore;

        public ContentEngine(string repositoryName)
        {
            repo = manager.GetRepositoryByName(repositoryName);
            contentStore = manager.GetContentStore(repo.Id);
        }

        public string GetContentString(string contentName)
        {            
            IContentDefinition definition = repo.GetByName(contentName);
            ContentItem contentItem = contentStore.GetDefaultByDefinitionId(definition.DefinitionId).FirstOrDefault();
            return contentItem == null ? "" : contentItem.GetStringValue();
        }

        public IEnumerable<string> GetContentStrings(string contentName)
        {
            IContentDefinition definition = repo.GetByName(contentName);
            return contentStore.GetDefaultByDefinitionId(definition.DefinitionId).Select(m => m.GetStringValue());
        }

        public IEnumerable<ContentItem> GetContents(string contentName, object filters)
        {
            if (filters == null)
            {
                filters = new { };
            }

            IContentDefinition definition = repo.GetByName(contentName);
            List<ContentItem> contentitems = contentStore.GetByDefinitionId(definition.DefinitionId).ToList();

            var filterProperties = filters.GetType().GetProperties().ToLookup(m => m.Name);

            foreach (ContentItem contentitem in contentitems)
            {
                if (contentitem.ValidateFilters(filterProperties, filters))
                {
                    yield return contentitem;
                }
            }

        }
    }
}
