using System;
using System.Collections.Generic;

namespace FCms.Content
{
    public class ContentFilter: IContentFilter
    {
        public Guid FilterDefinitionId { get; set; }

        public int Index { get; set; }

        public List<object> Values { get; } = new List<object>();

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

        public object GetValue(int index)
        {
            if (Values.Count > index) { return Values[index]; }
            return null;
        }
        public string GetStringValue(int index)
        {
            return (GetValue(index) ?? "").ToString();
        }
        public string GetHashValue()
        {
            return Filter.Id + String.Join("-", Values);
        }
    }
}
