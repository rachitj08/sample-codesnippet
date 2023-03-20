using System;
using System.Collections.Generic;

namespace Sample.Admin.Service.Infrastructure.DataModels
{
    public partial class AuthenticationConfigKeys
    {
        public long AuthenticationConfigKeyId { get; set; }
        public string AuthenticationType { get; set; }
        public string ConfigKey { get; set; }
        public bool IsRequired { get; set; }
    }
}
