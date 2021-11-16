using Schaeffler.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schaeffler.Persistence.Interface
{
    public interface IUserDao
    {
        int Save(User U);
        IList<User> Get(Guid UserGuid, string Keyword = null, Int64 RoleId = 0, int PageIndex = 1, int PageSize = 10);
        int Delete(User U);
        int CheckUserNameAndMail(string Name, string Email, Guid GUID);
        IList<Role> GetRoles();

        int ChangePassword(User user);
    }
}
