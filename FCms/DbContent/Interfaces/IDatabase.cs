using FCms.DbContent.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCms.DbContent
{
    internal interface IDatabase
    {
        public Db.SqlGenerator GetSqlGenerator(string tablename);

        Task<IEnumerable<Models.DbTableModel>> GetTables();

        Task<bool> CreateTable(string tableName);

        Task<bool> CreateColumns(string tableName, IEnumerable<ColumnModel> columns);

        Task<int> AddRow(string tableName, List<object> values, List<ColumnModel> columns);

        Task<ContentModel> GetContent(string tableName, SqlQueryModel query);

        Task DeleteRow(string tableName, string id);
    }
}
