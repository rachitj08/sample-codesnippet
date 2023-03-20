using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class Users
    {
        public Users()
        {
            Reservation = new HashSet<Reservation>();
            TripPaxAndBags = new HashSet<TripPaxAndBags>();
            UserBankAccounts = new HashSet<UserBankAccounts>();
            UserDeviceHistory = new HashSet<UserDeviceHistory>();
            UserDrivingLicense = new HashSet<UserDrivingLicense>();
            UserGroupMappings = new HashSet<UserGroupMappings>();
            UserRights = new HashSet<UserRights>();
            UserVehicles = new HashSet<UserVehicles>();
            UsersPaymentMethod = new HashSet<UsersPaymentMethod>();
        }

        public long AccountId { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Mobile { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsMobileNumberVerified { get; set; }
        public short UserStatus { get; set; }
        public int FailedLoginAttempts { get; set; }
        public DateTime? PasswordExpirationDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public bool IntreastedInLanding { get; set; }
        public bool IntreastedInRenting { get; set; }
        public int? HomeAirport { get; set; }
        public string DeviceId { get; set; }
        public string AuthenticationCategory { get; set; }
        public string ExternalUserId { get; set; }
        public string ImagePath { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string DrivingLicense { get; set; }
        public string MobileCode { get; set; }
        public long? ParkingProvidersLocationId { get; set; }
        public string StripeCustomerId { get; set; }
        public long? AddressId { get; set; }

        public virtual ParkingProvidersLocations ParkingProvidersLocation { get; set; }
        public virtual ICollection<Reservation> Reservation { get; set; }
        public virtual ICollection<TripPaxAndBags> TripPaxAndBags { get; set; }
        public virtual ICollection<UserBankAccounts> UserBankAccounts { get; set; }
        public virtual ICollection<UserDeviceHistory> UserDeviceHistory { get; set; }
        public virtual ICollection<UserDrivingLicense> UserDrivingLicense { get; set; }
        public virtual ICollection<UserGroupMappings> UserGroupMappings { get; set; }
        public virtual ICollection<UserRights> UserRights { get; set; }
        public virtual ICollection<UserVehicles> UserVehicles { get; set; }
        public virtual ICollection<UsersPaymentMethod> UsersPaymentMethod { get; set; }
    }
}
