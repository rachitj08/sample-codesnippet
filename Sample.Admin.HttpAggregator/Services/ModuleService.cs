using Sample.Admin.HttpAggregator.Config.OperationsConfig;
using Sample.Admin.HttpAggregator.Config.UrlsConfig;
using Sample.Admin.HttpAggregator.IServices;
using Common.Model;
using Sample.Admin.Model;
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
using Utility;

namespace Sample.Admin.HttpAggregator.Services
{
    /// <summary>
    /// Module Service Class
    ///</summary>
    public class ModuleService : IModuleService
    {
        private readonly HttpClient httpClient;

        private readonly ILogger<ModuleService> logger;

        private readonly ICommonHelper commonHelper;

        private readonly BaseUrlsConfig urls;


        /// <summary>
        /// Module service constructor to inject required dependency
        /// </summary>
        /// <param name="httpClient">The httpClient</param>
        /// <param name="commonHelper">The Common Helper</param>
        /// <param name="logger">The logger service</param>
        /// <param name="config">The Base Urls config</param>
        public ModuleService(HttpClient httpClient,
            ICommonHelper commonHelper,
            ILogger<ModuleService> logger,
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
        /// Module Service to get module list
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResultList<ModulesModel>> GetAllModules(HttpContext httpContext,string ordering, string search, int offset, int pageSize, int pageNumber, string serviceName, bool all)
        {
            var responseResult = new ResponseResultList<ModulesModel>();
            var httpResponse = await this.httpClient.GetAsync(this.urls.AdminAPI + AdminOperations.GetAllModules(ordering,search,offset,pageSize,pageNumber, serviceName,all));
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

            var detail = JsonConvert.DeserializeObject<ResponseResultList<ModulesModel>>(httpResponse.Content.ReadAsStringAsync().Result);
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
        /// Get Module By Id
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult<ModulesModel>> GetModuleById(long moduleId)
        {
            var responseResult = new ResponseResult<ModulesModel>();
            var httpResponse = await this.httpClient.GetAsync(this.urls.AdminAPI + AdminOperations.GetModuleById(moduleId));
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

            var module = JsonConvert.DeserializeObject<ModulesModel>(httpResponse.Content.ReadAsStringAsync().Result);
            if (module == null)
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
            responseResult.Data = module;
            return responseResult;
        }

        /// <summary>
        /// To Create new  Module
        /// </summary>
        /// <param name="module">module object</param>
        /// <returns></returns>
        public async Task<ResponseResult<ModulesModel>> CreateModule(ModulesModel module)
        {
            ResponseResult<ModulesModel> responseResult = new ResponseResult<ModulesModel>();
            var postContent = new StringContent(JsonConvert.SerializeObject(module), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.AdminAPI + AdminOperations.CreateModule(), postContent);
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
            var response = JsonConvert.DeserializeObject<ResponseResult<ModulesModel>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                return new ResponseResult<ModulesModel>()
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
        /// To update existing Module
        /// </summary>
        /// <param name="module">module object</param>
        /// <param name="moduleId">module unique Id</param>
        /// <returns></returns>
        public async Task<ResponseResult<ModulesModel>> UpdateModule(long moduleId, ModulesModel module)
        {
            ResponseResult<ModulesModel> responseResult = new ResponseResult<ModulesModel>();
            var postContent = new StringContent(JsonConvert.SerializeObject(module), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PutAsync(this.urls.AdminAPI + AdminOperations.UpdateModule(moduleId), postContent);
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
            var response = JsonConvert.DeserializeObject<ResponseResult<ModulesModel>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                return new ResponseResult<ModulesModel>()
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
        /// To update existing Module 
        /// </summary>
        /// <param name="module">module object</param>
        /// <param name="moduleId">module unique Id</param>
        /// <returns></returns>
        public async Task<ResponseResult<ModulesModel>> UpdatePartialModule(long moduleId, ModulesModel module)
        {
            ResponseResult<ModulesModel> responseResult = new ResponseResult<ModulesModel>();
            var postContent = new StringContent(JsonConvert.SerializeObject(module), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PatchAsync(this.urls.AdminAPI + AdminOperations.UpdatePartialModule(moduleId), postContent);
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
            var response = JsonConvert.DeserializeObject<ResponseResult<ModulesModel>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                return new ResponseResult<ModulesModel>()
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
        /// To delete existing Module
        /// </summary>
        /// <param name="moduleId">module identifier</param>
        /// <returns></returns>
        public async Task<ResponseResult<ModulesModel>> DeleteModule(long moduleId)
        {
            var responseResult = new ResponseResult<ModulesModel>();
            var httpResponse = await this.httpClient.DeleteAsync(this.urls.AdminAPI + AdminOperations.DeleteModule(moduleId));
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
        /// Build Navigation
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult<List<ModuleNavigationModel>>> BuildNavigation()
        {
            var responseResult = new ResponseResult<List<ModuleNavigationModel>>();
            var httpResponseModule = await this.httpClient.GetAsync(this.urls.AdminAPI + AdminOperations.GetAllModules());

            // Get Modules
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

            var modules = JsonConvert.DeserializeObject<List<Module>>(httpResponseModule.Content.ReadAsStringAsync().Result);
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
            var userNavigations = modules.Select(
                x => new ModuleNavigationModel()
                {
                    ModuleId = x.ModuleId,
                    ModuleDescription = x.Description,
                    ModuleDisplayName = x.DisplayName,
                    ModuleName = x.Name,
                    ModuleUrl = x.Url,
                    DisplayOrder = (x.DisplayOrder != null ? Convert.ToInt32(x.DisplayOrder) : 0),
                    IsNavigationItem = x.IsNavigationItem,
                    IsVisible = x.IsVisible,
                    ParentModuleId = x.ParentModuleId
                }).ToList();


            // Create Navigation Hierarchy
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
            responseResult.Message = ResponseMessage.RecordFetched;
            responseResult.ResponseCode = ResponseCode.RecordFetched;
            return responseResult;
        }

        /// <summary>
        /// All Modules For Service
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult<List<ModulesAllModel>>> GetModulesForService(string serviceName)
        {
            var responseResult = new ResponseResult<List<ModulesAllModel>>();
            var httpResponseModule = await this.httpClient.GetAsync(this.urls.AdminAPI + AdminOperations.GetAllModulesForService(serviceName));

            // Get Modules
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

            var modules = JsonConvert.DeserializeObject<List<ModulesAllModel>>(httpResponseModule.Content.ReadAsStringAsync().Result);
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

              
            responseResult.Data = modules;
            responseResult.Message = ResponseMessage.RecordFetched;
            responseResult.ResponseCode = ResponseCode.RecordFetched;
            return responseResult;
        }
        /// <summary>
        /// Module Service to get module list
        /// </summary>
        /// <param name="accounId">Account Id</param>
        /// <returns></returns>
        public async Task<ResponseResult<List<Module>>> GetModulesByAccountId(long accounId)
        {
            var responseResult = new ResponseResult<List<Module>>();
            var httpResponse = await this.httpClient.GetAsync(this.urls.AdminAPI + AdminOperations.GetModulesByAccountId(accounId,false));
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

            List<Module> modules = JsonConvert.DeserializeObject<List<Module>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (modules != null && modules.Count > 0)
            {
                responseResult.Message = ResponseMessage.RecordFetched;
                responseResult.ResponseCode = ResponseCode.RecordFetched;
                responseResult.Data = modules;
                return responseResult;
            }
            else
            {
                responseResult.Message = ResponseMessage.NoRecordFound;
                responseResult.ResponseCode = ResponseCode.NoRecordFound;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.NoRecordFound
                };
                return responseResult;
            }
        }

    }
}
