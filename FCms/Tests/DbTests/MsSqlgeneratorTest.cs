using Xunit;
using FCms.DbContent.Db;
using FCms.DbContent.Models;

namespace FCms.Tests.DbTests
{
    public class MsSqlgeneratorTest
    {
        [Fact]
        public void TableNameSanitizerTest()
        {
            MsSqlGenerator generator = new MsSqlGenerator("tablename");
            Assert.Equal("test9", DbHelpers.SanitizeDbName(" *'t#)e#*s)(t ):;9"));
        }
    }
}
