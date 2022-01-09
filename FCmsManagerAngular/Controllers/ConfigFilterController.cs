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
        
        [Route("api/v1/config/filters")]
        public ApiResultModel Filters()
        {
            var manager = new CmsManager(config["DataLocation"]);

            return new ApiResultModel(ApiResultModel.SUCCESS) {
                 Data = manager.Data.Filters.Select(m => new FilterViewModel(m))
            };
        }

        [HttpGet]
        [Route("api/v1/config/filter/{id}")]
        public FilterViewModel Get(string id)
        {
            var manager = new CmsManager(config["DataLocation"]);
            Guid guid;
            if (Guid.TryParse(id, out guid)) {
                return manager.Data.Filters.Where(n => n.Id == guid).Select(m => new FilterViewModel(m)).FirstOrDefault();
            }
            return null;
        }

        [HttpPost]
        [Route("api/v1/config/filter")]
        public void Post(FilterViewModel model)
        {
            ICmsManager manager = new CmsManager(config["DataLocation"]);
            model.Add(manager);
        }

        [HttpPut]
        [Route("api/v1/config/filter")]
        public void Put(FilterViewModel model)
        {
            ICmsManager manager = new CmsManager(config["DataLocation"]);
            model.Update(manager);
        }

        [HttpDelete]
        [Route("api/v1/config/filter/{id}")]
        public ApiResultModel Delete(string id)
        {
            var manager = new CmsManager(config["DataLocation"]);
            Guid guid;
            if (Guid.TryParse(id, out guid)) {
                var filter = manager.Data.Filters.Where(n => n.Id == guid).FirstOrDefault();
                if (filter != null) {
                    manager.Data.Filters.Remove(filter);
                    manager.Save();
                    return new ApiResultModel(ApiResultModel.SUCCESS);
                }
            }
            return new ApiResultModel(ApiResultModel.NOT_FOUND);
        }
    }
}
