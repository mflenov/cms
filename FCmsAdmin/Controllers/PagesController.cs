using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using FCms.Content;
using FCmsManagerAngular.ViewModels;

namespace FCmsManagerAngular.Controllers
{
    [ApiController]
    public class PagesControllers
    {
        IConfiguration config;

        public PagesControllers(IConfiguration config) {
            this.config = config;
        }

        [HttpGet]
        [Route("api/v1/pages")]
        public IEnumerable<PageViewModel> Index()
        {
            var manager = new CmsManager(config["DataLocation"]);

            foreach (IRepository repository in manager.Data.Repositories.Where(m => m.ContentType == ContentType.Page))
            {
                yield return new PageViewModel(){
                    Id = repository.Id,
                    Name = repository.Name
                };
            }
        }

        [HttpGet]
        [Route("api/v1/page/structure/{id}")]
        public ApiResultModel Get(string id)
        {
            var manager = new CmsManager(config["DataLocation"]);
            Guid repositoryid;
            if (!Guid.TryParse(id, out repositoryid))
            {
                return new ApiResultModel(ApiResultModel.FAIL) {
                    Description = "Incorrect id: " + id
                    };
            }

            IRepository repository = manager.GetRepositoryById(repositoryid);
            if (repository == null)
            {
                return new ApiResultModel(ApiResultModel.NOT_FOUND);
            }

            return new ApiResultModel(ApiResultModel.SUCCESS)
            {
                Data = new PageStructureViewModel() {
                    Id = repository.Id,
                    Name = repository.Name,
                    ContentDefinitions = repository.ContentDefinitions.Select(m => new ContentDefinitionViewModel(m) {})
                    }
            };
         }

        [HttpPut]
        [Route("api/v1/page")]
        public ApiResultModel Put(PageStructureViewModel model)
        {
            var manager = new CmsManager(config["DataLocation"]);

            IRepository repository = manager.GetRepositoryById(model.Id);
            if (repository == null)
            {
                return new ApiResultModel(ApiResultModel.NOT_FOUND);
            }
            model.MapToModel(repository);
            manager.Save();

            return new ApiResultModel(ApiResultModel.SUCCESS);
         }

        [HttpPost]
        [Route("api/v1/page")]
        public ApiResultModel Post(NewPageViewModel model)
        {
            var manager = new CmsManager(config["DataLocation"]);

            var repository = new Repository();
            repository.Name = model.Name;
            repository.Id = Guid.NewGuid();
            repository.ContentType = model.Template == EnumViewModel.DATABASE_CONTENT ? ContentType.DbContent : ContentType.Page;
            if (model.Template == EnumViewModel.SIMPLE_PAGE)
            {
                RepositoryTemplate.ApplyTemplate(ContentTemplate.SimplePage, repository);
            }
            manager.AddRepository(repository);
            manager.Save();

            return new ApiResultModel(ApiResultModel.SUCCESS);
         }


        [HttpDelete]
        [Route("api/v1/page/{id}")]
        public ApiResultModel Delete(string id) {
            var manager = new CmsManager(config["DataLocation"]);

            Guid guid;
            if (Guid.TryParse(id, out guid)) {
                manager.DeleteRepository(guid);
                manager.Save();
                return new ApiResultModel(ApiResultModel.SUCCESS);
            }

            return new ApiResultModel(ApiResultModel.NOT_FOUND);
        }
    }
}