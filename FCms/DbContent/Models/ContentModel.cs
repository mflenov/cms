using System.Collections.Generic;

namespace FCms.DbContent.Models
{
    public class ContentModel
    {
        public List<string> ColumNames { get; set; } = new List<string>();
        public List<DbContentRow> Rows { get; set; } = new List<DbContentRow>();
    }
}
