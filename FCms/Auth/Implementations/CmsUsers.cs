using System.Collections.Generic;
using FCms.Auth.Abstract;
using FCms.Auth.Concrete;
    
namespace FCms.Auth.Implementations;

public class CmsUsers : ICmsUsers
{
    private readonly List<CmsUserModel> _users;

    public CmsUsers(string filename)
    {
        
    }

    public List<CmsUserModel> GetAllUsers()
    {
        return _users;
    }
}
