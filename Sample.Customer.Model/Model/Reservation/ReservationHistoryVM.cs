using System;
using System.Collections.Generic;
using System.Text;
using Sample.Customer.Model.Model.Reservation;
using Sample.Customer.Model;

namespace Sample.Customer.Model.Model.Reservation
{
    public class ReservationHistoryVM
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string EmailAddress { get; set; }
        public string ValletLocation { get; set; }
        public long CreatedBy { get; set; }
        public string ReservationCode { get; set; }
        public bool IsCancelled { get; set; }

        public long ReservationId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime CreatedOn { get; set; }
        public string VehicleKeyLocation { get; set; }
        public string VehicleLocation { get; set; }
        public bool IsParked { get; set; }
        public string Comment { get; set; }
        public string BookingConfirmationNo { get; set; }
        public string SourceName { get; set; }
        public DateTime CheckInDateTime { get; set; }
        public DateTime CheckOutDateTime { get; set; }
        public UserVM UserDetail { get; set; }
        public int ReservationCount { get; set; }
        public List<FlightAndParkingReservationVM> Reservations { get; set; }
        public List<VehicleDetailVM> VehicleDetail{ get; set; }
    }
   

}