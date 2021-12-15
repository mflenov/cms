using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FCms.Content;
using FCmsTests.Helpers;
using FCms;
using System.Collections.Generic;

namespace FCmsTests
{
    [TestClass]
    public class TargetedDateRangeContentTest
    {
        const string repositoryName = "TestRepository";
        const string contentName = "Title";
        Guid repositoryId = Guid.NewGuid();
        Guid definitionId = Guid.NewGuid();
        Guid datetimeFilterId = Guid.NewGuid();
        ICmsManager manager;
        IContentStore contentStore;

        [TestInitialize]
        public void InitTest()
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
            manager.Data.Filters.Add(new DateRangeFilter() { Id = datetimeFilterId, Name = "Active" });

            manager.Save();
        }

        [TestCleanup]
        public void CleanupTest()
        {
            Tools.DeleteCmsFile();
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
                FilterDefinitionId = datetimeFilterId
            };
            contentFilter.Values.Add(DateTime.Today.AddDays(-10));
            contentFilter.Values.Add(DateTime.Today.AddDays(10));
            contentItem.Filters.Add(contentFilter);
            contentStore.Items.Add(contentItem);
            manager.SaveContentStore(contentStore);
        }

        [TestMethod]
        public void TargetedValueNotFoundTest()
        {
            CreateTextContentValue();

            ContentEngine engine = new ContentEngine(repositoryName);
            List<ContentItem> items = engine.GetContents<ContentItem>(contentName, new { }).ToList();
            Assert.AreEqual(0, items.Count());
        }

        [TestMethod]
        public void TargetedValueDateTimeFilterTest()
        {
            CreateTextContentValue();

            ContentEngine engine = new ContentEngine(repositoryName);

            List<ContentItem> items = engine.GetContents<ContentItem>(contentName, new { Active = DateTime.Now.AddDays(-11) }).ToList();
            Assert.AreEqual(0, items.Count());

            items = engine.GetContents<ContentItem>(contentName, new { Active = DateTime.Now.AddDays(11) }).ToList();
            Assert.AreEqual(0, items.Count());

            items = engine.GetContents<ContentItem>(contentName, new { Active = DateTime.Now }).ToList();
            Assert.AreEqual(1, items.Count());
        }

        [TestMethod]
        public void TargetedValueExcludeDateTimeFilterTest()
        {
            CreateTextContentValue();
            contentStore.Items[0].Filters[0].FilterType = IContentFilter.ContentFilterType.Exclude;
            manager.SaveContentStore(contentStore);

            ContentEngine engine = new ContentEngine(repositoryName);

            List<ContentItem> items = engine.GetContents<ContentItem>(contentName, new { Active = DateTime.Now.AddDays(-11) }).ToList();
            Assert.AreEqual(1, items.Count());

            items = engine.GetContents<ContentItem>(contentName, new { Active = DateTime.Now.AddDays(11) }).ToList();
            Assert.AreEqual(1, items.Count());

            items = engine.GetContents<ContentItem>(contentName, new { Active = DateTime.Now }).ToList();
            Assert.AreEqual(0, items.Count());
        }
    }
}
