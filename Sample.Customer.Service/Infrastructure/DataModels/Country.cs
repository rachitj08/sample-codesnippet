using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class Country
    {
        public long Id { get; set; }
        public string CountryCode { get; set; }
        public string Name { get; set; }
        public long? PhoneCode { get; set; }
    }
}
