using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCms.DbContent.Models
{
    public class ContentSearchRequest
    {
        public int Top { get; set; } = 10;

        public List<string> Columns { get; set; } = new List<string>();

        public List<ContentRequestFilter> Filters { get; set; } = new List<ContentRequestFilter>();
    }
}
