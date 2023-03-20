using Sample.Customer.Service.ServiceWorker;
using Logger;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities;

namespace Sample.Customer.API.Controllers
{
    [Route("api/authenticationConfigKeysValues")]
    [ApiController]
    public class AuthenticationConfigKeysValuesController : BaseApiController
    {
        private readonly IAuthenticationConfigKeysValuesService service;

        /// <summary>
        /// Authentication Config Keys Values Controller constructor to Inject dependency
        /// </summary>
        /// <param name="service">Authentication Config Keys Values service </param>
        /// <param name="logger">logger service </param>
        public AuthenticationConfigKeysValuesController(IAuthenticationConfigKeysValuesService service, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(service), service);
            this.service = service;
        }

        /// <summary>
        ///  Information for all Authentication Config Keys Values
        /// </summary>
        /// <param name="accountId">AccountId</param>
        /// <returns></returns>
        [Route("getAuthConfigValuesByAccountId")]
        [HttpGet]
        public async Task<IActionResult> GetAllAuthenticationConfigKeysValues(long accountId)
        {
            return await Execute(async () =>
            {
                var result = await this.service.GetAuthenticationConfigKeysValues(accountId);
                if (result != null)
                    return Ok(result);
                else
                    return BadRequest(result);
            });

        }
    }
}
