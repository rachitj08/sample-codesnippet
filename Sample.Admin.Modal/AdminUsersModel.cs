using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Admin.Model
{
    public class AdminUsersModel
    {
        public int UserId { get; set; }
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
        public List<AdminUserRight> UserRights { get; set; }
        public List<AdminUserGroup> UserGroups { get; set; }
    }


    public class AdminUserGroup
    {
        public long UserGroupMappingId { get; set; }
        public int UserId { get; set; }
        public long GroupId { get; set; }
    }
}
