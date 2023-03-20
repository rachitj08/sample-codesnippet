using Common.Model;
using Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.HttpAggregator.IServices.UserManagement;
using Sample.Customer.Model;
using UserNavigation = Sample.Customer.Model.UserNavigation;

namespace Sample.Customer.HttpAggregator.Controllers.UserManagement
{
    /// <summary>
    /// Users Controller
    /// </summary>
    [Route("api/users")]
    [ApiController]
    public class UsersController : BaseApiController
    {
        private readonly IUserService usersService;

        /// <summary>
        /// Users Controller Constructor to inject services
        /// </summary>
        /// <param name="usersService">The user service</param>
        /// <param name="logger">The file logger</param>
        public UsersController(IUserService usersService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(usersService), usersService);

            this.usersService = usersService;
        }

        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="userModel">The user model which have all properties to Register user</param>
        /// <returns></returns>
        [Route("registeruser")]
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] CreateUserModel userModel)
        {
            Check.Argument.IsNotNull(nameof(userModel), userModel);

            return await Execute(async () =>
            {
                var result = await usersService.RegisterUser(userModel);
                if (result.ResponseCode == ResponseCode.RecordSaved)
                    return Ok(result);
                else
                    return BadRequest(result);
            });

        }

        /// <summary>
        /// TODO: Remove it , just for testing.
        /// </summary>
        [Route("registeruserkafkatest")]
        [HttpPost]
        public async Task<IActionResult> RegisterUserKafkaTest([FromBody] CreateUserModel userModel, string accountName = "account")
        {

            Check.Argument.IsNotNull(nameof(userModel), userModel);

            ResponseResult responseResult = new ResponseResult();
            return await Execute(async () =>
            {
                var userId = await usersService.RegisterUserKafka(userModel);
                responseResult.Data = userId;
                responseResult.Message = ResponseMessage.RecordSaved;
                return Ok(responseResult);
            });

        }

        /// <summary>
        /// Get User Modules by user Id
        /// </summary>
        /// <param name="ordering">Ordering Fields: (All Fields)</param>
        /// <param name="offset">The initial index from which to return the results.</param>
        /// <param name="pageSize">Number of results to return per page.</param>
        /// <param name="pageNumber">Page Number of results.</param>
        /// <param name="all">It will return results (all pages) without pagination.</param>
        /// <returns></returns>
        [Route("modules")]
        [HttpGet]
        [HasPermission(PermissionType.View)]
        [ProducesResponseType(typeof(ResponseResultList<UserModuleModel>), 200)]
        public async Task<IActionResult> GetUserModules([FromQuery] string ordering, [FromQuery] int offset, [FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] bool all = false)
        {
            return await Execute(async () =>
            {
                var userModules = await usersService.GetUserModules(loggedInUserId, headerAccountId, ordering, offset, pageSize, pageNumber, all);
                if (userModules != null)
                {
                    if (userModules.ResponseCode == ResponseCode.RecordFetched)
                    {
                        return Ok(userModules);
                    }
                    else
                    {
                        return BadRequest(userModules);
                    }
                }
                else
                {
                    return BadRequest(new ResponseResult<UserModuleModel>
                    {
                        ResponseCode = ResponseCode.NoRecordFound,
                        Message = ResponseMessage.NoRecordFound,
                        Error = new ErrorResponseResult
                        {
                            Message = ResponseMessage.NoRecordFound
                        }
                    });
                }
            });

        }

        /// <summary>
        /// Get User Modules by user Id
        /// </summary>
        /// <param name="userId">The userId</param>
        /// <param name="accountName">The accountName</param>
        /// <returns></returns>
        [Route("getusermodulesbyuserid")]
        [HttpGet]
        [HasPermission(PermissionType.View)]
        public async Task<IActionResult> GetUserModulesByUserId(long userId, string accountName = "account")
        {
            //TODO - Need to remove account name parameter.

            ResponseResult responseResult = new ResponseResult();
            return await Execute(async () =>
            {
                var userModules = await usersService.GetUserModulesByUserId(userId);
                responseResult.Data = userModules;
                responseResult.Message = ResponseMessage.RecordFetched;
                return Ok(responseResult);
            });

        }


        /// <summary>
        /// Get User Group by user Id
        /// </summary>
        /// <param name="userId">The userId</param>
        /// <param name="accountName">The accountName</param>
        /// <returns></returns>
        [Route("getusergroupsbyuserid")]
        [HttpGet]
        [HasPermission(PermissionType.View)]
        public async Task<IActionResult> GetUserGroupsByUserId(long userId, string accountName = "account")
        {
            //TODO - Need to remove account name parameter.

            ResponseResult responseResult = new ResponseResult();
            return await Execute(async () =>
            {
                var userGroups = await usersService.GetUserGroupsByUserId(userId);
                responseResult.Data = userGroups;
                responseResult.Message = ResponseMessage.RecordFetched;
                return Ok(responseResult);
            });
        }

        /// <summary>
        /// Get User Permission by user Id
        /// </summary>
        /// <param name="userId">The userId</param>
        /// <param name="accountid">The accountid</param>
        /// <returns></returns>
        [Route("getuserpermissionsbyuserid")]
        [HttpGet]
        [HasPermission(PermissionType.View)]
        public async Task<IActionResult> GetUserPermissionsByUserId(long userId, string accountid = "0")
        {
            //TODO - Need to remove account name parameter.

            ResponseResult responseResult = new ResponseResult();
            return await Execute(async () =>
            {
                var userGroups = await usersService.GetUserPermissionsByUserId(userId);
                responseResult.Data = userGroups;
                responseResult.Message = ResponseMessage.RecordFetched;
                return Ok(responseResult);
            });
        } 

        /// <summary>
        /// Build User Navigation by user Id
        /// </summary>
        /// <param name="userId">The userId</param>
        /// <returns></returns>
        [Route("{userId}/buildUserNavigationByUserId")]
        [ProducesResponseType(typeof(ResponseResult<UserNavigation>), 200)]
        [HttpGet]
        [HasPermission(PermissionType.View)]
        public async Task<IActionResult> BuildUserNavigationByUserId([FromRoute] long userId)
        {
            return await Execute(async () =>
            {
                var userNavigations = await usersService.BuildUserNavigationByUserId(userId, headerAccountId);
                //if (userNavigations.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(userNavigations);
                //else
                //    return BadRequest(userNavigations);
            });
        }

        /// <summary>
        /// Get Mfa Types By UserId
        /// </summary>
        /// <param name="userId">The userId</param>
        /// <param name="accountName">The accountName</param>
        /// <returns></returns>
        [Route("getmfatypesbyuserid")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetMfaTypesByUserId(long userId, string accountName = "account")
        {
            //TODO - Need to remove account name parameter.

            ResponseResult responseResult = new ResponseResult();
            return await Execute(async () =>
            {
                var mfaTypeId = await usersService.GetMfaTypesByUserId(userId);
                responseResult.Data = mfaTypeId;
                responseResult.Message = ResponseMessage.RecordFetched;
                return Ok(responseResult);
            });
        }


        /// <summary>
        /// Change password of User
        /// </summary>
        /// <param name="model"></param>
        /// <returns>return response result</returns>
        [Route("changePassword")]
        [HttpPut]
        [Authorize]
        [ProducesResponseType(typeof(ResponseResult<SuccessMessageModel>), 200)]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest model)
        {
            // Set account Id;
            long accountId = 0;
            if (HttpContext.Request.Headers.ContainsKey("accountId"))
            {
                Int64.TryParse(HttpContext.Request.Headers["accountId"], out accountId);
            };

            return await Execute(async () =>
            {
                var result = await usersService.ChangePassword(model, accountId, loggedInUserId);
                if (result.ResponseCode == ResponseCode.RecordSaved)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }

        /// <summary>
        ///  Information for all existing users
        /// </summary>
        /// <param name="ordering">Ordering Fields: (All Fields)</param>       
        /// <param name="offset">The initial index from which to return the results.</param>
        /// <param name="pageSize">Number of results to return per page.</param>
        /// <param name="pageNumber">Page Number of results.</param>
        /// <param name="all">It will return results (all pages) without pagination.</param>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResultList<UserVM>), 200)]
        [HasPermission(PermissionType.View)]
        public async Task<IActionResult> GetAllUsers([FromQuery]string ordering, [FromQuery] int offset, [FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] bool all = false)
        {
            return await Execute(async () =>
            {
                var users = await usersService.GetAllUsers(HttpContext,ordering, offset, pageSize, pageNumber, all);
                if (users.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(users);
                else
                    return BadRequest(users);
            });
        }

        /// <summary>
        /// This is used to create user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        [HasPermission(PermissionType.Add)]
        [ProducesResponseType(typeof(ResponseResult<UserVM>), 201)]
        public async Task<IActionResult> CreateUser(CreateUserModel model)
        {
       
            return await Execute(async () =>
            {
                if (model != null)
                {
                    var result = await this.usersService.CreateUser(model);
                    if (result != null)
                    {
                        if (result.ResponseCode == ResponseCode.RecordSaved)
                        {
                            return Created("api/users/", result);
                        }
                        else
                        {
                            return BadRequest(result);
                        }
                    }
                    else
                    {
                        return BadRequest(new ResponseResult<UserVM>
                        {
                            ResponseCode = ResponseCode.InternalServerError,
                            Message = ResponseMessage.InternalServerError,
                            Error = new ErrorResponseResult
                            {
                                Message = ResponseMessage.InternalServerError
                            }
                        });
                    }
                }

                return BadRequest(new ResponseResult<UserVM>
                {
                    ResponseCode = ResponseCode.ValidationFailed,
                    Message = ResponseMessage.NoRecordFound,
                    Error = new ErrorResponseResult
                    {
                        Message = ResponseMessage.ValidationFailed
                    }
                });
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
        [ProducesResponseType(typeof(ResponseResult<UserVM>), 200)]
        [HasPermission(PermissionType.Edit)]
        public async Task<IActionResult> UpdateUser([FromRoute] long userId, [FromBody] UsersModel model)
        {
            return await Execute(async () =>
            {
                if (userId != 0)
                {
                    var result = await this.usersService.UpdateUser(userId, model);
                    if(result != null)
                    {
                        if (result.ResponseCode == ResponseCode.RecordSaved)
                        {
                            return Ok(result);
                        }
                        else
                        {
                            return BadRequest(result);
                        }
                    }
                    else
                    {
                        return BadRequest(new ResponseResult<UserVM>
                        {
                            ResponseCode = ResponseCode.InternalServerError,
                            Message = ResponseMessage.InternalServerError,
                            Error = new ErrorResponseResult
                            {
                                Message = ResponseMessage.InternalServerError
                            }
                        });
                    }
                }

                return BadRequest(new ResponseResult<UserVM>
                {
                    ResponseCode = ResponseCode.ValidationFailed,
                    Message = ResponseMessage.NoRecordFound,
                    Error = new ErrorResponseResult
                    {
                        Message = ResponseMessage.ValidationFailed
                    }
                });
            });
        }

        /// <summary>
        ///  Information for user by id
        /// </summary>
        /// <param name="userId">A unique integer value identifying this User.</param>
        /// <returns></returns>
        [Route("{userId}")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<UserVM>), 200)]
        [HasPermission(PermissionType.View)]
        public async Task<IActionResult> GetUserById([FromRoute] long userId)
        {
            return await Execute(async () =>
            {
                var result = await this.usersService.GetUserById(userId);
                if (result != null)
                {
                    if (result.ResponseCode == ResponseCode.RecordFetched)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return BadRequest(result);
                    }
                }
                else
                {
                    return BadRequest(new ResponseResult<UserVM>
                    {
                        ResponseCode = ResponseCode.InternalServerError,
                        Message = ResponseMessage.InternalServerError,
                        Error = new ErrorResponseResult
                        {
                            Message = ResponseMessage.InternalServerError
                        }
                    });
                }
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
        [ProducesResponseType(typeof(ResponseResult<UserVM>), 200)]
        [HasPermission(PermissionType.Edit)]
        public async Task<IActionResult> UpdatePartialUser([FromRoute] long userId, [FromBody] UsersModel model)
        {
            return await Execute(async () =>
            {
                if (userId != 0 && model != null)
                {
                    var result = await this.usersService.UpdatePartialUser(userId, model);

                    if (result != null)
                    {
                        if (result.ResponseCode == ResponseCode.RecordSaved)
                        {
                            return Ok(result);
                        }
                        else
                        {
                            return BadRequest(result);
                        }
                    }
                    else
                    {
                        return BadRequest(new ResponseResult<UserVM>
                        {
                            ResponseCode = ResponseCode.InternalServerError,
                            Message = ResponseMessage.InternalServerError,
                            Error = new ErrorResponseResult
                            {
                                Message = ResponseMessage.InternalServerError
                            }
                        });
                    }
                }

                return BadRequest(new ResponseResult<UserVM>
                {
                    ResponseCode = ResponseCode.ValidationFailed,
                    Message = ResponseMessage.NoRecordFound,
                    Error = new ErrorResponseResult
                    {
                        Message = ResponseMessage.NoRecordFound
                    }
                });
            });
        }

        /// <summary>
        /// This api is used for deleing  User
        /// </summary>
        /// <param name="userId">A unique integer value identifying this User.</param>
        /// <returns></returns>
        [Route("{userId}")]
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseResult), 200)]
        [HasPermission(PermissionType.Delete)]
        public async Task<IActionResult> DeleteUser([FromRoute] long userId)
        {
            return await Execute(async () =>
            {
                var result = await this.usersService.DeleteUser(userId);
                if (result != null)
                {
                    if (result.ResponseCode == ResponseCode.RecordDeleted)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return BadRequest(result);
                    }
                }

                return BadRequest(new ResponseResult
                {
                    ResponseCode = ResponseCode.NoRecordFound,
                    Message = ResponseMessage.NoRecordFound,
                    Error = new ErrorResponseResult
                    {
                        Message = ResponseMessage.NoRecordFound
                    }
                });
            });
        }


        /// <summary>
        /// Refresh Token
        /// </summary>
        /// <param name="tokenRefresh">Token Refresh</param>
        /// <returns></returns>
        [Route("refresh")]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResult<RefreshTokenResultModel>), 200)]
        public async Task<IActionResult> RefreshToken(TokenRefreshModel tokenRefresh)
        {
            return await Execute(async () =>
            {
                var result = await usersService.RefreshToken(tokenRefresh);

                if (result != null)
                {
                    if (result.ResponseCode == ResponseCode.RecordFetched)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return BadRequest(result);
                    }
                }

                return BadRequest(new ResponseResult
                {
                    ResponseCode = ResponseCode.InternalServerError,
                    Message = ResponseMessage.InternalServerError,
                    Error = new ErrorResponseResult
                    {
                        Message = ResponseMessage.InternalServerError
                    }
                });
            });
        }


        /// <summary>
        /// Logout
        /// </summary>
        /// <param name="tokenRefresh">Token Refresh Model</param>
        /// <returns></returns>
        [Route("logout")]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResult<SuccessMessageModel>), 200)]
        [Authorize]
        public async Task<IActionResult> Logout(TokenRefreshModel tokenRefresh)
        {
            var token = string.Empty;
            if (HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                token = HttpContext.Request.Headers["Authorization"];
            }

            return await Execute(async () =>
            {
                var result = await this.usersService.Logout(tokenRefresh, token);

                if (result != null)
                {
                    if (result.ResponseCode == ResponseCode.RecordSaved)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return BadRequest(result);
                    }
                }

                return BadRequest(new ResponseResult
                {
                    ResponseCode = ResponseCode.InternalServerError,
                    Message = ResponseMessage.InternalServerError,
                    Error = new ErrorResponseResult
                    {
                        Message = ResponseMessage.InternalServerError
                    }
                });
            });
        }


        /// <summary>
        /// Forgot Password
        /// </summary>
        /// <param name="model">Forgot Password Request Model</param>
        /// <returns></returns>
        [Route("forgotPassword")]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResult<SuccessMessageModel>), 200)]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestModel model)
        {
            long accountId = 0;
            if (HttpContext.Request.Headers.ContainsKey("accountId"))
            {
                Int64.TryParse(HttpContext.Request.Headers["accountId"], out accountId);
            };
            
            return await Execute(async () =>
            {
                var result = await this.usersService.ForgotPassword(model, accountId);

                if (result != null)
                {
                    if (result.ResponseCode == ResponseCode.RecordFetched)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return BadRequest(result);
                    }
                }

                return BadRequest(new ResponseResult
                {
                    ResponseCode = ResponseCode.InternalServerError,
                    Message = ResponseMessage.InternalServerError,
                    Error = new ErrorResponseResult
                    {
                        Message = ResponseMessage.InternalServerError
                    }
                });
            });
        }

        /// <summary>
        /// Set Password
        /// </summary>
        /// <param name="token"></param>
        /// <param name="uid"></param>
        /// <param name="model">Set Password Request Model</param>
        /// <returns></returns>
        [Route("setPassword")]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResult<SuccessMessageModel>), 200)]
        public async Task<IActionResult> SetPassword([FromQuery]string token, [FromQuery]string uid, [FromBody]SetForgotPasswordModel model)
        {
            return await Execute(async () =>
            {
                var result = await this.usersService.SetPassword(token, uid, model);

                if (result != null)
                {
                    if (result.ResponseCode == ResponseCode.RecordSaved)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return BadRequest(result);
                    }
                }

                return BadRequest(new ResponseResult
                {
                    ResponseCode = ResponseCode.InternalServerError,
                    Message = ResponseMessage.InternalServerError,
                    Error = new ErrorResponseResult
                    {
                        Message = ResponseMessage.InternalServerError
                    }
                });
            });
        }


        /// <summary>
        /// This api is used for verifying Admin User Token
        /// </summary>
        /// <param name="tokenModel">Verify Token Model</param>
        /// <returns></returns>
        [Route("Verify")]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResult), 200)]
        public async Task<IActionResult> VerifyToken([FromBody] VerifyTokenModel tokenModel)
        {

            return await Execute(async () =>
            {
                var result = await this.usersService.VerifyToken(tokenModel);

                if (result.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(result);
                else
                    return Unauthorized(result);
            });

        }

        /// <summary>
        /// This api is used for Verify User Email Address for existing User
        /// </summary>
        /// <param name="userId">A unique integer value identifying this User.</param>
        /// <returns></returns>
        [Route("VerifyUserEmailAddress/{userId}")]
        [HttpPut]
        public async Task<IActionResult> VerifyUserEmailAddress([FromRoute] long userId)
        {
            return await Execute(async () =>
            {
                var result = await this.usersService.VerifyUserEmailAddress(userId);
                return Ok(result);
            });

        }


        /// <summary>
        /// This api is used for Verify User Mobile Number for existing User
        /// </summary>
        /// <param name="userId">A unique integer value identifying this User.</param>
        /// <returns></returns>
        [Route("VerifyUserMobileNumber/{userId}")]
        [HttpPut]
        public async Task<IActionResult> VerifyUserMobileNumber([FromRoute] long userId)
        {
            return await Execute(async () =>
            {
                var result = await this.usersService.VerifyUserMobileNumber(userId);
                return Ok(result);
            });

        }

        /// <summary>
        ///  Send Mobile OTP
        /// </summary>
        [HttpPost]
        [Route("SendMobileOTP")]
        public async Task<IActionResult> SendMobileOTP([FromBody] SendMobileOtpVM model)
        {
            return await Execute(async () =>
            {
                var result = await this.usersService.SendMobileOTP(model);
                if (result.ResponseCode == ResponseCode.RecordSaved)
                    return Ok(result);
                else
                    return BadRequest(result);
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
                var result = await this.usersService.VerifyOTP(model);
                if (result.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }

        /// <summary>
        /// Get User Profile
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getuserprofile")]
        [Authorize]
        [ProducesResponseType(typeof(ResponseResult<UserProfileModel>), 200)]
        public async Task<IActionResult> GetUserProfile()
        {
            return await Execute(async () =>
            {
                var result = await this.usersService.GetUserProfile();
                if (result.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }

        /// <summary>
        /// Update User Profile
        /// </summary>
        /// <param name="model">User Profile Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("updateuserprofile")]
        [Authorize]
        [ProducesResponseType(typeof(ResponseResult<bool>), 200)]
        public async Task<IActionResult> UpdateUserProfile([FromBody] SaveUserProfileModel model)
        {
            return await Execute(async () =>
            {
                var result = await this.usersService.UpdateUserProfile(model);
                if (result.ResponseCode == ResponseCode.RecordSaved)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }

        /// <summary>
        /// Update User Profile Image
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("updateuserprofileimage")]
        [Authorize]
        [ProducesResponseType(typeof(ResponseResult<bool>), 200)]
        public async Task<IActionResult> UpdateUserProfileImage([FromBody] UserProfileImageVM model)
        {
            return await Execute(async () =>
            {
                var result = await this.usersService.UpdateUserProfileImage(model);
                if (result.ResponseCode == ResponseCode.RecordSaved)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }


        /// <summary>
        /// send verification email link
        /// </summary>
        /// <returns></returns>
        [Route("sendVerificationCode")]
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ResponseResult<SuccessMessageModel>), 200)]
        public async Task<IActionResult> SendVerificationCode()
        {
            return await Execute(async () =>
            {
                var result = await this.usersService.SendVerificationCode();

                if (result.ResponseCode == ResponseCode.RecordFetched)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            });
        }

        /// <summary>
        /// Change password of User
        /// </summary>
        /// <param name="model"></param>
        /// <returns>return response result</returns>
        [Route("changepasswordbymobileno")]
        [HttpPut]
        [ProducesResponseType(typeof(ResponseResult<string>), 200)]
        public async Task<IActionResult> ChangePasswordByMobileNo(ChangePasswordRequestForSMS model)
        {
            // Set account Id;
            long accountId = 0;
            if (HttpContext.Request.Headers.ContainsKey("accountId"))
            {
                Int64.TryParse(HttpContext.Request.Headers["accountId"], out accountId);
            };

            return await Execute(async () =>
            {
                var result = await usersService.ChangePasswordByMobileNo(model, accountId, loggedInUserId);
                if (result.ResponseCode == ResponseCode.RecordSaved)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }

        /// <summary>
        ///  Forget Password By SMS
        /// </summary>
        [HttpPost]
        [Route("ForgetPasswordBySMS")]
        [ProducesResponseType(typeof(ResponseResult<bool>), 200)]
        public async Task<IActionResult> ForgetPasswordBySMS([FromBody] SendMobileOtpVM model)
        {
            return await Execute(async () =>
            {
                var result = await this.usersService.ForgetPasswordBySMS(model);
                if (result.ResponseCode == ResponseCode.RecordSaved)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }
    }
}
