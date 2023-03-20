using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class UserVehiclePreferenceCategory
    {
        public long UserVehiclePreferenceCategoryId { get; set; }
        public long VehicleCategoryId { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public long LoggedInUserId { get; set; }
        public short SeqNo { get; set; }
        public long? AccountId { get; set; }
    }
}
