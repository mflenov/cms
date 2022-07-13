using System.Collections.Generic;

namespace FCms.DbContent.Models
{
    class ContentSearchModel
    {
        public int? Top { get; set; }

        public List<string> Columns { get; set; }

        public List<ContentFiltersModel> Filters { get; set; }
    }
}
