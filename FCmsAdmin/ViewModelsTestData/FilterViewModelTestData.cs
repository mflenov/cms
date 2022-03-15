using System;
using System.Linq;
using System.Collections.Generic;
using FCmsManagerAngular.ViewModels;

namespace FCmsManagerAngular.ViewModelsTestData
{
    public class FilterViewModelTestData
    {
        private static List<FilterViewModel> testdata =  new List<FilterViewModel>()
            {
                new FilterViewModel() {  Id = Guid.Parse("c7f1caed-be45-4a9c-9996-04f676148801"), Name = "IsActive", Type = FCms.Content.IFilter.FilterType.Boolean.ToString() },
                new FilterViewModel() {  Id = Guid.Parse("4a1edd40-c264-4f8b-b493-dfb7390493cb"), Name = "DateRange", Type = FCms.Content.IFilter.FilterType.DateRange.ToString() },
                new FilterViewModel() {  Id = Guid.Parse("05db023b-61cb-484e-acc0-66d3661d655c"), Name = "Name", Type = FCms.Content.IFilter.FilterType.Text.ToString() },
            };

        public IEnumerable<FilterViewModel> GetTestData() {
            return testdata;
        }

        public void AddTestData(FilterViewModel model) {
            testdata.Add(model);
        }

        public void DeleteTestData(Guid id) {
            testdata.Remove(testdata.FirstOrDefault(m => m.Id == id));
        }

        public void UpdateTestData(FilterViewModel model)
        {
            if (model.Id != null) {
                DeleteTestData(model.Id.Value);
            }
            else
            {
                model.Id = Guid.NewGuid();
            }
            AddTestData(model);
        }
    }
}
