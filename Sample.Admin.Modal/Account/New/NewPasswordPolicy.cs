using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Admin.Model.Account.New
{
    public class NewPasswordPolicy
    {
        /// <summary>
        /// Minimum Password length to be set in Password 
        /// </summary>
        public int MinPasswordLength { get; set; }
        /// <summary>
        /// To check if One Upper case is mandatory in Password
        /// </summary>
        public bool OneUpperCase { get; set; }
        /// <summary>
        /// To check if One lower case is mandatory in Password
        /// </summary>
        public bool OneLowerCase { get; set; }
        /// <summary>
        /// To check if One number is mandatory in Password
        /// </summary>
        public bool OneNumber { get; set; }
        /// <summary>
        /// To check if one special character is mandatory in password
        /// </summary>
        public bool OneSpecialChar { get; set; }
        /// <summary>
        /// To check if password will expire or not, if set as false, password will never expire
        /// </summary>
        public bool PasswordExpiration { get; set; }
        /// <summary>
        /// To check in how many days password will expire
        /// </summary>
        public int ExpiryInDays { get; set; }
        /// <summary>
        /// To check if only Admin can rest password after password expires
        /// </summary>
        public bool PasswordExpirationRequiresAdminReset { get; set; }
        /// <summary>
        /// to check if users can change their password
        /// </summary>
        public bool AllowUsersToChangePassword { get; set; }
        /// <summary>
        /// To check if previous password can be reused or not
        /// </summary>
        public bool PreventPasswordReuse { get; set; }
        /// <summary>
        /// To check number of previous password to remeber in password history
        /// </summary>
        public int NoOfPwdToRemember { get; set; }
        /// <summary>
        /// To check if MobileVerification is required or not
        /// </summary>
        public bool IsMobileVerificationRequired { get; set; }
        //To check if Email Verification is required or not
        public bool IsEmailVerificationRequired { get; set; }
        /// <summary>
        /// To keep track of number of Failed Password attempts
        /// </summary>
        public int NoOfFailedAttemptsAllowed { get; set; }
    }
}
