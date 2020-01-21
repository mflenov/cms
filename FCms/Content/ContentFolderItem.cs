using System.Collections.Generic;
using System.Text;
using System.Linq;
using System; 

namespace FCms.Content
{
    public class ContentFolderItem: ContentItem
    {
        public string Name { get; set; }

        public List<IContent> Childeren { get; } = new List<IContent>();

        public override object GetValue()
        {
            return Childeren;
        }

        public FolderContentDefinition Definition { get; set; }

        public IContent GetItem(Guid? id)
        {
            if (id == null)
            {
                return null;
            }
            return Childeren.Where(m => m.DefinitionId == id).FirstOrDefault();
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
