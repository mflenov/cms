using FCms.Content;
using FCms.DbContent.Db;
using System.Threading.Tasks;

namespace FCms.DbContent
{
    public class DbRepository : Repository, IDbRepository
    {
        string tableName = null;
        public string TableName
        {
            get
            {
                if (tableName == null)
                {
                    tableName = DbHelpers.SanitizeDbName(Name);
                }
                return tableName;
            }
        }

        public System.Guid DatabaseConnectionId { get; set; }

        public IDbConnection DatabaseConnection { get; set; }
        
        public async Task<bool> Scaffold()
        {
            if (ContentType == ContentType.DbContent)
            {
                DbScaffold scaffold = new DbScaffold();
                return await scaffold.ScaffoldRepository(this);
            }
            return false;
        }
    }
}
