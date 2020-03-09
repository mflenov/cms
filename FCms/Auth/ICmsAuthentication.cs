using System;
namespace FCms.Auth
{
    public interface ICmsAuthentication
    {
        bool Authenticate(string username, string password);
    }
}
