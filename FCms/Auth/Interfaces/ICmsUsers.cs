using System;
using System.Collections.Generic;
using FCms.Auth.Concrete;

namespace FCms.Auth.Abstract;

public interface ICmsUsers
{
    List<CmsUserModel> GetAllUsers();

    void Add(CmsUserModel user);

    void Update(CmsUserModel user);

    void Delete(Guid id);
}
