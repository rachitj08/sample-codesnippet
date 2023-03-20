using System.Threading.Tasks;
using Sample.Admin.Service.ServiceWorker;
using Logger;
using Microsoft.AspNetCore.Mvc;
using Utilities;

namespace Sample.Admin.API.Controllers
{
    [Route("api/currencies")]
    [ApiController]
    public class CurrencyController : BaseApiController
    {
        private readonly ICurrencyService currencyService;

        /// <summary>
        /// Currency Type Controller constructor to Inject dependency
        /// </summary>
        /// <param name="currencyService">Service for currency</param>
        public CurrencyController(ICurrencyService currencyService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(currencyService), currencyService);
            this.currencyService = currencyService;
        }

        /// <summary>
        /// Information for currencies 
        /// </summary>
        /// <param name="search">Search Fields: (Code, DisplayName)</param>
        /// <param name="offset">The initial index from which to return the results.</param>
        /// <param name="ordering">Ordering Fields: (All Fields)</param>
        /// <param name="pageSize">Number of results to return per page</param>
        /// <param name="pageNumber">Page Number of results.</param>
        /// <param name="all">It will return results (all pages) without pagination.</param>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetAllCurrencies([FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] string ordering, [FromQuery] string search, [FromQuery] int offset, [FromQuery] bool all)
        {
            return await Execute(async () =>
            {
                var result = await this.currencyService.GetAllCurrencies(pageSize, pageNumber, ordering, search, offset, all);
                return Ok(result);
            });

        }


        /// <summary>
        /// Information for currencies 
        /// </summary>
        /// <param name="currencyId">Currency Id</param>
        /// <returns></returns>
        [Route("{currencyId}")]
        [HttpGet]
        public async Task<IActionResult> GetCurrencyDetail([FromRoute] int currencyId)
        {
            return await Execute(async () =>
            {
                var result = await this.currencyService.GetCurrencyDetail(currencyId);
                return Ok(result);
            });

        }

    }
}
