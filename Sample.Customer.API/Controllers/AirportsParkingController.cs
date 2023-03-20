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
    public class AirportsParkingController : BaseApiController
    {
        private readonly IAirportsParkingService _airportsParkingService;

        /// <summary>
        /// ReservationController Constructor
        /// </summary>
        /// <param name="reservationService"></param>
        /// <param name="logger"></param>
        public AirportsParkingController(IAirportsParkingService airportsParkingService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(airportsParkingService), airportsParkingService);
            this._airportsParkingService = airportsParkingService;
        }

        /// <summary>
        /// GetAllReservations 
        /// </summary>
        /// <param name="ordering"></param>
        /// <param name="offset"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        [Route("GetAllAirportsParkingByAirportId/{airportId}")]
        [HttpGet]
        public async Task<IActionResult> GetAllAirportsParkingByAirportId(long airportId)
        {
            return await Execute(async () =>
            {
                var userGroups = await this._airportsParkingService.GetAllAirportsParkingByAirportId(airportId);
                return Ok(userGroups);
            });

        }
    }
}
