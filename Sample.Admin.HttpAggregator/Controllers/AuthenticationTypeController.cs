using System.Threading.Tasks;
using Common.Model;
using Logger;
using Microsoft.AspNetCore.Mvc;
using Sample.Admin.HttpAggregator.IServices;
using Utilities;
using Microsoft.AspNetCore.Authorization;

namespace Sample.Admin.HttpAggregator.Controllers
{
    /// <summary>
    /// Authentication Type
    /// </summary>
    [Route("api/authenticationType")]
    [ApiController]
    [Authorize]
    public class AuthenticationTypeController : BaseApiController
    {
        private readonly IAuthenticationTypeService authenticationTypeService;

        /// <summary>
        /// Authentication type Controller Constructor to inject services
        /// </summary>
        /// <param name="authenticationTypeService">The authentication Type Service</param>
        /// <param name="logger">The file logger</param>
        public AuthenticationTypeController(IAuthenticationTypeService authenticationTypeService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(authenticationTypeService), authenticationTypeService);

            this.authenticationTypeService = authenticationTypeService;
        }

        /// <summary>
        /// Get list of All Authentication Types
        /// </summary>
        /// <param name="accountName">The accountName</param>
        /// <returns></returns>
        [Route("getallauthenticationtypes")]
        [HttpGet]
        public async Task<IActionResult> GetAllAuthenticationTypes(string accountName = "account")
        {
            //TODO - Need to remove account name parameter.

            ResponseResult responseResult = new ResponseResult();
            return await Execute(async () =>
            {
                var result = await authenticationTypeService.GetAllAuthenticationTypes();
                responseResult.Data = result;
                responseResult.Message = ResponseMessage.RecordFetched;                
                return Ok(responseResult);
            });
        }
    }
}
