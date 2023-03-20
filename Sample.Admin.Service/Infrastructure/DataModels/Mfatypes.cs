using System;
using System.Collections.Generic;

namespace Sample.Admin.Service.Infrastructure.DataModels
{
    public partial class Mfatypes
    {
        public long MfafactorsId { get; set; }
        public long UserId { get; set; }
        public bool PushNotification { get; set; }
        public bool Smsnotification { get; set; }
        public bool OneTimePassword { get; set; }
        public bool EmailNotification { get; set; }
    }
}
