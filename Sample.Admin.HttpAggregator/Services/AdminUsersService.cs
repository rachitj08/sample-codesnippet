using Sample.Admin.HttpAggregator.Config.OperationsConfig;
using Sample.Admin.HttpAggregator.Config.UrlsConfig;
using Sample.Admin.HttpAggregator.IServices;
using Common.Model;
using Sample.Admin.Model;
using Sample.Admin.Model.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Utilities;

namespace Sample.Admin.HttpAggregator.Services
{
    /// <summary>
    ///  Admin Users Service
    /// </summary>
    public class AdminUsersService : IAdminUsersService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<CurrencyService> logger;
        private readonly BaseUrlsConfig urls;

        /// <summary>
        /// Admin Users Service constructor to inject required dependency
        /// </summary>
        /// <param name="httpClient">The httpClient</param>
        /// <param name="logger">The logger service</param>
        /// <param name="config">The Base Urls config</param>
        public AdminUsersService(HttpClient httpClient, ILogger<CurrencyService> logger, IOptions<BaseUrlsConfig> config)
        {
            Check.Argument.IsNotNull(nameof(httpClient), httpClient);
            Check.Argument.IsNotNull(nameof(logger), logger);
            Check.Argument.IsNotNull(nameof(config), config);

            this.httpClient = httpClient;
            this.logger = logger;
            this.urls = config.Value;
        }

        /// <summary>
        /// To Get List of Admin Users
        /// </summary>
        /// <param name="httpContext">httpContext</param>
        /// <param name="ordering">Ordering Fields: (All Fields)</param>
        /// <param name="search">Search Fields: (UserId, UserName, Email)</param>
        /// <param name="offset">The initial index from which to return the results.</param>
        /// <param name="pageSize">Number of results to return per page</param>
        /// <param name="pageNumber">Page Number of results.</param>
        /// <param name="all">It will return results (all pages) without pagination.</param>
        /// <returns></returns>
        public async Task<ResponseResultList<AdminUsersModel>> GetAllAdminUsers(HttpContext httpContext, string ordering, string search, int pageSize, int pageNumber, int offset, bool all)
        {
            var responseResult = new ResponseResultList<AdminUsersModel>();
            var httpResponse = await this.httpClient.GetAsync(this.urls.AdminAPI + AdminOperations.GetAllAdminUsers(ordering, search, pageSize, pageNumber, offset, all));
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

            var detail = JsonConvert.DeserializeObject<ResponseResultList<AdminUsersModel>>(httpResponse.Content.ReadAsStringAsync().Result);
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
        /// Admin User Details
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <returns></returns>
        public async Task<ResponseResult<AdminUsersModel>> GetAdminUserDetail(int userId)
        {
            var responseResult = new ResponseResult<AdminUsersModel>();
            var httpResponse = await this.httpClient.GetAsync(this.urls.AdminAPI + AdminOperations.GetAdminUserDetails(userId));
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

            var detail = JsonConvert.DeserializeObject<AdminUsersModel>(httpResponse.Content.ReadAsStringAsync().Result);
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

            responseResult.Message = ResponseMessage.RecordFetched;
            responseResult.ResponseCode = ResponseCode.RecordFetched;
            responseResult.Data = detail;
            return responseResult;
        }

        /// <summary>
        /// To Create new  Admin User
        /// </summary>
        /// <param name="adminUser">admin User object</param>
        /// <returns></returns>
        public async Task<ResponseResult<UserCreationModel>> CreateAdminUser(UserCreationModel adminUser)
        {
            adminUser.AuthenticationCategory = "C";
            // Validate Model
            var isValidateDetail = UserValidate(adminUser);
            if (isValidateDetail.Count > 0)
            {
                return new ResponseResult<UserCreationModel>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Detail = isValidateDetail,
                        Message = ResponseMessage.ValidationFailed,
                    }
                };
            }

            var responseResult = new ResponseResult<UserCreationModel>();
            var postContent = new StringContent(JsonConvert.SerializeObject(adminUser), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.AdminAPI + AdminOperations.CreateAdminUser(), postContent);
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

