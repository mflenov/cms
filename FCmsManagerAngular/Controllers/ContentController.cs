using System;
using System.Linq;
using FCms.Content;
using FCmsManagerAngular.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FCmsManagerAngular.Controllers
{
    [ApiController]
    public class ContentController
    {
        IConfiguration config;

        public ContentController(IConfiguration config) {
            this.config = config;
        }

        [HttpGet]
        [Route("api/v1/content/{repositoryid}")]
        public ApiResultModel Index(Guid repositoryid, Guid definitionid)
        {
            CmsManager manager = new CmsManager(config["DataLocation"]);
            IRepository repository = manager.GetRepositoryById(repositoryid);

            IContentStore contentStore = manager.GetContentStore(repositoryid);

            ContentListViewModel model = new ContentListViewModel() {
                RepositoryId = repositoryid,
                RepositoryName = repository.Name,
                ContentDefinitions = repository.ContentDefinitions,
                ContentItems = contentStore.Items.Where(m => m.Filters.Count == 0).ToList()
                };


            return new ApiResultModel(ApiResultModel.SUCCESS) {
                Data = model
            };
        }
    }
}
