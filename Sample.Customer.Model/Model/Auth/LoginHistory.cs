using System;

namespace Sample.Customer.Model
{
    public class LoginHistory
    {
        public long UserId { get; set; }
        public string Ipaddress { get; set; }
        public string Token { get; set; }
        public string RequestHeader { get; set; }
        public DateTime LastRequestMade { get; set; }
        public DateTime LogoutTime { get; set; }
        public long LoginHistoryId { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool LoginStatus { get; set; }

    }
}
