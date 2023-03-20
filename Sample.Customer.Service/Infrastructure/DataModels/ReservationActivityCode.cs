using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class ReservationActivityCode
    {
        public long ReservationActivityCodeId { get; set; }
        public long AccountId { get; set; }
        public long ActivityCodeId { get; set; }
        public long ReservationId { get; set; }
        public long ParkingProvidersLocationSubLocationId { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public string ActivityDoneBy { get; set; }
        public bool? IsDropped { get; set; }

        public virtual ActivityCode ActivityCode { get; set; }
    }
}
