namespace FCms
{
    public static class CMSConfigurator
    {
        public static void Configure(string contentBaseFolder, string dbConnection = null)
        {
            _contentBaseFolder = contentBaseFolder;
        }

        private static string _contentBaseFolder = "./";
        public static string ContentBaseFolder
        {
            get { return _contentBaseFolder; }
        }
    }
}
