using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class UserRights
    {
        public long AccountId { get; set; }
        public long UserRightId { get; set; }
        public long UserId { get; set; }
        public long ModuleId { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public bool IsPermission { get; set; }

        public virtual Users User { get; set; }
    }
}
