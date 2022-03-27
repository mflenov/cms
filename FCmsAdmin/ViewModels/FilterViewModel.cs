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
            DisplayName = filter.DisplayName;
            Type = filter.Type;
            this.Values = (filter as ValueListFilter)?.Values ?? new List<string>();
        }

        public Guid? Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

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
            if (manager.Data.Filters[repoindex].Type != this.Type) {
                manager.Data.Filters[repoindex] = FCms.Factory.FilterFactory.CreateFilterByType((IFilter.FilterType)Enum.Parse(typeof(IFilter.FilterType), Type));
            }

            this.MapToModel(manager.Data.Filters[repoindex]);
            manager.Save();
        }

        public IFilter MapToModel(IFilter model)
        {
            model.Name = this.Name;
            model.DisplayName = this.DisplayName;
            model.Id = this.Id ?? Guid.NewGuid();
            if (model is ValueListFilter){
                (model as ValueListFilter).Values  = this.Values;
            }
            return model;
        }
    }
}
