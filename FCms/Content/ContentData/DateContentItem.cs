using System;

namespace FCms.Content
{
    public class DateContentItem : ContentItem
    {
        public DateTime Data { get; set; }

        public override object GetValue()
        {
            return Data.Date;
        }

        public override string GetHtmlString()
        {
            return Data.Date.ToString();
        }

        public override string GetTypeName()
        {
            return "String";
        }
    }
}
