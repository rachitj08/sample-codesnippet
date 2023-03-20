using Common.Model;
using Sample.Admin.Model;
using Sample.Admin.Model.User;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Sample.Admin.HttpAggregator.IServices
{
    /// <summary>
    ///  Admin Users Service interface
    /// </summary>
    public interface IAdminUsersService
    {
        /// <summary>
        /// To Get List of Admin Users
        /// </summary>
        /// <param name="httpContext">httpContext</param>
        /// <param name="ordering">Ordering Fields: (All Fields)</param>
        /// <param name="search">Search Fields: (UserId, UserName, Email)</param>
        /// <param name="offset">The initial index from which to return the results.</param>
        /// <param name="pageSize">Number of results to return per page</param>
        /// <param name="pageNumber">Page Number of results.</param>
        /// <param name="all">It will return results (all pages) without pagination.</param>
        /// <returns></returns>
        Task<ResponseResultList<AdminUsersModel>> GetAllAdminUsers(HttpContext httpContext, string ordering, string search, int pageSize, int pageNumber, int offset, bool all);

        /// <summary>
        /// Admin User Details
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <returns></returns>
        Task<ResponseResult<AdminUsersModel>> GetAdminUserDetail(int userId);

        /// <summary>
        /// Create Admin User
        /// </summary>
        /// <param name="adminUser"></param>
        /// <returns></returns>
        Task<ResponseResult<UserCreationModel>> CreateAdminUser(UserCreationModel adminUser);

        /// <summary>
        /// To Update existing Module
        /// </summary>
        /// <param name="adminUser">admin user object</param>
        /// /// <param name="userId">Unique user Id</param>
        /// <returns></returns>
        Task<ResponseResult<UserCreationModel>> UpdateAdminUser(int userId, UserCreationModel adminUser);

        /// <summary>
        /// To Update existing Admin User Partial
        /// </summary>
        /// <param name="userId">Unique user Id</param>
        /// <param name="adminUser">admin user object</param>
        /// <returns></returns>
        Task<ResponseResult<UserCreationModel>> UpdatePartialAdminUser(int userId, UserCreationModel adminUser);

        /// <summary>
        /// To Delete existing Admin User
        /// </summary>
        /// <param name="userId">admin user identifier</param>
        /// <returns></returns>
        Task<ResponseResult<AdminUsersModel>> DeleteAdminUser(int userId);

        /// <summary>
        /// Change password of Admin User
        /// </summary>
        /// <param name="model">Change Password Model</param>
        /// <param name="userId">User Id</param>
        /// <returns>return response result</returns>
        Task<ResponseResult<SuccessMessageModel>> ChangePassword(ChangePasswordRequestModel model, int userId);

        /// <summary>
        /// Refresh User Token
        /// </summary>
        /// <param name="tokenRefresh">Token Refresh Model</param>
        /// <returns></returns>
        Task<ResponseResult<RefreshTokenResultModel>> RefreshToken(TokenRefreshModel tokenRefresh);

        /// <summary>
        /// Logout
        /// </summary>
        /// <param name="tokenRefresh">Token Refresh Model</param>
        /// <param name="token">Token</param>
        /// <returns></returns>
        Task<ResponseResult<SuccessMessageModel>> Logout(TokenRefreshModel tokenRefresh, string token);

        /// <summary>
        /// ForgotPassword
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseResult<SuccessMessageModel>> ForgotPassword(ForgotPasswordRequestModel model);

        /// <summary>
        /// Set Password
        /// </summary>
        /// <param name="token"></param>
        /// <param name="uid"></param>
        /// <param name="model">Set Password model</param>
        /// <returns></returns>

        Task<ResponseResult<SuccessMessageModel>> SetPassword(string token, string uid, SetForgotPasswordModel model);

        /// <summary>
        /// To Verify Token
        /// </summary>
        /// <param name="tokenModel">Verify Token Model</param>
        /// <returns></returns>
        Task<ResponseResult> VerifyToken(VerifyTokenModel tokenModel);
    }
}
