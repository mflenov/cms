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

        public bool MatchFilters(List<IContentFilter> filters)
        {
            if (filters == null || Filters.Count != filters.Count)
            {
                return false;
            }
            var lookup = Filters.ToLookup(m => m.GetHashValue());
            foreach (var filter in filters)
            {
                if (!lookup.Contains(filter.GetHashValue()))
                {
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
