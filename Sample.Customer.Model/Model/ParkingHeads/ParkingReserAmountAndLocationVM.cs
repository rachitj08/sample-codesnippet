using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model.ParkingHeads
{
    public class ParkingReserAmountAndLocationVM
    {
        public long ReservationId { get; set; }
        public decimal ParkingTotalAmount { get; set; }
        public string ParkingAddress { get; set; }
        public long UserId { get; set; }
    }

    public class ParkingReservationAmountAndLocationVM: ParkingReserAmountAndLocationVM
    {
        public long ParkingProvidersLocationId { get; set; }
        public long AirportsParkingId { get; set; }
        public int InTimeGap { get; set; }
        public int OutTimeGap { get; set; }
        public long FlightReservationId { get; set; }

    }
}
