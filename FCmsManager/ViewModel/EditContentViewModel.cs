using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FCms.Content;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using FCms.Tools;

namespace FCmsManager.ViewModel
{
    public class EditContentViewModel
    {
        private ICmsManager manager = CmsManager.Load();
        public EditContentViewModel()
        {
        }

        public Guid RepositoryId { get; set; }

        public IContentDefinition ContentDefinition { get; set; }

        ContentItem item = new ContentItem();
        public ContentItem Item { get { return item;  } set { item = value; } }
        
        public IEnumerable<FilterValueViewModel> ContentFilters {
            get {
                int index = 1;
                foreach (ContentFilter filter in item.Filters)
                {
                    yield return new FilterValueViewModel()
                    {
                        ContentFilter = filter,
                        FilterDefinition = manager.Filters.Where(m => m.Id == filter.FilterDefinitionId).FirstOrDefault(),
                        Index = index
                    };
                    index++;
                }
            }
        }

        public ContentItem MapToModel(ContentItem model, HttpRequest request)
        {
            model.DefinitionId = Guid.Parse(Utility.GetRequestValueDef(request, "DefinitionId", ""));
            model.Id = Item.Id ?? Guid.NewGuid();
            if (request.Form.ContainsKey("Value")) {
                model.Value = Utility.GetRequestValueDef(request, "Value", "");
            }

            if (request.Form.ContainsKey("numbderoffilters"))
            {
                MapFilters(model, request);
            }

            return model;
        }

        private void MapFilters(ContentItem model, HttpRequest request)
        {
            int index = 0;
            model.Filters.Clear();
            int numberoffilters = Utility.GetRequestIntValueDef(request, "numbderoffilters", -1);
            for (int i = 1; i <= numberoffilters;  i++)
            {
                string typeName = Utility.GetRequestValueDef(request, "filtertype" + i.ToString(), "");
                if (String.IsNullOrEmpty(typeName))
                {
                    continue;
                }
                IFilter filter = FCms.Factory.FilterFactory.CreateFilterByTypeName(typeName);
                filter.Id = Guid.Parse(Utility.GetRequestValueDef(request, "filterid" + i.ToString(), ""));

                ContentFilter contentFilter = new ContentFilter()
                {
                    Filter = filter,
                    FilterType = (IContentFilter.ContentFilterType)Utility.GetRequestIntValueDef(request, "contentfiltertype" + i.ToString(), 0),
                    FilterDefinitionId = filter.Id,
                    Index = index
                };
                contentFilter.Values.AddRange(
                    filter.ParseValues(Utility.GetRequestList(request, "filtervalue" + i.ToString()))
                    );
                model.Filters.Add(contentFilter);
                index++;
            }
        }

        public IEnumerable<SelectListItem> GlobalFilters {
            get {
                var manager = CmsManager.Load();
                foreach (var filter in manager.Filters)
                {
                    yield return new SelectListItem { Text = filter.Name, Value = filter.Id.ToString() };
                }
            }
        }

    }
}
