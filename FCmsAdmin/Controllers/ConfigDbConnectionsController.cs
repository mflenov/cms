using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FCmsManagerAngular.ViewModels;
using FCms.Content;

namespace FCmsManagerAngular.Controllers;

[ApiController]
public class ConfigDbConnectionsController: ControllerBase
{
    public ConfigDbConnectionsController()
    {
    }
    
    [HttpGet]
    [Route("api/v1/config/dbconnections")]
    public ApiResultModel DbConnections()
    {
        var manager = CmsManager.GetInstance();

        return new ApiResultModel(ApiResultModel.SUCCESS) {
                Data = manager.Data.DbConnections.Select(m => new DbConnectionViewModel(m))
        };
    }

    [HttpGet]
    [Route("api/v1/config/dbconnection/{id}")]
    public DbConnectionViewModel Get(string id)
    {
        var manager = CmsManager.GetInstance();
        Guid guid;
        if (Guid.TryParse(id, out guid)) {
            return manager.Data.DbConnections.Where(n => n.Id == guid).Select(m => new DbConnectionViewModel(m)).FirstOrDefault();
        }
        return null;
    }

    [HttpPut]
    [Route("api/v1/config/dbconnection")]
    public void Post(DbConnectionViewModel model)
    {
        ICmsManager manager = CmsManager.GetInstance();
        model.Add(manager);
    }

    [HttpPatch]
    [Route("api/v1/config/dbconnection")]
    public void Put(DbConnectionViewModel model)
    {
        ICmsManager manager = CmsManager.GetInstance();
        model.Update(manager);
    }

    [HttpDelete]
    [Route("api/v1/config/dbconnection/{id}")]
    public ApiResultModel Delete(string id)
    {
        var manager = CmsManager.GetInstance();
        Guid guid;
        if (Guid.TryParse(id, out guid)) {
            var dbconnection = manager.Data.DbConnections.Where(n => n.Id == guid).FirstOrDefault();
            if (dbconnection != null) {
                manager.Data.DbConnections.Remove(dbconnection);
                manager.Save();
                return new ApiResultModel(ApiResultModel.SUCCESS);
            }
        }
        return new ApiResultModel(ApiResultModel.NOT_FOUND);
    }    
}