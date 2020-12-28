using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FCms.Content;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FCmsManager.ViewModel
{
    public class RepositoryViewModel : IValidatableObject
    {
        public Guid? Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public ReporitoryTypeTemplate Template { get; set; }

        public IRepository MapToModel(IRepository model)
        {
            model.Name = this.Name;
            model.Id = this.Id ?? Guid.NewGuid();
            return model;
        }

        public void ApplyTemplate(IRepository model)
        {
            model.ReporitoryType = this.Template == ReporitoryTypeTemplate.Content ? ReporitoryType.Content : ReporitoryType.Page;

            if (this.Template == ReporitoryTypeTemplate.SimplePage)
            {
                ApplySimpleTemplate(model);
            }
        }

        void ApplySimpleTemplate(IRepository model)
        {
            model.AddDefinition("Title", IContentDefinition.DefinitionType.String);
            model.AddDefinition("Description", IContentDefinition.DefinitionType.String);
            model.AddDefinition("Content", IContentDefinition.DefinitionType.LongString);
        }

        public bool IsNewRepository
            => this.Id.HasValue == false;

        public void MapToFrom(IRepository model)
        {
            this.Name = model.Name;
            this.Id = model.Id;
            this.Template = model.ReporitoryType == ReporitoryType.Content ? ReporitoryTypeTemplate.Content  : ReporitoryTypeTemplate.EmptyPage;
        }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            ICmsManager cmsmanager = CmsManager.Load();
            IRepository repository = cmsmanager.GetRepositoryByName(this.Name);
            if (repository != null && repository.Id != this.Id)
            {
                yield return new ValidationResult($"The repository {this.Name} already exists", new String[] { "Name" });
            }
            
            yield break;
        }

        public IEnumerable<SelectListItem> RepositoryTypeTemplateList
        {
            get
            {
                return new List<SelectListItem>
                    {
                        new SelectListItem { Text = "Empty Page", Value = "EmptyPage"},
                        new SelectListItem { Text = "Simple Page", Value = "SimplePage"},
                        new SelectListItem { Text = "Content Storage", Value = "Content"},
                    };
            }
        }

    }
}
