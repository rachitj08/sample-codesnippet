using Common.Model;
using Logger;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.Model;
using Sample.Customer.Service.ServiceWorker;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sample.Customer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : BaseApiController
    {
        #region [Private Variables]
        /// <summary>
        /// authentication Service private variable
        /// 
        /// </summary>
        private readonly IAuthenticationService authenticationService;

        #endregion

        #region [Constructor]
        /// <summary>
        /// /
        /// </summary>
        /// <param name="authenticationService"></param>
        /// <param name="logger"></param>
        public AuthenticationController(IAuthenticationService authenticationService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(authenticationService), authenticationService);
            this.authenticationService = authenticationService;
        }
        #endregion

        // GET api/<LoginController>
        /// <summary>
        ///  To authenticate user
        /// </summary>
        /// <param name="login">login class to authenticate user</param>
        /// <returns></returns>
        [Route("authenticate")]
        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] Login login)
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
                          Message = ResponseMessage.InternalServerError,
                        }
                    });
                }
                return Ok(userAuthenticationResponse);
            });
        }

        // GET api/<LoginController>
        /// <summary>
        ///  To authenticate external user
        /// </summary>
        /// <param name="login">login class to authenticate external user</param>
        /// <returns></returns>
        [Route("authenticateExternalUser")]
        [HttpPost]
        public async Task<IActionResult> AuthenticateExternalUser([FromBody] ExternalUserVM login)
        {
            return await Execute(async () =>
            {
                var userAuthenticationResponse = await this.authenticationService.AuthenticateExternalUser(login, HttpContext.Request.Host.ToUriComponent(), HttpContext.Request.Headers.ToString());
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
                var result = await this.authenticationService.VerifyToken(tokenModel, loggedInAccountId);
                return Ok(result);
            });
        }

    }
}
