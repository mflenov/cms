using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FCms.Content;
using FCmsTests.Helpers;

namespace FCmsTests
{
    [TestClass]
    public class FilterDefinitionTest
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
        public void CreateFiltersTest()
        {
            // create Filter
            Guid filter1 = Guid.NewGuid();
            Guid filter2 = Guid.NewGuid();
            Guid filter3 = Guid.NewGuid();
            Guid filter4 = Guid.NewGuid();
            ICmsManager manager = CmsManager.Load();
            manager.Filters.Add(new BooleanFilter { Id = filter1, Name = "Boolean Filter" });
            manager.Filters.Add(new DateRangeFilter { Id = filter2, Name = "DateRange Filter" });
            manager.Filters.Add(new RegExFilter { Id = filter3, Name = "Regex Filter" });
            manager.Filters.Add(new TextFilter { Id = filter4, Name = "Text Filter" });
            manager.Save();

            // load and make sure it is there
            ICmsManager loadedmanager = CmsManager.Load();
            Assert.AreEqual(4, loadedmanager.Filters.Count);

            Assert.AreEqual(filter1, loadedmanager.Filters[0].Id);
            Assert.AreEqual("Boolean Filter", loadedmanager.Filters[0].Name);
            Assert.IsTrue(loadedmanager.Filters[0] is BooleanFilter);

            Assert.AreEqual(filter2, loadedmanager.Filters[1].Id);
            Assert.AreEqual("DateRange Filter", loadedmanager.Filters[1].Name);
            Assert.IsTrue(loadedmanager.Filters[1] is DateRangeFilter);

            Assert.AreEqual(filter3, loadedmanager.Filters[2].Id);
            Assert.AreEqual("Regex Filter", loadedmanager.Filters[2].Name);
            Assert.IsTrue(loadedmanager.Filters[2] is RegExFilter);

            Assert.AreEqual(filter4, loadedmanager.Filters[3].Id);
            Assert.AreEqual("Text Filter", loadedmanager.Filters[3].Name);
            Assert.IsTrue(loadedmanager.Filters[3] is TextFilter);
        }

        [TestMethod]
        public void CreateValueListFiltersTest()
        {
            Guid filter1 = Guid.NewGuid();
            ICmsManager manager = CmsManager.Load();
            manager.Filters.Add(new ValueListFilter { Id = filter1, Name = "ValueList Filter" });
            manager.Save();

            // load and make sure it is there
            ICmsManager loadedmanager = CmsManager.Load();
            Assert.AreEqual(1, loadedmanager.Filters.Count);

            Assert.AreEqual(filter1, loadedmanager.Filters[0].Id);
            Assert.AreEqual("ValueList Filter", loadedmanager.Filters[0].Name);
            Assert.IsTrue(loadedmanager.Filters[0] is ValueListFilter);
        }
    }
}
