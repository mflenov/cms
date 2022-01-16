using System;
using System.Collections.Generic;
using FCms.Content;

namespace FCmsManagerAngular.ViewModels
{
    public class PageContentViewModel
    {
        public IEnumerable<ContentViewModel> ContentItems { get; set; }
    }
}
