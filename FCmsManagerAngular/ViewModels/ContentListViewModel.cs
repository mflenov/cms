using System;
using System.Collections.Generic;
using FCms.Content;

namespace FCmsManagerAngular.ViewModels
{
    public class ContentListViewModel
    {
        public Guid RepositoryId { get; set; }

        public string RepositoryName { get; set; }

        public List<IContentDefinition> ContentDefinitions { get; set; } = new List<IContentDefinition>();

        public List<ContentItem> ContentItems { get; set; } = new List<ContentItem>();
    }
}
