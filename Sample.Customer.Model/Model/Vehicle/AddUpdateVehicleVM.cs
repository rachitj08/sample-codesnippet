using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model.Vehicle
{
    public class AddUpdateVehicleVM
    {
        public long VehicleId { get; set; }
        public string VIN { get; set; }
        public string LicensePlate { get; set; }
        public long StateId { get; set; }
    }
}
