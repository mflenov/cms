﻿using System;
using System.Linq;
using System.Collections.Generic;
using FCms.Content;

namespace FCmsManagerAngular.ViewModels
{
    public class PageStructureViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<ContentDefinitionViewModel> ContentDefinitions { get; set; }

        public void MapToModel(IRepository repository)
        {
            repository.ContentDefinitions = ContentDefinitions.Select(m => m.ConvertToContentDefinition()).ToList();
            repository.Name = Name;
        }
    }
}
