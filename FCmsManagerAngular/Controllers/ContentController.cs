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
                ContentItems = contentItems.Select(m => new ContentViewModel(m))
                };

            return new ApiResultModel(ApiResultModel.SUCCESS) {
                Data = model
            };
        }
    }
}
