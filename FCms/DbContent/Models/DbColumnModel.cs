using System;
using FCms.Content;
using FCms.DbContent.Db;
using static FCms.Content.IContentDefinition;

namespace FCms.DbContent.Models
{
    class DbColumnModel
    {
        public DbColumnModel(IContentDefinition definition)
        {
            Name = DbHelpers.SanitizeDbName(definition.Name);
            if (definition.GetDefinitionType() == ContentDefinitionType.String)
            {

            }
        }

        public string Name { get; set; }
        
        public Type DataType { get; set; }
    }
}
