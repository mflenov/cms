using System.Collections.Generic;

namespace FCmsManagerAngular.ViewModels
{
    public class PagePreviewItemViewModel
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public List<PagePreviewItemViewModel> Children { get; set; } = new List<PagePreviewItemViewModel>();
    }

    public class PagePreviewViewModel
    {
        public List<PagePreviewItemViewModel> ContentItems { get; set; } = new List<PagePreviewItemViewModel>();
    }
}
