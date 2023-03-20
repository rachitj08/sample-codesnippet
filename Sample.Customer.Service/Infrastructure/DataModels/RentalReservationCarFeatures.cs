using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class RentalReservationCarFeatures
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

        public virtual RentalReservation RentalReservation { get; set; }
        public virtual VehicleFeatures VehicleFeature { get; set; }
    }
}
