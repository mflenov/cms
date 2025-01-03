﻿using System;
using System.Linq;
using Xunit;
using FCms.Content;
using FCmsTests.Helpers;
using FCms;

namespace FCmsTests
{
    [Collection("Sequential")]
    public class DefaultContentTest: IDisposable
    {
        const string repositoryName = "TestRepository";
        const string contentName = "Title";
        Guid repositoryId = Guid.NewGuid();
        Guid definitionId = Guid.NewGuid();
        ICmsManager manager;

        public DefaultContentTest()
        {
            TestTools.DeleteCmsFile();
            FCms.Tools.Cacher.Clear();

            manager = CmsManager.GetInstance();
            Repository repository = new Repository() { Id = repositoryId, Name = repositoryName };
            IContentDefinition definition = ContentDefinitionFactory.CreateContentDefinition(ContentDefinitionType.String);
            definition.DefinitionId = definitionId;
            definition.Name = contentName;
            repository.ContentDefinitions.Add(definition);
            manager.Data.Repositories.Add(repository);
            manager.Save();
        }

        public void Dispose()
        {
            TestTools.DeleteCmsFile();
            FCms.Tools.Cacher.Clear();
        }

        [Fact]
        public void DefaultContentNotFoundTest()
        {
            ContentEngine engine = new ContentEngine(repositoryName);
            
            Assert.Equal("", engine.GetContentString(contentName));
        }

        [Fact]
        public void DefaultContentExistTwoFoundTest()
        {
            IContentStore contentStore = ContentStore.Load(repositoryId);
            var contentItem = new StringContentItem()
            {
                Id = Guid.NewGuid(),
                DefinitionId = definitionId,
                Data = "UniqueValue"
            };
            contentStore.Items.Add(contentItem);
            contentStore.Save();

            ContentEngine engine = new ContentEngine(repositoryName);
            Assert.Equal("UniqueValue", engine.GetContentString(contentName));
            Assert.Single(engine.GetContentStrings(contentName));
        }

        [Fact]
        public void DefaultContenExistTwoFoundTest()
        {
            IContentStore contentStore = ContentStore.Load(repositoryId);
            contentStore.Items.Add(new StringContentItem()
                {
                    Id = Guid.NewGuid(),
                    DefinitionId = definitionId,
                    Data = "First"
                });
            contentStore.Items.Add(new StringContentItem()
            {
                Id = Guid.NewGuid(),
                DefinitionId = definitionId,
                Data = "Second"
            });
            contentStore.Save();

            // check GetContentString
            ContentEngine engine = new ContentEngine(repositoryName);
            Assert.Equal("First", engine.GetContentString(contentName));

            // check GetContentStrings
            var list = engine.GetContentStrings(contentName).ToList();
            Assert.Equal(2, list.Count);
            Assert.Equal("First", list[0]);
            Assert.Equal("Second", list[1]);
        }
    }
}

