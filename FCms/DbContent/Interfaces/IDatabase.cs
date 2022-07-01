using FCms.DbContent.Models;
using System.Collections.Generic;

namespace FCms.DbContent
{
    internal interface IDatabase
    {
        IEnumerable<Models.DbTableModel> GetTables();

        void CreateTable(string tableName);

        void CreateColumns(string tableName, IEnumerable<ColumnModel> columns);
    }
}
