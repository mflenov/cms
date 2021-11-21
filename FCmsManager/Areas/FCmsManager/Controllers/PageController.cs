using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FCms.Content;
using FCmsManager.ViewModel;
using System.Linq;

namespace FCmsManager.Controllers
{
    [Area("fcmsmanager")]
    [Authorize(AuthenticationSchemes = "fcms")]
    public class PageController : Controller
    {
        [HttpGet("fcmsmanager/page", Name = "fcmspageindex")]
        public IActionResult Index()
        {
            return View("index", new CmsManager());
        }

        [HttpGet("fcmsmanager/page/edit", Name = "fcmscontenteditorindex")]
        public IActionResult IndexAction(Guid repositoryid)
        {
            IRepository repository = new CmsManager().GetRepositoryById(repositoryid);

            IContentStore contentStore = new CmsManager().GetContentStore(repositoryid);

            PageEditorViewModel model = new PageEditorViewModel() {
                RepositoryId = repositoryid,
                RepositoryName = repository.Name,
                ContentDefinitions = repository.ContentDefinitions,
                ContentItems = contentStore.Items.Where(m => m.Filters.Count == 0).ToList()
                };

            return View("edit", model);
        }

        
        [HttpPost("fcmsmanager/page/edit", Name = "fcmscontenteditorindexpost")]
        public IActionResult PostAction(FilteredContentViewModel filter)
        {
            IRepository repository = new CmsManager().GetRepositoryById(filter.RepositoryId);

            IContentStore contentStore = new CmsManager().GetContentStore(filter.RepositoryId);

            var filters = ViewModelHelpers.GetFilters(Request);

            PageEditorViewModel model = new PageEditorViewModel() {
                RepositoryId = filter.RepositoryId,
                RepositoryName = repository.Name,
                ContentDefinitions = repository.ContentDefinitions,
                ContentItems = contentStore.Items.Where(m => m.MatchFilters(filters)).ToList(),
                Filters = filters
                };

            return View("edit", model);
        }

        [HttpPost("fcmsmanager/page/save", Name = "fcmscontenteditorsave")]
        public IActionResult SaveAction(PageEditorViewModel model)
        {
            var manager = new CmsManager();
            IRepository repository = manager.GetRepositoryById(model.RepositoryId);
            if (repository == null)
            {
                return Redirect("/fcmsmanager/page");
            }

            IContentStore contentStore = manager.GetContentStore(model.RepositoryId);
            model.RepositoryName = repository.Name;
            model.ContentDefinitions = repository.ContentDefinitions;
            model.ContentItems = contentStore.Items.Where(m => m.Filters.Count == 0).ToList();

            if (ModelState.IsValid) {

                foreach (var definition in model.ContentDefinitions.Where(m => m is not FolderContentDefinition))
                {
                    ContentItem item = model.ContentItems.FirstOrDefault(m => m.DefinitionId == definition.DefinitionId);
                    if (item == null)
                    {
                        item = FCms.Factory.ContentFactory.CreateContentByType(definition);
                        ViewModelHelpers.MapToContentItem(item, Request, null, definition);
                        contentStore.Items.Add(item);
                    }
                    else
                    {
                        ViewModelHelpers.MapToContentItem(item, Request, item.Id, definition);
                    }
                }

                manager.SaveContentStore(contentStore);
                return Redirect("/fcmsmanager/page");
            }

            return View("edit", model);
        }
    }
}
