using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sample.Admin.Model.Account.New
{
    public class NewUser
    {
        /// <summary>
        /// username for users account
        /// </summary>
        [StringLength(100, ErrorMessage = "Username maximum length is 100")]
        public string UserName { get; set; }

       

        /// <summary>
        /// First name of user
        /// </summary>
        [StringLength(100, ErrorMessage = "First Name maximum length is 100")]
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of user
        /// </summary>
        [StringLength(100, ErrorMessage = "Last Name maximum length is 100")]
        public string LastName { get; set; }

        /// <summary>
        /// User's Email Address
        /// </summary>
        [StringLength(100, ErrorMessage = "Email Address maximum length is 100")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; }

        /// <summary>
        /// User's Mobile Number
        /// </summary>
        [StringLength(10, ErrorMessage = "Mobile maximum length is 10")]
        public string Mobile { get; set; }

        /// <summary>
        /// Mfa type id of user's account
        /// </summary>
        public int? MFATypeId { get; set; }
    }
}
