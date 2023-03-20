using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model
{
   
    public class RootUserSetupModel
    {
        public long AccountId { get; set; }
        public long loggedInUserId { get; set; }
        public PasswordPolicyModel PasswordPolicy { get; set; }
        public AccountInformation CreatedAccount { get; set; }
        public List<long> VersionModules { get; set; }
        
    }

    public class AccountInformation
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }
    }
}
