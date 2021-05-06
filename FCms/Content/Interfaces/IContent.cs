using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FCms.Content
{
    public interface IContent
    {
        Guid? Id { get; set; }

        Guid DefinitionId { get; set; }

        List<IContentFilter> Filters { get; }

        public string ToolTip { get; set; }

        public bool MatchFilters(List<IContentFilter> filters);

        public bool ValidateFilters(ILookup<string, PropertyInfo> filterProperties, object filters);

        string GetHtmlString();

        string GetTypeName();

        object GetValue();
    }
}
