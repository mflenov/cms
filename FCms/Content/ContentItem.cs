using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace FCms.Content
{
    public abstract class Content: IContent
    {
        public Guid? Id { get; set; }

        public Guid DefinitionId { get; set; }

        public List<IContentFilter> Filters { get; } = new List<IContentFilter>();

        public string ToolTip { get; set; }

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
    }
}
