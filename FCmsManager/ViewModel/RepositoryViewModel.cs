using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using FCms.Content;

namespace FCmsManager.ViewModel
{
    public class RepositoryViewModel: IValidatableObject
    {
        public Guid? Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public IRepository MapToModel(IRepository model)
        {
            model.Name = this.Name;
            model.Id = this.Id ?? Guid.NewGuid();
            return model;
        }

        public void MapToFrom(IRepository model)
        {
            this.Name = model.Name;
            this.Id = model.Id;
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
    }
}
