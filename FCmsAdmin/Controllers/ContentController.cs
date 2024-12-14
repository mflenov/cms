using System;
using System.Collections.Generic;
using System.Linq;
using FCms;
using FCms.Content;
using FCmsManagerAngular.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FCmsManagerAngular.Controllers
{
    [ApiController]
    public class ContentController : ControllerBase
    {
        public ContentController() {
        }

        [HttpPost]
        [Route("api/v1/content/list/{repositoryId}/{definitionId}")]
        public ApiResultModel List(Guid repositoryId, Guid definitionId, ContentRequestModel request) {
            CmsManager manager = CmsManager.GetInstance();
            IRepository repository = manager.GetRepositoryById(repositoryId);
            IContentStore contentStore = ContentStore.Load(repositoryId);

            var searchFilters = request.getFiltersModel().ToList();
            IEnumerable<ContentItem> contentItems = 
                contentStore.Items.Where(m => m.DefinitionId == definitionId && m.MatchFilters(searchFilters, true));
            var contentDefinition = repository.ContentDefinitions.Where(m => m.DefinitionId == definitionId).Select(m => new ContentDefinitionViewModel(m) {}).FirstOrDefault();

            ContentListViewModel model = new ContentListViewModel() {
                ContentItems = contentItems.Select(m => new ContentViewModel(m)),
                Definition = contentDefinition,
                RepositoryName = repository.Name
                };

            return new ApiResultModel(ApiResultModel.SUCCESS) {
                Data = model
            };
        }

        [HttpPost]
        [Route("api/v1/content")]
        public ApiResultModel Index(ContentRequestModel request)
        {
            CmsManager manager = CmsManager.GetInstance();
            IRepository repository = manager.GetRepositoryById(request.repositoryid);

            IContentStore contentStore = ContentStore.Load(request.repositoryid);

            var searchFilters = request.getFiltersModel().ToList();
            IEnumerable<ContentItem> contentItems = request.filters == null ?
                contentStore.Items.Where(m => m.Filters.Count == 0):
                contentStore.Items.Where(m => m.MatchFilters(searchFilters));

            PageContentViewModel model = new PageContentViewModel() {
                RepositoryId = request.repositoryid,
                ContentItems = contentItems.Select(m => new ContentViewModel(m))
                };

            return new ApiResultModel(ApiResultModel.SUCCESS) {
                Data = model
            };
        }

        [HttpPost]
        [Route("api/v1/content/filter")]
        public ApiResultModel Filter(ContentRequestModel request)
        {
            CmsManager manager = CmsManager.GetInstance();
            IRepository repository = manager.GetRepositoryById(request.repositoryid);
            ContentEngine engine = new ContentEngine(repository.Name);

            var definitions = manager.Data.Filters.ToLookup(m => m.Id);
            
            var filters = request.filters
                    .Where(m => m.Values != null && m.Values.Count() > 0 && m.Values.First().ToString() != "")
                    .ToDictionary(
                        m => definitions[m.FilterDefinitionId].FirstOrDefault().Name,
                        m => definitions[m.FilterDefinitionId].FirstOrDefault().ParseValues(m.Values.Select(m => m.ToString()).ToList()).FirstOrDefault()
                    );

            List<PagePreviewItemViewModel> contentItems = new List<PagePreviewItemViewModel>();
            foreach (IContentDefinition definition in repository.ContentDefinitions)
            {
                if (definition.GetDefinitionType() == ContentDefinitionType.Folder)
                {
                    List<ContentItem> folders = engine.GetContents<ContentItem>(definition.Name, filters).ToList();
                    if (folders.Count == 0)
                        continue;

                    var childdefinitions = (definition as FolderContentDefinition).Definitions.ToLookup(m=> m.DefinitionId);

                    foreach(var folder in folders)
                    {
                        var foldermodel = new PagePreviewItemViewModel() { Name = definition.Name };
                        foldermodel.Children = new List<PagePreviewItemViewModel>();
                        foldermodel.Children = (folder as ContentFolderItem).Childeren.Select(m =>
                            new PagePreviewItemViewModel() { Name = childdefinitions[m.DefinitionId].First().Name, Value = m.GetValue().ToString() }
                            ).ToList();
                        contentItems.Add(foldermodel);
                    }
                }
                else
                {
                    ContentItem contentitem = engine.GetContents<ContentItem>(definition.Name, filters).FirstOrDefault();
                    if (contentitem == null)
                        continue;
                    contentItems.Add(new PagePreviewItemViewModel() { Name = definition.Name, Value = contentitem.GetValue().ToString() });
                }
            }

            return new ApiResultModel(ApiResultModel.SUCCESS) {
                Data = contentItems
            };
        }

        [HttpDelete]
        [Route("api/v1/content/{repositoryId}/{contentId}")]
        public ApiResultModel DeleteContent(Guid repositoryId, Guid contentId) {
            CmsManager manager = CmsManager.GetInstance();
            IContentStore contentStore = ContentStore.Load(repositoryId);
            contentStore.Items.RemoveAll(m => m.Id == contentId);
            contentStore.Save();
            return new ApiResultModel(ApiResultModel.SUCCESS) {
                
            };
        }

        [HttpPut]
        [Route("api/v1/content")]
        public ApiResultModel Save(PageContentViewModel model)
        {
            CmsManager manager = CmsManager.GetInstance();
            IRepository repository = manager.GetRepositoryById(model.RepositoryId);
            IContentStore contentStore = ContentStore.Load(model.RepositoryId);

            var definitionCache = repository.ContentDefinitions.ToDictionary(m => m.DefinitionId, m => m);
            var storeItems = contentStore.Items.Where(m => model.ContentItems.Any(c => c.Id == m.Id)).ToDictionary(m => m.Id ?? System.Guid.NewGuid(), m => m);

            contentStore.Items.RemoveAll(m => model.ContentItems.Any(n => n.IsDeleted && n.Id == m.Id));
            foreach (var item in model.ContentItems.Where(m => !m.IsDeleted))
            {
                var definition = definitionCache[item.DefinitionId];
                if (item.Id != null && storeItems.ContainsKey(item.Id.Value))
                {
                    item.MapToModel(storeItems[item.Id.Value], definition);
                }
                else
                {
                    var newitem = FCms.Factory.ContentFactory.CreateContentByType(definition);
                    item.MapToModel(newitem, definition);
                    contentStore.Items.Add(newitem);
                }

            }
            contentStore.Save();

            return new ApiResultModel(ApiResultModel.SUCCESS) {
                Data = model
            };
        }

        [HttpPut]
        [Route("api/v1/contentitem/{repositoryid}")]
        public ApiResultModel SaveContentItem(Guid repositoryid, ContentViewModel model)
        {
            CmsManager manager = CmsManager.GetInstance();
            IRepository repository = manager.GetRepositoryById(repositoryid);
            IContentStore contentStore = ContentStore.Load(repositoryid);

            var definition = repository.ContentDefinitions.Where(m => m.DefinitionId == model.DefinitionId).FirstOrDefault();
            var storeItem = contentStore.Items.Where(m => m.Id == model.Id.Value).FirstOrDefault();

            if (model.Id != null && storeItem != null)
            {
                model.MapToModel(storeItem, definition);
            }
            else
            {
                var newitem = FCms.Factory.ContentFactory.CreateContentByType(definition);
                model.MapToModel(newitem, definition);
                contentStore.Items.Add(newitem);
            }

            contentStore.Save();

            return new ApiResultModel(ApiResultModel.SUCCESS) {
                Data = model
            };
        }
    }
}
