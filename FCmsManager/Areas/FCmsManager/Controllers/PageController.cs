using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FCmsManager.ViewModel;
using FCms.Content;
using Microsoft.AspNetCore.Authorization;

namespace FCmsManager.Controllers
{
    [Area("fcmsmanager")]
    [Authorize(AuthenticationSchemes = "fcms")]
    public class PageController : Controller
    {
        [HttpGet("fcmsmanager/page", Name = "fcmspage")]
        public IActionResult Index(Guid repositoryid)
        {

            return View("Index", null);
        }
    }
}
