using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class Source
    {
        public long SourceId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
    }
}
