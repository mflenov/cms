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
            };
        }

        [HttpPut]
        [Route("api/v1/dbcontent")]
        public async Task<ApiResultModel> Save(DbContentItemModel model)
        {
            var manager = new CmsManager();
            IDbRepository repository = manager.Data.DbRepositories.Where(m => m.Id == model.RepositoryId).FirstOrDefault();
            if (repository == null)
                return new ApiResultModel(ApiResultModel.NOT_FOUND);

            DbContentStore store = new DbContentStore(repository);
            await store.Add(model.Row.Values);
            return new ApiResultModel(ApiResultModel.SUCCESS);
        }

        [HttpDelete]
        [Route("api/v1/dbcontent")]
        public async Task<ApiResultModel> Delete(string repositoryid, string id)
        {
            var manager = new CmsManager();
            Guid repoid = Guid.Parse(repositoryid);
            IDbRepository repository = manager.Data.DbRepositories.Where(m => m.Id == repoid).FirstOrDefault();
            if (repository == null)
                return new ApiResultModel(ApiResultModel.NOT_FOUND);

            DbContentStore store = new DbContentStore(repository);
            await store.Delete(id);
            return new ApiResultModel(ApiResultModel.SUCCESS);
        }
    }
}
