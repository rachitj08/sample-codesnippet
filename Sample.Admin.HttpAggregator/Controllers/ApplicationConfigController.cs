using Sample.Admin.HttpAggregator.IServices;
using Common.Model;
using Sample.Admin.Model;
using Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities;

namespace Sample.Admin.HttpAggregator.Controllers
{
    /// <summary>
    /// Application Config Controller
    /// </summary>
    [Route("api/applicationConfig")]
    [ApiController]
    public class ApplicationConfigController : BaseApiController
    {
        private readonly IApplicationConfigService applicationConfigService;

        /// <summary>
        /// Application Config Controller constructor
        /// </summary>
        public ApplicationConfigController(IApplicationConfigService applicationConfigService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(applicationConfigService), applicationConfigService);
            this.applicationConfigService = applicationConfigService;
        }

        /// <summary>
        /// Get Application level configuration details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<ApplicationConfig>), 200)]
        public async Task<IActionResult> Get()
        {
            return await Execute(async () =>
            {
                var appConfig = await applicationConfigService.GetApplicationConfig();
                if (appConfig.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(appConfig);
                else
                    return BadRequest(appConfig);
            });
        }

        /// <summary>
        /// Get Database level configuration details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDatabaseDetails")]
        [Authorize]
        [ProducesResponseType(typeof(ResponseResult<DatabaseDetails>), 200)]
        public async Task<IActionResult> GetDatabaseDetails()
        {
            return await Execute(async () =>
            {
                var appConfig = await applicationConfigService.GetDatabaseDetails();
                if (appConfig.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(appConfig);
                else
                    return BadRequest(appConfig);
            });
        }
    }
}
