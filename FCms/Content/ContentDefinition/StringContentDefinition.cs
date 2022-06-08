using System;

namespace FCms.Content
{
    public class StringContentDefinition : BaseContentDefinition, IContentDefinition
    {
        public override ContentDefinitionType GetDefinitionType()
        {
            return ContentDefinitionType.String;
        }
    }
}
