using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model.ParkingHeads
{
     public class ParkingHeadsVM
    {
        public ParkingHeadsVM()
        {
            ParkingHeadsRate = new HashSet<ParkingHeadsRateVM>();
        }

        public long ParkingHeadId { get; set; }
        public bool IsActive { get; set; }
        public short SeqNo { get; set; }
        public string HeadName { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public long? BasisOn { get; set; }
        public bool? IsGovtFees { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long AccountId { get; set; }

        public virtual ICollection<ParkingHeadsRateVM> ParkingHeadsRate { get; set; }
    }
}
