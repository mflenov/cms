using System.Collections.Generic;

namespace FCms.DbContent.Models
{
    class ContentSearchModel
    {
        public int Top { get; set; } = 10;

        public List<string> Columns { get; set; } = new List<string>();

        public List<ContentFiltersModel> Filters { get; set; } = new List<ContentFiltersModel>();
    }
}
