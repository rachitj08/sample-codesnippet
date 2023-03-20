using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class ParkingHeadsCustomRate
    {
        public long ParkingHeadsCustomRateId { get; set; }
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
        public long? ReservationId { get; set; }
        public bool IsActive { get; set; }

        public virtual ParkingHeads ParkingHead { get; set; }
        public virtual ParkingProvidersLocations ParkingProviderLocation { get; set; }
    }
}
