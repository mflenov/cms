using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using FCms.Content;
using FCmsManagerAngular.ViewModels;

namespace FCmsManagerAngular.Controllers
{
    [ApiController]
    public class PagesControllers
    {
        IConfiguration config;

        public PagesControllers(IConfiguration config) {
            this.config = config;
        }

        [HttpGet]
        [Route("api/v1/pages")]
        public IEnumerable<PageViewModel> Index()
        {
            var maanger = new CmsManager(config["DataLocation"]);

            foreach (IRepository repository in maanger.Data.Repositories.Where(m => m.ContentType == ContentType.Page))
            {
                yield return new PageViewModel(){
                    Id = repository.Id,
                    Name = repository.Name
                };
            }
        }
    }
}
