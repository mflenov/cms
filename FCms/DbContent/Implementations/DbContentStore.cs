using System;
using System.Collections.Generic;
using FCms.DbContent.Models;

namespace FCms.DbContent
{
    public class DbContentStore
    {
        Content.IRepository repository;

        public DbContentStore(Content.IRepository repository)
        {
            this.repository = repository;
        }

        public DbContentModel GetContent()
        {


            return new DbContentModel() { };
        }
    }
}
