using Common.Model;
using Sample.Admin.Service.Infrastructure.DataModels;
using Sample.Admin.Model;
using Sample.Admin.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Admin.Service.Infrastructure.Repository
{
    public interface IAdminUserRepository
    {
        /// <summary>
        /// To Create  New User
        /// </summary>
        /// <param name="user">user object</param>
        /// <returns></returns>
        Task<AdminUsers> CreateUser(AdminUsers userModel);

        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<AdminUsers> UpdateUser(AdminUsers user);

        /// <summary>
        /// To Get User By User Name
        /// </summary>
        /// <param name="userName">The userName to get user</param>
        /// <returns></returns>
        Task<AdminUsers> GetUserByUserName(string userName);

        Task<ResponseResultList<AdminUsersModel>> GetAllAdminUsers(string ordering, string search, int pageSize, int pageNumber, int offset, bool all);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<AdminUsersModel> GetAdminUserDetail(int userId);

        /// <summary>
        /// To Get Admin User By User Id
        /// </summary>
        /// <param name="userId">The userId to get admin user</param>
        /// <returns>User</returns>
        Task<AdminUsers> GetAdminUserByUserId(int userId);

        /// <summary>
        /// To Get Admin User ByAdmin User email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<AdminUsers> GetAdminUserByEmailId(string email);

        /// <summary>
        /// To Create new Admin User
        /// </summary>
        /// <param name="module">Admin User object</param>
        Task<AdminUsers> CreateAdminUser(AdminUsers adminUser);

        /// <summary>
        /// To Create new Admin User Groups
        /// </summary>
        /// <param name="adminUserGroups">Admin User Groups</param>
        Task CreateUserGroupMappings(List<UserGroupMappings> adminUserGroups, int updateUserId = 0);

        /// <summary>
        /// To Create new Admin User Rights
        /// </summary>
        /// <param name="adminUserRights">Admin User Rights</param>
        Task CreateUserRights(List<UserRights> adminUserRights, int updateUserId = 0);

        // <summary>
        /// To Update Admin User 
        /// </summary>
        /// /// <param name="userId">user Id</param>
        /// <param name="adminUser">New admin user object</param>
        /// <returns></returns>
        Task<AdminUsers> UpdateAdminUser(AdminUsers adminUser);


        /// <summary>
        /// Change password of User
        /// </summary>
        /// <param name="adminUser"></param>
        /// <returns>return Admin User</returns>
        Task<AdminUsers> ChangePassword(AdminUsers adminUser);

        /// <summary>
        /// To Delete Admin User
        /// </summary>
        /// <param name="userId">The userId to delete</param>
        /// <returns></returns>
        Task<int> DeleteAdminUser(int userId);

        /// <summary>
        /// To Get User By User Id
        /// </summary>
        /// <param name="userId">The userId to get user</param>
        /// <returns></returns>
        Task<AdminUsers> GetUserByUserId(int userId);

        /// <summary>
        /// To Verify Token
        /// </summary>
        /// <param name="tokenModel">Verify Token Model</param>
        /// <returns></returns>
        Task<Boolean> VerifyToken(VerifyTokenModel tokenModel);
        //added by Raj for validate phone no
        /// <summary>
        /// To Get Admin User ByAdmin User Mobile
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        Task<AdminUsers> GetAdminUserByMobile(string mobile);
    }
}
