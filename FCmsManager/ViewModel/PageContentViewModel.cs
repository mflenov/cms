using System;
using System.Collections.Generic;
using System.Text;
using FCms.Content;

namespace FCmsManager.ViewModel
{
    public class PageContentViewModel
    {
        public Guid RepositoryId { get; set; }

        public string RepositoryName { get; set; }

        public List<IContentDefinition> ContentDefinitions { get; set; }
    }
}
