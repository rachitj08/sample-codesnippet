using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model
{
    public class EmailVerificationVM 
    {
        public long AccountId { get; set; }
    }

    public class EmailVerificationRequestModel
    {
        public string Email { get; set; }
    }
}
