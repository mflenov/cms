using System.Collections.Generic;

namespace FCms.DbContent.Interfaces
{
    internal interface IDatabase
    {
        public IEnumerable<Models.DbTableModel> GetTables();

        public void CreateTable(string tableName);
    }
}
