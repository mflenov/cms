using System;
using System.Collections.Generic;

namespace FCms.Content
{
    public interface IContentFilter
    {
        public Guid FilterDefinitionId { get; set; }

        public enum ContentFilterType { Include, Exclude }

        public IFilter Filter { get; set; }

        int Index { get; set; }
    
        List<object> Values { get; }

        ContentFilterType FilterType { get; set; }

        bool Validate(object value);

        object GetValue(int index);

        string GetStringValue(int index);

        string GetHashValue();
    }
}
