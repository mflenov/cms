using System.Collections.Generic;
using System.Security.Claims;
using FCms.Auth.Abstract;

namespace FCms.Auth.Concrete
{
    public class CmsMember : ICmsMember
    {
        public string Username { get; set; }
        public bool IsLoggedIn { get; private set; }

        private readonly ICmsAuthentication _cmsAuthentication;

        public CmsMember(ICmsAuthentication cmsAuthentication)
        {
            _cmsAuthentication = cmsAuthentication;
        }

        public bool Authenticate(string username, string password)
        {
            this.IsLoggedIn = _cmsAuthentication.Authenticate(username, password);
            if (this.IsLoggedIn)
            {
                this.Username = username;
            }

            return this.IsLoggedIn;
        }

        public ClaimsPrincipal GetUserPrincipal()
        {
            if (!this.IsLoggedIn)
            {
                return null;
            }

            var cmsClaims = new List<Claim>() {new Claim(ClaimTypes.Name, this.Username)};
            var userIdentity = new ClaimsIdentity(cmsClaims, "cms");
            var userPrincipal = new ClaimsPrincipal(new[] {userIdentity});
            return userPrincipal;
        }
    }
}