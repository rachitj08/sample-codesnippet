using Common.Model;
using Sample.Admin.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sample.Admin.HttpAggregator.Config.OperationsConfig;
using Sample.Admin.HttpAggregator.Config.UrlsConfig;
using Sample.Admin.HttpAggregator.IServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Utilities;
using Utility;
using Microsoft.AspNetCore.Http;

namespace Sample.Admin.HttpAggregator.Services
{
    ///<summary>
    /// Version Service Class
    ///</summary>
    public class VersionService : IVersionService
    {
        private readonly HttpClient httpClient;

        private readonly ILogger<VersionService> logger;

        private readonly ICommonHelper commonHelper;

        private readonly BaseUrlsConfig urls;

        /// <summary>
        /// Version service constructor to inject required dependency
        /// </summary>
        /// <param name="httpClient">The httpClient</param>
        /// <param name="commonHelper">The Common Helper</param>
        /// <param name="logger">The logger service</param>
        /// <param name="config">The Base Urls config</param>
        public VersionService(HttpClient httpClient,
            ICommonHelper commonHelper,
            ILogger<VersionService> logger,
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
        /// Version Service to get version list
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResultList<VersionsModel>> GetAllVersion(HttpContext httpContext, string ordering, string search, int offset, int pageSize, int pageNumber, bool all)
        {
            var responseResult = new ResponseResultList<VersionsModel>();
            var httpResponse = await this.httpClient.GetAsync(this.urls.AdminAPI + AdminOperations.GetAllVersions(ordering, search, offset, pageSize, pageNumber, all));
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

            var detail = JsonConvert.DeserializeObject<ResponseResultList<VersionsModel>>(httpResponse.Content.ReadAsStringAsync().Result);
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

            var basePath = @$"{httpContext.Request.Scheme}://{httpContext.Request.Host}";
            if (!string.IsNullOrEmpty(detail.Next)) detail.Next = basePath + detail.Next;
            if (!string.IsNullOrEmpty(detail.Previous)) detail.Previous = basePath + detail.Previous;
            detail.Message = ResponseMessage.RecordFetched;
            detail.ResponseCode = ResponseCode.RecordFetched;
            return detail;
        }

        /// <summary>
        /// Get Version By Id
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult<VersionModel>> GetVersionById(long versionId)
        {
            var responseResult = new ResponseResult<VersionModel>();
            var httpResponse = await this.httpClient.GetAsync(this.urls.AdminAPI + AdminOperations.GetVersionById(versionId));
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

            var version = JsonConvert.DeserializeObject<VersionModel>(httpResponse.Content.ReadAsStringAsync().Result);
            if (version == null)
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
            responseResult.Data = version;
            return responseResult;
        }

        /// <summary>
        /// To Create new  Version
        /// </summary>
        /// <param name="version">version object</param>
        /// <returns></returns>
        public async Task<ResponseResult<VersionsModel>> CreateVersion(VersionsModel version)
        {
            ResponseResult<VersionsModel> responseResult = new ResponseResult<VersionsModel>();
            var postContent = new StringContent(JsonConvert.SerializeObject(version), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.AdminAPI + AdminOperations.CreateVersion(), postContent);
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
            var response = JsonConvert.DeserializeObject<ResponseResult<VersionsModel>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                return new ResponseResult<VersionsModel>()
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
        /// To update existing Version
        /// </summary>
        /// <param name="version">version object</param>
        /// <param name="versionId">version unique Id</param>
        /// <returns></returns>
        public async Task<ResponseResult<VersionsModel>> UpdateVersion(long versionId, VersionsModel version)
        {
            ResponseResult<VersionsModel> responseResult = new ResponseResult<VersionsModel>();
            var postContent = new StringContent(JsonConvert.SerializeObject(version), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PutAsync(this.urls.AdminAPI + AdminOperations.UpdateVersion(versionId), postContent);
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
            var response = JsonConvert.DeserializeObject<ResponseResult<VersionsModel>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                return new ResponseResult<VersionsModel>()
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
        /// To update existing Version partially
        /// </summary>
        /// <param name="version">version object</param>
        /// <param name="versionId">version unique Id</param>
        /// <returns></returns>
        public async Task<ResponseResult<VersionsModel>> UpdatePartialVersion(long versionId, VersionsModel version)
        {
            ResponseResult<VersionsModel> responseResult = new ResponseResult<VersionsModel>();
            var postContent = new StringContent(JsonConvert.SerializeObject(version), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PatchAsync(this.urls.AdminAPI + AdminOperations.UpdatePartialVersion(versionId), postContent);
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
            var response = JsonConvert.DeserializeObject<ResponseResult<VersionsModel>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                return new ResponseResult<VersionsModel>()
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
        /// To delete existing Version
        /// </summary>
        /// <param name="versionId">version identifier</param>
        /// <returns></returns>
        public async Task<ResponseResult<VersionsModel>> DeleteVersion(long versionId)
        {
            var responseResult = new ResponseResult<VersionsModel>();
            var httpResponse = await this.httpClient.DeleteAsync(this.urls.AdminAPI + AdminOperations.DeleteVersion(versionId));
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

            var version = Convert.ToInt16(httpResponse.Content.ReadAsStringAsync().Result);
            if (version < 1)
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
