using System;
using System.Collections.Generic;

namespace Sample.Admin.Service.Infrastructure.DataModels
{
    public partial class RentalHeads
    {
        public RentalHeads()
        {
            RentalHeadRates = new HashSet<RentalHeadRates>();
        }

        public long RentalHeadId { get; set; }
        public string OptionName { get; set; }
        public string Description { get; set; }
        public string RateType { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }

        public virtual ICollection<RentalHeadRates> RentalHeadRates { get; set; }
    }
}
