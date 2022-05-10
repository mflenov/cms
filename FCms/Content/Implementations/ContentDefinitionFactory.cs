using System;

namespace FCms.Content
{
    public static class ContentDefinitionFactory
    {
        public static IContentDefinition CreateContentDefinition(ContentDefinitionType contentType)
        {
            return CreateContentDefinition(contentType.ToString());
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1304:Specify CultureInfo", Justification = "<Pending>")]
        public static IContentDefinition CreateContentDefinition(string contentTypeName)
        {
            switch ((contentTypeName ?? "").ToLower().Trim())
            {
                case "string":
                    return new StringContentDefinition();
                case "longstring":
                    return new LongStringContentDefinition();
                case "folder":
                    return new FolderContentDefinition();
            }

            throw new NotSupportedException();
        }
    }
}
