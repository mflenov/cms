using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FCmsManager.ViewModel;
using FCms.Content;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace FCmsManager.Controllers
{
    [Area("fcmsmanager")]
    [Authorize(AuthenticationSchemes = "fcms")]
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

        [HttpGet("fcmsmanager/definition/deletedefinition", Name = "fcmsdefinitiondelete")]
        public IActionResult deletedefinition(Guid repositoryid, Guid id)
        {
            var cmsManager = CmsManager.Load();
            var repository = cmsManager.GetRepositoryById(repositoryid);
            repository.DeleteDefinition(id);
            cmsManager.Save();

            return Redirect("/fcmsmanager/definition?repositoryid=" + repositoryid);
        }


        [HttpGet("fcmsmanager/definition/add", Name = "fcmsdefinitionadd")]
        public IActionResult add(Guid repositoryid, Guid id)
        {
            return View("Edit", new ContentDefinitionViewModel() { 
                RepositoryId = repositoryid
            });
        }

        [HttpPost("fcmsmanager/definition/addchild", Name = "fcmsdefinitionaddchild")]
        public IActionResult addchild(string contenttype, int index)
        {
            return View(contenttype + "Value", new FolderItemViewModel()
            {
                Id = Guid.NewGuid(),
                Index = index
            });
        }


        [HttpGet("fcmsmanager/definition/edit", Name = "fcmsdefinitionedit")]
        public IActionResult edit(Guid repositoryid, Guid id)
        {
            ICmsManager manager = CmsManager.Load();
            var repo = manager.GetRepositoryById(repositoryid);
            IContentDefinition definition = repo?.ContentDefinitions.Where(m => m.DefinitionId == id).FirstOrDefault();
            if (definition == null)
                return Redirect("/fcmsmanager/repository");

            ContentDefinitionViewModel model = new ContentDefinitionViewModel()
            {
                RepositoryId = repositoryid
            };
            model.MapFromModel(definition);

            return View("Edit", model);
        }

        [HttpPost("fcmsmanager/definition/save"), ValidateAntiForgeryToken]
        public IActionResult saveDefinition(ContentDefinitionViewModel model)
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
                    repository.ContentDefinitions.Add(model.MapToModel(null, this.Request));
                }
                else {
                    model.MapToModel(repository.ContentDefinitions.Where(m => m.DefinitionId == model.DefinitionId).FirstOrDefault(), this.Request);
                }


                manager.Save();
                return Redirect("/fcmsmanager/definition?repositoryid=" + model.RepositoryId);
            }

            return View("Edit", new ContentDefinitionViewModel());
        }
    }
}
