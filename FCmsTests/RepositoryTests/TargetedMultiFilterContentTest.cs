using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FCms.Content;
using FCmsTests.Helpers;
using FCms;
using System.Collections.Generic;

namespace FCmsTests
{
    // tests with more than one filter
    [TestClass]
    public class TargetedMultiFilterContentTest
    {
        const string repositoryName = "TestRepository";
        const string contentName = "Title";
        Guid repositoryId = Guid.NewGuid();
        Guid definitionId = Guid.NewGuid();
        Guid regexFilterId = Guid.NewGuid();
        Guid booleanFilterId = Guid.NewGuid();
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
            manager.Data.Filters.Add(new RegExFilter() { Id = regexFilterId, Name = "Email" });
            manager.Data.Filters.Add(new BooleanFilter() { Id = booleanFilterId, Name = "IsLoggedIn" });

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

            // add emil filter
            var emailcontentFilter = new ContentFilter()
            {
                FilterDefinitionId = regexFilterId
            };
            emailcontentFilter.Values.Add(@"(\w+)@gmail.com");
            contentItem.Filters.Add(emailcontentFilter);

            // add isloggedin filter
            var isloggedincontentFilter = new ContentFilter()
            {
                FilterDefinitionId = booleanFilterId
            };
            isloggedincontentFilter.Values.Add(true);
            contentItem.Filters.Add(isloggedincontentFilter);

            contentStore.Items.Add(contentItem);
            contentStore.Save();
        }

        [TestMethod]
        public void TargetedValueNotFoundTest()
        {
            CreateTextContentValue();

            ContentEngine engine = new ContentEngine(repositoryName);
            List<ContentItem> items = engine.GetContents<ContentItem>(contentName, new { }).ToList();
            Assert.AreEqual(0, items.Count());

            items = engine.GetContents<ContentItem>(contentName, new { Email = "test@gmail.com" }).ToList();
            Assert.AreEqual(0, items.Count());

            items = engine.GetContents<ContentItem>(contentName, new { Email = "test@gmail.com", IsLoggedIn = false }).ToList();
            Assert.AreEqual(0, items.Count());

            items = engine.GetContents<ContentItem>(contentName, new { IsLoggedIn = false }).ToList();
            Assert.AreEqual(0, items.Count());
        }

        [TestMethod]
        public void TargetedValueStringFilterTest()
        {
            CreateTextContentValue();

            ContentEngine engine = new ContentEngine(repositoryName);

            List<ContentItem> items = engine.GetContents<ContentItem>(contentName, new { Email = "test@gmail.com", IsLoggedIn = true}).ToList();
            Assert.AreEqual(1, items.Count());
        }

        // exclude gmail and logged in users
        [TestMethod]
        public void TargetedValueExcludeStringFilterTest()
        {
            CreateTextContentValue();
            contentStore.Items[0].Filters[0].FilterType = IContentFilter.ContentFilterType.Exclude;
            contentStore.Items[0].Filters[1].FilterType = IContentFilter.ContentFilterType.Exclude;
            contentStore.Save();

            ContentEngine engine = new ContentEngine(repositoryName);

            List<ContentItem> items = engine.GetContents<ContentItem>(contentName, new { Email = "test@gmail.com", IsLoggedIn = true }).ToList();
            Assert.AreEqual(0, items.Count());

            items = engine.GetContents<ContentItem>(contentName, new { Email = "test@hotmail.com", IsLoggedIn = true }).ToList();
            Assert.AreEqual(0, items.Count());

            items = engine.GetContents<ContentItem>(contentName, new { Email = "test@gmail.com", IsLoggedIn = false }).ToList();
            Assert.AreEqual(0, items.Count());

            items = engine.GetContents<ContentItem>(contentName, new { Email = "test@hotmail.com", IsLoggedIn = false }).ToList();
            Assert.AreEqual(1, items.Count());
        }

        // exclude gmail include logged in users
        [TestMethod]
        public void TargetedValue_ExcludeRegEx_IncludeBoolean_StringFilterTest()
        {
            CreateTextContentValue();
            contentStore.Items[0].Filters[0].FilterType = IContentFilter.ContentFilterType.Exclude;
            contentStore.Save();

            ContentEngine engine = new ContentEngine(repositoryName);

            List<ContentItem> items = engine.GetContents<ContentItem>(contentName, new { Email = "test@gmail.com", IsLoggedIn = true }).ToList();
            Assert.AreEqual(0, items.Count());

            items = engine.GetContents<ContentItem>(contentName, new { Email = "test@hotmail.com", IsLoggedIn = false }).ToList();
            Assert.AreEqual(0, items.Count());

            items = engine.GetContents<ContentItem>(contentName, new { Email = "test@gmail.com", IsLoggedIn = false }).ToList();
            Assert.AreEqual(0, items.Count());

            items = engine.GetContents<ContentItem>(contentName, new { Email = "test@hotmail.com", IsLoggedIn = true }).ToList();
            Assert.AreEqual(1, items.Count());
        }
    }
}
