using System.Collections.Generic;

namespace Sample.Customer.Model
{
    public class UserGroup
    {
        /// <summary>
        /// User Group id in user services
        /// </summary>
        public long? GroupId { get; set; }
        /// <summary>
        /// User Group Name in user services
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// User ModuleId in user services
        /// </summary>
        public long ModuleId { get; set; }
        /// <summary>
        /// User Module Name in user services
        /// </summary>
        public string ModuleName { get; set; }
        /// <summary>
        /// ParentModuleId for Modules
        /// </summary>
        public long? ParentModuleId { get; set; }
        /// <summary>
        /// ParentModuleName for Modules
        /// </summary>
        public string ParentModuleName { get; set; }
        /// <summary>
        /// IsNavigationItem for Modules
        /// </summary>
        public bool IsNavigationItem { get; set; }
        /// <summary>
        /// IsVisible for Modules
        /// </summary>
        public bool IsVisible { get; set; }
        /// <summary>
        /// ModuleUrl for Modules
        /// </summary>
        public string ModuleUrl { get; set; }

    }

    public class UserModule
    {
        /// <summary>
        /// User Module id in user services
        /// </summary>
        public long ModuleId { get; set; }


        /// <summary>
        /// User Module Name in user services
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// User Group Description for user services
        /// </summary>
        public string Description { get; set; }
    }
 
    public class UserModuleNavigation
    {

        /// <summary>
        /// User Group id in user services
        /// </summary>
        public long? GroupId { get; set; }
        /// <summary>
        /// User Group Name in user services
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// User ModuleId in user services
        /// </summary>
        public int ModuleId { get; set; }
        /// <summary>
        /// User Module Name in user services
        /// </summary>
        public string ModuleName { get; set; }
        /// <summary>
        /// Display Name for Modules
        /// </summary>
        public string ModuleDisplayName { get; set; }
        /// <summary>
        /// ParentModuleId for Modules
        /// </summary>
        public int? ParentModuleId { get; set; }
       
        /// <summary>
        /// IsNavigationItem for Modules
        /// </summary>
        public bool IsNavigationItem { get; set; }
        /// <summary>
        /// IsVisible for Modules
        /// </summary>
        public bool IsVisible { get; set; }
        /// <summary>
        /// ModuleUrl for Modules
        /// </summary>
        public string ModuleUrl { get; set; }
        ///<summary>
        ///Name for Modules
        ///</summary>
        public string ModuleDescription { get; set; }

    }
    public class UserRightsNavigation
    {
        /// <summary>
        /// User Module ID for Modules in User Rights
        /// </summary>
        public long ModuleId { get; set; }
        /// <summary>
        /// IsPermission for User Rights
        /// </summary>
        public bool IsPermission { get; set; }
        /// <summary>
        /// Module Description for Modules
        /// </summary>
        public string ModuleDescription { get; set; }
        /// <summary>
        /// Module Display Name for Modules
        /// </summary>
        public string ModuleDisplayName { get; set; }
        /// <summary>
        /// IsNavigationItem for Modules ---If True--Navigation Item--if False--Permission
        /// </summary>
        public bool IsNavigationItem { get; set; }

        /// <summary>
        /// Display Order for Modules
        /// </summary>
        public int DisplayOrder { get; set; }
        /// <summary>
        /// IsVisible for Modules
        /// </summary>
        public bool IsVisible { get; set; }
        /// <summary>
        /// Parent Module Id for Modules
        /// </summary>
        public long? ParentModuleId { get; set; }
        /// <summary>
        /// ModuleUrl for Modules
        /// </summary>
        public string ModuleUrl { get; set; }
        /// <summary>
        /// Name for Modules
        /// </summary>
        public string ModuleName { get; set; }
    }
    public class UserNavigation
    {
        /// <summary>
        /// User ModuleId in user services
        /// </summary>
        public long ModuleId { get; set; }
        /// <summary>
        /// User Module Name in user services
        /// </summary>
        public string ModuleName { get; set; }
        /// <summary>
        /// Display Name for Modules
        /// </summary>
        public string ModuleDisplayName { get; set; }
        /// <summary>
        /// ParentModuleId for Modules
        /// </summary>
        public long? ParentModuleId { get; set; }

        /// <summary>
        /// IsNavigationItem for Modules
        /// </summary>
        public bool IsNavigationItem { get; set; }

        /// <summary>
        /// Display Order for Modules
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// ModuleUrl for Modules
        /// </summary>
        public string ModuleUrl { get; set; }
        ///<summary>
        ///Name for Modules
        ///</summary>
        public string ModuleDescription { get; set; }

        ///<summary>
        ///Name for Modules
        ///</summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// userNavigations for sub Navigation List
        /// </summary>
        public List<UserNavigation> Navigations { get; set; }

        public long? DataSourceId { get; set; }

    }     

    public class UserRightDetail
    { 
        public long ModuleId { get; set; }
        public bool IsPermission { get; set; }
    }
}
