using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model.ParkingHeads
{
    public class ParkingSpotTypeVM
    {
        public ParkingSpotTypeVM()
        {
            ParkingSpots = new HashSet<ParkingSpotsVM>();
        }

        public long ParkingSpotTypeId { get; set; }
        public long Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long AccountId { get; set; }

        public virtual ICollection<ParkingSpotsVM> ParkingSpots { get; set; }
    }
}
