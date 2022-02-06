using System;
using System.Linq;
using System.Collections.Generic;
using FCms.Content;

namespace FCmsManagerAngular.ViewModels {

    public class ContentRequestModel {
        public Guid repositoryid { get; set; }

        public List<ContentFilterViewModel> filters { get; set; }

        public IEnumerable<IContentFilter> getFiltersModel() {

            List<IContentFilter> result = new List<IContentFilter>();

            int index = 0;
            for (int i = 0; i < filters.Count;  i++)
            {
                IFilter filter = FCms.Factory.FilterFactory.CreateFilterByTypeName(filters[i].DataType);
                filter.Id = filters[i].FilterDefinitionId;

                ContentFilter contentFilter = new ContentFilter()
                {
                    Filter = filter,
                    FilterType = filters[i].FilterType == IContentFilter.ContentFilterType.Include.ToString() ? IContentFilter.ContentFilterType.Include : IContentFilter.ContentFilterType.Exclude,
                    FilterDefinitionId = filter.Id,
                    Index = index + 1
                };
                contentFilter.Values.AddRange(filter.ParseValues(filters[i].Values.Select(m => m.ToString()).ToList()));
                result.Add(contentFilter);
                index++;
            }
            return result;
        }
    };
}