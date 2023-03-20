using Sample.Admin.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Admin.Model.Account.Domain
{
    public class Account
    {
        public Account()
        { }
        /// <summary>
        /// Account Id
        /// </summary>
        public long AccountId { get; set; }
        /// <summary>
        /// Oranization Name
        /// </summary>
        public string OrganizationName { get; set; }
        /// <summary>
        /// Description for Account/Organization
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// ContactPerson name for organization
        /// </summary>
        public string ContactPerson { get; set; }
        /// <summary>
        /// ContactPerson email for organization
        /// </summary>
        public string ContactEmail { get; set; }
        /// <summary>
        /// Organization Billing Address
        /// </summary>
        public string BillingAddress { get; set; }
        /// <summary>
        /// ContactPerson name for billing
        /// </summary>
        public string BillingContactPerson { get; set; }
        /// <summary>
        /// ContactPerson email for billing
        /// </summary>
        public string BillingEmailAddress { get; set; }
        /// <summary>
        /// Account is Active or not
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// Inline CSS for Tenant Web page
        /// </summary>
        public string TenantCss { get; set; }
        /// <summary>
        /// Tenang Logo URL
        /// </summary>
        public string TenantLogo { get; set; }
        /// <summary>
        /// Region of organization
        /// </summary>
        public string Region { get; set; }
        /// <summary>
        /// Timezone of organization
        /// </summary>
        public string TimeZone { get; set; }
        /// <summary>
        /// Defines the User's Language
        /// </summary>
        public string Locale { get; set; }
        /// <summary>
        /// Local Language of users 
        /// </summary>
        public string Language { get; set; }
        /// <summary>
        /// Default Currency Id
        /// </summary>
        public int CurrencyId { get; set; }
        /// <summary>
        /// Created date
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// Created by user id
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Last udpated on date
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// Last updated by 
        /// </summary>
        public int? UpdatedBy { get; set; }
        
        /// <summary>
        /// Sub Domain name
        /// </summary>
        public string AccountUrl { get; set; }

        /// <summary>
        /// Account Guid value
        /// </summary>
        public Guid AccountGuid { get; set; }

        /// <summary>
        /// Authentication Category
        /// </summary>
        public string AuthenticationCategory { get; set; }

        /// <summary>
        /// Account Authentication Type
        /// </summary>
        public int AuthenticationTypeId { get; set; }

        /// <summary>
        /// To set Password policy for users created for account
        /// </summary>
        public ICollection<PasswordPolicyVM> PasswordPolicy { get; set; }
        /// <summary>
        /// To Create a group for account 
        /// </summary>
        public ICollection<Group> Groups { get; set; }
        //To create group rights for group created in account
        public ICollection<GroupsRight> GroupsRights { get; set; }
        /// <summary>
        /// To create subscription for account
        /// </summary>
        public ICollection<Subscription> Subscriptions { get; set; }
        /// <summary>
        /// To create default user for account
        /// </summary>
        public ICollection<User> Users { get; set; }
        /// <summary>
        /// To Create UserGroups Mapping for user created for account
        /// </summary>
        public ICollection<UsersGroupsMapping> UsersGroupsMapping { get; set; }
    }

    public class AccountModel : Account
    {
        public Dictionary<string, string> TenantThemeCSS { get; set; }
    }
}
