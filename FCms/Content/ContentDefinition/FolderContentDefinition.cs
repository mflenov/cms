using System;
using System.Collections.Generic;

namespace FCms.Content
{
    public class FolderContentDefinition : IContentDefinition
    {
        public Guid DefinitionId { get; set; }

        public string Name { get; set; }

        List<IContentDefinition> definitions = new List<IContentDefinition>();
        public List<IContentDefinition> Definitions {
            get { return definitions; }
        }

        public string GetTypeName()
        {
            return GetDefinitionType().ToString();
        }

        public ContentDefinitionType GetDefinitionType()
        {
            return ContentDefinitionType.Folder;
        }
    }
}
