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
    public class TargetedTextContentTest: IDisposable
    {
        const string repositoryName = "TestRepository";
        const string contentName = "Title";
        Guid repositoryId = Guid.NewGuid();
        Guid definitionId = Guid.NewGuid();
        Guid textFilterId = Guid.NewGuid();
        ICmsManager manager;
        IContentStore contentStore;

        public TargetedTextContentTest()
        {
            Tools.DeleteCmsFile();

            manager = new CmsManager();
            Repository repository = new Repository() { Id = repositoryId, Name = repositoryName };
            IContentDefinition definition = ContentDefinitionFactory.CreateContentDefinition(IContentDefinition.DefinitionType.String);
            definition.DefinitionId = definitionId;
            definition.Name = contentName;
            repository.ContentDefinitions.Add(definition);
            manager.Data.Repositories.Add(repository);

            // filters
            manager.Data.Filters.Add(new TextFilter() { Id = textFilterId, Name = "Card" });

            manager.Save();
        }

        public void Dispose()
        {
            Tools.DeleteCmsFile();
            FCms.Tools.Cacher.Clear();
        }

        void CreateTextContentValue()
        {
            contentStore = manager.GetContentStore(repositoryId);
            var contentItem = new StringContentItem()
            {
                Id = Guid.NewGuid(),
                DefinitionId = definitionId,
                Data = contentName
            };
            var contentFilter = new ContentFilter()
            {
                FilterDefinitionId = textFilterId
            };
            contentFilter.Values.Add("MyCoolCard");
            contentItem.Filters.Add(contentFilter);
            contentStore.Items.Add(contentItem);
            manager.SaveContentStore(contentStore);
        }

        [Fact]
        public void TargetedValueNotFoundTest()
        {
            CreateTextContentValue();

            ContentEngine engine = new ContentEngine(repositoryName);
            List<ContentItem> items = engine.GetContents<ContentItem>(contentName, new { }).ToList();
            Assert.Empty(items);
        }

        [Fact]
        public void TargetedValueStringFilterTest()
        {
            CreateTextContentValue();

            ContentEngine engine = new ContentEngine(repositoryName);

            List<ContentItem> items = engine.GetContents<ContentItem>(contentName, new { Card = "NotFoundCard" }).ToList();
            Assert.Empty(items);

            items = engine.GetContents<ContentItem>(contentName, new { Card = "MyCoolCard" }).ToList();
            Assert.Single(items);
        }

        [Fact]
        public void TargetedValueExcludeStringFilterTest()
        {
            CreateTextContentValue();
            contentStore.Items[0].Filters[0].FilterType = IContentFilter.ContentFilterType.Exclude;
            manager.SaveContentStore(contentStore);

            ContentEngine engine = new ContentEngine(repositoryName);

            List<ContentItem> items = engine.GetContents<ContentItem>(contentName, new { Card = "NotFoundCard" }).ToList();
            Assert.Single(items);

            items = engine.GetContents<ContentItem>(contentName, new { Card = "MyCoolCard" }).ToList();
            Assert.Empty(items);
        }

    }
}
