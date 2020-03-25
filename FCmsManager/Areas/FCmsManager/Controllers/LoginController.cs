using FCms.Auth.Abstract;
using FCmsManager.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace FCmsManager.Controllers
{
    [Area("fcmsmanager")]
    public class LoginController : Controller
    {
        private readonly ICmsMember _cmsMember;

        public LoginController(ICmsMember cmsMember)
        {
            _cmsMember = cmsMember;
        }

        [HttpGet("fcmsmanager/login")]
        public IActionResult Index()
        {
            return View("Index", new LoginViewModel());
        }

        [HttpPost("fcmsmanager/login")]
        public IActionResult IndexPost(LoginViewModel model)
        {
            if (ModelState.IsValid && _cmsMember.Authenticate(model.Username, model.Password))
            {
                HttpContext.SignInAsync(_cmsMember.GetUserPrincipal());

                return Redirect("/fcmsmanager/home");
            }

            ModelState.AddModelError("Username", "Username or password is incorrect");

            return View("Index", model);
        }
    }
}