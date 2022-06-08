using System;

namespace FCms.Content
{
    public class DateContentDefinition: BaseContentDefinition, IContentDefinition
    {
        public override ContentDefinitionType GetDefinitionType()
        {
            return ContentDefinitionType.Date;
        }
    }
}
