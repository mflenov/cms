using System;
using System.Collections.Generic;
using System.Text;

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
            return "Folder";
        }
    }
}
