using Common.Model;
using Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.HttpAggregator.IServices.Airline;
using Sample.Customer.Model.Model;

namespace Sample.Customer.HttpAggregator.Controllers.Airline
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AirlineController : BaseApiController
    {
        private readonly IAirlineService airlineService;

        /// <summary>
        /// Users Controller Constructor to inject services
        /// </summary>
        /// <param name="airlineService">The user service</param>
        /// <param name="logger">The file logger</param>
        public AirlineController(IAirlineService airlineService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(airlineService), airlineService);
            this.airlineService = airlineService;
        }
        /// <summary>
        /// Get Airline List
        /// </summary>
        /// <returns></returns>
        [Route("getairlinelist")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<List<AirlineVM>>), 200)]
        public async Task<IActionResult> GetAirlineList()
        {
            return await Execute(async () =>
            {
                var result = await this.airlineService.GetAirlineList();
                if (result.ResponseCode == ResponseCode.InternalServerError)
                    return BadRequest(result);
                else
                    return Ok(result);
            });
        }
    }


}
