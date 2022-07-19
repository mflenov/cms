using System;
using Microsoft.AspNetCore.Mvc;
using FCms.Content;
using FCms.DbContent;
using FCmsManagerAngular.ViewModels;
using System.Linq;

namespace FCmsManagerAngular.Controllers
{
    [ApiController]
    public class DbContentController : ControllerBase
    {
        [HttpPost]
        [Route("api/v1/db")]
        public ApiResultModel Index(Guid repositoryid)
        {
            var manager = new CmsManager();
            IDbRepository repository = manager.Data.DbRepositories.Where(m => m.Id == repositoryid).FirstOrDefault();
            if (repository == null)
                return new ApiResultModel(ApiResultModel.NOT_FOUND);

            DbContentStore store = new DbContentStore(repository);
            store.GetContent(new FCms.DbContent.Models.ContentSearchRequest());


            return new ApiResultModel(ApiResultModel.SUCCESS)
            {
                Data = null
            }; ;
        }
    }
}
