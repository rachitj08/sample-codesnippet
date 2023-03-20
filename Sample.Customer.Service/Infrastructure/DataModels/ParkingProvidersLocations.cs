using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class ParkingProvidersLocations
    {
        public ParkingProvidersLocations()
        {
            AirportsParking = new HashSet<AirportsParking>();
            ParkingHeadsCustomRate = new HashSet<ParkingHeadsCustomRate>();
            ParkingHeadsRate = new HashSet<ParkingHeadsRate>();
            ParkingProvidersLocationsSubLocations = new HashSet<ParkingProvidersLocationsSubLocations>();
            ParkingReservation = new HashSet<ParkingReservation>();
            ReservationVehicle = new HashSet<ReservationVehicle>();
            Users = new HashSet<Users>();
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
        public short? AirportToParkingEtamin { get; set; }
        public string Name { get; set; }
        public int InTimeGapInMin { get; set; }
        public int OutTimeGapInMin { get; set; }

        public virtual Address Address { get; set; }
        public virtual ParkingProviders Provider { get; set; }
        public virtual ICollection<AirportsParking> AirportsParking { get; set; }
        public virtual ICollection<ParkingHeadsCustomRate> ParkingHeadsCustomRate { get; set; }
        public virtual ICollection<ParkingHeadsRate> ParkingHeadsRate { get; set; }
        public virtual ICollection<ParkingProvidersLocationsSubLocations> ParkingProvidersLocationsSubLocations { get; set; }
        public virtual ICollection<ParkingReservation> ParkingReservation { get; set; }
        public virtual ICollection<ReservationVehicle> ReservationVehicle { get; set; }
        public virtual ICollection<Users> Users { get; set; }
    }
}
