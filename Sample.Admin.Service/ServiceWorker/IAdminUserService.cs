using Common.Model;
using Sample.Admin.Model;
using Sample.Admin.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Admin.Service.ServiceWorker
{
    public interface IAdminUserService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ordering"></param>
        /// <param name="search"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="offset"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        Task<ResponseResultList<AdminUsersModel>> GetAllAdminUsers(string ordering, string search, int pageSize, int pageNumber, int offset, bool all);

        Task<AdminUsersModel> GetAdminUserDetail(int userId);

        /// <summary>
        /// To Create new Admin User
        /// </summary>
        /// <param name="module">Admin User object</param>
        /// <returns></returns>
        Task<ResponseResult<UserCreationModel>> CreateAdminUser(UserCreationModel adminUser, int loggedInUserId);

        /// <summary>
        /// This service is used for Updating existing admin user
        /// </summary>
        /// <param name="adminUser">The existing admin user object.</param>
        /// <param name="userId">A unique integer value identifying this admin user.</param>
        /// <returns></returns>
        Task<ResponseResult<UserCreationModel>> UpdateAdminUser(int userId, UserCreationModel adminUser, int loggedInUserId);

        /// <summary>
        /// To Update Admin User Partially
        /// </summary>
        /// /// <param name="userId">user Id</param>
        /// <param name="adminUser">New admin user object</param>
        /// <returns></returns>
        Task<ResponseResult<UserCreationModel>> UpdatePartialAdminUser(int userId, UserCreationModel adminUser, int loggedInUserId);

        /// <summary>
        /// To Delete existing Admin User
        /// </summary>
        /// <param name="userId">admin user identifier</param>
        /// <returns></returns>
        Task<int> DeleteAdminUser(int userId);


        /// <summary>
        /// Change password of Admin User
        /// </summary>
        /// <param name="model"></param>
        /// <returns>return response result</returns>
        Task<ResponseResult<SuccessMessageModel>> ChangePassword(ChangePasswordModel model, int loggedInUserId);

        /// <summary>
        ///  Set password of Admin User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseResult<SuccessMessageModel>> SetPassword(SetPasswordModel model, int loggedInUserId);

        /// <summary>
        ///  Forgot password of Admin User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseResult<SuccessMessageModel>> ForgotPassword(ForgotPasswordRequestModel model);

        /// <summary>
        /// To Verify Token
        /// </summary>
        /// <param name="tokenModel">Verify Token Model</param>
        /// <returns></returns>
        Task<ResponseResult> VerifyToken(VerifyTokenModel tokenModel);
    }
}
