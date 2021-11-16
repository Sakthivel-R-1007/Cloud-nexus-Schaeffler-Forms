using Dapper;
using Schaeffler.Domain;
using Schaeffler.Persistence.DBConnectionFactory;
using Schaeffler.Persistence.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Schaeffler.Persistence.Implementation
{
    public class UserDao : IUserDao
    {
        #region Private variables and constructors

        protected readonly IDbConnectionFactory factory;

        public UserDao()
        {
            this.factory = new DbConnectionFactory("DefaultDb");
        }

        #endregion

        public int Save(User U)
        {
            int result = 0;
            DynamicParameters param = new DynamicParameters();

            if (U.GUID != Guid.Empty)
            {
                param.Add("@GUID", U.GUID, dbType: DbType.Guid);
            }
            param.Add("@Name", U.Name, dbType: DbType.String);
            param.Add("@Email", U.Email, dbType: DbType.String);
            param.Add("@ContactNo", U.ContactNo, dbType: DbType.String);
            param.Add("@Password", U.Password, dbType: DbType.String);
            param.Add("@RoleId", U.role.Id, dbType: DbType.Int64);
            param.Add("@Status", U.IsActive, dbType: DbType.Boolean);
            param.Add("@AdminUserId", U.CreatedBy, dbType: DbType.Guid);
            param.Add("@SystemIP", U.SystemIp, dbType: DbType.String);

            using (IDbConnection conn = factory.GetConnection())
            {
                conn.Open();
                string SQL = @"[USP_Users_Save]";
                result = conn.Execute(sql: SQL, param: param, commandType: CommandType.StoredProcedure);
                conn.Close();
            }
            return result;
        }

        public IList<User> Get(Guid UserGuid, string Keyword = null, Int64 RoleId = 0, int PageIndex = 1, int PageSize = 10)

        {
            IList<User> result = null;
            DynamicParameters _param = new DynamicParameters();
            if (UserGuid != Guid.Empty)
            {
                _param.Add("@UsersId", UserGuid, dbType: DbType.Guid);
            }

            if (!string.IsNullOrEmpty(Keyword))
            {
                _param.Add("@Keyword", Keyword, dbType: DbType.String);
            }

            if (RoleId != 0)
            {
                _param.Add("@RoleId", RoleId, dbType: DbType.Int64);
            }


            _param.Add("@PageIndex", PageIndex, dbType: DbType.Int32);
            _param.Add("@PageSize", PageSize, dbType: DbType.Int32);

            using (IDbConnection conn = factory.GetConnection())
            {
                conn.Open();
                string SQL = @"[USP_Users_GetAllOrById]";
                result = conn.Query<User, Role, User>(sql: SQL,
                   param: _param,
                   map: (U, R) =>
                   {
                       if (U != null)
                       {
                           U.role = R;
                       }
                       return U;
                   }, commandType: CommandType.StoredProcedure).ToList<User>();
                conn.Close();
            }
            return result;
        }


        public int Delete(User C)
        {
            int result = 0;
            DynamicParameters param = new DynamicParameters();

            if (C.GUID != Guid.Empty)
            {
                param.Add("@GUID", C.GUID, dbType: DbType.Guid);
            }

            param.Add("@AdminUserId", C.CreatedBy, dbType: DbType.Guid);
            param.Add("@SystemIP", C.SystemIp, dbType: DbType.String);

            using (IDbConnection conn = factory.GetConnection())
            {
                conn.Open();
                string SQL = @"[USP_User_Delete]";
                result = conn.Execute(sql: SQL, param: param, commandType: CommandType.StoredProcedure);
                conn.Close();
            }
            return result;
        }


        public int CheckUserNameAndMail(string Name, string Email, Guid GUID)
        {
            int result = 0;
            DynamicParameters param = new DynamicParameters();
            if (!string.IsNullOrEmpty(Name))
                param.Add("@Name", Name.Trim(), dbType: DbType.String);
            if (!string.IsNullOrEmpty(Email))
                param.Add("@Email", Email.Trim(), dbType: DbType.String);

            if (GUID != Guid.Empty)
            {
                param.Add("@GUID", GUID, dbType: DbType.Guid);
            }

            using (IDbConnection conn = factory.GetConnection())
            {
                conn.Open();
                string SQL = @"[USP_User_CheckNameAndEmail]";
                result = conn.Query<int>(sql: SQL, param: param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                conn.Close();
            }
            return result;
        }

        public IList<Role> GetRoles()
        {
            IList<Role> R = null;
            using (IDbConnection conn = factory.GetConnection())
            {
                conn.Open();
                string SQL = @"[USP_GetRoles]";
                R = conn.Query<Role>(SQL, commandType: CommandType.StoredProcedure).ToList<Role>();
            }
            return R;

        }

        public int ChangePassword(User user)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@GUID", user.GUID, DbType.Guid);
            param.Add("@CurrentPassword", user.Password, DbType.String);
            param.Add("@NewPassword", user.NewPassword, DbType.String);
            param.Add("@SystemIp", user.SystemIp, DbType.String);
            int result = 0;
            using (IDbConnection conn = factory.GetConnection())
            {
                conn.Open();
                string SQL = @"[USP_ProfileChangePassword]";
                result = conn.Execute(sql: SQL, param: param, commandType: CommandType.StoredProcedure);
                conn.Close();
            }
            return result;
        }
    }
}
