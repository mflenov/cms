using System;
using System.Collections.Generic;

namespace FCmsManagerAngular.ViewModels
{
    public class EnumViewModel
    {
        public IEnumerable<string> filterTypes
        {
            get { return Enum.GetNames(typeof(FCms.Content.IFilter.FilterType)); }
        }

        public IEnumerable<string> contentDataTypes
        {
            get { return Enum.GetNames(typeof(FCms.Content.ContentDefinitionType)); }
        }
    }
}
