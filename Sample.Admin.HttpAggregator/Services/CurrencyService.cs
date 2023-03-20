using Sample.Admin.HttpAggregator.Config.OperationsConfig;
using Sample.Admin.HttpAggregator.Config.UrlsConfig;
using Sample.Admin.HttpAggregator.IServices;
using Common.Model;
using Sample.Admin.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Utilities;

namespace Sample.Admin.HttpAggregator.Services
{
    /// <summary>
    /// Currency Service interface
    /// </summary>
    public class CurrencyService : ICurrencyService
    {
        private readonly HttpClient httpClient;


        private readonly ILogger<CurrencyService> logger;


        private readonly BaseUrlsConfig urls;


        /// <summary>
        /// currency service constructor to inject required dependency
        /// </summary>
        /// <param name="httpClient">The httpClient</param>
        /// <param name="logger">The logger service</param>
        /// <param name="config">The Base Urls config</param>
        public CurrencyService(HttpClient httpClient,
            ILogger<CurrencyService> logger,
            IOptions<BaseUrlsConfig> config)
        {
            Check.Argument.IsNotNull(nameof(httpClient), httpClient);
            Check.Argument.IsNotNull(nameof(logger), logger);
            Check.Argument.IsNotNull(nameof(config), config);

            this.httpClient = httpClient;
            this.logger = logger;
            this.urls = config.Value;
        }


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
        public async Task<ResponseResultList<CurrencyModel>> GetAllCurrencies(HttpContext httpContext, int pageSize, int pageNumber, string ordering, string search, int offset, bool all)
        {
            var responseResult = new ResponseResultList<CurrencyModel>();
            var httpResponse = await this.httpClient.GetAsync(this.urls.AdminAPI + AdminOperations.GetAllCurrencies(pageSize, pageNumber, ordering, search, offset, all));
            if (httpResponse == null || !httpResponse.IsSuccessStatusCode || httpResponse.Content == null)
            {
                responseResult.Message = ResponseMessage.InternalServerError;
                responseResult.ResponseCode = ResponseCode.InternalServerError;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.InternalServerError
                };
                return responseResult;
            }

            var detail = JsonConvert.DeserializeObject<ResponseResultList<CurrencyModel>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (detail == null)
            {
                responseResult.Message = ResponseMessage.NoRecordFound;
                responseResult.ResponseCode = ResponseCode.NoRecordFound;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.NoRecordFound
                };
                return responseResult;
            }

            var basePath = @$"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}?";
            if (!string.IsNullOrEmpty(detail.Next)) detail.Next = basePath + detail.Next;
            if (!string.IsNullOrEmpty(detail.Previous)) detail.Previous = basePath + detail.Previous;
            detail.Message = ResponseMessage.RecordFetched;
            detail.ResponseCode = ResponseCode.RecordFetched;
            return detail;
        }

        /// <summary>
        /// Currency Details
        /// </summary>
        /// <param name="currencyId">Currency Id</param>
        /// <returns></returns>
        public async Task<ResponseResult<CurrencyModel>> GetCurrencyDetail(int currencyId)
        {
            var responseResult = new ResponseResult<CurrencyModel>();
            var httpResponse = await this.httpClient.GetAsync(this.urls.AdminAPI + AdminOperations.GetCurrencyDetails(currencyId));
            if (httpResponse == null || !httpResponse.IsSuccessStatusCode || httpResponse.Content == null)
            {
                responseResult.Message = ResponseMessage.InternalServerError;
                responseResult.ResponseCode = ResponseCode.InternalServerError;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.InternalServerError
                };
                return responseResult;
            }

            var detail = JsonConvert.DeserializeObject<CurrencyModel>(httpResponse.Content.ReadAsStringAsync().Result);
            if (detail == null)
            {
                responseResult.Message = ResponseMessage.NoRecordFound;
                responseResult.ResponseCode = ResponseCode.NoRecordFound;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.NoRecordFound
                };
                return responseResult;
            }

            responseResult.Message = ResponseMessage.RecordFetched;
            responseResult.ResponseCode = ResponseCode.RecordFetched;
            responseResult.Data = detail;
            return responseResult;
        }

    }
}
