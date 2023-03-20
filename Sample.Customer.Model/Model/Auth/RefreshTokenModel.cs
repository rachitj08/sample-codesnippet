using System;

namespace Sample.Customer.Model
{
    public class RefreshTokenModel
    {
        public long AccountId { get; set; }
        public long UserId { get; set; }
        public string Token { get; set; }
        public string JwtToken { get; set; }
        public Guid JwtId { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevorked { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
