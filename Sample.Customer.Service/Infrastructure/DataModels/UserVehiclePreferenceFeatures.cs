using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class UserVehiclePreferenceFeatures
    {
        public long UserVehiclePreferenceFeaturesId { get; set; }
        public long VehicleFeatureId { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public long LoggedInUserId { get; set; }
        public long? AccountId { get; set; }
    }
}
