using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class VehicleFeatures
    {
        public VehicleFeatures()
        {
            RentalReservationCarFeatures = new HashSet<RentalReservationCarFeatures>();
            VehicleFeaturesMapping = new HashSet<VehicleFeaturesMapping>();
        }

        public long VehicleFeatureId { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long? LoggedInUserId { get; set; }
        public long AccountId { get; set; }

        public virtual ICollection<RentalReservationCarFeatures> RentalReservationCarFeatures { get; set; }
        public virtual ICollection<VehicleFeaturesMapping> VehicleFeaturesMapping { get; set; }
    }
}
