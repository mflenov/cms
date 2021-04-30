using System;

namespace FCms.Content
{
#pragma warning disable CA1720 // Identifier contains type name
    public enum ContentDefinitionType { LongString, String, Folder };
#pragma warning restore CA1720 // Identifier contains type name

    public interface IContentDefinition
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        public enum DefinitionType { String, Folder, LongString };

        Guid DefinitionId { get; set; }

        string Name { get; set; }

        string GetTypeName();

        ContentDefinitionType GetDefinitionType();
    }
}
