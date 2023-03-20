using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Admin.Model
{
    public class LoginAdminUserModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Int16 Status { get; set; }
        public string Token { get; set; }
        public string Refresh { get; set; }
    }
}
