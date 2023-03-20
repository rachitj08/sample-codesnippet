using Common.Model;
using Core.API.ExtensionMethods;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Utilities;
using Utility;
using Sample.Customer.HttpAggregator.Config.OperationsConfig;
using Sample.Customer.HttpAggregator.Config.UrlsConfig;
using Sample.Customer.HttpAggregator.IServices.Airline;
using Sample.Customer.Model.Model;

namespace Sample.Customer.HttpAggregator.Services.Airline
{
    /// <summary>
    /// 
    /// </summary>
    public class AirlineService  : IAirlineService
    {
        private readonly HttpClient httpClient;
        private readonly BaseUrlsConfig urls;
        private readonly CommonConfig _commonConfig;
        private readonly ICommonHelper _commonHelper;

        /// <summary>
        ///  Resercation Service
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config"></param>
        /// <param name="commonConfig"></param>
        /// <param name="commonHelper"></param>
        public AirlineService(HttpClient httpClient,
            IOptions<BaseUrlsConfig> config, IOptions<CommonConfig> commonConfig
            , ICommonHelper commonHelper
            )
        {
            Check.Argument.IsNotNull(nameof(httpClient), httpClient);
            Check.Argument.IsNotNull(nameof(config), config);
            this.httpClient = httpClient;
            this.urls = config.Value;
            _commonConfig = commonConfig.Value;
            _commonHelper = commonHelper;
        }
        /// <summary>
        /// Get Airline List
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<ResponseResult<List<AirlineVM>>> GetAirlineList()
        {
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetAirlineList());
            return httpResponse.GetResponseResult<List<AirlineVM>>();
        }
       
    }
}
