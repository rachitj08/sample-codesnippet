using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model
{
    public class UserVehicleCategoryFeaturesVM
    {
        public List<VehicleCategoryVM> VehicleCategory { get; set; }
        public long[] VehicleFeatureId { get; set; } 
    }

    public class VehicleCategoryVM
    {
        public long VehicleCategoryId { get; set; }
        public Int16 SqnNo { get; set; }
    }
}
