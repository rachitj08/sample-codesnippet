using Sample.Admin.HttpAggregator.IServices;
using Common.Model;
using Sample.Admin.Model;
using Sample.Admin.Model.User;
using Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities;

namespace Sample.Admin.HttpAggregator.Controllers
{
    /// <summary>
    /// Admin Users Controller
    /// </summary>
    [Route("api/adminUsers")]
    [ApiController]
    public class AdminUsersController : BaseApiController
    {
        private readonly IAdminUsersService adminUsersService;
        /// <summary>
        /// Admin Users Controller Constructor to inject services
        /// </summary>
        /// <param name="adminUsersService">The Admin Users Service</param>
        /// <param name="logger">The file logger</param>
        public AdminUsersController(IAdminUsersService adminUsersService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(adminUsersService), adminUsersService);

            this.adminUsersService = adminUsersService;
        }


        /// <summary>
        /// To Get List of Admin Users
        /// </summary>
        /// <param name="ordering">Ordering Fields: (All Fields)</param>
        /// <param name="search">Search Fields: (UserId, UserName, Email)</param>
        /// <param name="offset">The initial index from which to return the results.</param>
        /// <param name="pageSize">Number of results to return per page</param>
        /// <param name="pageNumber">Page Number of results.</param>
        /// <param name="all">It will return results (all pages) without pagination.</param>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResultList<AdminUsersModel>), 200)]
        [Authorize]
        public async Task<IActionResult> GetAllAllAdminUsers([FromQuery] string ordering, [FromQuery] string search, [FromQuery]int pageSize, [FromQuery] int pageNumber,[FromQuery] int offset, [FromQuery] bool all)
        {
            return await Execute(async () =>
            {
                var response = await adminUsersService.GetAllAdminUsers(HttpContext, ordering, search, pageSize, pageNumber,offset, all);
                if (response.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(response);
                else
                    return BadRequest(response);
            });
        }


        /// <summary>
        /// Get Admin User Detail
        /// </summary>
        /// <param name="userId">A unique integer value identifying Admin User.</param>
        /// <returns></returns>
        [Route("{userId}")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<AdminUsersModel>), 200)]
        [Authorize]
        public async Task<IActionResult> Get([FromRoute] int userId)
        {
            return await Execute(async () =>
            {
                var response = await adminUsersService.GetAdminUserDetail(userId);
                if (response.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(response);
                else
                    return BadRequest(response);
            });
        }

        /// <summary>
        /// This api is used for Creating new Admin User
        /// </summary>
        /// <param name="adminUser">The new module object.</param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResult<UserCreationModel>), 200)]
        [Authorize]
        public async Task<IActionResult> CreateAdminUser([FromBody] UserCreationModel adminUser)
        {
            return await Execute(async () =>
            {
                if (adminUser != null)
                {
                    var result = await this.adminUsersService.CreateAdminUser(adminUser);
                    if (result != null)
                    {
                        if(result.ResponseCode == ResponseCode.RecordSaved)
                        {
                            return Ok(result);
                        }
                        else
                        {
                            return BadRequest(result);
                        }
                    }
                }

                return BadRequest(new ResponseResult<UserCreationModel>
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
        /// This api is used for Updating existing Module
        /// </summary>
        /// <param name="adminUser">The existing admin user object.</param>
        /// <param name="userId">A unique integer value identifying this Admin Users.</param>
        /// <returns></returns>
        [Route("{userId}")]
        [HttpPut]
        [ProducesResponseType(typeof(ResponseResult<UserCreationModel>), 200)]
        [Authorize]
        public async Task<IActionResult> UpdateAdminUser([FromRoute] int userId, [FromBody] UserCreationModel adminUser)
        {
            return await Execute(async () =>
            {
                if (userId != 0 && adminUser != null)
                {
                    var result = await this.adminUsersService.UpdateAdminUser(userId, adminUser);
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
                }

                return BadRequest(new ResponseResult<UserCreationModel>
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
        /// This api is used for Updating existing Admin User partially
        /// </summary>
        /// <param name="adminUser">The existing admin user object.</param>
        /// <param name="userId">A unique integer value identifying this admin user.</param>
        /// <returns></returns>
        [Route("{userId}")]
        [HttpPatch]
        [ProducesResponseType(typeof(ResponseResult), 200)]
        [Authorize]
        public async Task<IActionResult> UpdatePartialAdminUser([FromRoute] int userId, [FromBody] UserCreationModel adminUser)
        {
            return await Execute(async () =>
            {
                if (userId != 0 && adminUser != null)
                {
                    var result = await this.adminUsersService.UpdatePartialAdminUser(userId, adminUser);
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
                }

                return BadRequest(new ResponseResult
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
        /// This api is used for deleing  Admin User
        /// </summary>
        /// <param name="userId">A unique integer value identifying this Admin User.</param>
        /// <returns></returns>
        [Route("{userId}")]
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseResult), 200)]
        [Authorize]
        public async Task<IActionResult> DeleteAdminUser([FromRoute] int userId)
        {

            return await Execute(async () =>
            {
                var result = await this.adminUsersService.DeleteAdminUser(userId);

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
        /// Change password of Admin User
        /// </summary>
        /// <param name="model"></param>
        /// <returns>return response result</returns>
        [Route("changePassword")]
        [HttpPut]
        [ProducesResponseType(typeof(ResponseResult<SuccessMessageModel>), 200)]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestModel model)
        {
            return await Execute(async () =>
            {
                var result = await adminUsersService.ChangePassword(model, loggedInUserId);

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
                var result = await adminUsersService.RefreshToken(tokenRefresh);

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
                var result = await this.adminUsersService.Logout(tokenRefresh, token);

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
        /// Forgot Password
        /// </summary>
        /// <param name="model">Forgot Password Request Model</param>
        /// <returns></returns>
        [Route("forgotPassword")]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResult<SuccessMessageModel>), 200)]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestModel model)
        {
            return await Execute(async () =>
            {
                var result = await this.adminUsersService.ForgotPassword(model);

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
        public async Task<IActionResult> SetPassword([FromQuery] string token, [FromQuery] string uid, [FromBody] SetForgotPasswordModel model)
        {
            return await Execute(async () =>
            {
                var result = await this.adminUsersService.SetPassword(token, uid, model);

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
                var result = await this.adminUsersService.VerifyToken(tokenModel);

                if (result.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(result);
                else
                    return Unauthorized(result);
            });

        }
    }
}