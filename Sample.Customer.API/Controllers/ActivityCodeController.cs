using Logger;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.Service.ServiceWorker;

namespace Sample.Customer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityCodeController : BaseApiController
    {
        private readonly IActivityCodeService ActivityCodeService;

        /// <summary>
        /// ActivityCodeController Constructor
        /// </summary>
        /// <param name="ActivityCodeService"></param>
        /// <param name="logger"></param>
        public ActivityCodeController(IActivityCodeService ActivityCodeService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(ActivityCodeService), ActivityCodeService);
            this.ActivityCodeService = ActivityCodeService;
        }
        /// <summary>
        /// GetActivityCodeById
        /// </summary>
        /// <param name="ActivityCodeid"></param>
        /// <returns></returns>
        [Route("GetActivityCodeById/{activityCodeid}")]
        [HttpGet]
        public async Task<IActionResult> GetActivityCodeById(long activityCodeid)
        {
            return await Execute(async () =>
            {
                var result = this.ActivityCodeService.GetActivityCodeById(activityCodeid);
                return Ok(result);
            });

        }
        /// <summary>
        /// GetAllActivity
        /// </summary>
        /// <param name="activityCodeid"></param>
        /// <returns></returns>
        [Route("GetAllActivity")]
        [HttpGet]
        public async Task<IActionResult> GetAllActivity(long parkingProviderLocationId)
        {
            return await Execute(async () =>
            {
                var result = this.ActivityCodeService.GetAllActivity(parkingProviderLocationId,loggedInAccountId);
                return Ok(result);
            });

        }
    }
}
