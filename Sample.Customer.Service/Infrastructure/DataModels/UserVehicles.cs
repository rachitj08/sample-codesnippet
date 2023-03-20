using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class UserVehicles
    {
        public long UserVehicleId { get; set; }
        public long VehicleId { get; set; }
        public long UserId { get; set; }
        public long AgreementTemplateId { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long? LoggedInUserId { get; set; }
        public long AccountId { get; set; }

        public virtual Users User { get; set; }
        public virtual Vehicles Vehicle { get; set; }
    }
}
