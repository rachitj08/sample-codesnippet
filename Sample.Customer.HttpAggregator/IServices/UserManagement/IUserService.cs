using Common.Model;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sample.Customer.Model;
using Sample.Customer.Model.Model.StorageModel;

namespace Sample.Customer.HttpAggregator.IServices.UserManagement
{
    /// <summary>
    ///  User Service
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        ///   To Register new user
        /// </summary>
        /// <param name="user">The new user object</param>
        /// <returns></returns>
        Task<ResponseResult<UserVM>> RegisterUser(CreateUserModel user);

        /// <summary>
        /// To Get List of user modules by their unique id 
        /// </summary>
        /// <param name="userId">user identifier</param>
        /// <returns></returns>
        Task<List<UserModule>> GetUserModulesByUserId(long userId);

        /// <summary>
        /// To Get List of User Groups by their unique id
        /// </summary>
        /// <param name="userId">user identifier</param>
        /// <returns></returns>
        Task<List<UserGroup>> GetUserGroupsByUserId(long userId);

        /// <summary>
        /// Get User Permissions By UserId
        /// </summary>
        /// <param name="userId">The userId is long</param>
        /// <returns></returns>
        Task<List<UserGroup>> GetUserPermissionsByUserId(long userId);

        /// <summary>
        /// Build User Navigation By UserId
        /// </summary>
        /// <param name="userId">user identifier</param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<ResponseResult<List<UserNavigation>>> BuildUserNavigationByUserId(long userId, long accountId);

        /// <summary>
        /// Get Mfa types By their unique id
        /// </summary>
        /// <param name="userId">user identifier</param>
        /// <returns></returns>
        Task<long> GetMfaTypesByUserId(long userId);


        /// <summary>
        /// Change password of User
        /// </summary>
        /// <param name="model">Change Password Model</param>
        /// <param name="accountId">Account Id</param>
        /// <param name="userId">User Id</param>
        /// <returns>return response result</returns>
        Task<ResponseResult<string>> ChangePassword(ChangePasswordRequest model, long accountId, long userId);

        /// <summary>
        /// Create user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseResult<UserVM>> CreateUser(CreateUserModel model);

        /// <summary>
        /// Get User Modules
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accountId"></param>
        /// <param name="ordering"></param>
        /// <param name="offset"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        Task<ResponseResultList<UserModuleModel>> GetUserModules(long userId, long accountId, string ordering, int offset, int pageSize, int pageNumber, bool all);

        /// <summary>
        /// Check User Modules
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accountId"></param>
        /// <param name="moduleName"></param>
        /// <returns>bool</returns>
        Task<bool> CheckUserPermission(long userId, long accountId, string moduleName);


        /// <summary>
        /// Get user list
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="ordering"></param>
        /// <param name="offset"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        Task<ResponseResultList<UserVM>> GetAllUsers(HttpContext httpContext, string ordering, int offset, int pageSize, int pageNumber, bool all);

        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseResult<UserVM>> UpdateUser(long userId, UsersModel model);

        /// <summary>
        /// Get user details
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ResponseResult<UserVM>> GetUserById(long userId);

        /// <summary>
        /// Update user partially
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseResult<UserVM>> UpdatePartialUser(long userId, UsersModel model);

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ResponseResult> DeleteUser(long userId);

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
        /// Forgot Password
        /// </summary>
        /// <param name="model">Forgot Password model</param>
        /// <param name="accountId">Account Id</param>
        /// <returns></returns>
        Task<ResponseResult<SuccessMessageModel>> ForgotPassword(ForgotPasswordRequestModel model, long accountId);

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
        /// <summary>
        /// This api is used for Verify User Email Address for existing User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ResponseResult> VerifyUserEmailAddress(long userId);

        /// <summary>
        /// This api is used for Verify User mobile number for existing User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ResponseResult> VerifyUserMobileNumber(long userId);

        /// <summary>
        /// Send Mobile OTP
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseResult<bool>> SendMobileOTP(SendMobileOtpVM model);

        /// <summary>
        /// Verify OTP
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseResult<User>> VerifyOTP(VerifyOtpVM model);

        /// <summary>
        /// Temperory Method, Just for testing kafka, TODO: Remove it
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<bool> RegisterUserKafka(CreateUserModel user);

        /// <summary>
        /// Get User Profile
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult<UserProfileModel>> GetUserProfile();

        /// <summary>
        /// Update User Profile
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseResult<bool>> UpdateUserProfile(SaveUserProfileModel model);


        /// <summary>
        /// Update User Profile Image
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns> 
        Task<ResponseResult<bool>> UpdateUserProfileImage(UserProfileImageVM model);

        /// <summary>
        /// Send varification code.
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult<SuccessMessageModel>> SendVerificationCode();
        /// <summary>
        /// Change Password By MobileNo
        /// </summary>
        /// <param name="model"></param>
        /// <param name="accountId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ResponseResult<string>> ChangePasswordByMobileNo(ChangePasswordRequestForSMS model, long accountId, long userId);
        /// <summary>
        /// Forget Password By SMS
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseResult<bool>> ForgetPasswordBySMS(SendMobileOtpVM model);
    }
}
