using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class Reservation
    {
        public Reservation()
        {
            FlightReservation = new HashSet<FlightReservation>();
            ParkingReservation = new HashSet<ParkingReservation>();
            PaymentDetails = new HashSet<PaymentDetails>();
            RentalReservation = new HashSet<RentalReservation>();
            RentalReservationOption = new HashSet<RentalReservationOption>();
            ReservationVehicle = new HashSet<ReservationVehicle>();
        }

        public long ReservationId { get; set; }
        public string ReservationCode { get; set; }
        public long UserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long AccountId { get; set; }
        public bool? IsChanged { get; set; }
        public bool? IsCancelled { get; set; }

        public virtual Users User { get; set; }
        public virtual ICollection<FlightReservation> FlightReservation { get; set; }
        public virtual ICollection<ParkingReservation> ParkingReservation { get; set; }
        public virtual ICollection<PaymentDetails> PaymentDetails { get; set; }
        public virtual ICollection<RentalReservation> RentalReservation { get; set; }
        public virtual ICollection<RentalReservationOption> RentalReservationOption { get; set; }
        public virtual ICollection<ReservationVehicle> ReservationVehicle { get; set; }
    }
}
