using FCms.DbContent.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCms.Content
{
    public class DbRepository: Repository, IDbRepository
    {
        string tableName = null;
        public string TableName
        {
            get { 
                if (tableName == null)
                {
                    tableName = DbHelpers.SanitizeDbName(Name);
                }
                return tableName;
            }
        }

        public void Scaffold()
        {
            if (ContentType == ContentType.DbContent)
            {
                FCms.DbContent.DbScaffold scaffold = new FCms.DbContent.DbScaffold();
                scaffold.ScaffoldRepository(this);
            }

        }
    }
}
