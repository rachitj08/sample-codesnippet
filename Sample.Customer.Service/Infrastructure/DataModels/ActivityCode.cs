using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class ActivityCode
    {
        public ActivityCode()
        {
            ParkingProvidersLocationsSubLocations = new HashSet<ParkingProvidersLocationsSubLocations>();
            ReservationActivityCode = new HashSet<ReservationActivityCode>();
            TripPaxAndBags = new HashSet<TripPaxAndBags>();
            VehiclesMediaStorage = new HashSet<VehiclesMediaStorage>();
        }

        public long ActivityCodeId { get; set; }
        public string Code { get; set; }
        public long Odering { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long AccountId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ActivityCodeFor { get; set; }
        public string ScanType { get; set; }

        public virtual ICollection<ParkingProvidersLocationsSubLocations> ParkingProvidersLocationsSubLocations { get; set; }
        public virtual ICollection<ReservationActivityCode> ReservationActivityCode { get; set; }
        public virtual ICollection<TripPaxAndBags> TripPaxAndBags { get; set; }
        public virtual ICollection<VehiclesMediaStorage> VehiclesMediaStorage { get; set; }
    }
}
