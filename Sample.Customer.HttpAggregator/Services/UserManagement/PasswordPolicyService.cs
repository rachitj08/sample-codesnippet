using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Common.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sample.Customer.HttpAggregator.Config.OperationsConfig;
using Sample.Customer.HttpAggregator.Config.UrlsConfig;
using Sample.Customer.HttpAggregator.IServices.UserManagement;
using Newtonsoft.Json;
using Sample.Customer.Model;
using Utilities;

namespace Sample.Customer.HttpAggregator.Services.UserManagement
{
    /// <summary>
    /// Password Policy
    /// </summary>
    public class PasswordPolicyService : IPasswordPolicyService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<PasswordPolicyService> logger;
        private readonly BaseUrlsConfig urls;

        /// <summary>
        /// Users Service constructor to inject required dependency
        /// </summary>
        /// <param name="httpClient">The httpClient</param>
        /// <param name="logger">The logger service</param>
        /// <param name="config">The Base Urls config</param>
        public PasswordPolicyService(HttpClient httpClient,
            ILogger<PasswordPolicyService> logger,
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
        /// Get Password Policy By Account Id
        /// </summary>
        public async Task<ResponseResult<PasswordPolicyModel>> GetPasswordPolicyByAccountId(long accountId)
        {
            var responseResult = new ResponseResult<PasswordPolicyModel>();
            var httpResponse = await httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetPasswordPolicyByAccountId(accountId));
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

            var passwordPolicy = (httpResponse != null) ? JsonConvert.DeserializeObject<PasswordPolicyModel>(httpResponse.Content.ReadAsStringAsync().Result) : null;
            if (passwordPolicy == null)
            {
                responseResult.Message = ResponseMessage.InternalServerError;
                responseResult.ResponseCode = ResponseCode.InternalServerError;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.InternalServerError
                };
                return responseResult;
            }

            responseResult.Message = ResponseMessage.RecordFetched;
            responseResult.ResponseCode = ResponseCode.RecordFetched;
            responseResult.Data = passwordPolicy;
            return responseResult;
        }

        /// <summary>
        /// To Create new Password policy
        /// </summary>
        /// <param name="model">passwordPolicy object</param>
        /// <returns></returns>
        public async Task<ResponseResult<PasswordPolicyModel>> CreatePasswordPolicy(PasswordPolicyModel model)
        {
            ResponseResult<PasswordPolicyModel> responseResult = new ResponseResult<PasswordPolicyModel>();
            var postContent = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + CustomerAPIOperations.CreatePasswordPolicy(), postContent);
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
            var response = JsonConvert.DeserializeObject<ResponseResult<PasswordPolicyModel>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                return new ResponseResult<PasswordPolicyModel>()
                {
                    Message = ResponseMessage.InternalServerError,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError
                    }
                };
            }

            return response;
        }

        /// <summary>
        /// To update existing Password policy
        /// </summary>
        /// <param name="model">password policy object</param>
        /// <returns></returns>
        public async Task<ResponseResult<PasswordPolicyModel>> UpdatePasswordPolicy(PasswordPolicyModel model)
        {
            ResponseResult<PasswordPolicyModel> responseResult = new ResponseResult<PasswordPolicyModel>();
            var postContent = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PutAsync(this.urls.CustomerAPI + CustomerAPIOperations.UpdatePasswordPolicy(), postContent);
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
            var response = JsonConvert.DeserializeObject<ResponseResult<PasswordPolicyModel>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                return new ResponseResult<PasswordPolicyModel>()
                {
                    Message = ResponseMessage.InternalServerError,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError
                    }
                };
            }

            return response;
        }
    }
}
