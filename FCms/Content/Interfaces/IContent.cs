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

        public string Data { get { return this.GetValue().ToString(); } }

        bool MatchFilters(List<IContentFilter> filters, Boolean skipEmpty = false);

        bool ValidateFilters(Dictionary<string, object> filters);

        string GetHtmlString();

        string GetTypeName();

        object GetValue();
    }
}
