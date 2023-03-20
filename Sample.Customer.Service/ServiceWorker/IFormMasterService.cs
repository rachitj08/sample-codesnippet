using Common.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sample.Customer.Model;

namespace Sample.Customer.Service.ServiceWorker
{
    public interface IFormMasterService
    {
        /// <summary>
        /// GetScreenControls
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="datasource"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<ResponseResult> GetScreenControls(string screen, string datasource, long accountId);
        /// <summary>
        /// GetScreenControlsData
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="datasource"></param>
        /// <param name="accId"></param>
        /// <param name="dataId"></param>
        /// <returns></returns>
        Task<ResponseResult> GetScreenControlsData(string screen, string datasource,long accId, long? dataId);
        /// <summary>
        /// GetScreenControlsListSearchData
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        Task<ResponseResult> GetScreenControlsListSearchData(ControlsDataResponseVM obj);
        /// <summary>
        /// InsertUpdateFormData
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        Task<ResponseResult> InsertUpdateFormData(ControlsDataResponseVM obj);
        /// <summary>
        /// DeleteFormData
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        Task<ResponseResult> DeleteFormData(ControlsDataResponseVM obj);

        /// <summary>
        /// SaveFormMasterConfigurationData
        /// </summary>
        /// <param name="objFormsMasterConfiguration"></param>
        /// <returns></returns>
        Task<ResponseResult> SaveFormMasterConfigurationData(FormsMasterConfiguration objFormsMasterConfiguration);

        /// <summary>
        /// GetFormMasterConfigurationData
        /// </summary>
        /// <param name="dataSourceID"></param>
        /// <returns></returns>
        Task<ResponseResult> GetFormMasterConfigurationData(int dataSourceID);
        /// <summary>
        /// GetListDataSourceConfigurationData
        /// </summary>
        /// <param name="dataSourceID"></param>
        /// <returns></returns>
        Task<ResponseResult> GetListDataSourceConfigurationData(int dataSourceID);

        /// <summary>
        /// GetAllValidationTypesOptions
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult<List<ValidationTypes>>> GetAllValidationTypesOptions();
        /// <summary>
        /// GetAllDataSources
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult> GetAllDataSources();
        /// <summary>
        /// GetAllControlTypes
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult> GetAllControlTypes();
        Task<ResponseResult<List<string>>> GetAllDataSourceTables();
        Task<ResponseResult<string>> SaveAllDataSourceTables(string[] tableList, long accountId);
    }
}
