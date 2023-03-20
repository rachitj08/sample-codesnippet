using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Admin.Model
{
    public class AuthResultModel
    {
        public string Token { get; set; }
        public RefreshTokenModel RefreshToken { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
