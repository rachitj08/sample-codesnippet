using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Admin.Model.Account.Domain
{
    public class AccountDetail
    {
        public AccountDetail()
        { }
        /// <summary>
        /// Account Guid
        /// </summary>
        public long AccountId { get; set; }

        /// <summary>
        /// Account Guid
        /// </summary>
        public Guid AccountGUID { get; set; }
        /// <summary>
        /// Oranization Name
        /// </summary>
        public string OrganizationName { get; set; }
        /// <summary>
        /// Description for Account/Organization
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Inline CSS for Tenant Web page
        /// </summary>
        public Dictionary<string, string> TenantCSS { get; set; }
        /// <summary>
        /// Tenang Logo URL
        /// </summary>
        public string TenantLogo { get; set; }
        /// <summary>
        /// Timezone of organization
        /// </summary>
        public string TimeZone { get; set; }

        /// <summary>
        /// Authentication Category
        /// </summary>
        public string AuthenticationCategory { get; set; }

        /// <summary>
        /// Authentication Type Id
        /// </summary>
        public int AuthenticationTypeId { get; set; }

        /// <summary>
        /// Renting Trun
        /// </summary>
        public bool? IsRentingTrunOn { get; set; }

        /// <summary>
        /// Tour Video URL
        /// </summary>
        public string TourVideoURL { get; set; }

        /// <summary>
        /// Start Video URL
        /// </summary>
        public string GetStartVideoURL { get; set; }

    }
}
