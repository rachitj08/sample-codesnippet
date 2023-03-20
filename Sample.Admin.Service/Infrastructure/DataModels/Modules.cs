using System;
using System.Collections.Generic;

namespace Sample.Admin.Service.Infrastructure.DataModels
{
    public partial class Modules
    {
        public long ModuleId { get; set; }
        public long? ParentModuleId { get; set; }
        public bool IsNavigationItem { get; set; }
        public int? ServiceId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public bool IsVisible { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public int? DisplayOrder { get; set; }
        public short Status { get; set; }
        public long? DataSourceId { get; set; }
    }
}
