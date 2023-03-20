using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class RequestLogs
    {
        public long RequestLogId { get; set; }
        public long UserId { get; set; }
        public string DeviceId { get; set; }
        public short DeviceType { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceManufacture { get; set; }
        public string DeviceOs { get; set; }
        public string DeviceOsversion { get; set; }
        public string RequestedFrom { get; set; }
        public string UserAgent { get; set; }
        public string Apiname { get; set; }
        public string AppVersion { get; set; }
        public string EncryptedToken { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Ipaddress { get; set; }
        public long? AccountId { get; set; }
    }
}
