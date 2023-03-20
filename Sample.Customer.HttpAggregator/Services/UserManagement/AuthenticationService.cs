using Common.Enum;
using Common.Model;
using Core.API.ExtensionMethods;
using Google.Apis.Auth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Utilities;
using Sample.Admin.Model.Account.Domain;
using Sample.Customer.HttpAggregator.Config.OperationsConfig;
using Sample.Customer.HttpAggregator.Config.UrlsConfig;
using Sample.Customer.HttpAggregator.IServices.UserManagement;
using Sample.Customer.Model;
using User = Sample.Customer.Model.User;

namespace Sample.Customer.HttpAggregator.Services.UserManagement
{
    /// <summary>
    /// Authentication Service
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {

        private readonly HttpClient httpClient;

        private readonly ILogger<AuthenticationService> logger;

        private readonly BaseUrlsConfig urls;

        private readonly BaseConfig baseConfig;

        private readonly GoogleConfig googleConfig;

        private readonly FacebookConfig facebookConfig;

        /// <summary>
        /// Users Service constructor to inject required dependency
        /// </summary>
        /// <param name="httpClient">The httpClient</param>
        /// <param name="logger">The logger service</param>
        /// <param name="config">The Base Urls config</param>
        /// <param name="baseConfig">The Base config</param>
        /// <param name="googleConfig">Google Auth Config</param>
        /// <param name="facebookConfig">Facebook Auth Config</param>
        public AuthenticationService(HttpClient httpClient,
            ILogger<AuthenticationService> logger,
            IOptions<BaseUrlsConfig> config, IOptions<BaseConfig> baseConfig, 
            IOptions<GoogleConfig> googleConfig, IOptions<FacebookConfig> facebookConfig)
        {
            Check.Argument.IsNotNull(nameof(httpClient), httpClient);
            Check.Argument.IsNotNull(nameof(logger), logger);
            Check.Argument.IsNotNull(nameof(config), config);
            Check.Argument.IsNotNull(nameof(baseConfig), baseConfig);
            Check.Argument.IsNotNull(nameof(googleConfig), baseConfig);
            Check.Argument.IsNotNull(nameof(facebookConfig), facebookConfig);
            this.httpClient = httpClient;
            this.logger = logger;
            this.urls = config.Value;
            this.baseConfig = baseConfig.Value;
            this.googleConfig = googleConfig.Value;
            this.facebookConfig = facebookConfig.Value;
        }
        /// <summary>
        /// Authenticate
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public ResponseResult<User> Authenticate(Login login)
        {
            var tasks = new List<Task<HttpResponseMessage>>();
            HttpResponseMessage accountResponse, iamResponse, moduleResponse;
            var postContent = new StringContent(JsonConvert.SerializeObject(login), System.Text.Encoding.UTF8, "application/json");
            var httpResponseAccountTask = this.httpClient.GetAsync(this.urls.AdminAPI + AdminAPIOperations.GetAccount(login.AccountId));
            tasks.Add(httpResponseAccountTask);
            var httpResponseIAM = this.httpClient.PostAsync(this.urls.CustomerAPI + CustomerAPIOperations.Authenticate(), postContent);
            tasks.Add(httpResponseIAM);
            var httpResponseModule = this.httpClient.GetAsync(this.urls.AdminAPI + AdminAPIOperations.GetAllModules(false, login.AccountId, (int)Common.Enum.Services.UserManagement));
            tasks.Add(httpResponseModule);
            Task.WaitAll(tasks.ToArray());
            foreach (var t in tasks)
            {
                if (t.IsCompleted)
                {
                    t.Wait();
                }
            }
            accountResponse = tasks[0].Result;
            iamResponse = tasks[1].Result;
            moduleResponse = tasks[2].Result;
            if (iamResponse == null || !iamResponse.IsSuccessStatusCode)
            {
                return new ResponseResult<User>()
                {
                    Message = ResponseMessage.InternalServerError,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError,
                    }
                };
            }

            var response = JsonConvert.DeserializeObject<ResponseResult<User>>(iamResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                return new ResponseResult<User>()
                {
                    Message = ResponseMessage.InternalServerError,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError,
                    }
                };
            }

            if (response.ResponseCode != ResponseCode.ValidLogin)
            {
                return new ResponseResult<User>()
                {
                    Message = response.Message,
                    ResponseCode = response.ResponseCode,
                    Error = response.Error,
                };
            }

