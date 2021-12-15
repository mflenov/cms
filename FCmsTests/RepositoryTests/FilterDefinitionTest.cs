using System;
using Xunit;
using FCms.Content;
using FCmsTests.Helpers;

namespace FCmsTests
{
    [Collection("Sequential")]
    public class FilterDefinitionTest: IDisposable
    {
        public FilterDefinitionTest()
        {
            Tools.DeleteCmsFile();
        }

        public void Dispose()
        {
            Tools.DeleteCmsFile();
            FCms.Tools.Cacher.Clear();
        }

        [Fact]
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
            Assert.Equal(4, loadedmanager.Data.Filters.Count);

            Assert.Equal(filter1, loadedmanager.Data.Filters[0].Id);
            Assert.Equal("Boolean Filter", loadedmanager.Data.Filters[0].Name);
            Assert.True(loadedmanager.Data.Filters[0] is BooleanFilter);

            Assert.Equal(filter2, loadedmanager.Data.Filters[1].Id);
            Assert.Equal("DateRange Filter", loadedmanager.Data.Filters[1].Name);
            Assert.True(loadedmanager.Data.Filters[1] is DateRangeFilter);

            Assert.Equal(filter3, loadedmanager.Data.Filters[2].Id);
            Assert.Equal("Regex Filter", loadedmanager.Data.Filters[2].Name);
            Assert.True(loadedmanager.Data.Filters[2] is RegExFilter);

            Assert.Equal(filter4, loadedmanager.Data.Filters[3].Id);
            Assert.Equal("Text Filter", loadedmanager.Data.Filters[3].Name);
            Assert.True(loadedmanager.Data.Filters[3] is TextFilter);
        }

        [Fact]
        public void CreateValueListFiltersTest()
        {
            Guid filter1 = Guid.NewGuid();
            ICmsManager manager = new CmsManager();
            manager.Data.Filters.Add(new ValueListFilter() { Id = filter1, Name = "ValueList Filter" });
            manager.Save();

            // load and make sure it is there
            ICmsManager loadedmanager = new CmsManager();
            Assert.Single(loadedmanager.Data.Filters);

            Assert.Equal(filter1, loadedmanager.Data.Filters[0].Id);
            Assert.Equal("ValueList Filter", loadedmanager.Data.Filters[0].Name);
            Assert.True(loadedmanager.Data.Filters[0] is ValueListFilter);
        }
    }
}
