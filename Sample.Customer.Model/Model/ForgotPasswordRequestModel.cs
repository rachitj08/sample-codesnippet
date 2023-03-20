using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model
{
    public class ForgotPasswordRequestModel
    {
        public string Email { get; set; }
    }
    public class ForgotPasswordRequestSMSModel
    {
        public string MobilePhone { get; set; }
    }

    public class ForgotPasswordModel : ForgotPasswordRequestModel
    {
        public long AccountId { get; set; }
    }public class ForgotPasswordSMSModel : ForgotPasswordRequestSMSModel
    {
        public long AccountId { get; set; }
    }
}
