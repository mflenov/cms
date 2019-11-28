using FCms.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCmsManager.ViewModel
{
    public class FilterValueViewModel
    {
        public IFilter FilterDefinition { get; set; }

        public int Index { get; set; }

        public IContentFilter ContentFilter { get; set; }
    }
}
