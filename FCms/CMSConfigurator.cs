using Microsoft.Extensions.DependencyInjection;
namespace FCms
{
    public static class CMSConfigurator
    {
        public static void Configure(string contentBaseFolder, string dbConnection = "")
        {
            _contentBaseFolder = contentBaseFolder;
            _dbConnection = dbConnection;
        }

        private static string _contentBaseFolder = "";
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
