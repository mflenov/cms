using System;

namespace FCms.Content
{
    public class LongStringContentDefinition : BaseContentDefinition, IContentDefinition
    {
        public override ContentDefinitionType GetDefinitionType()
        {
            return ContentDefinitionType.LongString;
        }
    }

}