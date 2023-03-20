using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class UserDeviceHistory
    {
        public long UserDeviceHistoryId { get; set; }
        public long UserId { get; set; }
        public string DeviceId { get; set; }
        public short DeviceType { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceManufacture { get; set; }
        public string DeviceOsversion { get; set; }
        public DateTime LoggedIn { get; set; }
        public DateTime? LoggedOn { get; set; }
        public string DeviceToken { get; set; }
        public string AppVersion { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }

        public virtual Users User { get; set; }
    }
}
