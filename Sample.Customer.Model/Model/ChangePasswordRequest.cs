using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model
{
    public class ChangePasswordRequest
    {
        public string Password { get; set; }
        public string OldPassword { get; set; }
        public string MobileNo { get; set; }
    }

    public class ChangePasswordRequestForSMS
    {
        public string Password { get; set; }
        public string MobileNo { get; set; }
    }
}
