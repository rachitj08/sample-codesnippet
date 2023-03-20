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
    public class ParkingHeadController : BaseApiController
    {
        private readonly IParkingHeadsService parkingHeadsService;

        /// <summary>
        /// ReservationController Constructor
        /// </summary>
        /// <param name="reservationService"></param>
        /// <param name="logger"></param>
        public ParkingHeadController(IParkingHeadsService _parkingHeadsService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(_parkingHeadsService), _parkingHeadsService);
            this.parkingHeadsService = _parkingHeadsService;
        }

        /// <summary>
        /// GetParkingTotalAmountAndLocation
        /// </summary>
        /// <returns></returns>
        [Route("GetParkingTotalAmountAndLocation")]
        [HttpGet]
        public async Task<IActionResult> GetParkingTotalAmountAndLocation()
        {
            return await Execute(async () =>
            {
                //var userGroups = this.parkingHeadsService.GetParkingTotalAmountAndLocation(5,3, "Indira gandhi airport");
                return Ok();
            });

        }
    }
}
