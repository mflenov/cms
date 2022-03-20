using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace FCms.Content
{
    public abstract class ContentItem: IContent
    {
        public Guid? Id { get; set; }

        public Guid DefinitionId { get; set; }

        public List<IContentFilter> Filters { get; } = new List<IContentFilter>();

        public string ToolTip { get; set; }

        public bool MatchFilters(List<IContentFilter> searchContentFilters, Boolean skipEmpty = false)
        {
            if (searchContentFilters == null)
                return skipEmpty;
            
            if (skipEmpty == false && Filters.Count != searchContentFilters.Count)
                return false;

            var lookup = Filters.ToLookup(m => m.GetHashValue());

            foreach (var searchContentFilter in searchContentFilters)
            {
                if (searchContentFilter.Filter is DateRangeFilter)
                {
                    var dateContentFilter = Filters.Where(m => m.Filter.Id == searchContentFilter.Filter.Id).FirstOrDefault();
                    if (dateContentFilter == null)
                        return false;

                    DateTime? datevalue = Tools.Utility.StringToDateTime(searchContentFilter.Values.FirstOrDefault().ToString());
                    if (datevalue == null || !dateContentFilter.Validate(datevalue))
                        return false;
                }
                else if (!lookup.Contains(searchContentFilter.GetHashValue())) {
                    return false;
                }
            }

            return true;
        }

        public bool ValidateFilters(ILookup<string, PropertyInfo> filterProperties, object filters)
        {
            if (filterProperties == null)
                return false;

            foreach (IContentFilter filter in Filters)
            {
                if (filterProperties[filter.Filter.Name].FirstOrDefault() == null)
                    return false;

                var propertyInfo = filterProperties[filter.Filter.Name].FirstOrDefault();
                var value = propertyInfo.GetValue(filters);
                if (value == null || !filter.Validate(value))
                {
                    return false;
                }
            }

            return true;
        }

        public abstract object GetValue();

        public abstract string GetHtmlString();

        public abstract string GetTypeName();
    }
}
