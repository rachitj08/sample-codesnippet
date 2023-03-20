using Common.Model;
using Sample.Customer.Model;
using Sample.Admin.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Customer.HttpAggregator.IServices.FormsMaster
{
    /// <summary>
    /// FormsMaster service interface
    /// </summary>
    public interface IFormMasterService
    {
        /// <summary>
        /// To Get all screen control for a particular screen provided in parameter
        /// </summary>
        /// <param name="httpContext">httpContext</param>
        /// <param name="screen">Name of the screen</param>
        /// <param name="datasource">Datasource for all the fields in the screen</param>
        /// <returns></returns>
        Task<ResponseResult> GetScreenControls(HttpContext httpContext, string screen, string datasource);

        /// <summary>
        /// To Get data for the screen controls
        /// </summary>
        /// <param name="httpContext">httpContext</param>
        /// <param name="screen">Name of the screen</param>
        /// <param name="datasource">Datasource table name for all the fields in the screen</param>
        /// <param name="dataId">Record Data Id</param>
        /// <returns></returns>
        Task<ResponseResult> GetScreenControlsData(HttpContext httpContext, string screen, string datasource, long? dataId);

        /// <summary>
        /// To Get Screen Controls List Search Data
        /// </summary>
        /// <param name="objControlsDataResponseVM">Form Data in the form of json object</param>
        /// <returns></returns>
        Task<ResponseResult> GetScreenControlsListSearchData(ControlsDataResponseVM objControlsDataResponseVM);

        /// <summary>
        /// To Insert Form Data
        /// </summary>
        /// <param name="objControlsDataResponseVM">Form Data in the form of json object</param>
        /// <returns></returns>
        Task<ResponseResult> InsertFormData(ControlsDataResponseVM objControlsDataResponseVM);

        /// <summary>
        /// To Update Form Data
        /// </summary>
        /// <param name="objControlsDataResponseVM">Form Data in the form of json object</param>
        /// <returns></returns>
        Task<ResponseResult> UpdateFormData(ControlsDataResponseVM objControlsDataResponseVM);

        /// <summary>
        /// To Delete Form Data
        /// </summary>
        /// <param name="objControlsDataResponseVM">Form Data in the form of json object</param>
        /// <returns></returns>
        Task<ResponseResult> DeleteFormData(ControlsDataResponseVM objControlsDataResponseVM);
        /// <summary>
        /// SaveFormMasterConfigurationData
        /// </summary>
        /// <param name="objFormsMasterConfigurationRoot"></param>
        /// <returns></returns>
        Task<ResponseResult> SaveFormMasterConfigurationData(FormsMasterConfigurationRoot objFormsMasterConfigurationRoot);
        /// <summary>
        /// GetFormMasterConfigurationData
        /// </summary>
        /// <param name="dataSourceID"></param>
        /// <returns></returns>
        Task<ResponseResult> GetFormMasterConfigurationData(int dataSourceID);
        /// <summary>
        /// GetFormMasterConfigurationData
        /// </summary>
        /// <param name="dataSourceID"></param>
        /// <returns></returns>
        Task<ResponseResult> GetListDataSourceConfigurationData(int dataSourceID);
        /// <summary>
        /// GetAllValidationTypesOptions
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult> GetAllValidationTypesOptions();
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

        /// <summary>
        /// Get All table list from IAM schema for Data Source
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult<List<string>>> GetAllDataSourceTables();

        /// <summary>
        /// Save table name into DataSource
        /// </summary>
        /// <param name="tableList">List of table Names</param>
        /// <returns></returns>
        Task<ResponseResult<string>> SaveAllDataSourceTables(string[] tableList);
    }
}
