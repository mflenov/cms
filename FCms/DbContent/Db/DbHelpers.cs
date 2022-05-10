using System.Text.RegularExpressions;

namespace FCms.DbContent.Db
{
    public static class DbHelpers
    {
        public static string SanitizeDbName(string name)
        {
            return Regex.Replace(name, "[^a-zA-Z0-9]", "");
        }
    }
}
