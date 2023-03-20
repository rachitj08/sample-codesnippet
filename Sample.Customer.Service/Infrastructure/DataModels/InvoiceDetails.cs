using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class InvoiceDetails
    {
        public long InvoiceDetailId { get; set; }
        public long InvoiceId { get; set; }
        public long AccountId { get; set; }
        public long ParkingHeadRateId { get; set; }
        public decimal Amount { get; set; }
        public short Qty { get; set; }
        public decimal Rate { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public decimal? DiscountAmount { get; set; }
        public string DiscountType { get; set; }
        public short SeqNo { get; set; }

        public virtual Invoice Invoice { get; set; }
        public virtual ParkingHeadsRate ParkingHeadRate { get; set; }
    }
}
