using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FCms.Content;
using FCmsManager.ViewModel;

namespace FCmsManager.Controllers
{
    [Area("fcmsmanager")]
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
            ContentListViewModel model = new ContentListViewModel
            {
                RepositoryId = repositoryid,
                DefinitionId = definitionid,
                ContentDefinition =
                    repository.ContentDefinitions.FirstOrDefault(m => m.DefinitionId == definitionid),
                Items = contentStore.Items.Where(m => m.DefinitionId == definitionid).ToList()
            };

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

            EditContentViewModel model = new EditContentViewModel
            {
                Item = contentStore.Items.FirstOrDefault(m => m.Id == contentid)
            };
            if (model.Item == null)
                return Redirect("/fcmsmanager/repository?d=" + contentid);

            model.ContentDefinition = repository.ContentDefinitions.FirstOrDefault(m => m.DefinitionId == model.Item.DefinitionId);
            model.RepositoryId = repositoryid;

            return View("Edit", model);
        }

        [HttpPost("fcmsmanager/content/save"), ValidateAntiForgeryToken]
        public IActionResult SaveContentPost(EditContentViewModel model)
        {
            if (!ModelState.IsValid) return View("Edit", new RepositoryViewModel());
            ICmsManager manager = CmsManager.Load();
            IContentStore contentStore = manager.GetContentStore(model.RepositoryId);
            ContentItem item = contentStore.Items.FirstOrDefault(m => m.Id == model.Item.Id);
            if (item == null)
            {
                IRepository repository = manager.GetRepositoryById(model.RepositoryId);
                item = FCms.Factory.ContentFactory.CreateContentByType(repository.ContentDefinitions.FirstOrDefault(m => m.DefinitionId == model.Item.DefinitionId));
                model.MapToModel(item, Request);
                contentStore.Items.Add(item);
            }
            else
            {
                model.MapToModel(contentStore.Items[contentStore.GetIndexById((Guid)model.Item.Id)], Request);
            }
            contentStore.Save();
            return Redirect("/fcmsmanager/content/list?repositoryid=" + model.RepositoryId + "&definitionid=" + item.DefinitionId);

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

            EditContentViewModel model = new EditContentViewModel
            {
                Item = FCms.Factory.ContentFactory.CreateContentByType(
                    repository.ContentDefinitions.FirstOrDefault(m => m.DefinitionId == definitionid)),
                ContentDefinition = repository.ContentDefinitions.FirstOrDefault(m => m.DefinitionId == definitionid),
                RepositoryId = repositoryid
            };

            model.Item.DefinitionId = definitionid;

            return View("Edit", model);
        }

        [HttpPost("fcmsmanager/content/filter")]
        public IActionResult Filter(Guid filterid, int index)
        {
            var manager = CmsManager.Load();
            FilterValueViewModel model = new FilterValueViewModel
            {
                FilterDefinition = manager.Filters.FirstOrDefault(m => m.Id == filterid),
                ContentFilter = new ContentFilter(),
                Index = index
            };


            if (model.FilterDefinition == null)
            {
                return new ContentResult();
            }

            return View("Filter" + model.FilterDefinition.Type, model);
        }
    }
}
