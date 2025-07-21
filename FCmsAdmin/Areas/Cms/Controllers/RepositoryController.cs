using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using FCms.Content;
using FCmsManagerAngular.ViewModels;
using FCms.DbContent;
using System.Threading.Tasks;

namespace FCmsManagerAngular.Controllers;

[Area("cms")]
[ApiController]
public class RepositoryController
{
    [HttpGet]
    [Route("cms/api/v1/repositories/{contenttype}")]
    public IEnumerable<PageViewModel> Index(string contenttype)
    {
        var manager = CmsManager.GetInstance();

        ContentType contentType = Enum.Parse<ContentType>(contenttype);

        foreach (IRepository repository in manager.Data.Repositories.Where(m => m.ContentType == contentType))
        {
            yield return new PageViewModel() {
                Id = repository.Id,
                Name = repository.Name
            };
        }
    }

    [HttpGet]
    [Route("cms/api/v1/repository/structure/{id}")]
    public ApiResultModel Get(string id)
    {
        var manager = CmsManager.GetInstance();
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

    [HttpPatch]
    [Route("cms/api/v1/repository/{contenttype}")]
    public async Task<ApiResultModel> Put(PageStructureViewModel model, string contenttype)
    {
        var manager = CmsManager.GetInstance();

        IRepository repository = manager.GetRepositoryById(model.Id);
        if (repository == null)
        {
            return new ApiResultModel(ApiResultModel.NOT_FOUND);
        }
        model.MapToModel(repository);
        manager.Save();

        var scaffold = (repository as IDbRepository)?.Scaffold();
        if (scaffold != null)
            await scaffold;

        return new ApiResultModel(ApiResultModel.SUCCESS);
        }

    [HttpPut]
    [Route("cms/api/v1/repository/{contenttype}")]
    public async Task<ApiResultModel> Post(NewPageViewModel model, string contenttype)
    {
        var manager = CmsManager.GetInstance();

        ContentType contentType = Enum.Parse<ContentType>(contenttype);

        var repository = FCms.Factory.RepositoryFactory.CreateRepository(contentType, model.Name);
        if (model.Template == EnumViewModel.SIMPLE_PAGE)
            RepositoryTemplate.ApplyTemplate(ContentTemplate.SimplePage, repository);

        manager.AddRepository(repository);
        manager.Save();

        var scaffold = (repository as IDbRepository)?.Scaffold();
        if (scaffold != null)
            await scaffold;

        return new ApiResultModel(ApiResultModel.SUCCESS);
        }


    [HttpDelete]
    [Route("cms/api/v1/repository/{contenttype}/{id}")]
    public ApiResultModel Delete(string id, string contenttype) {
        var manager = CmsManager.GetInstance();

        Guid guid;
        if (Guid.TryParse(id, out guid)) {
            manager.DeleteRepository(guid);
            manager.Save();
            return new ApiResultModel(ApiResultModel.SUCCESS);
        }

        return new ApiResultModel(ApiResultModel.NOT_FOUND);
    }
}
