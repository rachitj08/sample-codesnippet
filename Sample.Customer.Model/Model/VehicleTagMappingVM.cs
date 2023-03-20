using System;
using System.Collections.Generic;
using System.Text;
using Sample.Customer.Model.Model.Reservation;

namespace Sample.Customer.Model.Model
{
    public class VehicleTagMappingVM
    {
        public long VehicleTagMappingId { get; set; }
        public string TagId { get; set; }
        public long VehicleId { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public VehiclesVM Vehicle { get; set; }
    }

}
