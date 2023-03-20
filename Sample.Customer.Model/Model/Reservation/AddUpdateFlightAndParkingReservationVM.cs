using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model.Reservation
{
    public class AddUpdateFlightAndParkingReservationVM
    {
        public long DepaurtureAirportId { get; set; }
        public bool IsHomeAirport { get; set; }
        public DateTime DepaurtureDateTime { get; set; }
        public DateTime ReturnDateTime { get; set; }
        public long FlyingToAirportld { get; set; }
        public bool IsBorrowingCarForRent { get; set; }
        public string FlyingToAirline { get; set; }
        public string FlyingToFlightNo { get; set; }
        public long ReturningToAirportld { get; set; }
        public string ReturningToAirline { get; set; }
        public string ReturningToFlightNo { get; set; }
        public long CustomUserId { get; set; }
        public string Comment { get; set; }
        public long SourceId { get; set; }
        public string BookingConfirmationNo { get; set; }
    }

    public class AddUpdateFlightReservationVM : AddUpdateFlightAndParkingReservationVM
    {
        public long AccountId { get; set; }
        public long UserId { get; set; }
        public short FlightReservationStatus { get; set; }
        public int InTimeGap { get; set; }
        public int OutTimeGap { get; set; }
        public decimal TotalFinalAmount { get; set; }
        public long AirportsParkingId { get; set; }
        public long ParkingProvidersLocationId { get; set; }
        public string ComingFrom { get; set; }
    }

    public class AddFlightReservationVM : AddUpdateFlightReservationVM
    {
        public string ReservationCode { get; set; }
        
    }

    public class AddUpdateParkingReservationVM
    {
        public long ParkingReservationId { get; set; }
        public long AirportsParkingId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string IsConcentedToRent { get; set; }
        public long IsActive { get; set; }
        public long? AgreementTemplateId { get; set; }
        public long? ReservationId { get; set; }
    }
}