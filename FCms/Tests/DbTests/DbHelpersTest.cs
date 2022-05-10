using Xunit;
using FCms.DbContent.Db;

namespace FCmsTests.DbTests
{
    public class DbHelpersTest
    {
        [Fact]
        public void TableNameSanitizerTest()
        {
            Assert.Equal("test9", DbHelpers.SanitizeDbName(" *'t#)e#*s)(t ):;9"));
        }
    }
}
