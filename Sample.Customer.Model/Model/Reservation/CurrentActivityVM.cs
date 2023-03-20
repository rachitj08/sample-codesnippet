using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model.Reservation
{
    public class CurrentActivityVM
    {
        public long ReservationId { get; set; }
        public long ActivityCodeId { get; set; }
        public string ActivityCode { get; set; }
        public long ParkingProvidersLocationSubLocationId { get; set; }
        public string SubLocationType { get; set; }
    }
}
