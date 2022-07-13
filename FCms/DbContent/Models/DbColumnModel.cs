using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCms.DbContent.Models
{
    class DbColumnModel
    {
        public enum DbType { IntValue = 0, NvarcharValue = 1 }

        public string ColumnName { get; set; }

        public DbType DatabaseType { get; set; }
    }
}
