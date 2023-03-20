using System.Collections.Generic;

namespace Sample.Admin.Model
{

    public class VersionInfoModel
    {
        /// <summary>
        /// List of versions
        /// </summary>
        public List<VersionsDetail> Versions { get; set; }
    }

    public class VersionsDetail
    {
        /// <summary>
        /// version identifier used for version services
        /// </summary>
        public int VersionId { get; set; }

        /// <summary>
        /// version name used for version services
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of version modules used for version services
        /// </summary>
        public List<VersionModulesInfoVM> VersionModules { get; set; }
    }

    public class VersionModulesInfoVM
    {
        /// <summary>
        /// Module  identifier used for version services
        /// </summary>
        public int? Module { get; set; }

        /// <summary>
        /// version Module details name used for version services
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// version module navigation used for version services
        /// </summary>
        public bool Navigation { get; set; }

        /// <summary>
        /// url used for version services
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// Description used for version services
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Sub Modules List used for version services
        /// </summary>
        public List<SubModule> SubModules { get; set; }

        /// <summary>
        /// List of Permissions given to sub version module used for version services
        /// </summary>
        public List<SubVersionModule> Permissions { get; set; }
    }


    public class SubModule
    {
        /// <summary>
        /// Sub module  identifier used for version services
        /// </summary>
        public int? SubModuleId { get; set; }

        /// <summary>
        /// Sub module Name used for version services
        /// </summary>
        public string SubModuleName { get; set; }

        /// <summary>
        /// Sub module  Navigation used for version services
        /// </summary>
        public bool SubModuleNavigation { get; set; }

        /// <summary>
        /// List of permissions in Sub version module used for version services
        /// </summary>
        public List<SubVersionModule> Permissions { get; set; }

    }

}
