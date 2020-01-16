using System;
using System.Collections.Generic;

namespace FCms.Content
{
    public interface ICmsManager
    {
        public List<IRepository> Repositories {
            get;
        }

        public List<IFilter> Filters {
            get;
        }

        void Save();

        IRepository GetRepositoryByName(string name);

        IRepository GetRepositoryById(Guid repositoryid);

        IContentStore GetContentStore(Guid repositoryid);

        int GetIndexById(Guid id);

        void AddRepository(IRepository repository);
    }
}
