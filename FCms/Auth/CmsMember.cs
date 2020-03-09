using System.Collections.Generic;
using System.Security.Claims;

namespace FCms.Auth
{
    public class CmsMember
    {
        public string Username { get; set; }
        public bool IsLoggedIn { get; private set; }

        public CmsMember()
        {
            
        }

        public bool Authenticate(string username, string password)
        {
            ConfigAuthentication configAuthentication = new ConfigAuthentication();
            this.IsLoggedIn = configAuthentication.Authenticate(username, password);
            if (this.IsLoggedIn)
            {
                this.Username = username;
            }
            return this.IsLoggedIn;
        }

        public ClaimsPrincipal getUserPrincipal()
        {
            if (!this.IsLoggedIn)
            {
                return null;
            }

            var cmsClaims = new List<Claim>() { new Claim(ClaimTypes.Name, this.Username) };
            var userIdentity = new ClaimsIdentity(cmsClaims, "cms");
            var userPrincipal = new ClaimsPrincipal(new[] { userIdentity });
            return userPrincipal;
        }
    }
}
