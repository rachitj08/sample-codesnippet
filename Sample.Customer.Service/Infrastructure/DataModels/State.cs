using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class State
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long CountryId { get; set; }
    }
}
