﻿using System;
using System.Collections.Generic;
using System.Linq;
using FCms.Content;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FCmsManager.ViewModel
{
    public class ContentEditorViewModel
    {
        ICmsManager manager = CmsManager.Load();

        public ContentEditorViewModel()
        {
        }

		#region form info

        public IEnumerable<SelectListItem> GlobalFilters {
            get {
                foreach (var filter in manager.Filters)
                {
                    yield return new SelectListItem { Text = filter.Name, Value = filter.Id.ToString() };
                }
            }
        }

        public IEnumerable<FilterValueViewModel> ContentFilters {
            get {
                int index = 1;
                foreach (ContentFilter filter in this.Filters)
                {
                    yield return new FilterValueViewModel()
                    {
                        ContentFilter = filter,
                        FilterDefinition = manager.Filters.Where(m => m.Id == filter.FilterDefinitionId).FirstOrDefault(),
                        Index = index
                    };
                    index++;
                }
            }
        }
		public string RepositoryName { get; set; }

        public List<IContentDefinition> ContentDefinitions { get; set; }

        public List<ContentItem> ContentItems { get; set; }


        #endregion

        #region data

		public Guid RepositoryId { get; set; }

        public List<IContentFilter> Filters { get; set; } = new List<IContentFilter>();

		#endregion
    }
}
