using System;
using System.Collections.Generic;
using System.Text;

namespace FCms.Content
{
    public interface IContentDefinition
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        public enum DefinitionType { String, Folder };

        Guid DefinitionId { get; set; }

        string Name { get; set; }

        string GetTypeName();
    }
}
