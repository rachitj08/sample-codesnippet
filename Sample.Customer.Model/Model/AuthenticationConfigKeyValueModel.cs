using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model
{
    public class AuthenticationConfigKeyValueModel
    {
        public long AccountId { get; set; }
        public long AuthenticationConfigKeyId { get; set; }
        public string ConfigKeyValue { get; set; }
        public long AuthenticationConfigKeysValueId { get; set; }
    }
}
