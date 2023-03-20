using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class RentalHeadRates
    {
        public long RentalHeadRateId { get; set; }
        public long RentalHeadId { get; set; }
        public long ParkingProvidersLocationId { get; set; }
        public long RateType { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long AccountId { get; set; }

        public virtual RentalHeads RentalHead { get; set; }
    }
}
