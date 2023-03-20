using System.Collections.Generic;

namespace Sample.Customer.Model
{
    public class AuthResultModel
    {
        public string Token { get; set; }
        public RefreshTokenModel RefreshToken { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
