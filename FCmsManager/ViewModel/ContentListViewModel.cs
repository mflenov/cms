﻿using System;
using System.Collections.Generic;
using FCms.Content;

namespace FCmsManager.ViewModel
{
    public class ContentListViewModel
    {
        public Guid RepositoryId { get; set; }

        public Guid DefinitionId { get; set; }

        List<ContentItem> items = new List<ContentItem>();
        
        public List<ContentItem> Items { get { return items; } set { items = value; } }

        public IContentDefinition ContentDefinition { get; set; }
    }
}
