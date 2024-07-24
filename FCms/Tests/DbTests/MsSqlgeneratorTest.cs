using FCms.DbContent.Db;
using FCms.DbContent.Models;
using NUnit.Framework;

namespace FCmsTests.DbTests
{
    public class MsSqlGeneratorTest
    {
        [Test, Sequential]
        public void NullSearchModelTest()
        {
            string tablename = "tablename";
            MsSqlGenerator generator = new MsSqlGenerator(tablename);
            var query = generator.GetSearchQuery(null);
            
            string expected = @"SELECT TOP 10 * FROM tablename WHERE 1=1".Replace(" ", "");
            Assert.That(expected, Is.EqualTo(query.Sql.Replace(" ", "").Replace("\n", "").Replace("\r", "")));
        }

        [Test, Sequential]
        public void NullSearchEmptyModelTest()
        {
            string tablename = "tablename";
            MsSqlGenerator generator = new MsSqlGenerator(tablename);
            var query = generator.GetSearchQuery(new ContentSearchModel());

            string expected = @"SELECT TOP 10 * FROM tablename WHERE 1=1".Replace(" ", "");
            Assert.That(expected, Is.EqualTo(query.Sql.Replace(" ", "").Replace("\n", "").Replace("\r", "")));
        }
    }
}
