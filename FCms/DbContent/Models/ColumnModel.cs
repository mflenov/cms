using System;
using FCms.Content;
using FCms.DbContent.Db;
using static FCms.Content.IContentDefinition;

namespace FCms.DbContent.Models
{
    class ColumnModel
    {
        public ColumnModel(IContentDefinition definition)
        {
            Name = DbHelpers.SanitizeDbName(definition.Name);
            DataType = definition.GetDefinitionType();
        }

        public string Name { get; set; }
        
        public ContentDefinitionType DataType { get; set; }

        public string GetDbTypeName()
        {
            switch (DataType)
            {
                case ContentDefinitionType.Date:
                    return "date";
                case ContentDefinitionType.DateTime:
                    return "datetime";
                case ContentDefinitionType.String: 
                    return "nvarchar(255)";
                case ContentDefinitionType.LongString:
                    return "ntext";
            }
            return "";
        }
    }
}
