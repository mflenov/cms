using FCms.DbContent;
using System.Collections.Generic;

namespace FCms.Content
{
    public class CmsData
    {
        List<IRepository> repositories = new List<IRepository>();
        public List<IRepository> Repositories {
            get {
                return this.repositories;
            }
        }

        List<IDbRepository> dbrepositories = new List<IDbRepository>();
        public List<IDbRepository> DbRepositories
        {
            get
            {
                return this.dbrepositories;
            }
        }

        List<IFilter> filters = new List<IFilter>();
        public List<IFilter> Filters {
            get {
                return filters;
            }
        }

        List<IDbConnection> dbConnections = new List<IDbConnection>();
        public List<IDbConnection> DbConnections {
            get {
                return dbConnections;
            }
        }
    }
}
