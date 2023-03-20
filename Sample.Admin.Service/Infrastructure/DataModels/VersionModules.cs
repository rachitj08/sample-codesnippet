using System;
using System.Collections.Generic;

namespace Sample.Admin.Service.Infrastructure.DataModels
{
    public partial class VersionModules
    {
        public long VersionModuleId { get; set; }
        public int VersionId { get; set; }
        public long ModuleId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
