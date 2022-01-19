using System.Collections.Generic;
using System.Linq;
using FCms.Content;
using FCmsManagerAngular.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FCmsManagerAngular.Controllers
{
    [ApiController]
    public class ContentController : ControllerBase
    {
        IConfiguration config;

        public ContentController(IConfiguration config) {
            this.config = config;
        }

        [HttpPost]
        [Route("api/v1/content")]
        public ApiResultModel Index(ContentRequestModel request)
        {
            CmsManager manager = new CmsManager(config["DataLocation"]);
            IRepository repository = manager.GetRepositoryById(request.repositoryid);

            IContentStore contentStore = manager.GetContentStore(request.repositoryid);

            IEnumerable<ContentItem> contentItems = request.filters == null ?
                contentStore.Items.Where(m => m.Filters.Count == 0):
                contentStore.Items.Where(m => m.MatchFilters(request.filters));

            PageContentViewModel model = new PageContentViewModel() {
                RepositoryId = request.repositoryid,
                ContentItems = contentItems.Select(m => new ContentViewModel(m))
                };

            return new ApiResultModel(ApiResultModel.SUCCESS) {
                Data = model
            };
        }

        [HttpPut]
        [Route("api/v1/content")]
        public ApiResultModel Save(PageContentViewModel model)
        {
            CmsManager manager = new CmsManager(config["DataLocation"]);
            IRepository repository = manager.GetRepositoryById(model.RepositoryId);
            IContentStore contentStore = manager.GetContentStore(model.RepositoryId);

            var definitionCache = repository.ContentDefinitions.ToDictionary(m => m.DefinitionId, m => m);
            var storeItems = contentStore.Items.Where(m => model.ContentItems.Any(c => c.Id == m.Id)).ToDictionary(m => m.Id, m => m);

            foreach (var item in model.ContentItems)
            {
                var definition = definitionCache[item.DefinitionId];
                if (storeItems.ContainsKey(item.Id))
                {
                    item.MapToModel(storeItems[item.Id], definition);
                }
                else
                {
                    var newitem = FCms.Factory.ContentFactory.CreateContentByType(definition);
                    item.MapToModel(newitem, definition);
                    contentStore.Items.Add(newitem);
                }

            }
            manager.SaveContentStore(contentStore);

            return new ApiResultModel(ApiResultModel.SUCCESS) {
                Data = model
            };
        }
    }
}
