using System;

namespace Sample.Admin.Model
{
    public class RefreshTokenModel
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public Guid JwtId { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevorked { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string JwtToken { get; set; }
    }
}
