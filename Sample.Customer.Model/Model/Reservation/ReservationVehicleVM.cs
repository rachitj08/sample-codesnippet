using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model.Reservation
{
    public class ReservationVehicleVM
    { 
        public long ReservationVehicleId { get; set; }
        public long ParkingProvidersLocationId { get; set; }
        public long VehicleId { get; set; }        
        public long ReservationId { get; set; }
        public bool IsConsented { get; set; }
    }
}
