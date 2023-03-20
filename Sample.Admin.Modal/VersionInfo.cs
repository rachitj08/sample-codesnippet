using System.Collections.Generic;


namespace Sample.Admin.Model
{
    public class VersionInfo
    {
        /// <summary>
        /// List of versions
        /// </summary>
        public List<Version> Versions { get; set; }
    }
    public class Version
    {
        /// <summary>
        ///  Version identifier for version 
        /// </summary>
        public int VersionId { get; set; }

        /// <summary>
        /// Version Name used for versions
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of Version Modules 
        /// </summary>
        public List<VersionModulesVM> VersionModules { get; set; }
    }
    public class VersionModulesVM
    {
        /// <summary>
        /// Module identifier used as Module
        /// </summary>
        public int? Module { get; set; }

        /// <summary>
        /// Module  Name 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Navigation identifier used navigation as id
        /// </summary>
        public bool Navigation { get; set; }

        /// <summary>
        /// Sub Module Identifier
        /// </summary>
        public int? SubModule { get; set; }

        /// <summary>
        /// Sub Module Name for Version 
        /// </summary>
        public string SubModuleName { get; set; }

        /// <summary>
        /// Sub Module Navigation for version
        /// </summary>
        public bool SubModuleNavigation { get; set; }

        /// <summary>
        /// List of Sub version module permissions given
        /// </summary>
        public List<SubVersionModule> Permissions { get; set; }

    }
    public class SubVersionModule
    {
        /// <summary>
        /// Sub Module Identifier
        /// </summary>
        public int? SubModule { get; set; }

        /// <summary>
        /// Sub Module Name 
        /// </summary>
        public string SubModuleName { get; set; }

        /// <summary>
        /// Sub Module Navigation used in versions
        /// </summary>
        public bool SubModuleNavigation { get; set; }
    }


}
