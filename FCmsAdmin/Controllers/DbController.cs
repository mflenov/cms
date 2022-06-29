using System;
using System.Collections.Generic;
using System.Linq;
using FCms.Content;
using FCms.DbContent;
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

            foreach (IDbRepository repository in manager.Data.DbRepositories.Where(m => m.ContentType == ContentType.DbContent))
            {
                yield return new PageViewModel(){
                    Id = repository.Id,
                    Name = repository.Name
                };
            }
        }

        [HttpPut]
        [Route("api/v1/db")]
        public ApiResultModel Post(NewPageViewModel model)
        {
            var manager = new CmsManager();

            var repository = new DbRepository();
            repository.Name = model.Name;
            repository.Id = Guid.NewGuid();
            repository.ContentType = ContentType.DbContent;
            manager.AddDbRepository(repository);
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