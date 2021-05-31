using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FCms.Content;
using FCmsManager.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace FCmsManager.Controllers
{
    [Area("fcmsmanager")]
    [Authorize(AuthenticationSchemes = "fcms")]
    public class ContentController : Controller
    {
        [HttpGet("fcmsmanager/content")]
        public IActionResult Index()
        {
            return View("index", CmsManager.Load());
        }
    }
}