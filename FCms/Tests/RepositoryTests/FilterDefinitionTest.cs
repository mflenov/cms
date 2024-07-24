using System;
using NUnit.Framework;
using FCms.Content;
using FCmsTests.Helpers;

namespace FCmsTests
{
    public class FilterDefinitionTest: IDisposable
    {
        public FilterDefinitionTest()
        {
            TestTools.DeleteCmsFile();
        }

        public void Dispose()
        {
            TestTools.DeleteCmsFile();
            FCms.Tools.Cacher.Clear();
        }

        [Test, Sequential]
        public void CreateFiltersTest()
        {
            // create Filter
            Guid filter1 = Guid.NewGuid();
            Guid filter2 = Guid.NewGuid();
            Guid filter3 = Guid.NewGuid();
            Guid filter4 = Guid.NewGuid();
            ICmsManager manager = new CmsManager();
            manager.Data.Filters.Add(new BooleanFilter() { Id = filter1, Name = "Boolean Filter" });
            manager.Data.Filters.Add(new DateRangeFilter() { Id = filter2, Name = "DateRange Filter" });
            manager.Data.Filters.Add(new RegExFilter() { Id = filter3, Name = "Regex Filter" });
            manager.Data.Filters.Add(new TextFilter() { Id = filter4, Name = "Text Filter" });
            manager.Save();

            // load and make sure it is there
            ICmsManager loadedmanager = new CmsManager();
            Assert.That(loadedmanager.Data.Filters.Count, Is.EqualTo(4));

            Assert.That(filter1, Is.EqualTo(loadedmanager.Data.Filters[0].Id));
            Assert.That(loadedmanager.Data.Filters[0].Name, Is.EqualTo("Boolean Filter"));
            Assert.True(loadedmanager.Data.Filters[0] is BooleanFilter);

            Assert.That(filter2, Is.EqualTo(loadedmanager.Data.Filters[1].Id));
            Assert.That(loadedmanager.Data.Filters[1].Name, Is.EqualTo("DateRange Filter"));
            Assert.True(loadedmanager.Data.Filters[1] is DateRangeFilter);

            Assert.That(filter3, Is.EqualTo(loadedmanager.Data.Filters[2].Id));
            Assert.That(loadedmanager.Data.Filters[2].Name, Is.EqualTo("Regex Filter"));
            Assert.True(loadedmanager.Data.Filters[2] is RegExFilter);

            Assert.That(filter4, Is.EqualTo(loadedmanager.Data.Filters[3].Id));
            Assert.That(loadedmanager.Data.Filters[3].Name, Is.EqualTo("Text Filter"));
            Assert.True(loadedmanager.Data.Filters[3] is TextFilter);
        }

        [Test, Sequential]
        public void CreateValueListFiltersTest()
        {
            Guid filter1 = Guid.NewGuid();
            ICmsManager manager = new CmsManager();
            manager.Data.Filters.Add(new ValueListFilter() { Id = filter1, Name = "ValueList Filter" });
            manager.Save();

            // load and make sure it is there
            ICmsManager loadedmanager = new CmsManager();
            Assert.That(loadedmanager.Data.Filters.Count, Is.EqualTo(1));

            Assert.That(filter1, Is.EqualTo(loadedmanager.Data.Filters[0].Id));
            Assert.That(loadedmanager.Data.Filters[0].Name, Is.EqualTo("ValueList Filter"));
            Assert.True(loadedmanager.Data.Filters[0] is ValueListFilter);
        }
    }
}
