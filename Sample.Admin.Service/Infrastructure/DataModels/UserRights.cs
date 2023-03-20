using System;
using System.Collections.Generic;

namespace Sample.Admin.Service.Infrastructure.DataModels
{
    public partial class UserRights
    {
        public long UserRightId { get; set; }
        public int UserId { get; set; }
        public long ModuleId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public bool IsPermission { get; set; }

        public virtual AdminUsers User { get; set; }
    }
}
