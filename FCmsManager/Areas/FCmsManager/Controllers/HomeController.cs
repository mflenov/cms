using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FCmsManager.Controllers
{
    [Area("fcmsmanager")]
    [Authorize(AuthenticationSchemes = "fcms")]
    public class HomeController : Controller
    {
        [HttpGet("fcmsmanager/home", Name = "fcmsmanagerhome")]
        public IActionResult Index()
        {
            return View("index");
        }
    }
}
