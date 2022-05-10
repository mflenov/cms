using System;
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
            Tools.DeleteCmsFile();

            manager = new CmsManager();
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
            Tools.DeleteCmsFile();
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
            IContentStore contentStore = manager.GetContentStore(repositoryId);
            var contentItem = new StringContentItem()
            {
                Id = Guid.NewGuid(),
                DefinitionId = definitionId,
                Data = "UniqueValue"
            };
            contentStore.Items.Add(contentItem);
            manager.SaveContentStore(contentStore);

            ContentEngine engine = new ContentEngine(repositoryName);
            Assert.Equal("UniqueValue", engine.GetContentString(contentName));
            Assert.Single(engine.GetContentStrings(contentName));
        }

        [Fact]
        public void DefaultContenExistTwoFoundTest()
        {
            IContentStore contentStore = manager.GetContentStore(repositoryId);
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
            manager.SaveContentStore(contentStore);

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

