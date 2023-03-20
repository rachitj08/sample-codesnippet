using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Admin.Model
{
    public class VersionsModel
    {
        public List<long> Modules { get; set; }
        public int VersionId { get; set; }
        public string VersionCode { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public short Status { get; set; }
    }

    public class VersionModel
    {
        public List<ModulesModel> Modules { get; set; }
        public int VersionId { get; set; }
        public string VersionCode { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public short Status { get; set; }
    }
}
