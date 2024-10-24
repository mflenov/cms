using Xunit;
using FCms.DbContent.Db;
using FCms.DbContent.Models;

namespace FCmsTests.DbTests
{
    [Trait("Category", DbHelpersTest.TEST_CATEGORY_MSSQL)]
    public class MsSqlGeneratorTest
    {
        [Fact]
        public void NullSearchModelTest()
        {
            string tablename = "tablename";
            MsSqlGenerator generator = new MsSqlGenerator(tablename);
            var query = generator.GetSearchQuery(null);
            
            string expected = @"SELECT TOP 10 * FROM tablename WHERE 1=1".Replace(" ", "");
            Assert.Equal(expected, query.Sql.Replace(" ", "").Replace("\n", "").Replace("\r", ""));
        }

        [Fact]
        public void NullSearchEmptyModelTest()
        {
            string tablename = "tablename";
            MsSqlGenerator generator = new MsSqlGenerator(tablename);
            var query = generator.GetSearchQuery(new ContentSearchModel());

            string expected = @"SELECT TOP 10 * FROM tablename WHERE 1=1".Replace(" ", "");
            Assert.Equal(expected, query.Sql.Replace(" ", "").Replace("\n", "").Replace("\r", ""));
        }
    }
}
