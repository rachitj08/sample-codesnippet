using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model
{
    public class ReservationActivityCodeVM
    {
        public long ReservationId { get; set; }
        public string ActivityCode { get; set; }
        public long ParkingProvidersLocationSubLocationId { get; set; }
        public string SubLocationType { get; set; }
        public string ScannedBy { get; set; }
        public long ParkingSpotId { get; set; }
        public string ActivityDoneBy { get; set; }
    }
}
