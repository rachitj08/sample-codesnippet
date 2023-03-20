using System;

namespace Sample.Customer.Model
{
    public class ReservationDetailVM
    {
        public string ReservationCode { get; set; }
        public long ParkingReservationId { get; set; }
        public long ParkingProvidersLocationId { get; set; }
        public string DepaurtureAirportCode { get; set; }
        public string ParkingProvidersLocationName { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string Mobile { get; set; }
        public string MobileCode { get; set; }
        public string EmailAddress { get; set; }
        public long PaymentDetailId { get; set; }
        public string PaymentIntentId { get; set; }
    }
}
