using System;
using System.Collections.Generic;

namespace Sample.Admin.Service.Infrastructure.DataModels
{
    public partial class Services
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string EndPointBaseAddress { get; set; }
    }
}
