using Sample.Admin.HttpAggregator.Config.OperationsConfig;
using Sample.Admin.HttpAggregator.Config.UrlsConfig;
using Sample.Admin.HttpAggregator.IServices;
using Common.Model;
using Sample.Admin.Model;
using Sample.Admin.Model.Account.Domain;
using Sample.Admin.Model.Account.New;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Utilities;
using Utility;

namespace Sample.Admin.HttpAggregator.Services
{
    ///<summary>
    /// Account Service Class
    ///</summary>
    public class AccountService : IAccountService
    {
        private readonly HttpClient httpClient;

        private readonly ILogger<AccountService> logger;
   
        private readonly ICommonHelper commonHelper;
       

        private readonly BaseUrlsConfig urls;


        /// <summary>
        /// Accounts service constructor to inject required dependency
        /// </summary>
        /// <param name="httpClient">The httpClient</param>
        /// <param name="commonHelper">The Common Helper</param>
        /// <param name="logger">The logger service</param>
        /// <param name="config">The Base Urls config</param>
        public AccountService(HttpClient httpClient,
            ICommonHelper commonHelper,
            ILogger<AccountService> logger,
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
        /// Account Service to get account list
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResultList<AccountsListVM>> GetAllAccounts(HttpContext httpContext, string ordering, string search, int offset, int pageSize, int pageNumber, bool all)
        {
            var responseResult = new ResponseResultList<AccountsListVM>();
            var httpResponse = await this.httpClient.GetAsync(this.urls.AdminAPI + AdminOperations.GetAllAccounts(ordering, search, offset, pageSize, pageNumber, all));
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

            var detail = JsonConvert.DeserializeObject<ResponseResultList<AccountsListVM>>(httpResponse.Content.ReadAsStringAsync().Result);
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
        /// Get Account By Id
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult<AccountViewModel>> GetAccountById(long accountId)
        {
            var responseResult = new ResponseResult<AccountViewModel>();
            var httpResponse = await this.httpClient.GetAsync(this.urls.AdminAPI + AdminOperations.GetAccountById(accountId));
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

            var account = JsonConvert.DeserializeObject<AccountViewModel>(httpResponse.Content.ReadAsStringAsync().Result);
            if (account == null)
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
            responseResult.Data = account;
            return responseResult;
        }

        /// <summary>
        /// To Create new  Account
        /// </summary>
        /// <param name="account">account object</param>
        /// <returns></returns>
        public async Task<ResponseResult<AccountsCreateVM>> AddAccount(AccountViewModel account)
        {
            var responseResult = new ResponseResult<AccountsCreateVM>();
            var postContent = new StringContent(JsonConvert.SerializeObject(account), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.AdminAPI + AdminOperations.AddAccount(), postContent);
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
            var response = JsonConvert.DeserializeObject<ResponseResult<AccountsCreateVM>>(httpResponse.Content.ReadAsStringAsync().Result);
            
            if(response == null)
            {
                return new ResponseResult<AccountsCreateVM>()
                {
                    Message = ResponseMessage.SomethingWentWrong,
                    ResponseCode = ResponseCode.SomethingWentWrong,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.SomethingWentWrong
                    }
                };
            }
            return response;
        }
    
        /// <summary>
        /// To create new account 
        /// </summary>
        /// <param name="account">The new account object</param>
        /// <returns></returns>
        public async Task<ResponseResult> CreateAccount(NewAccount account)
        {
            ResponseResult responseResult = new ResponseResult();
            var postContent = new StringContent(JsonConvert.SerializeObject(account), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.AdminAPI + AdminOperations.CreateAccount(), postContent);
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
            var response = JsonConvert.DeserializeObject<ResponseResult>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                return new ResponseResult()
                {
                    Message = ResponseMessage.InternalServerError,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError
                    }
                };
            }
            if (response.ResponseCode == ResponseCode.RecordSaved)
            {
                responseResult.Data = JsonConvert.DeserializeObject<Account>(Convert.ToString(response.Data));
            }
            responseResult.Message = response.Message;
            responseResult.ResponseCode = response.ResponseCode;
            return responseResult;
        }

