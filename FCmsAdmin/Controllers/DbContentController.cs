using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FCmsManagerAngular.Controllers
{
    [ApiController]
    public class DbContentController : ControllerBase
    {
        /*
        [HttpPost]
        [Route("api/v1/db")]
        public IEnumerable<PageViewModel> Index()
        {
            var manager = new CmsManager();

            foreach (IDbRepositories repository in manager.Data.Repositories.Where(m => m.ContentType == ContentType.DbContent))
            {
                yield return new PageViewModel()
                {
                    Id = repository.Id,
                    Name = repository.Name
                };
            }
        }
        */
    }
}
