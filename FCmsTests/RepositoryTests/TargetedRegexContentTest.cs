﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FCms.Content;
using FCmsTests.Helpers;
using FCms;
using System.Collections.Generic;

namespace FCmsTests
{
    [TestClass]
    public class TargetedRegexContentTest
    {
        const string repositoryName = "TestRepository";
        const string contentName = "Title";
        Guid repositoryId = Guid.NewGuid();
        Guid definitionId = Guid.NewGuid();
        Guid regexFilterId = Guid.NewGuid();
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
                FilterDefinitionId = regexFilterId
            };
            contentFilter.Values.Add(@"(\w+)@gmail.com");
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
        public void TargetedValueStringFilterTest()
        {
            CreateTextContentValue();

            ContentEngine engine = new ContentEngine(repositoryName);

            List<ContentItem> items = engine.GetContents<ContentItem>(contentName, new { Email = "test@hotmail.com" }).ToList();
            Assert.AreEqual(0, items.Count());

            items = engine.GetContents<ContentItem>(contentName, new { Email = "test@gmail.com" }).ToList();
            Assert.AreEqual(1, items.Count());
        }

        [TestMethod]
        public void TargetedValueExcludeStringFilterTest()
        {
            CreateTextContentValue();
            contentStore.Items[0].Filters[0].FilterType = IContentFilter.ContentFilterType.Exclude;
            manager.SaveContentStore(contentStore);

            ContentEngine engine = new ContentEngine(repositoryName);

            List<ContentItem> items = engine.GetContents<ContentItem>(contentName, new { Email = "test@hotmail.com" }).ToList();
            Assert.AreEqual(1, items.Count());

            items = engine.GetContents<ContentItem>(contentName, new { Email = "test@gmail.com" }).ToList();
            Assert.AreEqual(0, items.Count());
        }
    }
}
