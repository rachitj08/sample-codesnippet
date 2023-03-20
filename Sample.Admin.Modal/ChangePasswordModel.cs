using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Admin.Model
{
    public class ChangePasswordModel : ChangePasswordRequestModel
    {
        public int UserId { get; set; }
    }
}
