using System;
using System.Collections.Generic;
using FCms.Content;

namespace FCmsManagerAngular.ViewModels
{
    public class PageContentViewModel
    {
        public Guid RepositoryId { get; set; }

        public IEnumerable<ContentViewModel> ContentItems { get; set; }
    }
}
