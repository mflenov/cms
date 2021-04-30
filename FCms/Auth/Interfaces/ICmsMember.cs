using System.Security.Claims;

namespace FCms.Auth.Abstract
{
    public interface ICmsMember
    {
        bool Authenticate(string username, string password);
        ClaimsPrincipal GetUserPrincipal();
    }
}