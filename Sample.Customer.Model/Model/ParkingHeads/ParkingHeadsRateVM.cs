using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model.ParkingHeads
{
    public class ParkingHeadsRateVM
    {
        public long ParkingHeadsRateId { get; set; }
        public long ParkingHeadId { get; set; }
        public long ParkingProviderLocationId { get; set; }
        public decimal Rate { get; set; }
        public decimal MaxDiscountPercentage { get; set; }
        public decimal MaxDiscountDollars { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long AccountId { get; set; }

        public virtual ParkingHeadsVM ParkingHead { get; set; }
        public virtual ParkingProvidersLocationsVM ParkingProviderLocation { get; set; }
    }
}
