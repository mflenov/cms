using FCms.DbContent.Models;
using System.Collections.Generic;

namespace FCmsManagerAngular.ViewModels
{
    public class DbContentListViewModel
    {
        public IEnumerable<ContentColumn> Columns { get; set; } = new List<ContentColumn>();
        public List<ContentRow> Rows { get; set; } = new List<ContentRow>();
    }
}
