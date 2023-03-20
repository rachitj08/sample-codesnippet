using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class OtpLog
    {
        public long OtpLogId { get; set; }
        public string Email { get; set; }
        public string PassCode { get; set; }
        public bool IsUsed { get; set; }
        public DateTime CodeExpiry { get; set; }
        public string CountryCode { get; set; }
        public string ContactNumber { get; set; }
        public string DeviceId { get; set; }
        public string ImeiNumber { get; set; }
        public string ApiName { get; set; }
        public bool? IsOtpSend { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public bool? IsOtpVerified { get; set; }
        public long AccountId { get; set; }
        public string Otptype { get; set; }
    }
}
