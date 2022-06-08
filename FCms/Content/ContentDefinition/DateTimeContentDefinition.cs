using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCms.Content
{
    public class DateTimeContentDefinition : BaseContentDefinition, IContentDefinition
    {
        public override ContentDefinitionType GetDefinitionType()
        {
            return ContentDefinitionType.DateTime;
        }
    }
}
