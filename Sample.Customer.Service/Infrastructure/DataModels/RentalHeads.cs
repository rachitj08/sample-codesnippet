using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class RentalHeads
    {
        public RentalHeads()
        {
            RentalHeadRates = new HashSet<RentalHeadRates>();
            RentalReservationOption = new HashSet<RentalReservationOption>();
        }

        public long RentalHeadId { get; set; }
        public string OptionName { get; set; }
        public string Description { get; set; }
        public string RateType { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long AccountId { get; set; }

        public virtual ICollection<RentalHeadRates> RentalHeadRates { get; set; }
        public virtual ICollection<RentalReservationOption> RentalReservationOption { get; set; }
    }
}
