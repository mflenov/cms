using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using FCms.Content;
using FCmsManagerAngular.ViewModels;
using FCms.Auth.Abstract;
using FCms.Auth.Concrete;

namespace FCmsManagerAngular.Controllers;

[Area("cms")]
[ApiController]
public class ConfigUsersController
{
    public static List<ICmsUsers> cmsUsers = null;
    public static ICmsUsers localUsers = FCms.Auth.Implementations.CmsUsers.GetInstance();

    public ConfigUsersController()
    {
        if (cmsUsers == null)
            cmsUsers = new List<ICmsUsers> { localUsers };
    }

    [HttpGet]
    [Route("cms/api/v1/config/users")]
    public ApiResultModel List(string contenttype)
    {
        List<UserViewModel> users = new List<UserViewModel>();
        foreach(var repository in cmsUsers.SelectMany(u => u.GetAllUsers()))
        {
            users.Add(new UserViewModel() {
                Id = repository.Id,
                Username = repository.Username
            });
        }

        return new ApiResultModel(ApiResultModel.SUCCESS) {
                Data = users
        };  
    }

    [HttpPut]
    [Route("cms/api/v1/config/user")]
    public void Post(UserViewModel model)
    {
        var user = new CmsUserModel() {
            Id = model.Id,
            Username = model.Username
        };
        user.PasswordHash = user.HashPassword(model.Password);
        localUsers.Add(user);
    }

    [HttpPatch]
    [Route("cms/api/v1/config/user")]
    public void Put(UserViewModel model)
    {
        var user = new CmsUserModel() {
            Id = model.Id,
            Username = model.Username
        };
        user.PasswordHash = user.HashPassword(model.Password);
        localUsers.Update(user);
    }

    [HttpDelete]
    [Route("cms/api/v1/config/user/{id}")]
    public ApiResultModel Delete(string id)
    {
        var manager = CmsManager.GetInstance();
        Guid guid;
        if (Guid.TryParse(id, out guid)) {
            localUsers.Delete(guid);
            return new ApiResultModel(ApiResultModel.SUCCESS);
        }
        return new ApiResultModel(ApiResultModel.NOT_FOUND);
    }
}