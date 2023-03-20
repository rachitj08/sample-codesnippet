using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class PasswordHistories
    {
        public long AccountId { get; set; }
        public long PwdHstId { get; set; }
        public long UserId { get; set; }
        public DateTime LastUsedOn { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
