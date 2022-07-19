using System.Linq;
using System.Collections.Generic;

namespace FCms.DbContent.Models
{
    class ContentSearchModel
    {
        public ContentSearchModel()
        {

        }
        public void MapRequest(ContentSearchRequest request, IDbRepository repository)
        {
            this.Top = request.Top;
            this.Columns = request.Columns;
            if (request.Filters != null)
                Filters = request.Filters.Select(m => new ContentFiltersModel()
                {
                    Column = new ColumnModel(repository.ContentDefinitions.First(m => m.Name == m.Name)),
                    Value = m.Value,
                    ExactMatch = m.ExactMatch
                }).ToList();
        }

        public int Top { get; set; } = 10;

        public List<string> Columns { get; set; } = new List<string>();

        public List<ContentFiltersModel> Filters { get; set; } = new List<ContentFiltersModel>();
    }
}
