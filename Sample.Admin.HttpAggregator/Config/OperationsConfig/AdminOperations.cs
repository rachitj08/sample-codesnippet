
namespace Sample.Admin.HttpAggregator.Config.OperationsConfig
{
    /// <summary>
    /// Admin Operation List
    /// </summary>
    public static class AdminOperations
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
        /// Root path for Get Version Modules By VersionId
        /// </summary>
        /// <param name="versionId">The Version Id</param>
        /// <returns></returns>
        public static string GetVersionModulesByVersionId(int versionId) => $"/api/versions/getversionmodulesbyversionid?versionId=" + versionId;

        /// <summary>
        /// Root path for Get Version with all features(Modules, Sub Modules, and Permissions)
        /// </summary>
        /// <param name="versionId">The Version Id</param>
        /// <returns></returns>
        public static string GetVersionByVersionId(int versionId) => $"/api/versions/GetVersionByVersionId?versionId=" + versionId;

        /// <summary>
        /// Root path for Get all Versions with all features(Modules, Sub Modules, and Permissions)
        /// </summary>
        /// <returns></returns>
        public static string GetVersions() => $"/api/versions/GetVersions";

        /// <summary>
        /// Root path for Create Account
        /// </summary>
        /// <returns></returns>
        public static string AddAccount() => $"/api/accounts/";

       

        /// <summary>
        /// Root path for Create Account
        /// </summary>
        /// <returns></returns>
        public static string CreateAccount() => $"/api/accounts/createaccount";

        /// <summary>
        /// Root path for Get Account Details
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
        /// <param name="accountIdentity"></param>
        /// <returns></returns>
        public static string GetAccountDetails(string accountIdentity) => $"/api/accounts/getaccountdetails?accountIdentity=" + accountIdentity;

        /// <summary>
        /// Root path for Get Account all Details
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
        /// Root path for Get All Authentication Types
        /// </summary>
        /// <returns></returns>
        public static string GetAllAuthenticationTypes() => $"/api/authenticationtype/getallauthenticationtypes";


        /// <summary>
        /// Root path for Authenticate User
        /// </summary>
        /// <returns></returns>
        public static string AuthenticateUser() => $"/api/authentication/authenticate";

