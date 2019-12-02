using System;
using Microsoft.AspNetCore.Mvc;
using FCmsManager.ViewModel;
using FCms.Content;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FCmsManager.Controllers
{
    [Area("fcmsmanager")]
    public class DefinitionController : Controller
    {
        [HttpGet("fcmsmanager/definition", Name = "fcmsdefinition")]
        public IActionResult Definition(Guid repositoryid)
        {
            var cmsManager = CmsManager.Load();
            IRepository repository = cmsManager.GetRepositoryById(repositoryid);

            if (repository == null)
            {
                return Redirect("/fcmsmanager/repository");
            }

            return View("Index", repository);
        }

        [HttpGet("fcmsmanager/definition/add", Name = "fcmsdefinitionadd")]
        public IActionResult add(Guid repositoryid)
        {
            return View("Edit", new ContentDefinitionViewModel
            { 
                RepositoryId = repositoryid
            });
        }

        [HttpPost("fcmsmanager/definition/save"), ValidateAntiForgeryToken]
        public IActionResult SaveDefinition(ContentDefinitionViewModel model)
        {
            if (ModelState.IsValid)
            {
                ICmsManager manager = CmsManager.Load();
                int repoindex = manager.GetIndexById(model.RepositoryId.Value);   
                if (repoindex < 0) {
                    throw new Exception("The content definition not found");
                }
                IRepository repository = manager.Repositories[repoindex];
                if (model.DefinitionId == null) {
                    repository.ContentDefinitions.Add(model.MapToModel(null));
                }

                manager.Save();
                return Redirect("/fcmsmanager/definition?repositoryid=" + model.RepositoryId);
            }

            return View("Edit", new ContentDefinitionViewModel());
        }
    }
}
