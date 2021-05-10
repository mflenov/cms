using FCms.Auth.Abstract;
using Microsoft.Extensions.Configuration;


namespace FCms.Auth.Concrete
{
    public class ConfigAuthentication : ICmsAuthentication
    {
        private readonly AdminAuthConfig _adminAuthConfig;

        public ConfigAuthentication(AdminAuthConfig adminAuthConfig)
        {
            _adminAuthConfig = adminAuthConfig;
        }

        public bool Authenticate(string username, string password)
            => _adminAuthConfig.Username == username 
               && _adminAuthConfig.Password == password;
    }
}