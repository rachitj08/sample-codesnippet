using Sample.Admin.HttpAggregator.Config.OperationsConfig;
using Sample.Admin.HttpAggregator.Config.UrlsConfig;
using Sample.Admin.HttpAggregator.IServices;
using Common.Model;
using Sample.Admin.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Utilities;
using Utility;

namespace Sample.Admin.HttpAggregator.Services
{
    /// <summary>
    /// PasswordPolicyService
    /// </summary>
    public class PasswordPolicyService : IPasswordPolicyService
    {
        private readonly HttpClient httpClient;

        private readonly ILogger<GroupService> logger;

        private readonly ICommonHelper commonHelper;

        private readonly BaseUrlsConfig urls;

        /// <summary>
        /// Group service constructor to inject required dependency
        /// </summary>
        /// <param name="httpClient">The httpClient</param>
        /// <param name="commonHelper">The Common Helper</param>
        /// <param name="logger">The logger service</param>
        /// <param name="config">The Base Urls config</param>
        public PasswordPolicyService(HttpClient httpClient,
            ICommonHelper commonHelper,
            ILogger<GroupService> logger,
            IOptions<BaseUrlsConfig> config)
        {
            Check.Argument.IsNotNull(nameof(httpClient), httpClient);
            Check.Argument.IsNotNull(nameof(commonHelper), commonHelper);
            Check.Argument.IsNotNull(nameof(logger), logger);
            Check.Argument.IsNotNull(nameof(config), config);

            this.commonHelper = commonHelper;
            this.httpClient = httpClient;
            this.logger = logger;
            this.urls = config.Value;
        }


        /// <summary>
        /// CreatePasswordPolicy
        /// </summary>
        /// <param name="passwordPolicy"></param>
        /// <returns></returns>
        public async Task<ResponseResult> CreateOrUpdatePasswordPolicy(PasswordPolicyVM passwordPolicy)
        {
            var responseResult = new ResponseResult();
            var postContent = new StringContent(JsonConvert.SerializeObject(passwordPolicy), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.AdminAPI + AdminOperations.CreateOrUpdatePasswordPolicy(), postContent);
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

            responseResult = JsonConvert.DeserializeObject<ResponseResult>(httpResponse.Content.ReadAsStringAsync().Result);
            if (responseResult == null)
            {
                responseResult.Message = ResponseMessage.InternalServerError;
                responseResult.ResponseCode = ResponseCode.InternalServerError;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.InternalServerError
                };
                return responseResult;
            }

            return responseResult;
        }
       
        /// <summary>
        /// GetPasswordPolicy
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult<PasswordPolicyVM>> GetPasswordPolicy()
        {
            var responseResult = new ResponseResult<PasswordPolicyVM>();
            var httpResponse = await this.httpClient.GetAsync(this.urls.AdminAPI + AdminOperations.GetPasswordPolicy());
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

            responseResult = JsonConvert.DeserializeObject<ResponseResult<PasswordPolicyVM>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (responseResult == null)
            {
                responseResult.Message = ResponseMessage.NoRecordFound;
                responseResult.ResponseCode = ResponseCode.NoRecordFound;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.NoRecordFound
                };
                return responseResult;
            }

            return responseResult;
        }
        
    }
}
