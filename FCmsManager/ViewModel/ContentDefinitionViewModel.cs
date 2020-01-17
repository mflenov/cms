using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FCms.Content;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using FCms.Tools;

namespace FCmsManager.ViewModel
{
    public class ContentDefinitionViewModel
    {
        public const string FOLDER_CONTENT_TYPE = "Folder";

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public Guid? RepositoryId { get; set; }

        public Guid? DefinitionId { get; set; }

        public string ContentType { get; set; }

        public List<FolderItemViewModel> Children { get; } = new List<FolderItemViewModel>();

        public IEnumerable<SelectListItem> ContentTypeList
        {
            get {
                return new List<SelectListItem>
                    {
                        new SelectListItem { Text = "String", Value = "String"},
                        new SelectListItem { Text = "Folder", Value = "Folder"},
                    };
            }
        }

        public IContentDefinition MapToModel(IContentDefinition model, HttpRequest request) { 
            if (model == null)
            {
                model = ContentDefinitionFactory.CreateContentDefinition(this.ContentType);
            }
            model.Name = this.Name;
            model.DefinitionId = this.DefinitionId ?? Guid.NewGuid();

            if (this.ContentType == FOLDER_CONTENT_TYPE)
            {
                MapChilderen(model as FolderContentDefinition, request);
            }
            return model;
        }

        void MapChilderen(FolderContentDefinition model, HttpRequest request)
        {
            model.Definitions.Clear();
            int numberoffilters = Utility.GetRequestIntValueDef(request, "numbderoffvalues", -1);
            for (int i = 1; i <= numberoffilters; i++)
            {
                IContentDefinition child = ContentDefinitionFactory.CreateContentDefinition(Utility.GetRequestValueDef(request, "childtype" + i.ToString(), ""));
                child.DefinitionId = Guid.Parse(Utility.GetRequestValueDef(request, "childid" + i.ToString(), ""));
                child.Name = Utility.GetRequestValueDef(request, "childname" + i.ToString(), "");
                model.Definitions.Add(child);
            }
        }

        public void MapFromModel(IContentDefinition model)
        {
            this.Name = model.Name;
            this.DefinitionId = model.DefinitionId;
            this.ContentType = model.GetTypeName();

            if (model is FolderContentDefinition) {
                int i = 1;
                foreach (var child in (model as FolderContentDefinition).Definitions)
                {
                    this.Children.Add(
                        new FolderItemViewModel()
                        {
                            Id = child.DefinitionId,
                            Name = child.Name,
                            Index = i,
                            Type = child.GetTypeName()
                        }
                        );
                    i++;
                }
            }
        }
    }
}
