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
        private ICmsManager manager = CmsManager.Load();
        public EditContentViewModel()
        {
        }

        public Guid RepositoryId { get; set; }

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

            if (model is ContentFolderItem)
            {
                MapFolder(model, request);
            }
            else
            {
                MapScalar(model, request);
            }

            if (request.Form.ContainsKey("numbderoffilters"))
            {
                MapFilters(model, request);
            }

            return model;
        }

        private void MapScalar(ContentItem model, HttpRequest request)
        {
            if (model is StringContentItem)
            {
                (model as StringContentItem).Data = Utility.GetRequestValueDef(request, "Value" + model.DefinitionId.ToString(), "");
            }
        }

        private void MapFolder(ContentItem model, HttpRequest request) {
            ((ContentFolderItem)model).Childeren.Clear();

            foreach (var definition in (this.ContentDefinition as FolderContentDefinition).Definitions)
            {
                if (definition is StringContentDefinition)
                {
                    ((ContentFolderItem)model).Childeren.Add(
                        new StringContentItem()
                        {
                            DefinitionId = definition.DefinitionId,
                            Data = Utility.GetRequestValueDef(request, "Value" + definition.DefinitionId.ToString(), ""),
                            Id = Utility.GetRequestGuidDefNew(request, "Id" + definition.DefinitionId.ToString())
                        }
                        );
                }
            }
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
                foreach (var filter in manager.Filters)
                {
                    yield return new SelectListItem { Text = filter.Name, Value = filter.Id.ToString() };
                }
            }
        }

    }
}