            var response = JsonConvert.DeserializeObject<ResponseResult<UserCreationModel>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                return new ResponseResult<UserCreationModel>()
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
        /// To Update existing Module
        /// </summary>
        /// <param name="adminUser">admin user object</param>
        /// /// <param name="userId">Unique user Id</param>
        /// <returns></returns>
        public async Task<ResponseResult<UserCreationModel>> UpdateAdminUser(int userId, UserCreationModel adminUser)
        {
            adminUser.AuthenticationCategory = "C";
            // Validate Model
            var isValidateDetail = UserValidate(adminUser, false);
            if (isValidateDetail.Count > 0)
            {
                return new ResponseResult<UserCreationModel>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Detail = isValidateDetail,
                        Message = ResponseMessage.ValidationFailed,
                    }
                };
            }

            var responseResult = new ResponseResult<UserCreationModel>();
            var postContent = new StringContent(JsonConvert.SerializeObject(adminUser), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PutAsync(this.urls.AdminAPI + AdminOperations.UpdateAdminUser(userId), postContent);
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
            var response = JsonConvert.DeserializeObject<ResponseResult<UserCreationModel>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                return new ResponseResult<UserCreationModel>()
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
        /// To Update existing Admin User Partial
        /// </summary>
        /// <param name="adminUser">admin user object</param>
        /// /// <param name="userId">Unique user Id</param>
        /// <returns></returns>
        public async Task<ResponseResult<UserCreationModel>> UpdatePartialAdminUser(int userId, UserCreationModel adminUser)
        {
            adminUser.AuthenticationCategory = "C";
            // Validate Model
            var isValidateDetail = UserValidate(adminUser, false);
            if (isValidateDetail.Count > 0)
            {
                return new ResponseResult<UserCreationModel>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Detail = isValidateDetail,
                        Message = ResponseMessage.ValidationFailed,
                    }
                };
            }

            var responseResult = new ResponseResult<UserCreationModel>();
            var postContent = new StringContent(JsonConvert.SerializeObject(adminUser), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PatchAsync(this.urls.AdminAPI + AdminOperations.UpdatePartialAdminUser(userId), postContent);
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
            var response = JsonConvert.DeserializeObject<ResponseResult<UserCreationModel>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                return new ResponseResult<UserCreationModel>()
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
        /// To Delete existing Admin User
        /// </summary>
        /// <param name="userId">admin user identifier</param>
        /// <returns></returns>
        public async Task<ResponseResult<AdminUsersModel>> DeleteAdminUser(int userId)
        {
            var responseResult = new ResponseResult<AdminUsersModel>();
            var httpResponse = await this.httpClient.DeleteAsync(this.urls.AdminAPI + AdminOperations.DeleteAdminUser(userId));
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

            var module = Convert.ToInt16(httpResponse.Content.ReadAsStringAsync().Result);
            if (module < 1)
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
        /// Change password of Admin User
        /// </summary>
        /// <param name="model">Change Password Model</param>
        /// <param name="userId">User Id</param>
        /// <returns>return response result</returns>
        public async Task<ResponseResult<SuccessMessageModel>> ChangePassword(ChangePasswordRequestModel model,int userId)
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
            else if (model.Password.Length <= 6)
            {
                errorDetails.Add("password", new string[] { "Ensure this field has at least 6 characters." });
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

            var payload = new ChangePasswordModel()
            {
                UserId = userId,
                OldPassword = model.OldPassword,
                Password = model.Password
            };
            var postContent = new StringContent(JsonConvert.SerializeObject(payload), System.Text.Encoding.UTF8, "application/json");

            var httpResponse = await this.httpClient.PutAsync(this.urls.AdminAPI + AdminOperations.ChangeAdminUserPassword(), postContent);
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
            var response = JsonConvert.DeserializeObject<ResponseResult<SuccessMessageModel>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                responseResult.Message = ResponseMessage.NoRecordFound;
                responseResult.ResponseCode = ResponseCode.NoRecordFound;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.SomethingWentWrong
                };
                return responseResult;
            } 

            return response;
        }

        /// <summary>
        /// validate Admin User
        /// </summary>
        /// <param name="adminUser"></param>
        /// <param name="validatePassword"></param>
        /// <returns></returns>
        private Dictionary<string, string[]> UserValidate(UserCreationModel adminUser, bool validatePassword = true)
        {
            // Validate Model
            var errorDetails = new Dictionary<string, string[]>();
            if (string.IsNullOrWhiteSpace(adminUser.UserName))
            {
                errorDetails.Add("userName", new string[] { "This field may not be blank." });
            }
            if (string.IsNullOrWhiteSpace(adminUser.FirstName))
            {
                errorDetails.Add("firstName", new string[] { "This field may not be blank." });
            }
            else if (adminUser.FirstName.Length > 100)
            {
                errorDetails.Add("firstName", new string[] { "Ensure this field has at least 100 characters." });
            }
            if (string.IsNullOrWhiteSpace(adminUser.LastName))
            {
                errorDetails.Add("lastName", new string[] { "This field may not be blank." });
            }
            else if (adminUser.LastName.Length > 100)
            {
                errorDetails.Add("lastName", new string[] { "Ensure this field has at least 100 characters." });
            }

            if (validatePassword)
            {
                if (string.IsNullOrWhiteSpace(adminUser.Password))
                {
                    errorDetails.Add("password", new string[] { "This field may not be blank." });
                }
                else if (adminUser.Password.Length <= 6)
                {
                    errorDetails.Add("password", new string[] { "Ensure this field has at least 6 characters." });
                }
                else if (adminUser.Password.Length > 100)
                {
                    errorDetails.Add("userName", new string[] { "Ensure this field has at least 100 characters." });
                }
            }

            if (string.IsNullOrWhiteSpace(adminUser.EmailAddress))
            {
                errorDetails.Add("emailAddress", new string[] { "This field may not be blank." });
            }
            else if (adminUser.EmailAddress.Length > 100)
            {
                errorDetails.Add("emailAddress", new string[] { "Ensure this field has no more than 100 characters." });
            }

            if (string.IsNullOrWhiteSpace(adminUser.AuthenticationCategory))
            {
                errorDetails.Add("authenticationCategory", new string[] { "This field may not be blank." });
            }
            else if (adminUser.AuthenticationCategory.Length > 10)
            {
                errorDetails.Add("authenticationCategory", new string[] { "Ensure this field has at least 10 characters." });
            }
            
            return errorDetails;
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
            var httpResponse = await this.httpClient.PostAsync(this.urls.AdminAPI + AdminOperations.TokenRefresh(), postContent);
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
            var httpResponse = await this.httpClient.PostAsync(this.urls.AdminAPI + AdminOperations.Logout(), postContent);
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
        /// <returns></returns>
        public async Task<ResponseResult<SuccessMessageModel>> ForgotPassword(ForgotPasswordRequestModel model)
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

            var postContent = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.AdminAPI + AdminOperations.ForgotAdminUserPassword(), postContent);
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
            var httpResponse = await this.httpClient.PostAsync(this.urls.AdminAPI + AdminOperations.SetAdminUserPassword(), postContent);
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
            var httpResponse = await this.httpClient.PostAsync(this.urls.AdminAPI + AdminOperations.VerifyToken(), postContent);
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
    }
}
