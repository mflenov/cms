using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using FCms.Content;
using FCmsManagerAngular.ViewModels;
using FCms.Auth.Abstract;

namespace FCmsManagerAngular.Controllers;

[Area("cms")]
[ApiController]
public class ConfigUsersController
{
    public static List<ICmsUsers> cmsUsers = null;

    public ConfigUsersController()
    {
        if (cmsUsers == null)
        {
            cmsUsers = new List<ICmsUsers> { new FCms.Auth.Implementations.CmsUsers("./users.json") };
        }
    }

    [HttpGet]
    [Route("cms/api/v1/config/users")]
    public IEnumerable<UserViewModel> List(string contenttype)
    {
        
        foreach(var repository in cmsUsers.SelectMany(u => u.GetAllUsers()))
        {
            yield return new UserViewModel() {
                Id = repository.Id,
                Username = repository.Username
            };
        }
    }
}