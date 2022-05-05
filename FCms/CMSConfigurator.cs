using Microsoft.Extensions.DependencyInjection;
namespace FCms
{
    public static class CMSConfigurator
    {
        public static void Configure(string contentBaseFolder)
        {
            _contentBaseFolder = contentBaseFolder;
        }

        private static string _contentBaseFolder = "";
        public static string ContentBaseFolder
        {
            get { return _contentBaseFolder; }
        }
    }
}
