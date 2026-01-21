using FCms.Auth.Abstract;

namespace FCms.Auth.Concrete
{
    public class ConfigAuthentication : ICmsAuthentication
    {
        private readonly CmsUserModel _adminAuthConfig;

        public ConfigAuthentication(CmsUserModel adminAuthConfig)
        {
            _adminAuthConfig = adminAuthConfig;
        }

        public bool Authenticate(string username, string password)
            => _adminAuthConfig.Username == username 
               && _adminAuthConfig.Password == password;
    }
}