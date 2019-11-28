using FCms.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCms.Factory
{
    public static class ContentFactory
    {
        public static ContentItem CreateContentByType(IContentDefinition definition)
        {
            if (definition is StringContentDefinition)
            {
                return new StringContentValue();
            }

            return new StringContentValue();
        }
    }
}
