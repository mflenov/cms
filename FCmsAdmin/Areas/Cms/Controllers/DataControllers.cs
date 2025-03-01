using System;
using FCmsManagerAngular.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FCmsManagerAngular.Controllers;

[ApiController]
public class DataControllers
{
    [HttpGet]
    [Route("cms/api/v1/data/enums")]
    public EnumViewModel Enums()
    {
        return new EnumViewModel();
    }
}

