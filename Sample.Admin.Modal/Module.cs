using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Admin.Model
{
    public class Module
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
        public int? DisplayOrder { get; set; }
        public short Status { get; set; }
        public long? DataSourceId { get; set; }
    }
}