            if (accountResponse == null || !accountResponse.IsSuccessStatusCode)
            {
                return new ResponseResult<User>()
                {
                    Message = ResponseMessage.InternalServerError,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError,
                    }
                };
            }

            var account = JsonConvert.DeserializeObject<AccountModel>(accountResponse.Content.ReadAsStringAsync().Result);
            if (account == null)
            {
                return new ResponseResult<User>()
                {
                    Message = ResponseMessage.InternalServerError,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError,
                    }
                };
            }

            var userAccount = new UserAccount()
            {
                AccountId = account.AccountId,
                AccountGUID = account.AccountGuid,
                OrganizationName = account.OrganizationName,
                TenantCSS = account.TenantThemeCSS,
                TenantLogo = account.TenantLogo,
                TimeZone = account.TimeZone,
                Description = account.Description,
                ContactEmail = account.ContactEmail,
                ContactPerson = account.ContactPerson,
                CurrencyId = account.CurrencyId,
                Language = account.Language,
                Locale = account.Locale,
                Region = account.Region,
                AccountUrl = account.AccountUrl
            };

            // Get All Module Data

            if (moduleResponse == null || !moduleResponse.IsSuccessStatusCode)
            {
                response.Message = ResponseMessage.SomethingWentWrong;
                response.ResponseCode = ResponseCode.SomethingWentWrong;
                response.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.SomethingWentWrong
                };

            }
            if (moduleResponse != null && moduleResponse.Content != null && response != null && response.Data != null && response.Data.UserRights != null && response.Data.UserRights.Count > 0)
            {
                var modules = JsonConvert.DeserializeObject<List<Sample.Admin.Model.Module>>(moduleResponse.Content.ReadAsStringAsync().Result);
                if (modules != null && modules.Count > 0)
                {
                    var userRights = response.Data.UserRights.Join(modules, x => x.ModuleId, y => y.ModuleId, (x, y) => new UserRight
                    {
                        AccountId = x.AccountId,
                        ModuleId = x.ModuleId,
                        IsPermission = x.IsPermission,
                        UserRightId = x.UserRightId,

                        IsNavigationItems = y.IsNavigationItem,
                        ModulesDisplayName = y.DisplayName,
                        ModulesName = y.Name,
                        IsVisible = y.IsVisible
                    }).ToList();
                    response.Data.UserRights = userRights;
                }
                else
                {
                    response.Message = ResponseMessage.NoRecordFound;
                    response.ResponseCode = ResponseCode.NoRecordFound;
                    response.Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.NoRecordFound
                    };
                }
            }
            response.Data.Account = userAccount;
            return response;
        }

        /// <summary>
        /// To Authenticate External User
        /// </summary>
        /// <param name="model">object of login parameter which is required to login</param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<ResponseResult> AuthenticateExternalUser(ExternalLoginVM model, long accountId)
        {
            if(model == null || model.AuthType == null)
            {
                return new ResponseResult()
                {
                    ResponseCode = ResponseCode.ValidationFailed,
                    Message = ResponseMessage.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed,
                    }
                };
            }

            var userInfo = new ExternalUserVM()
            {
                AccountId = accountId,
                AuthType = model.AuthType,
                //MobileNo = model.MobileNo,
                //MobileCode = model.MobileCode
            };

            if (model.AuthType == ExternalAuthTypes.Google)
            {
                //var googlePayload = await VerifyGoogleToken(externalAuth);

                //if (googlePayload == null)
                //{
                //    return new ResponseResult<User>()
                //    {
                //        ResponseCode = ResponseCode.Unauthorized,
                //        Message = ResponseMessage.Unauthorized,
                //        Error = new ErrorResponseResult()
                //        {
                //            Message = ResponseMessage.Unauthorized,
                //        }
                //    };
                //}

                userInfo.UserName = model.Email;
                userInfo.Email = model.Email;
                userInfo.FirstName = model.FirstName;
                userInfo.LastName = model.LastName;
                userInfo.AuthenticationCategory = "G";
                userInfo.ExternalUserId = model.UserId;
            }
            else if (model.AuthType == ExternalAuthTypes.Facebook)
            {
                var facebookUser = await VerifyFacebookToken(model);

                if (facebookUser == null)
                {
                    return new ResponseResult()
                    {
                        ResponseCode = ResponseCode.Unauthorized,
                        Message = ResponseMessage.Unauthorized,
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.Unauthorized,
                        }
                    };
                }

                if (string.IsNullOrWhiteSpace(facebookUser.Email)) 
                {
                    if (!string.IsNullOrWhiteSpace(model.Email))
                    {
                        facebookUser.Email = model.Email;
                    }
                    else
                    {
                        return new ResponseResult()
                        {
                            ResponseCode = ResponseCode.Unauthorized,
                            Message = ResponseMessage.Unauthorized,
                            Error = new ErrorResponseResult()
                            {
                                Message = ResponseMessage.Unauthorized,
                            }
                        };
                    }
                }

                userInfo.UserName = facebookUser.Email;
                userInfo.Email = facebookUser.Email;
                userInfo.FirstName = facebookUser.First_name;
                userInfo.LastName = facebookUser.Last_name;
                userInfo.AuthenticationCategory = "F";
                userInfo.ExternalUserId = !string.IsNullOrWhiteSpace(facebookUser.Id) ? facebookUser.Id : model.UserId;
            }
            else
            {
                return new ResponseResult()
                {
                    ResponseCode = ResponseCode.ValidationFailed,
                    Message = ResponseMessage.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed,
                    }
                };
            }

            var postContent = new StringContent(JsonConvert.SerializeObject(userInfo), System.Text.Encoding.UTF8, "application/json");
            var httpResponseAccountTask = this.httpClient.GetAsync(this.urls.AdminAPI + AdminAPIOperations.GetAccount(accountId));
            var httpResponseModuleTask = this.httpClient.GetAsync(this.urls.AdminAPI + AdminAPIOperations.GetAllModules(false, accountId, (int)Common.Enum.Services.UserManagement));
            var httpResponseIAM = await this.httpClient.PostAsync(this.urls.CustomerAPI + CustomerAPIOperations.AuthenticateExternal(), postContent);

            var response = httpResponseIAM.GetHttpResponse<ResponseResult>();
            if (!response.GetResponse || response.Value == null)
            {
                return new ResponseResult()
                {
                    Message = ResponseMessage.InternalServerError,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError,
                    }
                };
            }

            if(response.Value.ResponseCode != ResponseCode.ValidLogin)
            {
                if(response.Value.ResponseCode == ResponseCode.NoUser)
                {
                    response.Value.Data = userInfo;
                }
                return response.Value;
            }

            if (response.Value.Data == null)
            {
                return new ResponseResult()
                {
                    Message = ResponseMessage.InternalServerError,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError,
                    }
                };
            }

            var httpResponseAccount = await httpResponseAccountTask;
            var accountResponseResult = httpResponseAccount.GetHttpResponse<AccountModel>();
            if (!accountResponseResult.GetResponse || accountResponseResult.Value == null)
            {
                return new ResponseResult()
                {
                    Message = ResponseMessage.InternalServerError,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError,
                    }
                };
            }

            var account = accountResponseResult.Value;
            var userAccount = new UserAccount()
            {
                AccountId = account.AccountId,
                AccountGUID = account.AccountGuid,
                OrganizationName = account.OrganizationName,
                TenantCSS = account.TenantThemeCSS,
                TenantLogo = account.TenantLogo,
                TimeZone = account.TimeZone,
                Description = account.Description,
                ContactEmail = account.ContactEmail,
                ContactPerson = account.ContactPerson,
                CurrencyId = account.CurrencyId,
                Language = account.Language,
                Locale = account.Locale,
                Region = account.Region,
                AccountUrl = account.AccountUrl
            };

            // Get All Module Data
            var moduleResponse = await httpResponseModuleTask;
            var modulesResponseResult = moduleResponse.GetHttpResponse<List<Admin.Model.Module>>();

            if (modulesResponseResult.GetResponse && modulesResponseResult.Value != null && modulesResponseResult.Value.Count > 0
               && response.Value.Data.userRights != null && response.Value.Data.userRights.Count > 0)
            {
                var modules = modulesResponseResult.Value;
                string jsonData = JsonConvert.SerializeObject(response.Value.Data);
                var userData = JsonConvert.DeserializeObject<User>(jsonData);
                var userRights = userData.UserRights.Join(modules, x => x.ModuleId, y => y.ModuleId, (x, y) => new UserRight
                {
                    AccountId = x.AccountId,
                    ModuleId = x.ModuleId,
                    IsPermission = x.IsPermission,
                    UserRightId = x.UserRightId,

                    IsNavigationItems = y.IsNavigationItem,
                    ModulesDisplayName = y.DisplayName,
                    ModulesName = y.Name,
                    IsVisible = y.IsVisible
                }).ToList();
                userData.UserRights = userRights;
                userData.Account = userAccount;
                 
                return new ResponseResult()
                {
                    Message = response.Value.Message,
                    ResponseCode = response.Value.ResponseCode,
                    Error = response.Value.Error,
                    Data = userData
                };
            }
            else
            {
                response.Value.Message = "User does not have any rights.";
                response.Value.ResponseCode = ResponseCode.NoRecordFound;
                response.Value.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.NoRecordFound
                };
                return response.Value;
            }
        }


        /// <summary>
        /// Verify Google Token
        /// </summary>
        /// <param name="externalAuth"></param>
        /// <returns></returns>
        private async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(ExternalLoginVM externalAuth)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>() {this.googleConfig.ClientId}
            };
            var payload = await GoogleJsonWebSignature.ValidateAsync(externalAuth.IdToken, settings);
            return payload;
        }
        
        /// <summary>
        /// Verify Google Token
        /// </summary>
        /// <param name="externalAuth"></param>
        /// <returns></returns>
        private async Task<FacebookUser> VerifyFacebookToken(ExternalLoginVM externalAuth)
        {
            //var accessUrl = string.Format(this.facebookConfig.AccessTokenURL, this.facebookConfig.ClientId, this.facebookConfig.ClientSecret);
            //var httpResponseAccess = await this.httpClient.GetAsync(accessUrl);
            //var resultAccess = httpResponseAccess.GetHttpResponse<dynamic>();
            //if (resultAccess == null)
            //{
            //    return null;
            //}

            string userTokenUrl = string.Format(this.facebookConfig.UserTokenURL, externalAuth.IdToken);
            var httpResponseUser = await this.httpClient.GetAsync(userTokenUrl);
            var responseResult = httpResponseUser.GetHttpResponse<FacebookUser>();

            if(!responseResult.GetResponse || responseResult.Value == null)
            {
                return null;
            }

            return responseResult.Value;
        }
    }
}
