using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FCms.Content;
using FCmsManager.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace FCmsManager.Controllers
{
    [Area("fcmsmanager")]
    [Authorize(AuthenticationSchemes = "fcms")]
    public class PageContentController : Controller
    {
        [HttpGet("fcmsmanager/pagecontent", Name = "fcmscontent")]
        public IActionResult Index(Guid repositoryid)
        {
            IRepository repository = new CmsManager().GetRepositoryById(repositoryid);
            if (repository == null)
            {
                return Redirect("/fcmsmanager/page");
            }
            PageContentViewModel model = new PageContentViewModel();
            model.RepositoryId = repositoryid;
            model.RepositoryName = repository.Name;
            model.ContentDefinitions = repository.ContentDefinitions;

            return View("Index", model);
        }

        [HttpGet("fcmsmanager/pagecontent/list", Name = "fcmscontentlist")]
        public IActionResult List(Guid repositoryid, Guid definitionid)
        {
            IRepository repository = new CmsManager().GetRepositoryById(repositoryid);
            if (repository == null)
            {
                return Redirect("/fcmsmanager/page");
            }
            IContentStore contentStore = new CmsManager().GetContentStore(repositoryid);
            PageContentListViewModel model = new PageContentListViewModel();
            model.RepositoryId = repositoryid;
            model.RepositoryName = repository.Name;
            model.DefinitionId = definitionid;
            model.ContentDefinition = repository.ContentDefinitions.Where(m => m.DefinitionId == definitionid).FirstOrDefault();
            model.Items = contentStore.Items.Where(m => m.DefinitionId == definitionid).ToList();

            return View("List", model);
        }

        [HttpGet("fcmsmanager/pagecontent/delete", Name = "fcmscontentdelete")]
        public IActionResult Delete(Guid repositoryid, Guid contentid)
        {
            ICmsManager manager = new CmsManager();
            IRepository repository = manager.GetRepositoryById(repositoryid);
            if (repository == null)
                return Redirect("/fcmsmanager/home");

            IContentStore contentStore = manager.GetContentStore(repositoryid);

            var item = contentStore.Items.Where(m => m.Id == contentid).FirstOrDefault();
            if (item == null)
                return Redirect("/fcmsmanager/repository?d=" + contentid);

            contentStore.Items.Remove(item);
            contentStore.Save();

            return Redirect("/fcmsmanager/pagecontent/list?repositoryid=" + repositoryid + "&definitionid=" + item.DefinitionId.ToString());
        }

        [HttpGet("fcmsmanager/pagecontent/edit", Name = "fcmscontentedit")]
        public IActionResult Edit(Guid repositoryid, Guid contentid)
        {
            ICmsManager manager = new CmsManager();
            IRepository repository = manager.GetRepositoryById(repositoryid);
            if (repository == null)
                return Redirect("/fcmsmanager/home");

            IContentStore contentStore = manager.GetContentStore(repositoryid);

            EditContentViewModel model = new EditContentViewModel();
            model.RepositoryName = repository.Name;
            model.Item = contentStore.Items.Where(m => m.Id == contentid).FirstOrDefault();
            if (model.Item == null)
                return Redirect("/fcmsmanager/repository?d=" + contentid);

            model.ContentDefinition = repository.ContentDefinitions.Where(m => m.DefinitionId == model.Item.DefinitionId).FirstOrDefault();
            model.RepositoryId = repositoryid;

            return View("Edit", model);
        }

        [HttpPost("fcmsmanager/pagecontent/save"), ValidateAntiForgeryToken]
        public IActionResult EditContentSave(EditContentViewModel model)
        {
            if (ModelState.IsValid)
            {
                ICmsManager manager = new CmsManager();
                IContentStore contentStore = manager.GetContentStore(model.RepositoryId);
                ContentItem item = contentStore.Items.Where(m => m.Id == model.Item.Id).FirstOrDefault();

                IRepository repository = manager.GetRepositoryById(model.RepositoryId);
                model.ContentDefinition = repository.ContentDefinitions.Where(m => m.DefinitionId == model.DefinitionId).FirstOrDefault();

                if (item == null)
                {
                    item = FCms.Factory.ContentFactory.CreateContentByType(model.ContentDefinition);
                    model.MapToModel(item, Request);
                    contentStore.Items.Add(item);
                }
                else
                {
                    model.MapToModel(contentStore.GetById((Guid)model.Item.Id), Request);
                }
                contentStore.Save();
                return Redirect("/fcmsmanager/pagecontent/list?repositoryid=" + model.RepositoryId + "&definitionid=" + item.DefinitionId.ToString());
            }

            return View("Edit", new RepositoryViewModel());
        }

        [HttpGet("fcmsmanager/pagecontent/add")]
        public IActionResult Add(Guid repositoryid, Guid definitionid)
        {
            ICmsManager manager = new CmsManager();
            IRepository repository = manager.GetRepositoryById(repositoryid);
            if (repository == null)
            {
                throw new FCms.Exceptions.RepositoryNotFoundException();
            }

            IContentStore contentStore = manager.GetContentStore(repositoryid);

            EditContentViewModel model = new EditContentViewModel();
            model.Item = FCms.Factory.ContentFactory.CreateContentByType(repository.ContentDefinitions.Where(m => m.DefinitionId == definitionid).FirstOrDefault());
            model.ContentDefinition = repository.ContentDefinitions.Where(m => m.DefinitionId == definitionid).FirstOrDefault();

            model.RepositoryId = repositoryid;
            model.Item.DefinitionId = definitionid;

            return View("Edit", model);
        }

        [HttpPost("fcmsmanager/pagecontent/filter")]
        public IActionResult Filter(Guid filterid, int index)
        {
            var manager = new CmsManager();
            FilterValueViewModel model = new FilterValueViewModel();
            model.FilterDefinition = manager.Data.Filters.Where(m => m.Id == filterid).FirstOrDefault();

            model.ContentFilter = new ContentFilter();
            model.Index = index;

            if (model.FilterDefinition == null)
            {
                return new ContentResult();
            }

            return View("NewFilter", model);
        }
    }
}
