using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using FCms.Content;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FCmsManager.ViewModel
{
    public class ContentDefinitionViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public Guid? RepositoryId { get; set; }

        public Guid? DefinitionId { get; set; }

        public string ContentType { get; set; }

        public IEnumerable<SelectListItem> ContentTypeList {
            get {
                return new List<SelectListItem>
                    {
                        new SelectListItem { Text = "String", Value = "String"},
                        new SelectListItem { Text = "Folder", Value = "Folder"},
                    };
            }
        }

        public IContentDefinition MapToModel(IContentDefinition model) { 
            if (model == null)
            {
                model = ContentDefinitionFactory.CreateContentDefinition(this.ContentType);
            }
            model.Name = this.Name;
            model.DefinitionId = this.DefinitionId ?? Guid.NewGuid();
            return model;
        }
    }
}
