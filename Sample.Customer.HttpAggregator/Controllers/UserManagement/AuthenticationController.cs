using Common.Model;
using Logger;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.HttpAggregator.IServices.UserManagement;
using Sample.Customer.Model;


namespace Sample.Customer.HttpAggregator.Controllers.UserManagement
{
    /// <summary>
    /// Authentication Controller
    /// </summary>
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : BaseApiController
    {
        private readonly IAuthenticationService authenticationService;


        /// <summary>
        /// Authentication Controller Constructor to inject services
        /// </summary>
        /// <param name="authenticationService">The authentication Service</param>
        /// <param name="logger">The file logger</param>
        public AuthenticationController(IAuthenticationService authenticationService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(authenticationService), authenticationService);

            this.authenticationService = authenticationService;
        }

        /// <summary>
        /// Authenticate User
        /// </summary>
        /// <param name="loginModel">login object which have all property to authenticate user</param>
        /// <returns></returns>
        [Route("authenticate")]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResult<User>), 200)]
        public async Task<IActionResult> Authenticate([FromBody] LoginIAM loginModel)
        {           
            Check.Argument.IsNotNull(nameof(loginModel), loginModel);
           // var userAgent = Request.Headers..ToString();

            return await Execute(async () =>
            {
                var login = new Login()
                {
                    Password = loginModel.Password,
                    UserName = loginModel.UserName,
                    AccountId = headerAccountId,
                    ApplicationId= headerClientId
                };

                var result = authenticationService.Authenticate(login);
                if (result.ResponseCode == ResponseCode.ValidLogin)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }

        /// <summary>
        /// Authenticate User
        /// </summary>
        /// <param name="externalAuth">login object which have all property to authenticate user</param>
        /// <returns></returns>
        [Route("authenticateExternalUser")]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResult<User>), 200)]
        public async Task<IActionResult> AuthenticateExternalUser([FromBody] ExternalLoginVM externalAuth)
        {
            Check.Argument.IsNotNull(nameof(externalAuth), externalAuth);

            return await Execute(async () =>
            {
                var result = await authenticationService.AuthenticateExternalUser(externalAuth, headerAccountId);
                if (result.ResponseCode == ResponseCode.ValidLogin)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }
    }
}
