using FCms.Content;

namespace FCms.Factory
{
    public static class ContentFactory
    {
        public static ContentItem CreateContentByType(IContentDefinition definition)
        {
            if (definition is StringContentDefinition)
            {
                return new StringContentItem();
            }

            if (definition is FolderContentDefinition)
            {
                return new ContentFolder();
            }

            return new StringContentItem();
        }


        public static IContentDefinition CreateContentDefinition(string type)
        {
            if (type.ToLower() == "string")
            {
                return new StringContentDefinition();
            }
            return new StringContentDefinition();
        }
    }
}
