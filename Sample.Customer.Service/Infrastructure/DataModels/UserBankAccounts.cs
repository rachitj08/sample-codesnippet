using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class UserBankAccounts
    {
        public long UserBankAccountId { get; set; }
        public long MoneyTransferTypeId { get; set; }
        public long UserId { get; set; }
        public string RoutingNumber { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankName { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long AccountId { get; set; }

        public virtual MoneyTransferTypes MoneyTransferType { get; set; }
        public virtual Users User { get; set; }
    }
}
