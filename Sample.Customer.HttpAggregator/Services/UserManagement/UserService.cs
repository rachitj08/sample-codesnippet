using Common.Model;
using Core.API.ExtensionMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
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
using Sample.Customer.HttpAggregator.IServices.UserManagement;
using Sample.Customer.Model;
using Sample.Customer.Model.Model.StorageModel;

namespace Sample.Customer.HttpAggregator.Services.UserManagement
{
    /// <summary>
    /// Users Service
    /// </summary>
    public class UserService : IUserService
    {

        private readonly HttpClient httpClient;


        private readonly ILogger<UserService> logger;


        private readonly BaseUrlsConfig urls;

        /// <summary>
        /// Users Service constructor to inject required dependency
        /// </summary>
        /// <param name="httpClient">The httpClient</param>
        /// <param name="logger">The logger service</param>
        /// <param name="config">The Base Urls config</param>
        public UserService(HttpClient httpClient,
            ILogger<UserService> logger,
            IOptions<BaseUrlsConfig> config)
        {
            Check.Argument.IsNotNull(nameof(httpClient), httpClient);
            Check.Argument.IsNotNull(nameof(logger), logger);
            Check.Argument.IsNotNull(nameof(config), config);
            this.httpClient = httpClient;
            this.logger = logger;
            this.urls = config.Value;
        }

        #region [API Methods]

        /// <summary>
        /// To Register new user
        /// </summary>
        /// <param name="user">The new user object</param>
        /// <returns></returns>
        public async Task<ResponseResult<UserVM>> RegisterUser(CreateUserModel user)
        {
            var postContent = new StringContent(JsonConvert.SerializeObject(user), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + CustomerAPIOperations.RegisterUser(), postContent);
            return httpResponse.GetResponseResult<UserVM>();
        }

        /// <summary>
        /// Get User Modules
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accountId"></param>
        /// <param name="ordering"></param>
        /// <param name="offset"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        public async Task<ResponseResultList<UserModuleModel>> GetUserModules(long userId, long accountId, string ordering, int offset, int pageSize, int pageNumber, bool all)
        {
            var responseResult = new ResponseResultList<UserModuleModel>();
            var httpResponseModule = await this.httpClient.GetAsync(this.urls.AdminAPI + AdminAPIOperations.GetAllModules(false, accountId, (int)Common.Enum.Services.UserManagement));


            if (httpResponseModule == null || !httpResponseModule.IsSuccessStatusCode)
            {
                responseResult.Message = ResponseMessage.SomethingWentWrong;
                responseResult.ResponseCode = ResponseCode.SomethingWentWrong;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.SomethingWentWrong
                };
                return responseResult;
            }

            var modules = JsonConvert.DeserializeObject<List<Sample.Admin.Model.Module>>(httpResponseModule.Content.ReadAsStringAsync().Result);
            if (modules == null || modules.Count < 1)
            {
                responseResult.Message = ResponseMessage.NoRecordFound;
                responseResult.ResponseCode = ResponseCode.NoRecordFound;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.NoRecordFound
                };
                return responseResult;
            }

            // Build Response
            var userModules = modules.Select(
                x => new UserModuleModel()
                {
                    ModuleId = x.ModuleId,
                    ModuleDescription = x.Description,
                    ModuleDisplayName = x.DisplayName,
                    ModuleName = x.Name,
                    ModuleUrl = x.Url,
                    DisplayOrder = (x.DisplayOrder != null ? Convert.ToInt32(x.DisplayOrder) : 0),
                    IsNavigationItem = x.IsNavigationItem,
                    IsVisible = x.IsVisible,
                    ParentModuleId = x.ParentModuleId,
                }).ToList();

            // Create User Navigation Hierarchy
            var userModulesLookup = userModules.ToLookup(c => c.ParentModuleId);
            foreach (var nav in userModules.ToArray())
            {
                nav.SubModules = userModulesLookup[nav.ModuleId].OrderBy(x => x.DisplayOrder).ToList();

                if (nav.ParentModuleId != null && nav.ParentModuleId != 0)
                {
                    userModules.Remove(nav);
                }
            }

