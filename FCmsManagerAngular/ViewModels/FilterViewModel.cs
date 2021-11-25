using System;
using System.Linq;
using System.Collections.Generic;
using FCms.Content;

namespace FCmsManagerAngular.ViewModels
{
    public class FilterViewModel
    {
        public FilterViewModel()  {
        }

        public FilterViewModel(IFilter filter)  {
            Id = filter.Id;
            Name = filter.Name;
            Type = filter.Type;
        }

        public Guid? Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }
        
        public List<string> Values { get; set; }

        public void Add(ICmsManager manager) {
            IFilter filtermodel = FCms.Factory.FilterFactory.CreateFilterByType((IFilter.FilterType)Enum.Parse(typeof(IFilter.FilterType), Type));
            manager.Data.Filters.Add(MapToModel(filtermodel));
            manager.Save(); 
        }

        public void Update(ICmsManager manager) {
            int repoindex = manager.Data.Filters.Select((v, i) => new { filter = v, Index = i }).FirstOrDefault(x => x.filter.Id == Id)?.Index ?? -1;
            if (repoindex < 0)
            {
                throw new Exception("The filter definition not found");
            }
            this.MapToModel(manager.Data.Filters[repoindex]);
            manager.Save();
        }

        public IFilter MapToModel(IFilter model)
        {
            model.Name = this.Name;
            model.Id = this.Id ?? Guid.NewGuid();
            return model;
        }
    }
}
