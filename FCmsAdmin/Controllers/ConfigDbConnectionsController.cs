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
        var manager = new CmsManager();

        return new ApiResultModel(ApiResultModel.SUCCESS) {
                Data = manager.Data.DbConnections.Select(m => new DbConnectionViewModel(m))
        };
    }
}