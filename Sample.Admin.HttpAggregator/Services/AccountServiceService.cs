using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sample.Admin.HttpAggregator.Config.OperationsConfig;
using Sample.Admin.HttpAggregator.Config.UrlsConfig;
using Sample.Admin.HttpAggregator.IServices;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Utilities;
using Model=Sample.Admin.Model;
using Common.Model;
using Sample.Admin.Model;
using System;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Sample.Admin.HttpAggregator.Services
{
    /// <summary>
    /// AccountServiceService
    /// </summary>
    public class AccountServiceService : IAccountServiceService
    {


        private readonly HttpClient httpClient;

        private readonly ILogger<AccountServiceService> logger;

        private readonly BaseUrlsConfig urls;


        /// <summary>
        /// Account Services service constructor to inject required dependency
        /// </summary>
        /// <param name="httpClient">The httpClient</param>
        /// <param name="logger">The logger service</param>
        /// <param name="config">The Base Urls config</param>
        public AccountServiceService(HttpClient httpClient,

            ILogger<AccountServiceService> logger,
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
        /// AccountService Service to get accountService list
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResultList<AccountServicesVM>> GetAllAccountServices(HttpContext httpContext, string ordering, int offset, int pageSize, int pageNumber, bool all)
        {
            var responseResult = new ResponseResultList<AccountServicesVM>();
            var httpResponse = await this.httpClient.GetAsync(this.urls.AdminAPI + AdminOperations.GetAllAccountServices(ordering, offset, pageSize, pageNumber, all));
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

            var detail = JsonConvert.DeserializeObject<ResponseResultList<AccountServicesVM>>(httpResponse.Content.ReadAsStringAsync().Result);
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
        /// Get AccountService By Id
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult<AccountServicesVM>> GetAccountServiceById(int accountServiceId)
        {
            var responseResult = new ResponseResult<AccountServicesVM>();
            var httpResponse = await this.httpClient.GetAsync(this.urls.AdminAPI + AdminOperations.GetAccountServiceById(accountServiceId));
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

            var accountService = JsonConvert.DeserializeObject<AccountServicesVM>(httpResponse.Content.ReadAsStringAsync().Result);
            if (accountService == null)
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
            responseResult.Data = accountService;
            return responseResult;
        }

        /// <summary>
        /// To Create new  AccountService
        /// </summary>
        /// <param name="accountService">accountService object</param>
        /// <returns></returns>
        public async Task<ResponseResult<AccountServicesVM>> AddAccountService(AccountServicesModel accountService)
        {
            ResponseResult<AccountServicesVM> responseResult = new ResponseResult<AccountServicesVM>();
            var postContent = new StringContent(JsonConvert.SerializeObject(accountService), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.AdminAPI + AdminOperations.AddAccountService(), postContent);
            if (httpResponse == null || httpResponse.Content == null)
            {
                responseResult.Message = ResponseMessage.InternalServerError;
                responseResult.ResponseCode = ResponseCode.InternalServerError;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.InternalServerError
                };
                return responseResult;
            }
            var response = JsonConvert.DeserializeObject<ResponseResult<AccountServicesVM>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                return new ResponseResult<AccountServicesVM>()
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
        /// To update existing AccountService
        /// </summary>
        /// <param name="accountService">accountService object</param>
        /// <param name="accountServiceId">accountService unique Id</param>
        /// <returns></returns>
        public async Task<ResponseResult<AccountServicesVM>> UpdateAccountService(int accountServiceId, AccountServicesModel accountService)
        {
            ResponseResult<AccountServicesVM> responseResult = new ResponseResult<AccountServicesVM>();
            var postContent = new StringContent(JsonConvert.SerializeObject(accountService), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PutAsync(this.urls.AdminAPI + AdminOperations.UpdateAccountService(accountServiceId), postContent);
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

            var response = JsonConvert.DeserializeObject<ResponseResult<AccountServicesVM>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                return new ResponseResult<AccountServicesVM>()
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
        /// To update existing AccountService 
        /// </summary>
        /// <param name="accountService">accountService object</param>
        /// <param name="accountServiceId">accountService unique Id</param>
        /// <returns></returns>
        public async Task<ResponseResult<AccountServicesVM>> UpdatePartialAccountService(int accountServiceId, AccountServicesModel accountService)
        {
            ResponseResult<AccountServicesVM> responseResult = new ResponseResult<AccountServicesVM>();
            var postContent = new StringContent(JsonConvert.SerializeObject(accountService), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PatchAsync(this.urls.AdminAPI + AdminOperations.UpdatePartialAccountService(accountServiceId), postContent);
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
            var response = JsonConvert.DeserializeObject<ResponseResult<AccountServicesVM>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                return new ResponseResult<AccountServicesVM>()
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
        /// To delete existing AccountService
        /// </summary>
        /// <param name="accountServiceId">accountService identifier</param>
        /// <returns></returns>
        public async Task<ResponseResult<AccountServicesVM>> DeleteAccountService(int accountServiceId)
        {
            var responseResult = new ResponseResult<AccountServicesVM>();
            var httpResponse = await this.httpClient.DeleteAsync(this.urls.AdminAPI + AdminOperations.DeleteAccountService(accountServiceId));
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

            var accountService = Convert.ToInt16(httpResponse.Content.ReadAsStringAsync().Result);
            if (accountService < 1)
            {
                responseResult.Message = ResponseMessage.NoRecordFound;
                responseResult.ResponseCode = ResponseCode.NoRecordFound;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.NoRecordFound
                };
                return responseResult;
            }

            responseResult.Message = ResponseMessage.RecordDeleted;
            responseResult.ResponseCode = ResponseCode.RecordDeleted;
            responseResult.Data = null;
            return responseResult;
        }


        /// <summary>
        /// To Get List of All Account service 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Model.AccountService>> GetAllAccountServices()
        {

            var httpResponse = await this.httpClient.GetAsync(this.urls.AdminAPI + AdminOperations.GetAllAccountServices());
            if (httpResponse.IsSuccessStatusCode)
            {

                var accountServicseData = (httpResponse != null) ? JsonConvert.DeserializeObject<List<Model.AccountService>>(httpResponse.Content.ReadAsStringAsync().Result) : null;
                if (accountServicseData != null && accountServicseData.Count > 0)
                {
                    return accountServicseData;
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get AccountService By Id
        /// </summary>
        /// <param name="accountId">account Id</param>
        /// <param name="serviceId">service Id</param>
        /// <returns></returns>
        public async Task<ResponseResult<AccountServicesVM>> GetAccountServiceByAccountId(long accountId, int serviceId)
        {
            var responseResult = new ResponseResult<AccountServicesVM>();
            var httpResponse = await this.httpClient.GetAsync(this.urls.AdminAPI + AdminOperations.GetAccountServiceByAccountId(accountId, serviceId));
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

            var accountService = JsonConvert.DeserializeObject<AccountServicesVM>(httpResponse.Content.ReadAsStringAsync().Result);
            if (accountService == null)
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
            responseResult.Data = accountService;
            return responseResult;
        }


    }
}
