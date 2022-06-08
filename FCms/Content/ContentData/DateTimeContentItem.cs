using System;

namespace FCms.Content
{
    public class DateTimeContentItem: ContentItem
    {
        public DateTime Data { get; set; }

        public override object GetValue()
        {
            return Data;
        }

        public override string GetHtmlString()
        {
            return Data.ToString();
        }

        public override string GetTypeName()
        {
            return "String";
        }
    }
}