        /// <summary>
        /// To update existing Account
        /// </summary>
        /// <param name="account">account object</param>
        /// <param name="accountId">account unique Id</param>
        /// <returns></returns>
        public async Task<ResponseResult<AccountViewModel>> UpdateAccount(long accountId, AccountViewModel account)
        {
            var responseResult = new ResponseResult<AccountViewModel>();
            var postContent = new StringContent(JsonConvert.SerializeObject(account), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PutAsync(this.urls.AdminAPI + AdminOperations.UpdateAccount(accountId), postContent);
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
            var response = JsonConvert.DeserializeObject<ResponseResult<AccountViewModel>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                return new ResponseResult<AccountViewModel>()
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
        /// To update existing Account 
        /// </summary>
        /// <param name="account">account object</param>
        /// <param name="accountId">account unique Id</param>
        /// <returns></returns>
        public async Task<ResponseResult<AccountViewModel>> UpdatePartialAccount(long accountId, AccountViewModel account)
        {
            var responseResult = new ResponseResult<AccountViewModel>();
            var postContent = new StringContent(JsonConvert.SerializeObject(account), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PatchAsync(this.urls.AdminAPI + AdminOperations.UpdatePartialAccount(accountId), postContent);
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
            var response = JsonConvert.DeserializeObject<ResponseResult<AccountViewModel>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                return new ResponseResult<AccountViewModel>()
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
        /// To delete existing Account
        /// </summary>
        /// <param name="accountId">account identifier</param>
        /// <returns></returns>
        public async Task<ResponseResult> DeleteAccount(long accountId)
        {
            var responseResult = new ResponseResult();
            var httpResponse = await this.httpClient.DeleteAsync(this.urls.AdminAPI + AdminOperations.DeleteAccount(accountId));
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

            var account = Convert.ToInt16(httpResponse.Content.ReadAsStringAsync().Result);
            if (account < 1)
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
        /// Account Service to get account details
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<AccountDetail>> GetAccount(long accountId)
        {
            var responseResult = new ResponseResult<AccountDetail>();
            var httpResponse = await this.httpClient.GetAsync(this.urls.AdminAPI + AdminOperations.GetAccount(accountId));
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

            var account = JsonConvert.DeserializeObject<AccountDetail>(httpResponse.Content.ReadAsStringAsync().Result);
            if (account == null)
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
            responseResult.Data = account;
            return responseResult;
        }

        /// <summary>
        /// Account Service to get account details
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<AccountDetail>> GetAccountDetails(long accountId)
        {
            var responseResult = new ResponseResult<AccountDetail>();
            var httpResponse = await this.httpClient.GetAsync(this.urls.AdminAPI + AdminOperations.GetAccountDetails(accountId));
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

            var account = JsonConvert.DeserializeObject<AccountDetail>(httpResponse.Content.ReadAsStringAsync().Result);
            if (account == null)
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
            responseResult.Data = account;
            return responseResult;
        }

        /// <summary>
        /// Account Service to get account list
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult<List<AccountDetail>>> GetAccountList()
        {
            var responseResult = new ResponseResult<List<AccountDetail>>();
            var httpResponse = await this.httpClient.GetAsync(this.urls.AdminAPI + AdminOperations.GetAccountList());
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

            var account = JsonConvert.DeserializeObject<List<AccountDetail>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (account == null)
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
            responseResult.Data = account;
            return responseResult;
        }


        /// <summary>
        /// Account Service to get account details
        /// </summary>
        /// <param name="accountIdentity"></param>
        /// <returns></returns>
        public async Task<ResponseResult<AccountDetail>> GetAccountDetails(string accountIdentity)
        {
            var responseResult = new ResponseResult<AccountDetail>();
            var httpResponse = await this.httpClient.GetAsync(this.urls.AdminAPI + AdminOperations.GetAccountDetails(accountIdentity));
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

            var account = JsonConvert.DeserializeObject<AccountDetail>(httpResponse.Content.ReadAsStringAsync().Result);
            if (account == null)
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
            responseResult.Data = account;
            return responseResult;
        }
    }
}
