using Common.Model;
using Sample.Customer.Model;
using Sample.Admin.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sample.Customer.HttpAggregator.Config.OperationsConfig;
using Sample.Customer.HttpAggregator.Config.UrlsConfig;
using Sample.Customer.HttpAggregator.IServices.FormsMaster;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Utilities;

namespace Sample.Customer.HttpAggregator.Services.FormsMaster
{
    /// <summary>
    /// Currency Service interface
    /// </summary>
    public class FormsMasterService : IFormMasterService
    {
        private readonly HttpClient httpClient;




        private readonly BaseUrlsConfig urls;


        /// <summary>
        /// FormsMasterService
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config"></param>
        public FormsMasterService(HttpClient httpClient, IOptions<BaseUrlsConfig> config)
        {
            Check.Argument.IsNotNull(nameof(httpClient), httpClient);
            Check.Argument.IsNotNull(nameof(config), config);
            this.httpClient = httpClient;
            this.urls = config.Value;
        }


        /// <summary>
        /// To Get List of currencies
        /// </summary>
        /// <param name="httpContext">httpContext</param>
        /// <param name="screen">Name of the screen</param>
        /// <param name="datasource">Datasource for all the fields in the screen</param>
        /// <returns></returns>
        public async Task<ResponseResult> GetScreenControls(HttpContext httpContext, string screen, string datasource)
        {
            var responseResult = new ResponseResult();
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + FormsMasterOperations.GetScreenControls(screen, datasource));
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
            responseResult = JsonConvert.DeserializeObject<ResponseResult>(httpResponse.Content.ReadAsStringAsync().Result);
            if (responseResult == null)
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
            return responseResult;
        }

