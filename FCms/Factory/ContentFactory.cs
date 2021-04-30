using System.Globalization;
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
                return new ContentFolderItem();
            }

            return new StringContentItem();
        }


        public static IContentDefinition CreateContentDefinition(string typeName)
        {
            if ((typeName != null) && (typeName.ToUpperInvariant() == "STRING"))
            {
                return new StringContentDefinition();
            }
            return new StringContentDefinition();
        }
    }
}
