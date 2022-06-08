using System;

namespace FCms.Content
{
#pragma warning disable CA1720 // Identifier contains type name
    public enum ContentDefinitionType { LongString, String, Folder, DateTime, Date };
#pragma warning restore CA1720 // Identifier contains type name

    public interface IContentDefinition
    {
        Guid DefinitionId { get; set; }

        string Name { get; set; }

        string GetTypeName();

        ContentDefinitionType GetDefinitionType();
    }
}
