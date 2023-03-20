using Common.Model;
using Sample.Admin.Model;
using System.Threading.Tasks;

namespace Sample.Admin.Service.Infrastructure.Repository
{
    public interface ICurrencyRepository
    {
        /// <summary>
        /// To Get All Currencies
        /// </summary>
        /// <param name="search">Search Fields: (Code, DisplayName)</param>
        /// <param name="offset">The initial index from which to return the results.</param>
        /// <param name="ordering">Ordering Fields: (All Fields)</param>
        /// <param name="pageSize">Number of results to return per page</param>
        /// <param name="pageNumber">Page Number of results.</param>
        /// <param name="all">It will return results (all pages) without pagination.</param>
        /// <returns></returns>
        Task<ResponseResultList<CurrencyModel>> GetAllCurrencies(int pageSize, int pageNumber, string ordering, string search, int offset, bool all);


        /// <summary>
        /// Get Currency Detail
        /// </summary>
        /// <param name="currencyId">Currency Id</param>
        /// <returns></returns>
        Task<CurrencyModel> GetCurrencyDetail(int currencyId);
    }
}
