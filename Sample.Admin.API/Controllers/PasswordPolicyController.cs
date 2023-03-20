using Logger;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities;
using Sample.Admin.Service.ServiceWorker;
using Sample.Admin.Model;

namespace Sample.Admin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordPolicyController : BaseApiController
    {
        private readonly IPasswordPolicyService passwordPolicyService;

        /// <summary>
        /// Currency Type Controller constructor to Inject dependency
        /// </summary>
        /// <param name="currencyService">Service for currency</param>
        public PasswordPolicyController(IPasswordPolicyService passwordPolicyService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(passwordPolicyService), passwordPolicyService);
            this.passwordPolicyService = passwordPolicyService;
        }

        
        [Route("/CreateOrUpdatePasswordPolicy")]
        [HttpPost]
        public async Task<IActionResult> CreateOrUpdatePasswordPolicy([FromQuery]PasswordPolicyVM passwordPolicy)
        {
            return await Execute(async () =>
            {
                var result = await this.passwordPolicyService.CreateOrUpdatePasswordPolicy(passwordPolicy);
                return Ok(result);
            });

        }


        /// <summary>
        /// Information for currencies 
        /// </summary>
        /// <param name="currencyId">Currency Id</param>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await Execute(async () =>
            {
                var result = await this.passwordPolicyService.GetPasswordPolicy();
                return Ok(result);
            });
        }
        
    }
}
