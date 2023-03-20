using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model
{
    public class SetForgotPasswordModel
    {
        public string Password { get; set; }
    }
    public class SetPasswordModel : SetForgotPasswordModel
    {
        public string Token { get; set; }
        public string Uid { get; set; }
    }
}
