using System;
using System.Collections.Generic;

namespace Sample.Admin.Service.Infrastructure.DataModels
{
    public partial class RentalRate
    {
        public long RentalRateId { get; set; }
        public long ParkingProvidersLocationId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string CarType { get; set; }
        public long RentalRate1 { get; set; }
        public long LeaseRate { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
    }
}
