﻿using System;
using System.Collections.Generic;

namespace FCms.Content
{
    public interface ICmsManager
    {
        public string Filename {
            get;
        }

        public CmsData Data {
            get;
        }

        void Save();

        IRepository GetRepositoryByName(string name);

        IRepository GetRepositoryById(Guid repositoryid);

        void DeleteRepository(Guid repositoryid);

        int GetIndexById(Guid id);

        void AddRepository(IRepository repository);
    }
}
