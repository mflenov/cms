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

        public async Task<ContentModel> GetContent(ContentSearchRequest request)
        {
            SqlGenerator queryGenerator = database.GetSqlGenerator(repository.TableName);
            var searchModel = new ContentSearchModel();
            searchModel.MapRequest(request, this.repository);
            var query = queryGenerator.GetSearchQuery(searchModel);

            return await database.GetContent(repository.TableName, query);
        }

        public Task<int> Add(List<object> values)
        {
            return database.AddRow(repository.TableName, values, repository.ContentDefinitions.Select(m => new ColumnModel(m)).ToList());
        }
    }
}
