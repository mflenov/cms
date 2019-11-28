using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FCmsManager.ViewModel;
using FCms.Content;

namespace FCmsManager.Areas.FCmsManager.Controllers
{
    [Area("fcmsmanager")]
    public class FilterController : Controller
    {
        [HttpGet("fcmsmanager/filter", Name = "fcmsfilter")]
        public IActionResult Index()
        {
            var maanger = CmsManager.Load();
            return View("Index", maanger.Filters);
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
                ICmsManager manager = CmsManager.Load();
                if (model.Id == null)
                {
                    IFilter filtermodel = FCms.Factory.FilterFactory.CreateFilterByType((IFilter.FilterType)Enum.Parse(typeof(IFilter.FilterType), model.Type));
                    manager.Filters.Add(model.MapToModel(filtermodel));
                }
                else
                {
                    int repoindex = manager.Filters.Select((v, i) => new { filter = v, Index = i }).FirstOrDefault(x => x.filter.Id == model.Id)?.Index ?? -1;
                    if (repoindex < 0)
                    {
                        throw new Exception("The filter definition not found");
                    }
                    model.MapToModel(manager.Filters[repoindex]);
                }
                manager.Save();
                return Redirect("/fcmsmanager/filter");
            }

            return View("Add", new EditFilterViewModel());
        }
    }
}
