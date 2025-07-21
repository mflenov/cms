using System;
using System.Collections.Generic;
using FCms.Content;
using FCms.DbContent;
using FCmsManagerAngular.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FCmsManagerAngular.Controllers;

[ApiController]
public class DbRepositoryController : ControllerBase
{
    public DbRepositoryController() {
    }

    [HttpGet]
    [Route("cms/api/v1/db")]
    public IEnumerable<PageViewModel> Index()
    {
        var manager = CmsManager.GetInstance();

        foreach (IDbRepository repository in manager.Data.DbRepositories)
        {
            yield return new PageViewModel(){
                Id = repository.Id,
                Name = repository.Name
            };
        }
    }

    [HttpPut]
    [Route("cms/api/v1/db")]
    public ApiResultModel Post(NewDbRepoViewModel model)
    {
        var manager = CmsManager.GetInstance();

        var repository = new DbRepository();
        repository.Name = model.Name;
        repository.Id = Guid.NewGuid();
        repository.ContentType = ContentType.DbContent;
        repository.DatabaseConnectionId = Guid.Parse(model.ConnectionId);
        manager.AddRepository(repository);
        manager.Save();

        return new ApiResultModel(ApiResultModel.SUCCESS);
        }

    [HttpDelete]
    [Route("cms/api/v1/db/{id}")]
    public ApiResultModel Delete(Guid id) {
        var manager = CmsManager.GetInstance();
        manager.DeleteRepository(id);
        manager.Save();
        return new ApiResultModel(ApiResultModel.SUCCESS);
    }
}
