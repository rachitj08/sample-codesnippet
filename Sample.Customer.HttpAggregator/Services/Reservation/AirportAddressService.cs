using Common.Model;
using Core.API.ExtensionMethods;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.HttpAggregator.Config.OperationsConfig;
using Sample.Customer.HttpAggregator.Config.UrlsConfig;
using Sample.Customer.HttpAggregator.IServices;
using Sample.Customer.HttpAggregator.IServices.Reservation;
using Sample.Customer.Model.Model;

namespace Sample.Customer.HttpAggregator.Services.Reservation
{
    /// <summary>
    /// 
    /// </summary>
    public class AirportAddressService : IAirportAddressService
    {
        private readonly HttpClient httpClient;
        private readonly BaseUrlsConfig urls;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config"></param>
        public AirportAddressService(HttpClient httpClient, IOptions<BaseUrlsConfig> config)
        {
            this.httpClient = httpClient;
            this.urls = config.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult<List<AirportAddressVM>>> GetAllAirportAddress()
        {
            
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetAllAirportAddress());

            return httpResponse.GetResponseResult<List<AirportAddressVM>>();
        }
    }
}
