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

        public IEnumerable<ContentViewModel> Children { get; set; } = new List<ContentViewModel>();

        public bool IsFolder { get; set; }

        public ContentViewModel()
        {

        }

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
                FilterType = m.FilterType.ToString(),
                DataType = m.Filter.Type,
                Values = m.Values
            });
        }

        public void MapToModel(ContentItem model, IContentDefinition contentDefinition)
        {
            // map content
            if (model is ContentFolderItem)
                MapFolder(model, contentDefinition);
            else
                MapScalar(model);

            // map filters
            model.Filters.Clear();
            model.Filters.AddRange(this.GetFilters());
        }

        public List<IContentFilter> GetFilters()
        {
            List<IContentFilter> result = new List<IContentFilter>();
            int index = 0;
            foreach (var filter in Filters)
            {
                IFilter ifilter = FCms.Factory.FilterFactory.CreateFilterByTypeName(filter.DataType);
                ifilter.Id = filter.FilterDefinitionId;

                ContentFilter contentFilter = new ContentFilter()
                {
                    Filter = ifilter,
                    FilterType = (IContentFilter.ContentFilterType)Enum.Parse(typeof(IContentFilter.ContentFilterType), filter.FilterType),
                    FilterDefinitionId = ifilter.Id,
                    Index = index
                };
                contentFilter.Values.AddRange(
                    ifilter.ParseValues(filter.Values.Select(m => m.ToString()).ToList())
                    );
                result.Add(contentFilter);
                index++;
            }
            return result;
        }
        private void MapScalar(ContentItem model)
        {
            if (model is StringContentItem)
            {
                (model as StringContentItem).Data = Data.ToString();
                (model as StringContentItem).DefinitionId = DefinitionId;
            }
        }

        private  void MapFolder(ContentItem model, IContentDefinition contentDefinition) {
        }
    }
}