using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sample.Admin.Model
{
    public class AccountsModel
    {
        public long AccountId { get; set; }
        public string OrganizationName { get; set; }
        public string Description { get; set; }
        //[Required(ErrorMessage = "Please enter ContactPerson"), MaxLength(100, ErrorMessage = "Maximum length is 100")]
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
        public int CurrencyId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public Guid AccountGuid { get; set; }
        public string AccountUrl { get; set; }
        public string AuthenticationCategory { get; set; }
        public int AuthenticationTypeId { get; set; }
        public short IsolationType { get; set; }
    }

    public class AccountViewModel
    {
        public long AccountId { get; set; }
        public string OrganizationName { get; set; }
        public string Description { get; set; }
        public string ContactPerson { get; set; }
        public string ContactEmail { get; set; }
        public string BillingAddress { get; set; }
        public string BillingContactPerson { get; set; }
        public string BillingEmailAddress { get; set; }
        public bool Active { get; set; }
        public string TenantCSS { get; set; }
        public string TenantLogo { get; set; }
        public string Region { get; set; }
        public string TimeZone { get; set; }
        public string Locale { get; set; }
        public string Language { get; set; }
        public int CurrencyId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public Guid AccountGuid { get; set; }
        public string AccountUrl { get; set; }
        public string AuthenticationCategory { get; set; }
        public int AuthenticationType { get; set; }
        public short IsolationType { get; set; }
    }

    public class AccountsListVM
    {
        public long AccountId { get; set; }
        public Guid AccountGuid { get; set; }
        public string OrganizationName { get; set; }
        public string TenantCss { get; set; }
        public string TenantLogo { get; set; }
        public string Description { get; set; }
        public string TimeZone { get; set; }
        public short IsolationType { get; set; }
    }

    public class AccountsCreateVM
    {
        public long AccountId { get; set; }
        public Guid AccountGuid { get; set; }
        public string OrganizationName { get; set; }
        public string AccountUrl { get; set; }
        public string ContactPerson { get; set; }
        public string ContactEmail { get; set; }
        public string TenantCss { get; set; }
        public string TenantLogo { get; set; }
        public string Description { get; set; }
        public int CurrencyId { get; set; }
        public string Region { get; set; }
        public string TimeZone { get; set; }
        public string Locale { get; set; }
        public string Language { get; set; }

    }
}
