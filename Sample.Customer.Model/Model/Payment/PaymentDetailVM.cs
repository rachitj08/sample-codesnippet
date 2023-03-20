using System;

namespace Sample.Customer.Model
{
    public class PaymentDetailVM
    {
        public long PaymentDetailId { get; set; }
        public long AccountId { get; set; }
        public long ReservationId { get; set; }
        public long IntentAmount { get; set; }
        public long? FinalAmount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string PaymentIntentId { get; set; }
        public string ReceiptEmail { get; set; }
        public string CustomerId { get; set; }
        public string PaymentMethodId { get; set; }
        public long? ApplicationFeeAmount { get; set; }
    }
}
