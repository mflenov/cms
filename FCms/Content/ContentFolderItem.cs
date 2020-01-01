using System.Collections.Generic;
using System.Text;

namespace FCms.Content
{
    public class ContentFolderItem: ContentItem
    {
        public List<IContent> Childeren { get; } = new List<IContent>();

        public override object GetValue()
        {
            return Childeren;
        }

        public override string GetHtmlString()
        {
            StringBuilder result = new StringBuilder();
            foreach (var child in Childeren)
            {
                result.Append("<div>" + child.GetHtmlString() + "</div>");
            }
            return result.ToString();
        }

        public override string GetTypeName()
        {
            return "Folder";
        }

    }
}
