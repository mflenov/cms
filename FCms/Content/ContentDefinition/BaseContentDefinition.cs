using System;

namespace FCms.Content
{
    public abstract class BaseContentDefinition: IContentDefinition
    {
        public Guid DefinitionId { get; set; }

        public string Name { get; set; }

        public string GetTypeName()
        {
            return GetDefinitionType().ToString();
        }

        public abstract ContentDefinitionType GetDefinitionType();
    }
}
