using Common.Model;
using Logger;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.HttpAggregator.IServices.ActivityCode;

namespace Sample.Customer.HttpAggregator.Controllers.ActivityCode
{
    /// <summary>
    /// ActivityCodeController
    /// </summary>
    [Route("api/ActivityCode")]
    [ApiController]
    public class ActivityCodeController : BaseApiController
    {
        private readonly IActivityCodeService _activityCodeService;

        /// <summary>
        /// Users Controller Constructor to inject services
        /// </summary>
        /// <param name="activityCodeService">The user service</param>
        /// <param name="logger">The file logger</param>
        public ActivityCodeController(IActivityCodeService activityCodeService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(activityCodeService), activityCodeService);
            this._activityCodeService = activityCodeService;
        }
        /// <summary>
        /// GetActivityCodeById
        /// </summary>
        /// <param name="ActivityCodeid"></param>
        /// <returns></returns>
        [Route("GetActivityCodeById/{ActivityCodeid}")]
        [HttpGet]
        public async Task<IActionResult> GetActivityCodeById(long ActivityCodeid)
        {
            return await Execute(async () =>
            {
                var userGroups = await this._activityCodeService.GetActivityCodeById(ActivityCodeid);
                return Ok(userGroups);
            });

        }

        /// <summary>
        /// Get All Activity
        /// </summary>
        /// <returns></returns>
        [Route("GetAllActivity")]
        [HttpGet]
        public async Task<IActionResult> GetAllActivity(long parkingProviderLocationId)
        {
            return await Execute(async () =>
            {
                var activityCode = await this._activityCodeService.GetAllActivity(parkingProviderLocationId);
                if(activityCode.ResponseCode==ResponseCode.InternalServerError)
                    return BadRequest(activityCode);
                return Ok(activityCode);
            });

        }
    }
}
