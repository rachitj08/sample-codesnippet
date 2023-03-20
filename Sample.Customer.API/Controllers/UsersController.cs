using Logger;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.Model;
using Sample.Customer.Service.ServiceWorker;

namespace Sample.Customer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseApiController
    {
        private readonly IUserService userService;

        /// <summary>
        /// Users Controller constructor to Inject dependency
        /// </summary>
        /// <param name="userService">user service class</param>
        public UsersController(IUserService userService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(userService), userService);
            this.userService = userService;
        }

        ///// <summary>
        ///// Get User Modules
        ///// </summary>
        ///// <param name="ordering"></param>
        ///// <param name="search"></param>
        ///// <param name="pageSize"></param>
        ///// <param name="pageNumber"></param>
        ///// <param name="offset"></param>
        ///// <param name="all"></param>
        ///// <returns></returns>
        //[Route("getUserModules")]
        //[HttpGet]
        //public async Task<IActionResult> GetUserModules([FromQuery] long userId, [FromQuery] string ordering, [FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] int offset, [FromQuery] bool all)
        //{

        //    return await Execute(async () =>
        //    {
        //        var userModules = await this.userService.GetUserModules(userId, ordering, pageSize, pageNumber, offset, all);
        //        return Ok(userModules);
        //    });
        //}

        // GET: api/<UsersController>
        /// <summary>
        ///  Get All Users Group Mappings
        /// </summary>
        /// <returns>return user Group Mappings</returns>
        [Route("getallusersgroupmappings")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsersGroupMappings()
        {

            return await Execute(async () =>
            {
                var userGroupMappings = await this.userService.GetAllUsersGroupMappings();
                return Ok(userGroupMappings);
            });

        }

        // GET: api/<UsersController>
        /// <summary>
        /// Get User Group by user Id
        /// </summary>
        /// <param name="userId">user identifier</param>
        /// <returns>return user Groups</returns>
        [Route("getusergroupsbyuserid")]
        [HttpGet]
        public async Task<IActionResult> GetUserGroupsByUserId(long userId)
        {
            return await Execute(async () =>
            {
                var userGroups = await this.userService.GetUserGroupsByUserId(userId);
                return Ok(userGroups);
            });

        }

        // GET: api/<UsersController>
        /// <summary>
        /// Check Module Permission By UserId
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="moduleId">Module Id</param>
        /// <returns>return Module Permission</returns>
        [Route("checkmodulepermissionbyuserid")]
        [HttpGet]
        public async Task<IActionResult> CheckModulePermissionByUserId(long userId, long moduleId)
        {
            return await Execute(async () =>
            {
                var result = await this.userService.CheckModulePermissionByUserId(userId, moduleId);
                return Ok(result);
            });

        }

        //// GET: api/<UserController>
        ///// <summary>
        ///// Get User Permissions by user Id
        ///// </summary>
        ///// <param name="userId">user identifier</param>
        ///// <returns>return user permissions</returns>
        //[Route("getuserpermissionsbyuserid")]
        //[HttpGet]
        //public async Task<IActionResult> GetUserPermissionsByUserId(long userId)
        //{
        //    return await Execute(async () =>
        //    {
        //       var userPermissions = await this.userService.GetUserPermissionsByUserId(userId);
        //       return Ok(userPermissions);
        //    });
        //}

        // GET: api/<UserController>
        /// <summary>
        /// Build User Navigation By UserId
        /// </summary>
        /// <param name="userId">user identifier</param>
        /// <returns>return user permissions</returns>
        [Route("{userId}/buildUserNavigation")]
        [HttpGet]
        public async Task<IActionResult> BuildUserNavigationByUserId([FromRoute] long userId)
        {
            return await Execute(async () =>
            {
                var userPermissions = await this.userService.BuildUserNavigationByUserId(userId, loggedInAccountId);
                return Ok(userPermissions);
            });
        }



        // POST api/<AccountsController> 
        /// <summary>
        /// To Register New User
        /// </summary>
        /// <param name="user">user object</param>
        /// <returns>return registered user Id</returns>
        [Route("registeruser")]
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] CreateUserModel user)
        {
            return await Execute(async () =>
            {
                var userId = await this.userService.CreateUser(user, loggedInAccountId, loggedInUserId, deviceId);
                return Ok(userId);
            });
        }

        // PUT api/<AccountsController> 
        /// <summary>
        /// Change password of User
        /// </summary>
        /// <param name="Password"></param>
        /// <param name="OldPassword"></param>
        /// <returns>return response result</returns>
        [Route("ChangePassword")]
        [HttpPut]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {
            return await Execute(async () =>
            {
                var result = await this.userService.ChangePassword(model, loggedInUserId);
                return Ok(result);
            });
        }

        // POST api/<AccountsController> 
        /// <summary>
        /// Forgot Password of User
        /// </summary>
        /// <param name="model">Forgot Password Request Model</param>
        /// <returns>return response result</returns>
        [Route("forgotPassword")]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            return await Execute(async () =>
            {
                var result = await this.userService.ForgotPassword(model);
                return Ok(result);
            });
        }

        // POST api/<AccountsController> 
        /// <summary>
        /// Set Password of User
        /// </summary>
        /// <param name="model">Set Password Request Model</param>
        /// <returns>return response result</returns>
        [Route("setPassword")]
        [HttpPost]
        public async Task<IActionResult> SetPassword(SetPasswordModel model)
        {
            return await Execute(async () =>
            {
                var result = await this.userService.SetPassword(model, loggedInUserId);
                return Ok(result);
            });
        }

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetUsers(string ordering, int offset, int pageSize, int pageNumber, bool all = false)
        {
            return await Execute(async () =>
            {
                var result = await this.userService.GetAllUsers(loggedInAccountId, ordering, offset, pageSize, pageNumber, all);
                if (result != null)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }

        /// <summary>
        /// This api is used for Creating new User
        /// </summary>
        /// <param name="user">The new user object.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserModel model)
        {
            return await Execute(async () =>
            {
                var result = await this.userService.CreateUser(model, loggedInAccountId, loggedInUserId, deviceId);
                return Ok(result);
            });
        }

        /// <summary>
        /// Get User Profile
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getUserProfile")]
        public async Task<IActionResult> GetUserProfile()
        {
            return await Execute(async () =>
            {
                var result = await this.userService.GetUserProfile(loggedInAccountId, loggedInUserId);
                return Ok(result);
            });
        }

        /// <summary>
        /// Update User Profile
        /// </summary>
        /// <param name="model">User Profile Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("updateuserprofile")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] SaveUserProfileModel model)
        {
            return await Execute(async () =>
            {
                var result = await this.userService.UpdateUserProfile(model, loggedInAccountId, loggedInUserId);
                return Ok(result);
            });
        }

        /// <summary>
        /// Update User Profile Image
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("updateuserprofileimage")]
        public async Task<IActionResult> UpdateUserProfileImage([FromBody] UserProfileImageVM model)
        {
            return await Execute(async () =>
            {
                var result = await this.userService.UpdateUserProfileImage(model, loggedInAccountId, loggedInUserId);
                return Ok(result);
            });
        }

        /// <summary>
        /// This api is used for Updating existing User
        /// </summary>
        /// <param name="model">The existing user object.</param>
        /// <param name="userId">A unique integer value identifying this User.</param>
        /// <returns></returns>
        [Route("{userId}")]
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromRoute] long userId, [FromBody] UsersModel model)
        {
            return await Execute(async () =>
            {
                var result = await this.userService.UpdateUser(userId, loggedInAccountId, model, loggedInUserId);
                return Ok(result);
            });

        }

        /// <summary>
        ///  Information for user by id
        /// </summary>
        /// <param name="userId">A unique integer value identifying this User.</param>
        /// <returns></returns>
        [Route("{userId}")]
        [Route("{userId}/GetUserById")]
        [HttpGet]
        public async Task<IActionResult> GetUserById([FromRoute] long userId)
        {
            return await Execute(async () =>
            {
                var result = await this.userService.GetUserById(loggedInAccountId, userId);
                return Ok(result);
            });
        }

        /// <summary>
        ///  Information for user by id
        /// </summary>
        /// <param name="userId">A unique integer value identifying this User.</param>
        /// <returns></returns>
        [Route("{userId}/GetUserDetails")]
        [HttpGet]
        public async Task<IActionResult> GetUserDetails([FromRoute] long userId)
        {
            return await Execute(async () =>
            {
                var result = await this.userService.GetUserDetails(loggedInAccountId, userId);
                return Ok(result);
            });
        }
        [Route("{accountId}/GetUsersByAccountId")]
        [HttpGet]
        public async Task<IActionResult> GetUsersByAccountId([FromRoute] long accountId)
        {
            return await Execute(async () =>
            {
                var result = await this.userService.GetUsersByAccountId(accountId);
                return Ok(result);
            });
        }
        /// <summary>
        /// This api is used for Updating existing User partially
        /// </summary>
        /// <param name="model">The existing user object.</param>
        /// <param name="userId">A unique integer value identifying this User.</param>
        /// <returns></returns>
        [Route("{userId}")]
        [HttpPatch]
        public async Task<IActionResult> UpdatePartialUser([FromRoute] long userId, [FromBody] UsersModel model)
        {
            return await Execute(async () =>
            {
                var result = await this.userService.UpdatePartialUser(userId, loggedInAccountId, model, loggedInUserId);
                return Ok(result);
            });
        }

        /// <summary>
        /// This api is used for deleing User
        /// </summary>
        /// <param name="userId">user identifier</param>
        /// <returns></returns>
        [Route("{userId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromRoute] long userId)
        {
            return await Execute(async () =>
            {
                var result = await this.userService.DeleteUser(userId);
                return Ok(result);
            });
        }

        /// <summary>
        ///  Verify token for User
        /// </summary>
        /// <param name="tokenModel">Verify Token Model</param>
        /// <returns></returns>
        [Route("verify")]
        [HttpPost]
        public async Task<IActionResult> VerifyToken([FromBody] VerifyTokenModel tokenModel)
        {
            return await Execute(async () =>
            {
                var result = await this.userService.VerifyToken(tokenModel);
                return Ok(result);
            });
        }

        /// <summary>
        /// This api is used for Verify User Email Address for existing User
        /// </summary>
        /// <param name="model">The existing user object.</param>
        /// <param name="userId">A unique integer value identifying this User.</param>
        /// <returns></returns>
        [Route("VerifyUserEmailAddress/{userId}")]
        [HttpPut]
        public async Task<IActionResult> VerifyUserEmailAddress([FromRoute] long userId)
        {
            return await Execute(async () =>
            {
                var result = await this.userService.VerifyUserEmailAddress(loggedInAccountId, userId);
                return Ok(result);
            });

        }


        /// <summary>
        /// This api is used for Verify User Mobile Number for existing User
        /// </summary>
        /// <param name="model">The existing user object.</param>
        /// <param name="userId">A unique integer value identifying this User.</param>
        /// <returns></returns>
        [Route("VerifyUserMobileNumber/{userId}")]
        [HttpPut]
        public async Task<IActionResult> VerifyUserMobileNumber([FromRoute] long userId)
        {
            return await Execute(async () =>
            {
                var result = await this.userService.VerifyUserMobileNumber(loggedInAccountId, userId);
                return Ok(result);
            });

        }
        /// <summary>
        /// RootUserSetup
        /// </summary>
        /// <param name="model">The existing user object.</param>
        /// <param name="userId">A unique integer value identifying this User.</param>
        /// <returns></returns>
        [Route("RootUserSetup")]
        [HttpPost]
        public async Task<IActionResult> RootUserSetup(RootUserSetupModel rootUserSetupModel)
        {
            return await Execute(async () =>
            {
                var result = await this.userService.RootUserSetup(rootUserSetupModel);
                return Ok(result);
            });

        }

        /// <summary>
        ///  Send Mobile OTP
        /// </summary>
        [HttpPost]
        [Route("SendMobileOTP")]
        public async Task<IActionResult> SendMobileOTP([FromBody] SendMobileOtpMainVM model)
        {
            return await Execute(async () =>
            {
                var result = await this.userService.SendMobileOTP(model, loggedInAccountId, loggedInUserId, deviceId, apiName);
                return Ok(result);
            });
        }

        /// <summary>
        ///  Verify OTP
        /// </summary>
        [HttpPost]
        [Route("VerifyOTP")]
        public async Task<IActionResult> VerifyOTP([FromBody] VerifyOtpVM model)
        {
            return await Execute(async () =>
            {
                var result = await this.userService.VerifyAndAuthenticate(model, loggedInAccountId, loggedInUserId, deviceId);
                return Ok(result);
            });
        }

        /// <summary>
        /// This action method is to send email verification code.
        /// </summary>
        /// <param name="emailid"></param>
        /// <returns></returns>
        [Route("SendVerificationCode")]
        [HttpPost]
        public async Task<IActionResult> SendVerificationCode()
        {
            return await Execute(async () =>
            {
                var result = await this.userService.SendVerificationCode(loggedInAccountId,loggedInUserId);
                return Ok(result);
            });

        }
        [Route("ChangePasswordByMobileNo")]
        [HttpPut]
        public async Task<IActionResult> ChangePasswordByMobileNo(ChangePasswordMobile model)
        {
            
            return await Execute(async () =>
            {
                model.AccountId = loggedInAccountId;
                var result = await this.userService.ChangePasswordByMobile(model, loggedInUserId);
                return Ok(result);
            });
        }

        /// <summary>
        ///  Forget Password By SMS
        /// </summary>
        [HttpPost]
        [Route("ForgetPasswordBySMS")]
        public async Task<IActionResult> ForgetPasswordBySMS([FromBody] SendMobileOtpMainVM model)
        {
            return await Execute(async () =>
            {
                var result = await this.userService.ForgetPasswordBySMS(model, loggedInAccountId, loggedInUserId, deviceId, apiName);
                return Ok(result);
            });
        }
    }
}
