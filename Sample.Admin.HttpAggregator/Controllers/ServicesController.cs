using Sample.Admin.HttpAggregator.IServices;
using Common.Model;
using Sample.Admin.Model;
using Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities;

namespace Sample.Admin.HttpAggregator.Controllers
{
    /// <summary>
    /// Services Controller
    /// </summary>
    [Route("api/services")]
    [ApiController]
    [Authorize]
    public class ServicesController : BaseApiController
    {
        private readonly IServiceService serviceService;

        /// <summary>
        /// Services Controller constructor
        /// </summary>
        public ServicesController(IServiceService serviceService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(serviceService), serviceService);
            this.serviceService = serviceService;
        }

        /// <summary>
        /// Get Service List
        /// </summary>
        /// <param name="ordering">Ordering Fields: (All Fields)</param>
        /// <param name="search">Search Fields: (ServiceId, ServiceName)</param>
        /// <param name="offset">The initial index from which to return the results.</param>
        /// <param name="pageSize">Number of results to return per page</param>
        /// <param name="pageNumber">Page Number of results.</param>
        /// <param name="all">It will return results (all pages) without pagination.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<List<ServiceModel>>), 200)]
        public async Task<IActionResult> Get([FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] string ordering, [FromQuery] string search, [FromQuery] int offset, [FromQuery] bool all)
        {
            return await Execute(async () =>
            {
                var response = await serviceService.GetServiceList(HttpContext, pageSize, pageNumber, ordering, search, offset, all);
                if (response.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(response);
                else
                    return BadRequest(response);
            });
        }

        /// <summary>
        /// Get Service Detail
        /// </summary>
        /// <param name="serviceId">A unique integer value identifying service.</param>
        /// <returns></returns>
        [Route("{serviceId}")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<ServiceModel>), 200)]
        public async Task<IActionResult> Get([FromRoute] int serviceId)
        {
            return await Execute(async () =>
            {
                var account = await serviceService.GetServiceDetails(serviceId);
                if (account.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(account);
                else
                    return BadRequest(account);
            });
        }

    }
}
