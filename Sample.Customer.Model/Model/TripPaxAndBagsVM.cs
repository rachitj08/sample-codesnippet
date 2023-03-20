using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model
{
   public class TripPaxAndBagsVM
    {
        public long FlightReservationId { get; set; }
        public long ActivityCodeId { get; set; }
        public short NoOfPassangers { get; set; }
        public short NoOfBags { get; set; }
    }
}
