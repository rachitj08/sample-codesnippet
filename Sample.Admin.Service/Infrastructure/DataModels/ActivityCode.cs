using System;
using System.Collections.Generic;

namespace Sample.Admin.Service.Infrastructure.DataModels
{
    public partial class ActivityCode
    {
        public long ActivityCodeId { get; set; }
        public long Code { get; set; }
        public long Odering { get; set; }
        public string VehicleLocation { get; set; }
        public string VehicleKeyLocation { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
    }
}
