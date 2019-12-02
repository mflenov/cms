﻿using System;
using System.Collections.Generic;
using FCms.Content;

namespace FCmsManager.ViewModel
{
    public class ContentViewModel
    {
        public Guid RepositoryId { get; set; }
        
        public List<IContentDefinition> ContentDefinitions { get; set; }
    }
}
