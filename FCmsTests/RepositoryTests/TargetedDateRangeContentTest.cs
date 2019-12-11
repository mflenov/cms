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

            manager = CmsManager.Load();
            Repository repository = new Repository() { Id = repositoryId, Name = repositoryName };
            IContentDefinition definition = ContentDefinitionFactory.CreateContentDefinition(IContentDefinition.DefinitionType.String);
            definition.DefinitionId = definitionId;
            definition.Name = contentName;
            repository.ContentDefinitions.Add(definition);
            manager.Repositories.Add(repository);

            // filters
            manager.Filters.Add(new DateRangeFilter() { Id = datetimeFilterId, Name = "Active" });

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
            var contentItem = new ContentItem()
            {
                Id = Guid.NewGuid(),
                DefinitionId = definitionId,
                Value = contentName
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

        [TestMethod]
        public void TargetedValueNotFoundTest()
        {
            CreateTextContentValue();

            ContentEngine engine = new ContentEngine(repositoryName);
            List<ContentItem> items = engine.GetContents(contentName, new { }).ToList();
            Assert.AreEqual(0, items.Count());
        }

        [TestMethod]
        public void TargetedValueDateTimeFilterTest()
        {
            CreateTextContentValue();

            ContentEngine engine = new ContentEngine(repositoryName);

            List<ContentItem> items = engine.GetContents(contentName, new { Active = DateTime.Now.AddDays(-11) }).ToList();
            Assert.AreEqual(0, items.Count());

            items = engine.GetContents(contentName, new { Active = DateTime.Now.AddDays(11) }).ToList();
            Assert.AreEqual(0, items.Count());

            items = engine.GetContents(contentName, new { Active = DateTime.Now }).ToList();
            Assert.AreEqual(1, items.Count());
        }

        [TestMethod]
        public void TargetedValueExcludeDateTimeFilterTest()
        {
            CreateTextContentValue();
            contentStore.Items[0].Filters[0].FilterType = IContentFilter.ContentFilterType.Exclude;
            contentStore.Save();

            ContentEngine engine = new ContentEngine(repositoryName);

            List<ContentItem> items = engine.GetContents(contentName, new { Active = DateTime.Now.AddDays(-11) }).ToList();
            Assert.AreEqual(1, items.Count());

            items = engine.GetContents(contentName, new { Active = DateTime.Now.AddDays(11) }).ToList();
            Assert.AreEqual(1, items.Count());

            items = engine.GetContents(contentName, new { Active = DateTime.Now }).ToList();
            Assert.AreEqual(0, items.Count());
        }
    }
}
