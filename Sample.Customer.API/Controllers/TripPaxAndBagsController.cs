using Logger;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.Model.Model;
using Sample.Customer.Service.ServiceWorker;

namespace Sample.Customer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripPaxAndBagsController : BaseApiController
    {
        private readonly ITripPaxAndBagsService tripPaxAndBagsService;

        public TripPaxAndBagsController(ITripPaxAndBagsService tripPaxAndBagsService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(tripPaxAndBagsService), tripPaxAndBagsService);
            this.tripPaxAndBagsService = tripPaxAndBagsService;
        }

        [Route("SaveTripPaxAndBags")]
        [HttpPost]
        public async Task<IActionResult> SaveTripPaxAndBags(TripPaxAndBagsVM tripPaxAndBagsVM)
        {
            return await Execute(async () =>
            {
                var userGroups = await this.tripPaxAndBagsService.SaveTripPaxAndBags(tripPaxAndBagsVM, loggedInUserId, loggedInAccountId);
                return Ok(userGroups);
            });

        }
    }
}
