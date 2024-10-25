namespace FCms
{
    public static class CMSConfigurator
    {
        // MS SQL Connection
        const string DEFAULT_MS_DB_CONNECTION = "Data Source=.;Initial Catalog=fcms;Integrated Security=true;Trust Server Certificate=true;";

        // PG Connection
        const string DEFAULT_PG_DB_CONNECTION = "User ID=postgres;Password=password;Host=localhost;Port=5432;Database=fcms";

        public static void Configure(string contentBaseFolder, string dbConnection = null)
        {
            _contentBaseFolder = contentBaseFolder;
            _dbConnection = dbConnection == null ? DEFAULT_PG_DB_CONNECTION : dbConnection;
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
