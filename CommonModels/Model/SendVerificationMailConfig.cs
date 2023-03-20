using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Model
{
    public class SendVerificationMailConfig
    {
        public string URL { get; set; }
        public int ExpiryTimeInHours { get; set; }
        public string EncryptKey { get; set; }
    }
}
