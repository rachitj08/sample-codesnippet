using System;

namespace Sample.Customer.Model.Model.Reservation
{
    public class FlightReservationVM
    {
        public long FlightReservationId { get; set; }
        public long ReservationId { get; set; }
        public string ReservationCode { get; set; }
        public long DepaurtureAirportId { get; set; }
        public DateTime DepaurtureDateTime { get; set; }
        public long FlyingToAirportld { get; set; }
        public string FlyingToAirline { get; set; }
        public string FlyingToFlightNo { get; set; }
        public DateTime ReturnDateTime { get; set; }
        public long? ReturningToAirportld { get; set; }
        public string ReturningToAirline { get; set; }
        public string ReturningToFlightNo { get; set; }
        public short Status { get; set; }
        public bool IsHomeAirport { get; set; }
        public bool IsBorrowingCarForRent { get; set; }
        public AirportsVM DepaurtureAirport { get; set; }
        public AirportsVM FlyingToAirportldNavigation { get; set; }
        public AirportsVM ReturningToAirportldNavigation { get; set; }
    }
}
