using Schaeffler.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schaeffler.Persistence.Interface
{
   public interface IUserAccountDao
    {
        User AuthenticateUser(User UA);

        int SaveUserLoginLog(User UA, out Guid SessionId);

        ForgotPassword SaveForgotPassword(ForgotPassword FPP);

        ForgotPassword VerifyForgotPasswordUniqueId(ForgotPassword FP);

        User UpdatePassword(ForgotPassword FP);

        bool CheckLoginStatus(Guid SessionId, Guid UserGUID);

        int UpdateUserLoginLog(User UA);


      
    }
}
