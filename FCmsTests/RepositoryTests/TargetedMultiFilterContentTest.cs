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
    // tests with more than one filter
    public class TargetedMultiFilterContentTest: IDisposable
    {
        const string repositoryName = "TestRepository";
        const string contentName = "Title";
        Guid repositoryId = Guid.NewGuid();
        Guid definitionId = Guid.NewGuid();
        Guid regexFilterId = Guid.NewGuid();
        Guid booleanFilterId = Guid.NewGuid();
        ICmsManager manager;
        IContentStore contentStore;

        public TargetedMultiFilterContentTest()
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
            manager.SaveContentStore(contentStore);
        }

        [Fact]
        public void TargetedValueNotFoundTest()
        {
            CreateTextContentValue();

            ContentEngine engine = new ContentEngine(repositoryName);
            List<ContentItem> items = engine.GetContents<ContentItem>(contentName, new { }).ToList();
            Assert.Equal(0, items.Count());

            items = engine.GetContents<ContentItem>(contentName, new { Email = "test@gmail.com" }).ToList();
            Assert.Equal(0, items.Count());

            items = engine.GetContents<ContentItem>(contentName, new { Email = "test@gmail.com", IsLoggedIn = false }).ToList();
            Assert.Equal(0, items.Count());

            items = engine.GetContents<ContentItem>(contentName, new { IsLoggedIn = false }).ToList();
            Assert.Equal(0, items.Count());
        }

        [Fact]
        public void TargetedValueStringFilterTest()
        {
            CreateTextContentValue();

            ContentEngine engine = new ContentEngine(repositoryName);

            List<ContentItem> items = engine.GetContents<ContentItem>(contentName, new { Email = "test@gmail.com", IsLoggedIn = true}).ToList();
            Assert.Equal(1, items.Count());
        }

        // exclude gmail and logged in users
        [Fact]
        public void TargetedValueExcludeStringFilterTest()
        {
            CreateTextContentValue();
            contentStore.Items[0].Filters[0].FilterType = IContentFilter.ContentFilterType.Exclude;
            contentStore.Items[0].Filters[1].FilterType = IContentFilter.ContentFilterType.Exclude;
            manager.SaveContentStore(contentStore);

            ContentEngine engine = new ContentEngine(repositoryName);

            List<ContentItem> items = engine.GetContents<ContentItem>(contentName, new { Email = "test@gmail.com", IsLoggedIn = true }).ToList();
            Assert.Equal(0, items.Count());

            items = engine.GetContents<ContentItem>(contentName, new { Email = "test@hotmail.com", IsLoggedIn = true }).ToList();
            Assert.Equal(0, items.Count());

            items = engine.GetContents<ContentItem>(contentName, new { Email = "test@gmail.com", IsLoggedIn = false }).ToList();
            Assert.Equal(0, items.Count());

            items = engine.GetContents<ContentItem>(contentName, new { Email = "test@hotmail.com", IsLoggedIn = false }).ToList();
            Assert.Equal(1, items.Count());
        }

        // exclude gmail include logged in users
        [Fact]
        public void TargetedValue_ExcludeRegEx_IncludeBoolean_StringFilterTest()
        {
            CreateTextContentValue();
            contentStore.Items[0].Filters[0].FilterType = IContentFilter.ContentFilterType.Exclude;
            manager.SaveContentStore(contentStore);

            ContentEngine engine = new ContentEngine(repositoryName);

            List<ContentItem> items = engine.GetContents<ContentItem>(contentName, new { Email = "test@gmail.com", IsLoggedIn = true }).ToList();
            Assert.Equal(0, items.Count());

            items = engine.GetContents<ContentItem>(contentName, new { Email = "test@hotmail.com", IsLoggedIn = false }).ToList();
            Assert.Equal(0, items.Count());

            items = engine.GetContents<ContentItem>(contentName, new { Email = "test@gmail.com", IsLoggedIn = false }).ToList();
            Assert.Equal(0, items.Count());

            items = engine.GetContents<ContentItem>(contentName, new { Email = "test@hotmail.com", IsLoggedIn = true }).ToList();
            Assert.Equal(1, items.Count());
        }
    }
}
