using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model.Reservation
{
    public class CheckInCheckOutVM
    {
        public long ReservationId { get; set; }
        public long ProviderLocationId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime CheckOutDateTime { get; set; }
        public DateTime CheckInDateTime { get; set; }
        public long SpotId { get; set; }
        public bool IsCheckOut { get; set; }
    }
}
