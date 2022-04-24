using System;
using System.Collections.Generic;

namespace FCmsManagerAngular.ViewModels
{
    public class EnumViewModel
    {
        public const string EMPTY_PAGE = "Empty Page";
        public const string SIMPLE_PAGE = "Simple Page";
        public const string DATABASE_CONTENT = "Database Content";

        public IEnumerable<string> filterTypes
        {
            get { return Enum.GetNames(typeof(FCms.Content.IFilter.FilterType)); }
        }

        public IEnumerable<string> contentDataTypes
        {
            get { return Enum.GetNames(typeof(FCms.Content.ContentDefinitionType)); }
        }

        public IEnumerable<string> pageTemplates
        {
            get {
                return new List<string>() { EMPTY_PAGE, SIMPLE_PAGE };
            }
        }
    }
}
