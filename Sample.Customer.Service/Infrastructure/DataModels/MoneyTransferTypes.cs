using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class MoneyTransferTypes
    {
        public MoneyTransferTypes()
        {
            UserBankAccounts = new HashSet<UserBankAccounts>();
        }

        public long MoneyTransferTypeId { get; set; }
        public long Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long AccountId { get; set; }

        public virtual ICollection<UserBankAccounts> UserBankAccounts { get; set; }
    }
}
