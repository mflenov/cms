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
        public ReporitoryType ReporitoryType { get; set; }

        public IRepository MapToModel(IRepository model)
        {
            model.Name = this.Name;
            model.Id = this.Id ?? Guid.NewGuid();
            model.ReporitoryType = this.ReporitoryType;
            return model;
        }

        public bool IsItANewRepository() 
            => this.Id.HasValue == false;

        public void MapToFrom(IRepository model)
        {
            this.Name = model.Name;
            this.Id = model.Id;
            this.ReporitoryType = model.ReporitoryType;
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

        public IEnumerable<SelectListItem> RepositoryTypeList
        {
            get
            {
                return new List<SelectListItem>
                    {
                        new SelectListItem { Text = "Page", Value = "Page"},
                        new SelectListItem { Text = "Content Storage", Value = "Content"},
                    };
            }
        }

    }
}
