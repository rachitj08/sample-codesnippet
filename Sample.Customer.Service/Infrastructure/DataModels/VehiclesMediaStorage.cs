using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class VehiclesMediaStorage
    {
        public long VehiclesMediaStorageId { get; set; }
        public long ReservationId { get; set; }
        public long VehicleId { get; set; }
        public long VehiclesMediaTypeId { get; set; }
        public string MediaPath { get; set; }
        public long ActivityCodeId { get; set; }
        public DateTime EntryTimeStamp { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long? LoggedInUserId { get; set; }
        public long AccountId { get; set; }

        public virtual ActivityCode ActivityCode { get; set; }
        public virtual Vehicles Vehicle { get; set; }
        public virtual VehiclesMediaType VehiclesMediaType { get; set; }
    }
}
