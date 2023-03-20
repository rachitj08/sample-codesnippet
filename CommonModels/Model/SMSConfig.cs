using System;

namespace Common.Model
{
    public class SMSConfig
    {
        public SMSProvider SMSProvider { get; set; }
        public OTPConfig OTPConfig { get; set; }
    }

    public class SMSProvider
    {
        public string ProviderName { get; set; }
        public string ApiKey { get; set; }
        public string AccountId { get; set; }
        public string BaseUrl { get; set; }
        public string FromMobileNumber { get; set; }
    }

    public class OTPConfig
    {
        public string DefaultOTP { get; set; }
        public Int16 OTPExpiredInMin { get; set; }
        public string OTPMessage { get; set; }
        public Int16 OTPLength { get; set; }
    }
}
