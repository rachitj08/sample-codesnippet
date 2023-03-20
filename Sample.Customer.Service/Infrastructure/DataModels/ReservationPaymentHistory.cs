using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class ReservationPaymentHistory
    {
        public long ReservationPaymentHistoryId { get; set; }
        public long ReservationId { get; set; }
        public string PaymentType { get; set; }
        public long? SourceId { get; set; }
        public string PaymentMode { get; set; }
        public bool? IsSystemRate { get; set; }
        public bool? IsCustomRate { get; set; }
        public decimal BaseRate { get; set; }
        public decimal? Tax { get; set; }
        public decimal? TotalAmout { get; set; }
        public DateTime PaymentOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
