using System;
using System.Linq;
using System.Collections.Generic;
using FCms.Content;

namespace FCmsManagerAngular.ViewModels
{
    public class ContentDefinitionViewModel
    {
        public ContentDefinitionViewModel()
        {

        }

        public ContentDefinitionViewModel(IContentDefinition definition)
        {
            DefinitionId = definition.DefinitionId;
            Name = definition.Name;
            TypeName = definition.GetTypeName();
            if (definition is FolderContentDefinition) {
                ContentDefinitions = (definition as FolderContentDefinition).Definitions.Select(m => new ContentDefinitionViewModel(m) {}).ToList();
            }
        }

        public Guid DefinitionId { get; set; }

        public string Name { get; set; }

        public string TypeName { get; set; }

        public List<ContentDefinitionViewModel> ContentDefinitions { get; set; } = new List<ContentDefinitionViewModel>();

        public IContentDefinition ConvertToContentDefinition()
        {
            IContentDefinition definition = FCms.Factory.ContentFactory.CreateContentDefinition(TypeName);
            definition.DefinitionId = DefinitionId;
            definition.Name = Name;
            if (definition is FolderContentDefinition)
            {
                (definition as FolderContentDefinition).Definitions = ContentDefinitions.Select(m => m.ConvertToContentDefinition()).ToList();
            }
            return definition;
        }
    }
}
