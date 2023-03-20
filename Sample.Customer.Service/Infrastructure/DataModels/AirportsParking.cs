using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class AirportsParking
    {
        public AirportsParking()
        {
            ParkingReservation = new HashSet<ParkingReservation>();
        }

        public long AirportsParkingId { get; set; }
        public long AirportId { get; set; }
        public long ParkingProvidersLocationId { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long? LoggedInUserId { get; set; }
        public long AccountId { get; set; }

        public virtual Airports Airport { get; set; }
        public virtual ParkingProvidersLocations ParkingProvidersLocation { get; set; }
        public virtual ICollection<ParkingReservation> ParkingReservation { get; set; }
    }
}
