
namespace Sample.Customer.HttpAggregator.Config.OperationsConfig
{
    /// <summary>
    /// Instance Management Operations
    /// </summary>
    public static class AdminAPIOperations
    {
        /// <summary>
        /// Root path for Get Account Isolation Types
        /// </summary>
        /// <returns></returns>
        public static string GetAccountIsolationTypes() => $"/api/accountisolationtype/getaccountisolationtypes";

        /// <summary>
        /// Root path for Get Account Services
        /// </summary>
        /// <returns></returns>
        public static string GetAllAccountServices() => $"/api/accountservice/getallaccountservices";

        /// <summary>
        /// Root path for Get Modules
        /// </summary>
        /// <returns></returns>
        public static string GetAllModules(bool isNavigationItem, long accountId, int serviceId) => $"/api/modules/getmodules/?isNavigationItem=" + isNavigationItem + "&accountId=" + accountId + "&serviceId=" + serviceId;

        /// <summary>
        /// Root path for Get Modules
        /// </summary>
        /// <returns></returns>
        public static string GetModuleByName(long accountId, int serviceId, string moduleName) => $"/api/modules/getmodulebyname/?accountId=" + accountId + "&serviceId=" + serviceId + "&moduleName=" + moduleName;
        /// <summary>
        /// GetModulesByAccountId
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="isNavigationItem"></param>
        /// <returns></returns>
        public static string GetModulesByAccountId(long accountId, bool isNavigationItem = true) => $"/api/modules/" + accountId + "/" + isNavigationItem + "/getmodulesbyaccountid";


        /// <summary>
        /// Root path for Get Version Modules By VersionId
        /// </summary>
        /// <param name="versionId">The Version Id</param>
        /// <returns></returns>
        public static string GetVersionModulesByVersionId(int versionId) => $"/api/version/getversionmodulesbyversionid?versionId=" + versionId;

        /// <summary>
        /// Root path for Get Version with all features(Modules, Sub Modules, and Permissions)
        /// </summary>
        /// <param name="versionId">The Version Id</param>
        /// <returns></returns>
        public static string GetVersionByVersionId(int versionId) => $"/api/version/GetVersionByVersionId?versionId=" + versionId;

        /// <summary>
        /// Root path for Get all Versions with all features(Modules, Sub Modules, and Permissions)
        /// </summary>
        /// <returns></returns>
        public static string GetVersions() => $"/api/version/GetVersions";

        /// <summary>
        /// Root path for Create Account
        /// </summary>
        /// <returns></returns>
        public static string CreateAccount() => $"/api/accounts/createaccount";

        /// <summary>
        /// Root path for Get Application Config
        /// </summary>
        /// <returns></returns>
        public static string GetApplicationConfig() => $"/api/applicationconfig/";
        

        /// <summary>
        /// Root path for Get Account Details
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public static string GetAccountDetails(long accountId) => $"/api/accounts/getaccountdetails?accountId=" + accountId;
        
        /// <summary>
        /// Root path for Get Account Details
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public static string GetAccount(long accountId) => $"/api/accounts/getaccount?accountId=" + accountId;

        /// <summary>
        /// Root path for Get Account Details
        /// </summary>
        /// <returns></returns>
        public static string GetAccountList() => $"/api/accounts/getaccounts";

        /// <summary>
        /// Root path for Get All Currencies
        /// </summary>
        /// <returns></returns>
        public static string GetAllCurrencies() => $"/api/currency/getallcurrencies";

        /// <summary>
        /// Root path for Get All Authentication Types
        /// </summary>
        /// <returns></returns>
        public static string GetAllAuthenticationTypes() => $"/api/authenticationtype/getallauthenticationtypes";


        /// <summary>
        /// Root path for Get all Authentication Config Keys
        /// </summary>
        /// <returns></returns>
        public static string GetAllAuthConfigKeys(string type) => $"/api/authenticationConfigKeys/?authenticationType=" + type;

        /// <summary>
        /// Root path for Get Screen Controls List Search Data
        /// </summary>
        /// <returns></returns>
        public static string GetScreenControlsListSearchData() => $"/api/formsmaster/getscreencontrolslistsearchdata";

        /// <summary>
        /// Root path for get vehicle category and features
        /// </summary>
        /// <returns></returns>
        public static string GetVehicleCategoryAndFeatures() => $"/api/vehiclefeatures/getvehiclecategoryandfeatures";

       
    }
}
