using System.Collections.Generic;

namespace FCms.DbContent.Models
{
    public class ContentModel
    {
        List<string> ColumNames { get; set; } = new List<string>();
        List<List<string>> Values { get; set; } = new List<List<string>>();
    }
}
