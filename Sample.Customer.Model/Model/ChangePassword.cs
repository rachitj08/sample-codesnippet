using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model
{
    public class ChangePassword : ChangePasswordRequest
    {
        public long AccountId { get; set; }
        public long UserId { get; set; }
    }
    public class ChangePasswordMobile : ChangePasswordRequestForSMS
    {
        public long AccountId { get; set; }
    }
}
