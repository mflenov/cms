using System;
using System.Linq;
using FCms.Content;
using FCmsTests.Helpers;
using FCms;
using NUnit.Framework;

namespace FCmsTests
{
    [NonParallelizable]
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
            TestTools.DeleteCmsFile();
            FCms.Tools.Cacher.Clear();
        }

        [Test]
        [NonParallelizable]
        public void DefaultContentNotFoundTest()
        {
            ContentEngine engine = new ContentEngine(repositoryName);
            
            Assert.That(engine.GetContentString(contentName), Is.Empty);
        }

        [Test]
        [NonParallelizable]
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
            Assert.That(engine.GetContentString(contentName), Is.EqualTo("UniqueValue"));
            Assert.That(engine.GetContentStrings(contentName).ToList().Count, Is.EqualTo(1));
        }

        [Test]
        [NonParallelizable]
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
            Assert.That(engine.GetContentString(contentName), Is.EqualTo("First"));

            // check GetContentStrings
            var list = engine.GetContentStrings(contentName).ToList();
            Assert.That(list.Count, Is.EqualTo(2));
            Assert.That(list[0], Is.EqualTo("First"));
            Assert.That(list[1], Is.EqualTo("Second"));
        }
    }
}

