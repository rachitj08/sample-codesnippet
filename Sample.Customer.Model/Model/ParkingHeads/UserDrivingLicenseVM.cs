using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model.ParkingHeads
{
   public class UserDrivingLicenseVM
    {
        public long UserDrivingLicenseId { get; set; }
        public long UserId { get; set; }
        public long AddressId { get; set; }
        public string DrivingLicenseNumber { get; set; }
        public string State { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfIssue { get; set; }
        public DateTime DateOfExpiration { get; set; }
        public string Class { get; set; }
        public string Gender { get; set; }
        public string Hight { get; set; }
        public string EyeColor { get; set; }
        public bool IsOrgonDonor { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long AccountId { get; set; }

        public virtual AddressVM Address { get; set; }
        public virtual UserVM User { get; set; }
    }
}
