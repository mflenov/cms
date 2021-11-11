using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FCmsManager.ViewModel;
using FCms.Content;
using Microsoft.AspNetCore.Authorization;

namespace FCmsManager.Areas.FCmsManager.Controllers
{
    [Area("fcmsmanager")]
    [Authorize(AuthenticationSchemes = "fcms")]
    public class FilterController : Controller
    {
        [HttpGet("fcmsmanager/filter", Name = "fcmsfilter")]
        public IActionResult Index()
        {
            var maanger = new CmsManager();
            return View("Index", maanger.Data.Filters);
        }

        [HttpGet("fcmsmanager/filter/add", Name = "fcmsfilteradd")]
        public IActionResult Add()
        {
            return View("Edit", new EditFilterViewModel());
        }

        [HttpPost("fcmsmanager/filter/save", Name = "fcmsfilteraddsave")]
        public IActionResult Save(EditFilterViewModel model)
        {
            if (ModelState.IsValid)
            {
                ICmsManager manager = new CmsManager();
                if (model.Id == null)
                {
                    IFilter filtermodel = FCms.Factory.FilterFactory.CreateFilterByType((IFilter.FilterType)Enum.Parse(typeof(IFilter.FilterType), model.Type));
                    manager.Data.Filters.Add(model.MapToModel(filtermodel));
                }
                else
                {
                    int repoindex = manager.Data.Filters.Select((v, i) => new { filter = v, Index = i }).FirstOrDefault(x => x.filter.Id == model.Id)?.Index ?? -1;
                    if (repoindex < 0)
                    {
                        throw new Exception("The filter definition not found");
                    }
                    model.MapToModel(manager.Data.Filters[repoindex]);
                }
                manager.Save();
                return Redirect("/fcmsmanager/filter");
            }

            return View("Add", new EditFilterViewModel());
        }
    }
}
