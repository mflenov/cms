using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace FCms.Content
{
    public class ContentItem
    {
        public Guid? Id { get; set; }

        public Guid DefinitionId { get; set; }

        public Object Value { get; set; }

        public string ToolTip { get; set; }

        public ContentItem()
        {
            
        }

        public virtual string GetStringValue()
        {
            return Value.ToString();
        }

        public virtual string GetHtmlString()
        {
            return Value.ToString();
        }

        List<IContentFilter> filters = new List<IContentFilter>();
        public List<IContentFilter> Filters {
            get { return filters; }
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
    }
}