        /// <summary>
        /// Root path for Get Screen Controls
        /// </summary> 
        /// <param name="screen">Name of the screen</param>
        /// <param name="datasource">Datasource for all the fields in the screen</param>
        /// <param name="accountId">Tenant ID</param>
        /// <param name="sourceSchema">source schema name</param>
        /// <param name="targetSchema">target/dependent schema name, if any</param>
        /// <returns></returns>
        public static string GetScreenControls(string screen, string datasource, int accountId, string sourceSchema, string targetSchema)
        {
            var rtnURL = $"/api/formsmaster/getscreencontrols?";
            if (!string.IsNullOrWhiteSpace(screen))  rtnURL += "screen=" + screen;
            if (!string.IsNullOrWhiteSpace(datasource))  rtnURL += "&datasource=" + datasource;
            rtnURL += "&accountId=" + accountId + "&sourceschema=" + sourceSchema + "&targetschema=" +targetSchema;
            return rtnURL;
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
        public static string GetScreenControlsData(string screen, int accId,string datasource, string sourceSchema, string targetSchema,long? dataId)
        {
            var rtnURL = $"/api/formsmaster/getscreencontrolsdata?";
            if (!string.IsNullOrWhiteSpace(screen)) rtnURL += "screen=" + screen;
            rtnURL += "&accId=" + accId + "&datasource="+datasource;
            if (!string.IsNullOrWhiteSpace(sourceSchema)) rtnURL += "&sourceSchema=" + sourceSchema;
            if (!string.IsNullOrWhiteSpace(targetSchema)) rtnURL += "&targetSchema=" + targetSchema;
            rtnURL += "&dataId=" + dataId;
            return rtnURL;
        }

        /// <summary>
        /// Root path for Get Screen Controls List Search Data
        /// </summary>
        /// <returns></returns>
        public static string GetScreenControlsListSearchData() => $"/api/formsmaster/getscreencontrolslistsearchdata";

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
        /// Root path for Get Account Services
        /// </summary>
        /// <returns></returns>
        public static string GetPasswordPolicy() => $"/api/PasswordPolicy/";

        /// <summary>
        /// Root path for Get Account Services
        /// </summary>
        /// <returns></returns>
        public static string CreateOrUpdatePasswordPolicy() => $"/api/PasswordPolicy/CreateOrUpdatePasswordPolicy";

        #region Account Api's
        /// <summary>
        /// Account Service to get account list
        /// </summary>
        /// <returns></returns>
        public static string GetAllAccounts(string ordering, string search, int offset, int pageSize, int pageNumber, bool all)
        {
            var rtnURL = $"/api/accounts?";
            if (pageSize > 0) rtnURL += "pageSize=" + pageSize;
            if (pageNumber > 0) rtnURL += "&pageNumber=" + pageNumber;
            if (offset > 0) rtnURL += "&offset=" + offset;
            if (all) rtnURL += "&all=" + all;
            if (!string.IsNullOrWhiteSpace(ordering)) rtnURL += "&ordering=" + ordering;
            if (!string.IsNullOrWhiteSpace(search)) rtnURL += "&search=" + search;
            return rtnURL;
        }

        /// <summary>
        /// Get Account By Id
        /// </summary>
        /// <returns></returns>
        public static string GetAccountById(long accountId) => $"/api/accounts/" + accountId;

        ///// <summary>
        ///// Root path for Create Account
        ///// </summary>
        ///// <returns></returns>
        //public static string CreateAccount() => $"/api/accounts/";

        /// <summary>
        /// To update existing Account
        /// </summary>
        /// <returns></returns>
        public static string UpdateAccount(long accountId) => $"/api/accounts/" + accountId;

        /// <summary>
        /// To update existing Account partially
        /// </summary>
        /// <returns></returns>
        public static string UpdatePartialAccount(long accountId) => $"/api/accounts/" + accountId;

        /// <summary>
        /// To Delete Account
        /// </summary>
        /// <param name="accountId">The accountId to delete </param>
        /// <returns></returns>
        public static string DeleteAccount(long accountId) => $"/api/accounts/" + accountId;
        #endregion

        #region AccountService Api's
        /// <summary>
        /// AccountService Service to get accountService list
        /// </summary>
        /// <returns></returns>
        public static string GetAllAccountServices(string ordering, int offset, int pageSize, int pageNumber, bool all)
        {
            var rtnURL = $"/api/accountServices?";
            if (pageSize > 0) rtnURL += "pageSize=" + pageSize;
            if (pageNumber > 0) rtnURL += "&pageNumber=" + pageNumber;
            if (offset > 0) rtnURL += "&offset=" + offset;
            if (all) rtnURL += "&all=" + all;
            if (!string.IsNullOrWhiteSpace(ordering)) rtnURL += "&ordering=" + ordering;
            return rtnURL;
        }

        /// <summary>
        /// Get AccountService By Id
        /// </summary>
        /// <param name="accountId">account Id</param>
        /// <param name="serviceId">service Id</param>
        /// <returns></returns>
        public static string GetAccountServiceByAccountId(long accountId, int serviceId) => $"/api/accountServices/GetAccountServiceByAccountId/{accountId}/{serviceId}";

        

        /// <summary>
        /// Get AccountService By Id
        /// </summary>
        /// <returns></returns>
        public static string GetAccountServiceById(long accountServiceId) => $"/api/accountServices/" + accountServiceId;

        /// <summary>
        /// Root path for Create AccountService
        /// </summary>
        /// <returns></returns>
        public static string AddAccountService() => $"/api/accountServices/";

        /// <summary>
        /// To update existing AccountService
        /// </summary>
        /// <returns></returns>
        public static string UpdateAccountService(long accountServiceId) => $"/api/accountServices/" + accountServiceId;

        /// <summary>
        /// To update existing AccountService partially
        /// </summary>
        /// <returns></returns>
        public static string UpdatePartialAccountService(long accountServiceId) => $"/api/accountServices/" + accountServiceId;

        /// <summary>
        /// To Delete AccountService
        /// </summary>
        /// <param name="accountServiceId">The accountServiceId to delete </param>
        /// <returns></returns>
        public static string DeleteAccountService(long accountServiceId) => $"/api/accountServices/" + accountServiceId;
        #endregion

        #region Subscription Api's
        /// <summary>
        /// Subscription Service to get subscription list
        /// </summary>
        /// <returns></returns>
        public static string GetAllSubscriptions(string ordering, int offset, int pageSize, int pageNumber, bool all)
        {
            var rtnURL = $"/api/subscriptions?";
            if (pageSize > 0) rtnURL += "pageSize=" + pageSize;
            if (pageNumber > 0) rtnURL += "&pageNumber=" + pageNumber;
            if (offset > 0) rtnURL += "&offset=" + offset;
            if (all) rtnURL += "&all=" + all;
            if (!string.IsNullOrWhiteSpace(ordering)) rtnURL += "&ordering=" + ordering;
            return rtnURL;
        }

        /// <summary>
        /// Get Subscription By Id
        /// </summary>
        /// <returns></returns>
        public static string GetSubscriptionById(long subscriptionId) => $"/api/subscriptions/" + subscriptionId;

        /// <summary>
        /// to add Subscription
        /// </summary>
        /// <returns></returns>
        public static string AddSubscription() => $"/api/subscriptions/";

        /// <summary>
        /// To update existing Subscription
        /// </summary>
        /// <returns></returns>
        public static string UpdateSubscription(long subscriptionId) => $"/api/subscriptions/" + subscriptionId;

        /// <summary>
        /// To update existing Subscription partially
        /// </summary>
        /// <returns></returns>
        public static string UpdatePartialSubscription(long subscriptionId) => $"/api/subscriptions/" + subscriptionId;

        /// <summary>
        /// To Delete Subscription
        /// </summary>
        /// <param name="subscriptionId">The subscriptionId to delete </param>
        /// <returns></returns>
        public static string DeleteSubscription(long subscriptionId) => $"/api/subscriptions/" + subscriptionId;
        #endregion

        #region Group Api's
        /// <summary>
        /// Group Service to get group list
        /// </summary>
        /// <returns></returns>
        public static string GetAllGroups(string ordering, int offset, int pageSize, int pageNumber, bool all)
        {
            var rtnURL = $"/api/groups?";
            if (pageSize > 0) rtnURL += "pageSize=" + pageSize;
            if (pageNumber > 0) rtnURL += "&pageNumber=" + pageNumber;
            if (offset > 0) rtnURL += "&offset=" + offset;
            if (all) rtnURL += "&all=" + all;
            if (!string.IsNullOrWhiteSpace(ordering)) rtnURL += "&ordering=" + ordering;
            return rtnURL;
        }

        /// <summary>
        /// Get Group By Id
        /// </summary>
        /// <returns></returns>
        public static string GetGroupById(long groupId) => $"/api/groups/" + groupId;

        /// <summary>
        /// Root path for Create Group
        /// </summary>
        /// <returns></returns>
        public static string CreateGroup() => $"/api/groups/";

        /// <summary>
        /// To update existing Group
        /// </summary>
        /// <returns></returns>
        public static string UpdateGroup(long groupId) => $"/api/groups/" + groupId;

        /// <summary>
        /// To update existing Group partially
        /// </summary>
        /// <returns></returns>
        public static string UpdatePartialGroup(long groupId) => $"/api/groups/" + groupId;

        /// <summary>
        /// To Delete Group
        /// </summary>
        /// <param name="groupId">The groupId to delete </param>
        /// <returns></returns>
        public static string DeleteGroup(long groupId) => $"/api/groups/" + groupId;
        #endregion

        #region Module Api's
        /// <summary>
        /// Module Service to get module list
        /// </summary>
        /// <returns></returns>
        public static string GetAllModules(string ordering, string search, int offset, int pageSize, int pageNumber, string serviceName, bool all)
        {
            var rtnURL = $"/api/modules?";
            if (pageSize > 0) rtnURL += "pageSize=" + pageSize;
            if (pageNumber > 0) rtnURL += "&pageNumber=" + pageNumber;
            if (offset > 0) rtnURL += "&offset=" + offset;
            if (all) rtnURL += "&all=" + all;
            if (!string.IsNullOrWhiteSpace(ordering)) rtnURL += "&ordering=" + ordering;
            if (!string.IsNullOrWhiteSpace(search)) rtnURL += "&search=" + search;
            if (!string.IsNullOrWhiteSpace(serviceName)) rtnURL += "&serviceName=" + serviceName;
            return rtnURL;
        }

        /// <summary>
        /// Get Module By Id
        /// </summary>
        /// <returns></returns>
        public static string GetModuleById(long moduleId) => $"/api/modules/" + moduleId;

        /// <summary>
        /// Root path for Create Module
        /// </summary>
        /// <returns></returns>
        public static string CreateModule() => $"/api/modules/";

        /// <summary>
        /// To update existing Module
        /// </summary>
        /// <returns></returns>
        public static string UpdateModule(long moduleId) => $"/api/modules/" + moduleId;

        /// <summary>
        /// To update existing Module partially
        /// </summary>
        /// <returns></returns>
        public static string UpdatePartialModule(long moduleId) => $"/api/modules/" + moduleId;

        /// <summary>
        /// To Delete Module
        /// </summary>
        /// <param name="moduleId">The moduleId to delete </param>
        /// <returns></returns>
        public static string DeleteModule(long moduleId) => $"/api/modules/" + moduleId;

        /// <summary>
        /// Root path for Get Modules
        /// </summary>
        /// <returns></returns>
        public static string GetAllModules() => $"/api/modules/getmodules/";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="isNavigationItem"></param>
        /// <returns></returns>
        public static string GetModulesByAccountId(long accountId, bool isNavigationItem = true) => $"/api/modules/" + accountId + "/"+ isNavigationItem  + "/getmodulesbyaccountid";



        /// <summary>
        /// Root path for Get Modules
        /// </summary>
        /// <param name="serviceName">Service Name</param>
        /// <returns></returns>
        public static string GetAllModulesForService(string serviceName) => $"/api/modules/all/?serviceName=" + serviceName;

        #endregion

        #region VersionModules Api's
        /// <summary>
        /// VersionModule Service to get module list
        /// </summary>
        /// <returns></returns>
        public static string GetAllVersionModules(string ordering, int offset, int pageSize, int pageNumber, bool all) => $"/api/versionModules/?ordering=" + ordering + "&offset=" + offset + "&pageSize=" + pageSize + "&pageNumber=" + pageNumber + "&all=" + all;

        /// <summary>
        /// Get VersionModule By Id
        /// </summary>
        /// <returns></returns>
        public static string GetVersionModuleById(long versionModuleId) => $"/api/versionModules/" + versionModuleId;

        /// <summary>
        /// Root path for Create VersionModule
        /// </summary>
        /// <returns></returns>
        public static string CreateVersionModule() => $"/api/versionModules/";

        /// <summary>
        /// To update existing VersionModule
        /// </summary>
        /// <returns></returns>
        public static string UpdateVersionModule(long versionModuleId) => $"/api/versionModules/" + versionModuleId;

        /// <summary>
        /// To update existing VersionModule partially
        /// </summary>
        /// <returns></returns>
        public static string UpdatePartialVersionModule(long versionModuleId) => $"/api/versionModules/" + versionModuleId;

        /// <summary>
        /// To Delete VersionModule
        /// </summary>
        /// <param name="versionModuleId">The versionModuleId to delete </param>
        /// <returns></returns>
        public static string DeleteVersionModule(long versionModuleId) => $"/api/versionModules/" + versionModuleId;
        #endregion

        #region Version Api's

        /// <summary>
        /// Version Service to get version list
        /// </summary>
        /// <returns></returns>
        public static string GetAllVersions(string ordering, string search, int offset, int pageSize, int pageNumber, bool all) => $"/api/versions/?ordering=" + ordering + "&search=" + search + "&offset=" + offset + "&pageSize=" + pageSize + "&pageNumber=" + pageNumber + "&all=" + all;

        /// <summary>
        /// Get Version By Id
        /// </summary>
        /// <returns></returns>
        public static string GetVersionById(long versionId) => $"/api/versions/" + versionId;

        /// <summary>
        /// Root path for Create Version
        /// </summary>
        /// <returns></returns>
        public static string CreateVersion() => $"/api/versions/";

        /// <summary>
        /// To update existing Version
        /// </summary>
        /// <returns></returns>
        public static string UpdateVersion(long versionId) => $"/api/versions/" + versionId;

        /// <summary>
        /// To update existing Version partially
        /// </summary>
        /// <returns></returns>
        public static string UpdatePartialVersion(long versionId) => $"/api/versions/" + versionId;

        /// <summary>
        /// To Delete Version
        /// </summary>
        /// <param name="versionId">The versionId to delete </param>
        /// <returns></returns>
        public static string DeleteVersion(long versionId) => $"/api/versions/" + versionId;

        #endregion

        #region "Service APIs"
        /// <summary>
        /// Root path for service list
        /// </summary>
        /// <param name="ordering">Ordering Fields: (All Fields)</param>
        /// <param name="search">Search Fields: (ServiceId, ServiceName)</param>
        /// <param name="offset">The initial index from which to return the results.</param>
        /// <param name="pageSize">Number of results to return per page</param>
        /// <param name="pageNumber">Page Number of results.</param>
        /// <param name="all">It will return results (all pages) without pagination.</param>
        /// <returns></returns>
        public static string ServiceList(int pageSize, int pageNumber, string ordering, string search, int offset, bool all)
        {
            var rtnURL = "/api/services?";
            if (pageSize > 0) rtnURL += "pageSize=" + pageSize;
            if (pageNumber > 0) rtnURL += "&pageNumber=" + pageNumber;
            if (offset > 0) rtnURL += "&offset=" + offset;
            if (all) rtnURL += "&all=" + all;
            if (!string.IsNullOrWhiteSpace(ordering)) rtnURL += "&ordering=" + ordering;
            if (!string.IsNullOrWhiteSpace(search)) rtnURL += "&search=" + search;
            return rtnURL;
        }

        /// <summary>
        /// Root path for service details
        /// </summary>
        /// <returns></returns>
        public static string ServiceDetail(int serviceId) => $"/api/services/{serviceId}";
        #endregion

        #region "Admin Users APIs"

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
        public static string GetAllAdminUsers(string ordering, string search, int pageSize, int pageNumber, int offset, bool all)
        {
            var rtnURL = $"/api/AdminUsers?";
            if (!string.IsNullOrWhiteSpace(ordering)) rtnURL += "&ordering=" + ordering;
            if (!string.IsNullOrWhiteSpace(search)) rtnURL += "&search=" + search;
            if (pageSize > 0) rtnURL += "pageSize=" + pageSize;
            if (pageNumber > 0) rtnURL += "&pageNumber=" + pageNumber;
            if (offset > 0) rtnURL += "&offset=" + offset;
            if (all) rtnURL += "&all=" + all;
            return rtnURL;
        }

        /// <summary>
        /// Root path for Get Admin User details
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string GetAdminUserDetails(int userId) => $"/api/AdminUsers/" + userId;

        /// <summary>
        /// Root path for Create Admin User
        /// </summary>
        /// <returns></returns>
        public static string CreateAdminUser() => $"/api/AdminUsers/";

        /// <summary>
        /// To update existing Admin User
        /// </summary>
        /// <returns></returns>
        public static string UpdateAdminUser(int userId) => $"/api/AdminUsers/" + userId;

        /// <summary>
        /// To update existing Admin User partially
        /// </summary>
        /// <returns></returns>
        public static string UpdatePartialAdminUser(int userId) => $"/api/AdminUsers/" + userId;

        /// <summary>
        /// To Delete Admin User
        /// </summary>
        /// <param name="userId">The userId to delete </param>
        /// <returns></returns>
        public static string DeleteAdminUser(int userId) => $"/api/AdminUsers/" + userId;

        /// <summary>
        /// Change Password of Admin User
        /// </summary>
        /// <returns></returns>
        public static string ChangeAdminUserPassword() => $"/api/AdminUsers/ChangePassword";

        /// <summary>
        /// Set Password of Admin User
        /// </summary>
        /// <returns></returns>
        public static string SetAdminUserPassword() => $"/api/AdminUsers/setPassword";

        /// <summary>
        ///  Forgot Password of Admin User
        /// </summary>
        /// <returns></returns>
        public static string ForgotAdminUserPassword() => $"/api/AdminUsers/forgotPassword";

        /// <summary>
        ///  Logout
        /// </summary>
        /// <returns></returns>
        public static string Logout() => $"/api/Authentication/Logout";

        /// <summary>
        ///  Verify token for Admin User
        /// </summary>
        /// <returns></returns>
        public static string VerifyToken() => $"/api/AdminUsers/Verify";

        /// <summary>
        ///  Token Refresh
        /// </summary>
        /// <returns></returns>
        public static string TokenRefresh() => $"/api/Authentication/refresh";
        #endregion

        #region Common API's
        /// <summary>
        /// Get Records
        /// </summary>
        /// <returns></returns>
        public static string GetCommonAll(string controllerName, string ordering, int offset, int pageSize, int pageNumber, bool all)
        {
            var rtnURL = $"/api/{controllerName}?";
            if (pageSize > 0) rtnURL += "pageSize=" + pageSize;
            if (pageNumber > 0) rtnURL += "&pageNumber=" + pageNumber;
            if (offset > 0) rtnURL += "&offset=" + offset;
            if (all) rtnURL += "&all=" + all;
            if (!string.IsNullOrWhiteSpace(ordering)) rtnURL += "&ordering=" + ordering;
            return rtnURL;
        }

        /// <summary>
        /// Get Record By Id
        /// </summary>
        /// <returns></returns>
        public static string GetCommonById(string controllerName, long id) => $"/api/{controllerName}/" + id;

        /// <summary>
        /// Create Record
        /// </summary>
        /// <returns></returns>
        public static string CreateCommon(string controllerName) => $"/api/{controllerName}/";

        /// <summary>
        /// To update existing Record
        /// </summary>
        /// <returns></returns>
        public static string UpdateCommon(string controllerName, long id) => $"/api/{controllerName}/" + id;

        /// <summary>
        /// To update existing Record partially
        /// </summary>
        /// <returns></returns>
        public static string UpdatePartialCommon(string controllerName, long id) => $"/api/{controllerName}/" + id;

        /// <summary>
        /// To Delete Record
        /// </summary>
        /// <param name="controllerName">Controller Name </param>
        /// <param name="id">The Record to delete </param>
        /// <returns></returns>
        public static string DeleteCommon(string controllerName, long id) => $"/api/{controllerName}/" + id;
        #endregion
    }


}
