using System.Collections.Generic;

namespace Sample.Admin.Model
{
    public class ModulesAllModel
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
        /// User Service Name in user services
        /// </summary>
        public string ServiceName { get; set; }
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
        /// ModuleUrl for Modules
        /// </summary>
        public string ModuleUrl { get; set; }

        ///<summary>
        ///Module Description
        ///</summary>
        public string ModuleDescription { get; set; }

        /// <summary>
        /// Display Order for Modules
        /// </summary>
        public int? DisplayOrder { get; set; }

        ///<summary>
        ///Is Visible
        ///</summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Sub Modules List
        /// </summary>
        public List<ModulesAllModel> SubModules { get; set; }
    }

}
