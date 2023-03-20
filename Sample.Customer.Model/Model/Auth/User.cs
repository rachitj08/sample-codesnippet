using System;
using System.Collections.Generic;

namespace Sample.Customer.Model
{
    public class User
    {
        /// <summary>
        /// Account identifier used for login user
        /// </summary>
        public long? AccountId { get; set; }

        /// <summary>
        /// User identifier used for login user
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Username  for login user
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Looged in user's FirstName
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Looged in user's LastName
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Looged in user's Email Address
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Looged in user's Mobile
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// Looged in user's Parking Providers LocationId
        /// </summary>
        public long? ParkingProvidersLocationId { get; set; }

        ///// <summary>
        ///// Mfa Type id used in login services
        ///// </summary>
        //public long? MFATypeId { get; set; }

        /// <summary>
        /// Generated Token after successfull authentication
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Use Refresh Token after Token expire
        /// </summary>
        public string Refresh { get; set; }

        /// <summary>
        /// Looged in user Account
        /// </summary>
        public UserAccount Account { get; set; }

        /// <summary>
        /// Looged in user groups
        /// </summary>
        public List<UserGroupDetail> UserGroups { get; set; }

        /// <summary>
        /// Looged in user rights
        /// </summary>
        public List<UserRight> UserRights { get; set; }

        /// <summary>
        /// Looged in user status
        /// </summary>
        public short UserStatus { get; set; }
    }

    public class UserGroupDetail
    {
        /// <summary>
        /// User Group id in user services
        /// </summary>
        public long UserGroupMappingId { get; set; }

        /// <summary>
        /// User Account Id
        /// </summary>
        public long AccountId { get; set; }

        /// <summary>
        /// User Group id in user services
        /// </summary>
        public long GroupId { get; set; }

        /// <summary>
        /// User Group Name in user services
        /// </summary>
        public string GroupName { get; set; }

    }


    public class UserRight
    {
        /// <summary>
        /// User Group id in user services
        /// </summary>
        public long UserRightId { get; set; }

        /// <summary>
        /// User Account Id
        /// </summary>
        public long AccountId { get; set; }

        /// <summary>
        /// User Group id in user services
        /// </summary>
        public long ModuleId { get; set; }

        /// <summary>
        /// User Group id in user services
        /// </summary>
        public Boolean IsPermission { get; set; }
        public string ModulesName { get; set; }
        public string ModulesDisplayName { get; set; }
        public bool IsNavigationItems { get; set; }
        public bool IsVisible { get; set; }
    } 

    public class UserAccount
    {
        /// <summary>
        /// Account Guid
        /// </summary>
        public Guid? AccountGUID { get; set; }

        /// <summary>
        /// Account Id
        /// </summary>
        public long AccountId { get; set; }

        /// <summary>
        /// Account URL
        /// </summary>
        public string AccountUrl { get; set; }

        /// <summary>
        /// Organization Name
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// Account Logo
        /// </summary>
        public string TenantLogo { get; set; }

        /// <summary>
        /// Account CSS
        /// </summary>
        public Dictionary<string, string> TenantCSS { get; set; }

        /// <summary>
        /// Account Time Zone
        /// </summary>
        public string TimeZone { get; set; }

        /// <summary>
        /// Contact Email Id
        /// </summary>
        public string ContactEmail { get; set; }

        /// <summary>
        /// Contact Person
        /// </summary>
        public string ContactPerson { get; set; }

        /// <summary>
        /// Currency Id
        /// </summary>
        public int? CurrencyId { get; set; }

        /// <summary>
        /// Account Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Account Language
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Account Locale
        /// </summary>
        public string Locale { get; set; }

        /// <summary>
        /// Account Region
        /// </summary>
        public string Region { get; set; }
    }
}
