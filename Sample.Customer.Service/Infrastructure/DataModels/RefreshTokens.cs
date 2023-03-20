using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class RefreshTokens
    {
        public long UserId { get; set; }
        public string Token { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevorked { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public long AccountId { get; set; }
        public Guid JwtId { get; set; }
        public long RefreshTokenId { get; set; }
        public string JwtToken { get; set; }
    }
}
