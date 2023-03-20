using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model
{
    public class UpdatePaymentIntentRequest : CreatePaymentIntentRequest
    {
        public string EmailAddress { get; set; }
        public long PaymentDetailId { get; set; }
        public string PaymentIntentId { get; set; }
    }
}
