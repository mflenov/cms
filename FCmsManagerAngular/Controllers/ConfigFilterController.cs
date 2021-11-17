using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using FCmsManagerAngular.ViewModels;
using Microsoft.Extensions.Configuration;
using FCms.Content;

namespace FCmsManagerAngular.Controllers
{
    [ApiController]
    public class ConfigFiltersController: ControllerBase
    {
        IConfiguration config;

        public ConfigFiltersController(IConfiguration config)
        {
            this.config = config;
        }

        ViewModelsTestData.FilterViewModelTestData model = new ViewModelsTestData.FilterViewModelTestData();
        [Route("api/v1/config/filters")]
        public IEnumerable<FilterViewModel> Filters()
        {
            var maanger = new CmsManager();
            return model.GetTestData();
        }

        [HttpGet]
        [Route("api/v1/config/filter/{id}")]
        public FilterViewModel Get(string id)
        {
            Guid guid;
            if (Guid.TryParse(id, out guid)) {
                return model.GetTestData().FirstOrDefault(n => n.Id == guid);
            }
            return null;
        }

        [HttpPost]
        [Route("api/v1/config/filter")]
        public void Post(FilterViewModel data)
        {
            data.Id = Guid.NewGuid();
            model.AddTestData(data);
        }

        [HttpPut]
        [Route("api/v1/config/filter")]
        public void Put(FilterViewModel data)
        {
            model.UpdateTestData(data);
        }
    }
}
