using Microsoft.AspNetCore.Mvc;
using FCmsManager.ViewModel;
using FCms.Auth;

using Microsoft.AspNetCore.Authentication;

namespace FCmsManager.Controllers
{
    [Area("fcmsmanager")]
    public class LoginController: Controller
    {
        [HttpGet("fcmsmanager/login")]
        public IActionResult Index()
        {

            return View("index", new LoginViewModel());
        }

        [HttpPost("fcmsmanager/login")]
        public IActionResult IndexPost(LoginViewModel model)
        {
            CmsMember member = new CmsMember();
            if (ModelState.IsValid && member.Authenticate(model.Username, model.Password)) 
            {

                HttpContext.SignInAsync(member.getUserPrincipal());

                return Redirect("/fcmsmanager/home");
            }
            else
            {
                ModelState.AddModelError("Username", "Username or password is incorrect");
            }

            return View("index", model);
        }
    }
}
