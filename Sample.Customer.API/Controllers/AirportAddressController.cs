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
    public class AirportAddressController : BaseApiController
    {
        private readonly IAirportAddressService airportAddressService;
        public AirportAddressController(IAirportAddressService airportAddressService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(airportAddressService), airportAddressService);
            this.airportAddressService = airportAddressService;
        }
        [Route("getairportaddress")]
        [HttpGet]
        public async Task<IActionResult> GetAllAirportAddress()
        {
            return await Execute(async () =>
            {
                var result = this.airportAddressService.GetAllAirportAddress(loggedInAccountId,loggedInUserId);
                return Ok(result);
            });

        }
    }
}
