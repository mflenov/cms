using System;
using Microsoft.AspNetCore.Mvc;
using FCmsManager.ViewModel;
using FCms.Content;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FCmsManager.Controllers
{
    [Area("fcmsmanager")]
    public class RepositoryController : Controller
    {
        [HttpGet("fcmsmanager/repository", Name = "fcmsrepository")]
        public IActionResult Index()
        {
            return View("Index", CmsManager.Load());
        }


        [HttpGet("fcmsmanager/repository/add", Name = "fcmsrepositoryadd")]
        public IActionResult add()
        {
            return View("Edit", new RepositoryViewModel());
        }

        [HttpPost("fcmsmanager/repository/save"), ValidateAntiForgeryToken]
        public IActionResult savePost(RepositoryViewModel model)
        {
            if (!ModelState.IsValid) return View("Edit", new RepositoryViewModel());
            ICmsManager manager = CmsManager.Load();
            if (model.Id == null)
            {
                manager.Repositories.Add(model.MapToModel(new Repository()));
            }
            else
            {
                int repoindex = manager.GetIndexById(model.Id.Value);
                if (repoindex < 0)
                {
                    throw new Exception("The content definition not found");
                }
                model.MapToModel(manager.Repositories[repoindex]);
            }
            manager.Save();
            return Redirect("/fcmsmanager/repository");

        }
    }
}
