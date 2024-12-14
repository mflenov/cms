using FCms.Content;
using FCms.DbContent.Db;
using System.Threading.Tasks;
using System.Linq;

namespace FCms.DbContent
{
    public class DbRepository : Repository, IDbRepository
    {
        public DbRepository() {
        }

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

        private IDbConnection databaseConnection = null;
        public IDbConnection GetDatabaseConnection() {
            if (databaseConnection == null) {
                databaseConnection = CmsManager.GetInstance().Data.DbConnections.Where(m => m.Id == DatabaseConnectionId).FirstOrDefault();
            }
            return databaseConnection;
        }
        
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
