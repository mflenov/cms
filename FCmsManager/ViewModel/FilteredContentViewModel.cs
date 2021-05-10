using System;
using System.Collections.Generic;
using FCms.Content;

namespace FCmsManager.ViewModel
{
    public class FilteredContentViewModel
    {
        public Guid RepositoryId { get; set; }

        public List<IContentFilter> Filters { get; } = new List<IContentFilter>();
    }
}
