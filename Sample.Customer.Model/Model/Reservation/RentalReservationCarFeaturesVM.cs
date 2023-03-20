using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model.Reservation
{
    
    public partial class RentalReservationCarFeaturesVM
    {
        public long RentalReservationCarFeaturesId { get; set; }
        public long VehicleFeatureId { get; set; }
        public bool MustHave { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long AccountId { get; set; }
        public long? RentalReservationId { get; set; }

        public virtual RentalReservationVM RentalReservation { get; set; }
    }
}
