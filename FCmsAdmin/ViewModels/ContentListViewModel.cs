using System.Collections.Generic;
using FCms.Content;

namespace FCmsManagerAngular.ViewModels
{
    public class ContentListViewModel
    {
        public string RepositoryName { get; set; }
		public ContentDefinitionViewModel Definition { get; set; }
        public IEnumerable<ContentViewModel> ContentItems { get; set; }
    }
}
