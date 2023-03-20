using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model
{
    public class FacebookConfig
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AccessTokenURL { get; set; }
        public string UserTokenURL { get; set; }
    }
}
