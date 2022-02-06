using System;
using System.Collections.Generic;

namespace FCmsManagerAngular.ViewModels
{
    public class ContentFilterViewModel
    {
        public Guid FilterDefinitionId { get; set; }
    
        public IEnumerable<object> Values { get; set; } = new List<object>();

        public string FilterType { get; set; }

        public string DataType { get; set; }
    }
}
