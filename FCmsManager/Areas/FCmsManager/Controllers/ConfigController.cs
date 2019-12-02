using Microsoft.AspNetCore.Mvc;

namespace FCmsManager.Areas.FCmsManager.Controllers
{
    [Area("fcmsmanager")]
    public class ConfigController : Controller
    {
        [HttpGet("fcmsmanager/config", Name = "fcmsconfig")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
