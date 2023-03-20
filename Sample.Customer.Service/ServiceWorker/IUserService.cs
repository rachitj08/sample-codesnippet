using Sample.Customer.Service.Infrastructure.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sample.Customer.Model;
using Common.Model;
using User = Sample.Customer.Service.Infrastructure.DataModels.Users;

namespace Sample.Customer.Service.ServiceWorker
{
    public interface IUserService
    {
        /// <summary>
        /// Get All Users Group Mappings
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<UserGroupMappings>> GetAllUsersGroupMappings();

        /// <summary>
        /// Get User Group By UserId
        /// </summary>
        /// <param name="userId">user identifier</param>
        /// <returns></returns>
        Task<List<UserGroup>> GetUserGroupsByUserId(long userId);

        /// <summary>
        /// Build User Navigation By UserId
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns></returns>
        Task<bool> CheckModulePermissionByUserId(long userId, long moduleId);

        /// <summary>
        /// Build User Navigation By UserId
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="accountId">account id</param>
        /// <returns></returns>
        Task<ResponseResult<List<long>>> BuildUserNavigationByUserId(long userId, long accountId);

        /// <summary>
        /// Change password of User
        /// </summary>
        /// <param name="model"></param>
        /// <returns>return response result</returns>
        Task<ResponseResult<string>> ChangePassword(ChangePassword model, long loggedInUserId);

        /// <summary>
        /// Forgot Password of User
        /// </summary> 
        /// <param name="model">Forgot Password Request Model</param>
        /// <returns>return response result</returns>
        Task<ResponseResult<SuccessMessageModel>> ForgotPassword(ForgotPasswordModel model);

        /// <summary>
        /// Set Password of User
        /// </summary> 
        /// <param name="model">Set Password Request Model</param>
        /// <returns>return response result</returns>
        Task<ResponseResult<SuccessMessageModel>> SetPassword(SetPasswordModel model, long loggedInUserId);



        /// <summary>
        /// Get All Users
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="ordering"></param>
        /// <param name="offset"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        Task<ResponseResultList<UserVM>> GetAllUsers(long accountId, string ordering, int offset, int pageSize, int pageNumber, bool all);

        /// <summary>
        /// Get user details
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserVM> GetUserById(long accountId, long userId);
        /// <summary>
        /// GetUserDetails
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserBasicDataModel> GetUserDetails(long accountId, long userId);
        /// <summary>
        /// GetUsers By AccountId
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<List<UserBasicDataModel>> GetUsersByAccountId(long accountId);

        /// <summary>
        /// Update User Profile
        /// </summary>
        /// <param name="accountId">accountId</param>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        Task<ResponseResult<UserProfileModel>> GetUserProfile(long accountId, long userId);

        /// <summary>
        /// Update User Profile
        /// </summary>
        /// <param name="model">User Profile Model</param>
        /// <param name="accountId">accountId</param>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        Task<ResponseResult<bool>> UpdateUserProfile(SaveUserProfileModel model, long accountId, long userId);


        /// <summary>
        /// Update User Profile Image
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="accountId">accountId</param>
        /// <param name="loggedInUserId">loggedInUserId</param>
        /// <returns></returns>
        Task<ResponseResult<bool>> UpdateUserProfileImage(UserProfileImageVM model, long accountId, long loggedInUserId);

        /// <summary>
        /// Create user
        /// </summary>
        /// <param name="model"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<ResponseResult<UserVM>> CreateUser(CreateUserModel model, long accountId, long loggedInUserId, string deviceId);

        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accountId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseResult<UserVM>> UpdateUser(long userId, long accountId, UsersModel model, long loggedInUserId);

        /// <summary>
        /// Update user partially
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseResult<UserVM>> UpdatePartialUser(long userId, long accountId, UsersModel model, long loggedInUserId);

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<long> DeleteUser(long userId);
        Task<ResponseResult> VerifyToken(VerifyTokenModel tokenModel);

        /// <summary>
        /// This api is used for Verify User Email Address for existing User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ResponseResult> VerifyUserEmailAddress(long accountId,long userId);

        /// <summary>
        /// This api is used for Verify User mobile number for existing User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ResponseResult> VerifyUserMobileNumber(long accountId ,long userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootUserSetupModel"></param>
        /// <returns></returns>
        Task<ResponseResult> RootUserSetup(RootUserSetupModel rootUserSetupModel);

        /// <summary>
        /// CreateDefaultUser
        /// </summary>
        /// <param name="objAccountInformation"></param>
        /// <param name="accountId"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        Task<User> CreateDefaultUser(RootUserSetupModel objRootUserSetupModel, long accountId, long loggedInUserId);


        /// <summary>
        /// SendMobileOTP
        /// </summary>
        /// <param name="model"></param>
        /// <param name="accountId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        Task<ResponseResult<bool>> SendMobileOTP(SendMobileOtpMainVM model, long accountId, long loggedInUserId, string deviceId, string apiName);

        /// <summary>
        /// Verify OTP
        /// </summary>
        /// <param name="model"></param>
        /// <param name="accountId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        Task<ResponseResult<bool>> VerifyOTP(VerifyOtpVM model, long accountId, long loggedInUserId, string deviceId);

        /// <summary>
        /// Send email verification code.
        /// </summary>
        /// <param name="emailid"></param>
        /// <returns></returns>
        Task<ResponseResult<SuccessMessageModel>> SendVerificationCode(long accountId,long userId);

        /// <summary>
        /// Update Stripe CustomerId
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="userId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<User> UpdateStripeCustomerId(long accountId, long userId, string customerId);
        /// <summary>
        /// Change Password By Mobile
        /// </summary>
        /// <param name="changePassword"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        Task<ResponseResult<string>> ChangePasswordByMobile(ChangePasswordMobile changePassword, long loggedInUserId);
        /// <summary>
        /// Forget Password By SMS
        /// </summary>
        /// <param name="model"></param>
        /// <param name="accountId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="deviceId"></param>
        /// <param name="apiName"></param>
        /// <returns></returns>
        Task<ResponseResult<bool>> ForgetPasswordBySMS(SendMobileOtpMainVM model, long accountId, long loggedInUserId, string deviceId, string apiName);
        // TODO: Remove It After Initial Testing
        void KafkaTest(CreateUserModel model);
        /// <summary>
        /// VerifyAndAuthenticate
        /// </summary>
        /// <param name="model"></param>
        /// <param name="accountId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        Task<ResponseResult> VerifyAndAuthenticate(VerifyOtpVM model, long accountId, long loggedInUserId, string deviceId);
    }
}
