using System;
using Microsoft.AspNetCore.Mvc;
using FCms.Content;
using FCms.DbContent;
using FCmsManagerAngular.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace FCmsManagerAngular.Controllers
{
    [ApiController]
    public class DbContentController : ControllerBase
    {
        [HttpPost]
        [Route("api/v1/dbcontent")]
        public async Task<ApiResultModel> Index(DbContentRequestViewModel model)
        {
            var manager = new CmsManager();
            IDbRepository repository = manager.Data.DbRepositories.Where(m => m.Id == model.RepositoryId).FirstOrDefault();
            if (repository == null)
                return new ApiResultModel(ApiResultModel.NOT_FOUND);

            DbContentStore store = new DbContentStore(repository);
            var content = await store.GetContent(new FCms.DbContent.Models.ContentSearchRequest());

            return new ApiResultModel(ApiResultModel.SUCCESS)
            {
                Data = new DbContentListViewModel()
                {
                    Columns = content.Columns,
                    Rows = content.Rows,
                }
            }; ;
        }
    }
}
