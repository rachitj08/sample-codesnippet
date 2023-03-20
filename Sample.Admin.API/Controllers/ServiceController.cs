using System.Threading.Tasks;
using Common.Model;
using Sample.Admin.Service.ServiceWorker;
using Logger;
using Microsoft.AspNetCore.Mvc;
using Utilities;

namespace Sample.Admin.API.Controllers
{
    [Route("api/services")]
    [ApiController]
    public class ServiceController : BaseApiController
    {
        private readonly IServiceService serviceService;

        /// <summary>
        /// Service Type Controller constructor to Inject dependency
        /// </summary>
        /// <param name="serviceService">service for type service</param>
        /// 
        public ServiceController(IServiceService serviceService, IFileLogger logger) : base(logger: logger)
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
        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetAllServices([FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] string ordering, [FromQuery] string search, [FromQuery] int offset, [FromQuery] bool all)
        {
            
            return await Execute(async () =>
            {
                var result = await this.serviceService.GetAllServices(pageSize, pageNumber, ordering, search, offset, all);
                return Ok(result);
            });

        }

        /// <summary>
        /// Get Service Detail
        /// </summary>
        /// <param name="serviceId">A unique integer value identifying service.</param>
        /// <returns></returns>
        [Route("{serviceId}")]
        [HttpGet]
        public async Task<IActionResult> Get([FromRoute] int serviceId)
        {
            return await Execute(async () =>
            {
                var account = await serviceService.GetServiceDetail(serviceId);
                return Ok(account);
            });
        }
    }
}
