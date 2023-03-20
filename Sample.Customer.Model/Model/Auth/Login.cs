using Common.Enum;

namespace Sample.Customer.Model
{
  

    public class LoginIAM
    {
        /// <summary>
        /// Set Username for login
        /// </summary>
        //[Required(ErrorMessage = "Please enter User Name")]
        //[StringLength(100, ErrorMessage = "Username maximum length is 100")]
        public string UserName { get; set; }

        /// <summary>
        /// Set Password  in Login
        /// </summary>
        //[Required(ErrorMessage = "Please enter Password")]
        //[StringLength(100, ErrorMessage = "Password maximum length is 100")]
        public string Password { get; set; }

    }

    public class Login: LoginIAM
    {
        /// <summary>
        /// Tenant identifier used in Login 
        /// </summary>
        public long AccountId { get; set; } = 0;

        public long ApplicationId { get; set; }

    }

    public class ExternalLoginVM
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        //public string MobileCode { get; set; }
        //public string MobileNo { get; set; }
        public ExternalAuthTypes? AuthType { get; set; }
        public string IdToken { get; set; } 
    }

    public class ExternalUserVM : ExternalLoginVM
    { 
        public long AccountId { get; set; }
        public string UserName { get; set; }
        public string AuthenticationCategory { get; set; }
        public string ExternalUserId { get; set; }
    }

    public class FacebookUser
    {
        public string Id { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Middle_name { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
