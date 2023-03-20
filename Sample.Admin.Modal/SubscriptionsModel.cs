using System;
using System.Collections.Generic;

namespace Sample.Admin.Model
{
    public class SubscriptionsModel
    {
        public long SubscriptionId { get; set; }
        public long Account { get; set; }
        public int Version { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? CancelledOn { get; set; }
        public string CancelledReason { get; set; }        
        public bool Cancelled { get; set; }

        public virtual AccountsModel Accounts { get; set; }
    }

    public class SubscriptionsListVM
    {
        public long SubscriptionId { get; set; }
        public long AccountId { get; set; }
        public int VersionId { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? CancelledOn { get; set; }
        public string CancelledReason { get; set; }
        public bool Cancelled { get; set; }

        public AccountsModel Account { get; set; }
    }

    public class SubscriptionsVM
    {
        public long SubscriptionId { get; set; }
        public long AccountId { get; set; }
        public long Account { get; set; }
        public int Version { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? CancelledOn { get; set; }
        public string CancelledReason { get; set; }
        public bool Cancelled { get; set; }

    }
}
