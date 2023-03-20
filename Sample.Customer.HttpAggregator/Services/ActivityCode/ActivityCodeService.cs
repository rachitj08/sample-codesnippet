using Common.Model;
using Core.API.ExtensionMethods;
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
using Sample.Customer.HttpAggregator.IServices.ActivityCode;
using Sample.Customer.Model.Model;

namespace Sample.Customer.HttpAggregator.Services.ActivityCode
{
    /// <summary>
    /// ActivityCodeService
    /// </summary>
    public class ActivityCodeService : IActivityCodeService
    {
        private readonly HttpClient httpClient;
        private readonly BaseUrlsConfig urls;

        /// <summary>
        /// ActivityCodeService
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="_kafkaConfig"></param>
        /// <param name="config"></param>
        public ActivityCodeService(HttpClient httpClient,
            IOptions<BaseUrlsConfig> config)
        {
            Check.Argument.IsNotNull(nameof(httpClient), httpClient);
            Check.Argument.IsNotNull(nameof(config), config);
            this.httpClient = httpClient;
            this.urls = config.Value;
        }

        /// <summary>
        /// GetActivityCodeById
        /// </summary>
        /// <param name="activityCodeId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<ActivityCodeVM>> GetActivityCodeById(long activityCodeId)
        {
            var responseResult = new ResponseResult<ActivityCodeVM>();
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetRActivityCodeById(activityCodeId));
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
            responseResult = JsonConvert.DeserializeObject<ResponseResult<ActivityCodeVM>>(httpResponse.Content.ReadAsStringAsync().Result);
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
        /// <summary>
        /// Get All Activity
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult<List<ActivityCodeVM>>> GetAllActivity(long parkingProviderLocationId)
        {
            
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetAllActivity(parkingProviderLocationId));
            return httpResponse.GetResponseResult<List<ActivityCodeVM>>();
            
        }
    }
}
