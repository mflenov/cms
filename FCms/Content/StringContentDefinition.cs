using System;

namespace FCms.Content
{
    public class StringContentDefinition : IContentDefinition
    {
        public Guid DefinitionId { get; set; }

        public string Name { get; set; }

        public string GetTypeName()
        {
            return "String";
        }
    }
}
