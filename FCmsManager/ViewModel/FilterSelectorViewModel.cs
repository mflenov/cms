using System;
using System.Collections.Generic;
using FCms.Content;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FCmsManager.ViewModel
{
    public class FilterSelectorViewModel
    {
        ICmsManager manager = new CmsManager();

        public FilterSelectorViewModel()
        {
        }

        #region form info

        public string RepositoryName { get; set; }

		public IEnumerable<SelectListItem> GlobalFilters {
            get {
                foreach (var filter in manager.Data.Filters)
                {
                    yield return new SelectListItem { Text = filter.Name, Value = filter.Id.ToString() };
                }
            }
        }

        #endregion

        #region data

        public Guid RepositoryId { get; set; }

        #endregion

    }
}
