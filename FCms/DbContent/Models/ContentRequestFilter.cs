using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCms.DbContent.Models
{
    public class ContentRequestFilter
    {
        public string ColumnName { get; set; }
        public string Value { get; set; }
        public bool ExactMatch { get; set; }
    }
}
