using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class AuthenticationConfigKeysValues
    {
        public long AccountId { get; set; }
        public long AuthenticationConfigKeyId { get; set; }
        public string ConfigKeyValue { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long AuthenticationConfigKeysValueId { get; set; }
    }
}
