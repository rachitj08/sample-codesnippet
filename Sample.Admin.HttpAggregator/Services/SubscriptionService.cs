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
using Common.Model;
using Sample.Admin.Model;
using System;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Sample.Admin.HttpAggregator.Services
{
    /// <summary>
    /// Subscription Service
    /// </summary>
    public class SubscriptionService : ISubscriptionService
    {
        private readonly HttpClient httpClient;

        private readonly ILogger<SubscriptionService> logger;
        private readonly IModuleService moduleService;
        private readonly IPasswordPolicyService passwordPolicyService;
        private readonly IAccountServiceService accountServiceService;
        private readonly IAccountService accountService;

        private readonly BaseUrlsConfig urls;


        /// <summary>
        /// Account Services service constructor to inject required dependency
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="logger"></param>
        /// <param name="config"></param>
        /// <param name="moduleService"></param>
        /// <param name="passwordPolicyService"></param>
        /// <param name="accountServiceService"></param>
        /// <param name="accountService"></param>
        public SubscriptionService(HttpClient httpClient,
            ILogger<SubscriptionService> logger,
            IOptions<BaseUrlsConfig> config,
            IModuleService moduleService,
            IPasswordPolicyService passwordPolicyService,
            IAccountServiceService accountServiceService,
            IAccountService accountService)
        {
            Check.Argument.IsNotNull(nameof(httpClient), httpClient);
            Check.Argument.IsNotNull(nameof(logger), logger);
            Check.Argument.IsNotNull(nameof(config), config);
            this.httpClient = httpClient;
            this.logger = logger;
            this.urls = config.Value;
            this.moduleService = moduleService;
            this.passwordPolicyService = passwordPolicyService;
            this.accountServiceService = accountServiceService;
            this.accountService = accountService;
        }

        /// <summary>
        /// Subscription Service to get subscription list
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResultList<SubscriptionsListVM>> GetAllSubscriptions(HttpContext httpContext, string ordering, int offset, int pageSize, int pageNumber, bool all)
        {
            var responseResult = new ResponseResultList<SubscriptionsListVM>();
            var httpResponse = await this.httpClient.GetAsync(this.urls.AdminAPI + AdminOperations.GetAllSubscriptions(ordering, offset, pageSize, pageNumber, all));
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

            var detail = JsonConvert.DeserializeObject<ResponseResultList<SubscriptionsListVM>>(httpResponse.Content.ReadAsStringAsync().Result);
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
        /// Get Subscription By Id
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult<SubscriptionsVM>> GetSubscriptionById(long subscriptionId)
        {
            var responseResult = new ResponseResult<SubscriptionsVM>();
            var httpResponse = await this.httpClient.GetAsync(this.urls.AdminAPI + AdminOperations.GetSubscriptionById(subscriptionId));
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

            var subscription = JsonConvert.DeserializeObject<SubscriptionsVM>(httpResponse.Content.ReadAsStringAsync().Result);
            if (subscription == null)
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
            responseResult.Data = subscription;
            return responseResult;
        }

        /// <summary>
        /// To Create new  Subscription
        /// </summary>
        /// <param name="subscription">subscription object</param>
        /// <param name="currentloggedInUserID">subscription object</param>
        /// <returns></returns>
        public async Task<ResponseResult<SubscriptionsVM>> AddSubscription(SubscriptionsModel subscription, long currentloggedInUserID)
        {
            var responseResult = new ResponseResult<SubscriptionsVM>();
            if (subscription == null || subscription.Account < 1)
            {
                responseResult.Message = ResponseMessage.ValidationFailed;
                responseResult.ResponseCode = ResponseCode.ValidationFailed;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.ValidationFailed
                };

                return responseResult;
            }

            var accountServiceResult = await accountServiceService.GetAccountServiceByAccountId(subscription.Account, ((int)Common.Enum.Services.UserManagement));
            if (accountServiceResult != null && accountServiceResult.ResponseCode == ResponseCode.RecordFetched && accountServiceResult.Data != null)
            {
                var postContent = new StringContent(JsonConvert.SerializeObject(subscription), System.Text.Encoding.UTF8, "application/json");
                var httpResponse = await this.httpClient.PostAsync(this.urls.AdminAPI + AdminOperations.AddSubscription(), postContent);
                if (httpResponse == null || httpResponse.Content == null)
                {
                    responseResult.Message = ResponseMessage.SomethingWentWrong;
                    responseResult.ResponseCode = ResponseCode.SomethingWentWrong;
                    responseResult.Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.SomethingWentWrong
                    };
                    return responseResult;
                }

                var subscriptionResult = JsonConvert.DeserializeObject<ResponseResult<SubscriptionsVM>>(httpResponse.Content.ReadAsStringAsync().Result);
                if (subscriptionResult != null)
                {
                    if (subscriptionResult.ResponseCode == ResponseCode.RecordSaved)
                    {
                        var setupUserResult = await SetupRootUser(subscription.Account, currentloggedInUserID);
                        if (setupUserResult != null && (setupUserResult.ResponseCode == ResponseCode.RecordSaved || setupUserResult.ResponseCode == ResponseCode.RootUserConfigurationAlreadyExists))
                        {
                            return new ResponseResult<SubscriptionsVM>()
                            {
                                Message = ResponseMessage.SubscriptionCreated,
                                ResponseCode = ResponseCode.RecordSaved
                            };
                        }
                        else
                        {
                            if (subscriptionResult.Data != null && subscriptionResult.Data.SubscriptionId > 0)
                            {
                                await DeleteSubscription(subscriptionResult.Data.SubscriptionId);
                            }

                            return new ResponseResult<SubscriptionsVM>()
                            {
                                Message = setupUserResult.Message,
                                ResponseCode = setupUserResult.ResponseCode,
                                Error = setupUserResult.Error,
                                Data = setupUserResult.Data
                            };
                        }
                    }
                    else
                    {
                        return subscriptionResult;
                    }
                }
                else
                {
                    return new ResponseResult<SubscriptionsVM>()
                    {
                        Message = ResponseMessage.SomethingWentWrong,
                        ResponseCode = ResponseCode.SomethingWentWrong,
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.SomethingWentWrong
                        }
                    };
                }
            }
            else
            {
                return new ResponseResult<SubscriptionsVM>()
                {
                    Message = ResponseMessage.AccountServiceConfigurationNotAvaliable,
                    ResponseCode = ResponseCode.AccountServiceConfigurationNotAvaliable,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.AccountServiceConfigurationNotAvaliable
                    }
                };
            }
        }

        /// <summary>
        /// To update existing Subscription
        /// </summary>
        /// <param name="subscription">subscription object</param>
        /// <param name="subscriptionId">subscription unique Id</param>
        /// <returns></returns>
        public async Task<ResponseResult<SubscriptionsVM>> UpdateSubscription(long subscriptionId, SubscriptionsModel subscription)
        {
            ResponseResult<SubscriptionsVM> responseResult = new ResponseResult<SubscriptionsVM>();
            var postContent = new StringContent(JsonConvert.SerializeObject(subscription), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PutAsync(this.urls.AdminAPI + AdminOperations.UpdateSubscription(subscriptionId), postContent);
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
            var response = JsonConvert.DeserializeObject<ResponseResult<SubscriptionsVM>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                return new ResponseResult<SubscriptionsVM>()
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
        /// To update existing Subscription 
        /// </summary>
        /// <param name="subscription">subscription object</param>
        /// <param name="subscriptionId">subscription unique Id</param>
        /// <returns></returns>
        public async Task<ResponseResult<SubscriptionsVM>> UpdatePartialSubscription(long subscriptionId, SubscriptionsModel subscription)
        {
            ResponseResult<SubscriptionsVM> responseResult = new ResponseResult<SubscriptionsVM>();
            var postContent = new StringContent(JsonConvert.SerializeObject(subscription), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PatchAsync(this.urls.AdminAPI + AdminOperations.UpdatePartialSubscription(subscriptionId), postContent);
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
            var response = JsonConvert.DeserializeObject<ResponseResult<SubscriptionsVM>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                return new ResponseResult<SubscriptionsVM>()
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
        /// To delete existing Subscription
        /// </summary>
        /// <param name="subscriptionId">subscription identifier</param>
        /// <returns></returns>
        public async Task<ResponseResult<SubscriptionsVM>> DeleteSubscription(long subscriptionId)
        {
            var responseResult = new ResponseResult<SubscriptionsVM>();
            var httpResponse = await this.httpClient.DeleteAsync(this.urls.AdminAPI + AdminOperations.DeleteSubscription(subscriptionId));
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

            var subscription = Convert.ToInt16(httpResponse.Content.ReadAsStringAsync().Result);
            if (subscription < 1)
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
        /// SetupRootUser
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        public async Task<ResponseResult> SetupRootUser(long accountId, long loggedInUserId)
        {
            // fetch account id detils 
            var accountResult = await accountService.GetAccountById(accountId);
            
            if (accountResult != null && accountResult.ResponseCode == ResponseCode.RecordFetched && accountResult.Data != null)
            {
                //Get Password plocy from instance mangement
                var passwordPolicyResult = await passwordPolicyService.GetPasswordPolicy();
                 
                if(passwordPolicyResult.ResponseCode != ResponseCode.RecordFetched || passwordPolicyResult.Data == null)
                {
                    return new ResponseResult()
                    {
                        Message = passwordPolicyResult.Message,
                        ResponseCode = passwordPolicyResult.ResponseCode,
                        Data = passwordPolicyResult.Data,
                        Error = passwordPolicyResult.Error,
                    };
                }

                // Service Call to create default User 
                var rootUser = new RootUserSetupModel
                {
                    CreatedAccount = new AccountInformation
                    {
                        EmailAddress = accountResult.Data.ContactEmail,
                        FirstName = accountResult.Data.ContactPerson,
                        LastName = accountResult.Data.ContactPerson
                    },
                    AccountId = accountId,
                    loggedInUserId = loggedInUserId,
                    PasswordPolicy = passwordPolicyResult.Data
                };

                // Get all user navigation items
                var moduleResult = await moduleService.GetModulesByAccountId(accountId);
                if (moduleResult != null && moduleResult.ResponseCode == ResponseCode.RecordFetched && moduleResult.Data != null)
                {
                    rootUser.VersionModules = moduleResult.Data.Select(x=>x.ModuleId).ToList();
                }

                string serlizedData = JsonConvert.SerializeObject(rootUser);
                var rootUserPostContent = new StringContent(serlizedData, System.Text.Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Add("accountid", Convert.ToString(accountId));
                httpClient.DefaultRequestHeaders.Add("userid", Convert.ToString(loggedInUserId));
                string apiUrl = this.urls.CustomerAPI + CustomerOperations.SetupDefaultTenetUserConfiguration();
                var httpRootUserSetupResponse = await this.httpClient.PostAsync(apiUrl, rootUserPostContent);

                var responseResult = new ResponseResult();

                if (httpRootUserSetupResponse == null || !httpRootUserSetupResponse.IsSuccessStatusCode || httpRootUserSetupResponse.Content == null)
                {
                    responseResult.Message = ResponseMessage.InternalServerError;
                    responseResult.ResponseCode = ResponseCode.InternalServerError;
                    responseResult.Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError
                    };
                    return responseResult;
                }

                responseResult = JsonConvert.DeserializeObject<ResponseResult>(httpRootUserSetupResponse.Content.ReadAsStringAsync().Result);
                if (responseResult != null)
                {
                    return responseResult;
                }
                else
                {
                    return new ResponseResult()
                    {
                        Message = ResponseMessage.SomethingWentWrong,
                        ResponseCode = ResponseCode.SomethingWentWrong,
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.SomethingWentWrong
                        }
                    };
                }
            }
            else
            {
                return new ResponseResult()
                {
                    Message = ResponseMessage.AccountNotExist,
                    ResponseCode = ResponseCode.AccountNotExist,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.AccountNotExist
                    }
                };
            }
            
        }

    }
}
