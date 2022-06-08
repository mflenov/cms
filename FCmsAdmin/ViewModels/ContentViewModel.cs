using System;
using System.Linq;
using System.Collections.Generic;
using FCms.Content;
using System.Text.Json;

namespace FCmsManagerAngular.ViewModels {

    public class ContentViewModel
    {
        public Guid? Id { get; set; }

        public Guid DefinitionId { get; set; }

        public string ToolTip { get; set; }

        public object Data { get; set; }

        public bool IsDeleted { get; set; }

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
                MapFolder(model as ContentFolderItem, contentDefinition);
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
            if (model.Id == null || model.Id == Guid.Empty)
                model.Id = Guid.NewGuid();
            model.DefinitionId = DefinitionId;

            if (model is StringContentItem)
                (model as StringContentItem).Data = Data.ToString();
            if (model is DateContentItem)
                (model as DateContentItem).Data = FCms.Tools.Utility.StringToDateTime(Data.ToString()) ?? DateTime.Today;
            if (model is DateTimeContentItem)
                (model as DateTimeContentItem).Data = FCms.Tools.Utility.StringToDateTime(Data.ToString()) ?? DateTime.Today;
        }

        private void MapFolder(ContentFolderItem model, IContentDefinition contentDefinition) {
            model.Childeren.Clear();
            model.DefinitionId = this.DefinitionId;
            model.Id = this.Id ?? Guid.NewGuid();

            foreach (var definition in (contentDefinition as FolderContentDefinition).Definitions)
            {
                ContentViewModel content = this.Children.Where(m => m.DefinitionId == definition.DefinitionId).FirstOrDefault();
                if (content == null) {
                    continue;
                }
                if (definition is StringContentDefinition)
                {
                    model.Childeren.Add(new StringContentItem() {
                        DefinitionId = definition.DefinitionId,
                        Data = content.Data.ToString(),
                        Id = content.Id ?? Guid.NewGuid()
                    }
                    );
                }
            }
        }
    }
}