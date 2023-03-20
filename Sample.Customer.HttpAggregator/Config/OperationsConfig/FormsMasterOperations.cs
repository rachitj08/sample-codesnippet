using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Customer.HttpAggregator.Config.OperationsConfig
{
    /// <summary>
    /// Forms Master Operations
    /// </summary>
    public static class FormsMasterOperations
    {
        /// <summary>
        /// Root path for Insert Form Data
        /// </summary>
        /// <returns></returns>
        public static string InsertFormData() => $"/api/formsmaster/insertformdata";

        /// <summary>
        /// Root path to update Form Data
        /// </summary>
        /// <returns></returns>
        public static string UpdateFormData() => $"/api/formsmaster/updateformdata";

        /// <summary>
        /// Root path to delete Form Data
        /// </summary>
        /// <returns></returns>
        public static string DeleteFormData() => $"/api/formsmaster/deleteformdata";


        /// <summary>
        /// Root path to delete Form Data
        /// </summary>
        /// <returns></returns>
        public static string SaveFormMasterConfigurationData() => $"/api/FormMasterConfiguration/SaveFormMasterConfigurationData";
        /// <summary>
        /// GetFormMasterConfigurationData
        /// </summary>
        /// <returns></returns>
        public static string GetFormMasterConfigurationData(int dataSourceID) => $"/api/FormMasterConfiguration/GetFormMasterConfigurationData/" + dataSourceID;

        /// <summary>
        /// GetFormMasterConfigurationData
        /// </summary>
        /// <returns></returns>
        public static string GetListDataSourceConfigurationData(int dataSourceID) => $"/api/FormMasterConfiguration/GetListDataSourceConfigurationData/" + dataSourceID;


        /// <summary>
        /// GetAllValidationTypesOptions
        /// </summary>
        /// <returns></returns>
        public static string GetAllValidationTypesOptions() => $"/api/formsmaster/GetAllValidationTypesOptions";
        /// <summary>
        /// GetAllDataSources
        /// </summary>
        /// <returns></returns>
        public static string GetAllDataSources() => $"/api/formsmaster/GetAllDataSources";
        /// <summary>
        /// GetAllControlTypes
        /// </summary>
        /// <returns></returns>
        public static string GetAllControlTypes() => $"/api/formsmaster/GetAllControlTypes";

        /// <summary>
        /// Root path for Get Screen Controls
        /// </summary> 
        /// <param name="screen">Name of the screen</param>
        /// <param name="datasource">Datasource for all the fields in the screen</param>
        /// <returns></returns>
        public static string GetScreenControls(string screen, string datasource)
        {
            var rtnURL = $"/api/formsmaster/getscreencontrols?";
            if (!string.IsNullOrWhiteSpace(screen)) rtnURL += "screen=" + screen;
            if (!string.IsNullOrWhiteSpace(datasource)) rtnURL += "&datasource=" + datasource;
            return rtnURL;
        }

        /// <summary>
        /// Root path for the screen controls data
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="datasource"></param>
        /// <param name="dataId"></param>
        /// <returns></returns>
        public static string GetScreenControlsData(string screen, string datasource, long? dataId)
        {
            var rtnURL = $"/api/formsmaster/getscreencontrolsdata?";
            if (!string.IsNullOrWhiteSpace(screen)) rtnURL += "screen=" + screen;
            rtnURL += "&datasource=" + datasource;
            rtnURL += "&dataId=" + dataId;
            return rtnURL;
        }
        /// <summary>
        /// Root path for Get All Currencies
        /// </summary> 
        /// <param name="ordering">Ordering Fields: (All Fields)</param>
        /// <param name="search">Search Fields: (Code, DisplayName)</param>
        /// <param name="offset">The initial index from which to return the results.</param>
        /// <param name="pageSize">Number of results to return per page</param>
        /// <param name="pageNumber">Page Number of results.</param>
        /// <param name="all">It will return results (all pages) without pagination.</param>
        /// <returns></returns>
        public static string GetAllCurrencies(int pageSize, int pageNumber, string ordering, string search, int offset, bool all)
        {
            var rtnURL = $"/api/currencies?";
            if (pageSize > 0) rtnURL += "pageSize=" + pageSize;
            if (pageNumber > 0) rtnURL += "&pageNumber=" + pageNumber;
            if (offset > 0) rtnURL += "&offset=" + offset;
            if (all) rtnURL += "&all=" + all;
            if (!string.IsNullOrWhiteSpace(ordering)) rtnURL += "&ordering=" + ordering;
            if (!string.IsNullOrWhiteSpace(search)) rtnURL += "&search=" + search;
            return rtnURL;
        }

        /// <summary>
        /// Root path for Get currency details
        /// </summary>
        /// <param name="currencyId"></param>
        /// <returns></returns>
        public static string GetCurrencyDetails(long currencyId) => $"/api/currencies/" + currencyId;

        /// <summary>
        /// Get All table list from IAM schema for Data Source
        /// </summary>
        /// <returns>string</returns>
        public static string GetAllDataSourceTables() => "/api/FormMasterConfiguration/GetAllDataSourceTables";

        /// <summary>
        /// Save table name into DataSource
        /// </summary>
        /// <returns>string</returns>
        public static string SaveAllDataSourceTables() => "/api/FormMasterConfiguration/SaveAllDataSourceTables";

    }
}
