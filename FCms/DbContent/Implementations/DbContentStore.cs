using System;
using System.Linq;
using System.Collections.Generic;
using FCms.DbContent.Db;
using FCms.DbContent.Models;
using System.Threading.Tasks;

namespace FCms.DbContent
{
    public class DbContentStore
    {
        IDatabase database;
        IDbRepository repository;

        public DbContentStore(IDbRepository repository)
        {
            this.repository = repository;
            database = new MsSqlDatabase();
        }

        public DbContentModel GetContent()
        {
            return new DbContentModel() { 
              //  Values = database.GetContent(repository.TableName)
            };
        }

        public Task<int> Add(List<object> values)
        {
            return database.AddRow(repository.TableName, values, repository.ContentDefinitions.Select(m => new Models.ColumnModel(m)).ToList());
        }
    }
}
