using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Admin.Model
{
    public class GroupRightsModel
    {
        public long GroupRightId { get; set; }
        public long GroupId { get; set; }
        public long ModuleId { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
    }

    public class GroupRightsVM
    {
        public long GroupRightId { get; set; }
        public long ModuleId { get; set; }
    }
}
