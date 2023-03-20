using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model.ParkingHeads
{
    public class ParkingSpotsVM
    {
        public long AgreementTemplateId { get; set; }
        public long ParkingProvidersLocationId { get; set; }
        public long ParkingSpotTypeId { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long ParkingSpotId { get; set; }
        public long AccountId { get; set; }

        public virtual ParkingProvidersLocationsVM ParkingProvidersLocation { get; set; }
        public virtual ParkingSpotTypeVM ParkingSpotType { get; set; }
    }
}
