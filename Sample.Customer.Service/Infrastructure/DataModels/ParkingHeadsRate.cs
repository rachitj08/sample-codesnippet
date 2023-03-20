using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class ParkingHeadsRate
    {
        public ParkingHeadsRate()
        {
            InvoiceDetails = new HashSet<InvoiceDetails>();
        }

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

        public virtual ParkingHeads ParkingHead { get; set; }
        public virtual ParkingProvidersLocations ParkingProviderLocation { get; set; }
        public virtual ICollection<InvoiceDetails> InvoiceDetails { get; set; }
    }
}
