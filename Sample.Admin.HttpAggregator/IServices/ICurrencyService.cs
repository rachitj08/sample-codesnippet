using Common.Model;
using Sample.Admin.Model;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Admin.HttpAggregator.IServices
{
    /// <summary>
    /// Currency Service interface
    /// </summary>
    public interface ICurrencyService
    {

        /// <summary>
        /// To Get List of currencies
        /// </summary>
        /// <param name="httpContext">httpContext</param>
        /// <param name="search">Search Fields: (Code, DisplayName)</param>
        /// <param name="offset">The initial index from which to return the results.</param>
        /// <param name="ordering">Ordering Fields: (All Fields)</param>
        /// <param name="pageSize">Number of results to return per page</param>
        /// <param name="pageNumber">Page Number of results.</param>
        /// <param name="all">It will return results (all pages) without pagination.</param>
        /// <returns></returns>
        Task<ResponseResultList<CurrencyModel>> GetAllCurrencies(HttpContext httpContext, int pageSize, int pageNumber, string ordering, string search, int offset, bool all);

        /// <summary>
        /// Currency Details
        /// </summary>
        /// <param name="currencyId">Currency Id</param>
        /// <returns></returns>
        Task<ResponseResult<CurrencyModel>> GetCurrencyDetail(int currencyId);
    }
}
