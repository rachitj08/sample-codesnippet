using System;
using System.Collections.Generic;

namespace Sample.Admin.Service.Infrastructure.DataModels
{
    public partial class AdminUsers
    {
        public AdminUsers()
        {
            UserGroupMappings = new HashSet<UserGroupMappings>();
            UserRights = new HashSet<UserRights>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public string EmailAddress { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public short UserStatus { get; set; }
        public string AuthenticationCategory { get; set; }
        public string ExternalUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsMobileNumberVerified { get; set; }
        public int? MfatypeId { get; set; }
        public int FailedLoginAttempts { get; set; }
        public DateTime? PasswordExpirationDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

        public virtual ICollection<UserGroupMappings> UserGroupMappings { get; set; }
        public virtual ICollection<UserRights> UserRights { get; set; }
    }
}
