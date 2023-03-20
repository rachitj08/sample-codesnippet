using Common.Model;
using Logger;
using Microsoft.AspNetCore.Mvc;
using Sample.Customer.HttpAggregator.IServices.UserManagement;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities;

namespace Sample.Customer.HttpAggregator.Controllers.UserManagement
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/authenticationConfigKeysValues")]
    [ApiController]
    public class AuthenticationConfigKeysValuesController : BaseApiController
    {
        #region [Private Variables]
        /// <summary>
        /// Password Policy Service private variable
        /// </summary>
        private readonly IAuthenticationConfigKeysValuesService service;
        #endregion

        #region [Constructor]
        /// <summary>
        /// /
        /// </summary>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public AuthenticationConfigKeysValuesController(IAuthenticationConfigKeysValuesService service, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(service), service);
            this.service = service;
        }
        #endregion

        /// <summary>
        /// Get Tenant Auth Settings based on authenticationType
        /// </summary>
        /// <param name="authenticationType">Authentication Type (A-Azure|O-Okta)</param>
        /// <returns></returns>
        [HttpGet("getTenantAuthSettings/{authenticationType}")]
        [ProducesResponseType(typeof(ResponseResult<Dictionary<string, string>>), 200)]
        public async Task<IActionResult> GetTenantAuthSettings([FromRoute] string authenticationType)
        {
            return await Execute(async () =>
            {
                var result = await this.service.GetTenantAuthSettings(authenticationType);
                if (result.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }
    }
}
