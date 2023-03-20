using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Admin.Model
{
    public class ModuleNavigationModel
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
        public List<ModuleNavigationModel> Navigations { get; set; }
    }
}
