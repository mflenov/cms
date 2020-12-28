using System;

namespace FCms.Content
{
    public static class ContentDefinitionFactory
    {
        public static IContentDefinition CreateContentDefinition(IContentDefinition.DefinitionType contentType)
        {
            return CreateContentDefinition(contentType.ToString());
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1304:Specify CultureInfo", Justification = "<Pending>")]
        public static IContentDefinition CreateContentDefinition(string contentTypeName)
        {
            string cleartypename = (contentTypeName ?? "").Trim().ToUpperInvariant();
            if (IContentDefinition.DefinitionType.String.ToString().ToUpperInvariant() == cleartypename)
                return new StringContentDefinition();

            if (IContentDefinition.DefinitionType.LongString.ToString().ToUpperInvariant() == cleartypename)
                return new StringContentDefinition();

            if (IContentDefinition.DefinitionType.Folder.ToString().ToUpperInvariant() == cleartypename)
                return new FolderContentDefinition();

            throw new NotSupportedException();
        }
    }
}
