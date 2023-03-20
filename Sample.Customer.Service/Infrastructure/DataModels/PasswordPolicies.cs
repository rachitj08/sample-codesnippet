using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class PasswordPolicies
    {
        public long AccountId { get; set; }
        public int MinPasswordLength { get; set; }
        public bool OneUpperCase { get; set; }
        public bool OneLowerCase { get; set; }
        public bool OneNumber { get; set; }
        public bool OneSpecialChar { get; set; }
        public bool PasswordExpiration { get; set; }
        public int ExpiryInDays { get; set; }
        public bool PasswordExpirationRequiresAdminReset { get; set; }
        public bool AllowUsersToChangePassword { get; set; }
        public bool PreventPasswordReuse { get; set; }
        public int NoOfPwdToRemember { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public bool IsMobileVerificationRequired { get; set; }
        public bool IsEmailVerificationRequired { get; set; }
        public int NoOfFailedAttemptsAllowed { get; set; }
        public long PasswordPolicyId { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
