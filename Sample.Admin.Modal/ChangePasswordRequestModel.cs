using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Admin.Model
{
    public class ChangePasswordRequestModel
    {
        public string Password { get; set; }
        public string OldPassword { get; set; }
    }
}
