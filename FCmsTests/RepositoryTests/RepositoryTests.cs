using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FCms.Content;
using FCmsTests.Helpers;

namespace FCmsTests
{
    [TestClass]
    public class RepositoryTests
    {
        [TestInitialize] 
        public void InitTest()
        {
            Tools.DeleteCmsFile();
        }

        [TestCleanup]
        public void CleanupTest()
        {
            Tools.DeleteCmsFile();
        }

        [TestMethod]
        public void AddRepositoryTest()
        {
            // create repository
            Guid repositoryId1 = Guid.NewGuid();
            ICmsManager manager = new CmsManager();
            manager.Data.Repositories.Add(
                    new Repository()
                    {
                        Id = repositoryId1,
                        Name = "Test 1"
                    }
                );
            manager.Save();

            // load manager and make sure the first repo is there
            ICmsManager loadedmanager = new CmsManager();
            Assert.AreEqual(1, loadedmanager.Data.Repositories.Count);
            Assert.AreEqual(repositoryId1, loadedmanager.Data.Repositories[0].Id);
            Assert.AreEqual("Test 1", loadedmanager.Data.Repositories[0].Name);

            // add one more repo
            Guid repositoryId2 = Guid.NewGuid();
            loadedmanager.Data.Repositories.Add(
                new Repository()
                {
                    Id = repositoryId2,
                    Name = "Test 2"
                }
            );
            loadedmanager.Save();

            // load manager and make sure the first repo is there
            loadedmanager = new CmsManager();
            Assert.AreEqual(2, loadedmanager.Data.Repositories.Count);
            Assert.AreEqual(repositoryId1, loadedmanager.Data.Repositories[0].Id);
            Assert.AreEqual("Test 1", loadedmanager.Data.Repositories[0].Name);
            Assert.AreEqual(repositoryId2, loadedmanager.Data.Repositories[1].Id);
            Assert.AreEqual("Test 2", loadedmanager.Data.Repositories[1].Name);

            manager.Save();
        }
    }
}
