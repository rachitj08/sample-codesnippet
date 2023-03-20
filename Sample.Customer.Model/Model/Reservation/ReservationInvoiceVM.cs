using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model.Reservation
{
    public class ReservationInvoiceVM
    {
        public Dictionary<string, string> ReservationInvoiceItems { get; set; }
        public decimal ReservationTotalAmount { get; set; }
        
    }

    public class ReservationVM
    {
        public string ReservationCode { get; set; }
        public long ParkingReservationId { get; set; }
        public long ParkingProvidersLocationId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string Mobile { get; set; }
        public string MobileCode { get; set; }
        public string EmailAddress { get; set; }
    }
}
