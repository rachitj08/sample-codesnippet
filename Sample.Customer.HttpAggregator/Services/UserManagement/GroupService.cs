using Common.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sample.Customer.HttpAggregator.Config.OperationsConfig;
using Sample.Customer.HttpAggregator.Config.UrlsConfig;
using Sample.Customer.HttpAggregator.IServices.UserManagement;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Sample.Customer.Model;
using Utilities;

namespace Sample.Customer.HttpAggregator.Services.UserManagement
{
    /// <summary>
    /// Group Service Class
    ///</summary>
    public class GroupService : IGroupService
    {
        private readonly HttpClient httpClient;

        private readonly ILogger<GroupService> logger;

        //private readonly ICommonHelper commonHelper;

        private readonly BaseUrlsConfig urls;
        private readonly BaseConfig baseConfig;


        /// <summary>
        /// Group service constructor to inject required dependency
        /// </summary>
        /// <param name="httpClient">The httpClient</param>
        /// <param name="logger">The logger service</param>
        /// <param name="config">The Base Urls config</param>
        /// <param name="baseConfig"></param>
        public GroupService(HttpClient httpClient,
            //ICommonHelper commonHelper,
            ILogger<GroupService> logger,
            IOptions<BaseUrlsConfig> config, IOptions<BaseConfig> baseConfig)
        {
            Check.Argument.IsNotNull(nameof(httpClient), httpClient);
            //Check.Argument.IsNotNull(nameof(commonHelper), commonHelper);
            Check.Argument.IsNotNull(nameof(logger), logger);
            Check.Argument.IsNotNull(nameof(config), config);
            Check.Argument.IsNotNull(nameof(baseConfig), baseConfig);
            //this.commonHelper = commonHelper;
            this.httpClient = httpClient;
            this.logger = logger;
            this.urls = config.Value;
            this.baseConfig = baseConfig.Value;
        }

        /// <summary>
        /// Group Service to get group list
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResultList<GroupsVM>> GetAllGroups(HttpContext httpContext, string ordering, int offset, int pageSize, int pageNumber, bool all)
        {
            var responseResult = new ResponseResultList<GroupsVM>();
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetAllGroups(ordering, offset, pageSize, pageNumber, all));
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

            var detail = JsonConvert.DeserializeObject<ResponseResultList<GroupsVM>>(httpResponse.Content.ReadAsStringAsync().Result);
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
        /// Get Group By Id
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult<GroupsVM>> GetGroupById(long groupId)
        {
            var responseResult = new ResponseResult<GroupsVM>();
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetGroupById(groupId));
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

            var group = JsonConvert.DeserializeObject<GroupsVM>(httpResponse.Content.ReadAsStringAsync().Result);
            if (group == null)
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
            responseResult.Data = group;
            return responseResult;
        }

        /// <summary>
        /// To Create new  Group
        /// </summary>
        /// <param name="group">group object</param>
        /// <returns></returns>
        public async Task<ResponseResult<GroupsVM>> CreateGroup(GroupsModel group)
        {
            ResponseResult<GroupsVM> responseResult = new ResponseResult<GroupsVM>();
            var postContent = new StringContent(JsonConvert.SerializeObject(group), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + CustomerAPIOperations.CreateGroup(), postContent);
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
            var response = JsonConvert.DeserializeObject<ResponseResult<GroupsVM>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                return new ResponseResult<GroupsVM>()
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
        /// To update existing Group
        /// </summary>
        /// <param name="group">group object</param>
        /// <param name="groupId">group unique Id</param>
        /// <returns></returns>
        public async Task<ResponseResult<GroupsVM>> UpdateGroup(long groupId, GroupsModel group)
        {
            ResponseResult<GroupsVM> responseResult = new ResponseResult<GroupsVM>();
            var postContent = new StringContent(JsonConvert.SerializeObject(group), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PutAsync(this.urls.CustomerAPI + CustomerAPIOperations.UpdateGroup(groupId), postContent);
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
            var response = JsonConvert.DeserializeObject<ResponseResult<GroupsVM>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                return new ResponseResult<GroupsVM>()
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
        /// To update existing Group 
        /// </summary>
        /// <param name="group">group object</param>
        /// <param name="groupId">group unique Id</param>
        /// <returns></returns>
        public async Task<ResponseResult<GroupsVM>> UpdatePartialGroup(long groupId, GroupsModel group)
        {
            ResponseResult<GroupsVM> responseResult = new ResponseResult<GroupsVM>();
            var postContent = new StringContent(JsonConvert.SerializeObject(group), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PatchAsync(this.urls.CustomerAPI + CustomerAPIOperations.UpdatePartialGroup(groupId), postContent);
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
            var response = JsonConvert.DeserializeObject<ResponseResult<GroupsVM>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                return new ResponseResult<GroupsVM>()
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
        /// To delete existing Group
        /// </summary>
        /// <param name="groupId">group identifier</param>
        /// <returns></returns>
        public async Task<ResponseResult<GroupsModel>> DeleteGroup(long groupId)
        {
            var responseResult = new ResponseResult<GroupsModel>();
            var httpResponse = await this.httpClient.DeleteAsync(this.urls.CustomerAPI + CustomerAPIOperations.DeleteGroup(groupId));
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

            var group = Convert.ToInt16(httpResponse.Content.ReadAsStringAsync().Result);
            if (group < 1)
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
