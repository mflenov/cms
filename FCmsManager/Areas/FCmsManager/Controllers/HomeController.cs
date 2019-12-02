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
