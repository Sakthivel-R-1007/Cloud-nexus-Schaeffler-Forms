using Dapper;
using Schaeffler.Domain;
using Schaeffler.Persistence.DBConnectionFactory;
using Schaeffler.Persistence.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schaeffler.Persistence.Implementation
{
    public class UserAccountDao : IUserAccountDao
    {
        #region Private variables and constructors

        protected readonly IDbConnectionFactory factory;

        public UserAccountDao()
        {
            this.factory = new DbConnectionFactory("DefaultDB");
        }

        #endregion

        public User AuthenticateUser(User UA)
        {
            User result = null;
            DynamicParameters param = new DynamicParameters();
            param.Add("@UserName", UA.Email, dbType: DbType.String);
            param.Add("@Password", UA.Password, dbType: DbType.String);
            param.Add("@SystemIP", UA.SystemIp, dbType: DbType.String);
            using (IDbConnection conn = factory.GetConnection())
            {
                conn.Open();
                string SQL = @"[USP_AuthenticateUser]";
                result = conn.Query<User, Role, User>(sql: SQL,
                    param: param,
                    map: (U, R) =>
                    {
                        if (U != null)
                        {
                            U.role = R;
                        }
                        return U;
                    },
            commandType: CommandType.StoredProcedure).FirstOrDefault();
                conn.Close();
            }
            return result;
        }

        public int SaveUserLoginLog(User UA, out Guid SessionId)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@UserGUID", UA.GUID, dbType: DbType.Guid);
            param.Add("@SystemIP", UA.SystemIp, dbType: DbType.String);
            param.Add("@GUID", dbType: DbType.Guid, direction: ParameterDirection.Output);
            SessionId = Guid.Empty;
            int result = 0;
            using (IDbConnection conn = factory.GetConnection())
            {
                conn.Open();
                string SQL = @"[USP_SaveUserLoginLog]";
                result = conn.Execute(sql: SQL, param: param, commandType: CommandType.StoredProcedure);
                SessionId = param.Get<Guid>("@GUID");
                conn.Close();
            }
            return result;
        }

        public ForgotPassword SaveForgotPassword(ForgotPassword FPP)
        {
            ForgotPassword result = null;
            DynamicParameters param = new DynamicParameters();
            param.Add("@Email", FPP.user.Email, dbType: DbType.String);
            param.Add("@SystemIP", FPP.SystemIp, dbType: DbType.String);
            using (IDbConnection conn = factory.GetConnection())
            {
                conn.Open();
                string SQL = @"[USP_SaveForgotPassword]";

                result = conn.Query<ForgotPassword, User, ForgotPassword>(sql: SQL,
                    param: param,
                    map: (FP, U) =>
                    {
                        if (FP != null)
                        {
                            FP.user = U;
                        }
                        return FP;
                    },
            commandType: CommandType.StoredProcedure).FirstOrDefault();
                conn.Close();
            }
            return result;
        }

        public ForgotPassword VerifyForgotPasswordUniqueId(ForgotPassword FP)
        {
            ForgotPassword result = null;
            DynamicParameters param = new DynamicParameters();
            param.Add("@UniqueId", FP.UniqueId, dbType: DbType.Guid);
            param.Add("@Id", FP.user.GUID, dbType: DbType.Guid);
            using (IDbConnection conn = factory.GetConnection())
            {
                conn.Open();
                string SQL = @"[USP_VerifyForgotPasswordUniqueId]";
                result = conn.Query<ForgotPassword, User, ForgotPassword>(sql: SQL,
                    param: param,
                    map: (FPD, U) =>
                    {
                        if (FPD != null)
                        {
                            FPD.user = U;
                        }
                        return FPD;
                    },
            commandType: CommandType.StoredProcedure).FirstOrDefault();
                conn.Close();
            }
            return result;
        }

        public User UpdatePassword(ForgotPassword FP)
        {
            User result = null;
            DynamicParameters param = new DynamicParameters();
            param.Add("@UniqueId", FP.UniqueId, dbType: DbType.Guid);
            param.Add("@SystemIP", FP.SystemIp, dbType: DbType.String);
            param.Add("@UserId", FP.user.GUID, dbType: DbType.Guid);
            param.Add("@Password", FP.user.Password, dbType: DbType.String);
            using (IDbConnection conn = factory.GetConnection())
            {
                conn.Open();
                string SQL = @"[USP_UpdatePassword]";
                result = conn.Query<User>(sql: SQL,
                                      param: param,
                                      commandType: CommandType.StoredProcedure).FirstOrDefault();
                conn.Close();
            }
            return result;
        }

        public bool CheckLoginStatus(Guid SessionId, Guid UserGUID)
        {
            DynamicParameters param = new DynamicParameters();
            if (SessionId != Guid.Empty)
                param.Add("@GUID", SessionId, dbType: DbType.Guid);

            if (UserGUID != Guid.Empty)
                param.Add("@UserGUID", UserGUID, dbType: DbType.Guid);

            bool result = false;
            using (IDbConnection conn = factory.GetConnection())
            {
                conn.Open();
                string SQL = @"[USP_CheckLoginStatus]";
                var logs = conn.Query<long>(sql: SQL, param: param, commandType: CommandType.StoredProcedure).ToList();
                result = (logs != null && logs.Count() > 0);
            }
            return result;
        }

        public int UpdateUserLoginLog(User UA)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@GUID", UA.SessionId, DbType.Guid);
            param.Add("@UserGUID", UA.GUID, DbType.Guid);
            param.Add("@IsForcedLogOut", UA.LastLoginStatus, dbType: DbType.Boolean);
            param.Add("@SystemIp", UA.SystemIp, dbType: DbType.String);
            int result = 0;
            using (IDbConnection conn = factory.GetConnection())
            {
                conn.Open();
                string SQL = @"[USP_UpdateUserLoginLog]";
                result = conn.Execute(sql: SQL, param: param, commandType: CommandType.StoredProcedure);
                conn.Close();
            }
            return result;
        }
    }
}
