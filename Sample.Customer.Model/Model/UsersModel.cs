using System;
using System.Collections.Generic;

namespace Sample.Customer.Model
{
    public class UsersModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string MobileCode { get; set; }
        public string EmailAddress { get; set; }
        public int UserStatus { get; set; }
        public List<long> Groups { get; set; }
        public List<UserRightModel> UserRights { get; set; }
        public long? DefaultDashboardId { get; set; }
        public bool IntreastedInLanding { get; set; }
        public bool IntreastedInRenting { get; set; }
        public string ExternalUserId { get; set; }
        public string AuthenticationCategory { get; set; }
        public string StripeCustomerId { get; set; }
        public bool IsProvider { get; set; }
        public long AddressId { get; set; }
    }

    public class CreateUserModel : UsersModel
    {
        public string Password { get; set; }
        
    }

    public class SaveUserProfileModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DOB { get; set; }
        public string DrivingLicense { get; set; }
    }


    public class UserProfileModel : SaveUserProfileModel
    {
        public string MobileCode { get; set; }
        public string Mobile { get; set; }
        public string EmailAddress { get; set; }
        public bool IsEmailVerified { get; set; }
        public string ImagePath { get; set; }
    }

    public class UserGroupMappingModel
    {
        public long GroupId { get; set; }
        public long AccountId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Int16 Status { get; set; }
    }

    public class UserRightModel
    {
        public long UserRightId { get; set; }
        public long AccountId { get; set; }
        public long ModuleId { get; set; }
        public bool IsPermission { get; set; }
    }

    public class UserModelResponse : UsersModel
    {
        public long UserId { get; set; }
    }
}
