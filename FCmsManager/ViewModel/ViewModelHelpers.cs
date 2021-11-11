using System;
using System.Collections.Generic;
using FCms.Content;
using FCms.Tools;
using Microsoft.AspNetCore.Http;

namespace FCmsManager.ViewModel
{
    public static class ViewModelHelpers
    {
        public static string GetRepositoryBaseUrl(IRepository repo)
        {
            return repo.ContentType.ToString().ToLowerInvariant();
        }

        public static List<IContentFilter> GetFilters(HttpRequest request)
        {
            List<IContentFilter> filters = new List<IContentFilter>();

            int index = 0;
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
                filters.Add(contentFilter);
                index++;
            }
            return filters;
        }

        public static ContentItem MapToContentItem(ContentItem model, HttpRequest request, Guid? id, IContentDefinition contentDefinition)
        {
            model.DefinitionId = contentDefinition.DefinitionId;
            model.Id = id ?? Guid.NewGuid();

            if (model is ContentFolderItem)
            {
                MapFolder(model, request, contentDefinition);
            }
            else
            {
                MapScalar(model, request);
            }

            if (request.Form.ContainsKey("numbderoffilters"))
            {
                model.Filters.AddRange(ViewModelHelpers.GetFilters(request));
            }

            return model;
        }

        private static void MapScalar(ContentItem model, HttpRequest request)
        {
            if (model is StringContentItem)
            {
                (model as StringContentItem).Data = Utility.GetRequestValueDef(request, "Value" + model.DefinitionId.ToString(), "");
            }
        }

        private static  void MapFolder(ContentItem model, HttpRequest request, IContentDefinition ContentDefinition) {
            ((ContentFolderItem)model).Childeren.Clear();
            ((ContentFolderItem)model).Name = Utility.GetRequestValueDef(request, "FolderName" + model.DefinitionId, "");

            foreach (var definition in (ContentDefinition as FolderContentDefinition).Definitions)
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

    }
}
