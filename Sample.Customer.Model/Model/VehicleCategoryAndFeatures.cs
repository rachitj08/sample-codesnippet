using System.Collections.Generic;

namespace Sample.Customer.Model
{
    public class VehicleCategoryAndFeatures
    {
        public IEnumerable<VehicleCategory> VehicleCategory { get; set; }
        public IEnumerable<VehicleFeatures> VehicleFeatures { get; set; }

    }

    public class VehicleCategory
    {
        public long VehicleCategoryId { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public short SeqNo { get; set; }
       
    }

    public class VehicleFeatures
    {
        public long VehicleFeatureId { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public bool IsSelected { get; set; }
    }
}
