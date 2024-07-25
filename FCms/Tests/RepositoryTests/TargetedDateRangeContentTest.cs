using System;
using System.Linq;
using Xunit;
using FCms.Content;
using FCmsTests.Helpers;
using FCms;
using System.Collections.Generic;

namespace FCmsTests
{
    [Collection("Sequential")]
    public class TargetedDateRangeContentTest: IDisposable
    {
        const string repositoryName = "TestRepository";
        const string contentName = "Title";
        Guid repositoryId = Guid.NewGuid();
        Guid definitionId = Guid.NewGuid();
        Guid datetimeFilterId = Guid.NewGuid();
        ICmsManager manager;
        IContentStore contentStore;

        public TargetedDateRangeContentTest()
        {
            TestTools.DeleteCmsFile();

            manager = new CmsManager();
            Repository repository = new Repository() { Id = repositoryId, Name = repositoryName };
            IContentDefinition definition = ContentDefinitionFactory.CreateContentDefinition(ContentDefinitionType.String);
            definition.DefinitionId = definitionId;
            definition.Name = contentName;
            repository.ContentDefinitions.Add(definition);
            manager.Data.Repositories.Add(repository);

            // filters
            manager.Data.Filters.Add(new DateRangeFilter() { Id = datetimeFilterId, Name = "Active" });

            manager.Save();
        }

        public void Dispose()
        {
            TestTools.DeleteCmsFile();
            FCms.Tools.Cacher.Clear();
        }

        void CreateTextContentValue()
        {
            contentStore = ContentStore.Load(repositoryId);
            var contentItem = new StringContentItem()
            {
                Id = Guid.NewGuid(),
                DefinitionId = definitionId,
                Data = contentName
            };
            var contentFilter = new ContentFilter()
            {
                FilterDefinitionId = datetimeFilterId
            };
            contentFilter.Values.Add(DateTime.Today.AddDays(-10));
            contentFilter.Values.Add(DateTime.Today.AddDays(10));
            contentItem.Filters.Add(contentFilter);
            contentStore.Items.Add(contentItem);
            contentStore.Save();
        }

        [Fact]
        public void TargetedValueNotFoundTest()
        {
            CreateTextContentValue();

            ContentEngine engine = new ContentEngine(repositoryName);
            List<ContentItem> items = engine.GetContents<ContentItem>(contentName, new Dictionary<string, object>()).ToList();
            Assert.Empty(items);
        }

        [Fact]
        public void TargetedValueDateTimeFilterTest()
        {
            CreateTextContentValue();

            ContentEngine engine = new ContentEngine(repositoryName);
            
            List<ContentItem> items = engine.GetContents<ContentItem>(contentName, new Dictionary<string, object>() { { "Active", DateTime.Now.AddDays(-11) } }).ToList();
            Assert.Empty(items);

            items = engine.GetContents<ContentItem>(contentName, new Dictionary<string, object>() { { "Active", DateTime.Now.AddDays(11) } }).ToList();
            Assert.Empty(items);

            items = engine.GetContents<ContentItem>(contentName, new Dictionary<string, object>() { { "Active", DateTime.Now } }).ToList();
            Assert.Single(items);
        }

        [Fact]
        public void TargetedValueExcludeDateTimeFilterTest()
        {
            CreateTextContentValue();
            contentStore.Items[0].Filters[0].FilterType = IContentFilter.ContentFilterType.Exclude;
            contentStore.Save();

            ContentEngine engine = new ContentEngine(repositoryName);

            List<ContentItem> items = engine.GetContents<ContentItem>(contentName, new Dictionary<string, object>() { { "Active", DateTime.Now.AddDays(-11) }}).ToList();
            Assert.Single(items);

            items = engine.GetContents<ContentItem>(contentName, new Dictionary<string, object>() { { "Active", DateTime.Now.AddDays(11) } }).ToList();
            Assert.Single(items);

            items = engine.GetContents<ContentItem>(contentName, new Dictionary<string, object>() { { "Active", DateTime.Now }}).ToList();
            Assert.Empty(items);
        }
    }
}
