using System.Collections.Generic;
using FCms.Auth.Concrete;

namespace FCms.Auth.Abstract;

public interface ICmsUsers
{
    List<CmsUserModel> GetAllUsers();
}
