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
    public class TargetedBooleanContentTest: IDisposable
    {
        const string repositoryName = "TestRepository";
        const string contentName = "Title";
        Guid repositoryId = Guid.NewGuid();
        Guid definitionId = Guid.NewGuid();
        Guid booleanFilterId = Guid.NewGuid();
        ICmsManager manager;
        IContentStore contentStore;

        public TargetedBooleanContentTest()
        {
            Tools.DeleteCmsFile();

            manager = new CmsManager();
            Repository repository = new Repository() { Id = repositoryId, Name = repositoryName };
            IContentDefinition definition = ContentDefinitionFactory.CreateContentDefinition(ContentDefinitionType.String);
            definition.DefinitionId = definitionId;
            definition.Name = contentName;
            repository.ContentDefinitions.Add(definition);
            manager.Data.Repositories.Add(repository);

            // filters
            manager.Data.Filters.Add(new BooleanFilter() { Id = booleanFilterId, Name = "IsLoggedIn" });

            manager.Save();
        }

        public void Dispose()
        {
            Tools.DeleteCmsFile();
            FCms.Tools.Cacher.Clear();
        }

        void CreateBooleanContentValue()
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
                FilterDefinitionId = booleanFilterId
            };
            contentFilter.Values.Add(true);
            contentItem.Filters.Add(contentFilter);
            contentStore.Items.Add(contentItem);
            contentStore.Save();
        }

        [Fact]
        public void TargetedValueNotFoundTest()
        {
            CreateBooleanContentValue();

            ContentEngine engine = new ContentEngine(repositoryName);
            List<ContentItem> items = engine.GetContents<ContentItem>(contentName, new Dictionary<string, object>()).ToList();
            Assert.Empty(items);
        }

        [Fact]
        public void TargetedValueBooleanFilterTest()
        {
            CreateBooleanContentValue();

            ContentEngine engine = new ContentEngine(repositoryName);

            List<ContentItem> items = engine.GetContents<ContentItem>(contentName, new Dictionary<string, object>() { { "IsLoggedIn", false } }).ToList();
            Assert.Empty(items);

            items = engine.GetContents<ContentItem>(contentName, new Dictionary<string, object>() { { "IsLoggedIn", true } }).ToList();
            Assert.Single(items);
        }

        [Fact]
        public void TargetedValueExcludeBooleanFilterTest()
        {
            CreateBooleanContentValue();
            contentStore.Items[0].Filters[0].FilterType = IContentFilter.ContentFilterType.Exclude;
            contentStore.Save();


            ContentEngine engine = new ContentEngine(repositoryName);

            List<ContentItem> items = engine.GetContents<ContentItem>(contentName, new Dictionary<string, object>() { { "IsLoggedIn", false } }).ToList();
            Assert.Single(items);

            items = engine.GetContents<ContentItem>(contentName, new Dictionary<string, object>() { { "IsLoggedIn", true } }).ToList();
            Assert.Empty(items);
        }
    }
}
