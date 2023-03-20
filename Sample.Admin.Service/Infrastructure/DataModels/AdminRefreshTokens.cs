using System;
using System.Collections.Generic;

namespace Sample.Admin.Service.Infrastructure.DataModels
{
    public partial class AdminRefreshTokens
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevorked { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid JwtId { get; set; }
        public long AdminRefreshTokenId { get; set; }
        public string JwtToken { get; set; }
    }
}
