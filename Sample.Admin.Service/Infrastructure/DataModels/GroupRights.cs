using System;
using System.Collections.Generic;

namespace Sample.Admin.Service.Infrastructure.DataModels
{
    public partial class GroupRights
    {
        public long GroupRightId { get; set; }
        public long GroupId { get; set; }
        public long ModuleId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }

        public virtual Groups Group { get; set; }
    }
}
