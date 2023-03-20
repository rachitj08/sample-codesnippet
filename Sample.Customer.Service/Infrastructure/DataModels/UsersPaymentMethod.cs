using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class UsersPaymentMethod
    {
        public long UsersPaymentMethodId { get; set; }
        public long UserId { get; set; }
        public string PaymentMethodId { get; set; }
        public string StripeCustomerId { get; set; }
        public long AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public long UpdatedBy { get; set; }

        public virtual Users User { get; set; }
    }
}
