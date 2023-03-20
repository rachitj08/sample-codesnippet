using Sample.Admin.API.Controllers;
using Sample.Admin.Service.ServiceWorker;
using Logger;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities;

namespace Sample.Admin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationTypeController : BaseApiController
    {
        private readonly IAuthenticationTypeService authenticationTypeService;


        /// <summary>
        /// Authentication Type Controller constructor to Inject dependency
        /// </summary>
        /// <param name="authenticationTypeService"> authentication service for user</param>
        /// <param name="logger">logger to log into file</param>
        public AuthenticationTypeController(IAuthenticationTypeService authenticationTypeService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(authenticationTypeService), authenticationTypeService);
            this.authenticationTypeService = authenticationTypeService;
        }

        /// <summary>
        ///  Information for All Authentication Types
        /// </summary>
        /// <returns></returns>
        [Route("getallauthenticationtypes")]
        [HttpGet]
        public async Task<IActionResult> GetAllAuthenticationTypes()
        {

            return await Execute(async () =>
            {
                var result = await this.authenticationTypeService.GetAllAuthenticationTypes();
                return Ok(result);
            });


        }
    }
}
