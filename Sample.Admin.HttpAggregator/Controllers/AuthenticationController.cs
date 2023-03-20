using Sample.Admin.HttpAggregator.IServices;
using Common.Model;
using Sample.Admin.Model;
using Logger;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities;

namespace Sample.Admin.HttpAggregator.Controllers
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
        /// Authenticate Admin User
        /// </summary>
        /// <param name="login">login object which have all property to authenticate user</param>
        /// <returns></returns>
        [Route("authenticate")]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResult<LoginAdminUserModel>), 200)]
        public async Task<IActionResult> Authenticate([FromBody] LoginModel login)
        {
            //TODO - Need to remove account name parameter.
            Check.Argument.IsNotNull(nameof(login), login);
            
            return await Execute(async () =>
            {
                var result = await authenticationService.Authenticate(login);
                if (result.ResponseCode == ResponseCode.ValidLogin)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }
    }
}
