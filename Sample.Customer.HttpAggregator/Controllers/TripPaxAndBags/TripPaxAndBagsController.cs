using Common.Model;
using Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.HttpAggregator.IServices.TripPaxAndBags;
using Sample.Customer.Model.Model;

namespace Sample.Customer.HttpAggregator.Controllers.TripPaxAndBags
{
    /// <summary>
    /// TripPaxAndBagsController
    /// </summary>
    [Route("api/TripPaxAndBags")]
    [ApiController]
    public class TripPaxAndBagsController : BaseApiController
    {
        private readonly ITripPaxAndBagsService tripPaxAndBagsService;

        /// <summary>
        /// Users Controller Constructor to inject services
        /// </summary>
        /// <param name="tripPaxAndBagsService">The user service</param>
        /// <param name="logger">The file logger</param>
        public TripPaxAndBagsController(ITripPaxAndBagsService tripPaxAndBagsService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(tripPaxAndBagsService), tripPaxAndBagsService);
            this.tripPaxAndBagsService = tripPaxAndBagsService;
        }

        /// <summary>
        /// SaveTripPaxAndBags
        /// </summary>
        /// <param name="tripPaxAndBagsVM"></param>
        /// <returns></returns>
        [Route("SaveTripPaxAndBags")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SaveTripPaxAndBags(TripPaxAndBagsVM tripPaxAndBagsVM)
        {
            return await Execute(async () =>
            {
                var userGroups = await this.tripPaxAndBagsService.SaveTripPaxAndBags(tripPaxAndBagsVM);
                return Ok(userGroups);
            });

        }
    }
}
