﻿using System;
using System.Linq;
using Xunit;
using FCms.Content;
using FCmsTests.Helpers;
using FCms;
using System.Collections.Generic;

namespace FCmsTests
{
    [Collection("Sequential")]
    public class TargetedRegexContentTest: IDisposable
    {
        const string repositoryName = "TestRepository";
        const string contentName = "Title";
        Guid repositoryId = Guid.NewGuid();
        Guid definitionId = Guid.NewGuid();
        Guid regexFilterId = Guid.NewGuid();
        ICmsManager manager;
        IContentStore contentStore;

        public TargetedRegexContentTest()
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
                FilterDefinitionId = regexFilterId
            };
            contentFilter.Values.Add(@"(\w+)@gmail.com");
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

            List<ContentItem> items = engine.GetContents<ContentItem>(contentName, new { Email = "test@hotmail.com" }).ToList();
            Assert.Empty(items);

            items = engine.GetContents<ContentItem>(contentName, new { Email = "test@gmail.com" }).ToList();
            Assert.Single(items);
        }

        [Fact]
        public void TargetedValueExcludeStringFilterTest()
        {
            CreateTextContentValue();
            contentStore.Items[0].Filters[0].FilterType = IContentFilter.ContentFilterType.Exclude;
            manager.SaveContentStore(contentStore);

            ContentEngine engine = new ContentEngine(repositoryName);

            List<ContentItem> items = engine.GetContents<ContentItem>(contentName, new { Email = "test@hotmail.com" }).ToList();
            Assert.Single(items);

            items = engine.GetContents<ContentItem>(contentName, new { Email = "test@gmail.com" }).ToList();
            Assert.Empty(items);
        }
    }
}
