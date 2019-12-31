namespace FCms.Content
{
    public class StringContentItem: ContentItem
    {
        public string Data { get; set; }

        public override object GetValue()
        {
            return Data;
        }

        public override string GetHtmlString()
        {
            return Data;
        }
    }
}
