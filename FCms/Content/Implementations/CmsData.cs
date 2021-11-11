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


        List<IFilter> filters = new List<IFilter>();
        public List<IFilter> Filters {
            get {
                return filters;
            }
        }
    }
}
