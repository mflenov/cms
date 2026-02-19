using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using FCms.Auth.Abstract;
using FCms.Auth.Concrete;
using Newtonsoft.Json;
    
namespace FCms.Auth.Implementations;

public class CmsUsers : ICmsUsers
{
    const string filename = "users.json";

    static CmsUsers cmsUsers = null;

    private List<CmsUserModel> users = new List<CmsUserModel>();

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
        if (File.Exists(CMSConfigurator.ContentBaseFolder + filename))
        {
            this.users = JsonConvert.DeserializeObject<List<CmsUserModel>>(File.ReadAllText(CMSConfigurator.ContentBaseFolder + filename),
                new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });
        }
        else {
            this.users = new List<CmsUserModel>();
        }
    }

    public void SaveUsersToFile()
    {
        System.IO.File.WriteAllText(CMSConfigurator.ContentBaseFolder + filename, JsonConvert.SerializeObject(this.users, new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented
        }));
}

    public void Add(CmsUserModel user)
    {
        users.Add(user);
        SaveUsersToFile();
    }

    public void Update(CmsUserModel user)
    {
        var u = users.FirstOrDefault(u => u.Id == user.Id);
        if (u != null) {
            u.Username = user.Username;
            u.PasswordHash = user.PasswordHash;
            SaveUsersToFile();  
        }
    }

    public void Delete(Guid id)
    {
        users.RemoveAll(u => u.Id == id);
        SaveUsersToFile();
    }
}
