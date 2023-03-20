using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class City
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long StateId { get; set; }
    }
}
