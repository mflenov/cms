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
    public class ContentController : Controller
    {
        [HttpGet("fcmsmanager/content", Name = "fcmscontent")]
        public IActionResult Index(Guid repositoryid)
        {
            IRepository repository = CmsManager.Load().GetRepositoryById(repositoryid);
            if (repository == null)
            {
                return Redirect("/fcmsmanager/repository");
            }
            ContentViewModel model = new ContentViewModel();
            model.RepositoryId = repositoryid;
            model.RepositoryName = repository.Name;
            model.ContentDefinitions = repository.ContentDefinitions;

            return View("Index", model);
        }

        [HttpGet("fcmsmanager/content/list", Name = "fcmscontentlist")]
        public IActionResult List(Guid repositoryid, Guid definitionid)
        {
            IRepository repository = CmsManager.Load().GetRepositoryById(repositoryid);
            if (repository == null)
            {
                return Redirect("/fcmsmanager/repository");
            }
            IContentStore contentStore = CmsManager.Load().GetContentStore(repositoryid);
            ContentListViewModel model = new ContentListViewModel();
            model.RepositoryId = repositoryid;
            model.RepositoryName = repository.Name;
            model.DefinitionId = definitionid;
            model.ContentDefinition = repository.ContentDefinitions.Where(m => m.DefinitionId == definitionid).FirstOrDefault();
            model.Items = contentStore.Items.Where(m => m.DefinitionId == definitionid).ToList();

            return View("List", model);
        }

        [HttpGet("fcmsmanager/content/edit", Name = "fcmscontentedit")]
        public IActionResult Edit(Guid repositoryid, Guid contentid)
        {
            ICmsManager manager = CmsManager.Load();
            IRepository repository = manager.GetRepositoryById(repositoryid);
            if (repository == null)
                return Redirect("/fcmsmanager/repository");

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

        [HttpPost("fcmsmanager/content/save"), ValidateAntiForgeryToken]
        public IActionResult savecontentPost(EditContentViewModel model)
        {
            if (ModelState.IsValid)
            {
                ICmsManager manager = CmsManager.Load();
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
                    model.MapToModel(contentStore.Items[contentStore.GetIndexById((Guid)model.Item.Id)], Request);
                }
                contentStore.Save();
                return Redirect("/fcmsmanager/content/list?repositoryid=" + model.RepositoryId + "&definitionid=" + item.DefinitionId.ToString());
            }

            return View("Edit", new RepositoryViewModel());
        }

        [HttpGet("fcmsmanager/content/add")]
        public IActionResult Add(Guid repositoryid, Guid definitionid)
        {
            ICmsManager manager = CmsManager.Load();
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

        [HttpPost("fcmsmanager/content/filter")]
        public IActionResult Filter(Guid filterid, int index)
        {
            var manager = CmsManager.Load();
            FilterValueViewModel model = new FilterValueViewModel();
            model.FilterDefinition = manager.Filters.Where(m => m.Id == filterid).FirstOrDefault();

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
