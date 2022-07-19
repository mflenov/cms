using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCms.DbContent.Models
{
    public class DbContentRow
    {
        private List<object> columns = new List<object>();

        public DbContentRow()
        {

        }
        public DbContentRow(List<object> columns)
        {
            this.columns = columns;
        }

        public string GetStringValue(int i)
        {
            if (i < 0 || i >= columns.Count())
                throw new IndexOutOfRangeException();
            return columns[i].ToString();
        }
    }
}
