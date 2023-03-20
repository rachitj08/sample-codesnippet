using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class ParkingSpotType
    {
        public ParkingSpotType()
        {
            ParkingSpots = new HashSet<ParkingSpots>();
        }

        public long ParkingSpotTypeId { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long AccountId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ParkingSpots> ParkingSpots { get; set; }
    }
}
