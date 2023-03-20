using System;
using System.Collections.Generic;

namespace Sample.Admin.Service.Infrastructure.DataModels
{
    public partial class LoginHistories
    {
        public long LoginHistoryId { get; set; }
        public int UserId { get; set; }
        public string Ipaddress { get; set; }
        public string Token { get; set; }
        public string RequestHeader { get; set; }
        public DateTime? LastRequestMade { get; set; }
        public DateTime? LogoutTime { get; set; }
        public bool IsActive { get; set; }
        public bool LoginStatus { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
