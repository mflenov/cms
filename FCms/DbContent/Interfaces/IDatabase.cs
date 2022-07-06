using FCms.DbContent.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCms.DbContent
{
    internal interface IDatabase
    {
        IEnumerable<Models.DbTableModel> GetTables();

        void CreateTable(string tableName);

        void CreateColumns(string tableName, IEnumerable<ColumnModel> columns);

        Task<int> AddRow(string tableName, List<object> values, List<ColumnModel> columns);
    }
}
