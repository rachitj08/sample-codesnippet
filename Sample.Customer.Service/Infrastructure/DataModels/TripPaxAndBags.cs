using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class TripPaxAndBags
    {
        public long TripPaxAndBagsId { get; set; }
        public long FlightReservationId { get; set; }
        public long UserId { get; set; }
        public long ActivityCodeId { get; set; }
        public short NoOfPassangers { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public short NoOfBags { get; set; }
        public long AccountId { get; set; }

        public virtual ActivityCode ActivityCode { get; set; }
        public virtual Users User { get; set; }
    }
}
