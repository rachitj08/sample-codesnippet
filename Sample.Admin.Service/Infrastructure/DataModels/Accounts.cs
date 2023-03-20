using System;
using System.Collections.Generic;

namespace Sample.Admin.Service.Infrastructure.DataModels
{
    public partial class Accounts
    {
        public Accounts()
        {
            AccountServices = new HashSet<AccountServices>();
            Subscriptions = new HashSet<Subscriptions>();
        }

        public long AccountId { get; set; }
        public string AuthenticationCategory { get; set; }
        public int? AuthenticationTypeId { get; set; }
        public string OrganizationName { get; set; }
        public string Description { get; set; }
        public string ContactPerson { get; set; }
        public string ContactEmail { get; set; }
        public string BillingAddress { get; set; }
        public string BillingContactPerson { get; set; }
        public string BillingEmailAddress { get; set; }
        public bool Active { get; set; }
        public string TenantCss { get; set; }
        public string TenantLogo { get; set; }
        public string Region { get; set; }
        public string TimeZone { get; set; }
        public string Locale { get; set; }
        public string Language { get; set; }
        public int? CurrencyId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public short IsolationType { get; set; }
        public Guid AccountGuid { get; set; }
        public string AccountUrl { get; set; }
        public bool? IsRentingTrunOn { get; set; }
        public string TourVideoUrl { get; set; }
        public string GetStartVideoUrl { get; set; }

        public virtual ICollection<AccountServices> AccountServices { get; set; }
        public virtual ICollection<Subscriptions> Subscriptions { get; set; }
    }
}
