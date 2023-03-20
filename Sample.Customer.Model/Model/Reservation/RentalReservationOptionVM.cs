using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model.Reservation
{
    public class RentalReservationOptionVM
    {
        public long RentalReservationOptionId { get; set; }
        public long ReservationId { get; set; }
        public long RentalHeadId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long AccountId { get; set; }

        public virtual FlightAndParkingReservationVM Reservation { get; set; }
    }
}
