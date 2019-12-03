using System;
using System.Collections.Generic;

namespace FCms.Content
{
    public class ContentFilter: IContentFilter
    {
        public Guid FilterDefinitionId { get; set; }

        public int Index { get; set; }

        private List<object> values = new List<object>();
        public List<object> Values { get { return values; } }

        public IFilter Filter { get; set; }

        public IContentFilter.ContentFilterType FilterType { get; set; }

        public ContentFilter()
        {
        }

        public virtual bool Validate(object value)
        {
            bool isValid = Filter.Validate(Values, value);
            return FilterType == IContentFilter.ContentFilterType.Include ? isValid : !isValid;
        }

        public string GetValue(int index)
        {
            if (values.Count > index) { return values[index].ToString(); }
            return "";
        }
    }
}
