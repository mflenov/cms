using FCms.DbContent.Db;
using NUnit.Framework;

namespace FCmsTests.DbTests
{
    public class DbHelpersTest
    {
        [Test]
        public void TableNameSanitizerTest()
        {
            Assert.That(DbHelpers.SanitizeDbName(" *'t#)e#*s)(t ):;9"), Is.EqualTo("test9"));
        }
    }
}
