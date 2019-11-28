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
    public class TargetedContentTest
    {
        const string repositoryName = "TestRepository";
        const string contentName = "Title";
        Guid repositoryId = Guid.NewGuid();
        Guid definitionId = Guid.NewGuid();
        Guid booleanFilterId = Guid.NewGuid();
        ICmsManager manager;

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
            manager.Filters.Add(new BooleanFilter() { Id = booleanFilterId, Name = "IsLoggedIn" });

            manager.Save();
        }

        [TestCleanup]
        public void CleanupTest()
        {
            Tools.DeleteCmsFile();
        }

        void CreateBooleanContentValue()
        {
            IContentStore contentStore = manager.GetContentStore(repositoryId);
            var contentItem = new ContentItem()
            {
                Id = Guid.NewGuid(),
                DefinitionId = definitionId,
                Value = contentName
            };
            var contentFilter = new ContentFilter()
            {
                FilterDefinitionId = booleanFilterId
            };
            contentFilter.Values.Add("on");
            contentItem.Filters.Add(contentFilter);
            contentStore.Items.Add(contentItem);
            contentStore.Save();
        }

        [TestMethod]
        public void TargetedValueNotFoundTest()
        {
            CreateBooleanContentValue();

            ContentEngine engine = new ContentEngine(repositoryName);
            List<ContentItem> items = engine.GetContents(contentName, new { }).ToList();
            Assert.AreEqual(0, items.Count());
        }

        [TestMethod]
        public void TargetedValueBooleanFilterTest()
        {
            CreateBooleanContentValue();

            ContentEngine engine = new ContentEngine(repositoryName);

            List<ContentItem> items = engine.GetContents(contentName, new { IsLoggedIn = false }).ToList();
            Assert.AreEqual(0, items.Count());

            items = engine.GetContents(contentName, new { IsLoggedIn = true }).ToList();
            Assert.AreEqual(1, items.Count());
        }
    }
}
