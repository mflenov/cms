using System;
using NUnit.Framework;
using FCms.Content;
using FCmsTests.Helpers;
using System.Linq;

namespace FCmsTests
{
    public class RepositoryTests: IDisposable
    {
        public RepositoryTests()
        {
            TestTools.DeleteCmsFile();
        }

        public void Dispose()
        {
            TestTools.DeleteCmsFile();
            FCms.Tools.Cacher.Clear();
        }

        [Test, Sequential]
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
            Assert.That(loadedmanager.Data.Repositories.ToList().Count(), Is.EqualTo(1));
            Assert.That(repositoryId1, Is.EqualTo(loadedmanager.Data.Repositories[0].Id));
            Assert.That(loadedmanager.Data.Repositories[0].Name, Is.EqualTo("Test 1"));

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
            Assert.That(loadedmanager.Data.Repositories.Count, Is.EqualTo(2));
            Assert.That(repositoryId1, Is.EqualTo(loadedmanager.Data.Repositories[0].Id));
            Assert.That(loadedmanager.Data.Repositories[0].Name, Is.EqualTo(("Test 1")));
            Assert.That(repositoryId2, Is.EqualTo(loadedmanager.Data.Repositories[1].Id));
            Assert.That(loadedmanager.Data.Repositories[1].Name, Is.EqualTo("Test 2"));

            manager.Save();
        }
    }
}
