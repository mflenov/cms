using System;
using System.Collections.Generic;
using System.Linq;
using FCms.Content;
using FCmsManagerAngular.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FCmsManagerAngular.Controllers
{
    [ApiController]
    public class DbController : ControllerBase
    {
        IConfiguration config;

        public DbController(IConfiguration config) {
            this.config = config;
        }

        [HttpGet]
        [Route("api/v1/db")]
        public IEnumerable<PageViewModel> Index()
        {
            var manager = new CmsManager();

            foreach (IRepository repository in manager.Data.Repositories.Where(m => m.ContentType == ContentType.DbContent))
            {
                yield return new PageViewModel(){
                    Id = repository.Id,
                    Name = repository.Name
                };
            }
        }

        [HttpPost]
        [Route("api/v1/db")]
        public ApiResultModel Post(NewPageViewModel model)
        {
            var manager = new CmsManager();

            var repository = new Repository();
            repository.Name = model.Name;
            repository.Id = Guid.NewGuid();
            repository.ContentType = ContentType.DbContent;
            manager.AddRepository(repository);
            manager.Save();

            return new ApiResultModel(ApiResultModel.SUCCESS);
         }

        [HttpDelete]
        [Route("api/v1/db/{id}")]
        public ApiResultModel Delete(Guid id) {
            var manager = new CmsManager();
            manager.DeleteRepository(id);
            manager.Save();
            return new ApiResultModel(ApiResultModel.SUCCESS);
        }
   }
}