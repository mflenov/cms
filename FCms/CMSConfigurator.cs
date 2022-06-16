using Microsoft.Extensions.DependencyInjection;
namespace FCms
{
    public static class CMSConfigurator
    {
        const string DEFAULT_DB_CONNECTION = "Data Source=.;Initial Catalog=fcms;Integrated Security=true;Trust Server Certificate=true;";

        public static void Configure(string contentBaseFolder, string dbConnection = null)
        {
            _contentBaseFolder = contentBaseFolder;
            _dbConnection = dbConnection == null ? DEFAULT_DB_CONNECTION : dbConnection;
        }

        private static string _contentBaseFolder = "./";
        public static string ContentBaseFolder
        {
            get { return _contentBaseFolder; }
        }

        private static string _dbConnection = "";
        public static string DbConnection
        {
            get { return _dbConnection; }
        }
    }
}
