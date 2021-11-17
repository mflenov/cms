using System;
using System.Linq;
using System.Collections.Generic;

namespace FCmsManagerAngular.ViewModels
{
    public class FilterViewModel
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }


        public static IEnumerable<FilterViewModel> MapFromCmaFilter(List<FCms.Content.IFilter> filters)
        {
            return filters.Select(m =>
                new FilterViewModel()
                {
                    Id = m.Id,
                    Name = m.Name,
                    Type = m.Type
                }
           );
        }
    }
}
