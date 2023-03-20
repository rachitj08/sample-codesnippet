using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class ParkingHeads
    {
        public ParkingHeads()
        {
            InverseBasisOnNavigation = new HashSet<ParkingHeads>();
            ParkingHeadsCustomRate = new HashSet<ParkingHeadsCustomRate>();
            ParkingHeadsRate = new HashSet<ParkingHeadsRate>();
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
        public bool? IsCustomRate { get; set; }

        public virtual ParkingHeads BasisOnNavigation { get; set; }
        public virtual ICollection<ParkingHeads> InverseBasisOnNavigation { get; set; }
        public virtual ICollection<ParkingHeadsCustomRate> ParkingHeadsCustomRate { get; set; }
        public virtual ICollection<ParkingHeadsRate> ParkingHeadsRate { get; set; }
    }
}
