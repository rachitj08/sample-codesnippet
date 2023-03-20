using Common.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;
using Sample.Customer.Model;
using User = Sample.Customer.Service.Infrastructure.DataModels.Users;
using VerifyTokenModel = Sample.Customer.Model.VerifyTokenModel;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IUserRepository
    {
        ///// <summary>
        ///// User modules by their userid
        ///// </summary>
        ///// <param name="userId"> user identifier</param>
        ///// <returns></returns>
        //Task<List<UserModule>> GetUserModulesByUserId(long userId);

        /// <summary>
        /// user group mapping for all
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<UserGroupMappings>> GetAllUsersGroupMappings();

        /// <summary>
        /// user group for specific user
        /// </summary>
        /// <param name="userId">user identifier</param>
        /// <returns></returns>
        Task<List<UserGroup>> GetUserGroupsByUserId(long userId);

        ///// <summary>
        ///// Get User Permissions By UserId
        ///// </summary>
        ///// <param name="userId">user identifier</param>
        ///// <returns></returns>
        //Task<List<UserGroup>> GetUserPermissionsByUserId(long userId);

        /// <summary>
        /// To Create new User
        /// </summary>
        /// <param name="user">user object</param>
        /// <returns></returns>
        Task<User> CreateUser(User userModel); 
        /// <summary>
        /// To Create new User
        /// </summary>
        /// <param name="user">user object</param>
        /// <returns></returns>
        Task<User> CreateDefaultUser(User userModel);

        /// <summary>
        /// To Get User By User Name
        /// </summary>
        /// <param name="userName">The userName to get user</param>
        /// <param name="accountId">The accountId to get user</param>
        /// <returns></returns>
        Task<User> GetUserByUserName(string userName, long accountId);

        //added by Raj for validate MobileNo
        /// <summary>
        /// To Get User By Mobile
        /// </summary>
        /// <param name="Mobile">The Mobile No to get user</param>
        /// <param name="accountId">The accountId to get user</param>
        /// <returns></returns>
        Task<User> GetUserByUserMobile(string mobile, long accountId);

        /// <summary>
        /// To Get User details with rights By User Name
        /// </summary>
        /// <param name="accountId">The accountId to get user</param>
        /// <param name="userName">The userName to get user</param>
        /// <returns></returns>
        Task<User> GetUserWithRightsByUserName(long accountId, string userName);

        /// <summary>
        /// To Get User By EmailId
        /// </summary>
        /// <param name="emailId">The emailId to get user</param>
        /// <param name="accountId">The accountId to get user</param>
        /// <returns></returns>
        Task<User> GetUserByEmailId(string emailId, long accountId);

        /// <summary>
        /// To Get User By User Id
        /// </summary>
        /// <param name="userId">The userId to get user</param>
        /// <param name="accountId">The accountId to get user</param>
        /// <returns>User</returns>
        Task<User> GetUserByUserId(long accountId, long userId);


        ///// <summary>
        ///// To User Navigation Links Module Wise
        ///// </summary>
        ///// <param name="userId">User Identifier</param>
        ///// <returns></returns>
        //Task<List<UserNavigation>> GetUserModuleNavigation(long userId);

        ///// <summary>
        ///// To Get User Rights Modules
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //Task<List<UserRightsNavigation>> GetUserRightsNavigation(long userId);

        /// <summary>
        /// Get User Rights by user id
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <param name="accountId">account Id</param>
        /// <returns>List<UserRightDetail></returns>
        Task<List<UserRightDetail>> GetUserRights(long userId, long accountId);

        /// <summary>
        /// Get User group Rights by User Id
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <param name="accountId">account Id</param>
        /// <returns>List<long></returns>
        Task<List<long>> GetUserGroupRights(long userId, long accountId);

        /// <summary>
        /// Check Module For User Group Right
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <returns>List<long></returns>
        Task<int> CheckModuleForUserGroup(long userId, long moduleId);

        /// <summary>
        /// Check Module For User Right
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <param name="userId">module Id</param>
        /// <returns>List<UserRightDetail></returns>
        Task<int> CheckModuleForUserRight(long userId, long moduleId);

        /// <summary>
        /// Change password of User
        /// </summary>
        /// <param name="User"></param>
        /// <returns>return User</returns>
        Task<User> ChangePassword(User user);

        /// <summary>
        /// Get user list
        /// </summary>
        /// <param name="ordering"></param>
        /// <param name="offset"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        Task<ResponseResultList<UserVM>> GetAllUsers(long accountId, string ordering, int offset, int pageSize, int pageNumber, bool all);

        /// <summary>
        /// Get user by UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<User> GetUser(long accountId, long userId);
        /// <summary>
        /// GetUsers By AccountId
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<List<User>> GetUsersByAccountId(long accountId);
        /// <summary>
        /// Update User
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        Task<User> UpdateUser(User userModel);

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<long> DeleteUser(long userId);
        /// <summary>
        /// verify token
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        Task<Boolean> VerifyToken(VerifyTokenModel tokenModel);

        /// <summary>
        /// To Get User By User External Id & authentication category
        /// </summary>
        /// <param name="userExternalId">The Externa ld to get user</param>
        /// <param name="authenticationCategory">The authentication category to get user</param>
        /// <param name="accountId">The accountId to get user</param>
        /// <returns></returns>
        Task<User> GetUserByUserExternalId(long accountId, string userExternalId, string authenticationCategory);

        Task<int> CreateOtpLog(OtpLog model);
        Task<OtpLog> GetOtpLog(long accountId, string deviceId, string countryCode, string contactNumber, string email);
        Task<int> UpdateOtpLog(OtpLog model);

        User UpdateStripeId(User user);

        Task<bool> VerifyOTP(string mobileNo);

        /// <summary>
        /// get User by email id to by email address
        /// </summary>
        /// <param name="emailid"></param>
        /// <returns></returns>
        //Task<User> SendVerificationCode(string emailid);
        
        Task<User> GetUserDetails(long userId, long accountId);

        /// <summary>
        /// GetApplicationByUser
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>

        Task<bool> GetApplicationByUser(long userId, long accountId,long applicationId);

        Task<User> UpdateUserNew(User userModel);
    }
}
