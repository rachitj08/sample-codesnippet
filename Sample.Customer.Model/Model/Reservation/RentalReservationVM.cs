using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model.Reservation
{
  
    public partial class RentalReservationVM
    {
        public RentalReservationVM()
        {
            RentalReservationCarFeatures = new HashSet<RentalReservationCarFeaturesVM>();
        }

        public long RentalReservationId { get; set; }
        public string RentalReservationCode { get; set; }
        public long ReservationId { get; set; }
        public string PickupCity { get; set; }
        public DateTime PickupDateTime { get; set; }
        public string DropOffCity { get; set; }
        public DateTime DropOffDateTime { get; set; }
        public long VehicleCategoryId { get; set; }
        public long AgreementTemplateId { get; set; }
        public long ConfirmationStatus { get; set; }
        public string ThirdPartyConfirmationNumber { get; set; }
        public DateTime? ActualStart { get; set; }
        public DateTime? ActualEnd { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long AccountId { get; set; }

        public virtual FlightAndParkingReservationVM Reservation { get; set; }
        public virtual ICollection<RentalReservationCarFeaturesVM> RentalReservationCarFeatures { get; set; }
    }
}
