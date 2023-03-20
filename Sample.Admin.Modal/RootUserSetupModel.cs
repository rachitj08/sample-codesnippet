using Sample.Admin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Admin.Model
{
    public class RootUserSetupModel
    {
        public long AccountId { get; set; }
        public long loggedInUserId { get; set; }
        public PasswordPolicyVM PasswordPolicy { get; set; }
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
