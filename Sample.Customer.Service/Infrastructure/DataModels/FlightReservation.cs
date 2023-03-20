using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class FlightReservation
    {
        public long FlightReservationId { get; set; }
        public long ReservationId { get; set; }
        public string ReservationCode { get; set; }
        public long DepaurtureAirportId { get; set; }
        public bool IsHomeAirport { get; set; }
        public DateTime DepaurtureDateTime { get; set; }
        public DateTime ReturnDateTime { get; set; }
        public long FlyingToAirportld { get; set; }
        public bool IsBorrowingCarForRent { get; set; }
        public string FlyingToAirline { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long AccountId { get; set; }
        public string FlyingToFlightNo { get; set; }
        public long? ReturningToAirportld { get; set; }
        public string ReturningToAirline { get; set; }
        public string ReturningToFlightNo { get; set; }
        public short Status { get; set; }

        public virtual Airports DepaurtureAirport { get; set; }
        public virtual Airports FlyingToAirportldNavigation { get; set; }
        public virtual Reservation Reservation { get; set; }
        public virtual Airports ReturningToAirportldNavigation { get; set; }
    }
}
