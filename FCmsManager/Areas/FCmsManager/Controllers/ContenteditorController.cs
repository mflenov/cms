using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FCms.Content;
using FCmsManager.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace FCmsManager.Areas.FCmsManager.Controllers
{
    [Area("fcmsmanager")]
    [Authorize(AuthenticationSchemes = "fcms")]
    public class ContenteditorController: Controller
    {

    }
}
