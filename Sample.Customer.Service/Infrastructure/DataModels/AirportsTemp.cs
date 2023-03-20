using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class AirportsTemp
    {
        public long AirportId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public long AddressId { get; set; }
        public TimeSpan CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long AccountId { get; set; }
    }
}
