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
                ICmsManager manager = new CmsManager();
                IRepository repo;
                
                if (model.IsItANewRepository())
                {
                    repo = model.MapToModel(new Repository());
                    model.ApplyTemplate(repo);
                    manager.AddRepository(repo);
                }
                else
                {
                    try
                    {
                        int repoindex = manager.GetIndexById(model.Id.Value);
                        repo = manager.Data.Repositories[repoindex];
                        model.MapToModel(repo);
                    }
                    catch (InvalidOperationException)
                    {
                        throw new Exception("The content definition not found");
                    }

                }
                manager.Save();
                return Redirect("/fcmsmanager/" + ViewModelHelpers.GetRepositoryBaseUrl(repo));
            }

            return View("Edit", new RepositoryViewModel());
        }

        [HttpGet("fcmsmanager/repository/delete", Name = "fcmsrepodelete")]
        public IActionResult delete(Guid repositoryid)
        {
            var cmsManager = new CmsManager();
            var repo = cmsManager.GetRepositoryById(repositoryid);
            if (repo != null) {
                cmsManager.DeleteRepository(repositoryid);
                cmsManager.Save();
            }

            return Redirect("/fcmsmanager/" + ViewModelHelpers.GetRepositoryBaseUrl(repo));
        }
    }
}
