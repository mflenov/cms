using System;
using FCms.Content;

namespace FCmsManagerAngular.ViewModels
{
    public class ContentDefinitionViewModel
    {
        public Guid DefinitionId { get; set; }

        public string Name { get; set; }

        public string TypeName { get; set; }

        public IContentDefinition ConvertToContentDefinition()
        {
            IContentDefinition definition = FCms.Factory.ContentFactory.CreateContentDefinition(TypeName);
            definition.DefinitionId = DefinitionId;
            definition.Name = Name;            
            return definition;
        }
    }
}
