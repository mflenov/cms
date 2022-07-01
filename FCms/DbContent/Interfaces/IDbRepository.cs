using FCms.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCms.DbContent
{
    public interface IDbRepository : IRepository
    {
        public string TableName { get; }
        void Scaffold();
    }
}
