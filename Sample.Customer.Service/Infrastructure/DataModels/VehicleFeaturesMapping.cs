using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class VehicleFeaturesMapping
    {
        public long VehicleFeaturesMappingId { get; set; }
        public long VehicleId { get; set; }
        public long VehicleFeatureId { get; set; }
        public long AccountId { get; set; }

        public virtual Vehicles Vehicle { get; set; }
        public virtual VehicleFeatures VehicleFeature { get; set; }
    }
}
