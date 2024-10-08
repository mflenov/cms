﻿using FCms.Content;

namespace FCms.Factory
{
    public static class ContentFactory
    {
        public static ContentItem CreateContentByType(IContentDefinition definition)
        {
            if (definition is DateContentDefinition)
                return new DateContentItem();
            if (definition is DateTimeContentDefinition)
                return new DateTimeContentItem();
            if (definition is StringContentDefinition)
                return new StringContentItem();

            if (definition is FolderContentDefinition)
                return new ContentFolderItem();

            return new StringContentItem();
        }


        public static IContentDefinition CreateContentDefinition(string typeName)
        {
            if (typeName == null) {
                return new StringContentDefinition();
            }

            if (typeName.ToUpperInvariant() == ContentDefinitionType.Date.ToString().ToUpperInvariant())
            {
                return new DateContentDefinition();
            }
            if (typeName.ToUpperInvariant() == ContentDefinitionType.DateTime.ToString().ToUpperInvariant())
            {
                return new DateTimeContentDefinition();
            }
            if (typeName.ToUpperInvariant() == ContentDefinitionType.String.ToString().ToUpperInvariant()) {
                return new StringContentDefinition();
            }
            if (typeName.ToUpperInvariant() == ContentDefinitionType.LongString.ToString().ToUpperInvariant()) {
                return new LongStringContentDefinition();
            }
            if (typeName.ToUpperInvariant() == ContentDefinitionType.Folder.ToString().ToUpperInvariant()) {
                return new FolderContentDefinition();
            }
            throw new System.Exception("Not supported type");
        }
    }
}
