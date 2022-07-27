using System.Collections.Generic;

namespace FCms.DbContent.Models
{
    public class ContentModel
    {
        public List<ContentColumn> Columns { get; set; } = new List<ContentColumn>();
        public List<ContentRow> Rows { get; set; } = new List<ContentRow>();
    }
}
