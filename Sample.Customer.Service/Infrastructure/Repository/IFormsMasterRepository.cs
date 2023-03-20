using Common.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sample.Customer.Model;
using DataSources = Sample.Customer.Service.Infrastructure.DataModels.DataSources;
using ValidationTypes = Sample.Customer.Service.Infrastructure.DataModels.ValidationTypes;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IFormsMasterRepository
    {
        /// <summary>
        /// Get all screen controls
        /// </summary>
        /// <param name="screen">Screen Name</param>
        /// <param name="datasource">Datasource for the fields</param>
        /// <param name="accountId">Tenant ID</param>
        /// <param name="sourceSchema">Source Schema Name of database</param>
        /// <param name="targetSchema">Target or Dependent schema name of database</param>
        /// <returns></returns> 
        Task<ScreenControlsVM> GetScreenControls(string screen, string datasource, long accountId);

        /// <summary>
        /// To Get data for the screen controls
        /// </summary>
        /// <param name="httpContext">httpContext</param>
        /// <param name="screen">Name of the screen</param>
        /// <param name="accId">Account/Tenant ID</param>
        /// <param name="sourceSchema">Schema Name</param>
        /// <param name="targetSchema">Dependent Schema Name</param>
        /// <returns></returns>
        Task<ControlsDataVM> GetScreenControlsData(string screen,string datasource, long accId, long? dataId);

        Task<ControlsDataVM> GetScreenControlsListSearchData(ControlsDataResponseVM objControlsDataResponseVM);

        /// <summary>
        /// To insert data of the form
        /// </summary>
        Task<ResponseResult> InsertUpdateFormData(ControlsDataResponseVM objControlsDataResponseVM);

        /// <summary>
        /// To insert data of the form
        /// </summary>
        Task<ResponseResult> DeleteFormData(ControlsDataResponseVM objControlsDataResponseVM);

        /// <summary>
        /// Save Form Master Configuration Data
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
        Task<List<ValidationTypes>> GetAllValidationTypesOptions();
        /// <summary>
        /// GetAllDataSource
        /// </summary>
        /// <returns></returns>
        Task<List<DataSources>> GetAllDataSources();
        /// <summary>
        /// GetAllControlType
        /// </summary>
        /// <returns></returns>
        Task<List<ControlTypes>> GetAllControlTypes();

        /// <summary>
        /// Get All table list from IAM schema for Data Source
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetAllDataSourceTables();

        /// <summary>
        /// Save table name into DataSource
        /// </summary>
        /// <param name="accountId">Account Id</param>
        /// <param name="tableList">List of table Names</param>
        /// <returns></returns>
        Task<bool> SaveAllDataSourceTables(string[] tableList, long accountId);
    }
}
