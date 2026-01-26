using System.Collections.Generic;
using FCms.Auth.Abstract;
using FCms.Auth.Concrete;
    
namespace FCms.Auth.Implementations;

public class CmsUsers : ICmsUsers
{
    static CmsUsers cmsUsers = null;

    private readonly List<CmsUserModel> users = new List<CmsUserModel>();

    private CmsUsers()
    {
        LoadUsersFromFile();
    }

    public static CmsUsers GetInstance()
    {
        if (cmsUsers == null)
        {
            cmsUsers = new CmsUsers();
        }
        return cmsUsers;
    }

    public List<CmsUserModel> GetAllUsers()
    {
        return users;
    }

    public void LoadUsersFromFile()
    {
        users.Add(new CmsUserModel() { Id = System.Guid.NewGuid(), Username = "admin" });
    }
}
