using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FCms.Content;
using FCmsManager.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace FCmsManager.Areas.FCmsManager.Controllers
{
    [Area("fcmsmanager")]
    [Authorize(AuthenticationSchemes = "fcms")]
    public class ContenteditorController: Controller
    {
        [HttpGet("fcmsmanager/contenteditor", Name = "fcmscontenteditorindex")]
        public IActionResult IndexAction(Guid repositoryid)
        {
            IRepository repository = CmsManager.Load().GetRepositoryById(repositoryid);

            IContentStore contentStore = CmsManager.Load().GetContentStore(repositoryid);

            ContentEditorViewModel model = new ContentEditorViewModel() {
                RepositoryId = repositoryid,
                RepositoryName = repository.Name,
                ContentDefinitions = repository.ContentDefinitions,
                ContentItems = contentStore.Items.Where(m => m.Filters.Count == 0).ToList()
                };

            return View("Index", model);
        }

        
        [HttpPost("fcmsmanager/contenteditor", Name = "fcmscontenteditorindexpost")]
        public IActionResult PostAction(FilteredContentViewModel filter)
        {
            IRepository repository = CmsManager.Load().GetRepositoryById(filter.RepositoryId);

            IContentStore contentStore = CmsManager.Load().GetContentStore(filter.RepositoryId);

            var filters = ViewModelHelpers.GetFilters(Request);

            ContentEditorViewModel model = new ContentEditorViewModel() {
                RepositoryId = filter.RepositoryId,
                RepositoryName = repository.Name,
                ContentDefinitions = repository.ContentDefinitions,
                ContentItems = contentStore.Items.Where(m => m.MatchFilters(filters)).ToList(),
                Filters = filters
                };

            return View("Index", model);
        }

        [HttpPost("fcmsmanager/contenteditor/save", Name = "fcmscontenteditorsave")]
        public IActionResult SaveAction(ContentEditorViewModel model)
        {
            IRepository repository = CmsManager.Load().GetRepositoryById(model.RepositoryId);
            if (repository == null)
            {
                return Redirect("/fcmsmanager/repository");
            }

            IContentStore contentStore = CmsManager.Load().GetContentStore(model.RepositoryId);
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

                contentStore.Save();
                return Redirect("/fcmsmanager/repository");
            }

            return View("Index", model);
        }
    }
}
