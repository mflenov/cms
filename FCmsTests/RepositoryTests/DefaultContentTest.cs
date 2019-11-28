using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FCms.Content;
using FCmsTests.Helpers;
using FCms;

namespace FCmsTests
{
    [TestClass]
    public class DefaultContentTest
    {
        const string repositoryName = "TestRepository";
        const string contentName = "Title";
        Guid repositoryId = Guid.NewGuid();
        Guid definitionId = Guid.NewGuid();
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
            manager.Save();
        }

        [TestCleanup]
        public void CleanupTest()
        {
            Tools.DeleteCmsFile();
        }

        [TestMethod]
        public void DefaultContentNotFoundTest()
        {
            ContentEngine engine = new ContentEngine(repositoryName);
            
            Assert.AreEqual("", engine.GetContentString(contentName));
        }

        [TestMethod]
        public void DefaultContenExistOneFoundTest()
        {
            IContentStore contentStore = manager.GetContentStore(repositoryId);
            var contentItem = new ContentItem()
            {
                Id = Guid.NewGuid(),
                DefinitionId = definitionId,
                Value = "UniqueValue"
            };
            contentStore.Items.Add(contentItem);
            contentStore.Save();

            ContentEngine engine = new ContentEngine(repositoryName);
            Assert.AreEqual("UniqueValue", engine.GetContentString(contentName));
            Assert.AreEqual(1, engine.GetContentStrings(contentName).Count());
        }

        [TestMethod]
        public void DefaultContenExistTwoFoundTest()
        {
            IContentStore contentStore = manager.GetContentStore(repositoryId);
            contentStore.Items.Add(new ContentItem()
                {
                    Id = Guid.NewGuid(),
                    DefinitionId = definitionId,
                    Value = "First"
                });
            contentStore.Items.Add(new ContentItem()
            {
                Id = Guid.NewGuid(),
                DefinitionId = definitionId,
                Value = "Second"
            });
            contentStore.Save();

            // check GetContentString
            ContentEngine engine = new ContentEngine(repositoryName);
            Assert.AreEqual("First", engine.GetContentString(contentName));

            // check GetContentStrings
            var list = engine.GetContentStrings(contentName).ToList();
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual("First", list[0]);
            Assert.AreEqual("Second", list[1]);
        }
    }
}

