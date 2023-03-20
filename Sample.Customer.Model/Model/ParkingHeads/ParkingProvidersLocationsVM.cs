using System;
using System.Collections.Generic;
using System.Text;
using Sample.Customer.Model.Model.Reservation;

namespace Sample.Customer.Model.Model.ParkingHeads
{
    public class ParkingProvidersLocationsVM
    {
        public ParkingProvidersLocationsVM()
        {
            ParkingHeadsRate = new HashSet<ParkingHeadsRateVM>();
            ParkingSpots = new HashSet<ParkingSpotsVM>();
            ReservationVehicle = new HashSet<ReservationVehicleVM>();
        }

        public long ParkingProvidersLocationId { get; set; }
        public long ProviderId { get; set; }
        public long AddressId { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public long Capcity { get; set; }
        public bool IsValet { get; set; }
        public bool IsSelf { get; set; }
        public bool? IsRentingTrunOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long AccountId { get; set; }
        public virtual AddressVM Address { get; set; }
        public virtual ParkingProvidersVM Provider { get; set; }
        public virtual ICollection<ParkingHeadsRateVM> ParkingHeadsRate { get; set; }
        public virtual ICollection<ParkingSpotsVM> ParkingSpots { get; set; }
        public virtual ICollection<ReservationVehicleVM> ReservationVehicle { get; set; }
    }
}
