using Sample.Admin.Service.ServiceWorker;
using Logger;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities;

namespace Sample.Admin.API.Controllers
{
    [Route("api/authenticationConfigKeys")]
    [ApiController]
    public class AuthenticationConfigKeysController : BaseApiController
    {
        private readonly IAuthenticationConfigKeyService service;

        /// <summary>
        /// Authentication Config Key Controller constructor to Inject dependency
        /// </summary>
        /// <param name="accountService">Account Service </param>
        /// <param name="logger">Logger for file</param>
        public AuthenticationConfigKeysController(IAuthenticationConfigKeyService service, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(service), service);
            this.service = service;
        }

        // <summary>
        /// Get All Authentication Config Key
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetAuthenticationConfigKeys(string authenticationType)
        {
            return await Execute(async () =>
            {
                var result = await this.service.GetAuthenticationConfigKeys(authenticationType);
                return Ok(result);
            });
        }
    }
}
