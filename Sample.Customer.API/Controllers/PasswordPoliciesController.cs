using Common.Model;
using Logger;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Sample.Customer.API.Controllers;
using Sample.Customer.Service.ServiceWorker;
using Sample.Customer.Model;
using Utilities;

namespace Sample.Customer.API.Controllers
{
    /// <summary>
    /// Get Password Policies
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordPoliciesController : BaseApiController
    {
        #region [Private Variables]
        /// <summary>
        /// Password Policy Service private variable
        /// 
        /// </summary>
        private readonly IPasswordPolicyService passwordPolicyService;
        #endregion

        #region [Constructor]
        /// <summary>
        /// /
        /// </summary>
        /// <param name="passwordPolicyService"></param>
        /// <param name="logger"></param>
        /// public UsersController(IUserService userService, IFileLogger logger) : base(logger: logger)
        public PasswordPoliciesController(IPasswordPolicyService passwordPolicyService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(passwordPolicyService), passwordPolicyService);
            this.passwordPolicyService = passwordPolicyService;
        }
        #endregion

        /// <summary>
        /// Get Password Policy By AccountId
        /// </summary>
        /// <param name="accountId">The accountId to get PasswordPolicy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get(long accountId)
        {
            return await Execute(async () =>
            {
                var result = await this.passwordPolicyService.GetPasswordPolicyByAccountId(accountId);
                return Ok(result);
            });
        }

        /// <summary>
        /// This api is used for Creating new Password Policy
        /// </summary>
        /// <param name="passwordPolicy">The new Password Policy object.</param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        public async Task<IActionResult> CreatePasswordPolicy([FromBody] PasswordPolicyModel passwordPolicy)
        {
            return await Execute(async () =>
            {
                var result = await this.passwordPolicyService.CreatePasswordPolicy(passwordPolicy, loggedInUserId);
                return Ok(result);
            });
        }

        /// <summary>
        /// This api is used for Updating existing Password Policy
        /// </summary>
        /// <param name="passwordPolicy">The existing password policy object.</param>       
        /// <returns></returns>
        [Route("")]
        [HttpPut]
        public async Task<IActionResult> UpdatePasswordPolicy([FromBody] PasswordPolicyModel passwordPolicy)
        {
            return await Execute(async () =>
            {
                var result = await this.passwordPolicyService.UpdatePasswordPolicy(loggedInAccountId, passwordPolicy, loggedInUserId);
                return Ok(result);
            });
        }

    }
}
