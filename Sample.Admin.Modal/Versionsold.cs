using System.Collections;
using System.Collections.Generic;

namespace Sample.Admin.Model
{  

    public class Versionsold
    {
        /// <summary>
        /// version  identifier used in version services
        /// </summary>
        public int VersionId { get; set; }

        /// <summary>
        /// version name used in version services
        /// </summary>
        public string VersionName { get; set; }

        /// <summary>
        /// Module  identifier used in version services
        /// </summary>
        public long ModuleId { get; set; }

        /// <summary>
        /// Version module identifier used in version services
        /// </summary>
        public long VersionModuleId { get; set; }

        /// <summary>
        /// Parent module identifier used in version services
        /// </summary>
        public long? ParentModuleId { get; set; }

        /// <summary>
        /// Module name used for version services
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// Module Description used for version services
        /// </summary>
        public string ModuleDescription { get; set; }

        /// <summary>
        /// Navigationid Array status used for version services
        /// </summary>
        public bool IsNavigationId { get; set; }

        /// <summary>
        /// SubNavigation id used for version services
        /// </summary>
        public bool IsSubNavigationId { get; set; }

        /// <summary>
        /// Sub module  identifier used for version services
        /// </summary>
        public int? SubModuleId { get; set; }

        /// <summary>
        /// Sub module  name used for version services
        /// </summary>
        public string SubModuleName { get; set; }

        /// <summary>
        /// Sub module  description used for version services
        /// </summary>
        public string SubModuleDesc { get; set; }
        public List<Versionsold> versionNavigations { get; set; }
    }

   
}
