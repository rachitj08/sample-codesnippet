using System.Threading.Tasks;
using Common.Model;
using Sample.Admin.Service.ServiceWorker;
using Sample.Admin.Model;
using Logger;
using Microsoft.AspNetCore.Mvc;
using Utilities;

namespace Sample.Admin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : BaseApiController
    {
        private readonly IAuthenticationService authenticationService;

        /// <summary>
        /// Authentication Type Controller constructor to Inject dependency
        /// </summary>
        /// <param name="authenticationService"> authentication service for user</param>
        /// <param name="logger">logger to log into file</param>
        public AuthenticationController(IAuthenticationService authenticationService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(authenticationService), authenticationService);
            this.authenticationService = authenticationService;
        }
         
        /// <summary>
        ///  To authenticate user
        /// </summary>
        /// <param name="login">login class to authenticate user</param>
        /// <returns></returns>
        [Route("authenticate")]
        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] LoginModel login)
        {
            return await Execute(async () =>
            {
                var userAuthenticationResponse = await this.authenticationService.Authenticate(login, HttpContext.Request.Host.ToUriComponent(), HttpContext.Request.Headers.ToString());
                if (userAuthenticationResponse == null)
                {
                    return BadRequest(new ResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError,
                        ResponseCode = ResponseCode.InternalServerError,
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.InternalServerError
                        }
                    });
                }
                return Ok(userAuthenticationResponse);
            });
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <param name="model">Token Request Model</param>
        /// <returns></returns>
        [Route("logout")]
        [HttpPost]
        public async Task<IActionResult> Logout([FromBody] TokenRequestModel model)
        {
            string token = string.Empty;


            return await Execute(async () =>
            {
                var userAuthenticationResponse = await this.authenticationService.Logout(model);
                if (userAuthenticationResponse == null)
                {
                    return BadRequest(new ResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError,
                        ResponseCode = ResponseCode.InternalServerError,
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.InternalServerError,
                        }
                    });
                }
                return Ok(userAuthenticationResponse);
            });
        }

        /// <summary>
        /// Refresh Token
        /// </summary>
        /// <param name="model">Refresh Token model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRefreshModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await this.authenticationService.RefreshToken(model.Refresh);

                if (result == null)
                {
                    return BadRequest(new ResponseResult()
                    {
                        Message = "Invalid tokens",
                        ResponseCode = ResponseCode.InternalServerError,
                        Error = new ErrorResponseResult()
                        {
                            Message = "Invalid tokens",
                        }
                    });
                }

                return Ok(result);
            }
            else
            {
                return BadRequest(new ResponseResult()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed,
                    }
                });
            }
        }
    }
}
