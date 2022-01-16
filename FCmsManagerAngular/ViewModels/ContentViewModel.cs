using System;
using System.Linq;
using System.Collections.Generic;
using FCms.Content;

namespace FCmsManagerAngular.ViewModels {

    public class ContentViewModel
    {
        public Guid? Id { get; set; }

        public Guid DefinitionId { get; set; }

        public string ToolTip { get; set; }

        public object Data { get; set; }

        public IEnumerable<ContentFilterViewModel> Filters { get; set; } = new List<ContentFilterViewModel>();

        public IEnumerable<ContentViewModel> Children { get; set; }

        public bool IsFolder { get; set; }

        public ContentViewModel(IContent contentItem)
        {
            Id = contentItem.Id;
            DefinitionId = contentItem.DefinitionId;
            ToolTip = contentItem.ToolTip;
            Data = contentItem is ContentFolderItem ? null : contentItem.GetValue();
            Children = contentItem is ContentFolderItem ? (contentItem as ContentFolderItem).Childeren.Select(n => new ContentViewModel(n)) : null;
            IsFolder = contentItem is ContentFolderItem;
            Filters = contentItem.Filters.Select(m => new ContentFilterViewModel() {
                FilterDefinitionId = m.FilterDefinitionId,
                FilterType = m.FilterType,
                Values = m.Values
            });
        }
    }
}