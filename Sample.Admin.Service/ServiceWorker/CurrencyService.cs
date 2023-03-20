using Common.Model;
using Sample.Admin.Service.Infrastructure.Repository;
using Sample.Admin.Model;
using System.Threading.Tasks;

namespace Sample.Admin.Service.ServiceWorker
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository currencyRepository;

        /// <summary>
        /// Currency Service constructor to Inject dependency
        /// </summary>
        /// <param name="currencyRepository">currency repository</param>
        public CurrencyService(ICurrencyRepository currencyRepository)
        {
            this.currencyRepository = currencyRepository;
        }

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
        public async Task<ResponseResultList<CurrencyModel>> GetAllCurrencies(int pageSize, int pageNumber, string ordering, string search, int offset, bool all)
        {
            return await this.currencyRepository.GetAllCurrencies(pageSize, pageNumber, ordering, search, offset, all);
        }


        //// <summary>
        /// Get Currency Detail
        /// </summary>
        /// <param name="currencyId">Currency Id</param>
        /// <returns></returns>
        public async Task<CurrencyModel> GetCurrencyDetail(int currencyId)
        {
            return await this.currencyRepository.GetCurrencyDetail(currencyId);
        }
    }
}
