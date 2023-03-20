using System.Threading.Tasks;
using Common.Model;
using Logger;
using Microsoft.AspNetCore.Mvc;
using Sample.Admin.HttpAggregator.IServices;
using Utilities;
using System.Collections.Generic;
using Sample.Admin.Model;

namespace Sample.Admin.HttpAggregator.Controllers
{
    /// <summary>
    /// Currency Controller
    /// </summary>
    [Route("api/currencies")]
    [ApiController]

    public class CurrencyController : BaseApiController
    {
        private readonly ICurrencyService currencyService;

        /// <summary>
        /// Currency Controller Constructor to inject services
        /// </summary>
        /// <param name="currencyService">The currency Service</param>
        /// <param name="logger">The file logger</param>
        public CurrencyController(ICurrencyService currencyService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(currencyService), currencyService);

            this.currencyService = currencyService;
        }

        /// <summary>
        /// Get list of All Currencies
        /// </summary>
        /// <param name="search">Search Fields: (Code, DisplayName)</param>
        /// <param name="offset">The initial index from which to return the results.</param>
        /// <param name="pageSize">Number of results to return per page</param>
        /// <param name="pageNumber">Page Number of results.</param>
        /// <param name="all">It will return results (all pages) without pagination.</param>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResultList<CurrencyModel>), 200)]
        public async Task<IActionResult> GetAllCurrencies([FromQuery]int pageSize, [FromQuery] int pageNumber, [FromQuery] string search, [FromQuery] int offset, [FromQuery] bool all)
        {
            return await Execute(async () =>
            {
                var response = await currencyService.GetAllCurrencies(HttpContext, pageSize, pageNumber, "", search, offset, all);
                if (response.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(response);
                else
                    return BadRequest(response);
            });
        }


        /// <summary>
        /// Get Currency Detail
        /// </summary>
        /// <param name="currencyId">A unique integer value identifying currency.</param>
        /// <returns></returns>
        [Route("{currencyId}")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<CurrencyModel>), 200)]
        public async Task<IActionResult> Get([FromRoute] int currencyId)
        {
            return await Execute(async () =>
            {
                var response = await currencyService.GetCurrencyDetail(currencyId);
                if (response.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(response);
                else
                    return BadRequest(response);
            });
        }
    }
}
