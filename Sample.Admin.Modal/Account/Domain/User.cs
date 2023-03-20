using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Admin.Model.Account.Domain
{
    public class User
    {
        /// <summary>
        /// User ID
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// Account Id
        /// </summary>
        public long AccountId { get; set; }
        /// <summary>
        /// User Name
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// User First Name
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// User Last Name
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// User Email Address
        /// </summary>
        public string EmailAddress { get; set; }
        /// <summary>
        /// User Mobile
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// User status
        /// </summary>
        public int UserStatus { get; set; }
        /// <summary>
        /// Created on date
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// Created by User Id
        /// </summary>
        public long CreatedBy { get; set; }
        /// <summary>
        /// Updated on date
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// Updated by user id
        /// </summary>
        public long? UpdatedBy { get; set; }
        /// <summary>
        /// Acuthentication Type
        /// </summary>
        public int AuthenticationTypeId { get; set; }
        /// <summary>
        /// Mfatype Id
        /// </summary>
        public long MfatypeId { get; set; }
        /// <summary>
        /// Encrypted Password Hash
        /// </summary>
        public byte[] PasswordHash { get; set; }
        /// <summary>
        /// Encrypted Password salt
        /// </summary>
        public byte[] PasswordSalt { get; set; }
        /// <summary>
        /// If User Email id Verified
        /// </summary>
        public bool IsEmailVerified { get; set; }
        /// <summary>
        /// If User Mobile is verified
        /// </summary>
        public bool IsMobileNumberVerified { get; set; }
        /// <summary>
        /// Number of failed login attempts
        /// </summary>
        public int FailedLoginAttempts { get; set; }
    }
}
