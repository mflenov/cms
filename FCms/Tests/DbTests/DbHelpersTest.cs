using Xunit;
using FCms.DbContent.Db;

namespace FCmsTests.DbTests;
[Trait("Category", FCmsTests.DbTests.DbHelpersTest.TEST_CATEGORY_BASIC)]

public class DbHelpersTest
{
    public const string TEST_CATEGORY_BASIC = "Basic";
    public const string TEST_CATEGORY_INTEGRATION = "Integration";
    public const string TEST_CATEGORY_MSSQL = "MsSql";
    public const string TEST_CATEGORY_PGSQL = "PgSql";

    [Fact]
    [Trait("Category", TEST_CATEGORY_BASIC)]
    public void TableNameSanitizerTest()
    {
        Assert.Equal("test9", DbHelpers.SanitizeDbName(" *'t#)e#*s)(t ):;9"));
    }
}
