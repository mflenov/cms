using System;
using System.Collections.Generic;
using System.Linq;
using FCms.Content;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using FCms.Tools;

namespace FCmsManager.ViewModel
{
    public class EditContentViewModel
    {
        private ICmsManager manager = new CmsManager();
        public EditContentViewModel()
        {
        }

        public Guid RepositoryId { get; set; }

        public string RepositoryName { get; set; }

        public Guid DefinitionId { get; set; }

        public IContentDefinition ContentDefinition { get; set; }

        public ContentItem Item { get; set; } = new StringContentItem();
        
        public IEnumerable<FilterValueViewModel> ContentFilters {
            get {
                int index = 1;
                foreach (ContentFilter filter in Item.Filters)
                {
                    yield return new FilterValueViewModel()
                    {
                        ContentFilter = filter,
                        FilterDefinition = manager.Data.Filters.Where(m => m.Id == filter.FilterDefinitionId).FirstOrDefault(),
                        Index = index
                    };
                    index++;
                }
            }
        }

        public ContentItem MapToModel(ContentItem model, HttpRequest request)
        {
            return ViewModelHelpers.MapToContentItem(model, request, Item.Id, this.ContentDefinition);
        }

        public IEnumerable<SelectListItem> GlobalFilters {
            get {
                foreach (var filter in manager.Data.Filters)
                {
                    yield return new SelectListItem { Text = filter.Name, Value = filter.Id.ToString() };
                }
            }
        }

    }
}
