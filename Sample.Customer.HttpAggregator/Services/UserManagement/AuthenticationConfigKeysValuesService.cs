using Common.Model;
using Sample.Admin.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sample.Customer.HttpAggregator.Config.OperationsConfig;
using Sample.Customer.HttpAggregator.Config.UrlsConfig;
using Sample.Customer.HttpAggregator.IServices.UserManagement;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Sample.Customer.Model;
using Utilities;

namespace Sample.Customer.HttpAggregator.Services.UserManagement
{
    /// <summary>
    /// Authentication Config Servrice
    /// </summary>
    public class AuthenticationConfigKeysValuesService: IAuthenticationConfigKeysValuesService
    {
        private readonly HttpClient httpClient;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<AuthenticationConfigKeysValuesService> logger;

        //private readonly ICommonHelper commonHelper;

        private readonly BaseUrlsConfig urls;
        //private readonly BaseConfig baseConfig;

        /// <summary>
        /// Group service constructor to inject required dependency
        /// </summary>
        public AuthenticationConfigKeysValuesService(HttpClient httpClient,
            IHttpContextAccessor httpContext,
            ILogger<AuthenticationConfigKeysValuesService> logger,
            IOptions<BaseUrlsConfig> config
            )
        {
            Check.Argument.IsNotNull(nameof(httpClient), httpClient);
            Check.Argument.IsNotNull(nameof(httpContext), httpContext);
            Check.Argument.IsNotNull(nameof(logger), logger);
            Check.Argument.IsNotNull(nameof(config), config); 
            this.httpClient = httpClient;
            this.logger = logger;
            this.urls = config.Value;
            this.httpContextAccessor = httpContext;
        }


        /// <summary>
        /// Get Tenant Auth Settings
        /// </summary>
        /// <param name="authenticationType">Authentication Type</param>
        /// <returns></returns>
        public async Task<ResponseResult<Dictionary<string, string>>> GetTenantAuthSettings(string authenticationType)
        {
            var responseResult = new ResponseResult<Dictionary<string, string>>();
            //long accountId = 0;
            if (httpContextAccessor == null || httpContextAccessor?.HttpContext?.Request?.Headers == null
                || !httpContextAccessor.HttpContext.Request.Headers.ContainsKey("accountid")
                || !long.TryParse(httpContextAccessor.HttpContext.Request.Headers["accountid"], out var accountId)
                || accountId < 1)
            {
                responseResult.Message = ResponseMessage.ValidationFailed;
                responseResult.ResponseCode = ResponseCode.ValidationFailed;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.ValidationFailed
                };
                return responseResult;
            }


            var httpResponseKeysTask = this.httpClient.GetAsync(this.urls.AdminAPI + AdminAPIOperations.GetAllAuthConfigKeys(authenticationType));

            var httpResponseValues = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetAuthConfigKeyValue(accountId));
            if (httpResponseValues == null || !httpResponseValues.IsSuccessStatusCode || httpResponseValues.Content == null)
            {
                responseResult.Message = ResponseMessage.InternalServerError;
                responseResult.ResponseCode = ResponseCode.InternalServerError;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.InternalServerError
                };
                return responseResult;
            }

            // Get Auth values
            var authValues = JsonConvert.DeserializeObject<List<AuthenticationConfigKeyValueModel>>(httpResponseValues.Content.ReadAsStringAsync().Result);
            if (authValues == null)
            {
                responseResult.Message = ResponseMessage.NoRecordFound;
                responseResult.ResponseCode = ResponseCode.NoRecordFound;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.NoRecordFound
                };
                return responseResult;
            }

            //Get Auth Keys Details
            var keyResponse = await httpResponseKeysTask;
            if (keyResponse == null || !keyResponse.IsSuccessStatusCode || keyResponse.Content == null)
            {
                responseResult.Message = ResponseMessage.InternalServerError;
                responseResult.ResponseCode = ResponseCode.InternalServerError;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.InternalServerError
                };
                return responseResult;
            }

            var authKeys = JsonConvert.DeserializeObject<List<AuthenticationConfigKeyModel>>(keyResponse.Content.ReadAsStringAsync().Result);
            if (authKeys == null)
            {
                responseResult.Message = ResponseMessage.NoRecordFound;
                responseResult.ResponseCode = ResponseCode.NoRecordFound;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.NoRecordFound
                };
                return responseResult;
            }

            var result = authKeys
                .Join(authValues, x => x.AuthenticationConfigKeyId, y => y.AuthenticationConfigKeyId, (x, y) => new { x.ConfigKey, y.ConfigKeyValue })
                .ToDictionary(a => a.ConfigKey, a => a.ConfigKeyValue);

            responseResult.Message = ResponseMessage.RecordFetched;
            responseResult.ResponseCode = ResponseCode.RecordFetched;
            responseResult.Data = result;
            return responseResult;
        }

    }
}
