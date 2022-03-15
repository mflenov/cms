using System;
using Xunit;
using FCms.Content;
using FCmsTests.Helpers;

namespace FCmsTests
{
    [Collection("Sequential")]
    public class RepositoryTests: IDisposable
    {
        public RepositoryTests()
        {
            Tools.DeleteCmsFile();
        }

        public void Dispose()
        {
            Tools.DeleteCmsFile();
            FCms.Tools.Cacher.Clear();
        }

        [Fact]
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
            Assert.Single(loadedmanager.Data.Repositories);
            Assert.Equal(repositoryId1, loadedmanager.Data.Repositories[0].Id);
            Assert.Equal("Test 1", loadedmanager.Data.Repositories[0].Name);

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
            Assert.Equal(2, loadedmanager.Data.Repositories.Count);
            Assert.Equal(repositoryId1, loadedmanager.Data.Repositories[0].Id);
            Assert.Equal("Test 1", loadedmanager.Data.Repositories[0].Name);
            Assert.Equal(repositoryId2, loadedmanager.Data.Repositories[1].Id);
            Assert.Equal("Test 2", loadedmanager.Data.Repositories[1].Name);

            manager.Save();
        }
    }
}
