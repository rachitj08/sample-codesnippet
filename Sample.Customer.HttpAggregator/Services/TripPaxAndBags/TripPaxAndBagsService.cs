using Common.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.HttpAggregator.Config.OperationsConfig;
using Sample.Customer.HttpAggregator.Config.UrlsConfig;
using Sample.Customer.HttpAggregator.IServices.TripPaxAndBags;
using Sample.Customer.Model.Model;

namespace Sample.Customer.HttpAggregator.Services.TripPaxAndBags
{
    /// <summary>
    /// 
    /// </summary>
    public class TripPaxAndBagsService : ITripPaxAndBagsService
    {
        private readonly HttpClient httpClient;
        private readonly BaseUrlsConfig urls;

        /// <summary>
        /// ReservationService
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config"></param>
        public TripPaxAndBagsService(HttpClient httpClient, IOptions<BaseUrlsConfig> config)
        {
            Check.Argument.IsNotNull(nameof(httpClient), httpClient);
            Check.Argument.IsNotNull(nameof(config), config);
            this.httpClient = httpClient;
            this.urls = config.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tripPaxAndBags"></param>
        /// <returns></returns>
        public async Task<ResponseResult<TripPaxAndBagsVM>> SaveTripPaxAndBags(TripPaxAndBagsVM tripPaxAndBags)
        {
            var responseResult = new ResponseResult<TripPaxAndBagsVM>();
            var postContent = new StringContent(JsonConvert.SerializeObject(tripPaxAndBags), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + CustomerAPIOperations.SaveTripPaxAndBags(), postContent);
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
            responseResult = JsonConvert.DeserializeObject<ResponseResult<TripPaxAndBagsVM>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (responseResult == null)
            {
                responseResult.Message = ResponseMessage.SomethingWentWrong;
                responseResult.ResponseCode = ResponseCode.SomethingWentWrong;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.SomethingWentWrong
                };
            }
            return responseResult;
        }
    }
}
