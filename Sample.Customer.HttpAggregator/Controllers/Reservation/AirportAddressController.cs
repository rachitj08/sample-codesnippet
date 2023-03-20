using Common.Model;
using Logger;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.HttpAggregator.IServices.Reservation;

namespace Sample.Customer.HttpAggregator.Controllers.Reservation
{
    /// <summary>
    /// 
    /// </summary>
    public class AirportAddressController : BaseApiController
    {
        private readonly IAirportAddressService airportAddressService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="airportAddressService"></param>
        /// <param name="logger"></param>
        public AirportAddressController(IAirportAddressService airportAddressService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(airportAddressService), airportAddressService);
            this.airportAddressService = airportAddressService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        
        [Route("GetAllAirportAddress")]
        [HttpGet]
        public async Task<IActionResult> GetAllAirportAddres()
        {
            return await Execute(async () =>
            {
                var airportAddress = await (airportAddressService.GetAllAirportAddress());
                if (airportAddress.ResponseCode==ResponseCode.RecordFetched)
                    return Ok(airportAddress);
                else
                    return BadRequest(airportAddress);
            });
        }
    }
}
