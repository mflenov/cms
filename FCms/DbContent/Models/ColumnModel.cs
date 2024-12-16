using System;
using System.Data;
using FCms.Content;
using FCms.DbContent.Db;

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

        public string GetMsDbTypeName()
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

        public string GetPgDbTypeName() {
            switch (DataType)
            {
                case ContentDefinitionType.Date:
                    return "date";
                case ContentDefinitionType.DateTime:
                    return "timestamp";
                case ContentDefinitionType.String: 
                    return "varchar(255)";
                case ContentDefinitionType.LongString:
                    return "text";
            }
            return "";            
        }

        public SqlDbType GetSqlDbTypeName()
        {
            switch (DataType)
            {
                case ContentDefinitionType.Date:
                    return SqlDbType.Date;
                case ContentDefinitionType.DateTime:
                    return SqlDbType.DateTime;
                case ContentDefinitionType.String:
                    return SqlDbType.NVarChar;
                case ContentDefinitionType.LongString:
                    return SqlDbType.NText;
            }
            return SqlDbType.NVarChar;
        }

        public Object ParseValue(Object o) {
            switch (DataType)
            {
                case ContentDefinitionType.Date:
                case ContentDefinitionType.DateTime:
                    return (DateTime)o;
            }
            return o.ToString();
        }
    }
}
