namespace FCms.Auth.Abstract
{
    public interface ICmsAuthentication
    {
        bool Authenticate(string username, string password);
    }
}
