using System;
using Microsoft.AspNetCore.Mvc;
using FCmsManager.ViewModel;
using FCms.Content;
using Microsoft.AspNetCore.Authorization;

namespace FCmsManager.Controllers
{
    [Area("fcmsmanager")]
    [Authorize(AuthenticationSchemes = "fcms")]
    public class RepositoryController : Controller
    {
        [HttpGet("fcmsmanager/repository", Name = "fcmsrepository")]
        public IActionResult Index()
        {
            return View("Index", CmsManager.Load());
        }


        [HttpGet("fcmsmanager/repository/add", Name = "fcmsrepositoryadd")]
        public IActionResult Add()
        {
            return View("Edit", new RepositoryViewModel());
        }

        [HttpPost("fcmsmanager/repository/save"), ValidateAntiForgeryToken]
        public IActionResult SavePost(RepositoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                ICmsManager manager = CmsManager.Load();
                
                if (model.IsItANewRepository())
                {
                    var newRrepository = model.MapToModel(new Repository());
                    model.ApplyTemplate(newRrepository);
                    manager.AddRepository(newRrepository);
                }
                else
                {
                    try
                    {
                        int repoindex = manager.GetIndexById(model.Id.Value);
                        model.MapToModel(manager.Repositories[repoindex]);
                    }
                    catch (InvalidOperationException)
                    {
                        throw new Exception("The content definition not found");
                    }

                }
                manager.Save();
                return Redirect("/fcmsmanager/repository");
            }

            return View("Edit", new RepositoryViewModel());
        }
    }
}
