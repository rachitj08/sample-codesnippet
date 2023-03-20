using System;
using System.Collections.Generic;

namespace Sample.Admin.Service.Infrastructure.DataModels
{
    public partial class UserGroupMappings
    {
        public long UserGroupMappingId { get; set; }
        public int UserId { get; set; }
        public long GroupId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual Groups Group { get; set; }
        public virtual AdminUsers User { get; set; }
    }
}
