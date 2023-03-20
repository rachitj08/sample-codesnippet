using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class Airports
    {
        public Airports()
        {
            AirportsParking = new HashSet<AirportsParking>();
            FlightReservationDepaurtureAirport = new HashSet<FlightReservation>();
            FlightReservationFlyingToAirportldNavigation = new HashSet<FlightReservation>();
            FlightReservationReturningToAirportldNavigation = new HashSet<FlightReservation>();
        }

        public long AirportId { get; set; }
        public long AccountId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public long AddressId { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public int InTimeGapInMin { get; set; }
        public int OutTimeGapInMin { get; set; }

        public virtual Address Address { get; set; }
        public virtual ICollection<AirportsParking> AirportsParking { get; set; }
        public virtual ICollection<FlightReservation> FlightReservationDepaurtureAirport { get; set; }
        public virtual ICollection<FlightReservation> FlightReservationFlyingToAirportldNavigation { get; set; }
        public virtual ICollection<FlightReservation> FlightReservationReturningToAirportldNavigation { get; set; }
    }
}
