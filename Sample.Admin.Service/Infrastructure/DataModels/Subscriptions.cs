using System;
using System.Collections.Generic;

namespace Sample.Admin.Service.Infrastructure.DataModels
{
    public partial class Subscriptions
    {
        public long SubscriptionId { get; set; }
        public long AccountId { get; set; }
        public int VersionId { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? CancelledOn { get; set; }
        public string CancelledReason { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public bool Cancelled { get; set; }

        public virtual Accounts Account { get; set; }
    }
}
