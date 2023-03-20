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
using System.Net.Http;
using System.Threading.Tasks;
using Utilities;
using Utility;

namespace Sample.Admin.HttpAggregator.Services
{
    ///<summary>
    /// Module Service Class
    ///</summary>
    public class VersionModuleService : IVersionModuleService
    {
        private readonly HttpClient httpClient;

        private readonly ILogger<VersionModuleService> logger;

        private readonly ICommonHelper commonHelper;

        private readonly BaseUrlsConfig urls;


        /// <summary>
        /// Module service constructor to inject required dependency
        /// </summary>
        /// <param name="httpClient">The httpClient</param>
        /// <param name="commonHelper">The Common Helper</param>
        /// <param name="logger">The logger service</param>
        /// <param name="config">The Base Urls config</param>
        public VersionModuleService(HttpClient httpClient,
            ICommonHelper commonHelper,
            ILogger<VersionModuleService> logger,
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
        /// Module Service to get versionmodule list
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResultList> GetAllVersionModules(string ordering, int offset, int pageSize, int pageNumber, bool all)
        {
            var responseResult = new ResponseResultList();
            var httpResponse = await this.httpClient.GetAsync(this.urls.AdminAPI + AdminOperations.GetAllVersionModules(ordering,offset,pageSize,pageNumber,all));
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

            var response = JsonConvert.DeserializeObject<ResponseResultList>(httpResponse.Content.ReadAsStringAsync().Result);
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
            else
            {
                var versionmodules = JsonConvert.DeserializeObject<List<VersionModulesModel>>(Convert.ToString(response.Data));
                response.Data = versionmodules;
                return response;
            }
        }

        /// <summary>
        /// Get Module By Id
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult<VersionModulesModel>> GetVersionModuleById(long versionModuleId)
        {
            var responseResult = new ResponseResult<VersionModulesModel>();
            var httpResponse = await this.httpClient.GetAsync(this.urls.AdminAPI + AdminOperations.GetVersionModuleById(versionModuleId));
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

            var versionmodule = JsonConvert.DeserializeObject<VersionModulesModel>(httpResponse.Content.ReadAsStringAsync().Result);
            if (versionmodule == null)
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
            responseResult.Data = versionmodule;
            return responseResult;
        }

        /// <summary>
        /// To Create new  Module
        /// </summary>
        /// <param name="versionmodule">versionmodule object</param>
        /// <returns></returns>
        public async Task<ResponseResult<VersionModulesModel>> CreateVersionModule(VersionModulesModel versionmodule)
        {
            ResponseResult<VersionModulesModel> responseResult = new ResponseResult<VersionModulesModel>();
            var postContent = new StringContent(JsonConvert.SerializeObject(versionmodule), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.AdminAPI + AdminOperations.CreateVersionModule(), postContent);
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
            var response = JsonConvert.DeserializeObject<VersionModulesModel>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                return new ResponseResult<VersionModulesModel>()
                {
                    Message = ResponseMessage.InternalServerError,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError
                    }
                };
            }
            if (response.VersionModuleId > 0)
            {
                responseResult.Message = ResponseMessage.RecordSaved;
                responseResult.ResponseCode = ResponseCode.RecordSaved;
                responseResult.Data = response;
            }
            else
            {
                return new ResponseResult<VersionModulesModel>()
                {
                    Message = ResponseMessage.InternalServerError,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError
                    }
                };
            }
            return responseResult;
        }

        /// <summary>
        /// To update existing VersionModule
        /// </summary>
        /// <param name="versionModuleId">versionmodule object</param>
        /// <param name="versionModule">versionmodule unique Id</param>
        /// <returns></returns>
        public async Task<ResponseResult<VersionModulesModel>> UpdateVersionModule(long versionModuleId, VersionModulesModel versionModule)
        {
            ResponseResult<VersionModulesModel> responseResult = new ResponseResult<VersionModulesModel>();
            var postContent = new StringContent(JsonConvert.SerializeObject(versionModule), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PutAsync(this.urls.AdminAPI + AdminOperations.UpdateVersionModule(versionModuleId), postContent);
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
            var response = JsonConvert.DeserializeObject<VersionModulesModel>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                return new ResponseResult<VersionModulesModel>()
                {
                    Message = ResponseMessage.InternalServerError,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError
                    }
                };
            }
            responseResult.Message = ResponseMessage.RecordSaved;
            responseResult.ResponseCode = ResponseCode.RecordSaved;
            responseResult.Data = response;
            return responseResult;
        }

        /// <summary>
        /// To update existing VersionModule
        /// </summary>
        /// <param name="versionModuleId">versionmodule object</param>
        /// <param name="versionModule">versionmodule unique Id</param>
        /// <returns></returns>
        public async Task<ResponseResult<VersionModulesModel>> UpdatePartialVersionModule(long versionModuleId, VersionModulesModel versionModule)
        {
            ResponseResult<VersionModulesModel> responseResult = new ResponseResult<VersionModulesModel>();
            var postContent = new StringContent(JsonConvert.SerializeObject(versionModule), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PatchAsync(this.urls.AdminAPI + AdminOperations.UpdatePartialVersionModule(versionModuleId), postContent);
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
            var response = JsonConvert.DeserializeObject<VersionModulesModel>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                return new ResponseResult<VersionModulesModel>()
                {
                    Message = ResponseMessage.InternalServerError,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError
                    }
                };
            }
            responseResult.Message = ResponseMessage.RecordSaved;
            responseResult.ResponseCode = ResponseCode.RecordSaved;
            responseResult.Data = response;
            return responseResult;
        }

        /// <summary>
        /// To delete existing Module
        /// </summary>
        /// <param name="versionModuleId">versionmodule identifier</param>
        /// <returns></returns>
        public async Task<ResponseResult<VersionModulesModel>> DeleteVersionModule(long versionModuleId)
        {
            var responseResult = new ResponseResult<VersionModulesModel>();
            var httpResponse = await this.httpClient.DeleteAsync(this.urls.AdminAPI + AdminOperations.DeleteVersionModule(versionModuleId));
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

            var versionmodule = Convert.ToInt16(httpResponse.Content.ReadAsStringAsync().Result);
            if (versionmodule < 1)
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
    }
}
