using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace FCmsManager.Controllers
{
    [Area("fcmsmanager")]
    public class HomeController : Controller
    {
        [HttpGet("fcmsmanager/home", Name = "fcmsmanagerhome")]
        public IActionResult Index()
        {
            return View("index");
        }
    }
}
