using AutoMapper;
using Common.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.Model;
using Sample.Customer.Service.Infrastructure.Repository;

namespace Sample.Customer.Service.ServiceWorker
{
    public class FormsMasterService : IFormMasterService
    {
        private readonly IFormsMasterRepository formsMasterRepository;
        private readonly IMapper mapper;

        public FormsMasterService(IFormsMasterRepository formsMasterRepository, IMapper mapper)
        {
            Check.Argument.IsNotNull(nameof(formsMasterRepository), formsMasterRepository);
            this.formsMasterRepository = formsMasterRepository;
            this.mapper = mapper;
        }
        /// <summary>
        /// GetScreenControls
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="datasource"></param>
        /// <param name="accountId"></param>
        /// <param name="sourceSchema"></param>
        /// <param name="targetSchema"></param>
        /// <returns></returns>
        public async Task<ResponseResult> GetScreenControls(string screen, string datasource, long accountId)
        {
            ResponseResult objResponseResult = new ResponseResult();
            var responseResult = await this.formsMasterRepository.GetScreenControls(screen, datasource, accountId);
            if (responseResult != null && !string.IsNullOrWhiteSpace(responseResult.DataSourceName))
            {
                return new ResponseResult()
                {
                    Message = ResponseMessage.RecordFetched,
                    ResponseCode = ResponseCode.RecordFetched,
                    Data = responseResult
                };
            }
            else
            {
                return new ResponseResult()
                {
                    Message = ResponseMessage.MasterConfigurationNotDone,
                    ResponseCode = ResponseCode.MasterConfigurationNotDone,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.MasterConfigurationNotDone,
                    }
                };
            }
        }
        /// <summary>
        /// GetScreenControlsData
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="accId"></param>
        /// <param name="datasource"></param>
        /// <param name="sourceSchema"></param>
        /// <param name="targetSchema"></param>
        /// <param name="dataId"></param>
        /// <returns></returns>
        public async Task<ResponseResult> GetScreenControlsData(string screen, string datasource, long accId, long? dataId)
        {
            ResponseResult objResponseResult = new ResponseResult();
            var responseResult = await this.formsMasterRepository.GetScreenControlsData(screen, datasource, accId, dataId);
            if (responseResult != null && !string.IsNullOrWhiteSpace(responseResult.DataSourceName) && responseResult.Rows1 != null)
            {
                return new ResponseResult()
                {
                    Message = ResponseMessage.RecordFetched,
                    ResponseCode = ResponseCode.RecordFetched,
                    Data = responseResult
                };
            }
            else
            {
                return new ResponseResult()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.NoRecordFound,
                    }
                };
            }
        }
        /// <summary>
        /// GetScreenControlsListSearchData
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<ResponseResult> GetScreenControlsListSearchData(ControlsDataResponseVM obj)
        {
            ResponseResult objResponseResult = new ResponseResult();
            var responseResult = await this.formsMasterRepository.GetScreenControlsListSearchData(obj);
            if (responseResult != null && responseResult.Rows1 != null)
            {
                return new ResponseResult()
                {
                    Message = ResponseMessage.RecordFetched,
                    ResponseCode = ResponseCode.RecordFetched,
                    Data = responseResult
                };
            }
            else
            {
                return new ResponseResult()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.NoRecordFound,
                    }
                };
            }
        }
        /// <summary>
        /// InsertUpdateFormData
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ResponseResult> InsertUpdateFormData(ControlsDataResponseVM input)
        {
            var errorDetails = new Dictionary<string, string[]>();
            if (input != null)
            {
                if (!string.IsNullOrWhiteSpace(input.ScreenType))
                {
                    string screenType = input.ScreenType;
                    if (screenType.ToLower() == "edit")
                    {
                        if (string.IsNullOrWhiteSpace(input.DataSourcePrimaryKey))
                        {
                            errorDetails.Add("dataSourcePrimaryKey", new string[] { "This field may not be blank." });
                        }
                    }
                }
                else
                {
                    errorDetails.Add("screenType", new string[] { "This field may not be blank." });
                }

                if (input.DataSourceId == 0)
                {
                    errorDetails.Add("dataSourceId", new string[] { "This field may not be blank." });
                }
                if (string.IsNullOrWhiteSpace(input.DataSourceName))
                {
                    errorDetails.Add("dataSourceName", new string[] { "This field may not be blank." });
                }

                if (input.Data == null)
                {
                    errorDetails.Add("Data", new string[] { "This field may not be blank." });
                }
            }
            else
            {
                errorDetails.Add("JsonObject", new string[] { "Input Json may not be empty." });
            }
            if (errorDetails.Count > 0)
            {
                return new ResponseResult()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Detail = errorDetails,
                        Message = ResponseMessage.ValidationFailed,
                    }
                };
            }
            else
            {
                return await this.formsMasterRepository.InsertUpdateFormData(input);
            }
        }
        /// <summary>
        /// DeleteFormData
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<ResponseResult> DeleteFormData(ControlsDataResponseVM obj)
        {
            var errorDetails = new Dictionary<string, string[]>();
            if (string.IsNullOrWhiteSpace(obj.DataSourcePrimaryKey.ToString()))
            {
                errorDetails.Add("dataSourcePrimaryKey", new string[] { "This field may not be blank." });
            }
            if (errorDetails.Count > 0)
            {
                return new ResponseResult()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Detail = errorDetails,
                        Message = ResponseMessage.ValidationFailed,
                    }
                };
            }
            else
            {
                return await this.formsMasterRepository.DeleteFormData(obj);
            }
        }
        /// <summary>
        /// Save Form Master Configuration Data
        /// </summary>
        /// <param name="objFormsMasterConfiguration"></param>
        /// <returns></returns>
        public async Task<ResponseResult> SaveFormMasterConfigurationData(FormsMasterConfiguration objFormsMasterConfiguration)
        {
            ResponseResult objResponseResult = new ResponseResult();
            var errorDetails = new Dictionary<string, string[]>();
            if (string.IsNullOrWhiteSpace(objFormsMasterConfiguration.DataSourceName.ToString()))
            {
                errorDetails.Add("dataSourceName", new string[] { "This field may not be blank." });
            }
            if (errorDetails.Count > 0)
            {
                objResponseResult = new ResponseResult()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Detail = errorDetails,
                        Message = ResponseMessage.ValidationFailed,
                    }
                };
                return objResponseResult;
            }
            else
            {
                return await this.formsMasterRepository.SaveFormMasterConfigurationData(objFormsMasterConfiguration);
            }
        }

        /// <summary>
        /// GetFormMasterConfigurationData
        /// </summary>
        /// <param name="dataSourceID"></param>
        /// <returns></returns>
        public async Task<ResponseResult> GetFormMasterConfigurationData(int dataSourceID)
        {
            ResponseResult objResponseResult = new ResponseResult();
            objResponseResult = await formsMasterRepository.GetFormMasterConfigurationData(dataSourceID);
            return objResponseResult;
        }

        /// <summary>
        /// GetListDataSourceConfigurationData
        /// </summary>
        /// <param name="dataSourceID"></param>
        /// <returns></returns>
        public async Task<ResponseResult> GetListDataSourceConfigurationData(int dataSourceID)
        {
            ResponseResult objResponseResult = new ResponseResult();
            objResponseResult = await formsMasterRepository.GetListDataSourceConfigurationData(dataSourceID);
            return objResponseResult;
        }

        /// <summary>
        /// GetAllValidationTypesOptions
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult<List<ValidationTypes>>> GetAllValidationTypesOptions()
        {
            var objResponseResult = new ResponseResult<List<ValidationTypes>>();
            var result = await formsMasterRepository.GetAllValidationTypesOptions();
            if (result != null && result.Count > 0)
            {
                var data = mapper.Map<List<ValidationTypes>>(result);
                objResponseResult.Data = data;
                objResponseResult.Message = ResponseMessage.RecordFetched;
                objResponseResult.ResponseCode = ResponseCode.RecordFetched;
                return objResponseResult;
            }
            else
            {
                objResponseResult = new ResponseResult<List<ValidationTypes>>()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.NoRecordFound,
                    }
                };
                return objResponseResult;
            }
        }
        /// <summary>
        /// GetAllDataSources
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult> GetAllDataSources()
        {
            ResponseResult objResponseResult = new ResponseResult();
            var result = await formsMasterRepository.GetAllDataSources();
            if (result != null && result.Count > 0)
            {
                objResponseResult.Data = result;
                objResponseResult.Message = ResponseMessage.RecordFetched;
                objResponseResult.ResponseCode = ResponseCode.RecordFetched;
                return objResponseResult;
            }
            else
            {
                objResponseResult = new ResponseResult()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.NoRecordFound,
                    }
                };
                return objResponseResult;
            }
        }
        /// <summary>
        /// GetAllControlTypes
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult> GetAllControlTypes()
        {
            ResponseResult objResponseResult = new ResponseResult();
            var result = await formsMasterRepository.GetAllControlTypes();
            if (result != null && result.Count > 0)
            {
                objResponseResult.Data = result as List<ControlTypes>;
                objResponseResult.Message = ResponseMessage.RecordFetched;
                objResponseResult.ResponseCode = ResponseCode.RecordFetched;
                return objResponseResult;
            }
            else
            {
                objResponseResult = new ResponseResult()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.NoRecordFound,
                    }
                };
                return objResponseResult;
            }
        }

        /// <summary>
        /// Get All table list from IAM schema for Data Source
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult<List<string>>> GetAllDataSourceTables()
        {
            var result = await formsMasterRepository.GetAllDataSourceTables();

            if (result != null && result.Count > 0)
            {
                return new ResponseResult<List<string>>()
                {
                    Message = ResponseMessage.RecordFetched,
                    ResponseCode = ResponseCode.RecordFetched,
                    Data = result
                };
            }
            else
            {
                return new ResponseResult<List<string>>()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.NoRecordFound,
                    }
                };
            }
        }

        /// <summary>
        /// Save table name into DataSource
        /// </summary>
        /// <param name="accountId">Account Id</param>
        /// <param name="tableList">List of table Names</param>
        /// <returns></returns>
        public async Task<ResponseResult<string>> SaveAllDataSourceTables(string[] tableList, long accountId)
        {
            // Validations
            if (accountId < 1)
            {
                return new ResponseResult<string>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = "Invalid account Id"
                    }
                };
            }

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

            if (await formsMasterRepository.SaveAllDataSourceTables(tableList, accountId))
            {
                return new ResponseResult<string>()
                {
                    Message = ResponseMessage.RecordSaved,
                    ResponseCode = ResponseCode.RecordSaved,
                    Data = "Data Saved Successfully"
                };
            }
            else
            {
                return new ResponseResult<string>()
                {
                    Message = ResponseMessage.InternalServerError,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError,
                    }
                };
            }
        }
    }
}
