using System;
using System.Collections.Generic;

namespace Sample.Admin.Model
{
    public class UserCreationModel
    {
        public int UserId { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string EmailAddress { get; set; }
        public int FailedLoginAttempts { get; set; }
        public DateTime? PasswordExpirationDate { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsMobileNumberVerified { get; set; }
        public short UserStatus { get; set; }
        public string AuthenticationCategory { get; set; }
        public string ExternalUserId { get; set; }
        public int? MfatypeId { get; set; }
        public long[] Groups { get; set; }
        public List<AdminUserRight> UserRights { get; set; }
    }

    public class AdminUserRight
    { 
        public long UserRightId { get; set; }
        public long ModuleId { get; set; }
        public bool IsPermission { get; set; }
    }
}
