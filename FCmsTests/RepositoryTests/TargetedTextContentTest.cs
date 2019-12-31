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
    public class TargetedTextContentTest
    {
        const string repositoryName = "TestRepository";
        const string contentName = "Title";
        Guid repositoryId = Guid.NewGuid();
        Guid definitionId = Guid.NewGuid();
        Guid textFilterId = Guid.NewGuid();
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
            manager.Filters.Add(new TextFilter() { Id = textFilterId, Name = "Card" });

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
                FilterDefinitionId = textFilterId
            };
            contentFilter.Values.Add("MyCoolCard");
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
        public void TargetedValueStringFilterTest()
        {
            CreateTextContentValue();

            ContentEngine engine = new ContentEngine(repositoryName);

            List<ContentItem> items = engine.GetContents(contentName, new { Card = "NotFoundCard" }).ToList();
            Assert.AreEqual(0, items.Count());

            items = engine.GetContents(contentName, new { Card = "MyCoolCard" }).ToList();
            Assert.AreEqual(1, items.Count());
        }

        [TestMethod]
        public void TargetedValueExcludeStringFilterTest()
        {
            CreateTextContentValue();
            contentStore.Items[0].Filters[0].FilterType = IContentFilter.ContentFilterType.Exclude;
            contentStore.Save();

            ContentEngine engine = new ContentEngine(repositoryName);

            List<ContentItem> items = engine.GetContents(contentName, new { Card = "NotFoundCard" }).ToList();
            Assert.AreEqual(1, items.Count());

            items = engine.GetContents(contentName, new { Card = "MyCoolCard" }).ToList();
            Assert.AreEqual(0, items.Count());
        }

    }
}
