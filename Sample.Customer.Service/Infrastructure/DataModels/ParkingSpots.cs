using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class ParkingSpots
    {
        public long? AgreementTemplateId { get; set; }
        public long ParkingProvidersLocationSubLocationId { get; set; }
        public long ParkingSpotTypeId { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long ParkingSpotId { get; set; }
        public long AccountId { get; set; }
        public string Name { get; set; }
        public bool? IsOccupied { get; set; }

        public virtual ParkingProvidersLocationsSubLocations ParkingProvidersLocationSubLocation { get; set; }
        public virtual ParkingSpotType ParkingSpotType { get; set; }
    }
}
