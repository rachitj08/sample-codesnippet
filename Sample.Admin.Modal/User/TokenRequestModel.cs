using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Admin.Model
{
    public class TokenRequestModel
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}
