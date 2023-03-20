using Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.Service.ServiceWorker;

namespace Sample.Customer.API.Controllers
{

    public class AirlineController : BaseApiController
    {
        private readonly IAirlineService airlineService;
        public AirlineController(IAirlineService airlineService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(airlineService), airlineService);
            this.airlineService = airlineService;
        }
        [Route("getairlinelist")]
        [HttpGet]
        public async Task<IActionResult> GetAirlineList()
        {
            return await Execute(async () =>
            {
                var result = this.airlineService.GetAllAirlineList(loggedInAccountId, loggedInUserId);
                return Ok(result);
            });

        }

    }
}