            responseResult.Data = userModules.OrderBy(x => x.DisplayOrder).ToList();
            responseResult.Message = ResponseMessage.RecordFetched;
            responseResult.ResponseCode = ResponseCode.RecordFetched;
            return responseResult;
        }

        /// <summary>
        /// Check User Permission
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accountId"></param>
        /// <param name="moduleName"></param>
        /// <returns></returns>
        public async Task<bool> CheckUserPermission(long userId, long accountId, string moduleName)
        {
            var httpResponseModule = await this.httpClient.GetAsync(this.urls.AdminAPI + AdminAPIOperations.GetModuleByName(accountId, (int)Common.Enum.Services.UserManagement, moduleName));
            if (httpResponseModule == null || !httpResponseModule.IsSuccessStatusCode) return false;

            // 
            var moduleId = JsonConvert.DeserializeObject<long>(httpResponseModule.Content.ReadAsStringAsync().Result);
            if (moduleId < 1) return false;

            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.CheckModulePermissionByUserId(userId, moduleId));

            if (httpResponse == null || !httpResponse.IsSuccessStatusCode) return false;

            return JsonConvert.DeserializeObject<bool>(httpResponse.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// Get User Modules by their unique id
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns></returns>
        public async Task<List<UserModule>> GetUserModulesByUserId(long userId)
        {
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetUserModulesByUserId(userId));
            if (!httpResponse.IsSuccessStatusCode)
            {
                return null;
            }
            var userModules = (httpResponse != null) ? JsonConvert.DeserializeObject<List<UserModule>>(httpResponse.Content.ReadAsStringAsync().Result) : null;
            if (userModules == null && userModules.Count == 0)
            {
                return null;
            }
            return userModules;
        }

        /// <summary>
        /// Get User Group by their unique id
        /// </summary>
        /// <param name="userId">user identifier</param>
        /// <returns></returns>
        public async Task<List<UserGroup>> GetUserGroupsByUserId(long userId)
        {
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetUserGroupsByUserId(userId));
            if (!httpResponse.IsSuccessStatusCode)
            {
                return null;
            }

            var userGroups = (httpResponse != null) ? JsonConvert.DeserializeObject<List<UserGroup>>(httpResponse.Content.ReadAsStringAsync().Result) : null;
            if (userGroups == null && userGroups.Count == 0)
            {
                return null;
            }
            return userGroups;

        }

        /// <summary>
        /// Get User Permissions By UserId
        /// </summary>
        /// <param name="userId">The userId is int</param>
        /// <returns></returns>
        public async Task<List<UserGroup>> GetUserPermissionsByUserId(long userId)
        {
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetUserPermissionsByUserId(userId));
            if (!httpResponse.IsSuccessStatusCode)
            {
                return null;
            }
            var userGroups = (httpResponse != null) ? JsonConvert.DeserializeObject<List<UserGroup>>(httpResponse.Content.ReadAsStringAsync().Result) : null;
            if (userGroups == null && userGroups.Count == 0)
            {
                return null;
            }
            return userGroups;

        }


        /// <summary>
        /// Build User Navigation By UserId
        /// </summary>
        /// <param name="userId">The userId is int</param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<List<UserNavigation>>> BuildUserNavigationByUserId(long userId, long accountId)
        {
            var responseResult = new ResponseResult<List<UserNavigation>>();
            var httpResponseModuleTask = this.httpClient.GetAsync(this.urls.AdminAPI + AdminAPIOperations.GetModulesByAccountId(accountId));
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.BuildUserNavigationByUserId(userId));

            if (httpResponse == null || !httpResponse.IsSuccessStatusCode)
            {
                responseResult.Message = ResponseMessage.SomethingWentWrong;
                responseResult.ResponseCode = ResponseCode.SomethingWentWrong;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.SomethingWentWrong
                };
                return responseResult;
            }

            var response = JsonConvert.DeserializeObject<ResponseResult<List<long>>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null || response.Data == null || response.Data.Count < 1)
            {
                responseResult.Message = ResponseMessage.NoRecordFound;
                responseResult.ResponseCode = ResponseCode.NoRecordFound;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.NoRecordFound
                };
                return responseResult;
            }
            var finalModuleIds = response.Data;

            // Get Modules
            var httpResponseModule = await httpResponseModuleTask;
            if (httpResponseModule == null || !httpResponseModule.IsSuccessStatusCode)
            {
                responseResult.Message = ResponseMessage.SomethingWentWrong;
                responseResult.ResponseCode = ResponseCode.SomethingWentWrong;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.SomethingWentWrong
                };
                return responseResult;
            }

            var modules = JsonConvert.DeserializeObject<List<Sample.Admin.Model.Module>>(httpResponseModule.Content.ReadAsStringAsync().Result);
            if (modules == null || modules.Count < 1)
            {
                responseResult.Message = ResponseMessage.NoRecordFound;
                responseResult.ResponseCode = ResponseCode.NoRecordFound;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.NoRecordFound
                };
                return responseResult;
            }

            // Build Response
            var userNavigations = modules.Join(finalModuleIds, x => x.ModuleId, y => y, (x, y) => x).Select(
                x => new UserNavigation()
                {
                    ModuleId = x.ModuleId,
                    ModuleDescription = x.Description,
                    ModuleDisplayName = x.DisplayName,
                    ModuleName = x.Name,
                    ModuleUrl = x.Url,
                    DisplayOrder = (x.DisplayOrder != null ? Convert.ToInt32(x.DisplayOrder) : 0),
                    IsNavigationItem = x.IsNavigationItem,
                    IsVisible = (x.Name == "Agreements" || x.Name == "My Wallet" || x.Name == "Listed Cars" || x.Name ==
"Vehicle Preferences" || x.Name == "Ratings & Reviews" || x.Name == "Notification") ? false : x.IsVisible,
                    ParentModuleId = x.ParentModuleId,
                    DataSourceId = x.DataSourceId
                }).ToList();

            // Create User Navigation Hierarchy
            var userNavigationLookup = userNavigations.ToLookup(c => c.ParentModuleId);
            foreach (var nav in userNavigations.ToArray())
            {
                nav.Navigations = userNavigationLookup[nav.ModuleId].OrderBy(x => x.DisplayOrder).ToList();

                if (nav.ParentModuleId != null && nav.ParentModuleId != 0)
                {
                    userNavigations.Remove(nav);
                }
            }

            responseResult.Data = userNavigations.OrderBy(x => x.DisplayOrder).ToList();
            responseResult.Message = response.Message;
            responseResult.ResponseCode = response.ResponseCode;
            return responseResult;
        }


        /// <summary>
        /// Get Mfa Types by their unique id
        /// </summary>
        /// <param name="userId">user identifier</param>
        /// <returns></returns>
        public async Task<long> GetMfaTypesByUserId(long userId)
        {
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetMfaTypesByUserId(userId));
            if (!httpResponse.IsSuccessStatusCode)
            {
                return 0;
            }
            var userMFATypes = (httpResponse != null) ? JsonConvert.DeserializeObject<long>(httpResponse.Content.ReadAsStringAsync().Result) : 0;
            if (userMFATypes == 0)
            {
                return 0;
            }
            return userMFATypes;
        }


        /// <summary>
        /// Change password of User
        /// </summary>
        /// <param name="model">Change Password Model</param>
        /// <param name="accountId">Account Id</param>
        /// <param name="userId">User Id</param>
        /// <returns>return response result</returns>
        public async Task<ResponseResult<string>> ChangePassword(ChangePasswordRequest model, long accountId, long userId)
        {
            var responseResult = new ResponseResult<string>();

            if (model == null)
            {
                responseResult.Message = "Request is not valid";
                responseResult.ResponseCode = ResponseCode.ValidationFailed;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = "Request is not valid"
                };
                return responseResult;
            }

            if (userId < 1)
            {
                responseResult.Message = "User Id is not valid";
                responseResult.ResponseCode = ResponseCode.ValidationFailed;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = "User Id is not valid"
                };
                return responseResult;
            }

            if (accountId < 1)
            {
                responseResult.Message = "Account Id is not valid";
                responseResult.ResponseCode = ResponseCode.ValidationFailed;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = "Account Id is not valid"
                };
                return responseResult;
            }

            var errorDetails = new Dictionary<string, string[]>();

            // Validate Model
            if (string.IsNullOrWhiteSpace(model.OldPassword))
            {
                errorDetails.Add("oldPassword", new string[] { "This field may not be blank." });
            }

            if (string.IsNullOrWhiteSpace(model.Password))
            {
                errorDetails.Add("password", new string[] { "This field may not be blank." });
            }

            if (errorDetails.Count > 0)
            {
                responseResult.Message = ResponseMessage.ValidationFailed;
                responseResult.ResponseCode = ResponseCode.ValidationFailed;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.ValidationFailed,
                    Detail = errorDetails
                };
                return responseResult;
            }

            var payload = new ChangePassword()
            {
                AccountId = accountId,
                UserId = userId,
                OldPassword = model.OldPassword,
                Password = model.Password
            };
            var postContent = new StringContent(JsonConvert.SerializeObject(payload), System.Text.Encoding.UTF8, "application/json");

            var httpResponse = await this.httpClient.PutAsync(this.urls.CustomerAPI + CustomerAPIOperations.ChangeUserPassword(), postContent);
            if (httpResponse == null || !httpResponse.IsSuccessStatusCode)
            {
                responseResult.Message = ResponseMessage.SomethingWentWrong;
                responseResult.ResponseCode = ResponseCode.SomethingWentWrong;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.SomethingWentWrong
                };
                return responseResult;
            }
            var response = JsonConvert.DeserializeObject<ResponseResult<string>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null || string.IsNullOrEmpty(response.Data))
            {
                responseResult.Message = ResponseMessage.NoRecordFound;
                responseResult.ResponseCode = ResponseCode.NoRecordFound;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.SomethingWentWrong
                };
                return responseResult;
            }
            responseResult.Data = response.Data;
            responseResult.Message = response.Message;
            responseResult.ResponseCode = response.ResponseCode;
            responseResult.Error = response.Error;
            return responseResult;
        }

        #endregion

        /// <summary>
        /// User Service to get user list
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResultList<UserVM>> GetAllUsers(HttpContext httpContext, string ordering, int offset, int pageSize, int pageNumber, bool all)
        {
            var responseResult = new ResponseResultList<UserVM>();
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetAllUsers(ordering, offset, pageSize, pageNumber, all));
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

            var response = JsonConvert.DeserializeObject<ResponseResultList<UserVM>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                responseResult.Message = ResponseMessage.NoRecordFound;
                responseResult.ResponseCode = ResponseCode.NoRecordFound;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.NoRecordFound
                };
                return responseResult;
            }

            var basePath = @$"{httpContext.Request.Scheme}://{httpContext.Request.Host}";
            if (!string.IsNullOrEmpty(response.Next)) response.Next = basePath + response.Next;
            if (!string.IsNullOrEmpty(response.Previous)) response.Previous = basePath + response.Previous;
            response.Message = ResponseMessage.RecordFetched;
            response.ResponseCode = ResponseCode.RecordFetched;
            return response;
        }


        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseResult<UserVM>> CreateUser(CreateUserModel model)
        {
            ResponseResult<UserVM> responseResult = new ResponseResult<UserVM>();
            var postContent = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + CustomerAPIOperations.CreateUser(), postContent);
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
            var response = JsonConvert.DeserializeObject<ResponseResult<UserVM>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                return new ResponseResult<UserVM>()
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
        /// To update existing User
        /// </summary>
        /// <param name="model">user object</param>
        /// <param name="userId">module unique Id</param>
        /// <returns></returns>
        public async Task<ResponseResult<UserVM>> UpdateUser(long userId, UsersModel model)
        {
            ResponseResult<UserVM> responseResult = new ResponseResult<UserVM>();
            var postContent = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PutAsync(this.urls.CustomerAPI + CustomerAPIOperations.UpdateUser(userId), postContent);
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
            var response = JsonConvert.DeserializeObject<ResponseResult<UserVM>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                return new ResponseResult<UserVM>()
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
        /// Get User By Id
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult<UserVM>> GetUserById(long userId)
        {
            var responseResult = new ResponseResult<UserVM>();
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetUserById(userId));
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

            var userDetail = JsonConvert.DeserializeObject<UserVM>(httpResponse.Content.ReadAsStringAsync().Result);
            if (userDetail == null)
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
            responseResult.Data = userDetail;
            return responseResult;
        }

        /// <summary>
        /// To update existing User
        /// </summary>
        /// <param name="model">user object</param>
        /// <param name="userId">user unique Id</param>
        /// <returns></returns>
        public async Task<ResponseResult<UserVM>> UpdatePartialUser(long userId, UsersModel model)
        {
            ResponseResult<UserVM> responseResult = new ResponseResult<UserVM>();
            var postContent = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PatchAsync(this.urls.CustomerAPI + CustomerAPIOperations.UpdatePartialUser(userId), postContent);
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
            var response = JsonConvert.DeserializeObject<ResponseResult<UserVM>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                return new ResponseResult<UserVM>()
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
        /// To delete existing User
        /// </summary>
        /// <param name="userId">user identifier</param>
        /// <returns></returns>
        public async Task<ResponseResult> DeleteUser(long userId)
        {
            var responseResult = new ResponseResult();
            var httpResponse = await this.httpClient.DeleteAsync(this.urls.CustomerAPI + CustomerAPIOperations.DeleteUser(userId));
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

            var user = Convert.ToInt16(httpResponse.Content.ReadAsStringAsync().Result);
            if (user < 1)
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
        /// Refresh Token
        /// </summary>
        /// <param name="tokenRefresh">Token Refresh Model</param>
        /// <returns></returns>
        public async Task<ResponseResult<RefreshTokenResultModel>> RefreshToken(TokenRefreshModel tokenRefresh)
        {
            var responseResult = new ResponseResult<RefreshTokenResultModel>();

            if (tokenRefresh == null)
            {
                responseResult.Message = "Request is not valid";
                responseResult.ResponseCode = ResponseCode.ValidationFailed;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = "Request is not valid"
                };
                return responseResult;
            }

            if (string.IsNullOrEmpty(tokenRefresh.Refresh))
            {
                responseResult.Message = "Request is not valid";
                responseResult.ResponseCode = ResponseCode.ValidationFailed;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = "Request is not valid"
                };
                return responseResult;
            }


            var postContent = new StringContent(JsonConvert.SerializeObject(tokenRefresh), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + CustomerAPIOperations.TokenRefresh(), postContent);
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

            var response = JsonConvert.DeserializeObject<ResponseResult<RefreshTokenResultModel>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                responseResult.Message = ResponseCode.InternalServerError;
                responseResult.ResponseCode = ResponseCode.InternalServerError;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseCode.InternalServerError
                };
                return responseResult;
            }

            return response;
        }

        /// <summary>
        /// Logout Token
        /// </summary>
        /// <param name="tokenRefresh">Token Refresh Model</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<ResponseResult<SuccessMessageModel>> Logout(TokenRefreshModel tokenRefresh, string token)
        {
            var responseResult = new ResponseResult<SuccessMessageModel>();

            if (tokenRefresh == null)
            {
                responseResult.Message = "Request is not valid";
                responseResult.ResponseCode = ResponseCode.ValidationFailed;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = "Request is not valid"
                };
                return responseResult;
            }

            if (string.IsNullOrEmpty(tokenRefresh.Refresh))
            {
                responseResult.Message = "Request is not valid";
                responseResult.ResponseCode = ResponseCode.ValidationFailed;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = "Request is not valid"
                };
                return responseResult;

            }

            var tokenRequest = new TokenRequestModel()
            {
                RefreshToken = tokenRefresh.Refresh
            };

            if (!string.IsNullOrWhiteSpace(token))
            {
                tokenRequest.Token = token;
            }

            var postContent = new StringContent(JsonConvert.SerializeObject(tokenRequest), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + CustomerAPIOperations.Logout(), postContent);
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

            var response = JsonConvert.DeserializeObject<ResponseResult<SuccessMessageModel>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                responseResult.Message = ResponseCode.InternalServerError;
                responseResult.ResponseCode = ResponseCode.InternalServerError;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseCode.InternalServerError
                };
                return responseResult;
            }

            return response;
        }

        /// <summary>
        /// Forgot Password
        /// </summary>
        /// <param name="model">Forgot Password model</param>
        /// <param name="accountId">Account Id</param>
        /// <returns></returns>
        public async Task<ResponseResult<SuccessMessageModel>> ForgotPassword(global::Sample.Customer.Model.ForgotPasswordRequestModel model, long accountId)
        {
            var responseResult = new ResponseResult<SuccessMessageModel>();

            if (model == null || string.IsNullOrEmpty(model.Email))
            {
                responseResult.Message = "Request is not valid";
                responseResult.ResponseCode = ResponseCode.ValidationFailed;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = "Request is not valid"
                };
                return responseResult;
            }

            if (accountId < 1)
            {
                responseResult.Message = "Account Id is missing";
                responseResult.ResponseCode = ResponseCode.ValidationFailed;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = "Account Id is missing"
                };
                return responseResult;
            }

            var forgorModel = new ForgotPasswordModel()
            {
                AccountId = accountId,
                Email = model.Email
            };

            var postContent = new StringContent(JsonConvert.SerializeObject(forgorModel), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + CustomerAPIOperations.ForgotUserPassword(), postContent);
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

            var response = JsonConvert.DeserializeObject<ResponseResult<SuccessMessageModel>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                responseResult.Message = ResponseCode.InternalServerError;
                responseResult.ResponseCode = ResponseCode.InternalServerError;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseCode.InternalServerError
                };
                return responseResult;
            }

            return response;
        }

        /// <summary>
        /// Forgot Password
        /// </summary>
        /// <param name="token"></param>
        /// <param name="uid"></param>
        /// <param name="model">Forgot Password model</param>
        /// <returns></returns>
        public async Task<ResponseResult<SuccessMessageModel>> SetPassword(string token, string uid, SetForgotPasswordModel model)
        {
            var responseResult = new ResponseResult<SuccessMessageModel>();

            if (model == null)
            {
                responseResult.Message = "Request is not valid";
                responseResult.ResponseCode = ResponseCode.ValidationFailed;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = "Request is not valid"
                };
                return responseResult;
            }

            var finalSetPasswordModel = new SetPasswordModel()
            {
                Password = model.Password,
                Token = token,
                Uid = uid
            };
            var postContent = new StringContent(JsonConvert.SerializeObject(finalSetPasswordModel), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + CustomerAPIOperations.SetPassword(), postContent);
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

            var response = JsonConvert.DeserializeObject<ResponseResult<SuccessMessageModel>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                responseResult.Message = ResponseCode.InternalServerError;
                responseResult.ResponseCode = ResponseCode.InternalServerError;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseCode.InternalServerError
                };
                return responseResult;
            }

            return response;
        }
        /// <summary>
        /// To Verify Token
        /// </summary>
        /// <param name="tokenModel">Verify Token Model</param>
        /// <returns></returns>
        public async Task<ResponseResult> VerifyToken(VerifyTokenModel tokenModel)
        {
            ResponseResult responseResult = new ResponseResult();
            var postContent = new StringContent(JsonConvert.SerializeObject(tokenModel), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + CustomerAPIOperations.VerifyToken(), postContent);
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

            return response;
        }

        /// <summary>
        /// VerifyUserEmailAddress
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResponseResult> VerifyUserEmailAddress(long userId)
        {
            ResponseResult objResponseResult = null;
            var httpResponseModule = await this.httpClient.PutAsync(this.urls.CustomerAPI + CustomerAPIOperations.VerifyUserEmailAddress(userId), null);
            objResponseResult = JsonConvert.DeserializeObject<ResponseResult>(httpResponseModule.Content.ReadAsStringAsync().Result);
            return objResponseResult;

        }

        /// <summary>
        /// VerifyUserMobileNumber
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResponseResult> VerifyUserMobileNumber(long userId)
        {
            ResponseResult objResponseResult = null;
            var httpResponseModule = await this.httpClient.PutAsync(this.urls.CustomerAPI + CustomerAPIOperations.VerifyUserMobileNumber(userId), null);
            objResponseResult = JsonConvert.DeserializeObject<ResponseResult>(httpResponseModule.Content.ReadAsStringAsync().Result);
            return objResponseResult;
        }

        /// <summary>
        /// Send Mobile OTP
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseResult<bool>> SendMobileOTP(SendMobileOtpVM model)
        {
            if (model == null)
            {
                return new ResponseResult<bool>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed
                    }
                };
            }

            var postContent = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");
            var httpResponseModule = await this.httpClient.PostAsync(this.urls.CustomerAPI + CustomerAPIOperations.SendMobileOTP(), postContent);
            return httpResponseModule.GetResponseResult<bool>();
        }

        /// <summary>
        /// Verify OTP
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseResult<User>> VerifyOTP(VerifyOtpVM model)
        {
            if (model == null)
            {
                return new ResponseResult<User>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed
                    }
                };
            }
            var postContent = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");
            var httpResponseModule = await this.httpClient.PostAsync(this.urls.CustomerAPI + CustomerAPIOperations.VerifyOTP(), postContent);
            return httpResponseModule.GetResponseResult<User>();
        }

        // TODO: Remove It After Initial Testing
        public void KafkaTest(CreateUserModel model)
        {
            var text = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            System.Diagnostics.Debug.WriteLine(text);
            //Console.WriteLine(text);
        }

        /// <summary>
        /// Get User Profile
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult<UserProfileModel>> GetUserProfile()
        {
            var httpResponseModule = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetUserProfile());
            return httpResponseModule.GetResponseResult<UserProfileModel>();
        }

        /// <summary>
        /// Update User Profile
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseResult<bool>> UpdateUserProfile(SaveUserProfileModel model)
        {
            if (model == null)
            {
                return new ResponseResult<bool>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed
                    }
                };
            }
            var postContent = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");
            var httpResponseModule = await this.httpClient.PostAsync(this.urls.CustomerAPI + CustomerAPIOperations.UpdateUserProfile(), postContent);
            return httpResponseModule.GetResponseResult<bool>();
        }


        /// <summary>
        /// Update User Profile Image
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns> 
        public async Task<ResponseResult<bool>> UpdateUserProfileImage(UserProfileImageVM model)
        {
            if (model == null)
            {
                return new ResponseResult<bool>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed
                    }
                };
            }

            var postContent = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");
            var httpResponseModule = await this.httpClient.PostAsync(this.urls.CustomerAPI + CustomerAPIOperations.UpdateUserProfileImage(), postContent);
            return httpResponseModule.GetResponseResult<bool>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseResult<SuccessMessageModel>> SendVerificationCode()
        {
            var postContent = new StringContent("", System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + CustomerAPIOperations.SendEmailVerification(), postContent);
            return httpResponse.GetResponseResult<SuccessMessageModel>();

        }


        /// <summary>
        /// Change password of User
        /// </summary>
        /// <param name="model">Change Password Model</param>
        /// <param name="accountId">Account Id</param>
        /// <param name="userId">User Id</param>
        /// <returns>return response result</returns>
        public async Task<ResponseResult<string>> ChangePasswordByMobileNo(ChangePasswordRequestForSMS model, long accountId, long userId)
        {
            var responseResult = new ResponseResult<string>();

            if (model == null)
            {
                responseResult.Message = "Request is not valid";
                responseResult.ResponseCode = ResponseCode.ValidationFailed;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = "Request is not valid"
                };
                return responseResult;
            }

           

            var errorDetails = new Dictionary<string, string[]>();
            
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                errorDetails.Add("password", new string[] { "This field may not be blank." });
            }

            if (errorDetails.Count > 0)
            {
                responseResult.Message = ResponseMessage.ValidationFailed;
                responseResult.ResponseCode = ResponseCode.ValidationFailed;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.ValidationFailed,
                    Detail = errorDetails
                };
                return responseResult;
            }

            var payload = new ChangePasswordMobile()
            {
                MobileNo=model.MobileNo,
                Password = model.Password
            };
            var postContent = new StringContent(JsonConvert.SerializeObject(payload), System.Text.Encoding.UTF8, "application/json");

            var httpResponse = await this.httpClient.PutAsync(this.urls.CustomerAPI + CustomerAPIOperations.ChangePasswordByMobileNo(), postContent);
            return httpResponse.GetResponseResult<string>();
        }
        
        /// <summary>
        /// Forget Password By SMS
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseResult<bool>> ForgetPasswordBySMS(SendMobileOtpVM model)
        {
            if (model == null)
            {
                return new ResponseResult<bool>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed
                    }
                };
            }
            var postContent = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");
            var httpResponseModule = await this.httpClient.PostAsync(this.urls.CustomerAPI + CustomerAPIOperations.ForgetPasswordBySMS(), postContent);
            return httpResponseModule.GetResponseResult<bool>();
        }

        public Task<bool> RegisterUserKafka(CreateUserModel user)
        {
            throw new NotImplementedException();
        }
    }
}
