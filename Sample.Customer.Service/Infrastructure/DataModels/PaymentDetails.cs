using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class PaymentDetails
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
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? MarginAmount { get; set; }
        public string ConnectedAccountId { get; set; }

        public virtual Reservation Reservation { get; set; }
    }
}
