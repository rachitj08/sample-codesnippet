using System;
using System.Collections.Generic;
using Sample.Customer.Model.Model.Reservation;

namespace Sample.Customer.Model.Model
{
    public class AirportsParkingVM
    {
        public AirportsParkingVM()
        {
            ParkingReservation = new HashSet<ParkingReservationVM>();
        }

        public long AirportsParkingId { get; set; }
        public long AirportId { get; set; }
        public long ParkingProvidersLocationId { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long? LoggedInUserId { get; set; }

        public virtual AirportsVM Airport { get; set; }
        public virtual ICollection<ParkingReservationVM> ParkingReservation { get; set; }
    }

    public class AirportParkingVM
    { 
        public long AirportsParkingId { get; set; }
        public long AirportId { get; set; }
        public long ParkingProvidersLocationId { get; set; }
        public long? LoggedInUserId { get; set; } 
        public AirportsVM Airport { get; set; }
        public ParkingProvidersLocationsVM ParkingProvidersLocations { get; set; }
        public AddressVM ParkingProvidersLocationAddress { get; set; }
    }
}
