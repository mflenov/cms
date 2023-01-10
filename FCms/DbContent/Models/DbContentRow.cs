using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCms.DbContent.Models
{
    public class ContentRow
    {
        private List<object> values = new List<object>();
        public List<object> Values { get { return values; } set { values = value; } }

        public ContentRow()
        {

        }
        public ContentRow(List<object> values)
        {
            this.values = values;
        }

        public string GetStringValue(int i)
        {
            if (i < 0 || i >= values.Count())
                throw new IndexOutOfRangeException();
            return values[i].ToString();
        }
    }
}
