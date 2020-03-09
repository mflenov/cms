using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FCmsManager.Areas.FCmsManager.Controllers
{
    [Authorize(AuthenticationSchemes = "fcms")]
    [Authorize]
    public class ConfigController : Controller
    {
        [HttpGet("fcmsmanager/config", Name = "fcmsconfig")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
