using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class ReservationVehicle
    {
        public ReservationVehicle()
        {
            VehicleAvailablity = new HashSet<VehicleAvailablity>();
        }

        public long ReservationVehicleId { get; set; }
        public long ParkingProvidersLocationId { get; set; }
        public long VehicleId { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long ReservationId { get; set; }
        public bool IsConsented { get; set; }
        public long AccountId { get; set; }

        public virtual ParkingProvidersLocations ParkingProvidersLocation { get; set; }
        public virtual Reservation Reservation { get; set; }
        public virtual Vehicles Vehicle { get; set; }
        public virtual ICollection<VehicleAvailablity> VehicleAvailablity { get; set; }
    }
}
