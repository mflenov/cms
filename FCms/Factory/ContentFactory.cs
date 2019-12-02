using FCms.Content;

namespace FCms.Factory
{
    public static class ContentFactory
    {
        public static ContentItem CreateContentByType(IContentDefinition definition)
        {
            // review: what is it?
            if (definition is StringContentDefinition)
            {
                return new StringContentValue();
            }

            return new StringContentValue();
        }
    }
}
