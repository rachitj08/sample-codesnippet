using System;
using System.Collections.Generic;

namespace Sample.Customer.Model.Model.Reservation
{

    public class FlightAndParkingReservationVM
    {
        public long ReservationId { get; set; }
        public string ReservationCode { get; set; }
        public long UserId { get; set; }
        public bool IsCancel { get; set; }
        public FlightReservationVM FlightReservation { get; set; }
        public ParkingReservationVM ParkingReservation { get; set; }
        public ReservationVehicleVM ReservationVehicle { get; set; }
    }
}