        /// <summary>
        /// To Get data for the screen controls
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="screen"></param>
        /// <param name="datasource"></param>
        /// <param name="dataId"></param>
        /// <returns></returns>
        public async Task<ResponseResult> GetScreenControlsData(HttpContext httpContext, string screen, string datasource, long? dataId)
        {
            var responseResult = new ResponseResult();
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + FormsMasterOperations.GetScreenControlsData(screen, datasource, dataId));
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
            responseResult = JsonConvert.DeserializeObject<ResponseResult>(httpResponse.Content.ReadAsStringAsync().Result);
            if (responseResult == null)
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
            return responseResult;
        }

        /// <summary>
        /// Get Screen Controls List Search Data
        /// </summary>
        /// <param name="objControlsDataResponseVM">Form Data in the form of json object</param>
        /// <returns></returns>
        public async Task<ResponseResult> GetScreenControlsListSearchData(ControlsDataResponseVM objControlsDataResponseVM)
        {
            var responseResult = new ResponseResult();
            var postContent = new StringContent(JsonConvert.SerializeObject(objControlsDataResponseVM), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + AdminAPIOperations.GetScreenControlsListSearchData(), postContent);
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
            responseResult = JsonConvert.DeserializeObject<ResponseResult>(httpResponse.Content.ReadAsStringAsync().Result);
            if (responseResult == null)
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
            return responseResult;
        }

        /// <summary>
        /// To Insert Form Data
        /// </summary>
        /// <param name="objControlsDataResponseVM">Form Data in the form of json object</param>
        /// <returns></returns>
        public async Task<ResponseResult> InsertFormData(ControlsDataResponseVM objControlsDataResponseVM)
        {
            ResponseResult responseResult = new ResponseResult();
            var postContent = new StringContent(JsonConvert.SerializeObject(objControlsDataResponseVM), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + FormsMasterOperations.InsertFormData(), postContent);
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
            responseResult.Message = response.Message;
            responseResult.ResponseCode = response.ResponseCode;
            return responseResult;
        }

        /// <summary>
        /// To Update Form Data
        /// </summary>
        /// <param name="objControlsDataResponseVM">Form Data in the form of json object</param>
        /// <returns></returns>
        public async Task<ResponseResult> UpdateFormData(ControlsDataResponseVM objControlsDataResponseVM)
        {
            ResponseResult responseResult = new ResponseResult();
            var postContent = new StringContent(JsonConvert.SerializeObject(objControlsDataResponseVM), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PutAsync(this.urls.CustomerAPI + FormsMasterOperations.UpdateFormData(), postContent);
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
            responseResult.Message = response.Message;
            responseResult.ResponseCode = response.ResponseCode;
            return responseResult;
        }

        /// <summary>
        /// To Delete Form Data
        /// </summary>
        /// <param name="objControlsDataResponseVM">Form Data in the form of json object</param>
        /// <returns></returns>
        public async Task<ResponseResult> DeleteFormData(ControlsDataResponseVM objControlsDataResponseVM)
        {
            ResponseResult responseResult = new ResponseResult();
            var postContent = new StringContent(JsonConvert.SerializeObject(objControlsDataResponseVM), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + FormsMasterOperations.DeleteFormData(), postContent);
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
            responseResult.Message = response.Message;
            responseResult.ResponseCode = response.ResponseCode;
            return responseResult;
        }
        /// <summary>
        /// Save Form Master Configuration Data
        /// </summary>
        /// <param name="objFormsMasterConfigurationRoot"></param>
        /// <returns></returns>
        public async Task<ResponseResult> SaveFormMasterConfigurationData(FormsMasterConfigurationRoot objFormsMasterConfigurationRoot)
        {
            ResponseResult responseResult = new ResponseResult();
            var postContent = new StringContent(JsonConvert.SerializeObject(objFormsMasterConfigurationRoot), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + FormsMasterOperations.SaveFormMasterConfigurationData(), postContent);
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
            responseResult.Message = response.Message;
            responseResult.ResponseCode = response.ResponseCode;
            return responseResult;
        }
        /// <summary>
        /// GetAllValidationTypesOptions
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult> GetFormMasterConfigurationData(int dataSourceID)
        {
            ResponseResult responseResult = new ResponseResult();
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + FormsMasterOperations.GetFormMasterConfigurationData(dataSourceID));
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
        /// GetAllValidationTypesOptions
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult> GetAllValidationTypesOptions()
        {
            ResponseResult responseResult = new ResponseResult();
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + FormsMasterOperations.GetAllValidationTypesOptions());
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
        /// GetAllDataSources
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult> GetAllDataSources()
        {
            ResponseResult responseResult = new ResponseResult();
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + FormsMasterOperations.GetAllDataSources());
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
        /// GetAllControlTypes
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult> GetAllControlTypes()
        {
            ResponseResult responseResult = new ResponseResult();
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + FormsMasterOperations.GetAllControlTypes());
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
        /// GetListDataSourceConfigurationData
        /// </summary>
        /// <param name="dataSourceID"></param>
        /// <returns></returns>
        public async Task<ResponseResult> GetListDataSourceConfigurationData(int dataSourceID)
        {
            ResponseResult responseResult = new ResponseResult();
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + FormsMasterOperations.GetListDataSourceConfigurationData(dataSourceID));
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
        /// Get All table list from IAM schema for Data Source
        /// </summary> 
        /// <returns></returns>
        public async Task<ResponseResult<List<string>>> GetAllDataSourceTables()
        {
            var responseResult = new ResponseResult<List<string>>();
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + FormsMasterOperations.GetAllDataSourceTables());
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

            var response = JsonConvert.DeserializeObject<ResponseResult<List<string>>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                return new ResponseResult<List<string>>()
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
        /// Save table name into DataSource
        /// </summary>
        /// <param name="tableList">List of table Names</param>
        /// <returns></returns>
        public async Task<ResponseResult<string>> SaveAllDataSourceTables(string[] tableList)
        {
            // Validations
            if (tableList == null && tableList.Length < 1)
            {
                return new ResponseResult<string>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = "Table List is null or empty"
                    }
                };
            }

            // Create request
            var responseResult = new ResponseResult<string>();
            var postContent = new StringContent(JsonConvert.SerializeObject(tableList), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + FormsMasterOperations.SaveAllDataSourceTables(), postContent);

            // Handle Response
            if (httpResponse == null || httpResponse.Content == null)
            {
                responseResult.Message = ResponseMessage.InternalServerError;
                responseResult.ResponseCode = ResponseCode.InternalServerError;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.InternalServerError
                };
                return responseResult;
            }

            var response = JsonConvert.DeserializeObject<ResponseResult<string>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (response == null)
            {
                return new ResponseResult<string>()
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
