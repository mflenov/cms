using System;
using System.Collections.Generic;
using System.Linq;
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

    public void Add(CmsUserModel user)
    {
        users.Add(user);
    }

    public void Update(CmsUserModel user)
    {
        var u = users.FirstOrDefault(u => u.Id == user.Id);
        if (u != null) {
            u.Username = user.Username;
            u.PasswordHash = user.PasswordHash;
        }
    }

    public void Delete(Guid id)
    {
        users.RemoveAll(u => u.Id == id);
    }
}
