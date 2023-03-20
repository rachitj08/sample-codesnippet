using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model
{
    public class CancelPaymentIntentRequest
    {
        public long ReservationId { get; set; }
        public long AccountId { get; set; }
        public long UserId { get; set; }
        public long PaymentDetailId { get; set; }
        public string PaymentIntentId { get; set; }
    }
}
