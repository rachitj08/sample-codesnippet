using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class UserCreditCardsWillbedelete
    {
        public long UserCreditCardId { get; set; }
        public long MoneyTransferTypeId { get; set; }
        public long UserId { get; set; }
        public long AddressId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CardNumber { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public string Ccvcode { get; set; }
        public string BillingAddress1 { get; set; }
        public bool IsDefault { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
    }
}
