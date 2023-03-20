using Common.Model;
using Core.API.ExtensionMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Utilities;
using Utility;
using Sample.Admin.HttpAggregator.Config.OperationsConfig;
using Sample.Admin.HttpAggregator.Config.UrlsConfig;
using Sample.Admin.HttpAggregator.IServices;

namespace Sample.Admin.HttpAggregator.Services
{
    /// <summary>
    /// Common Service
    /// </summary>
    public class CommonService : ICommonService
    {
        private readonly HttpClient httpClient;

        private readonly ILogger<CommonService> logger;

        private readonly ICommonHelper commonHelper;

        private readonly BaseUrlsConfig urls; 

        /// <summary>
        /// Common service constructor to inject required dependency
        /// </summary>
        /// <param name="httpClient">The httpClient</param>
        /// <param name="commonHelper">The Common Helper</param>
        /// <param name="logger">The logger service</param>
        /// <param name="config">The Base Urls config</param>
        public CommonService(HttpClient httpClient, ICommonHelper commonHelper,
            ILogger<CommonService> logger, IOptions<BaseUrlsConfig> config)
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
        /// Get All Data
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResultList<T>> GetAll<T>(HttpContext httpContext, string ordering, int offset, int pageSize, int pageNumber, bool all)
        {
            string controllerName = httpContext.Request.RouteValues["controller"].ToString();

            var httpResponse = await this.httpClient.GetAsync(this.urls.AdminAPI + AdminOperations.GetCommonAll(controllerName, ordering, offset, pageSize, pageNumber, all));

            var basePath = @$"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}?";
            var detail = httpResponse.GetResponseResultList<T>(basePath);
            return detail;
        }

        /// <summary>
        /// Get By Id
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult<T>> GetById<T>(HttpContext httpContext, long id)
        {
            string controllerName = httpContext.Request.RouteValues["controller"].ToString();
            var httpResponse = await this.httpClient.GetAsync(this.urls.AdminAPI + AdminOperations.GetCommonById(controllerName, id));
            return httpResponse.GetResponseResult<T>();
        }

        /// <summary>
        /// To Create New Record
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="model">model object</param>
        /// <returns></returns>
        public async Task<ResponseResult<T>> Create<T>(HttpContext httpContext, T model)
        {
            if (model == null)
            {
                return new ResponseResult<T>
                {
                    ResponseCode = ResponseCode.ValidationFailed,
                    Message = ResponseMessage.NoRecordFound,
                    Error = new ErrorResponseResult
                    {
                        Message = ResponseMessage.ValidationFailed
                    }
                };
            }

            string controllerName = httpContext.Request.RouteValues["controller"].ToString();
            var postContent = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.AdminAPI + AdminOperations.CreateCommon(controllerName), postContent);
            return httpResponse.GetResponseResult<T>();
        }


        /// <summary>
        /// To update existing Record
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="model">model object</param>
        /// <param name="id">unique Id</param>
        /// <returns></returns>
        public async Task<ResponseResult<T>> Update<T>(HttpContext httpContext, long id, T model)
        { 
            if (id < 1 || model == null)
            {
                return new ResponseResult<T>
                {
                    ResponseCode = ResponseCode.ValidationFailed,
                    Message = ResponseMessage.NoRecordFound,
                    Error = new ErrorResponseResult
                    {
                        Message = ResponseMessage.ValidationFailed
                    }
                };
            }

            string controllerName = httpContext.Request.RouteValues["controller"].ToString();
            var postContent = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PutAsync(this.urls.AdminAPI + AdminOperations.UpdateCommon(controllerName, id), postContent);
            return httpResponse.GetResponseResult<T>();
        }

        ///// <summary>
        ///// Update partial to existing record
        ///// </summary>
        ///// <param name="httpContext"></param>
        ///// <param name="model">model object</param>
        ///// <param name="id">unique Id</param>
        ///// <returns></returns>
        //public async Task<ResponseResult<T>> UpdatePartial<T>(HttpContext httpContext, long id, T model)
        //{ 
        //    string controllerName = httpContext.Request.RouteValues["controller"].ToString();
        //    var postContent = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");
        //    var httpResponse = await this.httpClient.PatchAsync(this.urls.AdminAPI + AdminOperations.UpdatePartialCommon(controllerName, id), postContent);
        //    return httpResponse.GetResponseResult<T>();
        //}

        /// <summary>
        /// To delete existing record
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="id">id identifier</param>
        /// <returns></returns>
        public async Task<ResponseResult<bool>> Delete(HttpContext httpContext, long id)
        {
            string controllerName = httpContext.Request.RouteValues["controller"].ToString();
            var httpResponse = await this.httpClient.DeleteAsync(this.urls.AdminAPI + AdminOperations.DeleteCommon(controllerName, id));
            return httpResponse.GetResponseResult<bool>();
        }
    }
}
