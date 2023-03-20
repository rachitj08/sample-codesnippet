using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model.Reservation
{
    public class VehicleAvailablityVM
    {
        public long VehicleAvailablityId { get; set; }
        public long ReservationVehicleId { get; set; }
        public DateTime FromDateTime { get; set; }
        public DateTime ToDateTime { get; set; }
        public bool DurationInMinutes { get; set; }
        public long? SeqNumber { get; set; }
        public long? IsAvailable { get; set; }
        public long RentalReservationId { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long AccountId { get; set; }

        public virtual ReservationVehicleVM ReservationVehicle { get; set; }
    }
}
