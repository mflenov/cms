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
    public class DbContentController : Controller
    {
        [HttpGet("fcmsmanager/dbcontent")]
        public IActionResult Index()
        {
            return View("index", CmsManager.Load());
        }
    }
}