using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model.Reservation
{
    
    public class ParkingReservationVM
    {
        public long ParkingReservationId { get; set; }
        public long AirportsParkingId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string IsConcentedToRent { get; set; }
        public long IsActive { get; set; }
        public long? AgreementTemplateId { get; set; }
        public long? ReservationId { get; set; }
        public string VehicleKeyLocation { get; set; }
        public string VehicleLocation { get; set; }
        public bool IsParked { get; set; }
    }
}
