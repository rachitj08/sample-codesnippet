using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sample.Admin.Model.Account.New
{
    public class NewAccount
    {
        /// <summary>
        /// Organization Name for Account Service
        /// </summary>
        [StringLength(250, ErrorMessage = "Maximum length for Organisation name is 250")]
        public string OrganizationName { get; set; }

        /// <summary>
        /// Description for Account services
        /// </summary>
        [Required(ErrorMessage = "Please enter Description")]
        [StringLength(500, ErrorMessage = "Maximum length for description is 500")]
        public string Description { get; set; }

        /// <summary>
        ///  Contact Person Name for Account Service
        /// </summary>
        [StringLength(100, ErrorMessage = "Maximum length for Contact Person name is 100")]
        public string ContactPerson { get; set; }


        /// <summary>
        /// Contact Person's Email for Account Service
        /// </summary>
        [StringLength(100, ErrorMessage = "Contact Email maximum length is 100")]
        [EmailAddress]
        public string ContactEmail { get; set; }

        /// <summary>
        /// Billing Address used by Contact Person's account
        /// </summary>
        [StringLength(500, ErrorMessage = "Billing Address maximum length is 500")]
        public string BillingAddress { get; set; }

        /// <summary>
        /// Billing Contact Person Name 
        /// </summary>
        [StringLength(100, ErrorMessage = "Billing Contact Person name maximum length is 100")]
        public string BillingContactPerson { get; set; }

        /// <summary>
        /// Billing Email Address for Account Service
        /// </summary>
        [StringLength(100, ErrorMessage = "Billing Email Address maximum length is 100")]
        [EmailAddress(ErrorMessage = "Invalid Billing Email Address")]
        public string BillingEmailAddress { get; set; }

        /// <summary>
        /// CSS for Tenant 
        /// </summary>
        [Required(ErrorMessage = "Please enter Tenant CSS")]
        public string TenantCss { get; set; }

        /// <summary>
        /// Tenant Logo for Account
        /// </summary>
        [Required(ErrorMessage = "Please enter Tenant Logo")]
        [StringLength(255, ErrorMessage = "Tenant Logo maximum length is 255")]
        public string TenantLogo { get; set; }

        /// <summary>
        /// Account Region 
        /// </summary>
        [StringLength(100, ErrorMessage = "Region maximum length is 100")]
        public string Region { get; set; }

        /// <summary>
        ///  Timezone for the area
        /// </summary>
        [StringLength(50, ErrorMessage = "TimeZone maximum length is 50")]
        public string TimeZone { get; set; }

        /// <summary>
        /// Account Locale
        /// </summary>
        [StringLength(50, ErrorMessage = "Locale maximum length is 50")]
        public string Locale { get; set; }

        /// <summary>
        /// Account Language added 
        /// </summary>
        [StringLength(50, ErrorMessage = "Language maximum length is 50")]
        public string Language { get; set; }

        /// <summary>
        /// Created by type of user assigned in value
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Updated  by type of user assigned in value
        /// </summary>
        public int UpdatedBy { get; set; }

        /// <summary>
        /// unique id given for currency of any account
        /// </summary>
        public int CurrencyId { get; set; }

        //Account GUIID
        [Required(ErrorMessage = "Please enter AccountGUID")]
        public Guid AccountGUID { get; set; }
        /// <summary>
        /// Accounts Url
        /// </summary>
        [StringLength(250, ErrorMessage = "Url maximum length is 250")]
        public string Url { get; set; }

        //For Password Policy
        public NewPasswordPolicy PasswordPolicy { get; set; }
        /// <summary>
        /// To create subscription for account
        /// </summary>
        public NewSubscription Subscription { get; set; }
        /// <summary>
        /// To create default user for account
        /// </summary>
        public NewUser User { get; set; }
    }
   

   

}
